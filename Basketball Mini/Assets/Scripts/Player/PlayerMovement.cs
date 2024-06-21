using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Game Inputs")]
    [SerializeField] private GameInputs gameInput;

    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float rotationSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;

    [Header("Resistances")]
    [SerializeField] private float groundDrag;
    [SerializeField] private float airRes;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isMoving;

    private State currentState;

    // Track which state the player is in
    public enum State {
        Held,       // Player is being held by another player
        OnGround,   // Player is on the ground
        InAir,      // Player is in the air
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();

        gameInput.OnJump += GameInput_OnJump;
    }

    private void GameInput_OnJump(object sender, System.EventArgs e) {
        // Apply an upward force the the player if grounded
        if (isGrounded) {
            if(rb != null) {
                // Reset vertical velocity
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                // Apply jumpForce in upward direction
                Vector3 jumpDirection = Vector3.up * jumpForce;
                rb.AddForce(jumpDirection, ForceMode.Impulse);
            }
        }
    }
    
    // Uses collider as trigger on bottom portion of player
    // Checks if collider is on/off ground
    private void OnTriggerStay(Collider other) {
        if (groundMask == 1 << other.gameObject.layer) {
            SetState(State.OnGround);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (groundMask == 1 << other.gameObject.layer) {
            SetState(State.InAir);
        }
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    // Read movement input from gameInputs
    // Move rigidbody without force
    private void HandleMovement() {
        // Movement input
        Vector2 moveInput = gameInput.GetMoveInput();

        // Check for movement
        if (moveInput != Vector2.zero) {
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
            float moveDistance = moveSpeed * Time.fixedDeltaTime;
            
            // Calculate rotation angle based on move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
            
            // Apply movement based on currentState
            // Dont apply movement if being held
            if (currentState == State.OnGround) {
                rb.AddForce(moveDirection.normalized * moveDistance * speedMultiplier, ForceMode.Impulse);
                LimitSpeed();
            } else if (currentState == State.InAir) {
                // Apply air resistance if in air
                rb.AddForce(moveDirection.normalized * moveDistance * speedMultiplier * airRes, ForceMode.Impulse);
                LimitSpeed();
            }
        }

        isMoving = moveInput != Vector2.zero;
    }
    
    // Limit the players speed 
    private void LimitSpeed() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude > moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    // Set the players movement state
    public void SetState(State newState) {
        currentState = newState;
        switch (currentState) {
            case State.OnGround:
                isGrounded = true;
                rb.drag = groundDrag;
                break;
            case State.InAir:
                isGrounded = false;
                rb.drag = 0;
                break;
            case State.Held:
                isGrounded = false;
                break;
            default:
                break;
        }
    }

    // IsMoving parameter for moving animation
    public bool IsMoving() {
        return isMoving;
    }
}