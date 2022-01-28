using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static event Action<bool> TurnOffUnitUI;
    public static BattleManager Instance { get; private set; }

    [SerializeField]
    private BattlePhase battlePhase;
    [SerializeField]
    private bool battleStarted = false;
    public BattlePhase BattlePhase{ get { return battlePhase;}}
    public bool BattleStarted{ get { return battleStarted;} }

    [Header("Configurable")]
    public ScriptableGameDefinitions gameDefinitions;

    [Header("Units to Spawn")]
    public List<Unit> heroUnitsToSpawn;
    public List<Unit> enemyUnitsToSpawn;

    [Header("Unit action execution order")]
    [SerializeField]
    private List<Unit> unitsToAct;
    [SerializeField]
    private List<Unit> heroUnits;
    [SerializeField]
    private List<Unit> enemyUnits;

    delegate void PhaseToChangeTo();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        TileManager.TilesSetUpComplete += SpawnUnits;
        InterfaceManager.ExecutionPhaseStarted += ExecutionPhase;
        InterfaceManager.PreparationPhaseStarted += PreparationPhase;
        Unit.SkillExecutionEnded += AllowNextUnitToAct;
    }

    void OnDisable()
    {
        TileManager.TilesSetUpComplete -= SpawnUnits;
        InterfaceManager.ExecutionPhaseStarted -= ExecutionPhase;
        InterfaceManager.PreparationPhaseStarted -= PreparationPhase;
        Unit.SkillExecutionEnded -= AllowNextUnitToAct;
    }

    public void ChangeBattlePhase(BattlePhase phase)
    {
        battlePhase = phase;
        TurnOffUnitUI?.Invoke(false);
        ControlsManager.Instance.DisablePlayerControls();
        InterfaceManager.Instance.ToggleTurnOrderHUD(false);
        InterfaceManager.Instance.PlayNewBattlePhaseAnim(battlePhase);
    }

    void PreparationPhase()
    {
        ShowAndSetSkills();
        DefineUnitTurnOrder();
        InterfaceManager.Instance.ToggleTurnOrderHUD(true);

        if (!battleStarted)
        {
            battleStarted = true;
            return;
        }

        ControlsManager.Instance.EnablePlayerControls();

        Debug.Log("PREPARATION PHASE IS ON!");
    }

    void ExecutionPhase()
    {
        Debug.Log("EXECUTION PHASE STARTED!");
        AllowNextUnitToAct();
    }

    void AllowNextUnitToAct()
    {
        StartCoroutine(ExecuteUnitSkill());
    }

    IEnumerator ExecuteUnitSkill()
    {
        yield return new WaitForSeconds(gameDefinitions.delayToExecuteNextUnitSkill);

        if (unitsToAct.Count <= 0)
        {
            ExecutionPhaseEnd();
            yield break;
        }

        Unit currentUnitToAct = unitsToAct[0];
        unitsToAct.Remove(currentUnitToAct);

        currentUnitToAct.unitSkills.ExecuteSkill();
    }

    void ExecutionPhaseEnd()
    {
        Debug.Log("EXECUTION PHASE END");
        InterfaceManager.Instance.PlaySkillNameEndAnim();
        StartCoroutine(EndExecutionPhase(EnemyPhase));
    }

    void EnemyPhase()
    {
        //logic to move enemies around and let them select their moves
        Debug.Log("ENEMY PHASE END");
        ChangeBattlePhase(BattlePhase.PreparationPhase);
    }

    IEnumerator EndExecutionPhase(PhaseToChangeTo newPhase)
    {
        yield return new WaitForSeconds(gameDefinitions.delayToEndExecPhase);
        newPhase();
    }

    public void DefineUnitTurnOrder()
    {
        unitsToAct.Clear();

        foreach (Unit unit in heroUnits)
        {
            unitsToAct.Add(unit);
        }

        foreach (Unit unit in enemyUnits)
        {
            unitsToAct.Add(unit);
        }

        unitsToAct.Sort((p1, p2) => p2.unitStats.GetTurnOrderSpeed().CompareTo(
            p1.unitStats.GetTurnOrderSpeed()));

        InterfaceManager.Instance.ArrangeTurnOrderIcons(unitsToAct);
    }

    void ShowAndSetSkills()
    {
        foreach (Unit enemy in enemyUnits)
        {
            enemy.unitSkills.ShowSkillVisuals();
            enemy.unitSkills.CurrentSkillPlanningPhaseMethod();
        }

        foreach (Unit hero in heroUnits)
        {
            hero.unitSkills.ShowSkillVisuals();
            hero.unitSkills.CurrentSkillPlanningPhaseMethod();
        }
    }

    void SpawnUnits()
    {
        SpawnAllies();
        SpawnEnemies();
    }

    void SpawnAllies()
    {
        int count = 0;
        GameObject parentObj = new GameObject("Player Units");
        foreach (Unit unit in heroUnitsToSpawn)
        {
            var spawnedUnit = Instantiate(unit);
            spawnedUnit.InitiateUnit(TileManager.Instance.battleTiles[count, 0]);
            spawnedUnit.transform.SetParent(parentObj.transform);
            spawnedUnit.unitUI.ToggleUI(false);

            heroUnits.Add(spawnedUnit);

            count++;
        }
    }

    void SpawnEnemies()
    {
        int count = 0;
        GameObject parentObj = new GameObject("Enemy Units");
        foreach (Unit unit in enemyUnitsToSpawn)
        {
            var spawnedUnit = Instantiate(unit);
            spawnedUnit.ChangeSide(FacingSide.FacingLeft);
            spawnedUnit.ChangeType(UnitType.EnemyUnit);
            spawnedUnit.InitiateUnit(TileManager.Instance.battleTiles[count, 3]);
            spawnedUnit.transform.SetParent(parentObj.transform);
            spawnedUnit.unitUI.ToggleUI(false);

            enemyUnits.Add(spawnedUnit);

            count++;
        }
    }
}
