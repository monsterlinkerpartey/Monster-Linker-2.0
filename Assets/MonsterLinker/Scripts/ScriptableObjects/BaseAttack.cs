using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BaseAttack", menuName = "BaseAttack")]
public class BaseAttack : ScriptableObject
{
    [Header("Values for GD iterations")]
    [Tooltip("Basedamage the attack makes")]
    public float BaseDMG;

    [Header("--- No changes after this point---")]
    [Tooltip("Sprite shown in the Input Bar")]
    public Sprite InputBarIcon;
    [Tooltip("Sprite for the Button")]
    public Sprite ButtonSprite;
    [Tooltip("Animator of the Icon")]
    public Animator SlotAnim;

    //HACK called whenever icon is added or if it is used as a feral art?
    public void AnimateIcon()
    {
        SlotAnim.SetTrigger("zoom");
    }
}
