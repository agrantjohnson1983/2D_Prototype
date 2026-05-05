using UnityEngine;

public class sPongGoal : MonoBehaviour
{
    [Tooltip("Is this the goal behind the PLAYER paddle? (left side)")]
    public bool isPlayerGoal = false; // true = AI scores when ball enters; false = Player scores

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Projectile")) return;

        if (isPlayerGoal)
            sPongManager.pongManagerGlobal.AIScores();
        else
            sPongManager.pongManagerGlobal.PlayerScores();
    }
}
