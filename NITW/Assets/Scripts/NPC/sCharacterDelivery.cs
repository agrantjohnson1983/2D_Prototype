using Unity.VisualScripting;
using UnityEngine;

public class sCharacterDelivery : sCharacterGigMaster
{
    public bool isRecipient = false;

    public SO_Item itemToDeliver;

    bool isDelivering = false;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        Debug.Log("Delivery character is being triggered after base trigger");
    }

    public override void TriggerInteraction()
    {
        base.TriggerInteraction();

        // checks if is delivery receipient
        if (isRecipient)
        {
            // temp has item bool check
            bool _hasItem = false;

            // iterates through entire inventory list
            foreach (SO_Item item in cInventory.inventoryGlobal.ReturnItemList())
            {
                // checks if item to deliver is in inventory
                if (item == itemToDeliver && !_hasItem)
                {
                    _hasItem = true;
                }
            }

            // if you have the item then it displays the finish canvas
            if (_hasItem)
            {
                characterDialogueTextMesh.text = gigData.gigPropositionText;
            }

            // Displays other canvas
            else
            {
                // leave blank and just add the text in the canvas since it won't change
            }

            // Sets yes button to On Gig Accept
            //SetYesButton(OnGigAccept);

            // turns on canvas
            canvasDialogue.SetActive(true);

            // turns off player movement
            sPlayer.playerGlobal.ToggleMovement(false);

            // sets interactable button
            sGameManager.gm.SetEventSystem(buttonYes);
        }

        // If not recipient - shows the OFFER canvas 
        else
        {
            // is not delivering
            if (!isDelivering)
            {
                characterDialogueTextMesh.text = gigData.gigOfferText;
            }

            // delivery is in progress 
            else
            {
                characterDialogueTextMesh.text = gigData.gigInProgressText;
            }
        }
    }

    public override void OnGigAccept()
    {
        // runs base gig accept
        base.OnGigAccept();

        // toggles bool so canvas will trigger differently
        isDelivering = true;

        // adds item to inventory
        cInventory.inventoryGlobal.AddItem(itemToDeliver);
    }

    public override void OnGigComplete()
    {
        base.OnGigComplete();

        // not delivering
        isDelivering = false;

        // removes item from inventory
        cInventory.inventoryGlobal.RemoveItem(itemToDeliver);
    }

    public override void OnGigReject()
    {
        base.OnGigReject();
    }
}
