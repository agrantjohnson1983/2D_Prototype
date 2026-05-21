using UnityEngine;
using UnityEngine.SceneManagement;

public class sCorner : sInteractable
{
    [Header("Corner Transition Info")]

    public string sceneToTransitionTo;

    public eDirection directionToTurn;

    public Vector3 loadingOffset;

    public override void TriggerInteraction()
    {
        base.TriggerInteraction();

        sSceneManger.sceneManagerGlobal.LoadScene(sceneToTransitionTo, directionToTurn, loadingOffset);
    }
}
