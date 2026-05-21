using UnityEngine;

using UnityEngine.Events;

public enum eGigType {none, signSpin, fetch, deliver, taxi, }

[CreateAssetMenu(fileName = "SO_Gig", menuName = "Scriptable Objects/SO_Gig")]
public class SO_Gig : ScriptableObject
{
    public string gigName;

    public eGigType gigType;

    public float payAmount;

    float currentPayAmount;

    public Sprite iconSprite;

    public GameObject worldObject;

    bool isDoingGig = false;
    bool isGigComplete = false;

    public string 
        gigOfferText, gigAcceptedText,
        gigInProgressText, gigPropositionText, gigRejectionText,
        gigCompleteText;

    public UnityEvent 
        onGetGig, 
        onGigStart, 
        onGigComplete, 
        onGigFail;

    private void OnEnable()
    {
        if (onGetGig != null) onGetGig = new UnityEvent();

        if(onGigStart != null) onGigStart = new UnityEvent();

        if(onGigComplete != null) onGigComplete = new UnityEvent();

        if(onGigFail != null) onGigFail = new UnityEvent();

        currentPayAmount = payAmount;

        isDoingGig = false;
        isGigComplete = false;
    }

    public bool CheckIfDoingGig()
    {
        return isDoingGig;
    }

    public bool CheckIfGigComplete()
    {
        return isGigComplete; 
    }

    public void TriggerOnGetGig()
    {
        // toggles isDoingGig
        isDoingGig = true;

        // triggers event
        onGetGig.Invoke();

        // gets gig in gig mgr
        sGigManager.gigManagerGlobal.GetGig(this);

        // character text
        sPlayer.playerGlobal.DisplayText("New gig-er-ino!", 2f);
    }

    public void TriggerOnGigStart()
    {
        //Debug.Log("Triggering gig start!");

        // Triggers event
        onGigStart.Invoke();

        // starts gig in gig mgr
        sGigManager.gigManagerGlobal.StartGig(this);

        // starts gig with player character
        sPlayer.playerGlobal.StartGig(this);

        // character text
        sPlayer.playerGlobal.DisplayText("Here we go gig!", 2f);
    }

    public void TriggerOnGigComplete()
    {
        // toggles complete
        isGigComplete = true;

        // Triggers event
        onGigComplete.Invoke();

        // Finish gig in gig mgr - not sure yet if this is really needed
        sGigManager.gigManagerGlobal.FinishGig();

        // Get money
        cMoney.moneyGlobal.GetMoney(currentPayAmount);

        // Calls player to set end gig state
        sPlayer.playerGlobal.EndGig(this);

        // character text
        sPlayer.playerGlobal.DisplayText("Another gig bites the dust!", 5f);
    }

    public void TriggerOnGigFail()
    {
        // triggers event
        onGigFail.Invoke();

        // sends text message
        sPlayer.playerGlobal.DisplayText("Failure isn't your enemy...", 2f);
    }

    // The gig will call this before completing so the final pay amount will be set correct
    public void PayUp(float _payPercent)
    {
        // calculates pay amount based on percent - so 1 will equal full pay
        currentPayAmount = payAmount * _payPercent;

        // returns pay amount
        //return currentPayAmount;
    }
}
