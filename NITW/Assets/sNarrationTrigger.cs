using UnityEngine;
using UnityEngine.Events;

public class sNarrationTrigger : MonoBehaviour
{
    public SO_Narration narrationToTrigger;

    public GameObject[] objectsToTurnOnAtEnd, objectsToTurnOffAtEnd;

    public UnityEvent eventOnComplete;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sNarrator.narratorGlobal.TriggerNarration(narrationToTrigger, objectsToTurnOnAtEnd, objectsToTurnOffAtEnd, eventOnComplete);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
