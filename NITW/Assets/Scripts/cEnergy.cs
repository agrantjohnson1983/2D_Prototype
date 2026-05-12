using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class cEnergy : MonoBehaviour
{
    public Image imageEnergyBar;

    public float startingEnergy = 100f;
    float currentEnergy;
    public float energyDrainPerSec = 1f;

    // Toggle this statically to turn on/off stamina drain
    public static bool canDrain = false;

    public float drainCooldownTime = 5f;

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
            currentEnergy -= energyDrainPerSec * Time.deltaTime;

            // If energy is full depleted
            if(currentEnergy <= 0)
            {
                NoEnergyTrigger();
            }
        }

        else
        {
            currentEnergy += energyDrainPerSec * Time.deltaTime;

            if ((currentEnergy >= startingEnergy))
            {
                currentEnergy = startingEnergy;
            }
        }

        // Set UI

        imageEnergyBar.fillAmount = currentEnergy / startingEnergy;
    }

    void NoEnergyTrigger()
    {
        canDrain = false;

        StartCoroutine(StaminaDrainCooldown());
    }

    IEnumerator StaminaDrainCooldown()
    {
        //sCharacterControllerSideScroll.characterControllerSideScrollGlobal.SetCanMove(false);

        //sCharacterControllerFlyingSideToSide.characterControllerFlyingGlobal.SetCanMove(false);

        sPlayer.playerGlobal.ToggleMovement(false);

        uTextCharacter.textCharacterGlobal.SetText("Oh man I gotta chill... I'm out of energy", drainCooldownTime);

        yield return new WaitForSeconds(drainCooldownTime);

        canDrain = true;

        //sCharacterController.characterControllerGlobal.SetCanMove(true);

        //sCharacterControllerFlyingSideToSide.characterControllerFlyingGlobal.SetCanMove(true);

        sPlayer.playerGlobal.ToggleMovement(true);

        uTextCharacter.textCharacterGlobal.SetText("Ok I'm good now", drainCooldownTime);
    }
}
