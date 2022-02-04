using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ArrowRain : SkillBase
{
    protected override void ExecuteSkill()
    {
        Debug.Log(unit.unitStats.unitName + " Executed Skill: " + skillStats.skillName);
        unit.SkillExecutionEndedEvent();
    }
}
