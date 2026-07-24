using System.Collections.Generic;
using UnityEngine;

namespace AVSim.Dialogue
{
    [CreateAssetMenu(fileName = "SO_DialogueDatabase", menuName = "Dialogue/Dialogue Database")]
    public class SO_DialogueDatabase : ScriptableObject
    {
        [Tooltip("The raw CSV exported from the spreadsheet (Google Sheets / Excel).")]
        public TextAsset SourceCSV;

        [Tooltip("The node ID this conversation opens on.")]
        public string StartNodeID = "start";

        [HideInInspector]
        public List<DialogueLine> Lines = new List<DialogueLine>();

        private Dictionary<string, DialogueLine> _lookup;

        public void RebuildFromCSV()
        {
            Lines = sDialogueCSVImporter.Parse(SourceCSV);
            _lookup = null;
        }

        public DialogueLine GetLine(string nodeID)
        {
            if (_lookup == null)
            {
                _lookup = new Dictionary<string, DialogueLine>();
                foreach (var line in Lines)
                {
                    _lookup[line.NodeID] = line;
                }
            }

            if (_lookup.TryGetValue(nodeID, out var result))
            {
                return result;
            }

            Debug.LogWarning("Dialogue node not found: " + nodeID + " in " + name);
            return null;
        }
    }
}
