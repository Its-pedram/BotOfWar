using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitDetectionScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }
}
