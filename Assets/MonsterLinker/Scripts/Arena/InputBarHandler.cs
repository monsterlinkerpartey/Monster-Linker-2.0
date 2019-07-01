using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBarHandler : MonoBehaviour
{
    //List of base attacks the player has chosen in via button press
    public List<BaseAttack> PlayerAttackInput = new List<BaseAttack>();
    public int maxBaseAttackInputSlots = 5; //TODO abfragen vom profil des spielers?
    public FeralArtCheck feralartcheck;
    public InitiativeCheck initiativecheck;
    public ArenaUIHandler arenaui;

    public void AddInputSlot(int i)
    {
        maxBaseAttackInputSlots += i;
    }

    //call this via button to add the attack
    public void Add(BaseAttack baseAttack)
    {
        if (PlayerAttackInput.Count < maxBaseAttackInputSlots)
        {
            //print("adding " + baseAttack.name);
            PlayerAttackInput.Add(baseAttack);
            arenaui.UpdatePlayerInput(PlayerAttackInput);
            //HACK save BAs in the feralart output
            feralartcheck.SaveBAlist(baseAttack);
            arenaui.SetConfirmButtonStatus(true);

            //if (PlayerAttackInput.Count == maxBaseAttackInputSlots)
        }

        if (PlayerAttackInput.Count >= maxBaseAttackInputSlots)
        {
            print("input bar full: "+ PlayerAttackInput[0].name+", "+ PlayerAttackInput[1].name +", " + PlayerAttackInput[2].name +", " + PlayerAttackInput[3].name +", " + PlayerAttackInput[4].name);
            feralartcheck.CheckForChain();
            //TODO disable BA input
        }
    }

    //call this via button to reset input bar list
    public void Reset()
    {
        if (PlayerAttackInput.Count == 0)
            return;

        PlayerAttackInput.Clear();
        feralartcheck.ResetLists();
        arenaui.UpdatePlayerInput(PlayerAttackInput);
        arenaui.SetConfirmButtonStatus(false);
        arenaui.ResetBAColours(Color.white);        
        print("resetting Input bar");
        //TODO enable BA input
    }

    public void ConfirmInput()
    {
        initiativecheck.curPlayerInput = PlayerAttackInput;
        GameStateSwitch.Instance.SwitchState(eGameState.InitiativeCheck);
        //TODO
    }   

}

