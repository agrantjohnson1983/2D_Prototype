using TMPro;
using UnityEngine;

namespace AVSim.TextFX
{
    [RequireComponent(typeof(TMP_Text))]
    public class sTypewriter : MonoBehaviour
    {
        TMP_Text text;

        [Header("Typing")]
        public float charactersPerSecond = 35f;

        float timer;
        float pauseTimer;

        int visibleCharacters;

        bool typing;

        sTextEffectsAnimator animator;

        void Awake()
        {
            text = GetComponent<TMP_Text>();
            animator = GetComponent<sTextEffectsAnimator>();
        }

        void Update()
        {
            if (!typing)
                return;

            if (pauseTimer > 0f)
            {
                pauseTimer -= Time.deltaTime;
                return;
            }

            timer += Time.deltaTime;

            float interval = 1f / charactersPerSecond;

            while (timer >= interval)
            {
                timer -= interval;

                visibleCharacters++;

                text.maxVisibleCharacters = visibleCharacters;

                animator.RevealCharacter(visibleCharacters - 1);

                char revealedCharacter =
                text.textInfo.characterInfo[visibleCharacters - 1].character;

                float delay = GetPunctuationDelay(revealedCharacter);

                Debug.Log($"'{revealedCharacter}'  Unicode: {(int)revealedCharacter}  Delay: {delay}");

                // If this period is followed by another period,
                // don't pause yet. Wait until the last one.
                /*if (revealedCharacter == '.')
                {
                    bool nextIsPeriod =
                        visibleCharacters < text.text.Length &&
                        text.text[visibleCharacters] == '.';

                    if (nextIsPeriod)
                        delay = 0f;
                }*/

                pauseTimer = delay;

                

                if (visibleCharacters >= text.textInfo.characterCount)
                {
                    typing = false;
                    break;
                }
            }
        }

        public void BeginTyping()
        {
            text.ForceMeshUpdate(true);

            Debug.Log(
                "Begin Typing Count: " +
                text.textInfo.characterCount
            );

            timer = 0f;

            pauseTimer = 0f;
            visibleCharacters = 0;

            text.maxVisibleCharacters = 0;

            typing = true;
        }


        float GetPunctuationDelay(char c)
        {
            switch (c)
            {
                case ',':
                    return 0.08f;

                case '.':
                    return 0.20f;

                case '…':      // Unicode ellipsis
                    return 0.50f;

                case '!':
                case '?':
                    return 0.20f;

                default:
                    return 0f;
            }
        }

        public void Skip()
        {
            text.ForceMeshUpdate();

            typing = false;
            text.maxVisibleCharacters = text.textInfo.characterCount;
        }

        public bool IsTyping => typing;
    }
}