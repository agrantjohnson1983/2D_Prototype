using UnityEngine;

public class sProjectileAim : MonoBehaviour
{
    [Header("Offset when mouse is to the RIGHT of player")]
    public Vector2 rightOffset = new Vector2(0.3f, 0f);

    [Header("Offset when mouse is to the LEFT of player")]
    public Vector2 leftOffset = new Vector2(-0.3f, 0f);

    [Header("Angle correction (90 if sprite points up by default)")]
    public float angleOffset = 90f;

    private Camera mainCam;
    private Transform player;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        mainCam = Camera.main;
        // The pivot's parent is the player body
        player = transform.parent;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void LateUpdate()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(mainCam.transform.position.z);
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        // Use the player's world position as the origin, not the pivot's
        Vector3 origin = player != null ? player.position : transform.position;
        Vector2 dir = ((Vector2)mouseWorld - (Vector2)origin).normalized;

        // Shift the pivot left or right depending on mouse side
        bool mouseOnRight = mouseWorld.x >= origin.x;
        transform.localPosition = mouseOnRight ? rightOffset : leftOffset;
        spriteRenderer.flipX = !mouseOnRight;
        //2spriteRenderer.flipY = mouseOnRight;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - angleOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
