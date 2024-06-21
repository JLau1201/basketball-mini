using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballSpawner : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private GameObject basketballPrefab;
    [SerializeField] private Hoop hoop;

    private void Start() {
        // Subscribe to events
        BasketballGameManager.Instance.OnGameStart += BasketballGameManager_OnGameStart;
        hoop.OnScore += Hoop_OnScore;
    }

    private void BasketballGameManager_OnGameStart(object sender, System.EventArgs e) {
        SpawnBall();
    }

    private void Hoop_OnScore(object sender, System.EventArgs e) {
        // Spawn ball when player scores
        if (BasketballGameManager.Instance.GetGameState() != BasketballGameManager.GameState.Over) {
            SpawnBall();
        }
    }

    private void SpawnBall() {
        Instantiate(basketballPrefab, transform.position, Quaternion.identity);
    }
}
