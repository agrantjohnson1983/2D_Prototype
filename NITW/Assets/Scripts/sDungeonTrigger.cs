using UnityEngine;

public class sDungeonTrigger : MonoBehaviour
{
    public string dungeonSceneName;
    public eDirection directionToLoad;

    public bool isDungeonExit = false;

    public Vector3 loadingOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // Toggles dungeon canvas
            sGameManager.gm.ToggleDungeonCanvas(!isDungeonExit);

            // Toggles player dungeon mode
            sPlayer.playerGlobal.ToggleDungeon(!isDungeonExit);

            // Loads scene
            sSceneManger.sceneManagerGlobal.LoadScene(dungeonSceneName, directionToLoad, loadingOffset);
        }
    }
}
