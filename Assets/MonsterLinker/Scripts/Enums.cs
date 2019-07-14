// HACK: Delete this line

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
    Default,
    Normal_FA3Use,
    Normal_NoRP,
    LowHP_FA3Use,
    LowHP_NoRP,
};

public enum eTurn
{
    PlayerFirst,
    PlayerSecond,
    EnemyFirst,
    EnemySecond,
    BothDone,
}

public enum eQTEType
{
    Attack,
    Block,
    FAEndurance,
    FA
}

public enum eQTEState
{
    Waiting,
    Running,
    Endurance,
    Done
}

public enum eQTEZone
{
    None,
    Fail,
    Good,
    Perfect
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

public enum eAttackType
{
    FA,
    BA
}

public enum eImplant
{
    None,
    RisingRage,
    UnleashedMode,
    SuperFA,
    TempInputSlot
}

public enum eFightResult
{
    None,
    Victory,
    Defeat
}

public enum eQTEInput
{
    None,
    QTE,
    Endurance
}

public enum eLoadout
{
    LoadoutOnly,
    FeralArtChoice,
    ImplantChoice
};

public enum eSlotNo
{
    First,
    Second,
    Third
};