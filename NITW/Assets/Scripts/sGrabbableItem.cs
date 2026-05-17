using UnityEngine;

public class sGrabbableItem : MonoBehaviour
{
    public SO_Item item;

    public string grabSuccessText;

    public string audioGrabSuccessCue = "grabSuccess";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            
            // adds item to inventory
            cInventory.inventoryGlobal.AddItem(item);

            sPlayer.playerGlobal.DisplayText(grabSuccessText, 3f);

            sAudioPlayer.audioPlayerGlobal.TriggerSFX(audioGrabSuccessCue, eSFXTriggerType.eSFXtriggerBasic, eAudioMixerType.ui);

            Destroy(this.gameObject);
        }
    }
}
