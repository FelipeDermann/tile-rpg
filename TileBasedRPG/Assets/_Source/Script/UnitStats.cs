using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public event Action<float, float, float> HealthChanged;
    public event Action<bool> DamageTaken;
    public event Action UnitDied;

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

    [SerializeField] TurnPriority turnPriority;

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

    public void ChangeHealth(HealthChange healthChange)
    {
        currentHealth += healthChange.healthValue;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) currentHealth = 0;

        HealthChanged?.Invoke(currentHealth, maxHealth, healthChange.healthValue);

        if (healthChange.healthValue < 0) DamageTaken?.Invoke(healthChange.playCriticalDamageAnimation);
        if (currentHealth <= 0) UnitDied?.Invoke();
    }

    public float GetTurnOrderSpeed()
    {
        float finalSpeed = speed;

        switch (turnPriority)
        {
            case TurnPriority.First: finalSpeed = speed + 9999;
                break;
            case TurnPriority.Last: finalSpeed = speed - 9999;
                break;
        }

        return finalSpeed;
    }

    public void ChangeTurnPriority(TurnPriority newTurnPriority)
    {
        turnPriority = newTurnPriority;
    }
}
