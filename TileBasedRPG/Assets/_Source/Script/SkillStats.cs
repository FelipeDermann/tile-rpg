using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStats : MonoBehaviour
{
    public ScriptableSkillStats skillScriptable;
    private SkillBase skill;
    
    [Header("Info")]
    [HideInInspector] public string skillName;
    [HideInInspector] public SkillType skillType;

    [Header("Base Attributes")]
    [HideInInspector] public float energyCost;
    [HideInInspector] public int cooldown;
    [HideInInspector] public int effectDuration;
    [HideInInspector] public float baseValue;

    [Header("Stat Scalings")]
    [HideInInspector] public float powerScaling;
    [HideInInspector] public float aidScaling;
    [HideInInspector] public float techScaling;

    [HideInInspector] public string skillDescription;
    [HideInInspector] public string skillExtraInfo;

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

        skill = GetComponent<SkillBase>();
    }

    public string SetSkillExtraInfo()
    {
        string description;
        
        skill.SetSkillDescription(out description);

        return description;
    }
}
