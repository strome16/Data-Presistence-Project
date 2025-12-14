using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TMP_InputField nameInputField;
    public string gameSceneName = "main";

    void Start()
    {
        RefreshHighScoreText();

        if (DataManager.Instance != null && !string.IsNullOrEmpty(DataManager.Instance.PlayerName))
        {
            nameInputField.text = DataManager.Instance.PlayerName;
        }
    }

    void RefreshHighScoreText()
    {
        if (DataManager.Instance != null && DataManager.Instance.BestScore > 0)
        {
            highScoreText.text =
                $"High Score: {DataManager.Instance.BestPlayerName} : {DataManager.Instance.BestScore}";
        }
        else
        {
            highScoreText.text = "High Score:";
        }
    }

    public void OnStartButtonClicked()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.PlayerName = nameInputField.text;
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void OnExitButtonClicked()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.SaveHighScore();
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    // NEW: hook this to the "Reset High Score" button
    public void OnResetHighScoreClicked()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.ResetHighScore();
        }

        RefreshHighScoreText();
    }
}
