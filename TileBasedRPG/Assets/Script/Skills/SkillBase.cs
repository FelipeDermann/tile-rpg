using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillBase : MonoBehaviour
{
    public ScriptableSkillStats skillStats;

    private float energyCost;
    private float cooldown;

    private float powerScaling;
    private float aidScaling;
    private float techScaling;

    public virtual void ExecuteSkill()
    {
        Debug.Log("Skill Executed: Base Skill");
    }
}
