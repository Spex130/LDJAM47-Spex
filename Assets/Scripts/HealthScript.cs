using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    // Object public properties
    public int health = 10;
    public float invTimer = 0;

    public ParticleSystem DeathParticleSystem;
    public bool useParticleSystem = true;

    // Object private properties
    private PlayerScript player;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerScript>();
    }

    // Called every frame
	void Update()
    {
        // Update timer
        timer += Time.deltaTime;
    }

    // Default constructor for taking damage
    public void TakeDamage( Vector3 hitboxPos )
    {
        TakeDamage( 1, hitboxPos );
    }

    // Basic constructor for taking damage
    public void TakeDamage( int damage, Vector3 hitboxPos )
    {
        if( timer >= invTimer )
        {
            // Decrement health
            health -= damage;

            // Check if object should exist
            DeathCheck();

            // Reset timer
            timer = 0;
        }
    }

    // Checks if the character has enough health
    void DeathCheck()
    {
        if( health <= 0 )
        {
            Die();
        }
    }

    // Executes death function
    public void Die()
    {
        if(useParticleSystem)
            GameObject.Instantiate<ParticleSystem>(DeathParticleSystem,this.transform.position, Quaternion.identity);
        Destroy( this.gameObject );
    }

    public int GetHealth()
    {
        return health;
    }
}
