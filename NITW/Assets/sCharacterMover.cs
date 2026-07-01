using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class sCharacterMover : MonoBehaviour
{
    [System.Serializable]
    public class Movements
    {
        public Vector2 movementOffset;
        public float duration;
        public bool autoTriggersNextMovement = true;
        public UnityEvent onCompleteEvent = null;
    }

    public Movements[] movements;

    int index = 0;

    public bool moveOnStart = true;

    public bool destroyOnEnd = false;

    public bool isLooping = false;

    public GameObject[] objectsToTurnOnAtEnd, objectsToTurnOffAtEnd;

    public UnityEvent eventOnComplete;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(null);

        if (moveOnStart)
            StartMovement();
    }

    public void StartMovement()
    {
        StartCoroutine(MovementSequence());
    }

    public void NextMovement()
    {
        StartCoroutine(MovementSequence());
    }

    public IEnumerator MovementSequence()
    {
        float counter = 0f;

        Vector3 offset = new Vector3(movements[index].movementOffset.x, movements[index].movementOffset.y);

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + offset;

        // smooth moves character over specified duration
        while (counter < movements[index].duration)
        {
            this.transform.position = Vector3.Lerp(startPos, endPos
                , counter / movements[index].duration);
            counter += Time.deltaTime;

            yield return null;
        }

        if (movements[index].onCompleteEvent != null)
            movements[index].onCompleteEvent.Invoke();

        if(movements[index].autoTriggersNextMovement)
        {
            index++;

            if(index > movements.Length-1)
            {
                if (destroyOnEnd)
                    Destroy(this.gameObject);
            }
                
            else
            {
                NextMovement();
                yield return null;
            }
        }

        // iterates through each movement
        /*for (int i = 0; i < movements.Length; i++)
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
        }*/

       

        // calls the method again if set to loop
        if (isLooping)
        {
            StartCoroutine(MovementSequence());
            yield return null;
        }
            
        // toggles objects on/off at end
        else
        {
            sObjectsToggler.ToggleObjects(objectsToTurnOnAtEnd, objectsToTurnOffAtEnd);
            eventOnComplete.Invoke();
            yield return null;
        }
            

        
    }
}
