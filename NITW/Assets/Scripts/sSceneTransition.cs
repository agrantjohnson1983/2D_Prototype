using UnityEngine;
using UnityEngine.SceneManagement;

public class sSceneTransition : MonoBehaviour
{
    public string sceneToTransitionTo;

    public eMode modeToTransitionTo;


    // This handles the top down
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Scene change triggered - now going to " + sceneToTransitionTo);

            sGameManager.gm.SetGameMode(modeToTransitionTo);

            SceneManager.LoadScene(sceneToTransitionTo);
        }
    }

    // this is for the 2d side scroll
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Scene change triggered - now going to " + sceneToTransitionTo);

            sGameManager.gm.SetGameMode(modeToTransitionTo);

            SceneManager.LoadScene(sceneToTransitionTo);
        }
    }
}
