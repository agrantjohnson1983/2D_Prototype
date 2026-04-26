using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class sCharacterFetch : MonoBehaviour
{
    public GameObject canvasDialogueOffer, canvasDialogueFinish;

    public SO_Gig gig;

    public SO_Item itemToFetch;

    bool hasItem = false;

    bool gigComplete = false;

    // This handles turning on and off the canvas
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Character trigger hit by " + other.gameObject.name);

        if (other.gameObject.CompareTag("Player") && !gigComplete)
        {
            //Debug.Log("Player detected - turning on dialogue canvas");

            foreach(SO_Item item in cInventory.inventoryGlobal.ReturnItemList())
            {
                if(item == itemToFetch && !hasItem)
                {
                    hasItem = true;
                }
            }

            if(hasItem)
            {
                canvasDialogueFinish.SetActive(true);
            }

            else
            {
                canvasDialogueOffer.SetActive(true);
            }
        }

    }

    public void OnGigAccept()
    {
        gig.TriggerOnGetGig();
    }

    public void OnFetchComplete()
    {
        // sets gig complete to true
        gigComplete = true;

        // removes item from inventory
        cInventory.inventoryGlobal.RemoveItem(itemToFetch);

        // triggers SO
        gig.TriggerOnGigComplete();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvasDialogueFinish.SetActive(false);
            canvasDialogueOffer.SetActive(false);

            //Debug.Log("Player leaving trigger - turning off dialogue canvas and resetting dialogue tree");

            //canvasDialogue.SetActive(false);

            // resets dialogue
            //dialogueController.ResetDialogue();
        }
    }
}
