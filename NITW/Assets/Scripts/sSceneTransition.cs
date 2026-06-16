using UnityEngine;
using UnityEngine.SceneManagement;

public class sSceneTransition : MonoBehaviour
{
    public string sceneToTransitionTo;

    public eMode modeToTransitionTo;

    public bool triggerOnEnable = false;


    private void OnEnable()
    {
        if(triggerOnEnable)
        {
            TriggerSceneChange();
        }

    }

    void TriggerSceneChange()
    {
        Debug.Log("Scene change triggered - now going to " + sceneToTransitionTo);

        sGameManager.gm.SetGameMode(modeToTransitionTo);

        SceneManager.LoadScene(sceneToTransitionTo);
    }

    // this is for the 2d side scroll
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TriggerSceneChange();
        }
    }
}
