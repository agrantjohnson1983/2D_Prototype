using UnityEngine;

public enum eMode { sidescroll, topdown, frontend }

public class sGameManager : MonoBehaviour
{
    public static sGameManager gm;

    public eMode sceneMode;

    public GameObject canvasMain, canvasDialogue;

    public cInventory inventory;

    public cMoney money;

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

    public void ToggleCanvasMain(bool _isOn)
    {
        canvasMain.SetActive(_isOn);
    }
}
