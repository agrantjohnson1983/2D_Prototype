using UnityEngine;
using TMPro;
using System.Collections;

public class uTextCharacter : MonoBehaviour
{
    public TextMeshProUGUI characterText;

    //public static uTextCharacter textCharacterGlobal;

    public Vector3 spawnOffset;

    public Transform parent;

    bool isTyping = false;

    private void Awake()
    {
        //if (textCharacterGlobal == null)
        //    textCharacterGlobal = this;
        //else
        //    Destroy(this.gameObject);

        Invoke("TurnOffText", 0f);
    }

    private void Start()
    {
        //typewriter = GetComponent<uTypewriter>();
    }

    // optional parameter for typewriter speed
    public void SetText(string _text, float _duration, float _typewriterSpeed = 0.1f)
    {
        // sets transform to character pos + offset
        this.transform.position =
               parent.transform.position + spawnOffset;

        // sets text
        characterText.text = _text;

        // typewriter effect
        StartCoroutine(TypewriterEffect(characterText, _typewriterSpeed, _duration));

        // turns text back off after duration
        //Invoke("TurnOffText", _duration);
    }

    IEnumerator TypewriterEffect(TextMeshProUGUI _textMesh, float _rate, float _duration)
    {
        string textToDisplay = _textMesh.text;

        // sets speaker name
        _textMesh.text = "";

        // typewriter seqeunce
        isTyping = true;
        foreach (char _c in textToDisplay)
        {
            // if it's not typing (this will allow a quickstop) then it breaks;
            if (!isTyping) { _textMesh.text = textToDisplay; break; } // skip pressed

            // adds letter 
            _textMesh.text += _c;

            // waits for the letters per sec value from the SO
            yield return new WaitForSeconds(_rate);
        }

        isTyping = false;

        yield return new WaitForSeconds(_duration);
        
        TurnOffText();
    }

    public void SetTransform(Transform _transform)
    {
        parent = _transform;
    }

    void TurnOffText()
    {
        characterText.text = "";
        //isTyping = false;
    }
}
