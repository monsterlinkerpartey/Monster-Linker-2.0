public enum eTournament
{
    Demo,
    Train,
    One,
    Two,
    Three
}

public enum eQTEZones
{
    Early,
    Good,
    Perfect,
    Late
}

public enum eTurn
{
    Nobody,
    Player,
    Enemy
}

public enum eQTEState
{
    Ready,
    Delayed,
    Running,
    Done
}

public enum eQTEType
{
    None,
    Attack,
    Counter,
    Block
}

public enum eQTEResult
{
    Null,
    Fail,
    Good,
    Perfect    
}

public enum eAdvantage
{
    None,
    Advantage,
    Stalemate,
    Disadvantage
}

public enum eArena
{
    HardReset,
    StartFight,
    Intro,
    PlayerInput,
    EnemyInput,
    AdvantageCheck,
    QTEAttack,
    DealDamage,
    NextAttack,
    NextRound,
    Death,    
    Result
}


///<summary>
/// Alte enums
/// </summary>
public enum eGameMode
{    
    GameStart,
    HomeScreen,
    MapScreen,
    ArenaScreen,
    AStartFight,
    AComboInput,
    AConfirmCombo,
    AShowFinalCombos,
    AStartQTE,
    EnemyMove,
    AResultScreen
}

public enum eMenu
{
    None,
    Home,
    Map,
    Upgrade,
    Arena
}


