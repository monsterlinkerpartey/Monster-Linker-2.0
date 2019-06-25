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

    [SerializeField] float waitForSecs = 1f;

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
                arenaui.PlayerInitiativeArrow.enabled = true;
                yield return new WaitForSeconds(waitForSecs);
                turnchanger.SwitchTurn(eTurn.PlayerFirst);
            }
            else if (EnemySpeed > PlayerSpeed)
            {
                print("enemys turn");
                arenaui.EnemyInitiativeArrow.enabled = true;
                yield return new WaitForSeconds(waitForSecs);
                turnchanger.SwitchTurn(eTurn.EnemyFirst);
            }
            else
            {
                print("players turn");
                arenaui.PlayerInitiativeArrow.enabled = true;
                yield return new WaitForSeconds(waitForSecs);
                turnchanger.SwitchTurn(eTurn.PlayerFirst);
            }
        }

        //IEnumerator ShowSpeedandTurn(float waitTime)
        //{
        //    print("waiting for " + waitTime + " s to show INI");
        //    yield return new WaitForSeconds(waitTime);
        //}
    }
}
