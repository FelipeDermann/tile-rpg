using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_LifeDrain : SkillBase
{
    protected override void SetSkillTarget()
    {
        Debug.Log(unit.unitStats.unitName + " Executed Skill: " + skillStats.skillName);
        unit.SkillExecutionEndedEvent();
    }
}
