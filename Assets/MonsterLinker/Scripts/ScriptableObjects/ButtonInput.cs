using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Button", menuName = "Other/Buttons")]
public class ButtonInput : ScriptableObject
{
    [Header("a, b, x, y, start, select")]
    [Tooltip("String for button input \nInput > Settings; all lowercase")]
    public string inputString;
    [Tooltip("Image for the Button")]
    public Sprite buttonSprite;
}
