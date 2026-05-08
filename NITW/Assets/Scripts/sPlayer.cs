using UnityEngine;

public class sPlayer : MonoBehaviour
{
    public GameObject characterSideScroll;
    public GameObject characterSignSpin;
    public GameObject potionBrewing;

    public static sPlayer playerGlobal;

    public GameObject pong;

    public GameObject level;

    public GameObject cameraMain;

    //bool isOnBus = false;

    private void Awake()
    {
        if (playerGlobal == null)
        {
            playerGlobal = this;
            DontDestroyOnLoad(this.gameObject);
        }
            
        else
            Destroy(this.gameObject);

       
    }

    // TO - DO - Convert scripts to have methods for stopping and initing without turning on/off game objects


    // GIGGING - This controls a lot of the different gameplay

    public void StartGig(SO_Gig _gig)
    {
        switch (_gig.gigType)
        {
            case eGigType.signSpin:

                characterSideScroll.SetActive(false);
                characterSignSpin.SetActive(true);

                break;
        }
    }

    // Gets called at the end of every gig and handles canvas, payment
    public void EndGig(SO_Gig _gig)
    {
        // switches back to non-gig state
        switch (_gig.gigType)
        {
            case eGigType.signSpin:

                characterSideScroll.SetActive(true);
                characterSignSpin.SetActive(false);

                break;
        }
    }

    // POTION BREWING

    public void StartPotionBrew()
    {
        characterSideScroll.SetActive(false);
        potionBrewing.SetActive(true);
    }

    public void StopPotionBrew()
    {
        potionBrewing.SetActive(false);
        characterSideScroll.SetActive(true);
    }

    public void ResetPositions(Vector3 _newPos)
    {
        //Debug.Log("Resetting player children and setting parent object to " + _newPos.ToString());
        this.transform.position = _newPos;
        characterSideScroll.transform.localPosition = Vector3.zero;
        characterSignSpin.transform.localPosition = Vector3.zero;
    }

    public void PlayVideoGame(eVideoGames _game)
    {
        level.SetActive(false);

        sGameManager.gm.canvasMain.SetActive(false);

        characterSideScroll.gameObject.SetActive(false);

        //sCharacterController.characterControllerGlobal.SetCanMove(false);

        cameraMain.SetActive(false);

        switch (_game)
        {
            case eVideoGames.none:

                break;


            case eVideoGames.pong:

                pong.gameObject.SetActive(true);

                break;
        }
    }

    public void EndVideoGame(eVideoGames _game)
    {
        cameraMain.SetActive(true);

        level.SetActive(true);

        sGameManager.gm.canvasMain.SetActive(true);

        characterSideScroll.gameObject.SetActive(true);

        //sCharacterController.characterControllerGlobal.SetCanMove(true);
    }
}

