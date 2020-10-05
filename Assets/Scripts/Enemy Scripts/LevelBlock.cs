using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{

    public HealthScript Health;
    public Animator Anim;

    public bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        Health = this.GetComponent<HealthScript>();
        Anim = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D( Collider2D otherCollider )
    {
        //print( "onTriggerEnterEvent: " + otherCollider.gameObject.name );

        if(isInvincible == false)
        {
            Hitbox GotHitBox = otherCollider.GetComponent<Hitbox>();

            if(GotHitBox != null)
            {
                Health.TakeDamage(1, GotHitBox.transform.position);
                Anim.SetTrigger("TakeDamage");
            }

            if(Health.GetHealth() == 1)
            {
                Anim.SetTrigger("Dissolve");
            }
        }

        
    }

    public void BlockDie()
    {
        Health.TakeDamage(2000000, new Vector3(0,0,0));
        Destroy(this.gameObject);
    }

    public void DisableCollisions()
    {
        BoxCollider2D box =this.GetComponentInChildren<BoxCollider2D>();
        if(box != null)
        {
            box.enabled = false;
        }
    }

    // Called when another collider enters the hitbox
    void OnTriggerStay2D( Collider2D otherCollider )
        {
        
        print( "onTriggerStayEvent: " + otherCollider.gameObject.name );

        /*
        // Grab the health script of the "enemy"
        enemy = otherCollider.GetComponent<HealthScript>();

        if( enemy != null )
        {
            // Call the enemy's damage method
            enemy.TakeDamage( damage, transform.position );
        }
        //*/
    }
}
