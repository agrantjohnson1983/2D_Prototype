using UnityEngine;

public class sPlayer : MonoBehaviour
{
    public GameObject characterSideScroll;
    public GameObject characterSignSpin;
    public GameObject potionBrewing;

    public static sPlayer playerGlobal;

    private void Awake()
    {
        playerGlobal = this;
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
}

