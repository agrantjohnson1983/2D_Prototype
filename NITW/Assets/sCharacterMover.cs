using UnityEngine;
using System.Collections;

public class sCharacterMover : MonoBehaviour
{
    [System.Serializable]
    public class Movements
    {
        public Vector2 movementOffset;
        public float duration;
    }

    public Movements[] movements;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MovementSequence());
    }

    public IEnumerator MovementSequence()
    {
        float counter = 0f;

        for (int i = 0; i < movements.Length; i++)
        {
            Vector3 offset = new Vector3(movements[i].movementOffset.x, movements[i].movementOffset.y);

            while(counter < movements[i].duration)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + offset, counter / movements[i].duration);
                counter += Time.deltaTime;

                yield return null;
            }

            // resets counter
            counter = 0f;
        }
    }
}
