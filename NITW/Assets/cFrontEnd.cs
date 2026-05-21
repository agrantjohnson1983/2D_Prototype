using UnityEngine;

public class cFrontEnd : sButtonControllerBASE
{
    public string startingSceneToLoad;

    public void OnClickStart()
    {
        sGameManager.gm.StartGame();
        sSceneManger.sceneManagerGlobal.LoadScene(startingSceneToLoad, eDirection.north, Vector3.zero);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
