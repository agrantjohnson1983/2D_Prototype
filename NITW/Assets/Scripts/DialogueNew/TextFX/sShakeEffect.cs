using TMPro;
using UnityEngine;

namespace AVSim.TextFX
{
    public class sShakeEffect : sTextEffect
    {
        public float strength = 1.5f;

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

            Random.InitState(characterIndex + Mathf.FloorToInt(time * 60));

            Vector3 move = Random.insideUnitCircle * strength;

            vertices[vertexIndex + 0] += move;
            vertices[vertexIndex + 1] += move;
            vertices[vertexIndex + 2] += move;
            vertices[vertexIndex + 3] += move;
        }
    }
}