using UnityEngine;

[CreateAssetMenu(fileName = "SO_Item", menuName = "Scriptable Objects/SO_Item")]
public class SO_Item : ScriptableObject
{
    public string itemName;

    public Sprite itemSprite;

    public GameObject itemPrefab;
}
