using UnityEngine;

public class sBusStop : MonoBehaviour
{
    public static sBusStop busStopGlobal;

    public GameObject busCanvas;

    public Transform busStopExit;

    public static bool isOnBus = false;

    private void Awake()
    {
        // non-persistant singleton - so the bus stop exit will be different for every scene
        if (busStopGlobal == null)
            busStopGlobal = this;
        else
            Destroy(this.gameObject);
    }

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

    public void ExitBusStop()
    {
        //Debug.Log("Exiting bus station");

        // resets player pos and moves it to bus exit
        sPlayer.playerGlobal.ResetPositions(busStopExit.position);

        // resets character movement
        sCharacterController.characterControllerGlobal.SetCanMove(true);

        isOnBus = false;
    }

    void SetCharacterPos(Vector3 _pos)
    {
        // Should this be done with the sPlayer script?

        //sCharacterController.characterControllerGlobal.transform.position = _pos;

        sPlayer.playerGlobal.transform.position = _pos;
    }
}
