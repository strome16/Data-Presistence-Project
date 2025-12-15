using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;   // green label
    public TMP_InputField nameInputField;   // name box
    public string gameSceneName = "main";   // your game scene name

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
            Debug.Log($"[MenuUIHandler] Starting game as '{DataManager.Instance.PlayerName}'");
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

    public void OnResetHighScoreClicked()
    {
        Debug.Log("[MenuUIHandler] Reset button clicked");

        if (DataManager.Instance != null)
        {
            DataManager.Instance.ResetHighScore();
        }
        else
        {
            Debug.LogWarning("[MenuUIHandler] DataManager.Instance is null on reset!");
        }

        RefreshHighScoreText();   // <- this is what updates the label
    }
}
