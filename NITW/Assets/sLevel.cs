using UnityEngine;

public class sLevel : MonoBehaviour
{
    public SO_Level levelData;

    void OnEnable()
    {
        sLevelManager.levelManagerGlobal.AddLevel(levelData, this.gameObject);
    }
}
