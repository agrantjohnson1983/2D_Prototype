using UnityEngine;
using UnityEngine.ProBuilder;

public class sProjectileController : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Fire Settings")]
    public float fireRate = 5f;
    public bool autoFire = true;

    private float fireCooldown;
    private Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
        if (firePoint == null) firePoint = transform;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        bool shouldFire = autoFire ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        if (shouldFire && fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = fireRate > 0f ? 1f / fireRate : 0f;
        }
    }

    void Fire()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Shooter: No projectile prefab assigned!");
            return;
        }

        // resets mainCam if needed
        if(mainCam == null)
        {
            mainCam = Camera.main;
        }

        // IMPORTANT: pass abs camera z as the z component so world position is correct
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(mainCam.transform.position.z);
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        Vector2 direction = ((Vector2)mouseWorld - (Vector2)firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        sProjectile projectile = proj.GetComponent<sProjectile>();
        if (projectile != null)
            projectile.Initialize(direction);
    }

    void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(firePoint.position, 0.1f);
    }
}
