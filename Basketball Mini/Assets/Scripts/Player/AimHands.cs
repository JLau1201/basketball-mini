using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimHands : MonoBehaviour
{
    [Header("Arms")]
    [SerializeField] private Transform arms;
    [SerializeField] private float rotationSpeed;

    private void Update() {
        float mouseY = Input.mousePosition.y;

        // Convert mouseY to a rotation angle
        float targetAngle = Mathf.Clamp((mouseY / Screen.height - 0.5f) * -180f, -90f, 90f);

        // Create a target rotation based on the calculated angle
        Quaternion targetRotation = Quaternion.Euler(targetAngle, 0f, 0f);

        // Smoothly interpolate towards the target rotation
        arms.localRotation = Quaternion.Slerp(arms.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
