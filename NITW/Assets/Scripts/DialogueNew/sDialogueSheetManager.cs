using System;
using System.Collections.Generic;
using UnityEngine;

namespace AVSim.Dialogue
{
    public class sDialogueSheetManager : MonoBehaviour
    {
        public static sDialogueSheetManager Instance { get; private set; }

        public event Action<DialogueLine> OnLineShown;
        public event Action OnConversationEnded;

        // Fired with the TriggerEvent string whenever a line carrying one is
        // shown. Any sDialogueEventListener with a matching EventID reacts.
        public event Action<string> OnDialogueEvent;

        private SO_DialogueDatabase _activeDatabase;
        private DialogueLine _currentLine;
        private readonly HashSet<string> _flags = new HashSet<string>();

        // Lets a UI that subscribes late (after StartConversation already fired
        // the first OnLineShown) catch up to whatever is currently on screen.
        public DialogueLine CurrentLine => _currentLine;

        public SO_DialogueRegistry startingRegistry;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            StartConversation(startingRegistry, "test");
        }

        public void StartConversation(SO_DialogueDatabase database)
        {
            if (database == null || database.Lines.Count == 0)
            {
                Debug.LogWarning("sDialogueManager: tried to start an empty database.");
                return;
            }

            _activeDatabase = database;
            ShowNode(database.StartNodeID);
        }

        public void StartConversation(SO_DialogueRegistry registry, string conversationID)
        {
            if (registry == null)
            {
                Debug.LogWarning("sDialogueManager: no registry assigned.");
                return;
            }

            var database = registry.GetDatabase(conversationID);
            if (database != null)
            {
                StartConversation(database);
            }
        }

        public void SelectChoice(int choiceIndex)
        {
            if (_currentLine == null || choiceIndex < 0 || choiceIndex >= _currentLine.Choices.Count)
            {
                return;
            }

            var choice = _currentLine.Choices[choiceIndex];
            if (!choice.HasTarget)
            {
                EndConversation();
                return;
            }

            ShowNode(choice.TargetNodeID);
        }

        public void Advance()
        {
            // Called when the current line has no choices (a plain "next" beat).
            if (_currentLine == null || _currentLine.Choices.Count > 0)
            {
                return;
            }

            if (!string.IsNullOrEmpty(_currentLine.ContinueTarget))
            {
                ShowNode(_currentLine.ContinueTarget);
                return;
            }

            EndConversation();
        }

        public bool HasFlag(string flag)
        {
            return !string.IsNullOrEmpty(flag) && _flags.Contains(flag);
        }

        private void ShowNode(string nodeID)
        {
            var line = _activeDatabase.GetLine(nodeID);
            if (line == null)
            {
                EndConversation();
                return;
            }

            if (!string.IsNullOrEmpty(line.SetFlag))
            {
                _flags.Add(line.SetFlag);
            }

            _currentLine = line;
            OnLineShown?.Invoke(line);

            if (!string.IsNullOrEmpty(line.TriggerEvent))
            {
                OnDialogueEvent?.Invoke(line.TriggerEvent);
            }
        }

        private void EndConversation()
        {
            _currentLine = null;
            _activeDatabase = null;
            OnConversationEnded?.Invoke();
        }
    }
}
