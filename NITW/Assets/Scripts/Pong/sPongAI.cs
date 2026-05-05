using UnityEngine;

public class sPongAI : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 5f;
    public float boundaryY = 4f;
    public float reactionDistance = 8f; // AI only reacts when ball is within this X distance

    private Rigidbody2D rb;
    private Transform ball;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ball = GameObject.FindGameObjectWithTag("Projectile")?.transform;
    }

    void FixedUpdate()
    {
        if (ball == null) return;

        // Only track ball if it's on the AI's side or close enough
        float ballX = ball.position.x;
        if (Mathf.Abs(ballX - transform.position.x) > reactionDistance)
        {
            // Drift back to center slowly
            MoveToward(0f, speed * 0.5f);
            return;
        }

        MoveToward(ball.position.y, speed);
    }

    void MoveToward(float targetY, float moveSpeed)
    {
        float direction = Mathf.Sign(targetY - transform.position.y);
        float distance = Mathf.Abs(targetY - transform.position.y);

        // Dead zone to avoid jitter
        if (distance < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(0, direction * moveSpeed);

        float clampedY = Mathf.Clamp(transform.position.y, -boundaryY, boundaryY);
        transform.position = new Vector3(transform.position.x, clampedY, 0);
    }
}
