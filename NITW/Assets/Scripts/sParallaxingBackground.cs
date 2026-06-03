using UnityEngine;
using UnityEngine.SceneManagement;

public class sParallaxingBackground : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool infiniteLoopVertical = true;
    [SerializeField] private bool infiniteLoopHorizontal = false;

    Vector3 lastCamPos;
    float textureUnitSizeX;
    float textureUnitySizeY;
    Vector3 startingOffset;

    public static bool canParralax = true;

    bool initialized = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Fires after every scene load — snaps the baseline cleanly
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnsureCamera();
        lastCamPos = camTransform.position;
        startingOffset = transform.position;
    }

    private void Awake()
    {
        // Move texture sizing here so it's ready before Start/ReInitialize
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            textureUnitSizeX = sr.sprite.texture.width / sr.sprite.pixelsPerUnit;
            textureUnitySizeY = sr.sprite.texture.height / sr.sprite.pixelsPerUnit;
        }
    }

    void Start()
    {
        EnsureCamera();
        if (!initialized)   // don't overwrite if ReInitialize already ran
        {
            lastCamPos = camTransform.position;
            startingOffset = transform.position;
        }
    }

    private void EnsureCamera()
    {
        if (camTransform == null)
            camTransform = Camera.main.transform;
    }

    public void ReInitialize()
    {
        EnsureCamera();
        lastCamPos = camTransform.position;
        startingOffset = transform.position;
        initialized = true;
    }

    private void LateUpdate()
    {
        // Original behavior preserved — building transport still works
        if (!canParralax) return;

        Vector3 deltaMovement = camTransform.position - lastCamPos;
        transform.position += new Vector3(
            deltaMovement.x * parallaxEffectMultiplier.x,
            deltaMovement.y * parallaxEffectMultiplier.y, 0f);
        lastCamPos = camTransform.position;

        if (infiniteLoopHorizontal)
        {
            if (Mathf.Abs(camTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetPosX = (camTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(
                    camTransform.position.x - offsetPosX + startingOffset.x,
                    transform.position.y, 0f);
            }
        }

        if (infiniteLoopVertical)
        {
            if (Mathf.Abs(camTransform.position.y - transform.position.y) >= textureUnitySizeY)
            {
                float offsetPosY = (camTransform.position.y - transform.position.y) % textureUnitySizeY;
                transform.position = new Vector3(
                    transform.position.x,
                    camTransform.position.y - offsetPosY + startingOffset.y, 0f);
            }
        }
    }

    public void ResetX() =>
        transform.position = new Vector3(startingOffset.x, transform.position.y, 0f);

    public void ResetY() =>
        transform.position = new Vector3(transform.position.x, startingOffset.y, 0f);
}