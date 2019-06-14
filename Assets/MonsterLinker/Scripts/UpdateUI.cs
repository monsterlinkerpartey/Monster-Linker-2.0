using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    [Header("Drag n Drop Menus")]
    [Tooltip("General Menus")]
    public GameObject TitleScreen;
    public GameObject SaveScreen;
    public GameObject HomeScreen;
    public GameObject BlacklistScreen;
    [Tooltip("Active in certain cases")]
    public GameObject OverwriteProfileDialogue;
    public GameObject InsertPlayerDataDialogue;

    public void OpenDialogue(GameObject dialogue)
    {
        dialogue.SetActive(true);
    }

    public void CloseDialogue(GameObject dialogue)
    {
        dialogue.SetActive(false);
    }

    public void UIState(eArenaUI UIState)
    {
        switch (UIState)
        {
            case eArenaUI.ArenaIntro:
                break;
            case eArenaUI.PlayerInput:
                break;
            case eArenaUI.PlayerInputConfirm:
                break;
            case eArenaUI.InitiativeCheck:
                break;
            case eArenaUI.PlayerTurn:
                break;
            case eArenaUI.EnemyTurn:
                break;
            case eArenaUI.Result:
                break;
        }
    }


    
}