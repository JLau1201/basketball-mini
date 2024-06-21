using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Basketball : MonoBehaviour, IGrabbableObject
{
    [SerializeField] private LayerMask[] environmentMasks;

    // Return Basketballs parent GameObject
    public GameObject GetParentGameObject() {
        return gameObject;
    }

    private void OnCollisionEnter(Collision collision) {
        foreach (LayerMask mask in environmentMasks) {
            if (mask == 1 << collision.gameObject.layer) {
                BasketballGameManager.Instance.SetBasketballState(BasketballGameManager.BasketballState.OnGround);
            }
        }
    }
}
