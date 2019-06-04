using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles how much damage is made and who gets it
/// </summary>
public class DamageHandler : MonoBehaviour
{
    public Text PlayerDmg;
    public Text EnemyDmg;
    public Color thisColor;
    //public Color playerTurn;
    //public Color enemyTurn;
    
    [SerializeField] int BaseDmg;
    //[SerializeField] int EndDmg;
    [SerializeField] float DmgMultiplier;
    [SerializeField] float BlockDivider;

    [SerializeField] bool finishing = false;

    [SerializeField] string ThisGuy;


    void OnEnable()
    {
        PlayerDmg.color = new Color(PlayerDmg.color.r, PlayerDmg.color.g, PlayerDmg.color.b, 0f);
        EnemyDmg.color = new Color(EnemyDmg.color.r, EnemyDmg.color.g, EnemyDmg.color.b, 0f);
        BaseDmg = ArenaManager.Instance.BaseDmg;
        CalculateMultiplier();
        CheckTurn();
    }

    private void OnDisable()
    {
        finishing = false;
    }       

    public IEnumerator DoTheThing(float t, Text DamageText, Color A)
    {
        if (finishing)
            yield break;

        finishing = true;
        GlobalManager.Instance.News.text += (ThisGuy + " takes " + ArenaManager.Instance.EndDmg + " points damage!\n");

        if (ArenaManager.Instance.Turn == eTurn.Enemy)
        {
            StartCoroutine(ArenaManager.Instance.camShake.Shake(0.2f, 0.3f));
            ArenaManager.Instance.PlayerHealth -= ArenaManager.Instance.EndDmg;
            ArenaManager.Instance.PlayerRagePoints += 2;
            ArenaManager.Instance.EnemyRagePoints += 6;
        }
        if (ArenaManager.Instance.Turn == eTurn.Player)
        {
            StartCoroutine(ArenaManager.Instance.camShake.Shake(0.2f, 0.3f));
            ArenaManager.Instance.EnemyHealth -= ArenaManager.Instance.EndDmg;

            if (ArenaManager.Instance.curAttackRound == 2 && CheckForCombo.Feral)
            {
                ArenaManager.Instance.PlayerRagePoints -= ArenaManager.Instance.curAttack.RPCost;
                print("reducing player RP by " + ArenaManager.Instance.curAttack.RPCost);
            }
            else
            {
                ArenaManager.Instance.PlayerRagePoints += 6;
            }
            ArenaManager.Instance.EnemyRagePoints += 2;
        }

        //TODO start lerp of hp
        //StartCoroutine(ArenaManager.Instance.TickHP());
        //ArenaManager.Instance.timeStartedLerping = Time.deltaTime;
        //ArenaManager.Instance.HPlerping = true;
        //ArenaManager.Instance.HPTick();

        //StartCoroutine(ArenaManager.Instance.UpdateHealth());
        //StartCoroutine(ArenaManager.Instance.UpdateRage());

        DamageText.text = ("-"+ ArenaManager.Instance.EndDmg);
        DamageText.color = A;
        Vector3 newScale = new Vector3(0, 0, 0);
        int Endsize = 1;

        DamageText.transform.localScale = newScale;
        while (DamageText.transform.localScale.x < Endsize)
        {
            DamageText.transform.localScale = new Vector3(
                 DamageText.transform.localScale.x + (Time.deltaTime / t),
                 DamageText.transform.localScale.y + (Time.deltaTime / t),
                 DamageText.transform.localScale.z + (Time.deltaTime / t));
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        while (DamageText.color.a > 0f)
        {
            DamageText.color = new Color(DamageText.color.r, DamageText.color.g, DamageText.color.b, DamageText.color.a - (Time.deltaTime / 1));
            yield return null;
        }

        yield return new WaitForSeconds(0.75f); 

        if (ArenaManager.Instance.enemyHasAttacked && ArenaManager.Instance.playerHasAttacked)
        {
            GlobalManager.Instance.News.text += ("Next round triggered \n");
            ArenaManager.Instance.curAttackRound += 1;
            ArenaManager.Instance.ArenaState = eArena.NextAttack;
        }
        else if (ArenaManager.Instance.enemyHasAttacked)
        {
            GlobalManager.Instance.News.text += ("Player turn triggered \n");
            ArenaManager.Instance.Turn = eTurn.Player;
            ArenaManager.Instance.ArenaState = eArena.QTEAttack;
        }
        else if (ArenaManager.Instance.playerHasAttacked)
        {
            GlobalManager.Instance.News.text += ("Enemy turn triggered \n");
            ArenaManager.Instance.Turn = eTurn.Enemy;
            ArenaManager.Instance.ArenaState = eArena.QTEAttack;
        }
        else
        {
            GlobalManager.Instance.News.text += ("Halp, I dunno whose turn it was \n");
        }
    }

    void CheckTurn()
    {
        if (ArenaManager.Instance.Turn == eTurn.Enemy)
        {
            ThisGuy = "Player";
            StartCoroutine(DoTheThing(0.3f, PlayerDmg,thisColor));
        }
        else if (ArenaManager.Instance.Turn == eTurn.Player)
        {
            ThisGuy = "Enemy";
            StartCoroutine(DoTheThing(0.3f, EnemyDmg, thisColor));
        }
        else
        {
            ThisGuy = "I dunno who";
            StartCoroutine(DoTheThing(0.3f, PlayerDmg, thisColor));
        }
    }

    void CalculateMultiplier()
    {        
        switch (ArenaManager.Instance.Result)
        {
            case eQTEResult.Fail:
                switch (ArenaManager.Instance.AdvantageCheck)
                {
                    case eAdvantage.Advantage:
                        DmgMultiplier = 1.5f;
                        BlockDivider = 1f;
                        break;
                    case eAdvantage.Disadvantage:
                        DmgMultiplier = 0.75f;
                        BlockDivider = 2f;
                        break;
                    case eAdvantage.Stalemate:
                        DmgMultiplier = 1f;
                        BlockDivider = 1.5f;
                        break;
                }
                break;
            case eQTEResult.Good:
                switch (ArenaManager.Instance.AdvantageCheck)
                {
                    case eAdvantage.Advantage:
                        DmgMultiplier = 2f;
                        BlockDivider = 0.5f;
                        break;
                    case eAdvantage.Disadvantage:
                        DmgMultiplier = 1f;
                        BlockDivider = 1.5f;
                        break;
                    case eAdvantage.Stalemate:
                        DmgMultiplier = 1.5f;
                        BlockDivider = 1f;
                        break;
                }
                break;
            case eQTEResult.Perfect:
                switch (ArenaManager.Instance.AdvantageCheck)
                {
                    case eAdvantage.Advantage:
                        DmgMultiplier = 3f;
                        BlockDivider = 0.11f;
                        break;
                    case eAdvantage.Disadvantage:
                        DmgMultiplier = 1.5f;
                        BlockDivider = 1f;
                        break;
                    case eAdvantage.Stalemate:
                        DmgMultiplier = 2f;
                        BlockDivider = 0.5f;
                        break;
                }
                break;
        }

        if (ArenaManager.Instance.QTEType == eQTEType.Attack)
            ArenaManager.Instance.EndDmg = (int)Mathf.Round(BaseDmg * DmgMultiplier);
        else if (ArenaManager.Instance.QTEType == eQTEType.Block)
            ArenaManager.Instance.EndDmg = (int)Mathf.Round(BaseDmg * BlockDivider);
    }

    void UpdateHealth()
    {
        if (ArenaManager.Instance.Turn == eTurn.Enemy)
        {
            ArenaManager.Instance.PlayerHealth -= ArenaManager.Instance.EndDmg;
        }
        if (ArenaManager.Instance.Turn == eTurn.Enemy)
        {
            ArenaManager.Instance.EnemyHealth -= ArenaManager.Instance.EndDmg;
        }

        if (ArenaManager.Instance.PlayerHealth <= 0)
        {
            //TODO go to result player lose
        }
        if (ArenaManager.Instance.EnemyHealth <= 0)
        {
            //TODO go to result player win
        }
    }
}
