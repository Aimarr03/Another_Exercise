using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    private int maxHealth;
    private int health;

    public HealthSystem(int health)
    {
        maxHealth = health;
        this.health = maxHealth;
    }

    public bool TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        Debug.Log("Unit take " + damage + " damage");
        Debug.Log("heatlh: " + health);
        return health <= 0;
    }
    public int CurrentHealth()
    {
        return health;
    }
}
