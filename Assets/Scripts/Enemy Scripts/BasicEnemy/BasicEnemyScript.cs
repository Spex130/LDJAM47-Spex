using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour
{
    public float timer;
    private float timerOriginal;

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
            Attack( enemyShootPrefab, 2, 0, 0 );
            timer = timerOriginal;
        }
    }

    void OnTriggerEnter2D( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
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
