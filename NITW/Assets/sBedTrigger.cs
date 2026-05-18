using UnityEngine;

public class sBedTrigger : MonoBehaviour
{
    public GameObject bedCanvas;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            bedCanvas.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bedCanvas.SetActive(false);
        }
    }
}
