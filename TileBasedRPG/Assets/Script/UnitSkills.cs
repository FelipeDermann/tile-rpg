using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkills : MonoBehaviour
{
    public event Action<string, SkillType> SkillChanged;

    [Header("Unit Skills")]
    public SkillBase currentSelectedSkill;
    public List<SkillBase> skillList;

    public void SetFirstSkill()
    {
        currentSelectedSkill = skillList[0];
        CallSkillChangeEvent();
    }

    public void ChangeCurrentSkill(int swapDirection)
    {
        int currentSkillIndex = skillList.IndexOf(currentSelectedSkill);
        int newIndex = currentSkillIndex + swapDirection;

        if (newIndex < 0) newIndex = skillList.Count - 1;
        if (newIndex > skillList.Count - 1) newIndex = 0;

        currentSelectedSkill = skillList[newIndex];

        CallSkillChangeEvent();
    }

    void CallSkillChangeEvent()
    {
        SkillChanged?.Invoke(currentSelectedSkill.skillStats.skillName, currentSelectedSkill.skillStats.skillType);
    }
}
