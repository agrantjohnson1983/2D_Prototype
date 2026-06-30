using UnityEngine;

[CreateAssetMenu(fileName = "SO_Narration", menuName = "Scriptable Objects/SO_Narration")]
public class SO_Narration : ScriptableObject
{
    public NarratorBits[] narrationBits;

    [System.Serializable]
    public class NarratorBits
    {
        public float typingLettersPerSec = 0.1f;

        [TextArea(2, 5)]
        public string textNarration;

        public NarratorBits[] nextNarratorBit;

        [HideInInspector]
        public Vector2 editorPosition;
    }
}
