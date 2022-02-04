using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

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

    [Header("Skill Description")] 
    [TextArea(3,20)] public string skillDescription;
    [TextArea(3,20)] public string skillExtraInfo;

    public void Test()
    {
        Debug.Log("Scriptables can execute code???");
    }
}
