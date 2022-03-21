using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_FirstAid : SkillBase
{  
    [HideInInspector] public float finalHealValue;
    [HideInInspector] public float baseValue;
    [HideInInspector] public float scalingValue;
    
    public override void SetSkillValues()
    {
        baseValue = skillStats.baseValue;
        scalingValue = skillStats.aidScaling * unit.unitStats.aid;
        finalHealValue = baseValue + scalingValue;
    }

    public override void SetSkillDescription(out string skillDescription)
    {
        SetSkillValues();
        
        string description = skillStats.skillDescription;
        description = description.Replace("[FinalHeal]", 
            "<color=#00FB2C>" + finalHealValue.ToString() + "</color>");
        skillDescription = description;
    }

    protected override void SetSkillTarget()
    {
        List<BattleTile> hexesToAffect = new List<BattleTile>();
        vfxsToPlay = new List<BaseVFXEvents>();

        for (int i = 0; i < 1; i++)
        {
            HexPos hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row,
            unit.CurrentTile.hexPos.column + (1 * unit.GetSideDirection()));
            BattleTile newHex = TileManager.Instance.GetTileInMatrix(hexToAffectPos);

            if (newHex == null) continue;

            hexesToAffect.Add(newHex);
            vfxsToPlay.Add(skillVFX[i]);
        }

        if (hexesToAffect.Count <= 0)
        {
            Debug.Log("No hexes on target. Ending skill...");
            unit.SkillExecutionEndedEvent();
        }

        for (int u = 0; u < hexesToAffect.Count; u++)
        {
            vfxsToPlay[u].ApplyEffectEvent += ApplyDamage;
            vfxsToPlay[u].PlayAnimation(hexesToAffect[u], unit);
        }

        unit.unitAnims.DOTSpellCast(unit);
    }

    private void ApplyDamage(BaseVFXEvents vfxUsed, BattleTile hexToAffect)
    {
        vfxUsed.ApplyEffectEvent -= ApplyDamage;
        vfxsToPlay.Remove(vfxUsed);

        SetSkillValues();

        float healToGive = finalHealValue;
        Unit unitToAffect = hexToAffect.UnitStandingOnHex;

        HealthChange healthToChange;
        healthToChange.healthValue = healToGive;
        healthToChange.isBackStab = false;
        healthToChange.playCriticalDamageAnimation = false;

        if (unitToAffect != null)
        {
            Debug.Log(unit.unitStats.unitName + " Healing " + unitToAffect.unitStats.unitName);
            unitToAffect.unitStats.ChangeHealth(healthToChange);
        }

        if (vfxsToPlay.Count <= 0) unit.SkillExecutionEndedEvent();
    }

    public override void VisualAidBegin()
    {
        VisualAidEnd();

        HexPos hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row,
        unit.CurrentTile.hexPos.column + (1 * unit.GetSideDirection()));
        BattleTile hexToAffect = TileManager.Instance.GetTileInMatrix(hexToAffectPos);

        if (hexToAffect == null || hexToAffect.PlayingDangerAnim)
        {
            Debug.Log(unit.unitStats.unitName + "'s " + skillStats.skillName + " failed to find a valid hex.");
            return;
        }

        animatedHexes.Add(hexToAffect);

        foreach (BattleTile hex in animatedHexes)
            hex.PlayHealAnim();
    }

    public override void PlanningPhaseMethod()
    {
        unit.unitStats.ChangeTurnPriority(TurnPriority.Normal);
    }

    public override void VisualAidEnd()
    {
        if (animatedHexes.Count <= 0) return;

        foreach (BattleTile hex in animatedHexes)
            hex.StopAnimations();
        animatedHexes.Clear();
    }
}
