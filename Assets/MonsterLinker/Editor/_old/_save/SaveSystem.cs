using UnityEngine;
using System.IO;    //for reading/writing on the OS
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Handles Save/Load Function 
/// </summary>
public static class SaveSystem
{
    public static void SavePlayer(PlayerStats player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        //Creating a folder and file in a persistant path of the OS' choosing
        string path = Application.persistentDataPath + "/monster.fun";  //name it anything
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(player);

        formatter.Serialize(stream, data);
        stream.Close();    //Never forget
    }

    public static SaveData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/monster.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("No save file found in " + path + " D:");
            return null;
        }
    }
}
