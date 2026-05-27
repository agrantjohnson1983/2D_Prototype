using UnityEngine;
using UnityEngine.SceneManagement;

public class sCorner : sInteractable
{
    [Header("Corner Transition Info")]

    public string sceneToTransitionTo;

    public eDirection directionToTurn;

    public Vector3 loadingOffset;

    public SO_Level levelData;

    public override void TriggerInteraction()
    {
        base.TriggerInteraction();

        sSceneManger.sceneManagerGlobal.LoadScene(loadingOffset, levelData);
    }
}
