public enum TileType
{
    NeutralTile,
    AllyTile,
    EnemyTile,
    NullTile
}

public enum UnitType
{
    AllyUnit,
    EnemyUnit,
    NeutralUnit,
    ObjectUnit
}

public enum FacingSide
{
    FacingRight,
    FacingLeft
}

public enum BattlePhase
{
    PreparationPhase,
    ExecutionPhase,
    EnemyPhase
}

public enum SkillType
{
    None,
    Heal,
    Attack,
    Defense,
    Buff,
    Debuff,
    Ailment,
    Hazard,
    Move,
    Steal,
    Obstacle,
    Unknown
}

public enum TurnPriority
{
    Normal,
    First,
    Last
}

public enum ControlType
{
    KeyboardMouse,
    Gamepad
}

public enum BuffType
{
    PowerBuff,
    AidBuff,
    TechniqueBuff,
    SpeedBuff,
    DamageReductionBuff
}

[System.Serializable]
public class Buff
{
    public BuffType buffType;
    public int buffAmount;
    public int durationInTurns;

    public Buff(BuffType newBuffType, int newBuffAmount, int newDuration)
    {
        buffType = newBuffType;
        buffAmount = newBuffAmount;
        durationInTurns = newDuration;
    }
    
    public void ApplyOrCancelBuff(Unit myUnit, bool removeBuff)
    {
        if (removeBuff) buffAmount = -buffAmount;
        
        switch (buffType)
        {
            case BuffType.PowerBuff:
                myUnit.unitStats.power += buffAmount;
                break;
            case BuffType.AidBuff:
                myUnit.unitStats.aid += buffAmount;
                break;
            case BuffType.TechniqueBuff:
                myUnit.unitStats.technique += buffAmount;
                break;
            case BuffType.SpeedBuff:
                myUnit.unitStats.speed += buffAmount;
                break;
            case BuffType.DamageReductionBuff:
                myUnit.unitStats.damageReduction += buffAmount;
                break;
        }
    }

    public bool CheckIfBuffExpired()
    {
        bool expired = false;
        
        if (durationInTurns <= 0) expired = true;
        else durationInTurns -= 1;

        return expired;
    }
}

public struct HealthChange
{
    public float healthValue;
    public bool isBackStab;
    public bool playCriticalDamageAnimation;

    public HealthChange(float value, bool backStab, bool playCritAnim)
    {
        healthValue = value;
        isBackStab = backStab;
        playCriticalDamageAnimation = playCritAnim;
    }

    public HealthChange(float value)
    {
        healthValue = value;
        isBackStab = false;
        playCriticalDamageAnimation = false;
    }
}

[System.Serializable]
public struct HexPos
{
    public int row, column;

    public HexPos(int _row, int _column)
    {
        row = _row;
        column = _column;
    }
}

