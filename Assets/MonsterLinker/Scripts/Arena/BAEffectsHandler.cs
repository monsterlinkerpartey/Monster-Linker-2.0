using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BAEffectsHandler : MonoBehaviour
{
    public bool Playerturn;

    public float maxPlayerHP;
    public float curPlayerHP;
    public float curPlayerRP;

    public float maxEnemyHP;
    public float curEnemyHP;
    public float curEnemyRP;
    
    public float TotalDmgTaken;
    public float TotalDmgDealt;

    public ArenaUIHandler arenaui;

    [HideInInspector] public Attack curAttack;

    //Set by QTEHandler
    [Tooltip("Influenced by the QTE result")]
    public float DMGModifier;

    [Tooltip("Influenced by the Endurance result")]
    public float EnduranceModifier;

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
        if (mashCount <= 0)
            EnduranceModifier = 1.0f;
        else
        {
            EnduranceModifier = (mashCount / 100) + 1;
        }
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

    public void DMGModification(float dmgModifier)
    {
        float curDMG = (curAttack.DMG * EnduranceModifier) * dmgModifier;
        if (Playerturn)
        {
            EnemyTakesDmg(Mathf.Round(curDMG));
        }
        else
        {
            PlayerTakesDmg(Mathf.Round(curDMG));
        }        
    }

    public void PlayerTakesDmg(float curDMG)
    {
        print("dealing dmg to player");
        curPlayerHP -= curDMG;
        curEnemyHP += curAttack.HPGain;
        curEnemyRP += curAttack.RPGain;
        TotalDmgTaken += curDMG;

        arenaui.SetPlayerHPandRP(Mathf.RoundToInt(curPlayerHP), Mathf.RoundToInt(curPlayerRP));
        arenaui.SetEnemyHPandRP(Mathf.RoundToInt(curEnemyHP), Mathf.RoundToInt(curEnemyRP));

        print("Player takes " + (curDMG) + " DMG\n Enemy gains " + curAttack.RPGain + " RP");
        print("Player HP: " + curPlayerHP + ", Player RP: " + curPlayerRP +", Enemy HP: " + curEnemyHP+", Enemy RP: " + curEnemyRP);
    }

    public void PlayerPaysRP()
    {
        curPlayerRP -= curAttack.RPCost;
    }

    public void EnemyPaysRP()
    {
        curEnemyRP -= curAttack.RPCost;
    }

    public void EnemyTakesDmg(float curDMG)
    {       
        print("dealing dmg to enemy");
        curEnemyHP -= curDMG;
        curPlayerHP += curAttack.HPGain;
        curPlayerRP += curAttack.RPGain;
        TotalDmgDealt += curDMG;

        arenaui.SetPlayerHPandRP(Mathf.RoundToInt(curPlayerHP), Mathf.RoundToInt(curPlayerRP));
        arenaui.SetEnemyHPandRP(Mathf.RoundToInt(curEnemyHP), Mathf.RoundToInt(curEnemyRP));

        print("Enemy takes " + (curDMG) + " DMG\n Player gains " + curAttack.RPGain + " RP");
        print("Player HP: " + curPlayerHP + ", Player RP: " + curPlayerRP + ", Enemy HP: " + curEnemyHP+", Enemy RP: " + curEnemyRP);
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

    public void CheckForDeath(float hitpoints, eGameState gameState)
    {
        if (Mathf.Round(hitpoints) <= 0)
        {
            //TODO: set result screen to open up
            switch (gameState)
            {
                case eGameState.QTEAttack:
                    print("enemy died");

                    break;
                case eGameState.QTEBlock:
                    print("player died");

                    break;
                default:
                    Debug.LogError("I dunno who died, check BAEffectsHandler");
                    break;
            }
        }
    }
}
