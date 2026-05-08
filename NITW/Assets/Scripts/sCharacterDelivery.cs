using Unity.VisualScripting;
using UnityEngine;

public class sCharacterDelivery : sCharacterGigMaster
{
    public bool isRecipient = false;

    public SO_Item itemToDeliver;

    bool isDelivering = false;

    //bool hasItem = false;

    bool gigComplete = false;

    public GameObject[] textResponses;

    public GameObject[] buttons;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Character trigger hit by " + other.gameObject.name);

        // Doesn't trigger after gig complete
        if (other.gameObject.CompareTag("Player") && !gigComplete)
        {
            //Debug.Log("Player detected - turning on dialogue canvas");

            if(isRecipient)
            {
                bool hasItem = false;

                // iterates through entire inventory list
                foreach (SO_Item item in cInventory.inventoryGlobal.ReturnItemList())
                {
                    // checks if item to deliver is in inventory
                    if (item == itemToDeliver && !hasItem)
                    {
                        hasItem = true;
                    }
                }

                // if you have the item then it displays the finish canvas
                if (hasItem)
                {
                    canvasDialogueFinish.SetActive(true);
                }

                // Displays other canvas
                else
                {
                    canvasDialogueOffer.SetActive(true);
                }
            }

            // If not recipient - shows the OFFER canvas 
            else
            {
                if(!isDelivering)
                {
                    canvasDialogueOffer.SetActive(true);
                }

                else
                {
                    canvasDialogueFinish.SetActive(true);
                }
            }
        }

    }

    public void OnGigAccept()
    {
        // toggles bool so canvas will trigger differently
        isDelivering = true;

        // fires event trigger
        gig.TriggerOnGetGig();

        // adds item to inventory
        cInventory.inventoryGlobal.AddItem(itemToDeliver);
    }

    public void OnDeliveryComplete()
    {
        // sets gig complete to true
        gigComplete = true;

        // removes item from inventory
        cInventory.inventoryGlobal.RemoveItem(itemToDeliver);

        // triggers SO
        gig.TriggerOnGigComplete();

        // clears canvas in 5 seconds
        Invoke("ClearCanvas", 5f);
    }

    public void OnDeliveryReject()
    {
        // clears canvas in 5 seconds
        Invoke("ResetCanvas", 2.5f);
    }

    void ResetCanvas()
    {
        // turns off canvases
        canvasDialogueFinish.SetActive(false);
        canvasDialogueOffer.SetActive(false);

        // resets all text responses
        foreach(GameObject go in textResponses)
        {
            go.SetActive(false); 
        }

        // turns buttons back on
        foreach (GameObject go in buttons)
        {
            go.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ResetCanvas();

            //Debug.Log("Player leaving trigger - turning off dialogue canvas and resetting dialogue tree");

            //canvasDialogue.SetActive(false);

            // resets dialogue
            //dialogueController.ResetDialogue();
        }
    }
}
