using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimations : MonoBehaviour
{
    Unit playerUnit;

    Animator anim;

    [SerializeField] SpriteRenderer deathVFXSprite;

    void Awake()
    {
        playerUnit = GetComponent<Unit>();
        anim = GetComponent<Animator>();
    }

    public void DOTMeleeAttack(Unit unit)
    {
        HexPos hexInFront = new HexPos(unit.CurrentTile.hexPos.row,
        unit.CurrentTile.hexPos.column + (1 * unit.GetSideDirection()));
        BattleTile newHexInFront = TileManager.Instance.GetTileInMatrix(hexInFront);
        if (newHexInFront == null) return;

        Transform charTransform = unit.spriteParentObj.transform;

        Vector3 initialPos = charTransform.position;
        Vector3 destinationPos = newHexInFront.unitTransformPosition.position;

        var sequence = DOTween.Sequence()
            .Append(charTransform.DOMove(destinationPos, 0.3f))
            .Append(charTransform.DOMove(initialPos, 0.3f));
    }

    public void DOTSpellCast(Unit unit)
    {
        Transform charTransform = unit.spriteParentObj.transform;
        Vector3 offset = Vector3.right * (2 * unit.GetSideDirection());

        charTransform.DOPunchRotation(charTransform.position + offset, 0.8f, 10);
    }

    public void DOTTakeDamage(bool playCriticalHitAnim)
    {
        if (!playCriticalHitAnim)
        {
            playerUnit.spriteParentObj.transform.DOShakeRotation(0.8f, 40, 10, 90);
            playerUnit.spriteParentObj.transform.DOShakeScale(0.8f, 0.4f, 10, 90);
        }
        else
        {
            playerUnit.spriteParentObj.transform.DOShakePosition(0.8f, 0.15f, 10, 100);
            playerUnit.spriteParentObj.transform.DOShakeRotation(0.8f, 40, 10, 90);
            playerUnit.spriteParentObj.transform.DOShakeScale(0.8f, 0.4f, 10, 100);
        }
    }

    public void DeathAnimation()
    {
        Debug.Log("YOU ARE DEAD DEAD DEAD!");

        deathVFXSprite.sortingOrder = playerUnit.CurrentTile.orderInLayer+2;
        anim.SetTrigger("Death");
    }

    public void DeathAnimationEvent()
    {
        gameObject.SetActive(false);
    }
}
