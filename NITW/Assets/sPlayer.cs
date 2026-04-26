using UnityEngine;

public class sPlayer : MonoBehaviour
{
    public GameObject characterSideScroll;
    public GameObject characterSignSpin;

    public static sPlayer playerGlobal;

    private void Awake()
    {
        playerGlobal = this;
    }


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
}
