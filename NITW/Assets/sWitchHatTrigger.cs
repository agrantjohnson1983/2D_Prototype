using UnityEngine;
using System.Collections;

public class sWitchHatTrigger : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public GameObject broom;

    public float playerMoveDelay;

    public float playerMoveTime;

    public Vector3 movementOffset;

    public GameObject cameraFollowToggle;

    public GameObject sceneChangeTrigger;

    bool hasTriggered = false;



    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;

        if(collision.CompareTag("Player"))
        {
            hasTriggered = true;

            sPlayer.playerGlobal.DisplayText("Oooh this feels right!", 2f);

            sPlayer.playerGlobal.ToggleMovement(false);

            spriteRenderer.enabled = false;

            broom.SetActive(true);

            MovePlayer();
        }
    }

    void MovePlayer()
    {
        StartCoroutine(MovementSequence());
    }

    IEnumerator MovementSequence()
    {
        yield return new WaitForSeconds(playerMoveDelay);

        cameraFollowToggle.SetActive(true);

        float counter = 0f;

        sPlayer.playerGlobal.ToggleFlying(true);

        Vector3 startingPos = sPlayer.playerGlobal.GetActiveMovementObject().transform.position;
        Vector3 endPos = startingPos + movementOffset;

        sPlayer.playerGlobal.DisplayText("Whoa what is happening!", 2f);

        while (counter < playerMoveTime)
        {
            sPlayer.playerGlobal.GetActiveMovementObject().transform.position = Vector3.Lerp(startingPos, endPos, (counter / playerMoveTime));

            counter += Time.deltaTime;

            yield return null;
        }

        Destroy(sPlayer.playerGlobal.gameObject);

        sceneChangeTrigger.SetActive(true);

        Destroy(this.gameObject);
    }
}
