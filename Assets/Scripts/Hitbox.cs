using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damageValue = 1;
    public float timer = 1f;
    public float speed = 0;
    public int direction = 1;

    [HideInInspector]
    public Rigidbody2D rigidbody;

    // Called before the first frame update
    void Start()
    {
        
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
    }

    // Called when another collider exits the hitbox
    void OnTriggerExit2D( Collider2D otherCollider )
    {
        Debug.Log( "onTriggerExitEvent: " + otherCollider.gameObject.name );
    }

    public void setDirection( int newDirection )
    {
        direction = newDirection;
    }
}
