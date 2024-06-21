using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomHoopCollider : MonoBehaviour 
{
    [Header("Hoop")]
    [SerializeField] private Hoop hoop;
    [SerializeField] private LayerMask ballMask;
    private void OnTriggerEnter(Collider other) {
        // Check for ball collision 
        if (ballMask == 1 << other.gameObject.layer) {
            hoop.canScore = false;
        }
    }
}
