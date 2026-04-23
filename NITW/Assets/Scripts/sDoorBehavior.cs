using UnityEngine;

public class sDoorBehavior : MonoBehaviour
{
    public GameObject outsideGround;

    public GameObject uDoorArrow;

    public GameObject insideHouse, outsideHouse;

    bool isTouchingDoor = false;

    //bool isInside = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uDoorArrow.SetActive(false);

        insideHouse.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(sCharacterController.isOutside && !sCharacterController.isFlying)
        {
            if (isTouchingDoor && Input.GetKey(KeyCode.W))
            {
                sCharacterController.isOutside = false;
                insideHouse.SetActive(true);
                outsideHouse.SetActive(false);
                outsideGround.SetActive(false);
            }
        }

        // when inside
        else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && sCharacterController.isOutside == true && !sCharacterController.isFlying)
        {
            isTouchingDoor=true;
            uDoorArrow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingDoor = false;
            uDoorArrow.SetActive(false);
        }
    }
}
