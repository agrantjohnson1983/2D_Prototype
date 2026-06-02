using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SO_Item", menuName = "Scriptable Objects/SO_Item")]
public class SO_Item : ScriptableObject
{
    public string itemName;

    public Sprite itemSprite;

    public GameObject itemPrefab;

    public UnityEvent onGrabEvent;

    private void OnEnable()
    {
        if (onGrabEvent == null)
            onGrabEvent = new UnityEvent();
    }

    public void OnGrabTrigger()
    {
        onGrabEvent.Invoke();
    }
}
