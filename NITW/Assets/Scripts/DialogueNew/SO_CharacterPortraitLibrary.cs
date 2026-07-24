using System.Collections.Generic;
using UnityEngine;

namespace AVSim.Dialogue
{
    [System.Serializable]
    public class CharacterPortraitEntry
    {
        [Tooltip("Must exactly match the Speaker column in the spreadsheet.")]
        public string CharacterName;
        public Sprite Portrait;
    }

    [CreateAssetMenu(fileName = "SO_CharacterPortraitLibrary", menuName = "Dialogue/Character Portrait Library")]
    public class SO_CharacterPortraitLibrary : ScriptableObject
    {
        public List<CharacterPortraitEntry> Entries = new List<CharacterPortraitEntry>();

        private Dictionary<string, Sprite> _lookup;

        public Sprite GetPortrait(string characterName)
        {
            if (string.IsNullOrEmpty(characterName))
            {
                return null;
            }

            if (_lookup == null)
            {
                BuildLookup();
            }

            if (_lookup.TryGetValue(characterName, out var sprite))
            {
                return sprite;
            }

            Debug.LogWarning("SO_CharacterPortraitLibrary: no portrait for '" + characterName + "' in " + name);
            return null;
        }

        private void BuildLookup()
        {
            _lookup = new Dictionary<string, Sprite>();
            foreach (var entry in Entries)
            {
                if (string.IsNullOrEmpty(entry.CharacterName))
                {
                    continue;
                }

                if (_lookup.ContainsKey(entry.CharacterName))
                {
                    Debug.LogWarning("SO_CharacterPortraitLibrary: duplicate name '" + entry.CharacterName + "' in " + name);
                    continue;
                }

                _lookup[entry.CharacterName] = entry.Portrait;
            }
        }

        private void OnEnable()
        {
            _lookup = null; // rebuild lazily; entries may have changed in the editor
        }
    }
}
