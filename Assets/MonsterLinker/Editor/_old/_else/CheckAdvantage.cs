using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Checks if the player is in advantage or disadvantage
/// </summary>
public class CheckAdvantage : MonoBehaviour
{
    public Color Normal;
    public Color Done;
    public Color Advantage;
    public Color Disadvantage;
    public Color Stalemate;

    public Text AdvantageCall;

    public float TextZoomInTimer = 2f;

    public AttackIcon enemyAttack;
    public AttackIcon playerAttack;
    public AttackSlot enemySlot;
    public AttackSlot playerSlot;

    public int curAttackRound;
    public int maxAttackRounds;

    void OnEnable()
    {
        Normal = ArenaManager.Instance.white;
        Done = ArenaManager.Instance.grey;
        Advantage = ArenaManager.Instance.green;
        Disadvantage = ArenaManager.Instance.red;
        Stalemate = ArenaManager.Instance.yellow;
        
        maxAttackRounds = ArenaManager.Instance.maxAttackRounds;
        curAttackRound = ArenaManager.Instance.curAttackRound;
        maxAttackRounds = ArenaManager.Instance.maxAttackRounds;        

        //Sets colours for the previously used attacks to "used"
        if (curAttackRound != 0)
            SetColours(Done, Done);

        HowManyRounds();
        GetCurAttacks();
    }

    void OnDisable()
    {

    }

    void HowManyRounds()
    {
        if (ArenaManager.Instance.MaxPlayerSlots > ArenaManager.Instance.MaxEnemySlots)
        {
            ArenaManager.Instance.maxAttackRounds = ArenaManager.Instance.MaxPlayerSlots;
        }
        else
        {
            ArenaManager.Instance.maxAttackRounds = ArenaManager.Instance.MaxEnemySlots;
        }
    }

    void GetCurAttacks()
    {
        enemyAttack = EnemyAttackHandler.enemyAttacks[curAttackRound];
        playerAttack = ComboHandler.attacks[curAttackRound];

        enemySlot = EnemyAttackHandler.enemySlots[curAttackRound];
        playerSlot = ComboHandler.playerSlots[curAttackRound];

        GlobalManager.Instance.News.text += ("enemy attack is: " + enemyAttack.name + "\n");
        GlobalManager.Instance.News.text += ("player attack is: " + playerAttack.name + "\n");

        CompareCurAttacks(playerAttack, enemyAttack);
    }

    public void CompareCurAttacks(AttackIcon player, AttackIcon enemy)
    {
        string p = player.attackType.ToLower();
        string e = enemy.attackType.ToLower();

        string textline = "Nothing set";
        Vector3 zero = new Vector3(0, 0, 0);

        //Player at advantage
        if ((p == "high" && e == "mid") || (p == "mid" && e == "low") || (p == "low" && e == "high"))
        {
            textline = "Player has the advantage!";
            ArenaManager.Instance.AdvantageCheck = eAdvantage.Advantage;
            SetColours(Advantage, Disadvantage);
        }
        //Stalemate 
        else if ((p == "high" && e == "high") || (p == "mid" && e == "mid") || (p == "low" && e == "low"))
        {
            textline = "Stalemate!";
            ArenaManager.Instance.AdvantageCheck = eAdvantage.Stalemate;
            SetColours(Stalemate, Stalemate);
        }
        //Player at disadvantage
        else if ((p == "mid" && e == "high") || (p == "low" && e == "mid") || (p == "high" && e == "low"))
        {
            textline = "Player at disadvantage!";
            ArenaManager.Instance.AdvantageCheck = eAdvantage.Disadvantage;
            SetColours(Disadvantage, Advantage);
        }
        else if (p == "0")
        {
            textline = "Player at disadvantage!";
            ArenaManager.Instance.AdvantageCheck = eAdvantage.Disadvantage;
            SetColours(Disadvantage, Advantage);
        }
        else if (e == "0")
        {
            textline = "Player has the advantage!";
            ArenaManager.Instance.AdvantageCheck = eAdvantage.Advantage;
            SetColours(Advantage, Disadvantage);

        }
        SetTurn();
        StartCoroutine(WaitaSec(1f));
    }
    

    void SetColours(Color A, Color B)
    {
        if (playerSlot != null && enemySlot != null)
        {
            playerSlot.icon.color = A;
            enemySlot.icon.color = B;
        }
        else
        {
            enemySlot = EnemyAttackHandler.enemySlots[curAttackRound - 1];
            playerSlot = ComboHandler.playerSlots[curAttackRound - 1];
            playerSlot.icon.color = A;
            enemySlot.icon.color = B;
        }
    }

    //void ResetAllColours(Color A)
    //{
    //    for (int i1 = 0; i1 < ComboHandler.playerSlots.Length; i1++)
    //    {
    //        ComboHandler.playerSlots[i1].frame.color = A;
    //    }

    //    for (int i2 = 0; i2 < EnemyAttackHandler.enemySlots.Length; i2++)
    //    {
    //        EnemyAttackHandler.enemySlots[i2].frame.color = A;
    //    }
    //}

    void SetTurn()
    {
        switch (ArenaManager.Instance.AdvantageCheck)
        {
            case eAdvantage.Advantage:
                ArenaManager.Instance.Turn = eTurn.Player;
                break;

            case eAdvantage.Disadvantage:
                ArenaManager.Instance.Turn = eTurn.Enemy;
                break;

            case eAdvantage.Stalemate:
                if (ArenaManager.Instance.Turn == eTurn.Nobody)
                {
                    int ran = Random.Range(1, 2);
                    {
                        if (ran == 1)
                            ArenaManager.Instance.Turn = eTurn.Player;
                        else if (ran == 2)
                            ArenaManager.Instance.Turn = eTurn.Enemy;
                    }
                }
                else if (ArenaManager.Instance.Turn == eTurn.Player)
                    ArenaManager.Instance.Turn = eTurn.Enemy;
                else if (ArenaManager.Instance.Turn == eTurn.Enemy)
                    ArenaManager.Instance.Turn = eTurn.Player;
                break;
        }

        if (ArenaManager.Instance.Turn == eTurn.Enemy)
            ArenaManager.Instance.curAttack = enemyAttack;
        if (ArenaManager.Instance.Turn == eTurn.Player)
            ArenaManager.Instance.curAttack = playerAttack;
    }

    //IEnumerator FadeInText(Text textfield, string textline, Color textColor, float t)
    //{
    //    textfield.text = (textline);
    //    print(textline);

    //    textfield.color = new Color(textColor.r, textColor.g, textColor.b, 0);
    //    while (textfield.color.a < 1.0f)
    //    {
    //        textfield.color = new Color(textfield.color.r, textfield.color.g, textfield.color.b, textfield.color.a + (Time.deltaTime / t));
    //        yield return null;
    //    }
    //}

    IEnumerator ZoomInText(Text textfield, string textline, Vector3 newScale, float Endsize, float t)
    {
        if (ArenaManager.Instance.AdvantageCheck == eAdvantage.Advantage)
            textfield.color = Advantage;
        else if (ArenaManager.Instance.AdvantageCheck == eAdvantage.Disadvantage)
            textfield.color = Disadvantage;
        else if (ArenaManager.Instance.AdvantageCheck == eAdvantage.Stalemate)
            textfield.color = Stalemate;

        textfield.text = (textline);
        textfield.color = new Color(textfield.color.r, textfield.color.g, textfield.color.b, 255f);

        textfield.transform.localScale = newScale;
        while (textfield.transform.localScale.x < Endsize)
        {
            textfield.transform.localScale = new Vector3(
                 textfield.transform.localScale.x + (Time.deltaTime / t),
                 textfield.transform.localScale.y + (Time.deltaTime / t),
                 textfield.transform.localScale.z + (Time.deltaTime / t));
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        GlobalManager.Instance.News.text += (textline + "\n");
        GlobalManager.Instance.News.text += ("Advantagestatus: " + ArenaManager.Instance.AdvantageCheck + "\n");
        GlobalManager.Instance.News.text += ("QTE triggered \n");
        ArenaManager.Instance.ArenaState = eArena.QTEAttack;
    }

    IEnumerator WaitaSec(float t)
    {
        yield return new WaitForSeconds(t);
        ArenaManager.Instance.ArenaState = eArena.QTEAttack;
    }
}
