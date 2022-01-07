using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillStats", menuName = "ScriptableObjects/SkillStats", order = 1)]
public class ScriptableSkillStats : ScriptableObject
{
    [Header("Main Info")]
    public string skillName;
    public SkillType skillType;

    [Header("Cost")]
    public float energyCost;
    public float cooldown;

    [Header("Stat Scalings")]
    public float powerScaling;
    public float aidScaling;
    public float techScaling;
}
