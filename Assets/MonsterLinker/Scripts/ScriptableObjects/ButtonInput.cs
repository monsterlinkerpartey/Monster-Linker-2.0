using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Button", menuName = "Other/ButtonInput")]
public class ButtonInput : ScriptableObject
{
    [Header("Input > Settings; all lowercase")]
    [Tooltip("String for button input")]
    public string inputString;
    [Tooltip("Image for the Button")]
    public Sprite buttonSprite;
}
