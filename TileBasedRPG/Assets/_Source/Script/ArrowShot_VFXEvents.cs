using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowShot_VFXEvents : BaseVFXEvents
{
    public SpriteRenderer sprRenderer;

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    public override void PlayAnimation(BattleTile _targetHex, Unit _unitUsingSkill)
    {
        sprRenderer.enabled = true;
        sprRenderer.flipX = _unitUsingSkill.characterSprite.flipX;
        sprRenderer.sortingOrder = _unitUsingSkill.characterSprite.sortingOrder +2;

        targetHex = _targetHex;
        Vector3 originPos = transform.position;
        
        transform.DOMove(_targetHex.unitTransformPosition.position, 1f).OnComplete(() =>
        {
            sprRenderer.enabled = false;
            transform.position = originPos;
            
            ApplyEffectAnimationEvent();
        });
    }
}
