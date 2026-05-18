using UnityEngine;

public class cSleeping : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnClickYes()
    {
        Debug.Log("Yes to sleeep clicked");
    }

    public void OnClickNo()
    {
        this.gameObject.SetActive(false);
    }
}
