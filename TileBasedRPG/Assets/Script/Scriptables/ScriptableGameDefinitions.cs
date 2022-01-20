using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDefinitions", menuName = "ScriptableObjects/GameDefinitions", order = 1)]
public class ScriptableGameDefinitions : ScriptableObject
{
    public float delayToEndExecPhase;
    public float delayToExecuteNextUnitSkill;
    public float endTurnButtonHoldTime;
    public float arrowMoveDelay;
}
