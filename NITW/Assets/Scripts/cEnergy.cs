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

    // lazy loading
    sPlayer player;// { get { if (player == null) player = sPlayer.playerGlobal; return player;  } set { player = value; } }

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
        player = sPlayer.playerGlobal;
    }

    // Update is called once per frame
    void Update()
    {
        if(canDrain)
        {
            currentEnergy -= energyDrainPerSec * Time.deltaTime;

            // If energy is full depleted
            if (currentEnergy <= 0)
            {
                NoEnergyTrigger();
            }
            
        }

        else if (isRecovering)
        {
            currentEnergy += energyDrainPerSec * Time.deltaTime;

            if ((currentEnergy >= startingEnergy))
            {
                currentEnergy = startingEnergy;
            }
            
        }

        // Set UI

        //Debug.Log("Setting eneryg bar to : " + currentEnergy / startingEnergy);

        imageEnergyBar.fillAmount = (currentEnergy / startingEnergy);
    }

    public void ToggleDrain(bool _canDrain)
    {
        //Debug.Log("Toggling can drain to: " + _canDrain);
        canDrain = _canDrain;
    }

    void NoEnergyTrigger()
    {
        canDrain = false;

        if (player == null) return;

        if(!isRecovering)
            StartCoroutine(StaminaDrainCooldown());
    }

    IEnumerator StaminaDrainCooldown()
    {
        //isRecovering = true;

        player.ToggleMovement(false);

        player.DisplayText("Oh man I gotta chill for a sec... I'm out of energy", drainCooldownTime);

        if(player.CheckIfFlying())
        {
            player.ToggleFlying(false);
        }

        yield return new WaitForSeconds(drainCooldownTime);

        player.SetPosition(player.GetActiveMovementObject().transform.position);

        player.ToggleMovement(true);

        player.DisplayText("Ok I'm good now but I need to sleep", drainCooldownTime);

        //isRecovering = false;
    }

    public void ToggleRecovery(bool _canRecover)
    {
        isRecovering = _canRecover;
    }

    public bool CheckIfEnergyIsFull()
    {
        if (currentEnergy == startingEnergy) return true;

        else return false;
    }
}
