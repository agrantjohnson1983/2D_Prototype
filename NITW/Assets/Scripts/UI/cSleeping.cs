using UnityEngine;

public class cSleeping : MonoBehaviour
{
    public GameObject yesButton;

    public void OnEnable()
    {
        sPlayer.playerGlobal.ToggleMovement(false);

        sGameManager.gm.SetEventSystem(yesButton);
    }

    public void OnClickYes()
    {
        //Debug.Log("Yes to sleeep clicked");

        // triggers sleep
        sPlayer.playerGlobal.TriggerSleep();

        // turns off gameobject
        this.gameObject.SetActive(false);
    }

    public void OnClickNo()
    {
        // turns player back on
        sPlayer.playerGlobal.ToggleMovement(true);

        // turns this off
        this.gameObject.SetActive(false);
    }
}
