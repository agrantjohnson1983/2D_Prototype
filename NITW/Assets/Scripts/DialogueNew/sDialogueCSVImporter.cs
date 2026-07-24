using System.Collections.Generic;
using UnityEngine;

namespace AVSim.Dialogue
{
    // Expected column order (header row required):
    // NodeID, Speaker, Text, Choice1Text, Choice1Target, Choice2Text, Choice2Target, Choice3Text, Choice3Target, SetFlag, ContinueTarget, TriggerEvent
    public static class sDialogueCSVImporter
    {
        public static List<DialogueLine> Parse(TextAsset csv)
        {
            var result = new List<DialogueLine>();

            if (csv == null)
            {
                Debug.LogWarning("sDialogueCSVImporter: no CSV assigned.");
                return result;
            }

            string[] rows = csv.text.Replace("\r\n", "\n").Split('\n');

            for (int i = 1; i < rows.Length; i++)
            {
                string row = rows[i];
                if (string.IsNullOrWhiteSpace(row))
                {
                    continue;
                }

                string[] cols = SplitCsvRow(row);
                if (cols.Length < 3 || string.IsNullOrWhiteSpace(cols[0]))
                {
                    continue;
                }

                var line = new DialogueLine
                {
                    NodeID = cols[0].Trim(),
                    Speaker = SafeGet(cols, 1),
                    Text = SafeGet(cols, 2),
                    SetFlag = SafeGet(cols, 9),
                    ContinueTarget = SafeGet(cols, 10).Trim(),
                    TriggerEvent = SafeGet(cols, 11).Trim()
                };

                AddChoiceIfPresent(line, SafeGet(cols, 3), SafeGet(cols, 4));
                AddChoiceIfPresent(line, SafeGet(cols, 5), SafeGet(cols, 6));
                AddChoiceIfPresent(line, SafeGet(cols, 7), SafeGet(cols, 8));

                result.Add(line);
            }

            return result;
        }

        private static void AddChoiceIfPresent(DialogueLine line, string text, string target)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            line.Choices.Add(new DialogueChoice
            {
                Text = text.Trim(),
                TargetNodeID = target.Trim()
            });
        }

        private static string SafeGet(string[] cols, int index)
        {
            if (index < 0 || index >= cols.Length)
            {
                return string.Empty;
            }

            return cols[index];
        }

        // Handles quoted fields that may contain commas, e.g. "Yeah, that's me"
        private static string[] SplitCsvRow(string row)
        {
            var fields = new List<string>();
            var current = new System.Text.StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < row.Length; i++)
            {
                char c = row[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < row.Length && row[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            fields.Add(current.ToString());
            return fields.ToArray();
        }
    }
}
