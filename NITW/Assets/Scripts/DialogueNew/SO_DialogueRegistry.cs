using System.Collections.Generic;
using UnityEngine;

namespace AVSim.Dialogue
{
    [System.Serializable]
    public class DialogueRegistryEntry
    {
        public string ID;
        public SO_DialogueDatabase Database;
    }

    [CreateAssetMenu(fileName = "SO_DialogueRegistry", menuName = "Dialogue/Dialogue Registry")]
    public class SO_DialogueRegistry : ScriptableObject
    {
        public List<DialogueRegistryEntry> Entries = new List<DialogueRegistryEntry>();

        private Dictionary<string, SO_DialogueDatabase> _lookup;

        public SO_DialogueDatabase GetDatabase(string id)
        {
            if (_lookup == null)
            {
                BuildLookup();
            }

            if (_lookup.TryGetValue(id, out var db))
            {
                return db;
            }

            Debug.LogWarning("SO_DialogueRegistry: no entry found for ID '" + id + "'.");
            return null;
        }

        private void BuildLookup()
        {
            _lookup = new Dictionary<string, SO_DialogueDatabase>();
            foreach (var entry in Entries)
            {
                if (string.IsNullOrEmpty(entry.ID) || entry.Database == null)
                {
                    continue;
                }

                if (_lookup.ContainsKey(entry.ID))
                {
                    Debug.LogWarning("SO_DialogueRegistry: duplicate ID '" + entry.ID + "' in " + name);
                    continue;
                }

                _lookup[entry.ID] = entry.Database;
            }
        }

        private void OnEnable()
        {
            _lookup = null; // rebuild lazily; entries may have changed in the editor
        }
    }
}
