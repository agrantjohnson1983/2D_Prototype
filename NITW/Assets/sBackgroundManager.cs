using UnityEngine;

public class sBackgroundManager : MonoBehaviour
{
    public static sBackgroundManager backgroundManagerGlobal;

    public sParallaxingBackground[] backgrounds; 

    private void Awake()
    {
        if (backgroundManagerGlobal == null)
            backgroundManagerGlobal = this;
        else
            Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetBackgrounds()
    {
        for(int i = 0; i < backgrounds.Length; i++)
        {
            //backgrounds[i].ResetBackground();
        }
    }
}
