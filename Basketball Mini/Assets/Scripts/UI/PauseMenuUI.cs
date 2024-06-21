using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : BaseUI
{

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Start() {
        Hide();

        BasketballGameManager.Instance.OnGamePause += BasketballGameManager_OnGamePause;
        BasketballGameManager.Instance.OnGameUnpause += BasketballGameManager_OnGameUnpause;
    }

    private void BasketballGameManager_OnGamePause(object sender, System.EventArgs e) {
        Show();
    }

    private void BasketballGameManager_OnGameUnpause(object sender, System.EventArgs e) {
        Hide();
    }

    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            BasketballGameManager.Instance.ToggleGamePause();
        });

        settingsButton.onClick.AddListener(() => {
        
        });

        quitButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

}
