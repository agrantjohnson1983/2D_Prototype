using UnityEngine;

public class sPongBall : MonoBehaviour
{
    [Header("Settings")]
    public float initialSpeed = 5f;
    public float speedIncrement = 0.5f;
    public float maxSpeed = 15f;

    [Header("Stuck Detection")]
    public float stuckTimeout = 4.5f; // seconds without a paddle hit before resetting

    private Rigidbody2D rb;
    private float currentSpeed;
    private float timeSinceLastHit;
    private bool ballInPlay = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Launch();
    }

    void Update()
    {
        if (!ballInPlay) return;

        timeSinceLastHit += Time.deltaTime;
        if (timeSinceLastHit >= stuckTimeout)
        {
            Debug.Log("Ball stuck — resetting.");
            ResetBall();
        }
    }

    public void Launch()
    {
        currentSpeed = initialSpeed;
        timeSinceLastHit = 0f;
        ballInPlay = true;

        // Random initial direction (left or right, slight vertical angle)
        float x = Random.value > 0.5f ? 1f : -1f;
        float y = Random.Range(-0.5f, 0.5f);
        rb.linearVelocity = new Vector2(x, y).normalized * currentSpeed;
    }

    public void ResetBall()
    {
        ballInPlay = false;
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        Invoke(nameof(Launch), 1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset the stuck timer on any collision
        timeSinceLastHit = 0f;

        // Speed up on each paddle hit
        if (collision.gameObject.CompareTag("Player"))
        {
            currentSpeed = Mathf.Min(currentSpeed + speedIncrement, maxSpeed);

            // Adjust bounce angle based on where ball hits the paddle
            float hitPoint = (transform.position.y - collision.transform.position.y)
                             / collision.collider.bounds.size.y;

            Vector2 dir = new Vector2(rb.linearVelocity.x > 0 ? -1 : 1, hitPoint).normalized;
            rb.linearVelocity = dir * currentSpeed;
        }
    }
}
