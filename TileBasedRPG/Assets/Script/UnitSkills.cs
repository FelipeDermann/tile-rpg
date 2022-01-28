using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkills : MonoBehaviour
{
    public event Action<string, SkillType> SkillChanged;

    [Header("Unit Skills")]
    public Transform skillObjectsParent;
    public List<SkillBase> skillList;
    public SkillBase currentSelectedSkill;

    void Awake()
    {
        ControlsManager.PlayerTurnEnded += EndCurrentSkillVisuals;
    }

    void OnDestroy()
    {
        ControlsManager.PlayerTurnEnded -= EndCurrentSkillVisuals;
    }

    public void SkillSetup(Unit unit)
    {
        for (int i = 0; i < skillObjectsParent.childCount; i++)
        {
            skillList.Add(skillObjectsParent.GetChild(i).GetComponent<SkillBase>());
        }

        currentSelectedSkill = skillList[0];
        SkillChangedEvent();

        foreach (SkillBase skill in skillList)
        {
            skill.unit = unit;
        }
    }

    public void ChangeCurrentSkill(int swapDirection)
    {
        int currentSkillIndex = skillList.IndexOf(currentSelectedSkill);
        int newIndex = currentSkillIndex + swapDirection;

        if (newIndex < 0) newIndex = skillList.Count - 1;
        if (newIndex > skillList.Count - 1) newIndex = 0;

        EndCurrentSkillVisuals();
        currentSelectedSkill = skillList[newIndex];

        CurrentSkillPlanningPhaseMethod();
        BattleManager.Instance.DefineUnitTurnOrder();
        ShowSkillVisuals();
        SkillChangedEvent();
    }

    public void ExecuteSkill()
    {
        currentSelectedSkill.StartSkill();
    }

    void SkillChangedEvent()
    {
        SkillChanged?.Invoke(currentSelectedSkill.skillStats.skillName, currentSelectedSkill.skillStats.skillType);
    }

    public void ShowSkillVisuals()
    {
        if (currentSelectedSkill == null) return;
        currentSelectedSkill.VisualAidBegin();
    }

    public void EndCurrentSkillVisuals()
    {
        if (currentSelectedSkill == null) return;
        currentSelectedSkill.VisualAidEnd();
    }

    public void CurrentSkillPlanningPhaseMethod()
    {
        currentSelectedSkill.PlanningPhaseMethod();
    }
}
