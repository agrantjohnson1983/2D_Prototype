using UnityEngine;

public class sGrabbableItem : MonoBehaviour
{
    public SO_Item item;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            cInventory.inventoryGlobal.AddItem(item);

            Destroy(this.gameObject);
        }
    }
}
