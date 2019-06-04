using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class AttackIcon : ScriptableObject
{
    [Tooltip("Attack Type for comparison etc")]
    public string attackType;
    [Tooltip("Sprite shown in the combo panel")]
    public Sprite comboPanelIcon;
    [Tooltip("Image for the Button")]
    public Sprite buttonSprite;
    [Tooltip("Basedamage the attack makes")]
    public float basedmg;    
    [Tooltip("Time for attack to hit")]
    public float startupTime;
    [Tooltip("RP Cost if this is used as a feral")]
    public int RPCost;
    [Tooltip("Gets set by ComboChecker if this attack is a combo finish")]
    public bool isSpecial = false;
}
