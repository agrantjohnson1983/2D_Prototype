using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Manages the popup that shows the hold prompt.
//
// Required UI hierarchy:
//   Canvas (Screen Space - Overlay)
//     InteractionPopup  [RectTransform]
//       Background      [Image]
//       LabelText       [TextMeshProUGUI]
//       ProgressBar     [Slider, Interactable = false]
public class uInteractionPopup : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Root panel to show / hide.")]
    public GameObject popupPanel;

    [Tooltip("Optional label - text is set at runtime or left as-is.")]
    public TextMeshProUGUI promptLabel;

    //[Tooltip("Slider used as a progress bar (Min=0, Max=1, Interactable=false).")]
    //public Slider progressBar;

    public Image progressImage;

    [Header("Options")]
    //public string promptText = "Hold SPACE to interact";

    [Tooltip("Animate the panel fading in / out.")]
    public bool useFade = true;

    [Tooltip("Fade speed (units per second).")]
    public float fadeSpeed = 8f;

    private CanvasGroup _group;
    private bool _visible = false;

    private void Awake()
    {
        if (popupPanel != null)
        {
            _group = popupPanel.GetComponent<CanvasGroup>();
            if (_group == null) _group = popupPanel.AddComponent<CanvasGroup>();
        }

        if (promptLabel != null)
            promptLabel.text = "";

        SetAlpha(0f);
        if (popupPanel != null) popupPanel.SetActive(false);
        //if (progressBar != null) progressBar.value = 0f;
        if (progressImage != null) progressImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (!useFade || _group == null) return;

        float target = _visible ? 1f : 0f;
        float current = _group.alpha;

        if (!Mathf.Approximately(current, target))
        {
            _group.alpha = Mathf.MoveTowards(current, target, fadeSpeed * Time.deltaTime);

            if (_group.alpha <= 0f && !_visible)
                popupPanel.SetActive(false);
        }
    }

    public void ShowPopup()
    {
        if (popupPanel == null) return;

        _visible = true;
        popupPanel.SetActive(true);

        if (!useFade) SetAlpha(1f);

        SetProgress(0f);
    }

    public void SetText(string _text)
    {
        //Debug.Log("setting poup text to: " + _text);
        promptLabel.text = _text;
    }

    public void HidePopup()
    {
        _visible = false;
        SetProgress(0f);

        if (!useFade && popupPanel != null)
            popupPanel.SetActive(false);
    }

    public void SetProgress(float normalised)
    {
        //if (progressBar != null)
        //    progressBar.value = Mathf.Clamp01(normalised);

        if (progressImage != null)
            progressImage.fillAmount = Mathf.Clamp01(normalised);
    }

    private void SetAlpha(float a)
    {
        if (_group != null) _group.alpha = a;
    }
}