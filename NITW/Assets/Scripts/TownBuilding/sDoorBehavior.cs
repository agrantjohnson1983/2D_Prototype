using UnityEngine;
using UnityEngine.SceneManagement;

public class sDoorBehavior : sInteractable
{
    //public GameObject uDoorArrow;

    public GameObject[] turnOffOnEnter, turnOnOnEnter;

    public Transform enterTransform;

    public Vector2 entranceOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        // sets ui to zero
        //uDoorArrow.transform.localScale = Vector3.zero;

        // quick init for buildings - turns on and then scales to zero
        for (int i = 0; i < turnOnOnEnter.Length; i++)
        {
            turnOnOnEnter[i].gameObject.SetActive(true);

            turnOnOnEnter[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    // Overrides the base for the Door's interaction trigger
    public override void TriggerInteraction()
    {
        base.TriggerInteraction();

        // tells player script you are inside
        player.ToggleOutside(false);

        // turns off parallaxing backgrounds
        sParallaxingBackground.canParralax = false;

        // calculates pos as Vector 2
        Vector2 _pos = new Vector2(enterTransform.position.x, enterTransform.position.y);

        // adds offset
        _pos += entranceOffset;

        // sets player pos
        player.SetPosition(_pos);

        // turns on objects on enter
        for (int i = 0; i < turnOnOnEnter.Length; i++)
        {
            turnOnOnEnter[i].SetActive(true);
        }

        // turns off objects on enter
        for (int i = 0; i < turnOffOnEnter.Length; i++)
        {
            turnOffOnEnter[i].SetActive(false);
        }
    }

    
}
