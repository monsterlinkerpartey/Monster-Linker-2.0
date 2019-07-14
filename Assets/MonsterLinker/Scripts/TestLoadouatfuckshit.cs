using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLoadouatfuckshit : MonoBehaviour
{

    public Button curLeftButton;
    public Text curLeftText;

    public GameObject shittypanel;
    public Button shittyChoiceButton1;

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            shittypanel.SetActive(false);
        }
    }

    public void TestFA(Button curButton)
    {
        curLeftButton = curButton;
        print("choosing fa slot");
        shittypanel.SetActive(true);
        shittyChoiceButton1.Select();
    }

    public void TestWriteFA()
    {
        print("writing this fucking fa");
        shittypanel.SetActive(false);
        curLeftButton.Select();
    }
}
