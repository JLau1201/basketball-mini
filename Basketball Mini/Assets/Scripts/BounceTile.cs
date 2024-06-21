using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceTile : MonoBehaviour
{
    [Header("Bounce Force")]
    [SerializeField] private float bounceForce;
    private Rigidbody rb;

    private void OnCollisionEnter(Collision collision) {
        // Check if collision contains a rigidbody
        rb = collision.gameObject.GetComponent<Rigidbody>();
        // Apply bounceForce in the upward direction of the bounce tile
        Vector3 bounceDirection = transform.up.normalized * bounceForce;
        rb.AddForce(bounceDirection, ForceMode.Impulse);
    }
}
