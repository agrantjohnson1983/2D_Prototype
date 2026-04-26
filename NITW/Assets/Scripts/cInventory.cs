using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class cInventory : MonoBehaviour
{
    public GameObject inventoryOpen;

    public GameObject hud_ButtonInventory;

    public GameObject inventoryItemPrefab;

    List<SO_Item> itemList;

    List<GameObject> itemButtonList;

    public Transform inventoryTransform;

    public static cInventory inventoryGlobal;

    private void Awake()
    {
        inventoryGlobal = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemList = new List<SO_Item>();

        itemButtonList = new List<GameObject>();

        inventoryOpen.SetActive(false);
    }

    public void OnHUD_InventoryButtonClick()
    {
        // turns off hud button
        hud_ButtonInventory.SetActive(false);

        // turns on inventory
        inventoryOpen.SetActive(true);

        // iterates through item count
        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject tempObj;

            // spawns inventory button
            tempObj = Instantiate(inventoryItemPrefab, inventoryTransform);

            // sets button from item list
            tempObj.GetComponent<uInventoryButton>().SetButton(itemList[i]);

            // Add to List
            itemButtonList.Add(tempObj);
        }

    }

    // Closes inventory
    public void OnInventoryCloseButtonClick()
    {
        // turns on hud button
        hud_ButtonInventory.SetActive(true);

        // turns off inventory
        inventoryOpen.SetActive(false);

        // destroys all buttons on inventory close
        foreach (GameObject tempObj in itemButtonList)
        {
            Destroy(tempObj);
        }
    }

    public void AddItem(SO_Item _itemToAdd)
    {
        // adds item to inventory list
        itemList.Add(_itemToAdd);
    }

    public void RemoveItem(SO_Item _itemToRemove)
    {
        // searches item list
        for (int i = 0;i < itemList.Count;i++)
        {
            // checks if item is same as iterator
            if(itemList[i] == _itemToRemove)
            {
                //Debug.Log("Item has been found and is being removed");

                // removes item
                itemList.RemoveAt(i);
            }
        }
    }

    // returns the item list
    public List<SO_Item> ReturnItemList()
    {
        return itemList;
    }
}
