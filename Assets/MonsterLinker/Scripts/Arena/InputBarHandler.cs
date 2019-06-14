using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBarHandler : MonoBehaviour
{
    //List of base attacks the player has chosen in via button press
    public List<BaseAttack> PlayerAttackInput = new List<BaseAttack>();
    public int maxBaseAttackInputSlots = 5; //TODO abfragen vom profil des spielers?
    public FeralArtCheck feralartcheck;
    public ArenaUIHandler arenaui;

    void Start()
    {
        //feralartcheck = GetComponentInChildren<FeralArtCheck>();
    }

    public void AddInputSlot(int i)
    {
        maxBaseAttackInputSlots += i;
    }

    //call this via button to add the attack
    public void Add(BaseAttack baseAttack)
    {
        if (PlayerAttackInput.Count < maxBaseAttackInputSlots)
        {
            print("adding " + baseAttack.name);
            PlayerAttackInput.Add(baseAttack);
            //HACK save BAs in the feralart output
            feralartcheck.SaveOutput(baseAttack);
            arenaui.SetConfirmButtonStatus(true);
        }

        if (PlayerAttackInput.Count >= maxBaseAttackInputSlots)
        {
            print("input bar full: "+ PlayerAttackInput[0].name+", "+ PlayerAttackInput[1].name +", " + PlayerAttackInput[2].name +", " + PlayerAttackInput[3].name +", " + PlayerAttackInput[4].name);
            //feralArtCheck.CheckForFeralArt(PlayerAttackInput);
        }
    }

    //call this via button to reset input bar list
    public void Reset()
    {
        if (PlayerAttackInput.Count == 0)
            return;

        PlayerAttackInput.Clear();
        feralartcheck.ResetAll();
        arenaui.SetConfirmButtonStatus(false);
        print("resetting Input bar");
    }     
    
    public void ConfirmInput()
    {
        GameStateSwitch.Instance.SwitchState(eGameState.InitiativeCheck);
        //TODO
    }
    

    //TODO make own script to handle UI updates
    //void UIUpdate()
    //{
    //    for (int i = 0; i < playerSlots.Length; i++)
    //    {
    //        if (i < attacks.Count)
    //        {
    //            GlobalManager.Instance.News.text += ("adding combo icon in UI\n");
    //            playerSlots[i].AddCombo(attacks[i]);
    //        }
    //        else
    //        {
    //            playerSlots[i].ClearSlot();
    //        }
    //    }
    //}
    
}

