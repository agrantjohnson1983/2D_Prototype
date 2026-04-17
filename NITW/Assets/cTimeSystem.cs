using UnityEngine;
using TMPro;

public class cTimeSystem : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public static float ElapsedTime; // in seconds

    // 60 = 1 real second = 1 in game minute
    public float timeScale = 60f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Advance time
        ElapsedTime += Time.deltaTime * timeScale;

        // Convert to minutes
        int minutes = Mathf.FloorToInt(ElapsedTime / 60f);

        // Update UI
        if (timeText != null)
        {
            timeText.text = minutes.ToString();
        }
    }
}
