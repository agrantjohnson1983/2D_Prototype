using UnityEngine;

public class uStoreButton : MonoBehaviour
{

    cStore store;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        store = GetComponentInParent<cStore>();
    }

    public void OnClick()
    {
        if (store != null)
        {
            store.OnClick();
        }

        else
        {
            Debug.LogError("Store is null for " + this.name);
        }

    }
}
