using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{

    public bool FileExists(string fileName)
    {
        string path = Application.persistentDataPath + ("/" + fileName + ".json");
        return File.Exists(path);
    }

    public void SaveIntoJson(string fileName, GameData gameData)
    {
        Debug.Log("Saving to: " + (Application.persistentDataPath + "/" + fileName));
        string json = JsonUtility.ToJson(gameData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", json);
    }

    public T LoadJson<T>(string fileName)
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/" + fileName + ".json");
        return JsonUtility.FromJson<T>(json);
    }
}
