using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IGrabbableObject
{
    // Returns Player gameObject
    public GameObject GetParentGameObject() {
        return gameObject;
    }
}
