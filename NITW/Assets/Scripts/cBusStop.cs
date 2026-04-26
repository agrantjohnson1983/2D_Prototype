using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum eNeighborhood { basin, burbs, downtown, outskirts, centerSquare}

public class cBusStop : MonoBehaviour
{
    public GameObject cityMap, busStopUI;

    public eNeighborhood currentNeighborhood;

    uCityMap cityMapUI;

    public TextMeshProUGUI displayText;

    public static int numberOfBusPasses = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cityMap.SetActive(false);

        busStopUI.SetActive(true);

        // sets display text based on number of bus passes
        displayText.text = "Wecome to the " + currentNeighborhood.ToString() + 
            " bus station! \n You have " + numberOfBusPasses + " bus pass for today";
    }

    public void OnSelectARoute()
    {
        // turn on city map
        cityMap.SetActive(true);

        // Gets ref to city map
        cityMapUI = GetComponentInChildren<uCityMap>();

        // sets the current neighborhood in the map
        cityMapUI.SetNeighborhood(currentNeighborhood);

        // sets bus UI inactive
        busStopUI.gameObject.SetActive(false);
    }

    public void GoToNewPlace(eNeighborhood _neighborhoodToGoTo)
    {
        // checks if current neighborhood has been selected
        if(_neighborhoodToGoTo == currentNeighborhood)
        {
            // turns on bus UI to reset
            busStopUI.SetActive(true);

            // turns off canvas object
            this.gameObject.SetActive(false);

            // unpauses character movement
            sCharacterController.characterControllerGlobal.SetCanMove(true);
        }

        else
        {
            // TO DO - ADD SCENE CHANGING
            // switches the neighborhoods and then does a scene change to the new one
            switch (currentNeighborhood)
            {
                case eNeighborhood.basin:



                    break;
                case eNeighborhood.burbs:




                    break;
                   case eNeighborhood.downtown:




                    break;
                case eNeighborhood.outskirts:




                    break;
                case eNeighborhood.centerSquare:



                    break;
            }
        }
    }
}
