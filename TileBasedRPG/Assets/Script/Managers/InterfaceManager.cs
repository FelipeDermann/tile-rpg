using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    public static event Action BattleStarted;
    public static event Action PreparationPhaseStarted;
    public static event Action ExecutionPhaseStarted;

    Animator anim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void BattleStartedAnimEnd()
    {
        BattleStarted?.Invoke();
    }

    public void PreparationPhaseAnimEnd()
    {
        PreparationPhaseStarted?.Invoke();
    }

    public void ExecutionPhaseAnimEnd()
    {
        ExecutionPhaseStarted?.Invoke();
    }
}
