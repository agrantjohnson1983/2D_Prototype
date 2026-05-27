using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eDungeon { none, cactusForest, }

public class sCharacterDungeonCrawl : sCharacterControllerBASE
{
    // HEALTH

    public int maxHealth;
    int currentHealth;

    // HEALTH UI

    public GameObject healthHeart;
    public Transform healthHeartPanel;
    List<GameObject> healthHeartList;

    // MAGIC UI

    public float maxMagic;
    float currentMagic;
    public Image magicBar;

    // DAMAGE

    public float damageSequenceTime = 1.5f;

    eDungeon currentDungeon;
    string exitDungeonScene;
    public Vector3 dungeonExitOffset;

    public SO_Level exitLevelData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        // turns off state switch so flying can't be turned on in dungeon
        //canSwitchState = false;

        healthHeartList = new List<GameObject>();

        // inits health
        currentHealth = maxHealth;
        UpdateHealthUI();

        // inits magic
        currentMagic = maxMagic;
        UpdateMagicUI();
    }

    public void SetDungeon(eDungeon _dungeon, string _exitScene)
    {
        currentDungeon = _dungeon;
        exitDungeonScene = _exitScene;
    }

    void UpdateHealthUI()
    {
        foreach(GameObject go in healthHeartList)
        {
            Destroy(go);
        }

        healthHeartList.Clear();

        for (int i = 0; i < currentHealth; i++)
        {
            healthHeartList.Add(Instantiate(healthHeart, healthHeartPanel));
        }
    }

    void UpdateMagicUI()
    {
        magicBar.fillAmount = (currentMagic / maxMagic);
    }

    public void AdjustHealth(int _amount)
    {
        // incrememnts amount to current health
        currentHealth += _amount;

        // if amound is less than zero
        if(_amount < 0)
        {
            TakeDamage();

            sPlayer.playerGlobal.DisplayText("-" + _amount + " damage", 0.5f);
        }

        // if amount is greater than zero
        else if (_amount > 0)
        {
            Heal();

            sPlayer.playerGlobal.DisplayText("+" + _amount + " health", 1f);
        }

        // if amount is zero
        else
        {
            Debug.LogWarning("Health adjustment had 0 value");
        }

        // Updates UI
        UpdateHealthUI();
    }

    public void UseMagic(float _amount)
    {
        currentMagic -= _amount;
        UpdateMagicUI();
    }

    public float ReturnMagicAmount()
    {
        return currentMagic;
    }

    void TakeDamage()
    {
        if (currentHealth <= 0)
        {
            GameOver();
            return;
        }

        StartCoroutine(DamageSeqeunce());
    }

    IEnumerator DamageSeqeunce()
    {
        sPlayer.playerGlobal.ToggleMovement(false);

        rb.AddForce((-this.transform.right + this.transform.up) * 10f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(damageSequenceTime);

        sPlayer.playerGlobal.ToggleMovement(true);
    }

    void Heal()
    {
        Debug.Log("Healing");
    }

    void GameOver()
    {
        Debug.Log("Your crawl has come to and end player");

        // ends dungeon crawl
        if(exitDungeonScene != null)
        {
            sSceneManger.sceneManagerGlobal.LoadScene(dungeonExitOffset, exitLevelData);
        }
    }

}
