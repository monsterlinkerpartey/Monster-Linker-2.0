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
    [Header("QTE type, 'Attack'/'Block'/'FA'")]
    public string Type;
}
