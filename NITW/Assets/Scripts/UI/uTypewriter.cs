using UnityEngine;
using TMPro;
using System.Collections;


// Drop this and link a text mesh pro and it will display a typewriter effect upon start

public class uTypewriter : MonoBehaviour
{
    public TextMeshProUGUI text;

    public bool startOnEnable = false;

    public float typingLettersPerSec = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (text != null)
       //     StartCoroutine(TypewriterEffect(text));
    }

    private void OnEnable()
    {
        if (text != null && startOnEnable)
            StartCoroutine(TypewriterEffect(text, typingLettersPerSec));
    }

    public void StartTypewriter(TextMeshProUGUI _textMeshPro, float _lettersPecSec)
    {
        StartCoroutine(TypewriterEffect(_textMeshPro, _lettersPecSec));
    }

    IEnumerator TypewriterEffect(TextMeshProUGUI _textMesh, float _rate)
    {
        string textToDisplay = _textMesh.text;

        // sets speaker name
        _textMesh.text = "";

        // typewriter seqeunce
        //isTyping = true;
        foreach (char _c in textToDisplay)
        {
            // if it's not typing (this will allow a quickstop) then it breaks;
            //if (!isTyping) { textDialogue.text = _dialogueBit.textDialogue; break; } // skip pressed

            // adds letter 
            _textMesh.text += _c;

            // waits for the letters per sec value from the SO
            yield return new WaitForSeconds(typingLettersPerSec);
        }
    }

        // not typing
        //isTyping = false;
}
