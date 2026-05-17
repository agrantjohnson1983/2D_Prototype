using UnityEngine;
using TMPro;

public class uTextCharacter : MonoBehaviour
{
    public TextMeshProUGUI characterText;

    //public static uTextCharacter textCharacterGlobal;

    public Vector3 spawnOffset;

    public Transform parent;

    private void Awake()
    {
        //if (textCharacterGlobal == null)
        //    textCharacterGlobal = this;
        //else
        //    Destroy(this.gameObject);

        Invoke("TurnOffText", 0f);
    }

    public void SetText(string _text, float _duration)
    {
        // sets transform to character pos + offset
        this.transform.position =
               parent.transform.position + spawnOffset;

        // sets text
        characterText.text = _text;

        // turns text back off after duration
        Invoke("TurnOffText", _duration);
    }

    public void SetTransform(Transform _transform)
    {
        parent = _transform;
    }

    void TurnOffText()
    {
        characterText.text = "";
    }
}
