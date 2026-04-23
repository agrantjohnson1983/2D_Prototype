using UnityEngine;

public class sBusStop : MonoBehaviour
{
    public GameObject busCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        busCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EnterBusStop();
        }
    }

    void EnterBusStop()
    {
        busCanvas.SetActive(true);
        sCharacterControllerTopDown.PauseMovement();
    }
}
