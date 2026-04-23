using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class sProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 3f;

    private Rigidbody2D rb;

    public GameObject imageObject;
    public float rotSpeed = 1000f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(imageObject != null)
            imageObject.transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Call immediately after Instantiate to set direction.
    /// </summary>
    public void Initialize(Vector2 direction)
    {
        direction = direction.normalized;

        // Rotate sprite to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Use physics velocity so direction is world space, not local
        rb.linearVelocity = direction * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
            Destroy(gameObject);
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player")) return;
    //        Destroy(gameObject);
    //}
}
