using UnityEngine;

public class sOverworldTrigger : MonoBehaviour
{
    public string sceneToChange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            sSceneManger.sceneManagerGlobal.LoadScene(sceneToChange, eDirection.north);
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
        
    //}
}
