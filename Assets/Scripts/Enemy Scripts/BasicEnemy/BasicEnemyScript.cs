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
        // If time has expired, attack
        timer-= Time.deltaTime;
        if( timer <= 0 )
        {
            Attack( enemyShootPrefab, 2, 2, 0 );
            timer = timerOriginal;
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
            enemy.TakeDamage( damage, transform.position );
        }
    }

    void OnTriggerExit2D( Collider2D col )
    {
        Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
    }

    public void Attack( Hitbox prefab, int damage, float xOffset, float yOffset )
    {
        Vector3 xOffsetVector = new Vector3( xOffset , 0, 0 );
        Vector3 yOffsetVector = new Vector3( 0 , yOffset, 0 );
        Vector3 spawnPos = this.transform.position + xOffsetVector + yOffsetVector;
        
        Hitbox hitbox = GameObject.Instantiate<Hitbox>( prefab, spawnPos, Quaternion.identity );
        hitbox.SetDirection( Vector3.right );
    }
}
