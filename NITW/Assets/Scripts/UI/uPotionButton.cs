using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uPotionButton : MonoBehaviour
{
    public Image inventoryImage;

    public TextMeshProUGUI inventoryName;

    SO_Item item = null;

    cPotionBrewing potionBrewing;

    private void Start()
    {
        potionBrewing = GetComponentInParent<cPotionBrewing>();
    }

    public void SetButton(SO_Item _item)
    {
        //Debug.Log("Setting button");

        item = _item;
        inventoryImage.sprite = _item.itemSprite;
        inventoryName.text = _item.itemName;
    }

    public void OnItemClick()
    {
        // call potion brewing to brew this baby
        potionBrewing.OnItemClickToAddToBrew(item);

        Destroy(this.gameObject);

        //Debug.Log("Item " + item.itemName + " was clicked in inventory");
    }
}
