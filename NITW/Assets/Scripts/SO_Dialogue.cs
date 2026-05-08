using UnityEngine;

[CreateAssetMenu(fileName = "SO_Dialogue", menuName = "Scriptable Objects/SO_Dialogue")]
public class SO_Dialogue : ScriptableObject
{
    public DialogueBits[] dialogueBits;

    [System.Serializable]
    public class DialogueBits
    {
        public string characterName;
        public Sprite characterImage;
        public float typingLettersPerSec = 0.1f;
        //public float delayAtEndOfTyping = 1f;
        public string textDialogue;
        public DialogueChoice[] choices;
        public DialogueBits nextDialogueBit;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string textChoice;
        public DialogueBits nextDialogueBit;
    }
}
