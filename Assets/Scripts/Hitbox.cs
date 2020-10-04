using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damageValue = 1;
    public float timer = 1f;
    public float speed = 0;

    private Rigidbody2D rb;
    private Vector3 direction;
    private HealthScript enemy;

    // Called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if( rb != null )
        {
            rb.velocity = speed * direction;
        }
    }

    // Called once per frame
    void FixedUpdate()
    {
        // If time has expired, destroy object
        timer-= Time.deltaTime;
        if( timer <= 0 )
        {
            Destroy( this.gameObject );
        }
    }

    // Called when another collider enters the hitbox
    void OnTriggerEnter2D( Collider2D otherCollider )
    {
        Debug.Log( "onTriggerEnterEvent: " + otherCollider.gameObject.name );

        // Grab the health script of the "enemy"
        enemy = otherCollider.GetComponent<HealthScript>();

        if( enemy != null )
        {
            // Call the enemy's damage method
            enemy.TakeDamage( damageValue );
        }

        // Destroy the object after hitting a collider
        Destroy( this.gameObject );
    }

    // Called when another collider exits the hitbox
    void OnTriggerExit2D( Collider2D otherCollider )
    {
        Debug.Log( "onTriggerExitEvent: " + otherCollider.gameObject.name );
    }

    // Sets the direction of the object
    public void SetDirection( Vector3 newDirection )
    {
        direction = newDirection;
    }
}
