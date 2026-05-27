using UnityEngine;

public class sOverworldTrigger : MonoBehaviour
{
    public string sceneToChange;

    public Vector3 loadingOffset;

    public SO_Level levelData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // sets game mode
            sGameManager.gm.SetGameMode(eMode.topdownLow);

            // loads scene
            sSceneManger.sceneManagerGlobal.LoadScene(loadingOffset, levelData);
        }
    }
}
