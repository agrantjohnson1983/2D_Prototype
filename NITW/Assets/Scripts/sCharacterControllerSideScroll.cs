using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class sCharacterController : MonoBehaviour
{

    Rigidbody2D rb;

    [Header("Movemement Side To Side")]
    public float characterSpeed;
    private float characterStartingSpeed;
    private Vector2 inputVelocity;
    private Vector3 startingPosition;
    private Vector2 directionSideToSide;

    [Header("Movement Flying")]
    public GameObject broom, witchHat;
    public float characterFlyingSpeed;
    public static bool isFlying = false;
    private Vector2 directionFlying;

    [Header("Movement State")]
    public float movementStateSwitchCooldownTime = 1.5f;
    bool canSwitchMovementState = true;

    public SpriteRenderer spriteRenderer;

    [Header("Jumping")]
    public float jumpPower;
    private bool isJumping = false;

    private bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public GameObject aimArm;
    public GameObject reticleCanvas;

    sProjectileController projectileController;

    public static bool isOutside = true;

    public static sCharacterController characterControllerGlobal;

    private void Awake()
    {
        characterControllerGlobal = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterStartingSpeed = characterSpeed;
        startingPosition = rb.position;
        inputVelocity = new Vector2();
        directionSideToSide = new Vector2();

        broom.SetActive(isFlying);
        
        witchHat.SetActive(isFlying);
        
        reticleCanvas.SetActive(isFlying);

        aimArm.SetActive(isFlying);

        projectileController = GetComponent<sProjectileController>();
        projectileController.enabled = isFlying;

        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpCheck();

        MovementInputs();

        MovementStateSwitcher();
    }

    private void FixedUpdate()
    {
        MovementPhysics();

        Jumping();
    }

    void MovementStateSwitcher()
    {
        // Input for switching between flight
        if ((Input.GetKey(KeyCode.F)) && canSwitchMovementState)
        {
            canSwitchMovementState = false;

            // When flying - switchies back to walk state and turns gravity on
            if(isFlying)
            {
                Debug.Log("Switching to Walk state");
                isFlying = false;
                rb.gravityScale = 1f;
            }

            // When not flying - switching to flight state and turns gravity off
            else
            {
                Debug.Log("Switching to Flight state");
                isFlying = true;
                rb.gravityScale = 0f;
            }

            broom.SetActive(isFlying);
            witchHat.SetActive(isFlying);
            reticleCanvas.SetActive(isFlying);
            projectileController.enabled = isFlying;
            aimArm .SetActive(isFlying);

            StartCoroutine(MovementStateSwitchCooldown());
        }
    }

    // Cooldown for the state switch so player can't spam it
    IEnumerator MovementStateSwitchCooldown()
    {
        Debug.Log("Starting State Switch Cooldown");

        yield return new WaitForSeconds(movementStateSwitchCooldownTime);

        Debug.Log("State Switch Cooldown Complete");

        canSwitchMovementState = true;
    }

    void MovementInputs()
    {
        // Takes input from vertical and horizontal axis
        inputVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // converts inputs to side to side direction
        directionSideToSide = new Vector2(inputVelocity.x, 0);

        //directionFlying = new Vector2(inputVelocity.x)

        // flips the sprite based on input direction
        if(inputVelocity.x > 0)
        {
            spriteRenderer.flipX = true;
        }

        else if (inputVelocity.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void JumpCheck()
    {
        // Perform the ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        //Debug.Log("isGrounded = " + isGrounded);

        // checks if linear velocity is less than or equal to zero when jumping
        /*if (rb.linearVelocity.y == 0 && isJumping)
        {
            // toggles jump bool off
            isJumping = false;
            Debug.Log("Toggling Jump off");
        }*/
    }

    void Jumping()
    { 
        // jump input
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("jumping!");

            // toggles jump bool on
            isJumping = true;

            // resets velocity before jump
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            // jump movement physics
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    void MovementPhysics()
    {
        // checks if there is any input magnitute
        if (inputVelocity.sqrMagnitude > 0.1f)
        {
            float totalSpeed = characterSpeed;

            if(!isFlying)
            {
                // handles the side to side physics movement
                rb.linearVelocity = new Vector2(directionSideToSide.x * characterSpeed, rb.linearVelocity.y);

                //Debug.Log("Moving Character");
            }

            else
            {
                

                rb.linearVelocity = inputVelocity * characterFlyingSpeed;
            }

        }
    }

    public void SetLocation(Vector3 _pos)
    {
        this.transform.position = _pos;
    }
}
