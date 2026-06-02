using UnityEngine;

public class cFrontEnd : sButtonControllerBASE
{
    public SO_Level levelData;

    public void OnClickStart()
    {
        // starts game thru GM
        sGameManager.gm.StartGame();

        // loads scene
        sSceneManger.sceneManagerGlobal.LoadScene(Vector3.zero, levelData);

        // turns off
        this.gameObject.SetActive(false);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
