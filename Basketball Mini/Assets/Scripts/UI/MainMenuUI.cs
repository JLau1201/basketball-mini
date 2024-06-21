using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    [Header("UI")]
    [SerializeField] private GameSelectionUI gameSelectionUI;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            Hide();
            gameSelectionUI.Show();
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
