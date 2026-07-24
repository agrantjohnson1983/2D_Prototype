using UnityEngine;
using UnityEngine.Events;

namespace AVSim.Dialogue
{
    // Attach to any GameObject that should react to a dialogue event: a door
    // to open, an object to switch on, a sound to play, whatever. Give it the
    // same string used in the spreadsheet's TriggerEvent column and wire up
    // the response entirely in the Inspector - no code needed per event.
    public class sDialogueEventListener : MonoBehaviour
    {
        [Tooltip("Must exactly match a TriggerEvent value in the spreadsheet.")]
        public string EventID;

        public UnityEvent OnTriggered;

        private void OnEnable()
        {
            if (sDialogueSheetManager.Instance != null)
            {
                Debug.Log("DialogueEvent added for " + EventID);
                sDialogueSheetManager.Instance.OnDialogueEvent += HandleDialogueEvent;
            }
        }

        private void OnDisable()
        {
            if (sDialogueSheetManager.Instance != null)
            {
                sDialogueSheetManager.Instance.OnDialogueEvent -= HandleDialogueEvent;
            }
        }

        private void HandleDialogueEvent(string eventID)
        {
            if (!string.IsNullOrEmpty(EventID) && EventID == eventID)
            {
                Debug.Log("String found! Triggering event " + eventID);
                OnTriggered?.Invoke();
            }
        }
    }
}
