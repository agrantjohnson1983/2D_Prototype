using UnityEngine;

public class sExitBehavior : MonoBehaviour
{
    public Transform exitTransform;

    public bool goesOutside;

    public GameObject[] turnOffOnTrigger;
    public GameObject[] turnOnOnTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Toggles if outside
        sCharacterControllerBASE.isOutside = goesOutside;

        // Sets exit position
        sPlayer.playerGlobal.SetPosition(exitTransform.position);

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
