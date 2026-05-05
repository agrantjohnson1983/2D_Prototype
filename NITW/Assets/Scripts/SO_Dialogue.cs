using UnityEngine;

[CreateAssetMenu(fileName = "SO_Dialogue", menuName = "Scriptable Objects/SO_Dialogue")]
public class SO_Dialogue : ScriptableObject
{
    public DialogueBits[] dialogueBits;

    [System.Serializable]
    public class DialogueBits
    {
        public Sprite characterImage;
        public float duration;
        public string textDialogue;
    }
}
