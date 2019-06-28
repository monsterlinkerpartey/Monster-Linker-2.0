using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BAEffectsHandler : MonoBehaviour
{
    public float PlayerCurHP;
    public float EnemyCurHP;

    [SerializeField] float PlayerCurRP;
    [SerializeField] float EnemyCurRP;

    [SerializeField] float TotalDmgTaken;
    [SerializeField] float TotalDmgDealt;

    //Set by GameStateSwitch during Ini Check
    public List<Attack> curEnemyAttacks;
    public List<Attack> curPlayerAttacks;

    [SerializeField] Attack curAttack;

    //Set by QTEHandler
    [Tooltip("Influenced by the QTE result")]
    public float DMGModifier;

    //GlobalVars.AttackRound for curAttack

    //HACK possible needs to be rewritten later when FAs are implemented
    public void GetAttackLists(List<BaseAttack> Playerlist, List<BaseAttack> Enemylist)
    {
        foreach (Attack attack in Playerlist)
        {
            curPlayerAttacks.Add(attack);
        }
        foreach (Attack attack in Enemylist)
        {
            curEnemyAttacks.Add(attack);
        }
    }

    public void SetCurAttack()
    {
        switch(GameStateSwitch.Instance.GameState)
        {
            case (eGameState.QTEAttack):
                curAttack = curPlayerAttacks[GlobalVars.AttackRound - 1];
                EnemyTakesDmg();
                print("setting curAttack to " + curPlayerAttacks[GlobalVars.AttackRound - 1]);
                break;
            case (eGameState.QTEBlock):
                curAttack = curEnemyAttacks[GlobalVars.AttackRound - 1];
                print("setting curAttack to " + curEnemyAttacks[GlobalVars.AttackRound - 1]);
                PlayerTakesDmg();
                break;
        }
    }

    public void PlayerTakesDmg()
    {
        print("dealing dmg to player");
        PlayerCurHP -= curAttack.DMG * DMGModifier;
        EnemyCurHP += curAttack.HPGain;
        EnemyCurRP += curAttack.RPGain;
        print("Player takes " + (curAttack.DMG * DMGModifier) + " DMG\n Enemy gains " + curAttack.RPGain + " RP");
        print("Player HP: " + PlayerCurHP + ", Player RP: " + PlayerCurRP +", Enemy HP: " + EnemyCurHP+", Enemy RP: " + EnemyCurRP);

        TotalDmgTaken += curAttack.DMG * DMGModifier;
    }

    public void EnemyTakesDmg()
    {       
        print("dealing dmg to enemy");
        EnemyCurHP -= curAttack.DMG * DMGModifier;
        PlayerCurHP += curAttack.HPGain;
        PlayerCurRP += curAttack.RPGain;
        print("Enemy takes " + (curAttack.DMG * DMGModifier) + " DMG\n Player gains " + curAttack.RPGain + " RP");
        print("Player HP: " + PlayerCurHP + ", Player RP: " + PlayerCurRP + ", Enemy HP: " + EnemyCurHP+", Enemy RP: " + EnemyCurRP);

        TotalDmgDealt += curAttack.DMG * DMGModifier;
    }
}
