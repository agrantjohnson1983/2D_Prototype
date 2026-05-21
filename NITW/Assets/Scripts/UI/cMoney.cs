using UnityEngine;
using TMPro;
using Unity.Collections;
using System.Collections;

public class cMoney : MonoBehaviour
{
    public float startingMoney;
    float currentMoney;

    public TextMeshProUGUI textMoney;

    public static cMoney moneyGlobal;

    [Header("Animation Settings")]
    public float duration = 1.2f;
    public float floatHeight = 80f;
    public float expandScale = 1.6f;

    [Header("Colors")]
    public Color startColor = Color.white;
    public Color peakColor = new Color(1f, 0.85f, 0.1f);
    public Color endColor = new Color(1f, 0.85f, 0.1f, 0f);

    private TextMeshProUGUI _text;
    private RectTransform _rect;

    public GameObject textAnimatePrefab;
    public Transform textAnimateSpawnTransform;

    private void Awake()
    {
        if (cMoney.moneyGlobal == null)
            moneyGlobal = this;
        else
            Destroy(this);

        _text = GetComponent<TextMeshProUGUI>();
        _rect = GetComponent<RectTransform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentMoney = startingMoney;
        SetTextUI(currentMoney);
    }

    public void GetMoney(float _amount)
    {
        //Debug.Log("Getting money...");

        if (_amount < 0)
            return;

        currentMoney += _amount;

        GameObject tempObj;

        tempObj = Instantiate(textAnimatePrefab, textAnimateSpawnTransform);

        _text = tempObj.GetComponent<TextMeshProUGUI>();
        _rect = tempObj.GetComponent<RectTransform>();

        Play(_amount, tempObj);

        SetTextUI(currentMoney);
    }

    void SetTextUI(float _amount)
    {
        //Debug.Log("Setting money text UI");
        textMoney.text = "$"+_amount.ToString();
    }
    public void Play(float amount, GameObject _tempObj)
    {
        _text.text = "+" + amount.ToString();
        StartCoroutine(Animate(_tempObj));
    }

    private IEnumerator Animate(GameObject _tempObj)
    {
        float elapsed = 0f;
        Vector2 startPos = _rect.anchoredPosition;
        Vector3 startScale = Vector3.one;
        Vector3 peakScale = Vector3.one * expandScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            _rect.anchoredPosition = startPos + Vector2.up * (floatHeight * t);

            float scaleCurve = Mathf.Sin(t * Mathf.PI);
            _rect.localScale = Vector3.Lerp(startScale, peakScale, scaleCurve);

            if (t < 0.5f)
                _text.color = Color.Lerp(startColor, peakColor, t * 2f);
            else
                _text.color = Color.Lerp(peakColor, endColor, (t - 0.5f) * 2f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(_tempObj);
    }
}
