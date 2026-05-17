using UnityEngine;

public class sStreetBoundary : MonoBehaviour
{
    // set this to true if the character should reverse and move to right when hitting collider
    public bool reversesToRight = true;

    public float offsetMovementAmount = 3f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks for character controller on trigger
        if(collision.TryGetComponent<sCharacterControllerSideScroll>(out sCharacterControllerSideScroll _controller))
        {
            //Debug.Log("Triggering street boundary reversal");
            if (!reversesToRight)
            {
                offsetMovementAmount *= -1f;
            }

            // triggers reversal - might need a toggle but should only need a single collision and the reverse pushes it outside collider
            _controller.BoundaryTrigger(offsetMovementAmount);

            // displays text
            sPlayer.playerGlobal.DisplayText("I can't cross this sreet", 3.5f);
        }
    }
}
