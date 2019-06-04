using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackInputButtons : MonoBehaviour
{
    public Button HighButton;
    public Button MidButton;
    public Button LowButton;
    
    void Update()
    {
        if (!ArenaManager.Instance.disableAttackInput)
        {
            HighButton.enabled = true;
            MidButton.enabled = true;
            LowButton.enabled = true;            

            //high = Input.GetAxisRaw("PADV") == 1f;
            //low = Input.GetAxisRaw("PADV") == -1f;
            //mid = Input.GetAxisRaw("PADH") == 1f;

            if (DPadButtons.Up)
            {
                HighButton.animator.SetTrigger("Highlighted");
                HighButton.onClick.Invoke();
            }          
            else
            {
                HighButton.animator.SetTrigger("Normal");
            }
            if (DPadButtons.Right)
            {
                MidButton.animator.SetTrigger("Highlighted");
                MidButton.onClick.Invoke();
            }
            else
            {
                MidButton.animator.SetTrigger("Normal");
            }
            if (DPadButtons.Down)
            {
                LowButton.animator.SetTrigger("Highlighted");
                LowButton.onClick.Invoke();
            }
            else
            {
                LowButton.animator.SetTrigger("Normal");
            }           
           
        }
       else
        {
            HighButton.enabled = false;
            MidButton.enabled = false;
            LowButton.enabled = false;
        }        
    }
}
