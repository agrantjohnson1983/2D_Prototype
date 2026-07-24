using AVSim.TextFX;
using System.Collections.Generic;

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