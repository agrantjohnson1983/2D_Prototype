using UnityEngine;

public class sPotionBrewTrigger : sInteractable
{
    public GameObject ui_DisplayOnTriggerEnter;

    public override void TriggerInteraction()
    {
        base.TriggerInteraction();

        //isBrewing = true;

        player.StartPotionBrew();
    }
}
