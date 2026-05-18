using UnityEngine;
using TMPro;

public class cGigOffer : MonoBehaviour
{
    public SO_Gig gigToOffer;

    public TextMeshProUGUI textOffer;

    uTypewriter typewriter;

    public float typingSpeed = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textOffer.text = gigToOffer.gigOfferText;

        typewriter = GetComponent<uTypewriter>();

        typewriter.StartTypewriter(textOffer, typingSpeed);
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
