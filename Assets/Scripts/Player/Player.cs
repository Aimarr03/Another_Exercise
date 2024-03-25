using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int startingHealth;

    private HealthSystem healthSystem;

    public static event EventHandler DeathTrigger;

    private void Awake()
    {
        healthSystem = new HealthSystem(startingHealth);
    }
    public void TakeDamage(int damage)
    {
        bool isDead = healthSystem.TakeDamage(damage);
        if (isDead)
        {
            Debug.Log("Player is Dead");
            DeathTrigger?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
