using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class SaveLoader : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void JS_FileSystem_Sync();


    [Serializable]
    public class SaveData
    {
        public string Language;
        public int PointsTotal;
    }

    public static void LoadFromJson()
    {
        Debug.Log(Application.persistentDataPath);

        string savePath = (Application.persistentDataPath + "/Savefile.Json");

#if UNITY_WEBGL && !UNITY_EDITOR
         string saveFolder  = "/idbfs/GeoQuiz";
          if (!Directory.Exists(saveFolder)) {
             Directory.CreateDirectory(saveFolder);
             Debug.Log("Creating save directory: " + saveFolder);
         }
         savePath = (saveFolder + "/Savefile.Json");
#endif
        Debug.Log(savePath);
        if (File.Exists(savePath))
        {
            SaveData savedata = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
            
            SaveResources.Language = savedata.Language;
            SaveResources.PointsTotal = savedata.PointsTotal;

            Debug.Log("Successfully loaded.");
        }
        else
        {
            Debug.Log("Using default.");
        }
    }

    public static void SaveToJson()
    {
        string savePath = (Application.persistentDataPath + "/Savefile.Json");

#if UNITY_WEBGL && !UNITY_EDITOR
         string saveFolder  = "/idbfs/GeoQuiz";
          if (!Directory.Exists(saveFolder)) {
             Directory.CreateDirectory(saveFolder);
             Debug.Log("Creating save directory: " + saveFolder);
         }
         savePath = (saveFolder + "/Savefile.Json");
#endif

        SaveData data = new()
        {
            Language = SaveResources.Language,
            PointsTotal = SaveResources.PointsTotal
        };
        Debug.Log(savePath);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Application.ExternalEval("_JS_FileSystem_Sync();");
        Debug.Log("Data saved to: " + savePath);
    }

    [Serializable]
    public static class SaveResources
    {
        public static string Language;
        public static int PointsTotal;
    }
}
 