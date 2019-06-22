public enum eGameState
{
    Loadout,
    Intro,
    PlayerInput,
    InitiativeCheck,
    QTEAttack,
    QTEBlock,
    NextRound,
    Result
};

public enum eEnemyState
{
    Normal,
    LowHP
};

public enum eTurn
{
    NextRound,
    Player,
    Enemy
}

public enum eQTEState
{
    Waiting,
    Running,
    Fail,
    Good,
    Perfect,
    Done
}

public enum eHomeUI
{
    Title,
    SaveProfile,
    Home,
    Settings,
    Loadout,
    Blacklist,
};

public enum eArenaUI
{
    ArenaIntro,
    PlayerInput,
    PlayerInputConfirm,
    InitiativeCheck,
    PlayerTurn,
    EnemyTurn,
    Result
};

