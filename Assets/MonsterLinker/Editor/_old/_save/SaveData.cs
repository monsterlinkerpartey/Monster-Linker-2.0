using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Serializable data save
/// </summary>
[Serializable]
public class SaveData
{
    public int SceneID;
    public float [] position;

    public string LinkerName;
    public string MonsterName;

    public SaveData(PlayerStats player)
    {
        Scene curScene = SceneManager.GetActiveScene();

        SceneID = curScene.buildIndex;
        LinkerName = player.LinkerName;
        MonsterName = player.MonsterName;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        
    }

}
