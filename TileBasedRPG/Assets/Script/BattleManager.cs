using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<Unit> allyUnitsToSpawn;
    public List<Unit> enemyUnitsToSpawn;
    public TileManager tileManager;

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
            spawnedUnit.InitiateUnit(tileManager.battleTiles[count, 0]);
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
            spawnedUnit.InitiateUnit(tileManager.battleTiles[count, tileManager.columns-1]);
            spawnedUnit.transform.SetParent(parentObj.transform);
            spawnedUnit.ChangeSide(FacingSide.FacingLeft);

            count++;
        }
    }
}
