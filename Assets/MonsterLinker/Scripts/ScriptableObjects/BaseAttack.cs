using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Baseattack", menuName = "Attack/BaseAttack")]
public class BaseAttack : Attack
{
    [Tooltip("Sprite shown in the Input Bar")]
    public Sprite InputBarIcon;
    [Tooltip("Sprite shown in the FA Info Panel under 'Input'")]
    public Sprite InfoPanelIcon;
    [Tooltip("Animator of the Icon")]
    public Animator SlotAnim;

    //HACK called whenever icon is added or if it is used as a feral art?
    public void AnimateIcon()
    {
        SlotAnim.SetTrigger("zoom");
    }
}
