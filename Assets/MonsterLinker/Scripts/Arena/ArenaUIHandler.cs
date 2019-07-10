using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaUIHandler : MonoBehaviour
{
    [Tooltip("Temporary image to show Initiative")]
    public Image PlayerInitiativeArrow;
    public Image EnemyInitiativeArrow;

    [Header("Drag n Drop")]
    public GameObject FALoadout;
    public GameObject InputPanel;
    public GameObject PlayerInputBar;
    public GameObject EnemyInputBar;
    public GameObject StatusBars;
    public GameObject InitiativeCheck;
    public GameObject QTEPanel;
    public GameObject AttackQTE;
    public Button ConfirmBAsButton;
    public GameObject ResultPanel;

    [Header("Health and Rage")]
    public Text PlayerHPtxt;
    public Text PlayerRPtxt;
    public Text EnemyHPtxt;
    public Text EnemyRPtxt;

    [Header("Buttons for BA Input")]
    public Button HeavyAttack;
    public Button NormalAttack;
    public Button LightAttack;

    [Header("Buttons for Result Screen")]
    public GameObject RetryButton;
    public GameObject LoadoutButton;
    public GameObject NextButton;
    public Text ResultText;

    [HideInInspector]
    public AttackSlot[] playerSlots;
    [HideInInspector]
    public AttackSlot[] enemySlots;
    [HideInInspector]
    public InputBarHandler inputbarhandler;

    public bool hidden;

    public void Update()
    {
        switch (GameStateSwitch.Instance.GameState)
        {
            case eGameState.PlayerInput:
                CheckForPlayerBAInput();
                break;
            default:
                break;
        }
    }

    public void SetPlayerHPandRP(int HP, int RP)
    {
        PlayerHPtxt.text = ""+HP;
        PlayerRPtxt.text = ""+RP;
    }

    public void SetEnemyHPandRP(int HP, int RP)
    {
        EnemyHPtxt.text = "" + HP;
        EnemyRPtxt.text = "" + RP;
    }

    public void CheckForPlayerBAInput()
    {
        if (inputbarhandler.PlayerAttackInput.Count < inputbarhandler.maxBaseAttackInputSlots)
        {
            HeavyAttack.enabled = true;
            NormalAttack.enabled = true;
            LightAttack.enabled = true;

            if (DPadButtons.Down)
            {
                LightAttack.animator.SetTrigger("Highlighted");
                LightAttack.onClick.Invoke();
            }
            else
            {
                LightAttack.animator.SetTrigger("Normal");
            }

            if (DPadButtons.Right)
            {
                NormalAttack.animator.SetTrigger("Highlighted");
                NormalAttack.onClick.Invoke();
            }
            else
            {
                NormalAttack.animator.SetTrigger("Normal");
            }

            if (DPadButtons.Up)
            {
                HeavyAttack.animator.SetTrigger("Highlighted");
                HeavyAttack.onClick.Invoke();
            }
            else
            {
                HeavyAttack.animator.SetTrigger("Normal");
            }
        }
        else
        {
            HeavyAttack.enabled = false;
            NormalAttack.enabled = false;
            LightAttack.enabled = false;
        }

    }

    public void GetAttackSlots()
    {
        print("getting attack slots");
        playerSlots = PlayerInputBar.GetComponentsInChildren<AttackSlot>();
        enemySlots = EnemyInputBar.GetComponentsInChildren<AttackSlot>();
    }

    public void SetConfirmButtonStatus(bool enabled)
    {
        enabled = ConfirmBAsButton.enabled;
    }

    //TODO Show/Hide FA Info and Input Buttons
    public void ShowHideInputInfo()
    {
        //TODO: animate FA info and input buttons
        if (hidden)
        {
            //FAInfoAndInputButtons.SetActive(true);
        }
        else
        {
            //FAInfoAndInputButtons.SetActive(false);
        }
    }

    public void SlideInputBarIn(GameObject inputbar)
    {
        //TODO: animate inputbars
        inputbar.SetActive(true);
    }

    public void SlideInputBarOut(GameObject inputbar)
    {
        inputbar.SetActive(false);
    }

    //player input shown in ui
    public void UpdatePlayerInput(List<BaseAttack> attacks)
    {
        //playerSlots[position].AddCombo(attack);

        print("updating icon slots");

        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (i < attacks.Count)
            {
                playerSlots[i].AddCombo(attacks[i]);
            }
            else
            {
                playerSlots[i].ClearSlot();
            }
        }
    }

    //player input shown in ui
    public void UpdateEnemyInput(List<BaseAttack> attacks)
    {
        //playerSlots[position].AddCombo(attack);

        print("updating icon slots");

        for (int i = 0; i < enemySlots.Length; i++)
        {
            if (i < attacks.Count)
            {
                enemySlots[i].AddCombo(attacks[i]);
            }
            else
            {
                enemySlots[i].ClearSlot();
            }
        }
    }

    public void VisializeFAs(List<int> Positions, Color thiscolor)
    {
        foreach (int i in Positions)
        {
            AttackSlot slot = playerSlots[i].GetComponentInChildren<AttackSlot>();
            slot.icon.color = thiscolor;
        }
    }

    public void ResetBAColours(Color thiscolor)
    {
        foreach (AttackSlot slot in playerSlots)
        {
            slot.icon.color = thiscolor;
        }
    }
}
