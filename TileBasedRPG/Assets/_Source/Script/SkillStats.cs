using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStats : MonoBehaviour
{
    public ScriptableSkillStats skillScriptable;
    private SkillBase skill;
    
    [Header("Info")]
    public string skillName;
    public SkillType skillType;

    [Header("Base Attributes")]
    public float energyCost;
    public int cooldown;
    public int effectDuration;
    public float baseValue;

    [Header("Stat Scalings")]
    public float powerScaling;
    public float aidScaling;
    public float techScaling;

    public string skillDescription;
    public string skillExtraInfo;

    public void InitialSetup()
    {
        skillName = skillScriptable.skillName;
        skillType = skillScriptable.skillType;

        energyCost = skillScriptable.energyCost;
        cooldown = skillScriptable.cooldown;
        effectDuration = skillScriptable.effectDuration;
        baseValue = skillScriptable.baseValue;

        powerScaling = skillScriptable.powerScaling;
        aidScaling = skillScriptable.aidScaling;
        techScaling = skillScriptable.techScaling;

        skillDescription = skillScriptable.skillDescription;
        skillExtraInfo = skillScriptable.skillExtraInfo;

        skill = GetComponent<SkillBase>();
    }

    public string SetSkillExtraInfo()
    {
        string description;
        
        skill.SetSkillDescription(out description);

        return description;
    }
}
