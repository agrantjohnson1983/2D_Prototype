using System.Collections;
using UnityEngine;

public enum eDirection { north, south, west, east }

public class cCompass : MonoBehaviour
{
    public static cCompass compassGlobal;

    public Transform 
        transformNorth,
        transformSouth,
        transformWest,
        transformEast;

    public GameObject textN;

    public GameObject arrow;

    public eDirection startingDirection;

    private void Awake()
    {
        if (compassGlobal == null)
            compassGlobal = this;
        else
            Destroy(compassGlobal);
    }

    private void Start()
    {
        SetDirection(startingDirection);
    }

    public void SetDirection(eDirection _directionToSetTo)
    {
        switch(_directionToSetTo)
        {
            case eDirection.north:

                textN.gameObject.transform.parent = transformNorth;

                textN.gameObject.transform.localPosition = Vector3.zero;

                arrow.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);

                break;

            case eDirection.south:

                textN.gameObject.transform.parent = transformSouth;

                textN.gameObject.transform.localPosition = Vector3.zero;

                arrow.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0, 180));

                break;

            case eDirection.west:

                textN.gameObject.transform.parent = transformWest;

                textN.gameObject.transform.localPosition = Vector3.zero;

                arrow.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));

                break;

            case eDirection.east:

                textN.gameObject.transform.parent = transformEast;

                textN.gameObject.transform.localPosition = Vector3.zero;

                arrow.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

                break;
        }
    }
}
