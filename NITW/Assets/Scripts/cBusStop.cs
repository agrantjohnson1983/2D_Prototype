using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum eNeighborhood { basin, burbs, downtown, outskirts, centerSquare}

public class cBusStop : MonoBehaviour
{
    public GameObject cityMap, busStopUI;

    public eNeighborhood currentNeighborhood;

    // scene names
    public string 
        basinSceneName, 
        burbsSceneName, 
        downtownScenName, 
        outskirtsSceneName, 
        centerSquareSceneName;

    public eDirection
        basinDirection,
        burbsDirection,
        downtownDirection,
        outskirtsDirection,
        centerSquareDirection;

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
        // sets bool to on bus
        sBusStop.isOnBus = true;

        // checks if current neighborhood has been selected
        if(_neighborhoodToGoTo == currentNeighborhood)
        {
            Debug.Log("Current neighborhood was clicked");

            // turns on bus UI to reset
            busStopUI.SetActive(true);

            // unpauses character movement
            //sCharacterController.characterControllerGlobal.SetCanMove(true);

            // turns off canvas object
            this.gameObject.SetActive(false);
        }

        else
        {
            Debug.Log("Going to new neighborhood - " + _neighborhoodToGoTo.ToString());

            // turns on bus UI to reset
            busStopUI.SetActive(true);

            // unpauses character movement
            sCharacterControllerBASE.canMove = false;

            // switches the neighborhoods and then does a scene change to the new one
            switch (_neighborhoodToGoTo)
            {
                case eNeighborhood.basin:

                    cLocation.locationGlobal.SetTextLocation("The Basin");
                    sSceneManger.sceneManagerGlobal.LoadScene(basinSceneName, basinDirection);

                    break;

                case eNeighborhood.burbs:

                    cLocation.locationGlobal.SetTextLocation("The Burbs");
                    sSceneManger.sceneManagerGlobal.LoadScene(burbsSceneName, burbsDirection);

                    break;

                   case eNeighborhood.downtown:

                    cLocation.locationGlobal.SetTextLocation("Downtown");
                    sSceneManger.sceneManagerGlobal.LoadScene(downtownScenName, downtownDirection);

                    break;

                case eNeighborhood.outskirts:

                    cLocation.locationGlobal.SetTextLocation("The Outskirts");
                    sSceneManger.sceneManagerGlobal.LoadScene(outskirtsSceneName, outskirtsDirection);

                    break;

                case eNeighborhood.centerSquare:

                    cLocation.locationGlobal.SetTextLocation("Center Square");
                    sSceneManger.sceneManagerGlobal.LoadScene(centerSquareSceneName, centerSquareDirection);

                    break;
            }

            // turns off canvas object
            this.gameObject.SetActive(false);
        }
    }
}
