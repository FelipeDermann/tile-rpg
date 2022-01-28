using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "ScriptableObjects/UnitStats", order = 1)]
public class ScriptableUnitStats : ScriptableObject
{
    [Header("Name")]
    public string unitName;

    [Header("Base stats")]
    public int health;
    public int energy;
    public int power;
    public int aid;
    public int technique;
    public int speed;
}
