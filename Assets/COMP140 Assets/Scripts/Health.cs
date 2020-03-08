using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    float currentHealth = 100.0f;

    bool dead = false;


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth<0)
        {
            dead = true;
            Destroy(gameObject, 0.5f);
        }
    }

    public bool IsDead()
    {
        return true;
    }
}
