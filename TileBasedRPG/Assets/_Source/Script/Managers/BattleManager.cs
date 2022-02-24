using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static event Action<bool> ToggleUnitUI;
    public static event Action TurnOffManagerUI;
    public static event Action PlanningPhaseStarted;
    
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

    [SerializeField]
    private List<Unit> deadHeroUnits;
    [SerializeField]
    private List<Unit> deadEnemyUnits;

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
        
        TileManager.TilesSetUpComplete += SpawnUnits;
        InterfaceManager.ExecutionPhaseStarted += ExecutionPhase;
        InterfaceManager.PreparationPhaseStarted += PreparationPhase;
        InterfaceManager.BattleVictory += RestartScene;
        InterfaceManager.BattleDefeat += RestartScene;
        Unit.SkillExecutionEnded += AllowNextUnitToAct;
        ControlsManager.ShowDetailsButtonPressedState += ShowAllSkillVisuals;
    }

    void OnDestroy()
    {
        TileManager.TilesSetUpComplete -= SpawnUnits;
        InterfaceManager.ExecutionPhaseStarted -= ExecutionPhase;
        InterfaceManager.PreparationPhaseStarted -= PreparationPhase;
        InterfaceManager.BattleVictory -= RestartScene;
        InterfaceManager.BattleDefeat -= RestartScene;
        Unit.SkillExecutionEnded -= AllowNextUnitToAct;
        ControlsManager.ShowDetailsButtonPressedState -= ShowAllSkillVisuals;
    }

    public void ChangeBattlePhase(BattlePhase phase)
    {
        battlePhase = phase;
        ToggleUnitUI?.Invoke(false);
        TurnOffManagerUI?.Invoke();
        ControlsManager.Instance.DisablePlayerControls();
        InterfaceManager.Instance.ToggleTurnOrderHUD(false);
        InterfaceManager.Instance.PlayNewBattlePhaseAnim(battlePhase);
    }

    void PreparationPhase()
    {
        EnableSkillsPlanningMethods();
        DefineUnitTurnOrder();
        InterfaceManager.Instance.ToggleTurnOrderHUD(true);

        bool ShowDetails = ControlsManager.Instance.ShowingDetails;
        ShowAllSkillVisuals(ShowDetails);
        ToggleUnitUI?.Invoke(ShowDetails);
        ControlsManager.Instance.ShowSelectedUnitDetails();
        
        PlanningPhaseStarted?.Invoke();
        
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

        CheckGameState();
    }

    void CheckGameState()
    {
        if (heroUnits.Count <= 0)
        {
            BattleDefeat();
            return;
        }

        if (enemyUnits.Count <= 0)
        {
            BattleVictory();
            return;
        }

        ChangeBattlePhase(BattlePhase.PreparationPhase);
    }

    void BattleVictory()
    {
        InterfaceManager.Instance.PlayBattleVictoryAnim();
        Debug.Log("YOU WIN!!");
    }

    void BattleDefeat()
    {
        InterfaceManager.Instance.PlayBattleDefeatAnim();
        Debug.Log("YOU Lose...");
    }

    void RestartScene()
    {
        StartCoroutine(RestartSceneCoroutine());
    }

    IEnumerator RestartSceneCoroutine()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    void EnableSkillsPlanningMethods()
    {
        foreach (Unit enemy in enemyUnits)
        {
            enemy.unitSkills.CurrentSkillPlanningPhaseMethod();
        }

        foreach (Unit hero in heroUnits)
        {
            hero.unitSkills.CurrentSkillPlanningPhaseMethod();
        }
    }

    void ShowAllSkillVisuals(bool show)
    {
        foreach (Unit enemy in enemyUnits)
        {
            if (show) enemy.unitSkills.ShowSkillVisuals();
            else enemy.unitSkills.EndCurrentSkillVisuals();
        }

        foreach (Unit hero in heroUnits)
        {
            if (show) hero.unitSkills.ShowSkillVisuals();
            else hero.unitSkills.EndCurrentSkillVisuals();
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

    public void UnitDeath(Unit unitThatDied)
    {
        if (unitsToAct.Contains(unitThatDied))
            unitsToAct.Remove(unitThatDied);

        if (heroUnits.Contains(unitThatDied))
        {
            heroUnits.Remove(unitThatDied);
            deadHeroUnits.Add(unitThatDied);
        }

        if (enemyUnits.Contains(unitThatDied))
        {
            enemyUnits.Remove(unitThatDied);
            deadEnemyUnits.Add(unitThatDied);
        }

        Debug.Log("Battle manager knows that unit is dead: " + unitThatDied.unitStats.unitName);
    }
}
