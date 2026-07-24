using AVSim.TextFX;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



namespace AVSim.TextFX
{
    [RequireComponent(typeof(TMP_Text))]
    public class sTextEffectsAnimator : MonoBehaviour
    {
        TMP_Text text;

        private ParsedText currentParsedText;

        [SerializeField]
        bool autoParse = false;

        void Awake()
        {
            text = GetComponent<TMP_Text>();
        }
        void Start()
        {
            if (!autoParse)
                return;

            ParsedText parsed =
                sTextTagParser.Parse(text.text);

            SetParsedText(parsed);
        }


        void Update()
        {
            AnimateText();
        }


        void AnimateText()
        {
            if (currentParsedText == null)
                return;

            text.ForceMeshUpdate();

            TMP_TextInfo textInfo = text.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (i >= text.maxVisibleCharacters)
                    continue;

                if (!charInfo.isVisible)
                    continue;


                //bool wiggle = currentParsedText.Characters[i]
                //    .Effects.Contains(TextEffectType.Wiggle);


                //if (wiggle)
                //{
                //    AnimateWiggle(charInfo, textInfo);
                //}

                if (i >= currentParsedText.Characters.Count)
                    continue;

                ParsedCharacter characterData = currentParsedText.Characters[i];

                foreach (TextEffectType effect in characterData.Effects)
                {
                    switch (effect)
                    {
                        case TextEffectType.Wiggle:
                            AnimateWiggle(charInfo, textInfo);
                            break;

                        case TextEffectType.Wave:
                            AnimateWave(i, charInfo, textInfo);
                            break;

                        case TextEffectType.Shake:
                            AnimateShake(charInfo, textInfo);
                            break;

                        case TextEffectType.Pulse:
                            AnimatePulse(i, charInfo, textInfo);
                            break;

                        case TextEffectType.Rotate:
                            AnimateRotate(i, charInfo, textInfo);
                            break;
                    }
                }
            }

            text.UpdateVertexData();
        }

        void AnimateWiggle(
            TMP_CharacterInfo charInfo,
            TMP_TextInfo textInfo)
        {
            float y =
                Mathf.Sin(Time.time * 10f + charInfo.vertexIndex)
                * 5f;

            TransformCharacter(
                charInfo,
                textInfo,
                Vector3.up * y,
                0,
                Vector3.one);
        }

        void AnimateWave(
            int characterIndex,
            TMP_CharacterInfo charInfo,
            TMP_TextInfo textInfo)
        {
            ParsedCharacter data = currentParsedText.Characters[characterIndex];

            if (!data.Revealed)
                return;

            float delay = .15f;

            if (Time.time < data.RevealTime + delay)
                return;

            float y =
                Mathf.Sin(Time.time * 4f + characterIndex * .5f)
                * 8f;

            TransformCharacter(
                charInfo,
                textInfo,
                Vector3.up * y,
                0,
                Vector3.one);
        }

        void AnimateRotate(
            int characterIndex,
            TMP_CharacterInfo charInfo,
            TMP_TextInfo textInfo)
        {
            float angle =
                Mathf.Sin(Time.time * 5f + characterIndex)
                * 15f;

            TransformCharacter(
                charInfo,
                textInfo,
                Vector3.zero,
                angle,
                Vector3.one);
        }

        void AnimateShake(
            TMP_CharacterInfo charInfo,
            TMP_TextInfo textInfo)
        {
            float speed = 25f;
            float strength = 2f;

            float x =
                (Mathf.PerlinNoise(
                    Time.time * speed,
                    charInfo.vertexIndex) - 0.5f)
                * strength * 2f;

            float y =
                (Mathf.PerlinNoise(
                    charInfo.vertexIndex,
                    Time.time * speed) - 0.5f)
                * strength * 2f;

            OffsetCharacter(
                charInfo,
                textInfo,
                new Vector3(x, y, 0));
        }

        void AnimatePulse(
            int characterIndex,
            TMP_CharacterInfo charInfo,
        TMP_TextInfo textInfo)
        {
            float scale =
                1f +
                Mathf.Sin(
                    Time.time * 5f +
                    characterIndex * 0.4f)
                * 0.15f;

            TransformCharacter(
                charInfo,
                textInfo,
                Vector3.zero,
                0f,
                Vector3.one * scale);
        }

        void OffsetCharacter(
            TMP_CharacterInfo charInfo,
            TMP_TextInfo textInfo,
            Vector3 offset)
        {
            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] vertices =
                textInfo.meshInfo[materialIndex].vertices;

            for (int i = 0; i < 4; i++)
                vertices[vertexIndex + i] += offset;
        }


        public void SetParsedText(ParsedText parsed)
        {
            text.text = parsed.VisibleText;

            currentParsedText = parsed;

            text.ForceMeshUpdate();

            Debug.Log(
                "TMP Character Count After Set: " +
                text.textInfo.characterCount
            );
        }

        public void RevealCharacter(int index)
        {
            if (currentParsedText == null)
                return;

            if (index < 0 || index >= currentParsedText.Characters.Count)
                return;

            ParsedCharacter character =
                currentParsedText.Characters[index];

            character.Revealed = true;
            character.RevealTime = Time.time;
        }

        void TransformCharacter(
            TMP_CharacterInfo charInfo,
            TMP_TextInfo textInfo,
            Vector3 positionOffset,
            float rotationDegrees,
            Vector3 scale)
        {
            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            // Find character center
            Vector3 center =
                (vertices[vertexIndex] + vertices[vertexIndex + 2]) * 0.5f;

            Matrix4x4 matrix =
                Matrix4x4.TRS(
                    positionOffset,
                    Quaternion.Euler(0, 0, rotationDegrees),
                    scale);

            for (int i = 0; i < 4; i++)
            {
                Vector3 offset = vertices[vertexIndex + i] - center;
                vertices[vertexIndex + i] =
                    center + matrix.MultiplyPoint3x4(offset);
            }
        }


        
    }
}