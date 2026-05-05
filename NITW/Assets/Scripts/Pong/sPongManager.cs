using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class sPongManager : MonoBehaviour
{
    public static sPongManager pongManagerGlobal;

    [Header("Score")]
    public int winScore = 7;
    private int playerScore = 0;
    private int aiScore = 0;

    [Header("UI References")]
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI aiScoreText;
    public GameObject winPanel;
    public TextMeshProUGUI winMessageText;

    private sPongBall ball;
    private bool gameOver = false;

    void Awake()
    {
        if (pongManagerGlobal == null) pongManagerGlobal = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ball = FindObjectOfType<sPongBall>();
        if (winPanel) winPanel.SetActive(false);
        UpdateUI();
    }

    public void PlayerScores()
    {
        if (gameOver) return;
        playerScore++;
        UpdateUI();
        CheckWin();
        if (!gameOver) ball.ResetBall();
    }

    public void AIScores()
    {
        if (gameOver) return;
        aiScore++;
        UpdateUI();
        CheckWin();
        if (!gameOver) ball.ResetBall();
    }

    void CheckWin()
    {
        if (playerScore >= winScore)
        {
            EndGame("Player Wins!");
        }
        else if (aiScore >= winScore)
        {
            EndGame("AI Wins!");
        }
    }

    void EndGame(string message)
    {
        gameOver = true;
        ball.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        if (winPanel)
        {
            winPanel.SetActive(true);
            if (winMessageText) winMessageText.text = message;
        }
        else
        {
            Debug.Log(message + " — Press R to restart.");
        }
    }

    void UpdateUI()
    {
        if (playerScoreText) playerScoreText.text = "PLAYER:" + playerScore.ToString();
        if (aiScoreText) aiScoreText.text = "AI:" + aiScore.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Triggers player end game
            sPlayer.playerGlobal.EndVideoGame(eVideoGames.pong);

            // Disables Pong
            this.gameObject.SetActive(false);
        }
            
    }
}
