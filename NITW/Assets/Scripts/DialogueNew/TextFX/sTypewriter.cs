using TMPro;
using UnityEngine;

namespace AVSim.TextFX
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class sTypewriter : MonoBehaviour
    {
        TextMeshProUGUI text;

        [Header("Typing")]
        public float charactersPerSecond = 35f;

        float timer;
        int visibleCharacters;

        bool typing;

        void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if (!typing)
                return;

            timer += Time.deltaTime;

            float interval = 1f / charactersPerSecond;

            while (timer >= interval)
            {
                timer -= interval;

                visibleCharacters++;

                text.maxVisibleCharacters = visibleCharacters;

                if (visibleCharacters >= text.textInfo.characterCount)
                {
                    typing = false;
                    break;
                }
            }
        }

        public void BeginTyping()
        {
            text.ForceMeshUpdate();

            timer = 0f;
            visibleCharacters = 0;

            text.maxVisibleCharacters = 0;

            typing = true;
        }

        public void Skip()
        {
            typing = false;
            text.maxVisibleCharacters = text.textInfo.characterCount;
        }

        public bool IsTyping => typing;
    }
}