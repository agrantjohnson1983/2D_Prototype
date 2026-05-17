using UnityEngine;

public enum eMode { sidescroll, topdown, frontend }

public class sGameManager : MonoBehaviour
{
    public static sGameManager gm;

    public eMode sceneMode;

    public GameObject canvasMain, canvasDialogue,
        inventoryUI, moneyUI, energyUI, timeOfDayUI, locationUI, compassUI, phoneUI;

    public cInventory inventory;

    public cMoney money;

    public cEnergy energy;

    public TimeOfDay timeOfDay;

    public cLocation location;

    public cCompass compass;

    public cPhone phone;

    private void Awake()
    {

        // Persistant Singleton setup
        if (sGameManager.gm != null)
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
        
    }


    // This returns the current game mode
    public eMode GetGameMode()
    {
        return sceneMode;
    }

    // This gets called to change game mode
    public void SetGameMode(eMode _sceneMode)
    {
        sceneMode = _sceneMode;
    }

    public void ToggleDungeonCanvas(bool _dungeonModeOn)
    {
        // turns off broom energy in dungeon mode
        energy.enabled = !_dungeonModeOn;
        energyUI.SetActive(!_dungeonModeOn);

        // turns off phone in dungeon mode
        phone.enabled = !_dungeonModeOn;
        phoneUI.gameObject.SetActive(!_dungeonModeOn);

        // turns off location when in dungeon mode
        location.enabled = !_dungeonModeOn;
        locationUI.gameObject.SetActive(!_dungeonModeOn);
    }

    public void ToggleCanvasMain(bool _isOn)
    {
        canvasMain.SetActive(_isOn);
    }
}
