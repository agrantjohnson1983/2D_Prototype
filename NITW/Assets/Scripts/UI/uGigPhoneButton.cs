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

    sPlayer player;// { get { if (player == null) player = sPlayer.playerGlobal; return player; } set { } }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonBG = GetComponent<Image>();

        buttonImage = GetComponentInChildren<Image>();

        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        phone = GetComponentInParent<cPhone>();

        player = sPlayer.playerGlobal;
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
        if(!player.CheckIfOutside())
        {
            sPlayer.playerGlobal.DisplayText("Gotta be outside to gig!", 2f);

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
