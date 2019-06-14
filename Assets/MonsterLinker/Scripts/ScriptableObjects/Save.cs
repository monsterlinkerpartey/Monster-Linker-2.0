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
    public string LinkerName;
    public string MonsterName;

    public bool Write()
    {
        if (Used)
            return false;

        //write stuff in from the players choice
        Used = true;
        return true;

    }
}
