using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Unit : MonoBehaviour
{
    public static event Action SkillExecutionEnded;

    [Header("Unit Info")]
    public UnitType unitType;
    public FacingSide facingSide;
    private BattleTile currentTile;

    public BattleTile CurrentTile { get { return currentTile; } }

    [Header("Object References")]
    public SpriteRenderer characterSprite;
    public SpriteRenderer shadowSprite;

    [Header("Script References")]
    public UnitSkills unitSkills;
    public UnitUI unitUI;
    public UnitStats unitStats;
    
    [Header("Test")]
    public float healthTest = -20;

    public void InitiateUnit(BattleTile _tileToSpawnAt)
    {
        AssignEvents();
        AssignFirstTile(_tileToSpawnAt);

        unitStats.InitiateStats();
        unitSkills.SkillSetup(this);

        facingSide = FacingSide.FacingRight;
    }

    void AssignEvents()
    {
        unitSkills.SkillChanged += unitUI.UpdateSkillText;
        unitStats.HealthChanged += unitUI.UpdateHealthBar;
    }

    public void HealthTest()
    {
        unitStats.ChangeHealth(healthTest);
    }

    void AssignFirstTile(BattleTile _tileToSpawnAt)
    {
        currentTile = _tileToSpawnAt;
        _tileToSpawnAt.ChangeCurrentUnit(this);
    }

    void ChangePhysicalPosition(BattleTile _newTile)
    {
        transform.position = _newTile.unitTransformPosition.position;

        int newSortingOrder = _newTile.orderInLayer;
        characterSprite.sortingOrder = newSortingOrder + 3;
        shadowSprite.sortingOrder = characterSprite.sortingOrder - 1;

        unitUI.ChangeSortingOrder(characterSprite.sortingOrder + 2);
    }

    public void ChangeSide(FacingSide _newSide)
    {
        facingSide = _newSide;

        if (facingSide == FacingSide.FacingRight)
            characterSprite.flipX = false;
        else
            characterSprite.flipX = true;
    }

    public void ChangeSide()
    {
        if (facingSide == FacingSide.FacingRight) facingSide = FacingSide.FacingLeft;
        else facingSide = FacingSide.FacingRight;

        characterSprite.flipX = !characterSprite.flipX;
    }

    public void ChangeType(UnitType _newType)
    {
        unitType = _newType;
    }

    public void ChangeCurrentTile(BattleTile _newTile)
    {
        currentTile = _newTile;
        ChangePhysicalPosition(currentTile);
    }

    public int GetSideDirection()
    {
        int facingDirection = 1;
        if (facingSide == FacingSide.FacingLeft) facingDirection = -1;

        return facingDirection;
    }

    public void SkillExecutionEndedEvent()
    {
        SkillExecutionEnded?.Invoke();
    }
}
