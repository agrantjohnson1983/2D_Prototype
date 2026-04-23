using UnityEngine;

public class sExitBehavior : MonoBehaviour
{
    public GameObject outside;
    public GameObject inside;
    public GameObject outsideGround;

    public Transform exitTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sCharacterController.isOutside = true;
        sCharacterController.characterControllerGlobal.SetLocation(exitTransform.position);
        outsideGround.SetActive(true);
        outside.SetActive(true);
        inside.SetActive(false);
    }
}
