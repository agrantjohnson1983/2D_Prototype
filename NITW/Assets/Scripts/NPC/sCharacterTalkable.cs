using JetBrains.Annotations;
using UnityEngine;

public class sCharacterTalkable : sCharacterNPC_BASE
{
    public SO_Dialogue dialogue;

    public override void TriggerInteraction()
    {
        base.TriggerInteraction();

        // gm toggles dialogue
        sGameManager.gm.ToggleDialoge(true);

        // Calls dialogue mgr to start Dialogue
        sDialogueManager.dialogueManagerGlobal.StartDialogue(dialogue, eDialogueBoxLocation.center);
    }
}
