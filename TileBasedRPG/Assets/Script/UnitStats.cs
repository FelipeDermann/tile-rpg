using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [Header("Main Stats")]
    public int currentHealth;
    public int currentEnergy;

    [Header("References")]
    public ScriptableUnitStats stats;

    public void InitiateStats()
    {
        currentHealth = stats.health;
        currentEnergy = stats.energy;
    }
}
