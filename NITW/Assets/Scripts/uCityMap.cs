using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class uCityMap : MonoBehaviour
{
    eNeighborhood currentNeighborhood;
    //string str_currentNeighborhood;

    public Button bBasin, bBurbs, bOutskirts, bDowntown, bCenterSquare;

    cBusStop busStopCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // gets  reference to the bus stop canvas
        busStopCanvas = GetComponentInParent<cBusStop>();
    }

    public void SetNeighborhood(eNeighborhood _neighborhood)
    {
        //Debug.Log("Setting map neighborhood to : " + _neighborhood);

        switch(_neighborhood)
        {
            case eNeighborhood.basin:

                break;

            case eNeighborhood.burbs:

                //str_currentNeighborhood = "TheBurbs";

                // Sets button color to green of current neighborhood
                bBurbs.GetComponent<Image>().color = Color.green;

                // Changes text
                bBurbs.GetComponentInChildren<TextMeshProUGUI>().text = "You are here! \n The Burbs";
                
                //bBurbs.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                bBurbs.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Italic;

                break;

            case eNeighborhood.downtown:

                //str_currentNeighborhood = "Downtown";

                // Sets button color to green of current neighborhood
                bDowntown.GetComponent<Image>().color = Color.green;

                // Changes text
                bDowntown.GetComponentInChildren<TextMeshProUGUI>().text = "You are here! \n Downtown";

                //bBurbs.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                bDowntown.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Italic;


                break;

            case eNeighborhood.outskirts:

                break;

            case eNeighborhood.centerSquare:

                break;
        }
    }

    public void OnButtonClick(string _neighborHood)
    {
        //Debug.Log("Button clicked for neighborhood: " + _neighborHood.ToString());

        switch (_neighborHood)
        {
            case "TheBasin":

                //Debug.Log("You clicked The basin");

                busStopCanvas.GoToNewPlace(eNeighborhood.basin);

                break;

            case "TheBurbs":

                //Debug.Log("You clicked The burbs");

                busStopCanvas.GoToNewPlace(eNeighborhood.burbs);

                break;

            case "Downtown":

                //Debug.Log("You clicked downtown");

                busStopCanvas.GoToNewPlace(eNeighborhood.downtown);

                break;

            case "TheOutskirts":

                //Debug.Log("You clicked the outskirts");

                busStopCanvas.GoToNewPlace(eNeighborhood.outskirts);

                break;
            case "CenterSquare":

                //Debug.Log("You clicked center square");

                busStopCanvas.GoToNewPlace(eNeighborhood.centerSquare);

                break;
        }

        // turns off game objectw
        this.gameObject.SetActive(false);
    }
}
