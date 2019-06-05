using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBarHandler : MonoBehaviour
{
    //List of base attacks the player has chosen in via button press
    [SerializeField] public List<BaseAttack> PlayerAttackInput = new List<BaseAttack>();
    public int maxBaseAttackInputSlots = 5;
    [SerializeField] FeralArtCheck feralArtCheck;

    void Start()
    {
        feralArtCheck = GetComponentInChildren<FeralArtCheck>();
    }

    //call this via button to add the attack
    public void Add(BaseAttack baseAttack)
    {
        if (PlayerAttackInput.Count >= maxBaseAttackInputSlots)
        {
            print("input bar full: "+ PlayerAttackInput[0].name+", "+ PlayerAttackInput[1].name +", " + PlayerAttackInput[2].name +", " + PlayerAttackInput[3].name +", " + PlayerAttackInput[4].name);
            return;
        }

        PlayerAttackInput.Add(baseAttack);
        feralArtCheck.CurPlayerInput.Add(baseAttack);
        print("adding " + baseAttack.name);              
        feralArtCheck.CheckForFeralArt();
    }

    //call this via button to reset input bar list
    public void Reset()
    {
        if (PlayerAttackInput.Count == 0)
            return;

        PlayerAttackInput.Clear();
        feralArtCheck.CurPlayerInput.Clear();
        feralArtCheck.ResetStartPosition();
        print("resetting Input bar");
    }            
    
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

