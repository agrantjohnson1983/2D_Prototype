using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class cSleeping : MonoBehaviour
{
    public GameObject yesButton;

    public GameObject canvas;

    public Image blackBG;

    public float sleepFadeTime = 2f;

    public void OnEnable()
    {
        sPlayer.playerGlobal.ToggleMovement(false);

        sGameManager.gm.SetEventSystem(yesButton);
    }

    public void OnClickYes()
    {
        //Debug.Log("Yes to sleeep clicked");

        // background fade
        StartCoroutine(BackgroundFade());
    }

    public void OnClickNo()
    {
        // turns player back on
        sPlayer.playerGlobal.ToggleMovement(true);

        // turns this off
        this.gameObject.SetActive(false);
    }

    IEnumerator BackgroundFade()
    {
        canvas.SetActive(false);

        float counter = 0f;

        Color tempColor = Color.black;

        // fade in to black from transparent
        while (counter < sleepFadeTime)
        {
            tempColor.a = Mathf.Lerp(0, 1, (counter / sleepFadeTime));

            blackBG.color = tempColor;

            counter += Time.deltaTime;

            yield return null;
        }

        // triggers sleep
        sPlayer.playerGlobal.TriggerSleep();

        // small pause between fade
        yield return new WaitForSeconds(0.5f);

        // resets counter
        counter = 0f;

        // fade from black
        while (counter < sleepFadeTime)
        {
            tempColor.a = Mathf.Lerp(1, 0, (counter / sleepFadeTime));

            blackBG.color = tempColor;

            counter += Time.deltaTime;

            yield return null;
        }

        // turns canvas back on
        canvas.SetActive(true);

        // turns off gameobject
        this.gameObject.SetActive(false);
    }
}
