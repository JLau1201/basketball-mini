using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputs : MonoBehaviour
{
    public event EventHandler OnJump;
    public event EventHandler OnGrab;
    public event EventHandler OnRelease;
    public event EventHandler OnPauseAction;

    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Subscribe to events
        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.Grab.started += Grab_started;
        playerInputActions.Player.Grab.canceled += Grab_canceled;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() {
        playerInputActions.Player.Jump.performed -= Jump_performed;
        playerInputActions.Player.Grab.started -= Grab_started;
        playerInputActions.Player.Grab.canceled -= Grab_canceled;
        playerInputActions.Player.Pause.performed -= Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Grab_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnRelease?.Invoke(this, EventArgs.Empty);
    }

    private void Grab_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnGrab?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMoveInput() {
        return playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    public void ForceRelease() {
        OnRelease?.Invoke(this, EventArgs.Empty);
    }
}
