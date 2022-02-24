using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public static Action<float, Unit> ShowDamageText;
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
    public float damageReduction = 0;

    [Header("Buffs")] 
    [SerializeField] public List<Buff> buffList;
    
    [Header("References")]
    public ScriptableUnitStats stats;

    [SerializeField] TurnPriority turnPriority;
    private Unit myUnit;

    void Awake()
    {
        myUnit = GetComponent<Unit>();
        buffList = new List<Buff>();
        BattleManager.PlanningPhaseStarted += CheckBuffs;
    }
    
    void OnDestroy()
    {
        BattleManager.PlanningPhaseStarted -= CheckBuffs;
    }

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
        float healthValueToChange = healthChange.healthValue;
        if (healthValueToChange < 0)
        {
            float damageReductionCalc = (100 - damageReduction) / 100;
            
            healthValueToChange = healthValueToChange * damageReductionCalc;
            DamageTaken?.Invoke(healthChange.playCriticalDamageAnimation);
        }
            
        currentHealth += healthValueToChange;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) currentHealth = 0;

        HealthChanged?.Invoke(currentHealth, maxHealth, healthValueToChange);
        ShowDamageText(healthValueToChange, myUnit);
        
        if (currentHealth <= 0) UnitDied?.Invoke();
    }

    public void ReceiveBuff(Buff buff)
    {
        for (var i = buffList.Count - 1; i >= 0; i--)
        {
            Buff activeBuff = buffList[i];
            if(activeBuff.buffType == buff.buffType) RemoveBuff(activeBuff);
        }
        
        buff.ApplyOrCancelBuff(GetComponent<Unit>(), false);
        buffList.Add(buff);
    }

    void CheckBuffs()
    {
        for (var i = buffList.Count - 1; i >= 0; i--)
        {
            Buff buff = buffList[i];
            if (buff.CheckIfBuffExpired()) RemoveBuff(buff);
        }
    }

    public void RemoveBuff(Buff buff)
    {
        buff.ApplyOrCancelBuff(GetComponent<Unit>(), true);
        Debug.Log(buffList.Count);
        buffList.Remove(buff);
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
