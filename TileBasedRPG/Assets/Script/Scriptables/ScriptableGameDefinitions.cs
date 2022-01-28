using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDefinitions", menuName = "ScriptableObjects/GameDefinitions", order = 1)]
public class ScriptableGameDefinitions : ScriptableObject
{
    [Header("Times and delays")]
    public float delayToEndExecPhase;
    public float delayToExecuteNextUnitSkill;
    public float endTurnButtonHoldTime;
    public float arrowMoveDelay;

    [Header("Gameplay")]
    [Tooltip("Skill damage is multiplied by this number when attacking an enemy from behind")]
    [Range(1, 10)]
    public float backstabDamageMultiplier;
}
