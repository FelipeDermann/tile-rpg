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
    public HexPos hexPos;

    [Header("References")]
    public Transform hexCenterSpriteParent;
    public Transform effectSpritesParent;
    public Transform unitTransformPosition;

    [Header("Borders")]
    public Transform borderSpriteParent;
    public GameObject allyBorder;
    public GameObject enemyBorder;
    public GameObject neutralBorder;
    public GameObject extraBorder;

    SpriteRenderer hexSprite;
    Animator anim;

    [SerializeField]
    private bool playingDangerAnim;

    public bool PlayingDangerAnim { get { return playingDangerAnim; } set { playingDangerAnim = value; } }

    private void Awake()
    {
        anim = GetComponent<Animator>();
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

    public void PlayDangerAnim()
    {
        anim.Play("Danger", 0, UpdateManager.Instance.globalHexAnimationTime);
        playingDangerAnim = true;
    }

    public void PlayHealAnim()
    {
        if (playingDangerAnim) return;

        anim.Play("Heal", 0, UpdateManager.Instance.globalHexAnimationTime);
    }

    public void PlayBuffAnim()
    {
        if (playingDangerAnim) return;

        anim.Play("Buff", 0, UpdateManager.Instance.globalHexAnimationTime);
    }

    public void PlayDebuffAnim()
    {
        if (playingDangerAnim) return;

        anim.Play("Debuff", 0, UpdateManager.Instance.globalHexAnimationTime);
    }

    public void StopAnimations()
    {
        playingDangerAnim = false;
        anim.Play("Idle");
    }

    void OnValidate()
    {
        foreach (Transform obj in borderSpriteParent)
        {
            obj.gameObject.SetActive(false);
        }

        switch (tileType)
        {
            case TileType.AllyTile:
                allyBorder.SetActive(true);
                break;
            case TileType.EnemyTile:
                enemyBorder.SetActive(true);
                break;
            case TileType.NeutralTile:
                neutralBorder.SetActive(true);
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

        SpriteRenderer[] centerSprites = hexCenterSpriteParent.GetComponentsInChildren<SpriteRenderer>(true);
        SpriteRenderer[] borderSprites = borderSpriteParent.GetComponentsInChildren<SpriteRenderer>(true);
        SpriteRenderer[] effectSprites = effectSpritesParent.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer sprite in centerSprites)
        {
            sprite.sortingOrder = _layerOrderToSet;
        }

        foreach (SpriteRenderer sprite in borderSprites)
        {
            sprite.sortingOrder = _layerOrderToSet+1;
        }

        foreach (SpriteRenderer sprite in effectSprites)
        {
            sprite.sortingOrder = _layerOrderToSet+2;
        }
    }

    void ChangeHexSprite(bool isDisplaying)
    {
        SpriteRenderer[] centerSprites = hexCenterSpriteParent.GetComponentsInChildren<SpriteRenderer>(true);
        SpriteRenderer[] borderSprites = borderSpriteParent.GetComponentsInChildren<SpriteRenderer>(true);
        SpriteRenderer[] effectSprites = effectSpritesParent.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer sprite in centerSprites)
        {
            sprite.enabled = isDisplaying;
        }

        foreach (SpriteRenderer sprite in borderSprites)
        {
            sprite.enabled = isDisplaying;
        }

        foreach (SpriteRenderer sprite in effectSprites)
        {
            sprite.enabled = isDisplaying;
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
