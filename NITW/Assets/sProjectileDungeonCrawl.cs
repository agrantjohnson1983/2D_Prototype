using UnityEngine;

public class sProjectileDungeonCrawl : sProjectileBASE
{
    public float speed = 15f;
    public float lifetime = 3f;

    public int damageAmount = 5;

    private Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;
    //public float rotSpeed = 1000f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        /*if (imageObject != null)
        {
            imageObject.transform.Rotate(0, 0, rotSpeed * Time.fixedDeltaTime);
        }*/
    }

    /// <summary>
    /// Call immediately after Instantiate to set direction.
    /// </summary>
    public override void Initialize(Vector3 direction)
    {
        direction = direction.normalized;

        //// WILL PROBABALY NEED TO CHANGE FLIP Y to FLIP X with asset swap
        //if(direction.x >= 0)
        //{
        //    spriteRenderer.flipY = false;
        //}

        //else
        //{
        //    spriteRenderer.flipY = true;
        //}

            // Rotate sprite to face direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Use physics velocity so direction is world space, not local
        rb.linearVelocity = direction * speed;

        //Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<sDungeonEnemy>(out sDungeonEnemy _enemy))
        {
            _enemy.TakeDamage(damageAmount);
            Destroy(this.gameObject);
        }
    }
}
