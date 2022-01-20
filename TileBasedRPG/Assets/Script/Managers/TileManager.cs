using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public static event Action TilesSetUpComplete;

    public BattleTile[,] battleTiles;

    [Header("Battlefield Hexes creator")]
    public Transform hexPrefab;
    public Vector3 hexSpacing;
    public int rows;
    public int columns;

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

    void Start()
    {
        SetupTiles();
    }

    public BattleTile GetTileInMatrix(HexPos _hexToCheck)
    {
        BattleTile targetTile = null;

        if (_hexToCheck.row > rows - 1 || _hexToCheck.row < 0 ||
            _hexToCheck.column > columns - 1 || _hexToCheck.column < 0)
            return null;

        targetTile = battleTiles[_hexToCheck.row, _hexToCheck.column];

        if (targetTile.tileType == TileType.NullTile)
            return null;

        return targetTile;
    }

    public BattleTile MoveThroughMatrixTiles(HexPos _hexToCheck)
    {
        BattleTile targetTile = null;

        if (_hexToCheck.row > rows-1) _hexToCheck.row = 0;
        if (_hexToCheck.row < 0) _hexToCheck.row = rows-1;

        if (_hexToCheck.column > columns-1) _hexToCheck.column = 0;
        if (_hexToCheck.column < 0) _hexToCheck.column = columns-1;

        targetTile = battleTiles[_hexToCheck.row, _hexToCheck.column];

        if (battleTiles[_hexToCheck.row, _hexToCheck.column].tileType == TileType.NullTile) 
            targetTile = null;

        return targetTile;
    }

    void SetupTiles()
    {
        battleTiles = new BattleTile[rows, columns];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                battleTiles[r, c] = transform.GetChild(r).transform.GetChild(c).GetComponent<BattleTile>();
            }
        }
        TilesSetUpComplete?.Invoke();
    }

    public void BattlefieldGeneratorEditor()
    { 
        battleTiles = new BattleTile[rows,columns];
        Debug.Log("Grid size: [" + battleTiles.GetLength(0) + ", " + battleTiles.GetLength(1) + "]");

        for (int i = transform.childCount; i > 0; --i)
            DestroyImmediate(transform.GetChild(0).gameObject);

        int hexSortingOrder = 0;
        int hexNumber = 0;

        for (int r = 0; r < rows; r++)
        {
            GameObject rowParent = new GameObject("Row " + r);
            rowParent.transform.parent = transform;

            hexSortingOrder = 5 * r;

            for (int c = 0; c < columns; c++)
            {
                Transform newHex = Instantiate(hexPrefab, transform.position, transform.rotation);
                newHex.transform.parent = rowParent.transform;

                newHex.gameObject.name += " " + (hexNumber+1);
                hexNumber++;

                Vector3 hexPosOffset = hexSpacing;
                hexPosOffset.x *= c;
                hexPosOffset.y *= -r;

                //odd numbered hex rows are placed a bit to the left
                if (r % 2 == 1) hexPosOffset.x -= hexSpacing.x / 2;
                newHex.position = transform.position + hexPosOffset;

                BattleTile newHexScript = newHex.GetComponent<BattleTile>();
                newHexScript.SetLayerOrder(hexSortingOrder);
                newHexScript.SetArrayPos(r,c);

                battleTiles[r,c] = newHexScript;
                Debug.Log(battleTiles[r, c].gameObject.name + " in [" + r + "," + c + "]");
            }
        }
    }

}
