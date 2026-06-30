using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class sCharacterTalkable : sCharacterNPC_BASE
{
    public SO_Dialogue dialogue;

    public UnityEvent OnCompleteEvent;

    public override void TriggerInteraction()
    {
        base.TriggerInteraction();

        // gm toggles dialogue
        sGameManager.gm.ToggleDialogue(true);

        // Calls dialogue mgr to start Dialogue
        sDialogueManager.dialogueManagerGlobal.StartDialogue(dialogue, eDialogueBoxLocation.center, OnCompleteEvent);
    }
}
