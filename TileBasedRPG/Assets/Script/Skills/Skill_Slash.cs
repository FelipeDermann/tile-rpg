using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill_Slash : SkillBase
{
    public AnimationEvents skillVFX;

    protected override void ExecuteSkill()
    {
        HexPos hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row,
        unit.CurrentTile.hexPos.column + (1 * unit.GetSideDirection()));
        BattleTile hexToAffect = TileManager.Instance.GetTileInMatrix(hexToAffectPos);
        
        if (hexToAffect == null) return;

        skillVFX.ApplyEffectEvent += ApplyDamage;
        skillVFX.PlayAnimation(hexToAffect, unit);
    }

    private void ApplyDamage(AnimationEvents vfxUsed, BattleTile _hexToAffect)
    {
        vfxUsed.ApplyEffectEvent -= ApplyDamage;

        float damageToDeal = unit.unitStats.power * skillStats.powerScaling;
        Unit unitToAffect = _hexToAffect.UnitStandingOnHex;
        if (unitToAffect != null)
        {
            Debug.Log(unit.unitStats.unitName + " Attacking " + unitToAffect.unitStats.unitName);
            unitToAffect.unitStats.ChangeHealth(-damageToDeal);
        }

        unit.SkillExecutionEndedEvent();
    }
}
