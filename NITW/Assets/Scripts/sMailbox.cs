using UnityEngine;

public class sMailbox : MonoBehaviour
{
    bool isFilled = false;

    public Sprite mailboxFilled;

    SpriteRenderer spriteRenderer;

    public float deliveryRewardAmount = 10.50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject + " triggered mailbox trigger");

        if (collision.TryGetComponent<sProjectile>(out sProjectile _projectile) && !isFilled)
        {
            isFilled = true;

            //Debug.Log("Projectile Triggered Mailbox");

            Destroy(_projectile.gameObject);

            spriteRenderer.sprite = mailboxFilled;

            cMoney.moneyGlobal.GetMoney(deliveryRewardAmount);
        }
    }
}
