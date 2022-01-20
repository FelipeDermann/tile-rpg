using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct HexPos
{
    public int row, column;

    public HexPos(int _row, int _column)
    {
        row = _row;
        column = _column;
    }
}

[SelectionBase]
public class BattleTile : MonoBehaviour
{
    [Header("Hex Info")]
    [SerializeField]
    private Unit unitStandingOnHex;

    public Unit UnitStandingOnHex
    {
        get { return unitStandingOnHex; }
    }

    [Header("Hex Config")]
    public TileType tileType;
    public int orderInLayer;
    public float highlightFadeDuration;

    [Header("Debug info")]
    public HexPos hexPos;

    [Header("References")]
    public ScriptableTileDefinitions tileDefinitions;
    public Transform hexSpriteParent;
    public Transform unitTransformPosition;

    SpriteRenderer hexSprite;
    public Color originalColor;

    void Start()
    {
        hexSprite = hexSpriteParent.GetChild(0).GetComponent<SpriteRenderer>();
        originalColor = hexSprite.color;
    }

    public void SetArrayPos(int _row, int _column)
    {
        hexPos.row = _row;
        hexPos.column = _column;
    }

    public void ChangeCurrentUnit(Unit _newUnit)
    {
        unitStandingOnHex = _newUnit;
        if (_newUnit == null) return;

        _newUnit.ChangeCurrentTile(this);
    }

    void OnValidate()
    {
        switch (tileType)
        {
            case TileType.AllyTile: 
                hexSpriteParent.GetChild(0).GetComponent<SpriteRenderer>().color = tileDefinitions.allyTileColor;
                break;
            case TileType.EnemyTile:
                hexSpriteParent.GetChild(0).GetComponent<SpriteRenderer>().color = tileDefinitions.enemyTileColor;
                break;
            case TileType.NeutralTile:
                hexSpriteParent.GetChild(0).GetComponent<SpriteRenderer>().color = tileDefinitions.neutralTileColor;
                break;
            case TileType.NullTile:
                ChangeHexSprite(false);
                break;
        }

        if (tileType != TileType.NullTile)
            ChangeHexSprite(true);           
    }

    public void SetLayerOrder(int _layerOrderToSet)
    {
        orderInLayer = _layerOrderToSet;

        for (int i = 0; i < hexSpriteParent.childCount; i++)
        {
            hexSpriteParent.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = _layerOrderToSet;
        }
    }

    void ChangeHexSprite(bool isDisplaying)
    {
        for (int i = 0; i < hexSpriteParent.childCount; i++)
        {
            hexSpriteParent.GetChild(i).GetComponent<SpriteRenderer>().enabled = isDisplaying;
        }

        AssignLabel(gameObject, !isDisplaying);
    }

    public static void AssignLabel(GameObject g, bool isIconDisplaying)
    {
        Texture2D tex = EditorGUIUtility.IconContent("GameObject Icon").image as Texture2D;
        Type editorGUIUtilityType = typeof(EditorGUIUtility);
        BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
        if (!isIconDisplaying) tex = null;
        object[] args = new object[] { g, tex };
        editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
    }
}
