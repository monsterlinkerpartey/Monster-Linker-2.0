using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Feral Art", menuName = "Attack/FeralArt")]
public class FeralArt : Attack
{
    [Tooltip("FA Name shown in menus")]
    public string FAName;
    [Header("Base Attacks for this FA")]
    [Tooltip("List of BAs needed for this FA")]
    public List<BaseAttack> FeralArtInput = new List<BaseAttack>();
}
