using System.Collections.Generic;
using System.Text;

namespace AVSim.TextFX
{
    public static class sTextTagParser
    {
        public static ParsedText Parse(string input)
        {
            ParsedText parsed = new ParsedText();

            StringBuilder visible = new StringBuilder();

            List<TextEffectType> activeEffects =
                new List<TextEffectType>();

            bool insideTag = false;

            StringBuilder currentTag =
                new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (c == '<')
                {
                    insideTag = true;
                    currentTag.Clear();
                    continue;
                }

                if (c == '>')
                {
                    insideTag = false;

                    ProcessTag(
                        currentTag.ToString(),
                        activeEffects);

                    continue;
                }

                if (insideTag)
                {
                    currentTag.Append(c);
                    continue;
                }

                visible.Append(c);

                ParsedCharacter pc =
                    new ParsedCharacter();

                pc.Character = c;

                pc.Effects.AddRange(activeEffects);

                parsed.Characters.Add(pc);
            }

            parsed.VisibleText = visible.ToString();

            return parsed;
        }

        static void ProcessTag(
            string tag,
            List<TextEffectType> activeEffects)
        {
            switch (tag)
            {
                case "wiggle":
                    activeEffects.Add(TextEffectType.Wiggle);
                    break;

                case "/wiggle":
                    activeEffects.Remove(TextEffectType.Wiggle);
                    break;

                case "wave":
                    activeEffects.Add(TextEffectType.Wave);
                    break;

                case "/wave":
                    activeEffects.Remove(TextEffectType.Wave);
                    break;

                case "rotate":
                    activeEffects.Add(TextEffectType.Rotate);
                    break;

                case "/rotate":
                    activeEffects.Remove(TextEffectType.Rotate);
                    break;

                case "shake":
                    activeEffects.Add(TextEffectType.Shake);
                    break;

                case "/shake":
                    activeEffects.Remove(TextEffectType.Shake);
                    break;

                case "pulse":
                    activeEffects.Add(TextEffectType.Pulse);
                    break;

                case "/pulse":
                    activeEffects.Remove(TextEffectType.Pulse);
                    break;
            }
        }
    }
}