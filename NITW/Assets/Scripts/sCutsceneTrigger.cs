using UnityEngine;

public class sCutsceneTrigger : MonoBehaviour
{
    public bool triggerOnEnable = false;

    public bool destroyAfterTrigger = false;

    public SO_Dialogue dialogue;

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
        sGameManager.gm.ToggleDialoge(true);

        // Calls dialogue mgr to start Dialogue
        sDialogueManager.dialogueManagerGlobal.StartDialogue(dialogue, eDialogueBoxLocation.center);

        // Destroys after starting scene
        if (destroyAfterTrigger)
            Destroy(this.gameObject);
    }
}
