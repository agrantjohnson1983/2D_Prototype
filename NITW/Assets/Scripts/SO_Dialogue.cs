using UnityEngine;

[CreateAssetMenu(fileName = "SO_Dialogue", menuName = "Scriptable Objects/SO_Dialogue")]
public class SO_Dialogue : ScriptableObject
{
    public DialogueBits[] dialogueBits;

    public bool turnsOffDialogueAtEnd = false;

    [System.Serializable]
    public class DialogueBits
    {
        public SO_Character character;

        public float typingLettersPerSec = 0.1f;

        [TextArea(2,5)]
        public string textDialogue;

        public DialogueChoice[] choices;

        public DialogueBits[] nextDialogueBit;

        [HideInInspector]
        public Vector2 editorPosition;

        public float delayBetweenBits = 0f;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string textButtonChoice;

        public DialogueBits nextDialogueBit;
    }
}
