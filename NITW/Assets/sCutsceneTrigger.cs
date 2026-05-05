using UnityEngine;

public class sCutsceneTrigger : MonoBehaviour
{
    public bool triggerOnEnable = false;

    public SO_Dialogue dialogue;

    private void OnEnable()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartScene()
    {
        sDialogueManager.dialogueManagerGlobal.StartDialogue(dialogue, eDialogueBoxLocation.center);
    }
}
