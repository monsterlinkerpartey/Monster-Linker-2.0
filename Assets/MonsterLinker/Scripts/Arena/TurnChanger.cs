using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnChanger : MonoBehaviour
{
    public int turns = 0;
    
    public void SwitchTurn(eTurn Turn)
    {
        turns += 1;

        switch (Turn)
        {
            case eTurn.NextRound:

                break;
            case eTurn.Player:
                GameStateSwitch.Instance.SwitchState(eGameState.QTEAttack);
                break;
            case eTurn.Enemy:
                GameStateSwitch.Instance.SwitchState(eGameState.QTEBlock);
                break;
        }

        print("turn: " + Turn);
    }
}