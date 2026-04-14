using UnityEngine;

public class sCharacterController : MonoBehaviour
{

    Rigidbody2D rb;

    [Header("Movemement")]
    public float characterSpeed;
    private float characterStartingSpeed;
    private Vector2 inputVelocity;
    private Vector3 startingPosition;
    private Vector2 directionSideToSide;

    SpriteRenderer spriteRenderer;

    [Header("Jumping")]
    public float jumpPower;
    private bool isJumping = false;

    private bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterStartingSpeed = characterSpeed;
        startingPosition = rb.position;
        inputVelocity = new Vector2();
        directionSideToSide = new Vector2();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpCheck();
        MovementInputs();
    }

    private void FixedUpdate()
    {
        MovementPhysics();

        Jumping();
    }

    void MovementInputs()
    {
        // Takes input from vertical and horizontal axis
        inputVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // converts inputs to side to side direction
        directionSideToSide = new Vector2(inputVelocity.x, 0);

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

            // handles the side to side physics movement
            rb.linearVelocity = new Vector2(directionSideToSide.x * characterSpeed, rb.linearVelocity.y);

            //Debug.Log("Moving Character");
        }
    }
}
