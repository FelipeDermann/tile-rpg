using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill_Slash : SkillBase
{
    protected override void ExecuteSkill()
    {
        List<BattleTile> hexesToAffect = new List<BattleTile>();
        vfxsToPlay = new List<AnimationEvents>();

        for (int i = 0; i < rowsToAffect; i++)
        {
            HexPos hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row,
            unit.CurrentTile.hexPos.column + (1 * unit.GetSideDirection()));
            BattleTile newHex = TileManager.Instance.GetTileInMatrix(hexToAffectPos);

            if (newHex == null) continue;

            hexesToAffect.Add(newHex);
            vfxsToPlay.Add(skillVFX[i]);
        }

        if (hexesToAffect.Count <= 0) unit.SkillExecutionEndedEvent();

        for (int u = 0; u < hexesToAffect.Count; u++)
        {
            vfxsToPlay[u].ApplyEffectEvent += ApplyDamage;
            vfxsToPlay[u].PlayAnimation(hexesToAffect[u], unit);
        }

        unit.unitAnims.DOTMeleeAttack(unit);
    }

    private void ApplyDamage(AnimationEvents vfxUsed, BattleTile hexToAffect)
    {
        vfxUsed.ApplyEffectEvent -= ApplyDamage;
        vfxsToPlay.Remove(vfxUsed);

        float damageToDeal = unit.unitStats.power * skillStats.powerScaling;
        Unit unitToAffect = hexToAffect.UnitStandingOnHex;

        HealthChange healthToChange;
        healthToChange.isBackStab = CheckIfBackStab(unit, unitToAffect);
        healthToChange.playCriticalDamageAnimation = healthToChange.isBackStab;
        
        damageToDeal = healthToChange.isBackStab ? damageToDeal * 
            BattleManager.Instance.gameDefinitions.backstabDamageMultiplier : damageToDeal;
        healthToChange.healthValue = -damageToDeal;

        if (unitToAffect != null)
        {
            Debug.Log(unit.unitStats.unitName + " Attacking " + unitToAffect.unitStats.unitName);
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
            hex.PlayDangerAnim();
    }

    public override void PlanningPhaseMethod()
    {
        unit.unitStats.ChangeTurnPriority(TurnPriority.Normal);
        skillStats.Test();
    }

    public override void VisualAidEnd()
    {
        if (animatedHexes.Count <= 0) return;

        foreach (BattleTile hex in animatedHexes)
            hex.StopAnimations();
        animatedHexes.Clear();
    }
}
