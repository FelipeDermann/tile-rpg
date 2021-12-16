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
    public int row, column;

    [Header("References")]
    public ScriptTileDefinitions tileDefinitions;
    public Transform hexSpriteParent;
    public Transform unitTransformPosition;

    SpriteRenderer hexSprite;
    public Color originalColor;
    bool stopHighlight;

    void Start()
    {
        hexSprite = hexSpriteParent.GetChild(0).GetComponent<SpriteRenderer>();
        originalColor = hexSprite.color;
    }

    public void SetArrayPos(int _row, int _column)
    {
        row = _row;
        column = _column;
    }

    public void ChangeCurrentUnit(Unit _newUnit)
    {
        unitStandingOnHex = _newUnit;
        if (_newUnit == null) return;

        _newUnit.ChangePhysicalPosition(this);
    }

    public void StartHighlight()
    {
        StopCoroutine(HighlightEffect());
        StartCoroutine(HighlightEffect());
    }
    
    public void StopHighlight()
    {
        stopHighlight = true;
    }

    IEnumerator HighlightEffect()
    {
        Color targetColor = Color.white;
        Color colorToGive = Color.white;
        hexSprite.color = colorToGive;

        bool backToOriginalColor = false;
        float t = 0;

        while (!stopHighlight)
        {
            if (!backToOriginalColor) colorToGive = Color.Lerp(targetColor, originalColor, t);
            else colorToGive = Color.Lerp(originalColor, targetColor, t);

            t += Time.deltaTime / highlightFadeDuration;
            if (t >= highlightFadeDuration)
            {
                backToOriginalColor = !backToOriginalColor;
                t = 0;
            }

            hexSprite.color = colorToGive;
            yield return null;
        }

        stopHighlight = false;
        hexSprite.color = originalColor;
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
