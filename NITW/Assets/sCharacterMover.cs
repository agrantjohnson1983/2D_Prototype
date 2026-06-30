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

    public bool destroyOnEnd = false;

    public bool isLooping = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(null);

        StartCoroutine(MovementSequence());
    }

    public IEnumerator MovementSequence()
    {
        float counter = 0f;

        // iterates through each movement
        for (int i = 0; i < movements.Length; i++)
        {
            Vector3 offset = new Vector3(movements[i].movementOffset.x, movements[i].movementOffset.y);

            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + offset;

            // smooth moves character over specified duration
            while(counter < movements[i].duration)
            {
                this.transform.position = Vector3.Lerp(startPos, endPos
                    , counter / movements[i].duration);
                counter += Time.deltaTime;

                yield return null;
            }

            // resets counter before next iteration
            counter = 0f;
        }

        // calls the method again if set to loop
        if(isLooping)
            StartCoroutine(MovementSequence());

        if (destroyOnEnd)
            Destroy(this.gameObject);
    }
}
