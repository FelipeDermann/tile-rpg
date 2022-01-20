using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public event Action<float, float, float> HealthChanged;

    [Header("Main Stats")]
    public string unitName;
    public float currentHealth;
    public float currentEnergy;

    [Header("Secondary Stats")]
    public int maxHealth;
    public int maxEnergy;
    public int power;
    public int aid;
    public int technique;
    public int speed;

    [Header("References")]
    public ScriptableUnitStats stats;

    public void InitiateStats()
    {
        unitName = stats.unitName;

        currentHealth = stats.health;
        currentEnergy = stats.energy;
        maxHealth = stats.health;
        maxEnergy = stats.energy;

        power = stats.power;
        aid = stats.aid;
        technique = stats.technique;
        speed = stats.speed;
    }

    public void ChangeHealth(float healthChange)
    {
        currentHealth += healthChange;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) currentHealth = 0;

        HealthChanged?.Invoke(currentHealth, maxHealth, healthChange);
    }
}
