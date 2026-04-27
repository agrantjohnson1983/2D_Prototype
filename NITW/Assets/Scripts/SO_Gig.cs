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

        // character text
        uTextCharacter.textCharacterGlobal.SetText("New gig-er-ino!", 2f);


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

        // character text
        uTextCharacter.textCharacterGlobal.SetText("Here we go gig!", 2f);
    }

    public void TriggerOnGigComplete()
    {
        // Triggers event
        onGigComplete.Invoke();

        // Finish gig in gig mgr
        sGigManager.gigManagerGlobal.FinishGig();

        // Get money
        cMoney.moneyGlobal.GetMoney(payAmount);

        // character text
        uTextCharacter.textCharacterGlobal.SetText("Another gig bites the dust!", 5f);
    }

    public void TriggerOnGigFail()
    {
        // triggers event
        onGigFail.Invoke();

        uTextCharacter.textCharacterGlobal.SetText("Failure isn't your enemy...", 2f);
    }
}
