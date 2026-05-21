using UnityEngine;

public class sBusStop : sInteractable
{
    public static sBusStop busStopGlobal;

    [Space][Header("Bus Stop Info")]

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
    public override void Start()
    {
        base.Start();

        busCanvas.SetActive(false);
        //busCanvas.transform.localScale = Vector3.zero;
    }

    public override void TriggerInteraction()
    {
        base.TriggerInteraction();

        EnterBusStop();
    }

    // gets called when player enters trigger zone
    void EnterBusStop()
    {
        // turns on bus canvas
        busCanvas.SetActive(true);

        // turns player movement off
        sPlayer.playerGlobal.ToggleMovement(false);
    }

    public void ExitBusStop()
    {
        //Debug.Log("Exiting bus station");

        // resets player pos and moves it to bus exit
        sPlayer.playerGlobal.SetPosition(busStopExit.position);

        // toggles player movement
        sPlayer.playerGlobal.ToggleMovement(true);

        isOnBus = false;
    }
}
