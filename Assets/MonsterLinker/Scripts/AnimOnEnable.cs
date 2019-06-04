using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Actives the animation effect of the input bar icons on first input
/// Might be deleted if base attack function works
/// </summary>
public class AnimOnEnable : MonoBehaviour
{
    [SerializeField] Animator SlotAnim;
    
    private void OnEnable()
    {
        SlotAnim = GetComponent<Animator>();
        SlotAnim.SetTrigger("zoom");
    }    
}
