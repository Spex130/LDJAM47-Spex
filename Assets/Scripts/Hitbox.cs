using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Object public properties
    public int dmgValue = 1;
    public float speed = 0;
    public float lifespan = 1f;

    // Object private properties
    private Rigidbody2D rb;
    private Vector3 direction;

    // Private variable declarations
    private HealthScript enemy;

    // Called before the first frame update
    void Start()
    {
        // Grab the object's rigidbody
        rb = GetComponent<Rigidbody2D>();

        // Move the object
        if( rb != null )
        {
            rb.velocity = speed * direction;
        }
    }

    // Called once per frame
    void FixedUpdate()
    {
        // Update timer
        lifespan -= Time.deltaTime;

        // If time has expired, destroy the object
        if( lifespan <= 0 )
        {
            Destroy( this.gameObject );
        }
    }

    // Called when another collider enters the hitbox
    void OnTriggerEnter2D( Collider2D otherCollider )
    {
        // Grab the health script of the "enemy"
        enemy = otherCollider.GetComponent<HealthScript>();
        if( enemy != null )
        {
            // Call the enemy's damage method
            enemy.TakeDamage( dmgValue, this.GetPosition() );
        }

        // Destroy the object after hitting a collider
        Destroy( this.gameObject );
    }

    // Gets the position of the object
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    // Gets the direction of the object
    public Vector3 GetDirection()
    {
        return direction;
    }

    // Sets the direction of the object
    public void SetDirection( Vector3 newDirection )
    {
        direction = newDirection;
    }
}
