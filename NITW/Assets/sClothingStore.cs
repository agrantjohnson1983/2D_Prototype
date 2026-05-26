using UnityEngine;

public class sClothingStore : sInteractable
{
    public GameObject clothingCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        
    }

    public override void TriggerInteraction()
    {
        base.TriggerInteraction();

        clothingCanvas.SetActive(true);
    }
}
