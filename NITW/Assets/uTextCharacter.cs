using UnityEngine;
using TMPro;

public class uTextCharacter : MonoBehaviour
{
    public TextMeshProUGUI characterText;

    public uTextCharacter textCharacterGlobal;
    private void Awake()
    {
        textCharacterGlobal = this;
    }

    public void SetText(string _text)
    {
        characterText.text = _text;
    }
}
