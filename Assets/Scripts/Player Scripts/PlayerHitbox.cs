using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : Hitbox
{
    public GameObject originObject;

    private BasicEnemyScript enemy;

    // Called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = speed * direction;
    }

    // Called when another collider enters the hitbox
    void OnTriggerEnter2D( Collider2D otherCollider )
    {
        Debug.Log( "onTriggerEnterEvent: " + otherCollider.gameObject.name );

        // Grab the enemy script
        enemy = otherCollider.GetComponent<BasicEnemyScript>();

        if( enemy != null )
        {
            // Call the enemy's damage method
            enemy.TakeDamage(damageValue);
        }

        // Destroy the object after hitting a collider
        Destroy(this.gameObject);
    }
}
