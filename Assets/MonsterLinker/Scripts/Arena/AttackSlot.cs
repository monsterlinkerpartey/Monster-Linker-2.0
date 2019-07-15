using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSlot : MonoBehaviour
{
    public Image icon;
    public Image frame;  
    BaseAttack attack;
    public AnimOnEnable enableAnimation;

    public void AddCombo(BaseAttack newattack)
    {
        attack = newattack;
        icon.sprite = attack.InputBarIcon;
        icon.enabled = true;
        enableAnimation.enabled = true;
    }

    public void ClearSlot()
    { 
        attack = null;
        icon.sprite = null;
        icon.enabled = false;
        enableAnimation.enabled = false;
    }

}
