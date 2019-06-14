using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Button", menuName = "Other/ButtonInput")]
public class ButtonInput : ScriptableObject
{
    [Tooltip("Input > Settings; ALL UPPERCASE")]
    public string inputString;
    [Tooltip("Image for the Button")]
    public Sprite buttonSprite;
}
