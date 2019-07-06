using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QTE", menuName = "Other/QTE")]
public class QTE : ScriptableObject
{
    [Header("Dmg Modifiers")]
    [Tooltip("Modifier for QTE fail result")]
    public float ModifierFail;
    [Tooltip("Modifier for QTE good result")]
    public float ModifierGood;
    [Tooltip("Modifier for QTE perfect result")]
    public float ModifierPerfect;
    [Header("Timing for the QTEs to read out by QTEHandler")]
    public float QTEAnimationLength1;
    public float QTEAnimationLength2;
    public float QTEAnimationLength3;
    public float QTEAnimationLength4;
    public float QTEAnimationLength5;
    public float QTEAnimationLength6;
    [Header("QTE type, 'Attack'/'Block'/'Endurance'/'FeralArt'")]
    public string Type;
}
