using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "ScriptableObjects/UnitStats", order = 1)]
public class ScriptUnitStats : ScriptableObject
{
    public string unitName;
    public int health, energy, attack, defense, speed;
}
