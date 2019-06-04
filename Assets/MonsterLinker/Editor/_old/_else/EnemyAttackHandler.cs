using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates attacks entered by Player and Player Output UI
/// </summary>
public class EnemyAttackHandler : MonoBehaviour
{
    public AttackIcon emptyAttack;
    public AttackIcon HighAttack;
    public AttackIcon MidAttack;
    public AttackIcon LowAttack;

    public Color normal;
    //Basically determines if the round is even or not
    public int attackState;

    bool inputting = false;

    public static AttackSlot[] enemySlots;
    public static List<AttackIcon> enemyAttacks = new List<AttackIcon>();

    private void OnEnable()
    {
        normal = ArenaManager.Instance.white;
        enemySlots = GetComponentsInChildren<AttackSlot>();

        //if (ArenaManager.Instance.curAttackRound == 0)
        //{
        //    attackState = 0;

        //    ////if attackround is an even number
        //    //if (ArenaManager.Instance.curAttackRound % 2 == 0)
        //    //{
        //    //    attackState = 0;
        //    //}
        //    ////if it is not
        //    //else
        //    //{
        //    //    attackState = 1;
        //    //}
        //}
        //else
        //{
        //    attackState = 1;
        //}


        if (ArenaManager.Instance.curAttackRound == 0)
            ResetFrameColours(normal);

        if (ArenaManager.Instance.ArenaState == eArena.EnemyInput)
        {
            switch (ArenaManager.Instance.curAttackRound)
            {
                case 0:
                    StartCoroutine(EnemyInput(HighAttack, MidAttack, HighAttack));
                    break;
                case 1:
                    StartCoroutine(EnemyInput(HighAttack, HighAttack, LowAttack));
                    break;
                default:
                    StartCoroutine(EnemyInput(HighAttack, MidAttack, HighAttack));
                    break;
            }
        }
    }

    private void OnDisable()
    {
        inputting = false;
    }

    void ResetAttacks()
    {
        enemyAttacks.Clear();
        UpdateUI();
    }

    IEnumerator EnemyInput(AttackIcon A, AttackIcon B, AttackIcon C)
    {
        if (inputting)
            yield break;

        inputting = true;
        ResetAttacks();
        yield return new WaitForSeconds(0.5f);
        enemyAttacks.Add(A);
        UpdateUI();
        yield return new WaitForSeconds(0.5f);
        enemyAttacks.Add(B);
        UpdateUI();
        yield return new WaitForSeconds(0.5f);
        enemyAttacks.Add(C);
        UpdateUI();
        yield return new WaitForSeconds(0.5f);
        ArenaManager.Instance.ArenaState = eArena.AdvantageCheck;
    }

    void UpdateUI()
    {
        for (int i = 0; i < enemySlots.Length; i++)
        {
            if (i < enemyAttacks.Count)
            {
                print("adding combo icon in UI\n");
                enemySlots[i].AddCombo(enemyAttacks[i]);
            }
            else
            {
                enemySlots[i].ClearSlot();
            }
        }
    }

    void ResetFrameColours(Color A)
    {
        for (int i = 0; i < enemySlots.Length; i++)
        {
            enemySlots[i].icon.color = A;
        }
    }
}
