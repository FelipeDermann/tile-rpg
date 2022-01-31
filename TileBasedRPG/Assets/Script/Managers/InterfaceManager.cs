using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    public static event Action BattleStarted;
    public static event Action PreparationPhaseStarted;
    public static event Action ExecutionPhaseStarted;
    public static event Action BattleVictory;
    public static event Action BattleDefeat;

    Animator anim;

    public TextMeshProUGUI skillNameText;
    public GameObject turnOrderHUD;
    public Transform turnOrderIconsParent;

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

    public void ArrangeTurnOrderIcons(List<Unit> unitList)
    {
        turnOrderIconsParent.DetachChildren();

        List<Unit> unitListTemp = new List<Unit>();
        foreach (Unit unit in unitList) unitListTemp.Add(unit);
        unitListTemp.Reverse();

        foreach (Unit unit in unitListTemp)
        {
            unit.turnOrderIcon.transform.parent = turnOrderIconsParent;
        }
    }

    public void ToggleTurnOrderHUD(bool active)
    {
        turnOrderHUD.SetActive(active);
    }

    public void PlayNewBattlePhaseAnim(BattlePhase phase)
    {
        if (phase == BattlePhase.ExecutionPhase) ExecutionPhaseAnim();
        else PreparationPhaseAnim();
    }

    void PreparationPhaseAnim()
    {
        anim.SetTrigger("PreparationPhase");
    }

    void ExecutionPhaseAnim()
    {
        anim.SetTrigger("ExecutionPhase");
    }

    public void PlayBattleVictoryAnim()
    {
        anim.SetTrigger("Victory");
    }

    public void PlayBattleDefeatAnim()
    {
        anim.SetTrigger("Defeat");
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

    public void PlaySkillNameAnim(ScriptableSkillStats skillStats)
    {
        skillNameText.text = "<sprite name=" + skillStats.skillType.ToString() + ">" + " " + skillStats.skillName;
        anim.SetTrigger("SkillNameStart");
    }

    public void PlaySkillNameEndAnim()
    {
        anim.SetTrigger("SkillNameEnd");
    }

    public void BattleVictoryAnimEnd()
    {
        BattleVictory?.Invoke();
    }

    public void BattleDefeatAnimEnd()
    {
        BattleDefeat?.Invoke();
    }
}
