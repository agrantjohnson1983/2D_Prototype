using UnityEngine;

public class cInventory : MonoBehaviour
{
    public GameObject inventoryOpen;
    public GameObject buttonInventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryOpen.SetActive(false);
    }

    public void OnInventoryButtonClick()
    {
        buttonInventory.SetActive(false);
        inventoryOpen.SetActive(true);
    }

    public void OnInventoryButtonClose()
    {
        buttonInventory.SetActive(true);
        inventoryOpen.SetActive(false);
    }
}
