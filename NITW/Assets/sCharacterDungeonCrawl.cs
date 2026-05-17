using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sCharacterDungeonCrawl : sCharacterControllerBASE
{
    //Rigidbody2D rb;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        healthHeartList = new List<GameObject>();

        // inits health
        currentHealth = maxHealth;
        UpdateHealthUI();

        // inits magic
        currentMagic = maxMagic;
        UpdateMagicUI();

        // turns off some canvas stuff for dungeon crawl
        sGameManager.gm.ToggleDungeonCanvas(true);
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
    }

}
