using UnityEngine;

public class cFrontEnd : sButtonControllerBASE
{
    public string startingSceneToLoad;

    public SO_Level levelData;

    public void OnClickStart()
    {
        sGameManager.gm.StartGame();
        sSceneManger.sceneManagerGlobal.LoadScene(Vector3.zero, levelData);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
