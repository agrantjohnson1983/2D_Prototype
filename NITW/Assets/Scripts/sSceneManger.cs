using UnityEngine;
using UnityEngine.SceneManagement;

public class sSceneManger : MonoBehaviour
{
    public static sSceneManger sceneManagerGlobal;

    sGameManager gm;

    sPlayer _player;

    sPlayer player { get { if (_player == null)
                        { _player = sPlayer.playerGlobal; } 
                         return _player; } 
                    set { _player = value; } }

    public static Vector2 loadPos;

    private void Awake()
    {
        if (sceneManagerGlobal == null)
            sceneManagerGlobal = this;
        else
            Destroy(sceneManagerGlobal.gameObject);

        gm = GetComponentInParent<sGameManager>();

        //loadPos = new Vector3();
    }

    private void Start()
    {
        
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
        // Sets new direction
        //cCompass.compassGlobal.SetDirection(_directionFacing);

        // sets load position
        loadPos = _loadPosOffset;

        // Loads scene
        SceneManager.LoadScene(_sceneToTransitionTo);
    }

    // This method is called whenever a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name + " with load pos of: " + loadPos);

        // Tells GM scene has loaded
        gm.OnSceneLoad();

        // sets player pos
        if (loadPos != Vector2.zero)
            player.SetPosition(loadPos);

        // resets load pos
        loadPos = Vector2.zero;

        // clears text
        player.DisplayText("", 0f);

        // Checks if player was on bus during scene change
        if (sBusStop.isOnBus)
        {
            //Debug.Log("Player was on bus from scene loaded - triggering exit");

            // triggering bus stop exit if player was on busdd
            sBusStop.busStopGlobal.ExitBusStop();
        }
    }
}
