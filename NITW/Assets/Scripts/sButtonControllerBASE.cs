using UnityEngine;

public class sButtonControllerBASE : MonoBehaviour
{
    public GameObject startingSelectedButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        if (startingSelectedButton)
            sGameManager.gm.SetEventSystem(startingSelectedButton);
    }
}
