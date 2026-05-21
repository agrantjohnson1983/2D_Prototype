using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class sCharacterGigMaster : sInteractable
{
    [Header("Gig Info")]
    [Space]
    public SO_Gig gigData;
    [Space]
    [Header("Dialogue")]
    public GameObject canvasDialogue;

    public TextMeshProUGUI characterDialogueTextMesh;

    public TextMeshProUGUI textButtonYes, textButtonNo;
    [Space]
    public GameObject buttonYes, buttonNo;
    
    //public TextMeshProUGUI characterTextMeshGigOffer;
    public uTypewriter typewriter;

    //protected bool gigComplete = false;
    //protected bool isDoingGig = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        //SetYesButton(OnGigAccept);

        // sets gig offer text
        if (characterDialogueTextMesh != null) characterDialogueTextMesh.text = gigData.gigOfferText;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    // TO-DO - create events to fire?

    protected void SetYesButton(UnityAction _action)
    {
        buttonYes.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonYes.GetComponent<Button>().onClick.AddListener(_action);
    }

    public virtual void OnGigAccept()
    {
        // is doing it baby!
        //isDoingGig = true;

        // triggers SO
        gigData.TriggerOnGetGig();

        // sets gig
        cPhone.phoneGlobal.GetGig(gigData);

        // turns off buttons
        ToggleButtons(false);

        // sets text to in progress
        characterDialogueTextMesh.text = gigData.gigAcceptedText;

        // sets yes button to do gig complete
        //SetYesButton(OnGigComplete);

        // resets canvas
        Invoke("ResetCanvas", 1f);
    }

    public virtual void OnGigReject()
    {
        // rejection text
        sPlayer.playerGlobal.DisplayText("yeahh unemployment baby!", 0.1f);

        // sets npc rejection text
        characterDialogueTextMesh.text = gigData.gigRejectionText;

        // typewriter effect
        typewriter.StartTypewriter(characterDialogueTextMesh, 0.5f);

        // turns off buttons temporarily - they get reset in the Invoke
        ToggleButtons(false);

        // Slight delay on resetting canvas to show rejection text
        Invoke("ResetCanvas", 1f);
    }

    protected void ToggleButtons(bool _isOn)
    {
        buttonYes.SetActive(_isOn);
        buttonNo.SetActive(_isOn);
    }

    protected void ResetCanvas()
    {
        // turns off canvas
        canvasDialogue.SetActive(false);

        if(gigData.CheckIfDoingGig())
        {
            // turns off buttons
            ToggleButtons(false);

            // sets in progress text
            characterDialogueTextMesh.text = gigData.gigInProgressText;
        }

        else
        {
            // turns buttons back on
            ToggleButtons(true);

            // sets text back to gig offer
            characterDialogueTextMesh.text = gigData.gigOfferText;
        }

        // turns on player movement
        sPlayer.playerGlobal.ToggleMovement(true);
    }

    public virtual void OnGigComplete()
    {
        // triggers SO
        gigData.TriggerOnGigComplete();

        // turns player movement back on
        sPlayer.playerGlobal.ToggleMovement(true);

        // buttons inactive
        ToggleButtons(false);

        // sets npc rejection text
        characterDialogueTextMesh.text = gigData.gigCompleteText;

        // Slight delay on resetting canvas to show rejection text
        Invoke("ResetCanvas", 1.5f);
    }

    public override void TriggerInteraction()
    {
        // returns if gig is completed
        if (gigData.CheckIfGigComplete()) return;

        // base trigger - currently nothing happening here...
        base.TriggerInteraction();

        // if not doing gig
        if(!gigData.CheckIfDoingGig())
        {
            // Sets yes button to On Gig Accept
            SetYesButton(OnGigAccept);

            // turns on canvas
            canvasDialogue.SetActive(true);

            // turns off player movement
            sPlayer.playerGlobal.ToggleMovement(false);

            // sets interactable button
            sGameManager.gm.SetEventSystem(buttonYes);
        }

        else
        {
            // sets yes button to On Gig complete
            SetYesButton(OnGigComplete);
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        // returns if gig is complete
        if (gigData.CheckIfGigComplete()) return;

        if (!collision.CompareTag("Player")) return;

        // runs base interactable trigger
        base.OnTriggerEnter2D(collision);
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        // returns if gig is complete
        if (gigData.CheckIfGigComplete()) return;

        // runs base interactable trigger
        base.OnTriggerExit2D(collision);

        
    }

}
