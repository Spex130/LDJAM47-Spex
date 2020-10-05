using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class BasicEnemyScript : MonoBehaviour
{
    public int damage = 2;
    public float gravity = -25f;
    	public float runSpeed = 8f;
	public float groundDamping = 20f;
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	public Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
    public float timer;
    public float currentAnimFrame;
    private float timerOriginal;
    private HealthScript enemy;

    //Private Variables
    [SerializeField]
    private CharacterController2D _controller;
    private Vector3 _velocity;

    public enum EnemyState {Idle, Shooting, Moving, Hurting }

    public EnemyState ActionState = EnemyState.Idle;

    [Tooltip("The Hitbox Prefab used to instantiate stuff.")]
    public Hitbox enemyShootPrefab;

    // Start is called before the first frame update
    void Start()
    {
        timerOriginal = timer;
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate();
        PhysicsUpdateLogic();
    }

    public void StateUpdate()
    {
        AnimatorStateInfo animInfo = _animator.GetCurrentAnimatorStateInfo(0);
        currentAnimFrame = animInfo.normalizedTime % 1;
        _animator.SetFloat("CurrentAnimFrame", currentAnimFrame);
        switch (ActionState)
        {
            case EnemyState.Idle:
                // Attack based on timer
                timer -= Time.deltaTime;
                if( timer <= 0 )
                {
                    timer = timerOriginal;
                    _animator.SetTrigger("Fire");
                    Attack( enemyShootPrefab, 2, 2, 0 );
                    ActionState = EnemyState.Shooting;
                    
                }
            break;

            case EnemyState.Shooting:
                
                if( currentAnimFrame > .9f)
                {
                    ActionState = EnemyState.Idle;
                }
            break;

            case EnemyState.Moving:

            break;

            case EnemyState.Hurting:

            break;

            default:

            break;
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
            _animator.SetTrigger("Hurt");
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
            _animator.SetTrigger("Hurt");
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


    void PhysicsUpdateLogic()
	{
		if( _controller.isGrounded )
			_velocity.y = 0;

    /*
		if( Input.GetKey( KeyCode.RightArrow ) )
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else if( Input.GetKey( KeyCode.LeftArrow ) )
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			if( _controller.isGrounded )
				//_animator.Play( Animator.StringToHash( "Idle" ) );
		}
        */

        /*
		// we can only jump whilst grounded
		if( _controller.isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
			_animator.Play( Animator.StringToHash( "Jump" ) );
		}
    //*/


		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets us jump down through one way platforms
		if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) )
		{
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

}
