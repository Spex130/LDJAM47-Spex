using UnityEngine;
using System.Collections;
using Prime31;


public class PlayerScript : MonoBehaviour
{
    // Must be set to true before we can interact with the game
    public bool hasPlayerSpawned = false;

	// Movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 10f;
    public float jumpRecoil = 100f;

    // Flag for resetting jumps
    private bool isJumping = false;

    public float detectDistance = 2f;
    public LayerMask platformMask = 0;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    [Header("Drag References")]
	private CharacterController2D _controller;
	public Animator _animator;

    [Tooltip("The Hitbox Prefab used to instantiate stuff.")]
    public Hitbox playerSlashPrefab;
    public Hitbox playerShootPrefab;

    // Private Stuff
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

    // Called when the object is instantiated
    void Awake()
    {
        // Makes sure that the PlayerObject has an attached Animator
        if( _animator != null )
        {
            _animator = GetComponentInChildren<Animator>();
        }

        _controller = GetComponent<CharacterController2D>();

        // Listens to some events for illustration purposes
        _controller.onControllerCollidedEvent += OnControllerCollider;
        _controller.onTriggerEnterEvent += OnTriggerEnterEvent;
        _controller.onTriggerExitEvent += OnTriggerExitEvent;
    }

    #region Event Listeners

    void OnControllerCollider( RaycastHit2D hit )
    {
        // bail out on plain old ground hits cause they arent very interesting
        if( hit.normal.y == 1f )
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        // Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }

    void OnTriggerEnterEvent( Collider2D col )
    {
        Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
    }


    void OnTriggerExitEvent( Collider2D col )
    {
        Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
    }

    #endregion

	// Called every frame
	void Update()
	{
        _animator.SetBool("IsGrounded", _controller.isGrounded);
        // Contains a very simple example of moving the character around and controlling the animation
        if( hasPlayerSpawned )
        {
            AnimatorStateInfo animInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float currentAnimFrame = animInfo.normalizedTime % 1;
            _animator.SetFloat("CurrentAnimFrame", currentAnimFrame);
            _animator.SetFloat("VertVelocity", _controller.velocity.y);

            // If the player is grounded, keep the height velocity at 0
            if( _controller.isGrounded )
                _velocity.y = 0;

            // Moves the player right
            if( Input.GetKey( KeyCode.RightArrow ) )
            {
                normalizedHorizontalSpeed = 1;
                if( transform.localScale.x < 0f )
                    transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

                if( _controller.isGrounded )
                    _animator.SetBool("Run", true);
            }
            // Moves the player left
            else if( Input.GetKey( KeyCode.LeftArrow ) )
            {
                normalizedHorizontalSpeed = -1;
                if( transform.localScale.x > 0f )
                    transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

                if( _controller.isGrounded )
                    _animator.SetBool("Run", true);
            }
            // Makes the player idle
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

            // Allows player jump on the ground
            if( _controller.isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) )
            {
                _velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
                _animator.SetBool("Jump", true);
                _animator.ResetTrigger("Idle");
                isJumping = true;
            }
            // Allows player to wall jump
            else if( Physics2D.Raycast( transform.position + Vector3.up, GetPlayerDirection(), detectDistance, platformMask ) && Input.GetKeyDown( KeyCode.UpArrow ) )
            {
                // Debug.DrawRay(transform.position, GetPlayerDirection() * detectDistance, Color.red);
                _velocity.x = GetPlayerDirection().x * -jumpRecoil;

                _velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
                _animator.SetBool("Jump", true);
                _animator.ResetTrigger("Idle");
                isJumping = true;
            }
            else
            {
                _animator.SetBool("Jump", false);
            }

            // Starts falling if the jump key is released early
            if( Input.GetKeyUp( KeyCode.UpArrow ) && isJumping )
            {
                _velocity.y = 0;
                isJumping = false;
                _animator.SetBool("Jump", false);
            }

            // Applies horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
            var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
            _velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

            // Applies gravity before moving
            _velocity.y += gravity * Time.deltaTime;

            // If holding 'Down', bumps up our movement amount and turn off one way platform detection for a frame
            // This lets us jump down through one way platforms
            if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) )
            {
                _velocity.y *= 3f;
                _controller.ignoreOneWayPlatformsThisFrame = true;
            }

            _controller.move( _velocity * Time.deltaTime );

            // Grabs our current _velocity to use as a base for all calculations
            _velocity = _controller.velocity;

            // Allows the player to melee attack
            if(Input.GetKeyUp( KeyCode.Z ))
            {
                if(_controller.isGrounded)
                {
                    _animator.SetTrigger("GroundSlash");
                    Attack(playerSlashPrefab, 2);
                }
            }

            // Allows the player to ranged attack
            if(Input.GetKeyUp( KeyCode.X ))
            {
                if(_controller.isGrounded)
                {
                    _animator.SetTrigger("GroundShot");
                    Attack(playerShootPrefab, 2);
                }
            }
        }
    }

    // Sets the player spawn flag
    public void MarkPlayerSpawned()
    {
        hasPlayerSpawned = true;
    }

    // Returns the direction of the player
    public Vector3 GetPlayerDirection()
    {
        int xDir = transform.localScale.x > 0 ? 1 : -1;

        return new Vector3( xDir, 0, 0 );
    }

    // Default constructor for attack
    public void Attack( Hitbox prefab, int damage )
    {
        Attack( prefab, damage, 5, 4 );
    }

    // Basic constructor for attack
    public void Attack( Hitbox prefab, int damage, float xOffset, float yOffset )
    {
        Vector3 xOffsetVector = new Vector3( xOffset , 0, 0 );
        Vector3 yOffsetVector = new Vector3( 0 , yOffset, 0 );
        Vector3 spawnPos = this.transform.position + ( GetPlayerDirection().x * xOffsetVector ) + yOffsetVector;
        
        Hitbox hitbox = GameObject.Instantiate<Hitbox>( prefab, spawnPos, Quaternion.identity );
        hitbox.SetDirection( GetPlayerDirection() );
    }
}
