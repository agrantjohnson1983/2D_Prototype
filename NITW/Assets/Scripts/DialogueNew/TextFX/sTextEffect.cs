using TMPro;
using UnityEngine;

namespace AVSim.TextFX
{
    public abstract class sTextEffect
    {
        public abstract void Animate(
            TMP_Text text,
            TMP_TextInfo textInfo,
            int characterIndex,
            float time);
    }
}