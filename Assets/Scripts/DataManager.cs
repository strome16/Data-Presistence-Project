using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string PlayerName;
    public string BestPlayerName;
    public int BestScore;

    [System.Serializable]
    class SaveData
    {
        public string bestPlayerName;
        public int bestScore;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.bestPlayerName = BestPlayerName;
        data.bestScore = BestScore;

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log($"[DataManager] Saving HS '{BestPlayerName}' : {BestScore} to {path}");
        File.WriteAllText(path, json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log($"[DataManager] Loading HS from {path}");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log($"[DataManager] Loaded json: {json}");
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            BestPlayerName = data.bestPlayerName;
            BestScore = data.bestScore;
        }
        else
        {
            Debug.Log("[DataManager] No save file, using defaults");
            BestPlayerName = "";
            BestScore = 0;
        }
    }

    public void ResetHighScore()
    {
        Debug.Log("[DataManager] ResetHighScore called");
        BestPlayerName = "";
        BestScore = 0;
        SaveHighScore();
    }
}
