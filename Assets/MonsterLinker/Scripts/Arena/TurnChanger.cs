using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnChanger : MonoBehaviour
{
    public int turns = 0;
    
    public void SwitchTurn(eTurn Turn)
    {
        turns += 1;

        if (turns > 2)
        {
            print("both monsters attacked, starting next round");
            Turn = eTurn.NextRound;
            turns = 0;
        }

        print("turn: " + Turn);

        switch (Turn)
        {
            case eTurn.NextRound:
                GameStateSwitch.Instance.SwitchState(eGameState.NextRound);
                break;
            case eTurn.Player:
                GameStateSwitch.Instance.SwitchState(eGameState.QTEAttack);
                break;
            case eTurn.Enemy:
                GameStateSwitch.Instance.SwitchState(eGameState.QTEBlock);
                break;
        }
    }
}