using UnityEngine;

public class sInteractable : MonoBehaviour
{
    protected sPlayer player;

    [Header("Interaction Info")]

    public KeyCode interactKeyCode;

    protected bool canInteract = false;
    public bool canInteractOutsideOnly = true;
    public bool canInteractWhenFlying = false;

    protected sCharacterInteraction interactionCharacterController = null;

    [Header("Popup")]
    public string interactionPopupText;// = "Hold Space to Enter";

    public float buttonHoldDuration = 0f;

    public Vector3 popupOffset = new Vector3(0f, 0.5f, 0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        player = sPlayer.playerGlobal;
    }

    public virtual void Update()
    {
        //if (player == null) return;

        //// Checks if you can interact outside or when flying
        //if (player.CheckIfOutside() != canInteractOutsideOnly || player.CheckIfFlying() != canInteractWhenFlying) return;

        //// if you are touching door and press the interact key
        //if (canInteract && Input.GetKey(interactKeyCode))
        //{
        //    TriggerInteraction();
        //}
    }

    public KeyCode GetKeyCodeForInteraction()
    {
        return interactKeyCode;
    }

    public virtual void TriggerInteraction()
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<sCharacterInteraction>(out interactionCharacterController))
        {
            //Debug.Log("Character interaction detected");

            canInteract = true;

            interactionCharacterController.TogglePopup(true, this);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<sCharacterInteraction>(out interactionCharacterController))
        {
            //Debug.Log("Character interaction detected");

            canInteract = true;

            interactionCharacterController.TogglePopup(false, this);
        }
    }

}
