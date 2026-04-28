using UnityEngine;
using UnityEngine.SceneManagement;

public class sCorner : MonoBehaviour
{
    public GameObject ui_Display;

    bool isTouchingDoor = false;

    public string sceneToTransitionTo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ui_Display.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (sCharacterController.isOutside && !sCharacterController.isFlying)
        {
            if (isTouchingDoor && Input.GetKey(KeyCode.W))
            {

                SceneManager.LoadScene(sceneToTransitionTo);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && sCharacterController.isOutside == true && !sCharacterController.isFlying)
        {
            isTouchingDoor = true;
            ui_Display.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingDoor = false;
            ui_Display.SetActive(false);
        }
    }
}
