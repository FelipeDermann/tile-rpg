using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill_Slash : SkillBase
{
    public override void ExecuteSkill()
    {
        Debug.Log("Skill Executed: Slash");
    }
}
