using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnChanger : MonoBehaviour
{
    public eTurn Turns;

    public void SwitchTurn(eTurn Turn)
    {
        Turns = Turn;
        
        print("turn: " + Turn);

        switch (Turn)
        {
            case eTurn.PlayerFirst:
                GameStateSwitch.Instance.SwitchState(eGameState.QTEAttack);
                break;
            case eTurn.EnemyFirst:
                GameStateSwitch.Instance.SwitchState(eGameState.QTEBlock);
                break;
            case eTurn.PlayerSecond:
                GameStateSwitch.Instance.SwitchState(eGameState.QTEAttack);
                break;
            case eTurn.EnemySecond:
                GameStateSwitch.Instance.SwitchState(eGameState.QTEBlock);
                break;
            case eTurn.BothDone:
                GameStateSwitch.Instance.SwitchState(eGameState.NextRound);
                break;
            default:
                break;
        }
    }
}