using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles Player Input except for QTEs
/// </summary>
public class PlayerInput : MonoBehaviour
{
    public void AddBaseattack(BaseAttack baseAttack)
    {
        //bool wasPressed = InputBarHandler.Add(baseAttack);
    }

    public void ResetPlayerInput()
    {
        //bool wasPressed = InputBarHandler.Reset();
    }


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
