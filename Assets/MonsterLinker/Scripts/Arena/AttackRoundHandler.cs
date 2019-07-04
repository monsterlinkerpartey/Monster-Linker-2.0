using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handling the Fight rounds, calling animations and qtes and damage
public class AttackRoundHandler : MonoBehaviour
{
    public List<Attack> curAttackList;
    public Attack curAttack;
    public int maxRounds;
    public int curRound;

    public AnimationHandler animationhandler;
    public QTEHandler qtehandler;
    public BAEffectsHandler baeffectshandler;
    public TurnChanger turnchanger;

    //Called by GameStateSwitch, depending on state gives enemy or player attack list
    public void GetAttackList(List<Attack> curAttacks)
    {
        curAttackList = curAttacks;
        maxRounds = curAttackList.Count;
        curRound = 1;
        curAttack = curAttackList[curRound];
    }   
    
    public void NextAttack()
    {
        if (curRound <= maxRounds)
        {
            curRound += 1;
            curAttack = curAttackList[curRound];
        }
        else
        {
            CheckForTurn();
        }
    }

    public void CheckForTurn()
    {
        if (turnchanger.Turns == eTurn.EnemyFirst)
        {
            turnchanger.SwitchTurn(eTurn.PlayerSecond);
        }
        else if (turnchanger.Turns == eTurn.PlayerFirst)
        {
            turnchanger.SwitchTurn(eTurn.EnemySecond);
        }
        else
        {
            turnchanger.SwitchTurn(eTurn.BothDone);
        }
    }

    //Called by GameStateSwitch at the beginning of a turn
    public void StartAttack()
    {
        switch (GameStateSwitch.Instance.GameState)
        {
            case eGameState.QTEAttack:
                baeffectshandler.Playerturn = true;
                if (curAttack.AttackType == eAttackType.BA)
                {
                    qtehandler.SetType(eQTEType.Attack);
                }
                else if (curAttack.AttackType == eAttackType.FA)
                {
                    //qtehandler.SetType(eQTEType.FA, maxRounds);
                }
                animationhandler.PlayerAttack(curAttack.AnimationName);
                break;
            case eGameState.QTEBlock:
                baeffectshandler.Playerturn = false;
                qtehandler.SetType(eQTEType.Block);       
                animationhandler.EnemyAttack(curAttack.AnimationName);
                break;
            default:
                Debug.LogError("not the right game state!");
                break;
        }
    }

    public void SetEffectValues()
    {
        baeffectshandler.curAttack = curAttack;
    }

    //TODO:
    //- attacklist von feralartcheck/enemystatemachine für wieviele "runden"
    //- auslesen aus curattack: 
    //      - BA oder FA
    //      - animation: an animationshandler
    //      - base effects: an baeffectshandler
    //      - qtehandler: info geben ob block/attack, BA/FA
    //      - nach letzter runde an turnchanger weitergeben

        //    if (turnchanger.Turns == eTurn.EnemyFirst)
        //{
        //    turnchanger.SwitchTurn(eTurn.PlayerSecond);
        //}
        //else if (turnchanger.Turns == eTurn.PlayerFirst)
        //{
        //    turnchanger.SwitchTurn(eTurn.EnemySecond);
        //}
        //else
        //{
        //    turnchanger.SwitchTurn(eTurn.BothDone);
        //}


}
