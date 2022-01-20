using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Vector3 fxSpawnOffset;

    public event Action<AnimationEvents, BattleTile> ApplyEffectEvent;
    Animator anim;
    SpriteRenderer spr;
    BattleTile targetHex;

    public void PlayAnimation(BattleTile _targetHex, Unit _unitUsingSkill)
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (spr == null) spr = GetComponent<SpriteRenderer>();

        targetHex = _targetHex;
        transform.position = targetHex.transform.position + fxSpawnOffset;

        spr.sortingOrder = _unitUsingSkill.characterSprite.sortingOrder +2;
        spr.flipX = _unitUsingSkill.characterSprite.flipX;
        anim.SetTrigger("Play");
    }

    public void ApplyEffectAnimationEvent()
    {
        ApplyEffectEvent?.Invoke(this, targetHex);
    }
}
