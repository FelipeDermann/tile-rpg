using System;
using DG.Tweening;
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
    public bool isDead = false;
    private BattleTile currentTile;

    public BattleTile CurrentTile { get { return currentTile; } }

    [Header("Object References")]
    public GameObject spriteParentObj;
    public SpriteRenderer characterSprite;
    public SpriteRenderer shadowSprite;
    public TurnOrderIcon turnOrderIcon;

    [Header("Script References")]
    public UnitSkills unitSkills;
    public UnitUI unitUI;
    public UnitStats unitStats;
    public UnitAnimations unitAnims;

    [Header("Test")]
    public float healthTest = -20;

    public void InitiateUnit(BattleTile _tileToSpawnAt)
    {
        AssignEvents();
        AssignFirstTile(_tileToSpawnAt);

        unitStats.InitiateStats();
        unitSkills.SkillSetup(this);
        turnOrderIcon.ChangeBorderColor(unitType);
    }

    void AssignEvents()
    {
        unitSkills.SkillChanged += unitUI.UpdateSkillText;
        unitStats.HealthChanged += unitUI.UpdateHealthBar;
        unitStats.DamageTaken += unitAnims.DOTTakeDamage;
        unitStats.UnitDied += UnitDeath;
        unitUI.PlayDeathAnim += unitAnims.DeathAnimation;
    }

    void OnDestroy()
    {
        unitSkills.SkillChanged -= unitUI.UpdateSkillText;
        unitStats.HealthChanged -= unitUI.UpdateHealthBar;
        unitStats.DamageTaken -= unitAnims.DOTTakeDamage;
        unitStats.UnitDied -= UnitDeath;
        unitUI.PlayDeathAnim -= unitAnims.DeathAnimation;
    }

    void AssignFirstTile(BattleTile _tileToSpawnAt)
    {
        currentTile = _tileToSpawnAt;
        _tileToSpawnAt.ChangeCurrentUnit(this);
    }

    void ChangePhysicalPosition(BattleTile _newTile)
    {
        transform.position = _newTile.unitTransformPosition.position;

        ChangeSpriteSortingOrder(_newTile);
    }

    public void ChangeSpriteSortingOrder(BattleTile _newTile)
    {
        int newSortingOrder = _newTile.orderInLayer;
        characterSprite.sortingOrder = newSortingOrder + 3;
        shadowSprite.sortingOrder = characterSprite.sortingOrder - 1;

        unitUI.ChangeSortingOrder(characterSprite.sortingOrder + 2);
    }

    public void ChangeSpriteSortingOrder(int _newSortingOrder)
    {
        int newSortingOrder = _newSortingOrder;
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
        unitSkills.ShowSkillVisuals();
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

    void UnitDeath()
    {
        isDead = true;

        currentTile.ChangeCurrentUnit(null);
            
        BattleManager.Instance.UnitDeath(this);
    }

    void RestoreUnitFromDeath()
    {

    }
}
