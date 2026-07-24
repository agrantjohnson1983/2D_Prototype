using System;
using System.Collections.Generic;

namespace AVSim.Dialogue
{
    [Serializable]
    public struct DialogueChoice
    {
        public string Text;
        public string TargetNodeID;

        public bool HasTarget => !string.IsNullOrEmpty(TargetNodeID);
    }

    [Serializable]
    public class DialogueLine
    {
        public string NodeID;
        public string Speaker;
        public string Text;
        public List<DialogueChoice> Choices = new List<DialogueChoice>();
        public string SetFlag;

        // Only used when Choices is empty. If set, "Continue" advances here
        // instead of ending the conversation - lets you chain plain lines
        // together (a monologue) before a choice or a real ending.
        public string ContinueTarget;

        // Optional. A plain string broadcast the moment this line is shown.
        // Any sDialogueEventListener in the scene with a matching EventID
        // fires its UnityEvent - use this for side effects (open a door, spawn
        // something, play a one-off sound) as opposed to SetFlag, which is
        // just a persistent boolean for later branching checks.
        public string TriggerEvent;

        public bool IsEndOfConversation => Choices.Count == 0 && string.IsNullOrEmpty(ContinueTarget);
    }
}
