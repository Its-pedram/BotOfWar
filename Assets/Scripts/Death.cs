using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    CharacterController controller;
    public Transform targetLocation;
    private void Start() {
        
    }
    private void OnTriggerEnter(Collider other) {
        controller = other.GetComponent<CharacterController>();
        Teleport(other);
    }

    void Teleport(Collider other)
    {
        controller.enabled = false;
        other.transform.position = targetLocation.position;
        controller.enabled = true;
    }
}
