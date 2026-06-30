using Unity.Cinemachine;
using UnityEngine;

public class sCameraFollowToggle : MonoBehaviour
{
    public bool turnsOffOnStart = false;

    CinemachineCamera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = sPlayer.playerGlobal.GetActiveMovementObject().GetComponentInChildren<CinemachineCamera>();

        if (turnsOffOnStart)
            SetCamFollow(null);
    }

    void SetCamFollow(GameObject _objectToFollow)
    {
        //cam.Follow = null;
        //cam.LookAt = null;
        //cam.Target.TrackingTarget = null;

        //var positionComposer = cam.GetComponent<CinemachinePositionComposer>();

        //if(positionComposer != null )
        //{
        //    positionComposer.enabled = false;
        //}

        //cam.UpdateTargetCache();

        //var follow = cam.GetCinemachineComponent<CinemachineFollow>().enabled = false;

        Camera.main.GetComponent<CinemachineBrain>().enabled = false;

        Debug.Log("Cam follow is nulled out!");
    }
}
