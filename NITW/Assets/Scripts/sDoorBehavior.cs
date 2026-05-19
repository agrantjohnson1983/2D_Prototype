using UnityEngine;
using UnityEngine.SceneManagement;

public class sDoorBehavior : MonoBehaviour
{
    public GameObject uDoorArrow;

    public GameObject[] turnOffOnEnter, turnOnOnEnter;

    bool isTouchingDoor = false;

    sPlayer player;// { get { if (player == null) player = sPlayer.playerGlobal; return player; } set { player = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // sets ui to zero
        uDoorArrow.transform.localScale = Vector3.zero;

        // quick init for buildings - turns on and then scales to zero
        for (int i = 0; i < turnOnOnEnter.Length; i++)
        {
            turnOnOnEnter[i].gameObject.SetActive(true);

            turnOnOnEnter[i].gameObject.SetActive(false);
        }

        player = sPlayer.playerGlobal;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        if (!player.CheckIfOutside() || player.CheckIfFlying()) return;

        // if you are touching door and press 'W'
        if (isTouchingDoor && Input.GetKey(KeyCode.W))
        {
            // tells player script you are inside
            player.ToggleOutside(false);

            // turns on objects on enter
            for (int i = 0; i < turnOnOnEnter.Length; i++)
            {
                turnOnOnEnter[i].SetActive(true);
            }

            // turns off objects on enter
            for (int i = 0; i < turnOffOnEnter.Length; i++)
            {
                turnOffOnEnter[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // returns if you are not outside
        if (!player.CheckIfOutside()) return;

        if (collision.CompareTag("Player"))
        {
            // toggles if you are touching
            isTouchingDoor = true;

            // sets ui
            uDoorArrow.transform.localScale = Vector3.one;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // toggles if you are touching
            isTouchingDoor = false;

            // sets ui
            uDoorArrow.transform.localScale = Vector3.zero;
        }
    }
}
