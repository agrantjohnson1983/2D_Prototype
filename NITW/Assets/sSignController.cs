using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum eSignState { up, down, left, right, none }

public class sSignController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject signCanvas;
    public GameObject signMoveToBustArrowPrefab;
    public Transform signMoveToBustTransform;
    public Image timerBarImage;
    public Sprite[] arrowSprites; // Order: 0=up, 1=down, 2=left, 3=right

    [Header("Sign Movement")]
    public Transform signTransform;           // The actual sign sprite's transform
    public Vector3 homeOffset;
    public Transform signHomeTransform;       // Where the sign sits at rest
    public Transform signUpTransform;         // Target position for UP input
    public Transform signDownTransform;       // Target position for DOWN input
    public Transform signLeftTransform;       // Target position for LEFT input
    public Transform signRightTransform;      // Target position for RIGHT input
    public float signMoveDuration = 0.15f;    // How fast it snaps to the target position
    public float signReturnDuration = 0.1f;   // How fast it snaps back to home
    public float signSpinDuration = 0.35f;    // How long the 360 success spin takes
    public float signFailFlightDuration = 0.5f;   // How long the fail fly-off takes
    public float signFailReturnDuration = 0.3f;   // How long the return-to-home takes after fail
    public float signFailFlightDistance = 10f;    // How far off screen it flies

    [Header("Feedback Text")]
    public TMP_Text feedbackText;
    public RectTransform[] feedbackSpawnPoints;

    [Header("Correct Feedback Strings")]
    public string[] correctFeedbackStrings = {
        "Cool!", "Nice!", "Sick!", "Let's go!", "Nailed it!",
        "Smooth!", "Fire!", "Clean!", "Easy!", "Vibes!"
    };

    [Header("Fail Feedback Strings")]
    public string[] failFeedbackStrings = {
        "Shit!", "Nope!", "Miss!", "Oof!", "Wrong!",
        "Yikes!", "Trash!", "No chance!", "Really?!", "L."
    };

    [Header("Timing")]
    public float bufferTimeBeforeArrowDisplayAtStart = 5f;
    public float timeBetweenArrowDisplays = 0.5f;
    public float timeToInputPerSign = 2f;
    public float feedbackDisplayDuration = 0.8f;
    public float feedbackFadeTime = 0.3f;

    [Header("Game Settings")]
    public int numberOfRoundsTillExtraMoveAdded = 3;
    public int numberOfFailsAllowed = 3;

    // Private state
    List<int> currentIndexComboList = new List<int>();
    List<GameObject> spawnedArrows = new List<GameObject>();

    bool canControl = false;

    int round = 0;
    int roundSwitcher = 0;
    int currentNumberOfMoves = 1;
    int currentSignIndex = 0;
    int numberOfCurrentFails = 0;

    eSignState currentCorrectState;

    Coroutine timerCoroutine;
    Coroutine feedbackCoroutine;
    Coroutine signCoroutine;

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    void Start()
    {
        if (timerBarImage != null)
            timerBarImage.fillAmount = 0f;

        if (feedbackText != null)
        {
            Color c = feedbackText.color;
            c.a = 0f;
            feedbackText.color = c;
        }

        StartCoroutine(SignDisplayRoutine());
    }

    void Update()
    {
        if (canControl)
            SignInputs();
    }

    // -------------------------------------------------------------------------
    // Input
    // -------------------------------------------------------------------------

    void SignInputs()
    {
        Vector2 _inputs = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) _inputs = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) _inputs = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) _inputs = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) _inputs = Vector2.right;

        if (_inputs.sqrMagnitude <= 0)
            return;

        canControl = false;
        StopInputTimer();

        currentCorrectState = IndexToSignState(currentIndexComboList[currentSignIndex]);

        eSignState inputState = eSignState.none;

        if (_inputs.x > 0) inputState = eSignState.right;
        else if (_inputs.x < 0) inputState = eSignState.left;
        else if (_inputs.y > 0) inputState = eSignState.up;
        else if (_inputs.y < 0) inputState = eSignState.down;

        if (inputState == currentCorrectState)
            Correct(inputState);
        else
            StartCoroutine(FailWithRedFlash(_inputs));
    }

    // -------------------------------------------------------------------------
    // Sign Movement
    // -------------------------------------------------------------------------

    // Stops any running sign coroutine before starting a new one
    void RunSignCoroutine(IEnumerator routine)
    {
        if (signCoroutine != null)
            StopCoroutine(signCoroutine);

        signCoroutine = StartCoroutine(routine);
    }

    // Smoothly moves sign to a target position over a duration using lerp
    IEnumerator MoveSignTo(Vector3 targetPos, float duration)
    {
        Vector3 startPos = signTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            signTransform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        signTransform.position = targetPos;
    }

    // Snaps sign to the direction transform, spins 360, then returns home
    IEnumerator CorrectSignRoutine(eSignState direction)
    {
        // Snap to direction position
        Vector3 targetPos = SignStateToTransform(direction).position;
        yield return StartCoroutine(MoveSignTo(targetPos, signMoveDuration));

        // Full 360 spin
        float elapsed = 0f;
        float startAngle = signTransform.eulerAngles.z;

        while (elapsed < signSpinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / signSpinDuration;
            // Ease in-out so the spin feels punchy not robotic
            float easedT = t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
            signTransform.eulerAngles = new Vector3(0f, 0f, startAngle + 720f * easedT);
            yield return null;
        }

        // Snap rotation clean
        signTransform.eulerAngles = new Vector3(0f, 0f, startAngle);

        // Return home
        yield return StartCoroutine(MoveSignTo((signHomeTransform.position + homeOffset), signReturnDuration));

        signCoroutine = null;
    }

    // Flies the sign off screen wildly, then snaps it back to home
    IEnumerator FailSignRoutine(Vector2 inputDirection)
    {
        Vector3 startPos = signTransform.position + homeOffset;
        Vector3 startScale = signTransform.localScale;

        // Pick a random direction to fly off if input was zero (timer expiry)
        Vector2 flyDir = inputDirection.sqrMagnitude > 0
            ? inputDirection
            : new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        Vector3 flyTarget = startPos + new Vector3(flyDir.x, flyDir.y, 0f) * signFailFlightDistance;

        float elapsed = 0f;

        while (elapsed < signFailFlightDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / signFailFlightDuration;

            // Accelerate off screen (ease in)
            float easedT = t * t;

            signTransform.position = Vector3.Lerp(startPos, flyTarget, easedT);

            // Spin wildly — multiple rotations during flight
            signTransform.eulerAngles = new Vector3(0f, 0f, easedT * 720f);

            // Shrink to nothing as it flies
            signTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, easedT);

            yield return null;
        }

        // Snap back to home instantly while off screen / invisible
        signTransform.position = signHomeTransform.position;
        signTransform.eulerAngles = Vector3.zero;
        signTransform.localScale = startScale;

        // Brief pause before reappearing so the reset isn't jarring
        yield return new WaitForSeconds(0.1f);

        signCoroutine = null;
    }

    // Converts a sign state to its corresponding world-space transform
    Transform SignStateToTransform(eSignState state)
    {
        switch (state)
        {
            case eSignState.up: return signUpTransform;
            case eSignState.down: return signDownTransform;
            case eSignState.left: return signLeftTransform;
            case eSignState.right: return signRightTransform;
            default: return signHomeTransform;
        }
    }

    // -------------------------------------------------------------------------
    // Feedback Text
    // -------------------------------------------------------------------------

    void ShowFeedback(string[] strings, Color color)
    {
        if (feedbackText == null || feedbackSpawnPoints == null || feedbackSpawnPoints.Length == 0)
            return;

        if (feedbackCoroutine != null)
            StopCoroutine(feedbackCoroutine);

        List<RectTransform> validPoints = new List<RectTransform>();
        foreach (RectTransform rt in feedbackSpawnPoints)
            if (rt != null) validPoints.Add(rt);

        if (validPoints.Count == 0)
        {
            Debug.LogWarning("No valid feedback spawn points assigned!");
            return;
        }

        string randomString = strings[Random.Range(0, strings.Length)];
        RectTransform randomPoint = validPoints[Random.Range(0, validPoints.Count)];

        feedbackText.text = randomString;
        feedbackText.rectTransform.position = randomPoint.position;

        feedbackCoroutine = StartCoroutine(FeedbackRoutine(color));
    }

    IEnumerator FeedbackRoutine(Color baseColor)
    {
        baseColor.a = 1f;
        feedbackText.color = baseColor;

        yield return new WaitForSeconds(feedbackDisplayDuration);

        float elapsed = 0f;
        while (elapsed < feedbackFadeTime)
        {
            elapsed += Time.deltaTime;
            baseColor.a = Mathf.Lerp(1f, 0f, elapsed / feedbackFadeTime);
            feedbackText.color = baseColor;
            yield return null;
        }

        baseColor.a = 0f;
        feedbackText.color = baseColor;
        feedbackCoroutine = null;
    }

    // -------------------------------------------------------------------------
    // Timer
    // -------------------------------------------------------------------------

    void StartInputTimer()
    {
        StopInputTimer();
        timerCoroutine = StartCoroutine(InputTimerRoutine());
    }

    void StopInputTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        if (timerBarImage != null)
            timerBarImage.fillAmount = 0f;
    }

    IEnumerator InputTimerRoutine()
    {
        float elapsed = 0f;

        while (elapsed < timeToInputPerSign)
        {
            elapsed += Time.deltaTime;

            if (timerBarImage != null)
                timerBarImage.fillAmount = 1f - (elapsed / timeToInputPerSign);

            yield return null;
        }

        Debug.Log("Input timer expired!");
        canControl = false;
        StartCoroutine(FailWithRedFlash(Vector2.zero));
    }

    // -------------------------------------------------------------------------
    // Result Handlers
    // -------------------------------------------------------------------------

    void Correct(eSignState _signState)
    {
        Debug.Log($"Correct! ({_signState})");

        if (currentSignIndex < spawnedArrows.Count)
            spawnedArrows[currentSignIndex].GetComponent<Image>().color = new Color(0.4f, 1f, 0.4f, 0.5f);

        ShowFeedback(correctFeedbackStrings, new Color(0.4f, 1f, 0.4f));

        // Trigger the sign move and spin — don't await it, let it play alongside
        RunSignCoroutine(CorrectSignRoutine(_signState));

        currentSignIndex++;

        if (currentSignIndex >= currentIndexComboList.Count)
        {
            Debug.Log("Combo complete! Starting next round.");
            StartCoroutine(SignDisplayRoutine());
        }
        else
        {
            StartInputTimer();
            canControl = true;
        }
    }

    IEnumerator FailWithRedFlash(Vector2 _inputs)
    {
        Debug.Log($"Failed with input {_inputs}.");

        ShowFeedback(failFeedbackStrings, new Color(1f, 0.2f, 0.2f));

        // Trigger fail animation — runs alongside the red flash wait
        RunSignCoroutine(FailSignRoutine(_inputs));

        if (currentSignIndex < spawnedArrows.Count && spawnedArrows[currentSignIndex] != null)
        {
            Image arrowImage = spawnedArrows[currentSignIndex].GetComponent<Image>();
            arrowImage.color = new Color(1f, 0.2f, 0.2f, 1f);
            yield return new WaitForSeconds(0.5f);
        }

        numberOfCurrentFails++;

        if (numberOfCurrentFails >= numberOfFailsAllowed)
            GameOver();
        else
            StartCoroutine(SignDisplayRoutine());
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        canControl = false;
        StopInputTimer();
        ClearSpawnedArrows();

        ShowFeedback(new string[] { "Game Over!", "You're done.", "That's all!", "No more!" },
                     new Color(1f, 0.2f, 0.2f));

        // TODO: Show game-over screen, fire event, load scene, etc.
    }

    // -------------------------------------------------------------------------
    // Display Coroutine
    // -------------------------------------------------------------------------

    IEnumerator SignDisplayRoutine()
    {
        canControl = false;
        StopInputTimer();

        round++;
        roundSwitcher++;

        if (roundSwitcher >= numberOfRoundsTillExtraMoveAdded)
        {
            roundSwitcher = 0;
            currentNumberOfMoves++;
        }

        currentIndexComboList.Clear();
        currentSignIndex = 0;
        ClearSpawnedArrows();

        yield return new WaitForSeconds(bufferTimeBeforeArrowDisplayAtStart);

        for (int i = 0; i < currentNumberOfMoves; i++)
        {
            int randomIndex = Random.Range(0, arrowSprites.Length);
            currentIndexComboList.Add(randomIndex);

            GameObject arrow = Instantiate(signMoveToBustArrowPrefab, signMoveToBustTransform);
            arrow.GetComponent<Image>().sprite = arrowSprites[randomIndex];
            spawnedArrows.Add(arrow);

            yield return new WaitForSeconds(timeBetweenArrowDisplays);
        }

        StartInputTimer();
        canControl = true;
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    eSignState IndexToSignState(int index)
    {
        switch (index)
        {
            case 0: return eSignState.up;
            case 1: return eSignState.down;
            case 2: return eSignState.left;
            case 3: return eSignState.right;
            default: return eSignState.none;
        }
    }

    void ClearSpawnedArrows()
    {
        foreach (GameObject arrow in spawnedArrows)
            if (arrow != null) Destroy(arrow);

        spawnedArrows.Clear();
    }
}