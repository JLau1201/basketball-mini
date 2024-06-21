using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    public static ScoreboardManager Instance { get; private set; }

    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI shotClockText;
    [SerializeField] private TextMeshProUGUI timeText;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        UpdateShotClock();
        UpdateTime();
    }

    public void UpdateScore() {
        scoreText.text = ((int)Mathf.Ceil(BasketballGameManager.Instance.GetScore())).ToString("D3");
    }

    public void UpdateShotClock() {
        shotClockText.text = ((int)Mathf.Ceil(BasketballGameManager.Instance.GetShotClock())).ToString();
    }

    public void UpdateTime() {
        float time = BasketballGameManager.Instance.GetGameTime();
        if(time < 0) {
            time = 0;
        }
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
