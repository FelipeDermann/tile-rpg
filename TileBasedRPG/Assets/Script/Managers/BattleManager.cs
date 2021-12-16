using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField]
    private BattlePhase battlePhase;

    public BattlePhase BattlePhase{ get { return battlePhase;}}

    [Header("Units to Spawn")]
    public List<Unit> allyUnitsToSpawn;
    public List<Unit> enemyUnitsToSpawn;

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
    }

    void OnDisable()
    {
        TileManager.TilesSetUpComplete -= SpawnUnits;
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

            count++;
        }
    }
}
