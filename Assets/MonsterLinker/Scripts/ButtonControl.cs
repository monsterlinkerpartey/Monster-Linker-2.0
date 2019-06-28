using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    [Tooltip("Invis. Button to deselect the others")]
    public Button noButton;
    [Tooltip("Projekt Settings, Input, All UPPERCASE")]
    public string InputName;
    [Tooltip("Checked if Button is disabled")]
    public bool Bdisabled;
    [Tooltip("Button Object giving the button its sprite and input string")]
    public ButtonInput ButtonInfo;

    Button thisButton;
    Animator buttonAnim;
    Image buttonImg;

    void Start()
    {
        thisButton = GetComponent<Button>();
        buttonAnim = GetComponent<Animator>();
        buttonImg = gameObject.GetComponent<Image>();

        if (ButtonInfo != null)
        {
            InputName = ButtonInfo.inputString;
            buttonImg.sprite = ButtonInfo.buttonSprite;
        }

        if (InputName == null)
            Debug.LogError("No Input for " + thisButton.name + " set");
    }

    // Update is called once per frame
    void Update()
    {
        if (Bdisabled)
        {
            thisButton.enabled = false;
            buttonAnim.SetTrigger("Disabled");
        }
        else
        {
            thisButton.enabled = true;
            buttonAnim.SetTrigger("Normal");

            if (Input.GetButtonDown(InputName))
            {
                buttonAnim.SetTrigger("Highlighted");
                thisButton.onClick.Invoke();
            }
            else if (Input.GetButtonUp(InputName))
            {
                buttonAnim.SetTrigger("Normal");
                if (noButton != null)
                    noButton.Select();
            }
        }
    }
}

