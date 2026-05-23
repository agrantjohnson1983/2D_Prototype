using Unity.VisualScripting;
using UnityEngine;

public class sParallaxingBackground : MonoBehaviour
{
    /*float length, startPos;

    public GameObject cam;

    public float parallaxEffect;*/

    [SerializeField] private Transform camTransform;

    [SerializeField] private Vector2 parallaxEffectMultiplier;

    [SerializeField] private bool infiniteLoopVertical = true;
    [SerializeField] private bool infiniteLoopHorizontal = false;

    Vector3 lastCamPos;
    float textureUnitSizeX;
    float textureUnitySizeY;

    Vector3 startingOffset;

    public static bool canParralax = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*startPos = transform.localPosition.x;

        length = GetComponent<SpriteRenderer>().bounds.size.x;*/


        startingOffset = new Vector2(transform.localPosition.x, transform.localPosition.y);

        if (camTransform == null)
        {
            camTransform = Camera.main.transform;
        }

        lastCamPos = camTransform.position;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if(spriteRenderer != null)
        {
            Sprite sprite = spriteRenderer.sprite;
            Texture2D texture = sprite.texture;
            textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
            textureUnitySizeY = texture.height / sprite.pixelsPerUnit;
        }
    }

    private void FixedUpdate()
    {
        if (!canParralax) return;

        Vector3 deltaMovement = camTransform.position - lastCamPos;

        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y, 0f);
        lastCamPos = camTransform.position;

        if(infiniteLoopHorizontal)
        {
            if(Mathf.Abs(camTransform.position.x - transform.position.x) >= textureUnitSizeX )
            {
                float offsetPosX = (camTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(camTransform.position.x - offsetPosX + startingOffset.x, transform.position.y);
            }
        }

        if(infiniteLoopVertical)
        {
            if(Mathf.Abs(camTransform.position.y - transform.position.y) >= textureUnitySizeY)
            {
                float offsetPosY = (camTransform.position.y - transform.position.y) % textureUnitySizeY;
                transform.position = new Vector3(transform.position.x, camTransform.position.y - offsetPosY + startingOffset.y);
            }
        }
    }

    private void LateUpdate()
    {
        /*float temp = (cam.transform.position.x * (1 - parallaxEffect));

        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if(temp > startPos + length)
        {
            startPos += length;
        }

        else if(temp < startPos - length)
        {
            startPos -= length;
        }*/
    }
}
