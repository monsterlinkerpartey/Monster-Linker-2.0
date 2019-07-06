using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : ScriptableObject
{
    [Header("Values for GD iterations")]
    [Tooltip("Damage the attack makes incl. Multiplier")]
    public float DMG;
    [Tooltip("Ragepoints spend by using this attack, FA ONLY")]
    public int RPCost;
    [Tooltip("Ragepoints gained by using this attack")]
    public int RPGain;
    [Tooltip("Health recovered by using this attack: % of max HP value")]
    public float HPGain;
    [Tooltip("Speed of the attack")]
    public int Speed;

    [Header("Do not touch unless told by coder")]
    public string AnimationName;
    public eAttackType AttackType;
}
