using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectionUI : BaseUI
{
    [Header("UI")]
    [SerializeField] private MainMenuUI mainMenuUI;

    [Header("Buttons")]
    [SerializeField] private Button timeTrialButton;
    [SerializeField] private Button onlineButton;
    [SerializeField] private Button backButton;

    private void Awake() {
        Hide();
        timeTrialButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.TimeTrial);
        });
        
        onlineButton.onClick.AddListener(() => {

        });
        
        backButton.onClick.AddListener(() => {
            Hide();
            mainMenuUI.Show();
        });
    }
}
