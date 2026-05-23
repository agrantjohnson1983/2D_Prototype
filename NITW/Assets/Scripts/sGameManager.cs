using UnityEngine;
using UnityEngine.EventSystems;

public enum eMode { sidescroll, topdownLow, topdownHigh, frontend, dungeon }

public class sGameManager : MonoBehaviour
{
    public static sGameManager gm;

    public eMode gameMode;

    public GameObject canvasMain, canvasDialogue,
        inventoryUI, moneyUI, energyUI, timeOfDayUI, locationUI, compassUI, phoneUI, HUD_Main;

    public cInventory inventory;

    public cMoney money;

    public cEnergy energy;

    public TimeOfDay timeOfDay;

    public cLocation location;

    public cCompass compass;

    public cPhone phone;

    //public sPlayer player;// { get { if (player == null) player = sPlayer.playerGlobal; return player; } set { player = value; } }

    sPlayer _player;

    sPlayer player
    {
        get
        {
            if (_player == null)
            { _player = sPlayer.playerGlobal; }
            return _player;
        }
        set { _player = value; }
    }

    public EventSystem eventSystem;

    private void Awake()
    {

        // Persistant Singleton setup
        if (gm != null)
        {
            Destroy(this);
        }

        else
        {
            gm = this;
            DontDestroyOnLoad(gm.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = sPlayer.playerGlobal;

        if(gameMode == eMode.frontend)
        {
            player.gameObject.SetActive(false);
        }

        // hides cursor
        Cursor.visible = false;
    }

    public void StartGame()
    {
        SetGameMode(eMode.sidescroll);

        //ToggleCanvasMain(true);

        player.gameObject.SetActive(true);

        player.characterSideScroll.SetActive(true);
    }

    // This returns the current game mode
    public eMode GetGameMode()
    {
        return gameMode;
    }

    // This gets called to change game mode
    public void SetGameMode(eMode _mode)
    {
        Debug.Log("GameManager setting mode to: " + _mode);

        gameMode = _mode;
    }

    public void ToggleOverworld(bool _isInOverworld)
    {
        ToggleCanvasMain(!_isInOverworld);
    }

    public void ToggleDungeonCanvas(bool _dungeonModeOn)
    {
        // turns off broom energy in dungeon mode
        //energy.enabled = !_dungeonModeOn;
        energyUI.SetActive(!_dungeonModeOn);

        // turns off phone in dungeon mode
        //phone.enabled = !_dungeonModeOn;
        phoneUI.gameObject.SetActive(!_dungeonModeOn);

        // turns off location when in dungeon mode
        //location.enabled = !_dungeonModeOn;
        locationUI.gameObject.SetActive(!_dungeonModeOn);

        if (_dungeonModeOn)
            SetGameMode(eMode.dungeon);
    }

    public void ToggleCanvasMain(bool _isOn)
    {
        canvasMain.SetActive(_isOn);
    }

    public void ToggleDialoge(bool _isOn)
    {
        if(player == null )
        {
            Debug.LogWarning("Player ref is null");
            return;
        }

        // turns off player movement
        player.ToggleMovement(!_isOn);

        // turns off main canvas when dialogue mode is on
        canvasMain.SetActive(!_isOn);

        // turns dialogue canvas on when toggled on
        canvasDialogue.SetActive(_isOn);
    }

    public void SetEventSystem(GameObject _objectToSet)
    {
        eventSystem.SetSelectedGameObject(_objectToSet);
    }

    public void OnSceneLoad()
    {
        Debug.Log("GameManager called on scene load with mode of: " + gameMode);

        // turns off dialogue box
        ToggleDialoge(false);

        // runs game mode to toggle canvas
        switch (gameMode)
        {
            case eMode.topdownLow:

                // toggles overworld
                ToggleOverworld(true);

                // set player
                player.ToggleOverworldLow(true);

                //player.ToggleOverworldHigh(false);

                break;

            case eMode.topdownHigh:

                player.ToggleOverworldHigh(true);

                break;

            case eMode.frontend:

                ToggleCanvasMain(false);

                break;

            case eMode.sidescroll:

                ToggleCanvasMain(true);

                break;

            case eMode.dungeon:

                ToggleDungeonCanvas(true);

                break;
        }
    }
}
