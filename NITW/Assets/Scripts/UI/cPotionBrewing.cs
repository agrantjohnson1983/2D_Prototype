using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class cPotionBrewing : MonoBehaviour
{
    //public GameObject inventoryOpen;

    cInventory inventory;

    public Transform inventoryItemTransform;

    List<GameObject> itemButtonList;

    public GameObject potionInventoryButtonPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = cInventory.inventoryGlobal;

        //itemList = new List<SO_Item>();

        itemButtonList = new List<GameObject>();

        //inventoryOpen.SetActive(false);

        // opens inventory
        PopulateInventory();
    }

    public void OnItemClickToAddToBrew(SO_Item _itemToAdd)
    {
        //Debug.Log("Adding in " + _itemToAdd + " to the brew!");

        // removes item from inventory
        inventory.RemoveItem(_itemToAdd);

        // text display
        sPlayer.playerGlobal.DisplayText("Adding in a " + _itemToAdd.itemName, 5f);
    }

    public void PopulateInventory()
    {
        //Debug.Log("Populating Inventory");

        // new inventorylist
        List<SO_Item> _inventoryItemList = new List<SO_Item>();

        // sets item list
        _inventoryItemList = inventory.ReturnItemList();

        // iterates through item count
        for (int i = 0; i < _inventoryItemList.Count; i++)
        {
            GameObject tempObj;

            // spawns inventory button
            tempObj = Instantiate(potionInventoryButtonPrefab, inventoryItemTransform);

            // sets button from item list
            tempObj.GetComponent<uPotionButton>().SetButton(_inventoryItemList[i]);

            // Add to List
            itemButtonList.Add(tempObj);
        }
    }

    public void OnBrewThatShitClick()
    {
        Debug.Log("On Brew That Shit Clicked");
    }

    public void StopBrewing()
    {
        // resets trigger - there's only one so that's why this is static
        sPotionBrewTrigger.isBrewing = false;

        // sets player back to side scroll
        sPlayer.playerGlobal.StopPotionBrew();
    }
}
