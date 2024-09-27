using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : BaseUI
{
    private const string HIGHSCORE = "highscore";

    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI yourScoreText;

    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private void Start() {
        Hide();
        restartButton.onClick.AddListener(() => {
            Hide();
            SceneManager.LoadScene(0);
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });

        BasketballGameManager.Instance.OnGameOver += BasketballGameManager_OnGameOver;
    }

    private void BasketballGameManager_OnGameOver(object sender, System.EventArgs e) {
        Show();

        UpdateTexts();
    }

    private void UpdateTexts() {
        int highScore = PlayerPrefs.GetInt(HIGHSCORE);
        float yourScore = BasketballGameManager.Instance.GetScore();
        highScoreText.text = "High Score: " + highScore.ToString();
        yourScoreText.text = "Your Score: " + yourScore.ToString();

        if(yourScore > highScore) {
            PlayerPrefs.SetInt(HIGHSCORE, (int)yourScore);
            PlayerPrefs.Save();
        }
    }
}
