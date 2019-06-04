using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tournament", menuName = "Tournament")]
public class Tournament : ScriptableObject
{
    public string TournamentTitle;
    [Tooltip("Number e.g. 02")]
    public string ArenaNo;
    [Tooltip("For the star spawn")]
    public int Difficulty;
    [Tooltip("Drag Enemy here")]
    public Enemy Opponent;
    public bool isUnlocked;
}

