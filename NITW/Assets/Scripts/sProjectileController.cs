using UnityEngine;

public class sProjectileController : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Fire Settings")]
    public float fireRate = 5f;
    public bool autoFire = true;
    public Vector3 fireOffset;
    //public float firePower = 1f;

    private float fireCooldown;
    private Camera mainCam;

    eMode gameMode;

    public float magicCostAmount = 1f;
    sCharacterDungeonCrawl dungeonCrawler;

    public SO_AudioData audioData;
    public string audioFireCue;
    AudioSource audioSource;

    void Awake()
    {
        mainCam = Camera.main;
        if (firePoint == null) firePoint = transform;
    }

    private void Start()
    {
        gameMode = sGameManager.gm.GetGameMode();
        TryGetComponent<sCharacterDungeonCrawl>(out dungeonCrawler);

        //if (audioData != null)
        //    audioData.SetupAudio();

        audioSource = GetComponent<AudioSource>();
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

        if(dungeonCrawler != null)
        {
            // if magic is at zero;
            if(dungeonCrawler.ReturnMagicAmount() <= 0f)
            {
                //Debug.Log("out of magic");
                sPlayer.playerGlobal.DisplayText("Out of magic", 3f);
                return;
            }

            else
            {
                sPlayer.playerGlobal.DisplayText("-"+magicCostAmount+" magic", 3f);
                dungeonCrawler.UseMagic(magicCostAmount);
            }
        }

        // resets mainCam if needed
        if(mainCam == null)
        {
            mainCam = Camera.main;
        }

        if (audioSource != null)
            audioData.TriggerAudio(audioFireCue, audioSource);

        Vector2 direction = new Vector2();
        Quaternion rotation = new Quaternion();

        switch (gameMode)
        {
            case eMode.topdown:

                // converts to z direction motion
                direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

                rotation = Quaternion.Euler(90, 0, 0);

                //direction = new Vector2(aimDirection.x, aimDirection.z);

                break;

            case eMode.sidescroll:

                // IMPORTANT: pass abs camera z as the z component so world position is correct
                Vector3 mouseScreen = Input.mousePosition;
                mouseScreen.z = Mathf.Abs(mainCam.transform.position.z);
                Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
                mouseWorld.z = 0f;
                direction = ((Vector2)mouseWorld - (Vector2)firePoint.position).normalized;

                rotation = Quaternion.identity;

                break;
        }

        Debug.Log("Projectile direction is " + direction);

        GameObject proj = Instantiate(projectilePrefab, firePoint.position + fireOffset, rotation);
        sProjectileBASE projectile = proj.GetComponent<sProjectileBASE>();
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
