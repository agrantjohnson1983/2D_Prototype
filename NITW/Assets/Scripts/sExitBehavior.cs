using UnityEngine;

public class sExitBehavior : MonoBehaviour
{
    public Transform exitTransform;

    public bool goesOutside;

    public GameObject[] turnOffOnTrigger;
    public GameObject[] turnOnOnTrigger;

    sPlayer player;// { get { if (player == null) player = sPlayer.playerGlobal; return player; } set { } }

    private void Start()
    {
        player = sPlayer.playerGlobal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Toggles if outside
        player.ToggleOutside(goesOutside);

        // Sets exit position
        player.SetPosition(exitTransform.position);

        // turns parallax back on
        sParallaxingBackground.canParralax = true;

        // Turns on stuff - Do this first so you don't accidently turn this off before turning stuff on
        for (int i = 0; i < turnOnOnTrigger.Length; i++)
        {
            turnOnOnTrigger[i].SetActive(true);
        }

        // Turns off stuff - scales to zero
        for (int i = 0; i < turnOffOnTrigger.Length; i++)
        {
            turnOffOnTrigger[i].SetActive(false);
        }
    }
}
