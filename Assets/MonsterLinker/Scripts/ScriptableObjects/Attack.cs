using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : ScriptableObject
{
    [Header("Values for GD iterations")]
    [Tooltip("Damage the attack makes incl. Multiplier")]
    public float DMG;
    [Tooltip("Ragepoints spend by using this attack, FA ONLY")]
    public float RPCost;
    [Tooltip("Ragepoints gained by using this attack")]
    public float RPGain;
    [Tooltip("Health recovered by using this attack: % of max HP value")]
    public float HPGain;
    [Tooltip("Speed of the attack")]
    public int Speed;

    [Tooltip("Time til Startup in the attack animation")]
    public string TimeTilStartup;
}
