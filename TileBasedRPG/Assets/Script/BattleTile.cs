using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[SelectionBase]
public class BattleTile : MonoBehaviour
{
    [Header("Hex Info")]
    public Unit unitStandingOnHex;

    [Header("Hex Config")]
    public TileType tileType;
    public int orderInLayer;

    [Header("Debug info")]
    public int row, column;

    [Header("References")]
    public ScriptTileDefinitions tileDefinitions;
    public Transform hexSpriteParent;
    public Transform unitTransformPosition;

    public void SetArrayPos(int _row, int _column)
    {
        row = _row;
        column = _column;
    }

    public void ChangeCurrentUnit(Unit _newUnit)
    {
        unitStandingOnHex = _newUnit;
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