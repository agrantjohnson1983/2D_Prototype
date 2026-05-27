using System.Collections.Generic;
using UnityEngine;

public class sLevelManager : MonoBehaviour
{
    public static sLevelManager levelManagerGlobal;

    Dictionary<SO_Level, GameObject> levelDictionary;

    SO_Level currentLevel = null;

    private void Awake()
    {
        if(levelManagerGlobal == null)
        {
            levelManagerGlobal = this;
        }
        else
        {
            Destroy(this);
        }

        levelDictionary = new Dictionary<SO_Level, GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void AddLevel(SO_Level _levelData, GameObject _levelObject)
    {
        //Debug.Log("Adding level data for " + _levelData);

        // null checks
        if(_levelData == null || _levelObject == null)
        {
            Debug.LogError("Level data or level object is null");
            return;
        }

        // if there isn't a current level
        if(currentLevel == null)
        {
            // sets current level to level data
            currentLevel = _levelData;

            // adds to dictionary
            levelDictionary.Add(_levelData, _levelObject);
            return;
        }

        else
        {
            // turns off current level object
            //GetLevelObject(currentLevel).SetActive(false);
        }


        // if the level is already in the dictionary
        if (levelDictionary.ContainsKey(_levelData))
        {
            return;
        }

        // not in dictionary
        else
        {
            // adds level to dictionary
            levelDictionary.Add(_levelData, _levelObject);
        }
    }

    public GameObject GetLevelObject(SO_Level _levelData)
    {
        if(levelDictionary.ContainsKey(_levelData))
        {
            return levelDictionary[_levelData];
        }

        else
        {
            Debug.LogError("No level found for: " + _levelData);
            return null;
        }
    }

    public void ChangeLevel(SO_Level _levelToChangeTo)
    {
        if (currentLevel == null) return;

        // turns off current level object
        GetLevelObject(currentLevel).SetActive(false);

        // checks if the level to change is in dictionary cause it will need to be turned back on
        if(levelDictionary.ContainsKey(_levelToChangeTo))
        {
            // turns on level object
            GetLevelObject(_levelToChangeTo).SetActive(true);
        }

        // sets new current level
        currentLevel = _levelToChangeTo;

        sAudioManager.audioManagerGlobal.SceneChange();

        Debug.Log("Level swapped");

        // otherwise the level object will be on by default and will trigger AddLevel
    }
}
