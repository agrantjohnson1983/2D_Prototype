using UnityEngine;

public class sExitBehavior : MonoBehaviour
{
    //public GameObject outside;
    //public GameObject inside;
    //public GameObject outsideGround;

    public Transform exitTransform;

    public bool goesOutside;

    public GameObject[] turnOffOnTrigger;
    public GameObject[] turnOnOnTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sCharacterController.isOutside = goesOutside;
        sCharacterController.characterControllerGlobal.SetLocation(exitTransform.position);

        // Turns on stuff - Do this first so you don't accidently turn this off before turning stuff on
        for (int i = 0; i < turnOnOnTrigger.Length; i++)
        {
            turnOnOnTrigger[i].SetActive(true);
        }

        // Turns off stuff
        for (int i = 0; i < turnOffOnTrigger.Length; i++)
        {
            turnOffOnTrigger[i].SetActive(false);
        }

        //outsideGround.SetActive(true);
        //outside.SetActive(true);
        //inside.SetActive(false);
    }
}
