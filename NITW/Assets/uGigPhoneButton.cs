using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class uGigPhoneButton : MonoBehaviour
{
    public Image buttonBG;
    public Image buttonImage;
    public TextMeshProUGUI buttonText;

    SO_Gig soGIG;

    cPhone phone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonBG = GetComponent<Image>();
        buttonImage = GetComponentInChildren<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        phone = GetComponentInParent<cPhone>();
    }

    public void SetButton(SO_Gig _gig)
    {
        buttonImage.sprite = _gig.iconSprite;

        buttonText.text = _gig.gigName;

        soGIG = _gig;
    }

    public void OnClick()
    {
        if (soGIG != null)
            soGIG.TriggerOnGigStart();
        else
            Debug.LogWarning("NO SO FOR THIS GIG BUTTON NAMED: " + gameObject.name);

        phone.gameObject.SetActive(false);
    }
}
