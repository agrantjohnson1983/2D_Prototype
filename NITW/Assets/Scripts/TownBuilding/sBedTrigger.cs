using UnityEngine;

public class sBedTrigger : sInteractable
{
    public GameObject bedCanvas;

    //public GameObject firstButton;

    public override void TriggerInteraction()
    {
        base.TriggerInteraction();

        bedCanvas.SetActive(true);
    }
}
