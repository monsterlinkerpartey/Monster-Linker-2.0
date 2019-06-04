using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// Manages all global variables, Singleton instance
/// </summary>
public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    public eMenu MainMenu;
    //TODO enum für gamemode?

    //TODO get monster and linker name when it is entered and when loaded    
    [Header("Player Stats")]
    public string LinkerName;
    public string Monstername;
    public string curEnemyName;
    
    public int BaseDmg;
    public int PlayerMaxHealth;
    public int PlayerMaxRagePoints;
    public int EnemyMaxHealth;
    public int EnemyMaxRagePoints;

    public int MaxPlayerSlots;
    public int MaxEnemySlots;

    public GameObject DebugWindow;
    public Text News;

    //public int SaveFile;


    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0) && DebugWindow.activeSelf)
            DebugWindow.SetActive(false);
        else if (Input.GetKeyDown(KeyCode.Keypad0) && !DebugWindow.activeSelf)
            DebugWindow.SetActive(true);

        if (Input.GetKeyDown(KeyCode.KeypadMinus) && DebugWindow.activeSelf)
        {
            News.text = ("");
        }
    }
}

