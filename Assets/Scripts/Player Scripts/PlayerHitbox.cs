using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{

    public int damageValue = 1;

    public float Timer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Timer-= Time.deltaTime;
        if(Timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
        BasicEnemyScript enemy = col.GetComponent<BasicEnemyScript>();
        if(enemy != null)
        {
            enemy.TakeDamage(damageValue);
            Destroy(this.gameObject);
        }
	}


	void OnTriggerExit2D( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

}
