using System.Collections;
using UnityEngine;

public class sPlayer : MonoBehaviour
{
    public GameObject characterSideScroll;
    sCharacterControllerSideScroll characterControllerSideScroll;

    public GameObject characterFlying;
    sCharacterControllerFlyingSideToSide characterControllerFlyingSideToSide;

    public GameObject characterOverworldLow;

    public GameObject characterOverworldHigh;

    public GameObject characterSignSpin;
    sSignController signController;

    public GameObject characterDungeonCrawler;
    sCharacterDungeonCrawl dungeonCrawler;

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

    bool canSwitchState;

    public bool startOutside = true;

    bool isOutside;

    bool isFlying;

    public static bool canFly = false;

    public SO_Item broomItem;

    public float stateSwitchCooldownTime = 2.5f;

    private void Awake()
    {
        if (playerGlobal == null)
        {
            playerGlobal = this;
            DontDestroyOnLoad(this.gameObject);
        }
            
        else
            Destroy(this.gameObject);

        // Gets RB refs in children - TRUE is set to get the inactive ones
        movementRBs = GetComponentsInChildren<Rigidbody2D>(true);

        // sets outside state
        ToggleOutside(startOutside);

        // quick cooldown for state switch
        StartCoroutine(StateSwitchCooldown(0.5f));
    }

    private void OnEnable()
    {
        broomItem.onGrabEvent.AddListener(OnBroomGrab);
    }

    private void OnDisable()
    {
        broomItem.onGrabEvent.RemoveListener(OnBroomGrab);
    }

    private void Start()
    {
        textPopup = GetComponentInChildren<uTextCharacter>();
    }

    private void Update()
    {
        if(canFly)
            CheckFlyingToggleInput();
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
        //Debug.Log("Active movement object set to " + _go);

        activeMovementObject = _go;

        if (textPopup != null)
        {
            textPopup.SetTransform(activeMovementObject.transform);
        }

        else
        {
            textPopup = GetComponentInChildren<uTextCharacter>();

            if(textPopup == null)
                Debug.LogError("No text popup found!");
        }
    }

    // Sets player pos and resets all controller objects pos to zero
    public void SetPosition(Vector2 pos)
    {
        //Debug.Log("Setting pos to: " + pos);

        this.transform.position = pos;

        foreach (Transform child in transform)
        {
            GameObject childGO = child.gameObject;
            childGO.transform.localPosition = Vector3.zero;
        }
    }

    IEnumerator StateSwitchCooldown(float _time)
    {
        //Debug.Log("Starting state switch cooldown");

        yield return new WaitForSeconds(_time);

        canSwitchState = true;
    }

    // returns a bool whether player is outside or not
    public bool CheckIfOutside()
    {
        return isOutside;
    }

    // toggles the player being outside
    public void ToggleOutside(bool _isOutside)
    {
        isOutside = _isOutside;
    }


    void CheckFlyingToggleInput()
    {
        // returns if you are inside and not flying 
        if (!isFlying && !isOutside) return;

        if (Input.GetKeyDown(KeyCode.F) && canSwitchState)
        {
            Debug.Log("Input detected and can switch state");

            isFlying = !isFlying;

            // stops from spamming - needs cooldown
            canSwitchState = false;

            // Switches state with player - this should turn off this gameObject
            ToggleFlying(isFlying);
        }
    }

    // Checks if player is flying
    public bool CheckIfFlying()
    {
        return isFlying;
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

        isFlying = _canFly;
        StartCoroutine(StateSwitchCooldown(2f));

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

    // Handles player switching between dungeon mode and non-dungeon modes
    public void ToggleDungeon(bool _isInDungeonMode)
    {
        activeMovementObject.SetActive(!_isInDungeonMode);
        characterDungeonCrawler.SetActive(_isInDungeonMode);
        characterSideScroll.SetActive(!_isInDungeonMode);

        // resets the text
        DisplayText("", 0f);
    }

    // Use this for changing transform for text popup to current controller
    /*public void SetCurrentController(GameObject currentControllerObject)
    {
        if (textPopup != null)
        {
            textPopup.SetTransform(currentControllerObject.transform);
        }

        else
        {
            Debug.LogError("No text popup found!");
        }
    }*/

    // This gets called for transitions between the side to side and overworld
    public void ToggleOverworldLow(bool _isInOverworld)
    {
        Debug.Log("Overworld toggled to: " + _isInOverworld);

        characterFlying.SetActive(!_isInOverworld);
        characterOverworldLow.SetActive(_isInOverworld);
    }

    // This gets called bewteen the overworld and higher overworld
    public void ToggleOverworldHigh(bool _isInOverworld)
    {
        characterOverworldLow.SetActive(!_isInOverworld);
        characterOverworldHigh.SetActive(_isInOverworld);
    }

    public void TriggerSleep()
    {
        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        // TO-DO move character sprite into bed somehow

        // turns movement on
        ToggleMovement(false);

        // toggles recovery on
        cEnergy.energyGlobal.ToggleRecovery(true);

        // displays some yawn text
        DisplayText("Yawn...", 4f);

        // while energy is not full return
        while (!cEnergy.energyGlobal.CheckIfEnergyIsFull())
        {
            yield return null;
        }

        // done sleeping
        Debug.Log("Done sleeping");

        // TO DO - Character sprite gets out of bed here

        // displays rested text
        DisplayText("Ok I feel rested now!", 4f);


        // turns recovery state off in energy
        cEnergy.energyGlobal.ToggleRecovery(false);

        // turns movement back on 
        ToggleMovement(true);
    }

    void OnBroomGrab()
    {
        canFly = true;
    }

    public void DisplayText(string _text, float _duration)
    {
        if(textPopup != null)
        {
            textPopup.SetText(_text, _duration);
        }

        else
        {
            textPopup = GetComponentInChildren<uTextCharacter>();

            if(textPopup == null)
            Debug.LogError("No text popup found!");
        }
    }

    public void StopText()
    {
        textPopup.StopAllCoroutines();
    }
}

