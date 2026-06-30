using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum eDialogueBoxLocation { top, center, bottom }

public class sDialogueManager : MonoBehaviour
{
    public static sDialogueManager dialogueManagerGlobal;

    //public GameObject dialogueTransforms;

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

    //current dialogue ref
    SO_Dialogue currentDialogue = null;

    // button prefab
    public GameObject pButtonChoice;

    // where the buttons spawn
    public Transform transformButtonsChoice;

    // active button references
    private List<GameObject> activeButtons = new List<GameObject>();

    UnityEvent onCompleteEvent = null;

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

        //dialogueTransforms.SetActive(false);
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
    public void StartDialogue(SO_Dialogue _dialogue, eDialogueBoxLocation _boxLocation, UnityEvent _OnCompleteEvent)
    {
        //Debug.Log("Dialogue starting with " + _dialogue.ToString());

        dialogueBox.SetActive(true);

        // sets current dialogue
        currentDialogue = _dialogue;

        onCompleteEvent = _OnCompleteEvent;

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
            EndDialogue(currentDialogue.turnsOffDialogueAtEnd);
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
        textSpeakerName.text = _dialogueBit.character.characterName;

        // resets dialogue text to remove exisiting text
        textDialogue.text = "";

        // changes character head sprite
        imageCharacter.sprite = _dialogueBit.character.characterHead;

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
            // if the next bit is null or has nothing in array
            if (_dialogueBit.nextDialogueBit == null || _dialogueBit.nextDialogueBit.Length == 0)
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
                // delay before next bit
                yield return new WaitForSeconds(_dialogueBit.delayBetweenBits);

                // calls the next branching dialogue bit - there should only be one so the array index will always be 0
                NextBranchingDialogueBit(_dialogueBit.nextDialogueBit[0]);
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
                SpawnButton(_choice.textButtonChoice, () =>
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

            // sets active button to first index in the array
            sGameManager.gm.SetEventSystem(activeButtons[0]);
        }
    }

    void SpawnButton(string label, UnityEngine.Events.UnityAction onClick)
    {
        GameObject go = Instantiate(pButtonChoice, transformButtonsChoice);
        go.GetComponentInChildren<TextMeshProUGUI>().text = label;

        // sets selected color to green
        ColorBlock cb = go.GetComponent<Button>().colors;
        cb.selectedColor = Color.green;
        go.GetComponent<Button>().colors = cb;


        go.GetComponent<Button>().onClick.AddListener(onClick);
        activeButtons.Add(go);
    }

    void ClearButtons()
    {
        foreach (var b in activeButtons) Destroy(b);
        activeButtons.Clear();
    }

    void EndDialogue(bool _turnsOffDialogue)
    {
        // turns off dialogue canvas box
        //dialogueTransforms.SetActive(false);

        // clears buttons
        ClearButtons();

        // sets current dialogue to null
        currentDialogue = null;

        // resets index
        currentIndex = 0;

        dialogueBox.SetActive(false);

        if (onCompleteEvent != null)
            onCompleteEvent.Invoke();

        // toggles dialogue mode off
        if(_turnsOffDialogue)
            sGameManager.gm.ToggleDialogue(false);
    }
}
