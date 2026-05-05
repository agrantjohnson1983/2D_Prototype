using UnityEngine;

public class sPongPlayerPaddle : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 8f;
    public float boundaryY = 4f; // How far up/down the paddle can move

    [Header("Input")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float input = 0f;

        if (Input.GetKey(upKey)) input = 1f;
        if (Input.GetKey(downKey)) input = -1f;

        rb.linearVelocity = new Vector2(0, input * speed);

        // Clamp position within boundaries
        float clampedY = Mathf.Clamp(transform.position.y, -boundaryY, boundaryY);
        transform.position = new Vector3(transform.position.x, clampedY, 0);
    }
}
