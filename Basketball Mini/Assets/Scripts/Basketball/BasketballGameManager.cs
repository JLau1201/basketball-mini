using System;
using System.Collections;
using UnityEngine;

public class BasketballGameManager : MonoBehaviour
{
    // Singleton pattern for Game Manager
    public static BasketballGameManager Instance { get; private set; }

    // Events
    public event EventHandler OnGameStart;
    public event EventHandler OnGamePause;
    public event EventHandler OnGameUnpause;
    public event EventHandler OnGameOver;

    [Header("Game Inputs")]
    [SerializeField] private GameInputs gameInputs;

    [Header("Scoreboard")]
    [SerializeField] private float gameTimeMax;
    [SerializeField] private float shotClockMax;

    private float gameTime;
    private float shotClock;
    private float score = 0;
    private bool shotClockActive = false;
    private float countdown = 3;
    private bool isGamePaused = false;

    private GameState currentGameState;

    public enum GameState {
        Countdown,
        Playing,
        Over,
    }

    private BasketballState currentBasketballState;

    public enum BasketballState {
        Held,
        InAir,
        OnGround,
    }

    private Scores currentScoreInc;

    public enum Scores {
        ThreePoint,
        TwoPoint,
    }

    private void Start() {
        gameInputs.OnPauseAction += GameInputs_OnPauseAction;
    }

    // Pause game 
    private void GameInputs_OnPauseAction(object sender, EventArgs e) {
        ToggleGamePause();
    }

    public void ToggleGamePause() {
        isGamePaused = !isGamePaused;
        if (isGamePaused) {
            Time.timeScale = 0f;
            OnGamePause?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;
            OnGameUnpause?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Awake() {
        Instance = this;
        SetGameState(GameState.Countdown);
        ResetShotClock();
    }

    private void SetGameState(GameState newGameState) {
        currentGameState = newGameState;
        switch (currentGameState) {
            case (GameState.Countdown):
                Time.timeScale = 1f;
                gameTime = countdown;
                break;
            case (GameState.Playing):
                gameTime = gameTimeMax;
                OnGameStart?.Invoke(this, EventArgs.Empty);
                break;
            default:
                break;
        }
    }

    private void Update() {
        switch (currentGameState) {
            case (GameState.Countdown):
                gameTime -= Time.deltaTime;
                if (gameTime < 0) {
                    SetGameState(GameState.Playing);
                }
                break;
            case (GameState.Playing):
                gameTime -= Time.deltaTime;

                // Lower shot clock time if the shotclock is active
                if (shotClockActive) {
                    shotClock -= Time.deltaTime;
                }

                // Force throw the ball if shot clock runs out
                if (shotClock < 0) {
                    gameInputs.ForceRelease();
                }

                // Stop game when time is out
                if (gameTime < 0) {
                    SetGameState(GameState.Over);
                }
                break;
            case (GameState.Over):
                if (currentBasketballState != BasketballState.InAir) {
                    StartCoroutine(ReduceTimeScaleOverTime());
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator ReduceTimeScaleOverTime() {
        float slowTimeScale = .05f;
        while (Time.timeScale > slowTimeScale) { // Adjust the threshold as needed
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0, slowTimeScale * Time.deltaTime);
            yield return null;
        }
        Time.timeScale = 0;
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseScore() {
        switch (currentScoreInc) {
            case Scores.ThreePoint:
                score += 3;
                break;
            case Scores.TwoPoint:
                score += 2;
                break;
            default:
                break;
        }
        ScoreboardManager.Instance.UpdateScore();
    }

    public float GetScore() {
        return score;
    }

    public float GetShotClock() {
        return shotClock;
    }

    public float GetGameTime() {
        return gameTime;
    }
    
    public void SetScoreIncrement(Scores newScoreInc) {
        currentScoreInc = newScoreInc;
    }

    public void ResetShotClock() {
        shotClock = shotClockMax;
    }

    public void StartShotClock() {
        ResetShotClock();
        shotClockActive = true;
    }
    
    public void StopShotClock() {
        shotClockActive = false;
    }

    public void SetBasketballState(BasketballState newState) {
        currentBasketballState = newState;
    }

    public GameState GetGameState() {
        return currentGameState;
    }
}
