using UnityEngine;

public class sDialogueController : MonoBehaviour
{
    public GameObject dialogue1;

    public GameObject dialogue2_1, dialogue2_2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton1()
    {
        dialogue1.SetActive(false);
        dialogue2_1.SetActive(true);
    }

    public void OnClickButton2()
    {
        dialogue1.SetActive(false);
        dialogue2_2.SetActive(true);
    }

    public void ResetDialogue()
    {
        dialogue2_1.SetActive(false);
    
        dialogue2_2.SetActive(false);

        dialogue1.SetActive(true);
    }

}
