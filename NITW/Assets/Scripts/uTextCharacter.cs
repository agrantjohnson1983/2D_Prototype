using UnityEngine;
using TMPro;

public class uTextCharacter : MonoBehaviour
{
    public TextMeshProUGUI characterText;

    public static uTextCharacter textCharacterGlobal;

    public Vector3 spawnOffset;
    private void Awake()
    {
        if (textCharacterGlobal == null)
            textCharacterGlobal = this;
        else
            Destroy(this.gameObject);

        Invoke("TurnOffText", 0f);
    }

    public void SetText(string _text, float _duration)
    {
        // sets transform to character pos + offset
        this.gameObject.transform.position = 
               sCharacterController.characterControllerGlobal.transform.position + spawnOffset;

        // sets text
        characterText.text = _text;

        // turns text back off after duration
        Invoke("TurnOffText", _duration);
    }

    void TurnOffText()
    {
        characterText.text = "";
    }
}
