using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSlot : MonoBehaviour
{
    public Image icon;
    public Image frame;  
    AttackIcon attack;
    public AnimOnEnable enableAnimation;    

    public void AddCombo(AttackIcon newattack)
    {
        attack = newattack;
        icon.sprite = attack.comboPanelIcon;
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
