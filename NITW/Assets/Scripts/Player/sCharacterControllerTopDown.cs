using JetBrains.Annotations;
using UnityEngine;

public class sCharacterControllerTopDown : sCharacterControllerBASE
{
    //Rigidbody2D rb;

    [Header("Movemement")]
    public float characterSpeed;
    private float characterStartingSpeed;
    private Vector2 inputVelocity;
    private Vector3 startingPosition;

    //SpriteRenderer spriteRenderer;
    public Sprite[] spriteMovementArray;

    static bool isPaused = false;

    public bool isInOverworld = false;

    public string lowerworldScene;
    public string middleworldScene;
    public string overworldScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        characterStartingSpeed = characterSpeed;
        startingPosition = rb.position;
        inputVelocity = new Vector2();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if(isPaused) return;

        MovementInputs();

        ChangeLevelInputs();
    }

    private void FixedUpdate()
    {
        MovementPhysics();
    }

    void MovementInputs()
    {
        // Takes input from vertical and horizontal axis
        inputVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //inputVelocity.Normalize();

        //SpriteController(inputVelocity);
    }

    void ChangeLevelInputs()
    {
        /*if(Input.GetKey(KeyCode.Space))
        {
            if(isInOverworld)
            {
                sSceneManger.sceneManagerGlobal.LoadScene(middleworldScene, eDirection.north, Vector3.zero);
            }

            else
            {
                sGameManager.gm.ToggleCanvasMain(true);

                sSceneManger.sceneManagerGlobal.LoadScene(lowerworldScene, eDirection.north, Vector3.zero);

                // TO DO : each middleworld has a chunk that you can land in
            }
        }*/
    }

    void SpriteController(Vector2 _input)
    {
        // flips the sprite based on input direction
        if (_input.x == 1)
        {
            spriteRenderer.sprite = spriteMovementArray[3];
        }

        else if (_input.x == -1)
        {
            spriteRenderer.sprite = spriteMovementArray[2];
        }

        else if (_input.y == 1)
        {
            spriteRenderer.sprite = spriteMovementArray[0];
        }

        else if (_input.y == -1)
        {
            spriteRenderer.sprite = spriteMovementArray[1];
        }
    }

    void MovementPhysics()
    {
        // checks if there is any input magnitute
        if (inputVelocity.sqrMagnitude > 0.1f)
        {
            // sets total speed - leaving this for sprint/speed adjustments later
            float totalSpeed = characterSpeed;

            // converts direction to work with top down z-movement
            Vector2 movementDirection = new Vector3(inputVelocity.x, inputVelocity.y).normalized;

            // handles the side to side physics movement
            rb.linearVelocity = movementDirection * characterSpeed;

            //Debug.Log("Moving Character");
        }

        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public static void PauseMovement()
    {
        isPaused = !isPaused;
    }
}
