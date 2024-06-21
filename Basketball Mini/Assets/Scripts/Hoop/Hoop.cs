using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoop : MonoBehaviour {

    // Event when the player scores a basket
    public event EventHandler OnScore;

    [Header("Ball")]
    [SerializeField] private LayerMask ballMask;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem onScoreFX;

    public bool canScore;

    private void Awake() {
        // Subscribe to event
        OnScore += Hoop_OnScore;
    }

    private void Hoop_OnScore(object sender, EventArgs e) {
        BasketballGameManager.Instance.IncreaseScore();
        BasketballGameManager.Instance.ResetShotClock();
        BasketballGameManager.Instance.StopShotClock();
        BasketballGameManager.Instance.SetBasketballState(BasketballGameManager.BasketballState.OnGround);

        // Play FX
        Instantiate(onScoreFX);
    }

    private void OnTriggerEnter(Collider other) {
        // Check if ball collides with ball hitbox
        if (ballMask == 1 << other.gameObject.layer) {
            if (canScore) {
                Destroy(other.gameObject.transform.parent.gameObject);
                OnScore?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
