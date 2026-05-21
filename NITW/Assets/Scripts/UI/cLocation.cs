using UnityEngine;
using TMPro;

public class cLocation : MonoBehaviour
{
    public static cLocation locationGlobal;

    public TextMeshProUGUI textLocation;

    private void Awake()
    {
        if (locationGlobal == null)
            locationGlobal = this;
        else
            Destroy(locationGlobal);
    }

    public void SetTextLocation(string _text)
    {
        textLocation.text = _text;
    }

}
