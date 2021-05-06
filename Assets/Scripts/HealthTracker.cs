using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthTracker : MonoBehaviour
{
    public int healthValue = 100;
    public int maxHealth = 100;
    TextMeshProUGUI Health;

    void Start()
    {
        Health = GetComponent<TextMeshProUGUI>();
        SetText();
    }

    public void SetHealth(int health)
    {
        healthValue = health;
        SetText();
    }

    private void SetText()
    {
        Health.color = new Color32(255, 255, 255, 225);
        if (healthValue <= 25)
        {
            Health.color = new Color32(200, 0, 0, 225);
        }
        Health.text = "Health: " + healthValue.ToString() + " / " + maxHealth.ToString();
    }
}
