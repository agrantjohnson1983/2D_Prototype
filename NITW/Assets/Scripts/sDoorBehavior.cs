using UnityEngine;
using UnityEngine.SceneManagement;

public class sDoorBehavior : MonoBehaviour
{
    public GameObject uDoorArrow;

    public GameObject[] turnOffOnEnter, turnOnOnEnter;

    bool isTouchingDoor = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uDoorArrow.transform.localScale = Vector3.zero;

        //insideHouse.SetActive(false);

        // quick init for buildings - turns on and then scales to zero
        for (int i = 0; i < turnOnOnEnter.Length; i++)
        {
            turnOnOnEnter[i].gameObject.SetActive(true);
            //turnOnOnEnter[i].SetActive(true);
            //turnOnOnEnter[i].transform.localScale = Vector3.zero;
            turnOnOnEnter[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(sCharacterController.isOutside && !sCharacterController.isFlying)
        {
            if (isTouchingDoor && Input.GetKey(KeyCode.W))
            {
                sCharacterController.isOutside = false;

                for (int i = 0; i < turnOnOnEnter.Length; i++)
                {
                    turnOnOnEnter[i].SetActive(true);
                    //turnOnOnEnter[i].transform.localScale = Vector3.one;
                }

                for (int i = 0; i < turnOffOnEnter.Length; i++)
                {
                    //turnOffOnEnter[i].transform.localScale = Vector3.zero;
                    turnOffOnEnter[i].SetActive(false);
                }

                //insideHouse.SetActive(true);
                //outsideHouse.SetActive(false);
                //outsideGround.SetActive(false);
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
            uDoorArrow.transform.localScale = Vector3.one;
            //uDoorArrow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingDoor = false;
            uDoorArrow.transform.localScale = Vector3.zero;
            //uDoorArrow.SetActive(false);
        }
    }
}
