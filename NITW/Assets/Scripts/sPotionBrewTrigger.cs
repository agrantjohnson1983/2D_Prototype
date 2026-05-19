using UnityEngine;

public class sPotionBrewTrigger : MonoBehaviour
{
    public GameObject ui_DisplayOnTriggerEnter;

    bool isTouchingTrigger = false;

    public static bool isBrewing = false;

    sPlayer player;// { get { if (player == null) player = sPlayer.playerGlobal; return player; } set { } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ui_DisplayOnTriggerEnter.SetActive(false);
        player = sPlayer.playerGlobal;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!sCharacterController.isFlying)
        //{
            if (isTouchingTrigger && Input.GetKey(KeyCode.W) && !isBrewing)
            {
                isBrewing = true;

                player.StartPotionBrew();

                //sCharacterController.isOutside = false;

                //for (int i = 0; i < turnOnOnEnter.Length; i++)
                //{
                //    turnOnOnEnter[i].SetActive(true);
                //}

                //for (int i = 0; i < turnOffOnEnter.Length; i++)
                //{
                //    turnOffOnEnter[i].SetActive(false);
                //}

                //insideHouse.SetActive(true);
                //outsideHouse.SetActive(false);
                //outsideGround.SetActive(false);
            }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !player.CheckIfFlying())
        {
            isTouchingTrigger = true;
            ui_DisplayOnTriggerEnter.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingTrigger = false;
            ui_DisplayOnTriggerEnter.SetActive(false);
        }
    }
}
