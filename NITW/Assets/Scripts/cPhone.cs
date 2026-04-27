using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class cPhone : MonoBehaviour
{
    public static cPhone phoneGlobal;

    public GameObject pGigPhoneButton;

    List<SO_Gig> gigList;

    public Transform gigsButtonTransform;

    public GameObject notificationObject, hudPhoneNotificationObject;

    public Image notificationImage;

    public TextMeshProUGUI notificationText;

    int currentNumberOfNotifications = 0;

    List<GameObject> gigButtons;

    private void Awake()
    {
        if (phoneGlobal == null)
            phoneGlobal = this;
        else
            Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gigList = new List<SO_Gig>();

        gigButtons = new List<GameObject>();

        // inits notification object and hides it with zero scale
        notificationObject.SetActive(true);
        notificationObject.transform.localScale = Vector3.zero;

        // inits hud notification object and hides it with zero scale
        hudPhoneNotificationObject.SetActive(true);
        hudPhoneNotificationObject.transform.localScale = Vector3.zero;
    }

    // When the gigs button is pressed - spawns buttons
    public void OpenGigs()
    {
        gigButtons = new List<GameObject>();

        // this will spawn the gig buttons based on current gigs in the gig list
        for (int i = 0; i < gigList.Count; i++)
        {
            GameObject tempObj;

            // spawns button
            tempObj = Instantiate(pGigPhoneButton, gigsButtonTransform);

            // sets the button from the SO
            tempObj.GetComponent<uGigPhoneButton>().SetButton(gigList[i]);

            // adds to buttons list
            gigButtons.Add(tempObj);
        }


        // Resets notifications

        currentNumberOfNotifications = 0;

        //notificationObject.SetActive(false);
        notificationObject.transform.localScale = Vector3.zero;

        //hudPhoneNotificationObject.SetActive(false);
        hudPhoneNotificationObject.transform.localPosition = Vector3.zero;
    }

    // Destroys all the gig buttons when closing
    public void CloseGigs()
    {
        for (int i = 0;i < gigButtons.Count;i++)
        {
            Destroy(gigButtons[i]);
        }
    }

    public void GetGig(SO_Gig _gig)
    {
        Debug.Log("Got gig - " + _gig.gigName);
        
        // triggers SO event 
        _gig.TriggerOnGetGig();

        // adds gig to gig list
        gigList.Add(_gig);

        // sets notifications
        SetNotification();
    }

    // This resets phone when gig is clicked to start
    public void StartGig()
    {
        //hudPhoneNotificationObject.SetActive(true);

        hudPhoneNotificationObject.transform.localScale = Vector3.one;

        //this.gameObject.SetActive(false);

        this.gameObject.transform.localScale = Vector3.zero;
    }

    // This sets notification when you get a new gig
    void SetNotification()
    {
        // increments number of current notifications
        currentNumberOfNotifications++;

        // turns on notification object in the gigs app within the phone
        //notificationObject.SetActive(true);

        notificationObject.transform.localScale = Vector3.one;

        // turns on notification for the main HUD phone icon
        //hudPhoneNotificationObject.SetActive(true);

        hudPhoneNotificationObject.transform.localScale = Vector3.one;

        notificationImage.color = Color.red;

        notificationText.text = currentNumberOfNotifications.ToString();
    }

    public List<SO_Gig> ReturnGigList()
    {
        return gigList;
    }
}
