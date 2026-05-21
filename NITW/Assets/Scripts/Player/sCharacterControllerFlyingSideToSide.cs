using UnityEngine;

public class sCharacterControllerFlyingSideToSide : sCharacterControllerBASE
{
    [Header("Flying Movement")]
    public float broomAccelerationPower = 1f;
    public float maxSpeedMagnitude = 60f;

    [Header("Glide")]
    public float glideGravity = 6f;

    [Header("Loop Settings")]
    public float loopRadius = 2.2f;
    public float loopDegreesPerSecond = 200f;
    public float loopThrottleMultiplier = 1.35f;
    [Tooltip("World-space Y position the character must be at or above before a loop can start.")]
    public float minimumLoopHeight = 5f;

    [Header("Rotation")]
    [Tooltip("Child transform to rotate visually. Leave empty to rotate the root.")]
    public Transform spriteTransform;
    [Tooltip("How fast the sprite rotates to match flight angle during glide.")]
    public float rotationSmoothSpeed = 10f;
    [Tooltip("Offset in degrees if your sprite art is not drawn with the head pointing up.")]
    public float spriteRotationOffset = 0f;

    // Flight angle constants in world space (Unity Y+ = up)
    // 45 degrees  = up-right
    // 135 degrees = up-left
    private const float ANGLE_UP_RIGHT = 45f * Mathf.Deg2Rad;
    private const float ANGLE_UP_LEFT = 135f * Mathf.Deg2Rad;

    private enum FlightState { Glide, Loop, Drop }
    private FlightState flightState = FlightState.Glide;

    private bool isHoldingThrottle = false;
    private bool isFacingRight = true;

    private Vector2 inputVelocity;
    private float flightAngle;      // world-space radians: 0 = right, 90deg = up
    private float currentSpeed;

    // Loop orbit
    private Vector2 loopCenter;
    private float loopStartAngle;
    private float loopProgress;     // radians completed (0 to 2*PI)
    private int loopSpin;           // +1 = CCW, -1 = CW

    sProjectileController projectileController;

    public override void Start()
    {
        base.Start();

        //sPlayer.playerGlobal.isFlying = true;
        flightAngle = ANGLE_UP_RIGHT;
        currentSpeed = 0f;

        projectileController = GetComponent<sProjectileController>();
    }

    public override void Update()
    {
        base.Update();
        ReadInput();
        ThrottleCheck();
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        if (flightState == FlightState.Glide)
            GlidePhysics();
        else
            LoopPhysics();

        UpdateSpriteRotation();
    }

    // -------------------------------------------------------------------------
    // Input
    // -------------------------------------------------------------------------

    void ReadInput()
    {
        inputVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void ThrottleCheck()
    {
        isHoldingThrottle = Input.GetKey(KeyCode.Space);
        rb.gravityScale = (flightState == FlightState.Glide && !isHoldingThrottle) ? glideGravity : 0f;
        cEnergy.energyGlobal.ToggleDrain(isHoldingThrottle);
    }

    // -------------------------------------------------------------------------
    // Glide
    // -------------------------------------------------------------------------

    void GlidePhysics()
    {
        // Facing-relative loop keys
        bool loopKey = isFacingRight ? inputVelocity.x < 0f : inputVelocity.x > 0f;
        bool dropKey = isFacingRight ? inputVelocity.x > 0f : inputVelocity.x < 0f;

        // Throttle: accelerate along the current flight angle
        if (isHoldingThrottle)
        {
            currentSpeed += broomAccelerationPower * Time.fixedDeltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeedMagnitude);
        }
        else
        {
            currentSpeed *= 0.97f;
        }

        // Steer angle back toward the default 45-degree climb when no direction held.
        // flightAngle is pure world-space so no facingDir multiplier needed.
        if (!loopKey && !dropKey)
        {
            float targetAngle = isFacingRight ? ANGLE_UP_RIGHT : ANGLE_UP_LEFT;
            float delta = Mathf.DeltaAngle(
                flightAngle * Mathf.Rad2Deg,
                targetAngle * Mathf.Rad2Deg
            );
            flightAngle += delta * Mathf.Deg2Rad * 0.06f;
        }

        // Apply velocity directly from world-space angle.
        // The angle already encodes left vs right (45 deg vs 135 deg)
        // so no facingDir multiplier is needed here.
        rb.linearVelocity = new Vector2(
            Mathf.Cos(flightAngle) * currentSpeed,
            Mathf.Sin(flightAngle) * currentSpeed
        );

        // Transition into loop or drop — requires throttle, direction key, minimum speed, and height.
        if (flightState == FlightState.Glide && isHoldingThrottle && currentSpeed > 0.5f
            && transform.position.y >= minimumLoopHeight)
        {
            if (loopKey)
                BeginLoop(FlightState.Loop);
            else if (dropKey)
                BeginLoop(FlightState.Drop);
        }
    }

    // -------------------------------------------------------------------------
    // Loop entry
    // -------------------------------------------------------------------------

    void BeginLoop(FlightState newState)
    {
        flightState = newState;
        loopProgress = 0f;

        if (newState == FlightState.Loop)
        {
            // Pull-back loop: orbit center directly above, character starts at 6 o'clock.
            // Facing right travels CCW (loopSpin = -1), facing left travels CW (loopSpin = 1).
            loopCenter = (Vector2)transform.position + Vector2.up * loopRadius;
            loopStartAngle = -Mathf.PI / 2f;
            loopSpin = isFacingRight ? -1 : 1;
        }
        else
        {
            // Nose-dive drop: orbit center directly below, character starts at 12 o'clock.
            // Facing right travels CW (loopSpin = 1), facing left travels CCW (loopSpin = -1).
            loopCenter = (Vector2)transform.position + Vector2.down * loopRadius;
            loopStartAngle = Mathf.PI / 2f;
            loopSpin = isFacingRight ? 1 : -1;
        }

        rb.gravityScale = 0f;
    }

    // -------------------------------------------------------------------------
    // Loop physics
    // -------------------------------------------------------------------------

    void LoopPhysics()
    {
        bool triggerKey = flightState == FlightState.Loop
            ? (isFacingRight ? inputVelocity.x < 0f : inputVelocity.x > 0f)
            : (isFacingRight ? inputVelocity.x > 0f : inputVelocity.x < 0f);

        // Exit immediately if the trigger key or throttle is released mid-loop.
        // Only flip facing when released within the turnaround window:
        // top loop: 10-2 o'clock, bottom loop: 8-4 o'clock (both = 2PI/3 to 4PI/3).
        if (!triggerKey || !isHoldingThrottle)
        {
            bool inFlipWindow = loopProgress >= Mathf.PI * 2f / 3f
                             && loopProgress <= Mathf.PI * 4f / 3f;
            ExitLoop(!triggerKey && inFlipWindow);
            return;
        }

        // Advance around the circle.
        loopProgress += loopDegreesPerSecond * Mathf.Deg2Rad * Time.fixedDeltaTime * loopThrottleMultiplier;

        // Target position on the orbit circle.
        float curAngle = loopStartAngle + loopProgress * -loopSpin;
        Vector2 targetPos = loopCenter + new Vector2(
            Mathf.Cos(curAngle) * loopRadius,
            Mathf.Sin(curAngle) * loopRadius
        );

        // Drive position via velocity.
        rb.linearVelocity = (targetPos - (Vector2)transform.position) / Time.fixedDeltaTime;

        // Store tangent angle so glide exit feels continuous.
        flightAngle = curAngle + (Mathf.PI / 2f) * -loopSpin;

        // Full revolution completed.
        if (loopProgress >= Mathf.PI * 2f)
        {
            if (triggerKey)
            {
                // Key still held: restart from current position for another revolution.
                loopProgress = 0f;
                loopStartAngle = Mathf.Atan2(
                    transform.position.y - loopCenter.y,
                    transform.position.x - loopCenter.x
                );
                return;
            }

            ExitLoop();
        }
    }

    void ExitLoop(bool flipFacing = false)
    {
        if (flipFacing) isFacingRight = !isFacingRight;
        flightState = FlightState.Glide;
        loopProgress = 0f;
        currentSpeed = maxSpeedMagnitude * 0.75f;
        rb.linearVelocity = new Vector2(
            Mathf.Cos(flightAngle) * currentSpeed,
            Mathf.Sin(flightAngle) * currentSpeed
        );
    }

    // -------------------------------------------------------------------------
    // Sprite rotation
    // -------------------------------------------------------------------------

    void UpdateSpriteRotation()
    {
        spriteRenderer.flipX = isFacingRight;

        Transform t = spriteTransform != null ? spriteTransform : transform;
        float targetZDeg;

        if (flightState == FlightState.Glide)
        {
            // Return to default resting rotation (zero + any art offset) while gliding.
            Quaternion restRotation = Quaternion.Euler(0f, 0f, spriteRotationOffset);
            t.rotation = Quaternion.Slerp(t.rotation, restRotation, rotationSmoothSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // During a loop the head always points toward the orbit center (pull-back loop)
            // or away from it (drop), chosen so both start at 0-deg matching glide rest pose
            // and the Slerp has no ambiguous 180-deg crossings.
            Vector2 toCenter = loopCenter - (Vector2)transform.position;
            float angleOffset = flightState == FlightState.Loop ? -90f : 90f;
            targetZDeg = Mathf.Atan2(toCenter.y, toCenter.x) * Mathf.Rad2Deg + angleOffset + spriteRotationOffset;
            Quaternion loopTarget = Quaternion.Euler(0f, 0f, targetZDeg);
            t.rotation = Quaternion.Slerp(t.rotation, loopTarget, rotationSmoothSpeed * 2f * Time.fixedDeltaTime);
        }
    }

    // -------------------------------------------------------------------------
    // Scene view gizmos
    // -------------------------------------------------------------------------

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        if (flightState != FlightState.Glide)
        {
            UnityEditor.Handles.color = flightState == FlightState.Loop
                ? new Color(0.4f, 0.8f, 1f, 0.4f)
                : new Color(1f, 0.4f, 0.3f, 0.4f);
            UnityEditor.Handles.DrawWireDisc(loopCenter, Vector3.forward, loopRadius);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position,
            new Vector3(Mathf.Cos(flightAngle), Mathf.Sin(flightAngle)) * 1.5f);
    }
#endif
}