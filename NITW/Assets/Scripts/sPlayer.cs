using UnityEngine;

public class sPlayer : MonoBehaviour
{
    public GameObject characterSideScroll;
    sCharacterControllerSideScroll characterControllerSideScroll;

    public GameObject characterFlying;
    sCharacterControllerFlyingSideToSide characterControllerFlyingSideToSide;

    public GameObject characterSignSpin;
    sSignController signController;


    public GameObject potionBrewing;
    cPotionBrewing potionBrewingController;

    public static sPlayer playerGlobal;

    public GameObject pong;
    sPongManager pongManager;

    public GameObject level;

    public GameObject cameraMain;

    //bool isOnBus = false;

    GameObject activeMovementObject = null;

    //GameObject[] childrenObjects; 

    Rigidbody2D [] movementRBs;

    uTextCharacter textPopup;

    

    private void Awake()
    {
        if (playerGlobal == null)
        {
            playerGlobal = this;
            DontDestroyOnLoad(this.gameObject);
        }
            
        else
            Destroy(this.gameObject);

        textPopup = GetComponentInChildren<uTextCharacter>();

        // Gets RB refs in children - TRUE is set to get the inactive ones
        movementRBs = GetComponentsInChildren<Rigidbody2D>(true);
    }



    // TO - DO - Convert scripts to have methods for stopping and initing without turning on/off game objects


    // GIGGING - This controls a lot of the different gameplay

    public void StartGig(SO_Gig _gig)
    {
        switch (_gig.gigType)
        {
            case eGigType.signSpin:

                // toggles side scroll off and sign spin on
                characterSideScroll.SetActive(false);
                characterSignSpin.SetActive(true);

                // sets active object
                activeMovementObject = characterSignSpin.gameObject;

                // sets sign spin pos same as side scroll
                SetPosition(characterSideScroll.transform.position);

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

                activeMovementObject = characterSideScroll;

                break;
        }
    }

    // POTION BREWING

    public void StartPotionBrew()
    {
        characterSideScroll.SetActive(false);
        potionBrewing.SetActive(true);

        activeMovementObject = potionBrewing;
    }

    public void StopPotionBrew()
    {
        potionBrewing.SetActive(false);
        characterSideScroll.SetActive(true);

        activeMovementObject = characterSideScroll;
    }

    // VIDEO GAMES

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
                activeMovementObject = pong;

                break;
        }
    }

    public void EndVideoGame(eVideoGames _game)
    {
        cameraMain.SetActive(true);

        level.SetActive(true);

        sGameManager.gm.canvasMain.SetActive(true);

        characterSideScroll.gameObject.SetActive(true);

        activeMovementObject = characterSideScroll;

        //sCharacterController.characterControllerGlobal.SetCanMove(true);
    }


    // UTITLIES

    // Returns a reference to the active movement object
    public GameObject GetActiveMovementObject()
    {
        return activeMovementObject;
    }

    // Use this to set a reference to the active movement object
    public void SetActiveMovementObject(GameObject _go)
    {
        Debug.Log("Active movement object set to " + _go);

        activeMovementObject = _go;
    }

    // Sets player pos and resets all controller objects pos to zero
    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;

        foreach (Transform child in transform)
        {
            GameObject childGO = child.gameObject;
            childGO.transform.localPosition = Vector3.zero;
        }
    }

    // This toggles the flying on/off
    public void ToggleFlying(bool _canFly)
    {
        //Debug.Log("Toggling flying to " + _canFly);

        // Flying was switched so the position needs to be set to the side scroll
        if(_canFly)
        {
            SetPosition(characterSideScroll.transform.position);
            SetActiveMovementObject(characterFlying);
        }

        // Switching from flying back to walking so set pos to flying pos;
        else
        {
            SetPosition(characterFlying.transform.position);
            SetActiveMovementObject(characterSideScroll);
        }

        //sCharacterControllerBASE.isFlying = _canFly;

        characterSideScroll.SetActive(!_canFly);
        characterFlying.SetActive(_canFly);
    }

    // This toggle off movement inputs and stops velocity
    public void ToggleMovement(bool _canMove)
    {
        // turns off movement for character controller
        sCharacterControllerBASE.canMove = _canMove;

        // checks if movment is turned off
        if(_canMove == false)
        {
            // iterates through movement rb array
            foreach(Rigidbody2D _rb in movementRBs)
            {
                // sets velocity to zero
                _rb.linearVelocity = Vector3.zero;
            }
        }
    }

    // Use this for changing transform for text popup to current controller
    public void SetCurrentController(GameObject currentControllerObject)
    {
        if (textPopup != null)
        {
            textPopup.SetTransform(currentControllerObject.transform);
        }

        else
        {
            Debug.LogError("No text popup found!");
        }
    }

    public void DisplayText(string _text, float _duration)
    {
        if(textPopup != null)
        {
            textPopup.SetText(_text, _duration);
        }

        else
        {
            Debug.LogError("No text popup found!");
        }
    }
}

