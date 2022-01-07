using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
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
    public List<Unit> allyUnitsToSpawn;
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
    }

    void OnDisable()
    {
        TileManager.TilesSetUpComplete -= SpawnUnits;
        InterfaceManager.ExecutionPhaseStarted -= ExecutionPhase;
        InterfaceManager.PreparationPhaseStarted -= PreparationPhase;
    }

    public void ChangeBattlePhase(BattlePhase phase)
    {
        battlePhase = phase;
        ControlsManager.Instance.DisablePlayerControls();
        InterfaceManager.Instance.PlayNewBattlePhaseAnim(battlePhase);
    }

    void PreparationPhase()
    {
        DefineUnitTurnOrder();

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
        //logic to execute all unit's moves in order
        Debug.Log("EXECUTION PHASE END");
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

    void DefineUnitTurnOrder()
    {
        foreach (Unit unit in heroUnits)
        {
            unitsToAct.Add(unit);
        }

        foreach (Unit unit in enemyUnits)
        {
            unitsToAct.Add(unit);
        }

        unitsToAct.Sort((p1, p2) => p2.unitStats.stats.speed.CompareTo(p1.unitStats.stats.speed));
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
        foreach (Unit unit in allyUnitsToSpawn)
        {
            var spawnedUnit = Instantiate(unit);
            spawnedUnit.InitiateUnit(TileManager.Instance.battleTiles[count, 0]);
            spawnedUnit.transform.SetParent(parentObj.transform);

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
            spawnedUnit.InitiateUnit(TileManager.Instance.battleTiles[count, TileManager.Instance.columns-1]);
            spawnedUnit.transform.SetParent(parentObj.transform);
            spawnedUnit.ChangeSide(FacingSide.FacingLeft);
            spawnedUnit.ChangeType(UnitType.EnemyUnit);

            enemyUnits.Add(spawnedUnit);

            count++;
        }
    }
}
