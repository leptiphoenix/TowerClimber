using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string Path()
    {
        return Application.persistentDataPath + "/save.dat";
    }

    public static void Write(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Path(), FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SaveData Read()
    {
        if (File.Exists(Path()))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Path(), FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found");
            return null;
        }
    }
}