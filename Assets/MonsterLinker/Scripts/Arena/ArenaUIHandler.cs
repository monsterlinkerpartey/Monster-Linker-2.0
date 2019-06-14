using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaUIHandler : MonoBehaviour
{
    public GameObject BlackListPanel;
    public GameObject PlayerInputPanel;
    public GameObject FAInfoAndInputButtons;
    public GameObject PlayerInputBar;
    public GameObject EnemyInputBar;
    public GameObject InitiativeCheck;
    public GameObject ResultPanel;
    public Button ConfirmBAsButton;
        
    public bool hidden;

    public void SetConfirmButtonStatus(bool enabled)
    {
        enabled = ConfirmBAsButton.enabled;
    }
    
    //TODO Show/Hide FA Info and Input Buttons
    public void AnimateInputExtras()
    {
        //TODO: animate FA info and input buttons
        if (hidden)
        {
            //FAInfoAndInputButtons.SetActive(true);
        }
        else
        {
            //FAInfoAndInputButtons.SetActive(false);
        }
    }

    public void SlideInputBarIn(GameObject inputbar)
    {
        //TODO: animate inputbars
        inputbar.SetActive(true);
    }

    public void SlideInputBarOut(GameObject inputbar)
    {
        inputbar.SetActive(false);
    }
}
