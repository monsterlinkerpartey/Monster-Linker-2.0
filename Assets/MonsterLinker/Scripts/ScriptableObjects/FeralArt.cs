using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FeralArt", menuName = "FeralArt")]
public class FeralArt : ScriptableObject
{
    [Header("Values for GD iterations")]
    [Tooltip("Basedamage the FA makes")]
    public float BaseDMG;

    [Header("--- No changes after this point---")]
    [Tooltip("List of BAs needed for this FA")]
    public List<BaseAttack> FeralArtInput = new List<BaseAttack>();    

}
