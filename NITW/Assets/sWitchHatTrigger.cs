using UnityEngine;
using System.Collections;

public class sWitchHatTrigger : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public GameObject broom;

    public float playerMoveDelay;

    public float playerMoveTime;

    public Vector3 movementOffset;

    public GameObject sceneChangeTrigger;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
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

        float counter = 0f;

        sPlayer.playerGlobal.ToggleFlying(true);

        while (counter < playerMoveTime)
        {

            sPlayer.playerGlobal.transform.position = Vector3.Lerp(sPlayer.playerGlobal.transform.position, sPlayer.playerGlobal.transform.position + movementOffset, (counter / playerMoveTime));

            counter += Time.deltaTime;

            yield return null;
        }

        sceneChangeTrigger.SetActive(true);

        sPlayer.playerGlobal.transform.position = Vector3.zero;

        Destroy(this.gameObject);
    }
}
