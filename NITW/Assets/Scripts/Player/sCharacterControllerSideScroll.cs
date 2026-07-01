using System.Collections;
using UnityEngine;

public class sCharacterControllerSideScroll : sCharacterControllerBASE
{
    //Rigidbody2D rb;

    [Header("Movement Side To Side")]
    public float characterSpeed;
    private float characterStartingSpeed;
    private Vector2 inputVelocity;
    private Vector3 startingPosition;
    private Vector2 directionSideToSide;

    //[Header("Movement Flying")]
    //public GameObject broom, witchHat;
    //public float characterFlyingSpeed;
    //public static bool isFlying = false;
    //private Vector2 directionFlying;

    [Header("Movement State")]
    public float movementStateSwitchCooldownTime = 1.5f;
    //bool canSwitchMovementState = true;

    bool isSprinting = false;
    public float sprintMultiplier = 2.5f;
    float currentSprintMultiplier = 1f;

    [Header("Jumping")]
    public float jumpPower;
    //private bool isJumping = false;

    [Tooltip("How long after leaving a ledge the player can still jump (seconds)")]
    public float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    [Tooltip("How early a jump input can be buffered before landing (seconds)")]
    public float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    public string jumpAudioCue = "jump";

    private bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    [Header("Slopes")]
    public float maxSlopeAngle = 45f;
    private bool isOnSlope;
    private bool isOnSlopeMoving;
    private Vector2 slopeNormalPerp;
    private float slopeAngle;
    private PhysicsMaterial2D frictionMaterial;
    private PhysicsMaterial2D noFrictionMaterial;
    public BoxCollider2D boxCollider;

    private void Awake()
    {
        //if (characterControllerSideScrollGlobal == null)
        //    characterControllerSideScrollGlobal = this;
        //else
        //    Destroy(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();

    }

    public override void OnDisable()
    {
        base.OnDisable();

    }

    public override void Start()
    {
        //rb = GetComponent<Rigidbody2D>();

        // calls the base
        base.Start();

        characterStartingSpeed = characterSpeed;
        startingPosition = rb.position;
        inputVelocity = new Vector2();
        directionSideToSide = new Vector2();

        // Create two physics materials at runtime so no asset files are needed
        frictionMaterial = new PhysicsMaterial2D("Friction");
        frictionMaterial.friction = 1f;
        frictionMaterial.bounciness = 0f;

        noFrictionMaterial = new PhysicsMaterial2D("NoFriction");
        noFrictionMaterial.friction = 0f;
        noFrictionMaterial.bounciness = 0f;

        //broom.SetActive(isFlying);
        //witchHat.SetActive(isFlying);
        //reticleCanvas.SetActive(isFlying);
        //aimArm.SetActive(isFlying);

        //projectileController = GetComponent<sProjectileController>();
        //projectileController.enabled = isFlying;

        sPlayer.playerGlobal.SetActiveMovementObject(this.gameObject);
    }

    public override void Update()
    {
        base.Update();

        JumpCheck();
        MovementInputs();
        //MovementStateSwitcher();

        // Read jump input in Update so no frames are missed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            SlopeCheck();
            MovementPhysics();
            Jumping();
        }
    }

    void SlopeCheck()
    {
        // Cast a ray straight down from the ground check point to sample the surface normal
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.3f, groundLayer);

        if (hit)
        {
            // Perpendicular to the surface normal gives us the direction to move along the slope
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            // Angle between the surface normal and world up tells us how steep the slope is
            slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            isOnSlope = slopeAngle != 0f && slopeAngle <= maxSlopeAngle;
        }
        else
        {
            isOnSlope = false;
        }

        isOnSlopeMoving = isOnSlope && inputVelocity.sqrMagnitude > 0.1f;

        // Use friction material when standing still on a slope so the player does not slide down
        // Use no friction when moving or in the air so movement feels snappy
        if (isOnSlope && inputVelocity.sqrMagnitude < 0.1f && isGrounded)
        {
            boxCollider.sharedMaterial = frictionMaterial;
        }
        else
        {
            boxCollider.sharedMaterial = noFrictionMaterial;
        }
    }

    void JumpCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Coyote time: count down from the last moment the player was grounded
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void Jumping()
    {
        // Use buffered input + coyote time instead of raw GetKeyDown + isGrounded
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            audioData.TriggerAudio("jump", audioSource);

            //isJumping = true;

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    void MovementInputs()
    {
        inputVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        directionSideToSide = new Vector2(inputVelocity.x, 0);

        // checks if can move before flipping sprite
        if(canMove)
        {
            // flips sprites based on movement direction
            if (inputVelocity.x > 0)
                spriteRenderer.flipX = true;
            else if (inputVelocity.x < 0)
                spriteRenderer.flipX = false;
        }    
        
    }

    void MovementPhysics()
    {
        //if (!isFlying)
        //{
        if (inputVelocity.sqrMagnitude > 0.1f)
        {
            if(isSprinting)
            {
                currentSprintMultiplier = sprintMultiplier;
            }

            else
            {
                currentSprintMultiplier = 1f;
            }

            if (isOnSlope && isGrounded)
            {
                float slopeDir = -inputVelocity.x;
                rb.linearVelocity = slopeNormalPerp * slopeDir * characterSpeed * currentSprintMultiplier;
            }

            else
            {
                rb.linearVelocity = new Vector2(directionSideToSide.x * characterSpeed * currentSprintMultiplier, rb.linearVelocity.y);
            }
        }
        else
        {
            // No input - kill horizontal velocity but preserve vertical so gravity still works
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

}