using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class sCharacterControllerBASE : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    protected Rigidbody2D rb;

    public static bool canMove = true;

    //public static bool isOutside = true;

    //public static bool isFlying = false;

    //public float stateSwitchCooldownTime = 2.5f;
    //public static bool canSwitchState = false;

    public float reverseSequenceTime = 2.5f;

    sPlayer player;

    // AUDIO

    public SO_AudioData audioData;
    public AudioSource audioSource;

    protected bool _playerInside = false;
    protected bool _holdCompleted = false;   // prevent re-firing while still inside
    protected float _holdTimer = 0f;
    public float holdDuration = 2f;

    private void OnEnable()
    {
        // gets player reference
        player = GetComponentInParent<sPlayer>();

        // sets active movement object
        player.SetActiveMovementObject(this.gameObject);

        // Sets active movmenet object when enabled
        if (player != null)
            player.SetActiveMovementObject(this.gameObject);
        else
            Debug.LogWarning("Player is null for " + this.gameObject + " enable");

        //StartCoroutine(StateSwitchCooldown(stateSwitchCooldownTime));
    }

    // Use this for state checking
    public virtual void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.F) && canSwitchState)
        {
            Debug.Log("Input detected and can switch state");

            isFlying = !isFlying;

            // stops from spamming - needs cooldown
            canSwitchState = false;

            // Switches state with player - this should turn off this gameObject
            sPlayer.playerGlobal.ToggleFlying(isFlying);
        }*/
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        sPlayer.playerGlobal.SetActiveMovementObject(this.gameObject);
    }

    public virtual void SetLocation(Vector3 _pos)
    {
        this.transform.position = _pos;
    }


    public virtual void SetCanMove(bool _canMove)
    {
        canMove = _canMove;

        // stops velocity if can't move
        if (!canMove)
            rb.linearVelocity = Vector2.zero;
    }


    public void BoundaryTrigger(float _offsetAmount)
    {
        // stops coroutines
        StopAllCoroutines();

        // starts the corotuine
        StartCoroutine(BoundaryReverseSequence(_offsetAmount));
    }

    IEnumerator BoundaryReverseSequence(float _offsetAmount)
    {
        // turns off movment
        SetCanMove(false);

        // flips x offset to negative the reverse should go to left

        // checks if offset is greater than 0
        if (_offsetAmount > 0)
        {
            // sets sprite flip
            spriteRenderer.flipX = true;
        }

        else
        {
            spriteRenderer.flipX = false;
        }

        float counter = 0f;

        // checks if counter is less than reverse time
        while (counter < reverseSequenceTime)
        {
            // lerps postion to x offset
            this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + new Vector3(_offsetAmount, 0, 0), counter / reverseSequenceTime);

            // increments counter by time amount
            counter += Time.deltaTime;

            yield return null;
        }

        // toggles movement back on
        SetCanMove(true);
    }
}
