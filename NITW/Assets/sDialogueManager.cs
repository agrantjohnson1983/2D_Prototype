using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum eDialogueBoxLocation { top, center, bottom }

public class sDialogueManager : MonoBehaviour
{
    public static sDialogueManager dialogueManagerGlobal;

    public GameObject dialogueCanvas;

    public GameObject dialogueBox;

    // Transforms for box spawnning
    public Transform 
        transformTop, 
        transformCenter, 
        transformBottom;

    public Image imageCharacter;

    public TextMeshProUGUI textDialogue;

    int currentIndex;

    private void Awake()
    {
        // sets the singleton
        if (dialogueManagerGlobal == null)
            dialogueManagerGlobal = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        dialogueCanvas.SetActive(false);
    }

    // Call this at the beginning of a dialogue sequence
    public void StartDialogue(SO_Dialogue _dialogue, eDialogueBoxLocation _boxLocation)
    {
        Debug.Log("Dialogue starting with " + _dialogue.ToString());

        // Turns off character movement
        sCharacterController.characterControllerGlobal.SetCanMove(false);

        // turns off main canvas
        sGameManager.gm.ToggleCanvasMain(false);

        // turns on dialogue canvas
        dialogueCanvas.SetActive(true);

        // Sets the box location to top/mid/low transform
        switch(_boxLocation)
        {
            case eDialogueBoxLocation.top:

                dialogueBox.transform.parent = transformTop;

                break;

            case eDialogueBoxLocation.center:

                dialogueBox.transform.parent = transformCenter;

                break;

            case eDialogueBoxLocation.bottom:

                dialogueBox.transform.parent = transformBottom;

                break;
        }

        // resets local position to zero
        dialogueBox.transform.localPosition = Vector3.zero;

        // sets index to 0
        currentIndex = 0;

        // fires next dialogue
        NextDialogue(_dialogue);
    }

    // Call this to trigger the actual dialogue sequence - checks if the sequence "bits" are done
    void NextDialogue(SO_Dialogue _dialogue)
    {
        // checks if dialogue index is greater than bits array length
        if (currentIndex < _dialogue.dialogueBits.Length)
        {
            StartCoroutine(DialogueSequence(_dialogue));
        }

        else
        {
            // turns off dialogue canvas box
            dialogueCanvas.SetActive(false);

            // turns character movement back on
            sCharacterController.characterControllerGlobal.SetCanMove(true);

            // turns main canvas back on
            sGameManager.gm.ToggleCanvasMain(true);
        }
    }

    IEnumerator DialogueSequence(SO_Dialogue _dialogue)
    {
        // sets counter to 0
        float counter = 0f;

        // changes dialogue text
        textDialogue.text = _dialogue.dialogueBits[currentIndex].textDialogue;

        // changes character sprite
        imageCharacter.sprite = _dialogue.dialogueBits[currentIndex].characterImage;

        // runs a timer for the dialogue bit duration
        while(counter < _dialogue.dialogueBits[currentIndex].duration)
        {
            counter += Time.deltaTime;

            yield return null;
        }

        // increments index
        currentIndex++;

        // calls next dialogue
        NextDialogue(_dialogue);
    }
}
