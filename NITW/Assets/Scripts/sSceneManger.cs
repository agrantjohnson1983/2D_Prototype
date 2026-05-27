using NUnit.Framework;
using System.Collections.Generic;
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

    List<string> scenesLoadedList;

    private void Awake()
    {
        if (sceneManagerGlobal == null)
            sceneManagerGlobal = this;
        else
            Destroy(this.gameObject);

        gm = GetComponentInParent<sGameManager>();

        scenesLoadedList = new List<string>();

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

    public void LoadScene(Vector3 _loadPosOffset, SO_Level _levelData)
    {
        // Sets new direction
        //cCompass.compassGlobal.SetDirection(_directionFacing);

        // sets load position
        loadPos = _loadPosOffset;

        // does level swap
        sLevelManager.levelManagerGlobal.ChangeLevel(_levelData);

        // Checks if scene is already on the list
        if (CheckIfSceneIsInList(_levelData.sceneName))
        {
            Debug.Log("Scene was found in list - changing level without scene change ");

        }

        // if scene is not on list then it loads
        else
        {
            Debug.Log("No scene found in dictionary so loading level");

            // Loads scene async
            SceneManager.LoadSceneAsync(_levelData.sceneName, LoadSceneMode.Additive);
        }
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

        // Adds scene to list
        AddSceneToList(scene.name);
    }

    // This returns a bool whether a scene is in scene list
    bool CheckIfSceneIsInList(string _sceneToCheck)
    {
        // set to false by default
        bool _isInList = false;

        // iterates through scene list
        foreach (string _scene in scenesLoadedList)
        {
            // if a scene is in list it toggles bool
            if (_scene == _sceneToCheck)
            {
                _isInList = true;
            }
        }

        // returns bool
        return _isInList;
    }

    // returns a bool if 
    public void AddSceneToList(string _sceneName)
    {
        // if it's not in list it gets added to list
        if (!CheckIfSceneIsInList(_sceneName))
            scenesLoadedList.Add(_sceneName);
    }
}
