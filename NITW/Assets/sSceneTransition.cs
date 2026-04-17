using UnityEngine;
using UnityEngine.SceneManagement;

public class sSceneTransition : MonoBehaviour
{
    public string sceneToTransitionTo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This should handle the top down
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Scene change triggered - now going to " + sceneToTransitionTo);

            SceneManager.LoadScene(sceneToTransitionTo);
        }
    }

    // this is for the 2d side scroll
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Scene change triggered - now going to " + sceneToTransitionTo);

            SceneManager.LoadScene(sceneToTransitionTo);
        }
    }
}
