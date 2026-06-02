using UnityEngine;

public enum eVideoGames { none, pong }

public class sVideoGameConsoleTrigger : sInteractable
{
    public Transform gameListTransform;

    public GameObject gamesCanvas;

    public GameObject buttonFirstSelected;

    //public GameObject cGameCanvas;

    //public GameObject playButton;

    public override void TriggerInteraction()
    {
        base.TriggerInteraction();
        
        // turns on game canvas
        gamesCanvas.SetActive(true);

        // turns off player movement
        sPlayer.playerGlobal.ToggleMovement(false);

        // sets button controls
        sGameManager.gm.SetEventSystem(buttonFirstSelected);
    }

    // Buttons will call this and feed the game type through
    public void StartGame(eVideoGames _game)
    {
        // starts the player game
        sPlayer.playerGlobal.PlayVideoGame(_game);
    }

    public void QuitGames()
    {
        // turns off game canvas
        gamesCanvas.SetActive(false);

        // turns off player movement
        sPlayer.playerGlobal.ToggleMovement(true);
    }
}
