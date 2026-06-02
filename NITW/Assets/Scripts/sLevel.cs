using UnityEngine;

public class sLevel : MonoBehaviour
{
    public SO_Level levelData;

    public sParallaxingBackground[] backgrounds;

    void OnEnable()
    {
        sLevelManager.levelManagerGlobal.AddLevel(levelData, this.gameObject);

        foreach(sParallaxingBackground bg in backgrounds)
        {
            //bg.ResetX();
        }
    }
}
