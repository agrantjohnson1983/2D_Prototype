using UnityEngine;
using UnityEngine.SceneManagement;

public class sSceneManger : MonoBehaviour
{
    public static sSceneManger sceneManagerGlobal;

    public static Vector3 loadPos;

    private void Awake()
    {
        if (sceneManagerGlobal == null)
            sceneManagerGlobal = this;
        else
            Destroy(sceneManagerGlobal.gameObject);

        //loadPos = new Vector3();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string _sceneToTransitionTo, eDirection _directionFacing, Vector3 _loadPosOffset)
    {
        //Debug.Log("Load pos offset is " + _loadPosOffset);

        // Sets new direction
        //cCompass.compassGlobal.SetDirection(_directionFacing);

        // sets load position
        loadPos = _loadPosOffset;

        //Debug.Log("Load pos is set to:" + loadPos);

        // Loads scene
        SceneManager.LoadScene(_sceneToTransitionTo);
    }

    // This method is called whenever a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        Debug.Log("Scene Loaded - load pos is: " + loadPos);

        if(loadPos != null)
            sPlayer.playerGlobal.SetPosition(loadPos);

        // clears text
        sPlayer.playerGlobal.DisplayText("", 0f);

        // Checks if player was on bus during scene change
        if (sBusStop.isOnBus)
        {
            //Debug.Log("Player was on bus from scene loaded - triggering exit");

            // triggering bus stop exit if player was on busdd
            sBusStop.busStopGlobal.ExitBusStop();
        }

        // turns off dialogue box
        sGameManager.gm.ToggleDialoge(false);

        switch(sGameManager.gm.gameMode)
        {
            case eMode.topdown:

                sGameManager.gm.ToggleOverworld(true);

                goto case eMode.frontend;
                

            case eMode.frontend:

                sGameManager.gm.ToggleCanvasMain(false);

                break;

            case eMode.sidescroll:

                sGameManager.gm.ToggleCanvasMain(true);

                break;

            case eMode.dungeon:

                break;
        }
    }
}
