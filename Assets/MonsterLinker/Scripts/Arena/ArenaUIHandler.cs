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
    public GameObject BlackListPanel;
    public GameObject InputPanel;
    public GameObject PlayerInputBar;
    public GameObject EnemyInputBar;
    public GameObject InitiativeCheck;
    public GameObject AttackQTE;
    public Button ConfirmBAsButton;
    public GameObject ResultPanel;

    [HideInInspector]
    public AttackSlot[] playerSlots;
    [HideInInspector]
    public AttackSlot[] enemySlots;

    public bool hidden;

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
}
