using UnityEngine;

public class sObjectsToggler : MonoBehaviour
{
    public static void ToggleObjects(GameObject[] objectsToTurnOn, GameObject [] objectsToTurnOff)
    {
        if(objectsToTurnOn != null)
            for (int i = 0; i < objectsToTurnOn.Length; i++)
            {
                objectsToTurnOn[i].SetActive(true);
            }

        if(objectsToTurnOff != null)
            for (int i = 0; i < objectsToTurnOff.Length; i++)
            {
                objectsToTurnOff[i].SetActive(false);
            }
    }
}
