using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FeralArt", menuName = "FeralArt")]
public class FeralArt : ScriptableObject
{
    [Header("Values for GD iterations")]
    [Tooltip("Basedamage the FA makes")]
    public float DMG;

    [Header("Base Attacks for this FA")]
    [Tooltip("List of BAs needed for this FA")]
    public List<BaseAttack> FeralArtInput = new List<BaseAttack>();
    [Tooltip("Counter for FeralArtCheck")]
    [HideInInspector] public int Counter;



}
