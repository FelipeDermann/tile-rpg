using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill_Slash : SkillBase
{
    public AnimationEvents skillVFX;

    float damageToDeal;
    HexPos hexToAffectPos;
    BattleTile hexToAffect;
    Unit unitToAffect;

    protected override void ExecuteSkill()
    {
        hexToAffectPos = new HexPos(unit.CurrentTile.hexPos.row,
        unit.CurrentTile.hexPos.column + (1 * unit.GetSideDirection()));
        hexToAffect = TileManager.Instance.GetTileInMatrix(hexToAffectPos);
       
        unitToAffect = null;
        if (hexToAffect != null) unitToAffect = hexToAffect.UnitStandingOnHex;

        skillVFX.ApplyEffectEvent += ApplyDamage;
        skillVFX.PlayAnimation(hexToAffect.transform, unit.CurrentTile.orderInLayer+2);
    }

    private void ApplyDamage()
    {
        skillVFX.ApplyEffectEvent -= ApplyDamage;

        damageToDeal = unit.unitStats.power * skillStats.powerScaling;
        if (unitToAffect != null)
        {
            Debug.Log(unit.unitStats.unitName + " Attacking " + unitToAffect.unitStats.unitName);
            unitToAffect.unitStats.ChangeHealth(-damageToDeal);
        }

        unit.SkillExecutionEndedEvent();
    }
}
