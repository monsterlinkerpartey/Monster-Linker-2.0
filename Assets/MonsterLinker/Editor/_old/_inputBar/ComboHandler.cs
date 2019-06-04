using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates attacks entered by Player and Player Output UI
/// </summary>
public class ComboHandler : MonoBehaviour
{
    public AttackIcon emptyAttack;
    public Color normal;

    public delegate void OnAttackEntered();
    public static OnAttackEntered onAttackEnteredCallback;
    public static AttackSlot[] playerSlots;
    public static List<AttackIcon> attacks = new List<AttackIcon>(); 

    private void OnEnable()
    {
        normal = ArenaManager.Instance.white;
        playerSlots = GetComponentsInChildren<AttackSlot>();
        onAttackEnteredCallback += UIUpdate;

        if (ArenaManager.Instance.curAttackRound == 0)
            ResetFrameColours(normal);

        Reset();
    }

    void UIUpdate()
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (i < attacks.Count)
            {
                GlobalManager.Instance.News.text += ("adding combo icon in UI\n");
                playerSlots[i].AddCombo(attacks[i]);
            }
            else
            {
                playerSlots[i].ClearSlot();
            }
        }
    }

    public static bool Add(AttackIcon attack)
    {
        if (attacks.Count > ArenaManager.Instance.MaxPlayerSlots)
        {
            return false;
        }

        GlobalManager.Instance.News.text += ("adding " + attack.name +"\n");
        attacks.Add(attack);

        if (onAttackEnteredCallback != null)
        {
            onAttackEnteredCallback.Invoke();
            //print("button added: " + combo.pressedButton);
        }

        if (attacks.Count == ArenaManager.Instance.MaxPlayerSlots)
        {
            ArenaManager.Instance.AttackConfirmButton.SetActive(true);
            GlobalManager.Instance.News.text += ("combo panel full \n");
            GlobalManager.Instance.News.text = ("AttackListe, 1. " + attacks[0].name + ", 2. " + attacks[1].name + ", 3. " + attacks[2].name+"\n");
        }
        return true;
    }

    public static bool Reset()
    {
        if (attacks.Count == 0)
        {
            return true;
        }
        else
        {
            GlobalManager.Instance.News.text = ("resetting list\n");            
            ArenaManager.Instance.disableAttackInput = false;     
            attacks.Clear();
            ArenaManager.Instance.AttackConfirmButton.SetActive(false);

            //Remove Combo Icons from panel
            if (onAttackEnteredCallback != null)
                onAttackEnteredCallback.Invoke();
            return true;
        }
    }

    void ResetFrameColours(Color A)
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
            playerSlots[i].icon.color = A;
        }
    }
}
