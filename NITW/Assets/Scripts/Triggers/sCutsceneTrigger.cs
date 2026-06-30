using UnityEngine;
using UnityEngine.Events;

public class sCutsceneTrigger : MonoBehaviour
{
    public bool triggerOnEnable = false;

    public bool destroyAfterTrigger = false;

    public SO_Dialogue dialogue;

    public UnityEvent onCompleteEvent = null;

    private void OnEnable()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartScene();
    }


    void StartScene()
    {
        sPlayer.playerGlobal.DisplayText("", 0f);

        sGameManager.gm.ToggleDialogue(true);

        // Calls dialogue mgr to start Dialogue
        sDialogueManager.dialogueManagerGlobal.StartDialogue(dialogue, eDialogueBoxLocation.center, onCompleteEvent);

        // Destroys after starting scene
        if (destroyAfterTrigger)
            Destroy(this.gameObject);
    }
}
