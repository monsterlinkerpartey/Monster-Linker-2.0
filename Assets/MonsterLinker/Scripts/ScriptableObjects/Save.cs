using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All important vars
/// </summary>
[CreateAssetMenu(fileName = "new Profile", menuName = "Other/SaveProfile")]
public class Save : ScriptableObject
{
    [Tooltip("Has this save been used?")]
    public bool Used;
    public int maxBaseAttackInputSlots = 5;
    public List<FeralArt> FALoadout;
    public int lowestFAcost;
    public string LinkerName;
    public string MonsterName;
    public int MaxHitPoints;
    [Tooltip("Which arena has the player reached?")]
    public int Arena = 1;

    public void SetCheapestFAcost()
    {
        Debug.Log("setting lowest FA cost in profile");
        lowestFAcost = 100;

        for (int i = 0; i < FALoadout.Count; i++)
        {
            if (lowestFAcost > FALoadout[i].RPCost)
            {
                lowestFAcost = FALoadout[i].RPCost;
            }
            else
            {
                continue;
            }
        }
    }

    public bool Write()
    {
        if (Used)
            return false;

        //write stuff in from the players choice
        Used = true;
        return true;

    }

    public void Reset()
    {
        //reset all values for the player to overwrite?
    }
}
