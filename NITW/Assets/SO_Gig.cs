using UnityEngine;

using UnityEngine.Events;

public enum eGigType {none, signSpin, fetch, deliver, taxi, }

[CreateAssetMenu(fileName = "SO_Gig", menuName = "Scriptable Objects/SO_Gig")]
public class SO_Gig : ScriptableObject
{
    public string gigName;

    public eGigType gigType;

    public float payAmount;

    public Sprite iconSprite;

    public GameObject worldObject;

    public string gigOfferText;

    public UnityEvent onGetGig, onGigStart, onGigComplete, onGigFail;

    private void OnEnable()
    {
        if (onGetGig != null) onGetGig = new UnityEvent();

        if(onGigStart != null) onGigStart = new UnityEvent();

        if(onGigComplete != null) onGigComplete = new UnityEvent();

        if(onGigFail != null) onGigFail = new UnityEvent();
    }

    public void TriggerOnGetGig()
    {
        // triggers event
        onGetGig.Invoke();

        // gets gig in gig mgr
        sGigManager.gigManagerGlobal.GetGig(gigType);
    }

    public void TriggerOnGigStart()
    {
        //Debug.Log("Triggering gig start!");

        // Triggers event
        onGigStart.Invoke();

        // starts gig in gig mgr
        sGigManager.gigManagerGlobal.StartGig(gigType);

        // Sets player
        sPlayer.playerGlobal.StartGig(gigType);
    }

    public void TriggerOnGigComplete()
    {
        // Triggers event
        onGigComplete.Invoke();

        // Finish gig in gig mgr
        sGigManager.gigManagerGlobal.FinishGig();

        // Get money
        cMoney.moneyGlobal.GetMoney(payAmount);
    }

    public void TriggerOnGigFail()
    {
        // triggers event
        onGigFail.Invoke();
    }
}
