using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour
{
    public int damage = 2;
    public float timer;
    private float timerOriginal;
    private HealthScript enemy;

    [Tooltip("The Hitbox Prefab used to instantiate stuff.")]
    public Hitbox enemyShootPrefab;

    // Start is called before the first frame update
    void Start()
    {
        timerOriginal = timer;
    }

    // Update is called once per frame
    void Update()
    {
        // Attack based on timer
        timer -= Time.deltaTime;
        if( timer <= 0 )
        {
            Attack( enemyShootPrefab, 2, 2, 0 );
            timer = timerOriginal;
        }
    }

    // Called when another collider enters the hitbox
    void OnTriggerEnter2D( Collider2D otherCollider )
    {
        // Debug.Log( "onTriggerEnterEvent: " + otherCollider.gameObject.name );

        // Grab the health script of the "enemy"
        enemy = otherCollider.GetComponent<HealthScript>();

        if( enemy != null )
        {
            // Call the enemy's damage method
            enemy.TakeDamage( damage, transform.position );
        }
    }

    // Called when another collider enters the hitbox
    void OnTriggerStay2D( Collider2D otherCollider )
    {
        // Debug.Log( "onTriggerStayEvent: " + otherCollider.gameObject.name );

        // Grab the health script of the "enemy"
        enemy = otherCollider.GetComponent<HealthScript>();

        if( enemy != null )
        {
            // Call the enemy's damage method
            enemy.TakeDamage( damage, transform.position );
        }
    }

    // Gets the position of the object
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    // Gets the direction of the object
    public Vector3 GetDirection()
    {
        int xDir = transform.localScale.x > 0 ? 1 : -1;

        return new Vector3( xDir, 0, 0 );
    }

    // Attack method
    public void Attack( Hitbox prefab, int damage, float xOffset, float yOffset )
    {
        // Set hitbox position
        Vector3 xOffsetVector = new Vector3( xOffset , 0, 0 );
        Vector3 yOffsetVector = new Vector3( 0 , yOffset, 0 );
        Vector3 spawnPos = this.transform.position + ( GetDirection().x * xOffsetVector ) + yOffsetVector;
        // Create new hitbox object
        Hitbox hitbox = GameObject.Instantiate<Hitbox>( prefab, spawnPos, Quaternion.identity );
        // Set hitbox direction
        hitbox.SetDirection( GetDirection() );
    }
}
