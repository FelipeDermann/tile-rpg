using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVFXEvents : MonoBehaviour
{
    public Vector3 fxSpawnOffset;
    public bool mustMirrorXOffset;

    public event Action<BaseVFXEvents, BattleTile> ApplyEffectEvent;
    Animator anim;
    SpriteRenderer spr;
    public BattleTile targetHex;

    public virtual void PlayAnimation(BattleTile _targetHex, Unit _unitUsingSkill)
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (spr == null) spr = GetComponent<SpriteRenderer>();
        spr.flipX = _unitUsingSkill.characterSprite.flipX;

        targetHex = _targetHex;
        TransformEffectObject();

        spr.sortingOrder = _unitUsingSkill.characterSprite.sortingOrder +2;
        anim.SetTrigger("Play");
    }

    void TransformEffectObject()
    {
        Vector3 finalOffset = fxSpawnOffset;

        if (mustMirrorXOffset)
        {
            int side;
            if (spr.flipX) side = 1;
            else side = -1;

            finalOffset.x = finalOffset.x * side;
        }

        transform.position = targetHex.transform.position + finalOffset;
    }

    public void ApplyEffectAnimationEvent()
    {
        ApplyEffectEvent?.Invoke(this, targetHex);
    }
}
