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

        public List<TextEffectType> Effects =
            new List<TextEffectType>();
    }

    public class ParsedText
    {
        public string VisibleText;

        public List<ParsedCharacter> Characters =
            new List<ParsedCharacter>();
    }
}