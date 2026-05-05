using UnityEngine;
using UnityEngine.UI;

public class uButtonVideoGame : MonoBehaviour
{
    sVideoGameConsoleTrigger consoleTrigger;

    public eVideoGames thisGame;

    private void Start()
    {
        consoleTrigger = GetComponentInParent<sVideoGameConsoleTrigger>();
    }

    public void OnButtonClick()
    {
        consoleTrigger.StartGame(thisGame);
    }

    public void SetButton(Sprite _sprite, string _text)
    {

    }

}
