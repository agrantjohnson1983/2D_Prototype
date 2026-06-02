using UnityEngine;

public class sLock : MonoBehaviour
{
    public SO_Item keyItem;

    public string textOnUnlock;

    public string textIfNoKey;

    void TryUnlock(bool _hasKey)
    {
        if(_hasKey)
        {
            sPlayer.playerGlobal.DisplayText(textOnUnlock, 2f);
            Destroy(this.gameObject);
        }

        else
        {
            sPlayer.playerGlobal.DisplayText(textIfNoKey, 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // checks for player collision
        if (collision.gameObject.CompareTag("Player"))
        {
            bool hasKey = false;

            // iterates through inventory
            foreach (SO_Item _item in cInventory.inventoryGlobal.ReturnItemList())
            {
                // checks if the item equals key item
                if (_item == keyItem)
                {
                    hasKey = true;
                }
            }

            TryUnlock(hasKey);
        }
    }
}
