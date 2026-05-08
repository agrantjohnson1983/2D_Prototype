using UnityEngine;

public enum eVideoGames { none, pong }

public class sVideoGameConsoleTrigger : MonoBehaviour
{
    public Transform gameListTransform;

    public GameObject gameList;

    public GameObject cGameCanvas;

    public GameObject playButton;

    public void OnClickPlay()
    {
        playButton.SetActive(false);
        gameList.SetActive(true);
    }

    public void StartGame(eVideoGames _game)
    {
        sPlayer.playerGlobal.PlayVideoGame(_game);
    }

    // Toggles canvas on/off with Play? button

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            cGameCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cGameCanvas.SetActive(false);
        }
    }
}
