using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BAEffectsHandler : MonoBehaviour
{
    [Tooltip("If Rising Rage is active")]
    public float RisingRageModifier = 1.0f;

    public bool Playerturn;

    public float maxPlayerHP;
    public float curPlayerHP;
    public float curPlayerRP;

    public float PlayerRPatAttackStart;

    public float maxEnemyHP;
    public float curEnemyHP;
    public float curEnemyRP;
    
    public float TotalDmgTaken;
    public float TotalDmgDealt;

    public ArenaUIHandler arenaui;
    public StatusBarHandler enemystatusbar;
    public StatusBarHandler playerstatusbar;

    [HideInInspector] public Attack curAttack;

    //Set by QTEHandler
    [Tooltip("Influenced by the QTE result")]
    public float DMGModifier;
    [Tooltip("Influenced by the Endurance result")]
    public float EnduranceModifier;
    [Tooltip("Multiplier for percentage, e.g. x2 -> 1RP gains 2Dmg")]
    public float RRPercentage;

    ////Set by GameStateSwitch during Ini Check
    //public List<Attack> curEnemyAttacks;
    //public List<Attack> curPlayerAttacks;

    //GlobalVars.AttackRound for curAttack

    //HACK possible needs to be rewritten later when FAs are implemented
    //public void GetAttackLists(List<Attack> Playerlist, List<Attack> Enemylist)
    //{
    //    curPlayerAttacks = Playerlist;
    //    //foreach (Attack attack in Playerlist)
    //    //{
    //    //    curPlayerAttacks.Add(attack);
    //    //}

    //    foreach (Attack attack in Enemylist)
    //    {
    //        curEnemyAttacks.Add(attack);
    //    }
    //}

    public void SetMashValue(int mashCount)
    {
        EnduranceModifier = (mashCount / 100) + 1;      
    }

    public void StartHpandRPValues(float playerHP,int playerRP,float enemyHP,int enemyRP)
    {
        maxPlayerHP = playerHP;
        curPlayerHP = playerHP;
        curPlayerRP = playerRP;

        maxEnemyHP = enemyHP;
        curEnemyHP = enemyHP;
        curEnemyRP = enemyRP;
    }

    public void DMGModification(float dmgModifier, int RPgained)
    {
        DMGModifier = dmgModifier;

        if (Mathf.RoundToInt(EnduranceModifier) <= 0)
            EnduranceModifier = Mathf.Round(1);

        if (GameStateSwitch.Instance.Implant == eImplant.RisingRage)
        {            
            RisingRageModifier = 1 + (PlayerRPatAttackStart * (RRPercentage / 100));
            print("rising rage active: x" + RisingRageModifier + " dmg");
        }
        else
            RisingRageModifier = 1;

        switch (GameStateSwitch.Instance.GameState)
        {
            case eGameState.QTEAttack:
                float curDMG = ((curAttack.DMG * RisingRageModifier) * EnduranceModifier) * DMGModifier;
                EnemyTakesDmg(Mathf.Round(curDMG));
                break;
            case eGameState.QTEBlock:
                curDMG = curAttack.DMG * DMGModifier;
                PlayerTakesDmg(Mathf.Round(curDMG), RPgained);
                break;
            default:
                break;
        }        
    }

    public void PlayerPaysRP()
    {
        curPlayerRP -= curAttack.RPCost;
        playerstatusbar.RPTick(Mathf.RoundToInt(curPlayerRP));
    }

    public void EnemyPaysRP()
    {
        curEnemyRP -= curAttack.RPCost;
        enemystatusbar.RPTick(Mathf.RoundToInt(curEnemyRP));
    }

    public void PlayerTakesDmg(float curDMG, int RPgained)
    {
        print("dealing dmg to player");
        curPlayerHP -= curDMG;
        curPlayerRP += RPgained;
        curEnemyHP += curAttack.HPGain;

        if (Mathf.Round(curEnemyHP) >= Mathf.Round(maxEnemyHP))
            curEnemyHP = Mathf.Round(maxEnemyHP);

        curEnemyRP += curAttack.RPGain;
        if (Mathf.RoundToInt(curEnemyRP) >= (int)100)
            curEnemyRP = 100.0f;

        if (Mathf.RoundToInt(curPlayerRP) >= (int)100)
            curPlayerRP = 100.0f;

        TotalDmgTaken += curDMG;

        //GameStateSwitch.Instance.statusbarhandler.LerpPlayerHP();

        arenaui.SetPlayerHPandRP(Mathf.RoundToInt(curPlayerHP), Mathf.RoundToInt(curPlayerRP));
        arenaui.SetEnemyHPandRP(Mathf.RoundToInt(curEnemyHP), Mathf.RoundToInt(curEnemyRP));

        UpdateHPandRPbars();

        //print("Player takes " + (curDMG) + " DMG\n Enemy gains " + curAttack.RPGain + " RP");
        //print("Player HP: " + curPlayerHP + ", Player RP: " + curPlayerRP +", Enemy HP: " + curEnemyHP+", Enemy RP: " + curEnemyRP);
    }

    public void EnemyTakesDmg(float curDMG)
    {       
        print("dealing dmg to enemy");
        curEnemyHP -= curDMG;
        curPlayerHP += curAttack.HPGain;
        if (Mathf.Round(curPlayerHP) >= Mathf.Round(maxPlayerHP))
            curPlayerHP = Mathf.Round(maxPlayerHP);

        curPlayerRP += curAttack.RPGain;
        if (Mathf.RoundToInt(curPlayerRP) >= (int)100)
            curPlayerRP = 100.0f;

        TotalDmgDealt += curDMG;

        arenaui.SetPlayerHPandRP(Mathf.RoundToInt(curPlayerHP), Mathf.RoundToInt(curPlayerRP));
        arenaui.SetEnemyHPandRP(Mathf.RoundToInt(curEnemyHP), Mathf.RoundToInt(curEnemyRP));

        UpdateHPandRPbars();

        print("Enemy takes " + (curDMG) + " DMG\n Player gains " + curAttack.RPGain + " RP");
        print("Player HP: " + curPlayerHP + ", Player RP: " + curPlayerRP + ", Enemy HP: " + curEnemyHP+", Enemy RP: " + curEnemyRP);
    }

    public void UpdateHPandRPbars()
    {
        playerstatusbar.HPTick(Mathf.Round(curPlayerHP));
        playerstatusbar.RPTick(Mathf.RoundToInt(curPlayerRP));
        enemystatusbar.HPTick(Mathf.Round(curEnemyHP));
        enemystatusbar.RPTick(Mathf.RoundToInt(curEnemyRP));
    }

    public void ShowTotalDmg(float totaldmg)
    {
        print("total damage this round: " + totaldmg);
    }

    public void ResetDmgCount()
    {
        TotalDmgTaken = 0f;
        TotalDmgDealt = 0f;
    }

    public void CheckForDeath()
    {
        if ( Mathf.RoundToInt(curEnemyHP) > (int) 0 && Mathf.RoundToInt(curPlayerHP) > (int) 0)
        {
            GameStateSwitch.Instance.FightResult = eFightResult.None;
            print("fight state: " + GameStateSwitch.Instance.FightResult);
        }
        else if (Mathf.RoundToInt(curEnemyHP) <= (int) 0)
        {
            GameStateSwitch.Instance.FightResult = eFightResult.Victory;
            print("fight state: " + GameStateSwitch.Instance.FightResult);
            GameStateSwitch.Instance.SwitchState(eGameState.Result);
            return;
        }
        else if (Mathf.RoundToInt(curPlayerHP) <= (int) 0)
        {
            GameStateSwitch.Instance.FightResult = eFightResult.Defeat;
            print("fight state: " + GameStateSwitch.Instance.FightResult);
            GameStateSwitch.Instance.SwitchState(eGameState.Result);
            return;
        }
        //    if (Mathf.Round(hitpoints) <= 0)
        //{
        //    //TODO: set result screen to open up
        //    switch (gameState)
        //    {
        //        case eGameState.QTEAttack:
        //            print("enemy died");
        //            GameStateSwitch.Instance.FightResult = eFightResult.Victory;

        //            break;
        //        case eGameState.QTEBlock:
        //            print("player died");
        //            GameStateSwitch.Instance.FightResult = eFightResult.Defeat;

        //            break;
        //        default:
        //            Debug.LogError("I dunno who died, check BAEffectsHandler");
        //            GameStateSwitch.Instance.FightResult = eFightResult.None;
        //            break;
        //    }

        //    GameStateSwitch.Instance.SwitchState(eGameState.Result);
        //}
    }
}
