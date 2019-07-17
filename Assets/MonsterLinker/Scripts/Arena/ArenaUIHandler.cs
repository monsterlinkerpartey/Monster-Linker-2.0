using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaUIHandler : MonoBehaviour
{
    [Header("Initiative UI")]
    public GameObject IniArrow;
    public Text EnemySpeed;
    public Text PlayerSpeed;

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
    public GameObject DmgCounter;

    public List<Button> BAInputButtons = new List<Button>(3);

    [Header("Health and Rage")]
    public Text PlayerHPtxt;
    public Text PlayerRPtxt;
    public Text EnemyHPtxt;
    public Text EnemyRPtxt;

    public Text EnemyName;
    public Text PlayerName;

    public Text PlayerDmgTaken;
    public Text EnemyDmgTaken;

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
    //public AttackSlot[] playerSlots;
    public List<AttackSlot> playerSlots;
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
                if (inputbarhandler.PlayerAttackInput.Count < inputbarhandler.maxBaseAttackInputSlots)
                    CheckForPlayerBAInput();
                    CheckForPause();
                break;
            default:
                break;
        }
    }

    public IEnumerator ShowDmgCounters(int DmgTaken)
    {
        if (GameStateSwitch.Instance.GameState == eGameState.QTEAttack)
        {
            EnemyDmgTaken.text = "" + DmgTaken;
            PlayerDmgTaken.text = "";
        }
        else if (GameStateSwitch.Instance.GameState == eGameState.QTEBlock)
        {
            PlayerDmgTaken.text = "" + DmgTaken;
            EnemyDmgTaken.text = "";
        }

        DmgCounter.SetActive(true);
        yield return new WaitForSeconds(1f);
        DmgCounter.SetActive(false);
    }

    public void SetSpeedValues(int eSpeed, int pSpeed)
    {
        EnemySpeed.text = ""+eSpeed;
        PlayerSpeed.text = ""+pSpeed;
    }

    public void SetIniArrow(string who)
    {
        //float z = rotation;
        //Vector3 newPos = new Vector3(0, 0, z);
        //IniArrow.transform.Rotate(newPos);

        IniArrow.GetComponentInChildren<Animator>().Play("play"+who);
    }

    public void SetPlayerHPandRP(int HP, int RP)
    {
        if (HP < 0)
        PlayerHPtxt.text = "0";
        else
        PlayerHPtxt.text = ""+HP;

        PlayerRPtxt.text = ""+RP;
    }

    public void SetEnemyHPandRP(int HP, int RP)
    {
        if (HP < 0)
            EnemyHPtxt.text = "0";
        else
            EnemyHPtxt.text = "" + HP;
        
        EnemyRPtxt.text = "" + RP;
    }

    public void CheckForPause()
    {
        if (Input.GetButtonDown("start"))
        {
            //TODO: write pause screen stuff
        }
        if (Input.GetButtonDown("select"))
        {

        }
    }

    public void CheckForPlayerBAInput()
    {             
        if (DPadButtons.Down || Input.GetKeyDown(KeyCode.DownArrow))
        {
            LightAttack.animator.SetTrigger("Pressed");
            LightAttack.onClick.Invoke();
        }

        if (DPadButtons.Right || Input.GetKeyDown(KeyCode.RightArrow))
        {
            NormalAttack.animator.SetTrigger("Pressed");
            NormalAttack.onClick.Invoke();
        }

        if (DPadButtons.Up || Input.GetKeyDown(KeyCode.UpArrow))
        {
            HeavyAttack.animator.SetTrigger("Pressed");
            HeavyAttack.onClick.Invoke();
        }
    }

    public void GetAttackSlots()
    {
        print("getting attack slots");
        PlayerInputBar.GetComponentsInChildren<AttackSlot>(playerSlots);
        enemySlots = EnemyInputBar.GetComponentsInChildren<AttackSlot>();
    }

    public void SetConfirmButtonStatus(bool enabled)
    {
        //TODO: disable button anims somehow
        //ConfirmBAsButton.GetComponent<ButtonControl>().Bdisabled = enabled;
    }

    public void SetInputButtonsStatus(bool enabled)
    {
        print("input buttons enabled: " + enabled);
        HeavyAttack.enabled = enabled;
        LightAttack.enabled = enabled;
        NormalAttack.enabled = enabled;        
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

        for (int i = 0; i < playerSlots.Count; i++)
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
