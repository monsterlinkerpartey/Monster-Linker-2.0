using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVars
{
    [Tooltip("Which arena the player has reached")]
    public static int Arena = 1;
    public static int AttackRound;
    public static int PlayerMaxHP = 1000;
    public static int EnemyMaxHP = 1000;
    public static bool QTEfailed;
}
