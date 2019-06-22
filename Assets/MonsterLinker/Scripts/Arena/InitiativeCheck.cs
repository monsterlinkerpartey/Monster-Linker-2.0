﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeCheck : MonoBehaviour
{
    public List<BaseAttack> curPlayerInput;
    public List<BaseAttack> curEnemyInput;

    public int PlayerSpeed;
    public int EnemySpeed;

    public ArenaUIHandler arenaui;
    public TurnChanger turnchanger;

    public void GetSpeedValues()
    {
        foreach (BaseAttack attack in curPlayerInput)
        {
            PlayerSpeed += attack.Speed;
        }
        foreach (BaseAttack attack in curEnemyInput)
        {
            EnemySpeed += attack.Speed;
        }

        print("playerspeed: " + PlayerSpeed + "\n enemyspeed: " + EnemySpeed);
    }

    public void CompareSpeed()
    {
        print("comparing speeds");
        if (PlayerSpeed > EnemySpeed)
        {
            print("players turn");
            arenaui.PlayerInitiativeArrow.enabled = true;
            ShowSpeedandTurn(1f);
            turnchanger.SwitchTurn(eTurn.Player);
        }
        else if (EnemySpeed > PlayerSpeed)
        {
            print("enemys turn");
            arenaui.EnemyInitiativeArrow.enabled = true;
            ShowSpeedandTurn(1f);
            turnchanger.SwitchTurn(eTurn.Enemy);
        }
        else
        {
            print("players turn");
            arenaui.PlayerInitiativeArrow.enabled = true;
            ShowSpeedandTurn(1f);
            turnchanger.SwitchTurn(eTurn.Player);
        }
    }

    IEnumerator ShowSpeedandTurn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
