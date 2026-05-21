using UnityEngine;

public class sCharacterTalkable : MonoBehaviour
{

    public GameObject canvasDialogue;

    //sDialogueController dialogueController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //dialogueController = GetComponentInChildren<sDialogueController>();

        canvasDialogue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This handles turning on and off the canvas
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Character trigger hit by " + other.gameObject.name);

        if(other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player detected - turning on dialogue canvas");

            canvasDialogue.SetActive(true);
        }
            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player leaving trigger - turning off dialogue canvas and resetting dialogue tree");

            canvasDialogue.SetActive(false);

            // resets dialogue
            //dialogueController.ResetDialogue();
        }
    }
}
