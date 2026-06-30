using UnityEngine;
using UnityEngine.SceneManagement;

public class sSceneTransition : MonoBehaviour
{
    public SO_Level levelToLoad;

    public eMode modeToTransitionTo;

    public bool triggerOnEnable = false;

    public bool additiveLoading = true;

    private void OnEnable()
    {
        if(triggerOnEnable)
        {
            TriggerSceneChange();
        }
    }

    void TriggerSceneChange()
    {
        Debug.Log("Scene change triggered - now going to " + levelToLoad);

        sGameManager.gm.SetGameMode(modeToTransitionTo);

        if(additiveLoading)
            sSceneManger.sceneManagerGlobal.LoadScene(Vector3.zero, levelToLoad, eLoadMode.additive);
        else
            sSceneManger.sceneManagerGlobal.LoadScene(Vector3.zero, levelToLoad, eLoadMode.normal);

        //SceneManager.LoadScene(sceneToTransitionTo);
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
