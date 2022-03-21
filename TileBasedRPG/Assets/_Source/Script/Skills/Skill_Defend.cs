using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Defend : SkillBase
{
    public float finalBuffValue;
    public float baseValue;
    public float scalingValue;
    
    public override void SetSkillValues()
    {
        baseValue = skillStats.baseValue;
        scalingValue = skillStats.techScaling * unit.unitStats.technique;
        finalBuffValue = baseValue + scalingValue;
    }
    
    public override void SetSkillDescription(out string skillDescription)
    {
        SetSkillValues();
        
        string description = skillStats.skillDescription;
        description = description.Replace("[FinalBuff]", 
            "<color=#3685FF>" + finalBuffValue.ToString() + "%" + "</color>");
        skillDescription = description;
    }
    
    protected override void SetSkillTarget()
    {
        List<BattleTile> hexesToAffect = new List<BattleTile>();
        vfxsToPlay = new List<BaseVFXEvents>();

        for (int i = 0; i < 1; i++)
        {
            HexPos hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row, unit.CurrentTile.hexPos.column);

            BattleTile newHex = TileManager.Instance.GetTileInMatrix(hexToAffectPos);
            if (newHex == null) continue;

            hexesToAffect.Add(newHex);
            vfxsToPlay.Add(skillVFX[i]);
        }

        if (hexesToAffect.Count <= 0) unit.SkillExecutionEndedEvent();

        for (int u = 0; u < hexesToAffect.Count; u++)
        {
            vfxsToPlay[u].ApplyEffectEvent += ApplyEffect;
            vfxsToPlay[u].PlayAnimation(hexesToAffect[u], unit);
        }

        unit.unitAnims.DOTSpellCast(unit);
    }

    private void ApplyEffect(BaseVFXEvents vfxUsed, BattleTile hexToAffect)
    {
        vfxUsed.ApplyEffectEvent -= ApplyEffect;
        vfxsToPlay.Remove(vfxUsed);

        int buffValue = (int)finalBuffValue;
        Unit unitToAffect = hexToAffect.UnitStandingOnHex;

        Buff defenseBuff = new Buff(BuffType.DamageReductionBuff, buffValue, skillStats.skillScriptable.effectDuration);
        
        if (unitToAffect != null)
        {
            Debug.Log(unit.unitStats.unitName + " Attacking " + unitToAffect.unitStats.unitName);
            unitToAffect.unitStats.ReceiveBuff(defenseBuff);
        }

        if (vfxsToPlay.Count <= 0) unit.SkillExecutionEndedEvent();
    }

    public override void PlanningPhaseMethod()
    {
        unit.unitStats.ChangeTurnPriority(TurnPriority.First);
    }

    public override void VisualAidBegin()
    {
        VisualAidEnd();

        int a = -1;

        for (int i = 0; i < 1; i++)
        {
            HexPos hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row, unit.CurrentTile.hexPos.column);
            a++;

            BattleTile newHex = TileManager.Instance.GetTileInMatrix(hexToAffectPos);
            if (newHex == null || newHex.PlayingDangerAnim) continue;

            animatedHexes.Add(newHex);
        }

        foreach (BattleTile hex in animatedHexes)
            hex.PlayBuffAnim();
    }

    public override void VisualAidEnd()
    {
        if (animatedHexes.Count <= 0) return;

        foreach (BattleTile hex in animatedHexes)
            hex.StopAnimations();
        animatedHexes.Clear();
    }
}
