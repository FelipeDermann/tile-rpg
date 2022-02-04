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

public struct HealthChange
{
    public float healthValue;
    public bool isBackStab;
    public bool playCriticalDamageAnimation;
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

