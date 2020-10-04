﻿using UnityEngine;
using System.Collections;
using Prime31;


public class PlayerScript : MonoBehaviour
{
    //Must be set to true before we can interact with the game.
    public bool hasPlayerSpawned = false;

	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
    private bool isJumping = false;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    [Header("Drag References")]
	private CharacterController2D _controller;
	public Animator _animator;

    [Tooltip("The Hitbox Prefab used to instantiate stuff.")]
    public PlayerHitbox playerHitboxPrefab;


    //Private Stuff
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;


    void Awake()
    {
        _animator = _animator == null ? _animator = _animator : _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
    }


    #region Event Listeners

    void onControllerCollider( RaycastHit2D hit )
    {
        // bail out on plain old ground hits cause they arent very interesting
        if( hit.normal.y == 1f )
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent( Collider2D col )
    {
        Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
    }


    void onTriggerExitEvent( Collider2D col )
    {
        Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
    }

    #endregion

    public void markPlayerSpawned()
    {
        hasPlayerSpawned = true;
    }

    public void createHitbox(int damageIn)
    {
        GameObject.Instantiate<PlayerHitbox>(playerHitboxPrefab, this.transform.position + new Vector3(2,2,0), Quaternion.identity);
    }

    public void createHitbox(int damageIn, float xOffset, float yOffset)
    {
        GameObject.Instantiate<PlayerHitbox>(playerHitboxPrefab, this.transform.position + new Vector3(xOffset * getPlayerDirection(), yOffset, 0), Quaternion.identity);
    }

    public int getPlayerDirection()
    {
        return transform.localScale.x > 0 ? 1 : -1;
    }


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
        if(hasPlayerSpawned)
        {
            AnimatorStateInfo animInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float currentAnimFrame = animInfo.normalizedTime % 1;
            _animator.SetFloat("CurrentAnimFrame", currentAnimFrame);
            _animator.SetFloat("VertVelocity", _controller.velocity.y);

            if( _controller.isGrounded )
                _velocity.y = 0;

            if( Input.GetKey( KeyCode.RightArrow ) )
            {
                normalizedHorizontalSpeed = 1;
                if( transform.localScale.x < 0f )
                    transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

                if( _controller.isGrounded )
                    _animator.SetBool("Run", true);
            }
            else if( Input.GetKey( KeyCode.LeftArrow ) )
            {
                normalizedHorizontalSpeed = -1;
                if( transform.localScale.x > 0f )
                    transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

                if( _controller.isGrounded )
                    _animator.SetBool("Run", true);
            }
            else
            {
                _animator.SetBool("Run", false);
                normalizedHorizontalSpeed = 0;

                if( _controller.isGrounded )
                {
                    _animator.ResetTrigger("GroundSlash");
                    _animator.ResetTrigger("GroundShot");
                    //_animator.ResetTrigger("AirSlash");
                    _animator.SetBool("Idle", true);
                }
                else
                {
                    _animator.SetBool("Idle", false);
                }
                    
            }


            // we can only jump whilst grounded
            if( _controller.isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) )
            {
                _velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
                _animator.SetBool("Jump", true);
                _animator.ResetTrigger("Idle");
                isJumping = true;
            }
            else
            {
                _animator.SetBool("Jump", false);
            }

            // Immediately start falling if jump key is released early
            if( Input.GetKeyUp( KeyCode.UpArrow ) && isJumping )
            {
                _velocity.y = 0;
                isJumping = false;
            }


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

            if(Input.GetKeyUp( KeyCode.Z ))
            {
                if(_controller.isGrounded)
                {
                    _animator.SetTrigger("GroundSlash");
                    createHitbox(2, 5, 4);
                }
                
            }

            if(Input.GetKeyUp( KeyCode.X ))
            {
                if(_controller.isGrounded)
                {
                    _animator.SetTrigger("GroundShot");
                }
                
            }
        }
    }

}
