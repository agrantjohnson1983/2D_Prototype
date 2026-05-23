using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;

// Attach this to a GameObject with a Trigger Collider.
// When the player enters, a UI popup appears prompting them to hold Space.
// If held for the required duration, OnHoldComplete fires.
public class sCharacterInteraction : MonoBehaviour
{
    [Header("Hold Settings")]
    [Tooltip("How long (in seconds) the player must hold Space to trigger the event.")]
    float holdDuration = 0f;

    [Tooltip("Tag used to identify the player GameObject.")]
    public string playerTag = "Player";

    [Header("UI Reference")]
    [Tooltip("Drag your HoldInteractionUI component here.")]
    public uInteractionPopup interactionUI;

    [Header("Events")]
    [Tooltip("Fires when the hold is completed successfully.")]
    public UnityEvent OnHoldComplete;

    private bool _playerInside = false;
    private bool _holdCompleted = false;
    private float _holdTimer = 0f;

    sInteractable interactable = null;

    // Sets to none to start
    KeyCode currentKeyCode = KeyCode.None;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.CompareTag(playerTag)) return;

    //    _playerInside = true;
    //    _holdCompleted = false;
    //    _holdTimer = 0f;

    //    interactionUI?.ShowPopup();
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!other.CompareTag(playerTag)) return;

    //    _playerInside = false;
    //    _holdTimer = 0f;

    //    interactionUI?.HidePopup();
    //}

    // This gets called by other scripts to toggle popup on/off
    public void TogglePopup(bool _isOn, sInteractable _interactable)
    {
        if(_isOn)
        {
            _playerInside = true;
            _holdCompleted = false;
            _holdTimer = 0f;

            // sets interactable
            interactable = _interactable;

            // subscribes the interactable
            OnHoldComplete.AddListener(interactable.TriggerInteraction);

            // shows popup
            interactionUI?.ShowPopup();

            // sets poup text
            interactionUI?.SetText(_interactable.interactionPopupText);

            // sets hold duration
            holdDuration = _interactable.buttonHoldDuration;

            // converts character position to screen space + offset
            //Vector3 popupPos = Camera.main.WorldToScreenPoint(this.transform.position);

            Vector3 popupPos = _interactable.transform.position + _interactable.popupOffset;

            // sets popup to character
            interactionUI.transform.position = popupPos;

            // sets keycode allowing inteaction controls to work
            currentKeyCode = interactable.GetKeyCodeForInteraction();
        }

        else
        {
            // sets keycode to none which turns off controls
            currentKeyCode = KeyCode.None;

            OnHoldComplete.RemoveAllListeners();

            // sets interactable to null
            interactable = null;

            _playerInside = false;
            _holdTimer = 0f;

            // hides popup
            interactionUI?.HidePopup();
        }
    }

    private void Update()
    {
        if (!_playerInside || _holdCompleted || currentKeyCode == KeyCode.None) return;

        if (Input.GetKey(currentKeyCode))
        {
            _holdTimer += Time.deltaTime;

            float progress = Mathf.Clamp01(_holdTimer / holdDuration);
            interactionUI?.SetProgress(progress);

            if (_holdTimer >= holdDuration)
            {
                _holdCompleted = true;
                interactionUI?.HidePopup();
                OnHoldComplete?.Invoke();
            }
        }
        else
        {
            // Key released - reset progress but keep popup visible
            if (_holdTimer > 0f)
            {
                _holdTimer = 0f;
                interactionUI?.SetProgress(0f);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return;

        Gizmos.color = new Color(0.2f, 0.9f, 0.4f, 0.25f);

        if (col is BoxCollider box)
        {
            Matrix4x4 old = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(box.center, box.size);
            Gizmos.color = new Color(0.2f, 0.9f, 0.4f, 0.8f);
            Gizmos.DrawWireCube(box.center, box.size);
            Gizmos.matrix = old;
        }
        else if (col is SphereCollider sphere)
        {
            Gizmos.DrawSphere(transform.position + sphere.center, sphere.radius * transform.lossyScale.x);
        }
    }
#endif
}