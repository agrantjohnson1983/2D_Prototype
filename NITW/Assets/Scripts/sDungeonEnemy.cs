using UnityEngine;

public class sDungeonEnemy : MonoBehaviour
{
    sCharacterDungeonCrawl dungeonCralwer;

    public GameObject spriteObject;

    public int maxHealth;
    int currentHealth;

    public int damageAmount;

    public string audioTakeDamageCue;

    public string audioDoDamageCue;

    public string audioDeath;

    uTextCharacter textPopup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        textPopup = GetComponentInChildren<uTextCharacter>();
    }

    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;

        Debug.Log(this.gameObject + " has take damage of " + _amount + " and now has health of " + currentHealth);

        if (textPopup != null)
            textPopup.SetText("-" + _amount, 0.5f);

        sAudioPlayer.audioPlayerGlobal.TriggerSFX(audioTakeDamageCue, eSFXTriggerType.eSFXtriggerBasic, eAudioMixerType.sfx);

        if(currentHealth <= 0)
        {
            spriteObject.SetActive(false);

            if (textPopup != null)
                textPopup.SetText("Slayed " + this.gameObject.name, 2f);

            Invoke("KillEnemy", 4f);

            if(TryGetComponent<Collider2D>(out Collider2D _collider))
                _collider.enabled = false;
        }
    }

    void KillEnemy()
    {
        //Debug.Log(this.gameObject + " was killed");

        Destroy(this.gameObject, 2.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<sCharacterDungeonCrawl>(out dungeonCralwer))
        {
            //Debug.Log(this.gameObject + " has collider with dungeon crawler");

            // Takes away 10 health to do damage
            dungeonCralwer.AdjustHealth(-damageAmount);

            if (textPopup != null)
                textPopup.SetText("Bite!", 0.75f);

            sAudioPlayer.audioPlayerGlobal.TriggerSFX(audioDoDamageCue, eSFXTriggerType.eSFXtriggerBasic, eAudioMixerType.sfx);
        }
    }

}
