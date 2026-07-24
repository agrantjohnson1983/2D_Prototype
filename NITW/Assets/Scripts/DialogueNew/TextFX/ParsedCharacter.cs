using AVSim.TextFX;
using System.Collections.Generic;

[System.Serializable]
public class ParsedCharacter
{
    public char character;

    public List<TextEffectType> effects = new();

    public bool revealed;

    public float revealTime;
}

public class ParsedText
{
    public string VisibleText;

    public List<ParsedCharacter> Characters =
        new List<ParsedCharacter>();
}