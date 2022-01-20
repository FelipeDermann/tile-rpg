using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Vector3 fxSpawnOffset;

    public event Action ApplyEffectEvent;
    Animator anim;
    SpriteRenderer spr;

    public void PlayAnimation(Transform _positionToPlayAt, int _newSortingOrder)
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (spr == null) spr = GetComponent<SpriteRenderer>();

        anim.SetTrigger("Play");
        spr.sortingOrder = _newSortingOrder;

        transform.position = _positionToPlayAt.position + fxSpawnOffset;
    }

    public void ApplyEffectAnimationEvent()
    {
        ApplyEffectEvent?.Invoke();
    }
}
