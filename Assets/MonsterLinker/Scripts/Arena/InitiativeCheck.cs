using System.Collections;
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

    [SerializeField] float ShowIniCheckSecs;

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

    public IEnumerator CompareSpeed()
    {
        {
            print("comparing speeds");
            if (PlayerSpeed > EnemySpeed)
            {
                print("players turn");
                arenaui.SetSpeedValues(EnemySpeed, PlayerSpeed);
                arenaui.SetIniArrow("p");
                yield return new WaitForSeconds(ShowIniCheckSecs);
                turnchanger.SwitchTurn(eTurn.PlayerFirst);
            }
            else if (EnemySpeed > PlayerSpeed)
            {
                print("enemys turn");
                arenaui.SetSpeedValues(EnemySpeed, PlayerSpeed);
                arenaui.SetIniArrow("e");
                yield return new WaitForSeconds(ShowIniCheckSecs);
                turnchanger.SwitchTurn(eTurn.EnemyFirst);
            }
            else
            {
                print("players turn");
                arenaui.SetSpeedValues(EnemySpeed-1, PlayerSpeed);
                arenaui.SetIniArrow("p");
                yield return new WaitForSeconds(ShowIniCheckSecs);
                turnchanger.SwitchTurn(eTurn.PlayerFirst);
            }
        }
        GameStateSwitch.Instance.animationhandler.MoveToMiddle();
    }

    public void ResetSpeedValues()
    {
        PlayerSpeed = 0;
        EnemySpeed = 0;
    }
}
