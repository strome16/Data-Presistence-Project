using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;      // <-- NEW for Button
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI ScoreText;
    public GameObject GameOverText;
    public TextMeshProUGUI BestScoreText;

    public Button returnToMenuButton;   // <-- NEW
    public string menuSceneName = "Menu"; // <-- NEW (set this to your menu scene name)

    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        if (returnToMenuButton != null)          // <-- NEW
            returnToMenuButton.gameObject.SetActive(false);  // hide button at start

        UpdateBestScoreUI();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (DataManager.Instance != null && m_Points > DataManager.Instance.BestScore)
        {
            DataManager.Instance.BestScore = m_Points;
            DataManager.Instance.BestPlayerName = DataManager.Instance.PlayerName;
            DataManager.Instance.SaveHighScore();
        }

        UpdateBestScoreUI();

        if (returnToMenuButton != null)          // <-- NEW
            returnToMenuButton.gameObject.SetActive(true);   // show Menu button on game over
    }

    void UpdateBestScoreUI()
    {
        if (DataManager.Instance != null && DataManager.Instance.BestScore > 0)
        {
            BestScoreText.text =
                $"Best: {DataManager.Instance.BestPlayerName}: {DataManager.Instance.BestScore}";
        }
        else
        {
            BestScoreText.text = "Best: ---";
        }
    }

    // <-- NEW: called by the Menu button
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
