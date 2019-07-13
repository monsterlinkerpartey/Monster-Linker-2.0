using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutButtons : MonoBehaviour
{
    [Header("Drag n drop")]
    public eLoadout Window;
    public GameObject FeralArtChoice;
    public GameObject ImplantChoice;
    public Animator FeralArtChoiceAnim;
    public Animator ImplantChoiceAnim;

    public GameObject EventSystem_Normal;
    //public GameObject EventSystem_FAChoice;
    //public GameObject EventSystem_ImplantChoice;

    public Button FAChoiceButton1;
    public Button ImplantChoiceButton1;

    public List<FeralArt> LoadedFAs;

    [Header("No touchie")]
    public Button curLeftButton;
    public Text curLeftText;

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            switch (Window)
            {
                case eLoadout.LoadoutOnly:
                    print("all good, nothing to cancel here");
                    break;
                case eLoadout.FeralArtChoice:
                    WindowSwitch(eLoadout.LoadoutOnly);
                    break;
                case eLoadout.ImplantChoice:
                    WindowSwitch(eLoadout.LoadoutOnly);
                    break;
            }
        }
    }

    //TODO write script that tells left buttson which FA they are holding
    //TODO add/remove FAs from loaded list
    //TODO check which FA is in the loaded list and disable them in the choice selection

    public void CheckActiveFAs()
    {

    }

    public void OpenFAChoice(Button thisButton)
    {
        curLeftButton = thisButton;
        curLeftText = curLeftButton.GetComponentInChildren<Text>();
        WindowSwitch(eLoadout.FeralArtChoice);
        FAChoiceButton1.Select();
    }

    public void OpenImplantChoice(Button thisButton)
    {
        curLeftButton = thisButton;
        curLeftText = curLeftButton.GetComponentInChildren<Text>();
        WindowSwitch(eLoadout.ImplantChoice);
        ImplantChoiceButton1.Select();
    }

    public void ChooseFA(FeralArt thisFA)
    {
        print("pressed button for FA choice "+thisFA.name);
        curLeftText.text = thisFA.FAName;
        WindowSwitch(eLoadout.LoadoutOnly);
        curLeftButton.Select();
    }
    
    public void WindowSwitch(eLoadout window)
    {
        Window = window;

        switch (Window)
        {
            case eLoadout.LoadoutOnly:
                CloseWindows();
                //EventSystem_Normal.SetActive(true);
                //EventSystem_FAChoice.SetActive(false);
                //EventSystem_ImplantChoice.SetActive(false);
                break;
            case eLoadout.FeralArtChoice:
                FeralArtChoice.SetActive(true);
                //EventSystem_Normal.SetActive(false);
                //EventSystem_FAChoice.SetActive(true);
                break;
            case eLoadout.ImplantChoice:
                ImplantChoice.SetActive(true);
                //EventSystem_Normal.SetActive(false);
                //EventSystem_ImplantChoice.SetActive(true);
                break;
        }

    }

    public void CloseWindows()
    {
        FeralArtChoice.SetActive(false);
        ImplantChoice.SetActive(false);
        //Closes the choice windows
        //sets cur feral arts button as active button
    }


}
