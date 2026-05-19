using UnityEngine;

public class cSleeping : MonoBehaviour
{
    public void OnClickYes()
    {
        Debug.Log("Yes to sleeep clicked");

        // triggers sleep
        sPlayer.playerGlobal.TriggerSleep();

        // turns off gameobject
        this.gameObject.SetActive(false);
    }

    public void OnClickNo()
    {
        this.gameObject.SetActive(false);
    }
}
