using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ScriptableObject
{
    [Header("For the Blacklist menu")]
    [Tooltip("Sprite used in Blacklist")]
    public Sprite MonsterPic;
    [Tooltip("Sprite used in Blacklist")]
    public Sprite LinkerPic;
    [Tooltip("Name of the enemy monster")]
    public string MonsterName;

    [Header("In-fight values")]
    [Tooltip("Enemys max HP")]
    public int MaxHitPoints;
    [Tooltip("Modifier for the dmg; Multipilier, e.g. 1.5 ")]
    public int DmgModifier;

    [Header("FAs enemy can use")]
    [Tooltip("FAs the enemy can use")]
    public List<FeralArt> FAs;
}
