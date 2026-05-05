using UnityEngine;
using UnityEngine.SceneManagement;

public class sSceneManger : MonoBehaviour
{
    public static sSceneManger sceneManagerGlobal;

    private void Awake()
    {
        if (sceneManagerGlobal == null)
            sceneManagerGlobal = this;
        else
            Destroy(sceneManagerGlobal.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string _sceneToTransitionTo, eDirection _directionFacing)
    {
        // Sets new direction
        cCompass.compassGlobal.SetDirection(_directionFacing);

        // Loads scene
        SceneManager.LoadScene(_sceneToTransitionTo);
    }

    // This method is called whenever a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        //Debug.Log("Mode: " + mode);

        // Checks if player was on bus during scene change
        if (sBusStop.isOnBus)
        {
            //Debug.Log("Player was on bus from scene loaded - triggering exit");

            // triggering bus stop exit if player was on bus
            sBusStop.busStopGlobal.ExitBusStop();
        }

        // Toggles canvas off if in Front End
        if(sGameManager.gm.sceneMode == eMode.frontend)
        {
            sGameManager.gm.ToggleCanvasMain(false);
        }

        else
        {
            sGameManager.gm.ToggleCanvasMain(true);
        }
    }
}
