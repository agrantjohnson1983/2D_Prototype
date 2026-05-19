using UnityEngine;
using UnityEngine.SceneManagement;

public class sCorner : MonoBehaviour
{
    public GameObject ui_Display;

    bool isTouchingDoor = false;

    public string sceneToTransitionTo;

    public eDirection directionToTurn;

    public Vector3 loadingOffset;

    sPlayer player;// { get { if (player == null) player = sPlayer.playerGlobal; return player; } set { player = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ui_Display.SetActive(false);
        player = sPlayer.playerGlobal;
    }

    // Update is called once per frame
    void Update()
    {
        // returns if you aren't outside
        if (!player.CheckIfOutside()) return;

        // checks if you are touching and pressed W
        if (isTouchingDoor && Input.GetKey(KeyCode.W))
        {
            sSceneManger.sceneManagerGlobal.LoadScene(sceneToTransitionTo, directionToTurn, loadingOffset);

                
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && player.CheckIfOutside())
        {
            isTouchingDoor = true;
            ui_Display.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingDoor = false;
            ui_Display.SetActive(false);
        }
    }
}
