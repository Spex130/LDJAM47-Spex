using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int health = 2;
    public bool kbEnabled = false;
    public float kbHeight = 10f;
    public float kbRecoil = 100f;
    public float kbTimer = 1f;
    private float timer = 0;
    private bool isHit = false;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Called every frame
	void Update()
    {
        if( isHit )
        {
            timer += Time.deltaTime;
        }
        // If time has expired
        if( timer >= kbTimer )
        {
            Debug.Log("hi");
            // Stop the forces
            rb.velocity = Vector2.zero;
            // Reset the timer
            timer = 0;
            isHit = false;
        }
    }

    // Returns the direction of the object
    public Vector3 GetDirection()
    {
        int xDir = transform.localScale.x > 0 ? 1 : -1;

        return new Vector3( xDir, 0, 0 );
    }

    // Default constructor for taking damage
    public void TakeDamage( Vector3 hitboxPos )
    {
        TakeDamage( 1, hitboxPos );
    }

    // Basic constructor for taking damage
    public void TakeDamage( int damage, Vector3 hitboxPos )
    {
        health -= damage;
        if( kbEnabled )
        {
            float xComp = ( transform.position.x - hitboxPos.x ) * kbRecoil;
            float yComp = kbHeight;
            rb.velocity = new Vector2( xComp, yComp );
            isHit = true;
        }
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
