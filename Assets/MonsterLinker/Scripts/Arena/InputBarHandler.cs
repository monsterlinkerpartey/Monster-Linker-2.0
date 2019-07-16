using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBarHandler : MonoBehaviour
{
    //List of base attacks the player has chosen in via button press
    public List<BaseAttack> PlayerAttackInput = new List<BaseAttack>();
    public int maxBaseAttackInputSlots;
    public FeralArtCheck feralartcheck;
    public InitiativeCheck initiativecheck;
    public ArenaUIHandler arenaui;
    
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
        }

        if (PlayerAttackInput.Count >= maxBaseAttackInputSlots)
        {
            print("input bar full: "+ PlayerAttackInput[0].name+    
                                ", "+ PlayerAttackInput[1].name +
                                ", " + PlayerAttackInput[2].name +
                                ", " + PlayerAttackInput[3].name +
                                ", " + PlayerAttackInput[4].name);    
            
            //arenaui.ConfirmBAsButton.enabled = true;
            //TODO disable BA input and enable confirm button
            arenaui.SetConfirmButtonStatus(true);
            arenaui.SetInputButtonsStatus(false);
            feralartcheck.CheckForChain();
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
        arenaui.ResetBAColours(Color.white);        
        print("resetting Input bar");

        arenaui.SetConfirmButtonStatus(false);
        //arenaui.ConfirmBAsButton.enabled = false;
        arenaui.SetInputButtonsStatus(true);

        if (feralartcheck.superFAused)
            feralartcheck.superFAused = false;
    }

    public void ConfirmInput()
    {
        if (PlayerAttackInput.Count == maxBaseAttackInputSlots)
        {
            if (feralartcheck.superFAused)
            {
                feralartcheck.LoadedFeralArts.RemoveAt(3);
                GameStateSwitch.Instance.fainfowindow.SI.SetActive(false);
            }

            initiativecheck.curPlayerInput = PlayerAttackInput;
            GameStateSwitch.Instance.SwitchState(eGameState.InitiativeCheck);
        }
        else
        {
            arenaui.SetConfirmButtonStatus(false);
            //arenaui.ConfirmBAsButton.enabled = false;
            print("BA input not full, cannot start fight");
        }
    }   

}

