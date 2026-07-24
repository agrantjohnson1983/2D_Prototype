using System.Collections.Generic;

namespace AVSim.TextFX
{
    public enum TextEffectType
    {
        Wiggle,
        Shake,
        Wave,
        Pulse,
        Rotate
    }

    public class ParsedCharacter
    {
        public char Character;

        public List<TextEffectType> Effects = new();

        public bool Revealed = false;

        public float RevealTime;
    }

    public class ParsedText
    {
        public string VisibleText;

        public List<ParsedCharacter> Characters =
            new List<ParsedCharacter>();
    }
}