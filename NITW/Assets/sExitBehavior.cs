using UnityEngine;

public class sExitBehavior : MonoBehaviour
{
    public GameObject outside;
    public GameObject inside;
    public GameObject outsideGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        outsideGround.SetActive(true);
        outside.SetActive(true);
        inside.SetActive(false);
    }
}
