using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles Player Input (except for QTEs?)
/// </summary>
public class ArenaPlayerInput : MonoBehaviour
{
    public InputBarHandler inputbarhandler;
    public ArenaUIHandler arenaui;
    public FeralArtCheck feralartcheck;

    public List<FeralArt> ChosenFAs;
       
    public void ConfirmLoadout() //List<Attack> choosenFAs
    {
        arenaui.FALoadout.SetActive(false);

        //TODO: load FA into profile
        //GameStateSwitch.Instance.curProfile.FALoadout = ChosenFAs;
        GameStateSwitch.Instance.SwitchState(eGameState.Intro);
    }

    public void AddBaseattack(BaseAttack baseAttack)
    {
        inputbarhandler.Add(baseAttack);
        //update UI 
    }

    public void ResetPlayerInput()
    {
        inputbarhandler.Reset();
    }

    public void ConfirmOutput()
    {

    }

    public void RetryFight()
    {
        GameStateSwitch.Instance.ResetFight();
        GameStateSwitch.Instance.SwitchState(eGameState.PlayerInput);
        //Scene curScene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(curScene.name);
    }

    public void RetryWithLoadout()
    {
        GameStateSwitch.Instance.ResetFight();
        GameStateSwitch.Instance.SwitchState(eGameState.Loadout);
        //Scene curScene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(curScene.name);
    }

    public void NextFight()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("Home");
    }

    public void Test()
    {
        arenaui.FALoadout.SetActive(false);
        GameStateSwitch.Instance.FightResult = eFightResult.Defeat;
        GameStateSwitch.Instance.SwitchState(eGameState.Result);
    }

    //public void ChooseProfile(Save saveSlot)
    //{
    //    if (saveSlot.Used)
    //    {
    //        updateui.OpenDialogue(updateui.OverwriteProfileDialogue);
    //    }
    //    else
    //    {
    //        WriteProfile(saveSlot);
    //    }
    //}

    //public void WriteProfile(Save saveSlot)
    //{
    //    updateui.OpenDialogue(updateui.InsertPlayerDataDialogue);
    //}

    //public void DoNotOverwriteProfile()
    //{
    //    updateui.CloseDialogue(updateui.OverwriteProfileDialogue);
    //}

    //public void StartGame()
    //{
    //    //updateui.UIState(eHomeUI.Home);
    //}

    //-----------------------------------

    //public void ComboConfirm()
    //{
    //    ArenaManager.Instance.ArenaState = eArena.EnemyInput;
    //    //TODO state ändern
    //    //    print("combo confirmed");
    //    //    bool wasPressed = comboManager.Confirm();
    //    //    //TODO evaluate combo, go to fight animation screen

    //}

    //public void ExitArena()
    //{
    //    //TODO Go back to home menu - currently reloading scene
    //    GlobalManager.Instance.News.text += ("Demo end");
    //    Scene scene = SceneManager.GetActiveScene();
    //    SceneManager.LoadScene(scene.name);
    //}

    //public void StartFight()
    //{
    //    ArenaManager.Instance.ArenaState = eArena.Intro;
    //}

    //public void HideInfo()
    //{
    //    if (!hidden)
    //    {
    //        print("Hiding Info Panel");
    //        hidden = true;
    //        InputAnim.Play("hide");
    //        InputAnim.SetBool("normal", false);
    //        InputAnim.SetBool("hidden", true);
    //        InfoButtonText.text = "SHOW";
    //    }
    //    else
    //    {
    //        print("Showing Info panel");
    //        hidden = false;
    //        InputAnim.Play("show");
    //        InputAnim.SetBool("hidden", false);
    //        InputAnim.SetBool("normal", true);
    //        InfoButtonText.text = "HIDE";
    //    }
    //}
}
