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

    public void ResetHighScore()
    {
        BestScore = 0;
        BestPlayerName = "";

        // overwrite the save file with the cleared data
        SaveHighScore();
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.bestPlayerName = BestPlayerName;
        data.bestScore = BestScore;

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/savefile.json";
        File.WriteAllText(path, json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            BestPlayerName = data.bestPlayerName;
            BestScore = data.bestScore;
        }
        else
        {
            BestPlayerName = "";
            BestScore = 0;
        }
    }
}
