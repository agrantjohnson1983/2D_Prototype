using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class uInventoryButton : MonoBehaviour
{
    public Image inventoryImage;

    public TextMeshProUGUI inventoryName;

    SO_Item item = null;

    public void SetButton(SO_Item _item)
    {
        Debug.Log("Setting button");

        item = _item;
        inventoryImage.sprite = _item.itemSprite;
        inventoryName.text = _item.itemName;
    }

    public void OnItemClick()
    {
        // ???? what should happen here?

        Debug.Log("Item " + item.itemName + " was clicked in inventory");
    }    
}
