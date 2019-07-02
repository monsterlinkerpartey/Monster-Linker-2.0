﻿public enum eGameState
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

public enum eImplant
{
    RisingRage,
    UnleashedMode,
    SuperFA,
    TempInputSlot
}

