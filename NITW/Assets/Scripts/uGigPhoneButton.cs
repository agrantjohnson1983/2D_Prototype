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

    // This sets the phone buttons
    public void SetButton(SO_Gig _gig)
    {
        // sets image icon
        buttonImage.sprite = _gig.iconSprite;

        // sets text
        buttonText.text = _gig.gigName;

        // gets SO ref
        soGIG = _gig;
    }

    public void OnClick()
    {
        if(!sCharacterController.isOutside)
        {
            uTextCharacter.textCharacterGlobal.SetText("Gotta be outside to gig!", 2f);

            return;
        }

        // this triggers gig start on the SO
        if (soGIG != null)
            soGIG.TriggerOnGigStart();
        else
            Debug.LogWarning("NO SO FOR THIS GIG BUTTON NAMED: " + gameObject.name);

        // this just resets the phone
        phone.StartGig();
    }
}
