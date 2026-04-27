using UnityEngine;

public class sBusStop : MonoBehaviour
{
    public GameObject busCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        busCanvas.SetActive(false);
        //busCanvas.transform.localScale = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks for player collision
        if (collision.CompareTag("Player"))
        {
            EnterBusStop();
        }
    }

    // gets called when player enters trigger zone
    void EnterBusStop()
    {
        // turns on bus canvas
        busCanvas.SetActive(true);

        //busCanvas.transform.localScale = Vector3.one;

        // turns player movement off
        sCharacterController.characterControllerGlobal.SetCanMove(false);
    }
}
