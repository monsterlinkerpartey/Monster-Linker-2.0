using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutButtons : MonoBehaviour
{
    [Header("Drag n drop")]
    public eLoadout Window;
    public eImplant ChoosenImplant;

    public GameObject FeralArtChoice;
    public GameObject ImplantChoice;
    public Animator FeralArtChoiceAnim;
    public Animator ImplantChoiceAnim;

    public GameObject EventSystem_Normal;

    public Button FAChoiceButton1;
    public Button ImplantChoiceButton1;

    public List<FeralArt> LoadedFAs = new List<FeralArt>(3);
    public List<Button> FAChoiceButton;
    public List<Button> SIChoiceButton;

    [Header("No touchie")]
    public Button curLeftButton;
    public Text curLeftText;
    public Text ImplantText;

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

    public void ConfirmLoadout() 
    {
        if (LoadedFAs[0] != null && LoadedFAs[1] != null && LoadedFAs[2] != null && ChoosenImplant != eImplant.None)
        {
            GameStateSwitch.Instance.arenaui.FALoadout.SetActive(false);
            GameStateSwitch.Instance.curProfile.FALoadout = LoadedFAs;
            GameStateSwitch.Instance.SwitchState(eGameState.Intro);
        }
        else
            print("not enough FAs chosen");
    }

    public void OpenFAChoice(Button thisButton)
    {
        curLeftButton = thisButton;
        curLeftText = curLeftButton.GetComponentInChildren<Text>();        
        WindowSwitch(eLoadout.FeralArtChoice);
        FAChoiceButton1.Select();        
    }
    public void ChooseFA(FeralArt thisFA)
    {
        if (!LoadedFAs.Contains(thisFA))
        {
            int slotNo = int.Parse(curLeftButton.name);
            LoadedFAs[slotNo] = thisFA;
            print("pressed button for FA choice "+thisFA.name);
            curLeftText.text = thisFA.FAName;
        }
        else
        {
            print("FA already in list");
        }
        WindowSwitch(eLoadout.LoadoutOnly);
        curLeftButton.Select();
    }

    public void OpenImplantChoice(Button thisButton)
    {
        curLeftButton = thisButton;
        curLeftText = curLeftButton.GetComponentInChildren<Text>();        
        WindowSwitch(eLoadout.ImplantChoice);
        ImplantChoiceButton1.Select();
    }

    public void ChooseImplant(Implant implant)
    {
        if (ChoosenImplant != implant.implant)
        {
            ChoosenImplant = implant.implant;
            curLeftText.text = implant.name;
        }
        else
        {
            print("implant already chosen");
        }
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
                break;
            case eLoadout.FeralArtChoice:
                FeralArtChoice.SetActive(true);
                break;
            case eLoadout.ImplantChoice:
                ImplantChoice.SetActive(true);
                break;
        }
    }

    public void CloseWindows()
    {
        FeralArtChoice.SetActive(false);
        ImplantChoice.SetActive(false);
    }
}
