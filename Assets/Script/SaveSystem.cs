using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerData player) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.omo";
        //FileStream stream = new FileStream(path, FileMode.Create);

        //PlayerData data = new PlayerData(gm, am);

        //formatter.Serialize(stream, data);

        //stream.Close();

        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(player);
        writer.Close();
    }

    public static PlayerData LoadPlayer() {
        string path = Application.persistentDataPath + "/player.omo";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        } else {
            Debug.Log("No save file");
            return null;
        }
    }
}
