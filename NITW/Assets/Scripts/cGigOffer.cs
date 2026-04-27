using UnityEngine;
using TMPro;

public class cGigOffer : MonoBehaviour
{
    public SO_Gig gigToOffer;

    public TextMeshProUGUI textOffer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textOffer.text = gigToOffer.gigOfferText;
    }


    public void OnYeah()
    {
        cPhone.phoneGlobal.GetGig(gigToOffer);
        this.gameObject.SetActive(false);
    }

    public void OnNah()
    {
        this.gameObject.SetActive(false);
    }
}
