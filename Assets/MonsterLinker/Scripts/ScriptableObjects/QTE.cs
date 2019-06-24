using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QTE", menuName = "Other/QTE")]
public class QTE : ScriptableObject
{
    [Header("Timing for the QTEs to read out by QTEHandler")]
    public float QTEAnimationLength1;
    public float QTEAnimationLength2;
    public float QTEAnimationLength3;
    public float QTEAnimationLength4;
    public float QTEAnimationLength5;
    public float QTEAnimationLength6;
    [Header("QTE type, e.g. 'Attack'")]
    public string Type;
    public float ModifierFail;
    public float ModifierGood;
    public float ModifierPerfect;
}
