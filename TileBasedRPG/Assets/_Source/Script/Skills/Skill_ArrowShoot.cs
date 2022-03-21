using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_ArrowShoot : SkillBase
{
    public float finalDamageValue;
    public float baseValue;
    public float scalingValue;
    
    public override void SetSkillValues()
    {
        baseValue = skillStats.baseValue;
        scalingValue = skillStats.powerScaling * unit.unitStats.power;
        finalDamageValue = baseValue + scalingValue;
    }

    public override void SetSkillDescription(out string skillDescription)
    {
        SetSkillValues();
        
        string description = skillStats.skillDescription;
        description = description.Replace("[FinalDamage]", 
            "<color=#DB2410>" + finalDamageValue.ToString() + "</color>");
        skillDescription = description;
    }

    protected override void SetSkillTarget()
    {
        List<BattleTile> hexesToAffect = new List<BattleTile>();
        vfxsToPlay = new List<BaseVFXEvents>();

        int currentColumn = unit.CurrentTile.hexPos.column;
        BattleTile targetHex = null;
        
        if (unit.facingSide == FacingSide.FacingRight)
            for (int i = currentColumn; i < TileManager.Instance.columns; i++)
            {
                HexPos hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row, i);
                BattleTile newHex = TileManager.Instance.GetTileInMatrix(hexToAffectPos);

                if (newHex == null) continue;
                if (newHex.UnitStandingOnHex == null) continue;
                if (newHex.UnitStandingOnHex.unitType == UnitType.AllyUnit) continue;
                targetHex = newHex;
            }
        else
            for (int i = currentColumn; i > 0; i--)
            {
                HexPos hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row, i);
                BattleTile newHex = TileManager.Instance.GetTileInMatrix(hexToAffectPos);

                if (newHex == null) continue;
                if (newHex.UnitStandingOnHex == null) continue;
                if (newHex.UnitStandingOnHex.unitType == UnitType.AllyUnit) continue;
                targetHex = newHex;
            }

        if (targetHex != null)
        {
            hexesToAffect.Add(targetHex);
            vfxsToPlay.Add(skillVFX[0]);
        }

        if (hexesToAffect.Count <= 0) unit.SkillExecutionEndedEvent();

        for (int u = 0; u < hexesToAffect.Count; u++)
        {
            vfxsToPlay[u].ApplyEffectEvent += ApplyDamage;
            vfxsToPlay[u].PlayAnimation(hexesToAffect[u], unit);
        }

        unit.unitAnims.DOTMeleeAttack(unit);
    }

    private void ApplyDamage(BaseVFXEvents vfxUsed, BattleTile hexToAffect)
    {
        vfxUsed.ApplyEffectEvent -= ApplyDamage;
        vfxsToPlay.Remove(vfxUsed);

        SetSkillValues();

        float damageToDeal = finalDamageValue;
        Unit unitToAffect = hexToAffect.UnitStandingOnHex;

        bool isBackStab = CheckIfBackStab(unit, unitToAffect);
        bool playCriticalDamageAnimation = isBackStab;

        damageToDeal = isBackStab ? damageToDeal * 
            BattleManager.Instance.gameDefinitions.backstabDamageMultiplier : damageToDeal;

        HealthChange healthToChange = new HealthChange(-damageToDeal, isBackStab, playCriticalDamageAnimation);

        if (unitToAffect != null)
        {
            Debug.Log(unit.unitStats.unitName + " Attacking " + unitToAffect.unitStats.unitName);
            unitToAffect.unitStats.ChangeHealth(healthToChange);
        }
        
        if (vfxsToPlay.Count <= 0)
        {
            unit.SkillExecutionEndedEvent();
        }
    }

    public override void VisualAidBegin()
    {
        VisualAidEnd();

        int currentColumn = unit.CurrentTile.hexPos.column;
        BattleTile targetHex = null;
        
        if (unit.facingSide == FacingSide.FacingRight)
            for (int i = currentColumn; i < TileManager.Instance.columns; i++)
            {
                HexPos hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row, i);
                BattleTile newHex = TileManager.Instance.GetTileInMatrix(hexToAffectPos);

                if (newHex == null) continue;
                if (newHex.UnitStandingOnHex == null) continue;
                if (newHex.UnitStandingOnHex.unitType == UnitType.AllyUnit) continue;
                targetHex = newHex;
            }
        else
            for (int i = currentColumn; i > 0; i--)
            {
                HexPos hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row, i);
                BattleTile newHex = TileManager.Instance.GetTileInMatrix(hexToAffectPos);

                if (newHex == null) continue;
                if (newHex.UnitStandingOnHex == null) continue;
                if (newHex.UnitStandingOnHex.unitType == UnitType.AllyUnit) continue;
                targetHex = newHex;
            }

        animatedHexes.Add(targetHex);

        foreach (BattleTile hex in animatedHexes)
        {
            if (hex == null) continue;
            hex.PlayDangerAnim();
        }
    }

    public override void PlanningPhaseMethod()
    {
        unit.unitStats.ChangeTurnPriority(TurnPriority.Normal);
    }

    public override void VisualAidEnd()
    {
        if (animatedHexes.Count <= 0) return;

        foreach (BattleTile hex in animatedHexes)
        {
            if (hex == null) continue;
            hex.StopAnimations();
        }
        animatedHexes.Clear();
    }

}
