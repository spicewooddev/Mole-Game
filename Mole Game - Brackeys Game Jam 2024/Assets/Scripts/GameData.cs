using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData {
    public ulong molesSaved = 0;
    public ulong molesDead = 0;
}

static class GameDataFileHandler {
    public static GameData Load() {
        GameData gd = null;
        string filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
        try {
            string jsonData = "";
            using (FileStream stream = new FileStream(filePath, FileMode.Open)) {
                using (StreamReader reader = new StreamReader(stream)) {
                    jsonData = reader.ReadToEnd();
                }
            }

            gd = JsonUtility.FromJson<GameData>(jsonData);
        }
        catch (Exception e) {
            Debug.Log("Error loading GameData object: " + e);
        }

        if(gd == null) {
            gd = new GameData();
        }
        return gd;
    }

    public static void Save(GameData gd) {
        string filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
        try {
            Directory.CreateDirectory(Application.persistentDataPath);
            string jsonData = JsonUtility.ToJson(gd, true);
            using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(jsonData);
                }
            }
        }
        catch (Exception e) {
            Debug.LogError("Error saving GameData object: " + e);
        }
    }
}

