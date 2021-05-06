using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    int health = 100;
    public float healthTime = 1.5f;
    public HealthTracker healthText;
    public Transform targetLocation;
    CharacterController controller;
    float originalTime;

    void Start() {
        originalTime = healthTime;
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        healthTime -= Time.deltaTime;
        if (healthTime <= 0)
        {
            healthTime = originalTime;
            if(health < 100) health++;
            SetHealth();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            health -= 2;
            SetHealth();
        } else if (other.tag == "MechBullet") {
            health -= 6;
            SetHealth();
        }
        if (health <= 0) {
            Debug.Log("Dead");
            Teleport();
            health = 100;
            SetHealth();
        }
    }
    void SetHealth()
    {
        healthText.SetHealth(health);
    }

    void Teleport()
    {
        controller.enabled = false;
        transform.position = targetLocation.position;
        controller.enabled = true;
    }
}
