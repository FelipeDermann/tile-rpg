using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillBase : MonoBehaviour
{
    public ScriptableSkillStats skillStats;
    public Unit unit;

    public virtual void StartSkill()
    {
        Debug.Log(unit.unitStats.unitName + " Starting Skill: " + skillStats.skillName);
        InterfaceManager.Instance.PlaySkillNameAnim(skillStats);

        ExecuteSkill();
    }

    protected virtual void ExecuteSkill()
    {
        Debug.Log(unit.unitStats.unitName + " Executed Skill: " + skillStats.skillName);
        unit.SkillExecutionEndedEvent();
    }

    //when a unit's skill changes, check if skill modifies 
    //something during the planning phase
    public virtual void PlanningPhaseMethod()
    {

    }
}
