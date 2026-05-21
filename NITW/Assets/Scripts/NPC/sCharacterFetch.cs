using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class sCharacterFetch : sCharacterGigMaster
{
    [Header("Fetch Info")]
    public SO_Item itemToFetch;

    bool CheckForItem()
    {
        bool _hasItem = false;

        foreach (SO_Item item in cInventory.inventoryGlobal.ReturnItemList())
        {
            if (item == itemToFetch && !_hasItem)
            {
                _hasItem = true;
            }
        }

        return _hasItem;
    }

    public override void OnGigComplete()
    {
        // base gig complete
        base.OnGigComplete();

        // removes item from inventory
        cInventory.inventoryGlobal.RemoveItem(itemToFetch);
    }

    public override void TriggerInteraction()
    {
        // base interaction
        base.TriggerInteraction();

        // returns if not doing gig
        if(!gigData.CheckIfDoingGig())
        {
            return;
        }

        // toggles on dialogue
        canvasDialogue.SetActive(true);

        // if you have the fetch item
        if(CheckForItem())
        {
            // turns off player movement
            sPlayer.playerGlobal.ToggleMovement(false);

            // sets proposition text if you have item
            characterDialogueTextMesh.text = gigData.gigPropositionText;

            // turns on buttons
            ToggleButtons(true);

            // sets first button as selected
            sGameManager.gm.SetEventSystem(buttonYes);
        }

        // don't have fetch item but are doing gig
        else
        {
            characterDialogueTextMesh.text = gigData.gigInProgressText;
            Invoke("ResetCanvas", 1f);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
}
