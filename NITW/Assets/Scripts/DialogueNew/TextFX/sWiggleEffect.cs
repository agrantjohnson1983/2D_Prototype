using TMPro;
using UnityEngine;

namespace AVSim.TextFX
{
    public class sWiggleEffect : sTextEffect
    {
        public float amplitude = 3f;
        public float speed = 5f;

        public override void Animate(
            TMP_Text text,
            TMP_TextInfo textInfo,
            int characterIndex,
            float time)
        {
            if (!textInfo.characterInfo[characterIndex].isVisible)
                return;

            var charInfo = textInfo.characterInfo[characterIndex];

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] vertices =
                textInfo.meshInfo[materialIndex].vertices;

            float offset =
                Mathf.Sin(time * speed + characterIndex * .4f)
                * amplitude;

            Vector3 movement = new Vector3(0, offset, 0);

            vertices[vertexIndex + 0] += movement;
            vertices[vertexIndex + 1] += movement;
            vertices[vertexIndex + 2] += movement;
            vertices[vertexIndex + 3] += movement;
        }
    }
}