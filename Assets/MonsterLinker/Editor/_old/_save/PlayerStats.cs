using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player control and variables
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public string LinkerName;
    public string MonsterName;
    public int NumberOfAttackSlotslots;


    void Start()
    {
        
    }

    void Update()
    {
        //Calling Save
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SavePlayer();
        }
        //Calling Load
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadPlayer();
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        print("saved");
    }

    public void LoadPlayer()
    {
        SaveData data = SaveSystem.LoadPlayer();

        LinkerName = data.LinkerName;
        MonsterName = data.MonsterName;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        transform.position = position;
        print("loaded");
    }
}
