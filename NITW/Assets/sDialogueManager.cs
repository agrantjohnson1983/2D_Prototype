using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public enum eDialogueBoxLocation { top, center, bottom }

public class sDialogueManager : MonoBehaviour
{
    public static sDialogueManager dialogueManagerGlobal;

    public GameObject dialogueCanvas;

    public GameObject dialogueBox;

    public GameObject pressSpaceToContinueText;

    // Transforms for box spawnning
    public Transform 
        transformTop, 
        transformCenter, 
        transformBottom;

    public Image imageCharacter;

    public TextMeshProUGUI textDialogue, textSpeakerName;

    int currentIndex;

    bool isTyping = false;
    bool waitingForInput = false;

    SO_Dialogue currentDialogue = null;

    public GameObject pButtonChoice;

    public Transform transformButtonsChoice;

    // active button references
    private List<GameObject> activeButtons = new List<GameObject>();

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
        pressSpaceToContinueText.transform.localScale = Vector3.zero;

        dialogueCanvas.SetActive(false);
    }

    void Update()
    {
        // checks that dialogue box is active
        if (!dialogueBox.activeSelf) return;

        // will return unless space or left click are pressed
        bool pressed = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        if (!pressed) return;

        // checks if typing and will toggle it flase after above button press and then return
        if (isTyping)
        {
            // Skip typewriter — coroutine will check this flag
            isTyping = false;
            return;
        }

        // if waiting for input gets toggled on this will catch it and trigger next dialogue
        if (waitingForInput)
        {
            // toggles to false
            waitingForInput = false;

            // turns off press space to continue text
            pressSpaceToContinueText.transform.localScale = Vector3.zero;

            // increments index
            currentIndex++;

            // calls for next dialogue bit
            NextMainDialogueBit(currentDialogue.dialogueBits);
        }
    }

    // Call this at the beginning of a dialogue sequence
    public void StartDialogue(SO_Dialogue _dialogue, eDialogueBoxLocation _boxLocation)
    {
        //Debug.Log("Dialogue starting with " + _dialogue.ToString());

        // sets current dialogue
        currentDialogue = _dialogue;

        // Turns off character movement
        sCharacterController.characterControllerGlobal.SetCanMove(false);

        // turns off main canvas
        sGameManager.gm.ToggleCanvasMain(false);

        // turns on dialogue canvas
        dialogueCanvas.SetActive(true);

        // Clears Buttons
        ClearButtons();

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
        NextMainDialogueBit(currentDialogue.dialogueBits);
    }

    // Call this to trigger the actual dialogue sequence - checks if the sequence "bits" are done
    void NextMainDialogueBit(SO_Dialogue.DialogueBits[] _dialogueBits)
    {
        //Debug.Log("Next Dialogue Bit is being called");

        // checks if dialogue index is greater than bits array length
        if (currentIndex < _dialogueBits.Length)
        {
            StartCoroutine(DialogueSequence(_dialogueBits[currentIndex]));
        }

        // if the index is greater than the array length
        // then there are no more dialogue bits and dialogue can end
        else
        {
            // ends dialogue
            EndDialogue();
        }
    }

    void NextBranchingDialogueBit(SO_Dialogue.DialogueBits _nextBit)
    {
        // starts a coroutine for the next bit on a branching dialogue
        StartCoroutine(DialogueSequence(_nextBit));
    }

    // this gets called when a choice is clicked
    void ChoiceDialogueBit(SO_Dialogue.DialogueBits _dialogueChoice)
    {
        // starts coroutine for the dialogue sequence with the choice dialogue bit
        StartCoroutine(DialogueSequence(_dialogueChoice));
    }

    IEnumerator DialogueSequence(SO_Dialogue.DialogueBits _dialogueBit)
    {
        // sets speaker name
        textSpeakerName.text = _dialogueBit.characterName;

        // resets dialogue text to remove exisiting text
        textDialogue.text = "";

        // changes character sprite
        imageCharacter.sprite = _dialogueBit.characterImage;

        // typewriter seqeunce
        isTyping = true;
        foreach (char _c in _dialogueBit.textDialogue)
        {
            // if it's not typing (this will allow a quickstop) then it breaks;
            if (!isTyping) { textDialogue.text = _dialogueBit.textDialogue; break; } // skip pressed
            
            // adds letter 
            textDialogue.text += _c;

            // waits for the letters per sec value from the SO
            yield return new WaitForSeconds(_dialogueBit.typingLettersPerSec);
        }

        // not typing
        isTyping = false;

        // if there are no choices then it just waits for user input to go to next dialogue
        if (_dialogueBit.choices == null || _dialogueBit.choices.Length == 0)
        {
            if(_dialogueBit.nextDialogueBit == null)
            {
                // turns on press space to continue text
                pressSpaceToContinueText.gameObject.transform.localScale = Vector3.one;

                // No choices — wait for Space/click in Update()
                waitingForInput = true;
            }

            // this gets called if there is a next non-choice dialogue bit that branches out
            // this should get chained after choice branching has started, but should also work within the main system

            else
            {
                NextBranchingDialogueBit(_dialogueBit.nextDialogueBit);
            }

            
        }

        // if there are choices in the dialogue bit it will spawn and assign buttons
        else
        {
            // clears buttons
            ClearButtons();

            // iterates through each choice in the choices
            foreach (var _choice in _dialogueBit.choices)
            {
                //var captured = choice; // closure capture

                // spawns a button for each choice and sends it text and event subscription
                SpawnButton(_choice.textChoice, () =>
                {
                    if (_choice.nextDialogueBit != null)
                    {
                        // clears buttons
                        ClearButtons();

                        // when the button gets clicked
                        ChoiceDialogueBit(_choice.nextDialogueBit);
                    }

                    else
                    {
                        // if there are no choices on the dialogue bit then it just goes to next dialogue bit
                        //currentIndex++;

                        // toggles on waiting for input
                        waitingForInput = true;

                        pressSpaceToContinueText.gameObject.transform.localScale = Vector3.one;

                        //NextDialogueBit(currentDialogue.dialogueBits);
                    }
                        
                });
            }
        }
    }

    void SpawnButton(string label, UnityEngine.Events.UnityAction onClick)
    {
        GameObject go = Instantiate(pButtonChoice, transformButtonsChoice);
        go.GetComponentInChildren<TextMeshProUGUI>().text = label;
        go.GetComponent<Button>().onClick.AddListener(onClick);
        activeButtons.Add(go);
    }

    void ClearButtons()
    {
        foreach (var b in activeButtons) Destroy(b);
        activeButtons.Clear();
    }

    void EndDialogue()
    {
        // turns off dialogue canvas box
        dialogueCanvas.SetActive(false);

        // clears buttons
        ClearButtons();

        // sets current dialogue to null
        currentDialogue = null;

        // resets index
        currentIndex = 0;

        // turns character movement back on
        sCharacterController.characterControllerGlobal.SetCanMove(true);

        // turns main canvas back on
        sGameManager.gm.ToggleCanvasMain(true);
    }
}
