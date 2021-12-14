using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Info")]
    public UnitType unitType;
    public FacingSide facingSide;
    public int currentHealth;
    public int currentEnergy;
    public BattleTile currentTile;

    [Header("References")]
    public ScriptUnitStats stats;
    public SpriteRenderer characterSprite;
    public SpriteRenderer shadowSprite;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitiateUnit(BattleTile _tileToSpawnAt)
    {
        currentHealth = stats.health;
        currentEnergy = stats.energy;

        currentTile = _tileToSpawnAt;
        _tileToSpawnAt.unitStandingOnHex = this;

        transform.position = _tileToSpawnAt.unitTransformPosition.position;
        characterSprite.sortingOrder = _tileToSpawnAt.orderInLayer + 1;
        shadowSprite.sortingOrder = characterSprite.sortingOrder - 1;

        facingSide = FacingSide.FacingRight;
    }

    public void ChangeSide(FacingSide _newSide)
    {
        facingSide = _newSide;

        if (facingSide == FacingSide.FacingRight)
            characterSprite.flipX = false;
        else
            characterSprite.flipX = true;
    }
}
