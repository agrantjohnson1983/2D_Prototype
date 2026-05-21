using Unity.Cinemachine;
using UnityEngine;

public class sDynamicHeightZoom : MonoBehaviour
{
    public CinemachineCamera vcam;
    public Transform target;

    [Header("Height Limits")]
    public float minHeight = 0f;
    public float maxHeight = 30f;

    [Header("Zoom (Lens) Limits")]
    public float minZoom = 60f; // FOV or Ortho Size at min height
    public float maxZoom = 80f; // FOV or Ortho Size at max height

    void Update()
    {
        // Calculate where the target is between min and max height (0 to 1)
        float heightFactor = Mathf.InverseLerp(minHeight, maxHeight, target.position.y);

        // Linearly interpolate between zoom values
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, heightFactor);

        // Apply to the camera lens

        vcam.Lens.FieldOfView = targetZoom;

        //vcam.m_Lens.FieldOfView = targetZoom; // For 3D
        // vcam.m_Lens.OrthographicSize = targetZoom; // For 2D
    }
}
