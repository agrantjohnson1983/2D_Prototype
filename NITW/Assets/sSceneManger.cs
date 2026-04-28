using UnityEngine;
using UnityEngine.SceneManagement;

public class sSceneManger : MonoBehaviour
{
    public static sSceneManger sceneManagerGlobal;

    public string sceneToTransitionTo;

    public eMode modeToTransitionTo;

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

    // static function that can be called anywhere
    public void ChangeScene(string _sceneToTransitionTo)
    {
        // Debug.Log("Scene Transition called to " + _sceneToTransitionTo);
        SceneManager.LoadScene(_sceneToTransitionTo);
    }

    // This method is called whenever a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        //Debug.Log("Mode: " + mode);

        if (sBusStop.isOnBus)
        {
            Debug.Log("Player was on bus from scene loaded - triggering exit");

            // triggering bus stop exit
            sBusStop.busStopGlobal.ExitBusStop();
        }
    }
}
