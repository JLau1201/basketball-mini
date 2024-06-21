using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [Header("Game Inputs")]
    [SerializeField] private GameInputs gameInput;

    [Header("Hands")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float throwForce;
    [SerializeField] private Vector3 overlapBoxDims;

    [Header("Ennvironment")]
    [SerializeField] private Transform threePointLine;

    private GameObject grabbedObject;
    private Rigidbody grabbedObjectRb;

    private bool isGrabbing;

    private void Start() {
        // Subscrive to events
        gameInput.OnGrab += GameInput_OnGrab;
        gameInput.OnRelease += GameInput_OnRelease;
    }

    private void GameInput_OnRelease(object sender, System.EventArgs e) {
        // Check if the player is holding something
        if (isGrabbing && grabbedObject != null) {
            grabbedObjectRb = grabbedObject.AddComponent<Rigidbody>();
            
            // Apply throwForce in direction of the players holding point
            grabbedObjectRb.AddForce(holdPoint.forward * throwForce, ForceMode.Impulse);
            
            // Reset the grabbed objects parent
            grabbedObject.transform.SetParent(null);

            // Set the point at which the player threw the ball
            SetThrowPoint();
            BasketballGameManager.Instance.StopShotClock();

            // Check if object is basketball
            if (grabbedObject.GetComponent<Basketball>() != null) {

                BasketballGameManager.Instance.SetBasketballState(BasketballGameManager.BasketballState.InAir);
            }
        }
        isGrabbing = false;
    }

    private void GameInput_OnGrab(object sender, System.EventArgs e) {
        // Check if the player is not holding anything 
        // Check for a grabbable object
        if (!isGrabbing && IsGrabbableObjectInRange()) {
            // Set the grabbed object to the player
            grabbedObject.transform.SetParent(holdPoint);
            grabbedObject.transform.SetLocalPositionAndRotation(Vector3.zero, grabbedObject.transform.rotation);
            Destroy(grabbedObjectRb);

            BasketballGameManager.Instance.StartShotClock();
            BasketballGameManager.Instance.SetScoreIncrement(BasketballGameManager.Scores.TwoPoint);



            // Check if object is basketball
            if(grabbedObject.GetComponent<Basketball>() != null) {
                BasketballGameManager.Instance.SetBasketballState(BasketballGameManager.BasketballState.Held);
            }

            isGrabbing = true;
        }
    }

    private void SetThrowPoint() {
        // Set points to score based on where the player threw the ball
        if(transform.position.x < threePointLine.position.x) {
            BasketballGameManager.Instance.SetScoreIncrement(BasketballGameManager.Scores.ThreePoint);
        } else {
            BasketballGameManager.Instance.SetScoreIncrement(BasketballGameManager.Scores.TwoPoint);
        }
    }
    
    private bool IsGrabbableObjectInRange() {
        // Apply OverlapBox to check for all collisions within area
        Collider[] colliders = Physics.OverlapBox(grabPoint.position, overlapBoxDims);
        // Loop through all collisions
        foreach (Collider collider in colliders) {
            // Check if the collision parent inherits IGrabbableObject
            IGrabbableObject grabbableObject = collider.GetComponentInParent<IGrabbableObject>();
            if (grabbableObject != null && grabbableObject.GetParentGameObject() != gameObject) {
                grabbedObject = grabbableObject.GetParentGameObject();
                grabbedObjectRb = grabbedObject.GetComponent<Rigidbody>();

                return true;
            }
        }
        
        return false;
    }
}
