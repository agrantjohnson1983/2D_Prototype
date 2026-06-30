using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class sNarrator : MonoBehaviour
{
    public static sNarrator narratorGlobal;

    public TextMeshProUGUI narrationText;

    public GameObject narratorBG;

    public GameObject pressSpaceToContinueText;

    bool isTyping, waitingForInput;

    SO_Narration currentNarration = null;

    int index = 0;

    GameObject[] objectToTurnOnAtEnd;
    GameObject[] objectsToTurnOffAtEnd;

    UnityEvent eventOnComplete = null;

    private void Awake()
    {
        if (narratorGlobal == null)
            narratorGlobal = this;
        else
            Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        narrationText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
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

            NextNarrationBit();
        }
    }

    public void TriggerNarration(SO_Narration _narration, GameObject[] _objectsToTurnOnAtEnd, GameObject[] _objectsToTurnOffAtEnd, UnityEvent _eventCallbackOnComplete)
    {
        sGameManager.gm.ToggleDialogue(true);

        currentNarration = _narration;

        eventOnComplete = _eventCallbackOnComplete;

        objectsToTurnOffAtEnd = _objectsToTurnOffAtEnd;
        objectToTurnOnAtEnd = _objectsToTurnOnAtEnd;

        StartCoroutine(NarrationSequence(_narration.narrationBits[index]));
    }

    IEnumerator NarrationSequence(SO_Narration.NarratorBits _narrationBit)
    {
        // resets dialogue text to remove exisiting text
        narrationText.text = "";


        // typewriter seqeunce
        isTyping = true;
        foreach (char _c in _narrationBit.textNarration)
        {
            // if it's not typing (this will allow a quickstop) then it breaks;
            if (!isTyping) { narrationText.text = _narrationBit.textNarration; break; } // skip pressed

            // adds letter 
            narrationText.text += _c;

            // waits for the letters per sec value from the SO
            yield return new WaitForSeconds(_narrationBit.typingLettersPerSec);
        }

        // not typing
        isTyping = false;


        // if the next bit is null or has nothing in array
        if (_narrationBit.nextNarratorBit == null || _narrationBit.nextNarratorBit.Length == 0)
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
            // calls the next branching dialogue bit - there should only be one so the array index will always be 0
            NextNarrationBit();
        }
    }

    void NextNarrationBit()
    {
        // iterates index
        index++;

        // checks if index is greater than bits length
        if(index > currentNarration.narrationBits.Length-1)
        {
            EndNarration();
            return;
        }

        StartCoroutine(NarrationSequence(currentNarration.narrationBits[index]));
    }

    void EndNarration()
    {
        index = 0;
        currentNarration = null;

        narratorBG.SetActive(false);

        sGameManager.gm.ToggleDialogue(false);

        eventOnComplete.Invoke();

        eventOnComplete = null;

        sObjectsToggler.ToggleObjects(objectToTurnOnAtEnd, objectsToTurnOffAtEnd);
    }
}
