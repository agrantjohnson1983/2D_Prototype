using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class cEnergy : MonoBehaviour
{
    public static cEnergy energyGlobal;

    public Image imageEnergyBar;

    public float startingEnergy = 100f;
    float currentEnergy;
    public float energyDrainPerSec = 1f;

    // Toggle this statically to turn on/off stamina drain
    bool canDrain = false;

    bool isRecovering = false;

    public float drainCooldownTime = 5f;

    private void Awake()
    {
        if (energyGlobal == null)
            energyGlobal = this;
        else
            Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentEnergy = startingEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if(canDrain)
        {
            if(!isRecovering)
            {
                currentEnergy -= energyDrainPerSec * Time.deltaTime;

                // If energy is full depleted
                if (currentEnergy <= 0)
                {
                    NoEnergyTrigger();
                }
            }
        }

        else
        {
            if(!isRecovering)
            {
                currentEnergy += energyDrainPerSec * Time.deltaTime;

                if ((currentEnergy >= startingEnergy))
                {
                    currentEnergy = startingEnergy;
                }
            }
        }

        // Set UI

        Debug.Log("Setting eneryg bar to : " + currentEnergy / startingEnergy);

        imageEnergyBar.fillAmount = (currentEnergy / startingEnergy);
    }

    public void ToggleDrain(bool _canDrain)
    {
        Debug.Log("Toggling can drain to: " + _canDrain);
        canDrain = _canDrain;
    }

    void NoEnergyTrigger()
    {
        canDrain = false;

        if(!isRecovering)
            StartCoroutine(StaminaDrainCooldown());
    }

    IEnumerator StaminaDrainCooldown()
    {
        isRecovering = true;

        sPlayer.playerGlobal.ToggleMovement(false);

        sPlayer.playerGlobal.DisplayText("Oh man I gotta chill... I'm out of energy", drainCooldownTime);

        if(sCharacterControllerBASE.isFlying)
        {
            sPlayer.playerGlobal.ToggleFlying(false);
        }

        yield return new WaitForSeconds(drainCooldownTime);

        sPlayer.playerGlobal.SetPosition(sPlayer.playerGlobal.GetActiveMovementObject().transform.position);

        sPlayer.playerGlobal.ToggleMovement(true);

        sPlayer.playerGlobal.DisplayText("Ok I'm good now", drainCooldownTime);

        isRecovering = false;
    }
}
