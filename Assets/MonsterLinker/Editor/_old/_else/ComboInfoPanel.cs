using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Hide/Show Combo Info panel with button press
/// </summary>
public class ComboInfoPanel : MonoBehaviour
{
    [Header("For the Info Panel")]
    public Text InfoButtonText;

    [SerializeField] Animator ComboAnim;
    bool hidden = false;

    private void Start()
    {
        ComboAnim = GetComponent<Animator>();
    }

    public void HideInfo()
    {
        if (!hidden)
        {
            print("Hiding Info Panel");
            hidden = true;
            ComboAnim.Play("Hide");
            ComboAnim.SetBool("normal", false);
            ComboAnim.SetBool("hidden", true);
            InfoButtonText.text = "HIDE";
        }
        else
        {
            print("Showing Info panel");
            hidden = false;
            ComboAnim.Play("Show");
            ComboAnim.SetBool("hidden", false);
            ComboAnim.SetBool("normal", true);
            InfoButtonText.text = "SHOW";
        }
    }
}
