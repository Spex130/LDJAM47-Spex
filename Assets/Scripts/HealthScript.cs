using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int health = 2;

    // Default constructor for taking damage
    public void TakeDamage()
    {
        TakeDamage( 1 );
    }

    // Basic constructor for taking damage
    public void TakeDamage( int damage )
    {
        health -= damage;
        DeathCheck();
    }

    // Checks if the character has enough health
    void DeathCheck()
    {
        if( health <= 0 )
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy( this.gameObject );
    }
}
