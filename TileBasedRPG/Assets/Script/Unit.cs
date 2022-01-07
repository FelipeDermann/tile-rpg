using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Unit : MonoBehaviour
{
    [Header("Unit Info")]
    public UnitType unitType;
    public FacingSide facingSide;
    public BattleTile currentTile;

    [Header("Object References")]
    public SpriteRenderer characterSprite;
    public SpriteRenderer shadowSprite;

    [Header("Script References")]
    public UnitSkills unitSkills;
    public UnitUI unitUI;
    public UnitStats unitStats;

    public void InitiateUnit(BattleTile _tileToSpawnAt)
    {
        AssignEvents();
        AssignFirstTile(_tileToSpawnAt);

        unitStats.InitiateStats();
        unitSkills.SetFirstSkill();

        facingSide = FacingSide.FacingRight;
    }

    void AssignEvents()
    {
        unitSkills.SkillChanged += unitUI.UpdateSkillText;
    }

    void AssignFirstTile(BattleTile _tileToSpawnAt)
    {
        currentTile = _tileToSpawnAt;
        _tileToSpawnAt.ChangeCurrentUnit(this);
    }

    public void ChangePhysicalPosition(BattleTile _newTile)
    {
        transform.position = _newTile.unitTransformPosition.position;

        int newSortingOrder = _newTile.orderInLayer;
        characterSprite.sortingOrder = newSortingOrder + 1;
        shadowSprite.sortingOrder = characterSprite.sortingOrder - 1;

        unitUI.ChangeSortingOrder(newSortingOrder+2);
    }

    public void ChangeSide(FacingSide _newSide)
    {
        facingSide = _newSide;

        if (facingSide == FacingSide.FacingRight)
            characterSprite.flipX = false;
        else
            characterSprite.flipX = true;
    }

    public void ChangeType(UnitType _newType)
    {
        unitType = _newType;
    }
}
