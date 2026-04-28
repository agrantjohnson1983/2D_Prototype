using UnityEngine;

public class sPlayer : MonoBehaviour
{
    public GameObject characterSideScroll;
    public GameObject characterSignSpin;
    public GameObject potionBrewing;

    public static sPlayer playerGlobal;

    //bool isOnBus = false;

    private void Awake()
    {
        if (playerGlobal == null)
        {
            playerGlobal = this;
            DontDestroyOnLoad(this.gameObject);
        }
            
        else
            Destroy(this.gameObject);

       
    }

    // TO - DO - Convert scripts to have methods for stopping and initing without turning on/off game objects


    // GIGGING - This controls a lot of the different gameplay

    public void StartGig(eGigType _type)
    {
        Debug.Log("Starting gig..." + _type);

        switch(_type)
        {
            case eGigType.signSpin:

                characterSideScroll.SetActive(false);
                characterSignSpin.SetActive(true);

                break;
        }
    }

    public void EndGig(eGigType _type)
    {
        switch (_type)
        {
            case eGigType.signSpin:

                characterSideScroll.SetActive(true);
                characterSignSpin.SetActive(false);

                break;
        }
    }

    // POTION BREWING

    public void StartPotionBrew()
    {
        characterSideScroll.SetActive(false);
        potionBrewing.SetActive(true);
    }

    public void StopPotionBrew()
    {
        potionBrewing.SetActive(false);
        characterSideScroll.SetActive(true);
    }

    public void ResetPositions(Vector3 _newPos)
    {
        Debug.Log("Resetting player children and setting parent object to " + _newPos.ToString());
        this.transform.position = _newPos;
        characterSideScroll.transform.localPosition = Vector3.zero;
        characterSignSpin.transform.localPosition = Vector3.zero;
    }
}

