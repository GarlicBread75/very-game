using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] TextMeshProUGUI p1ScoreText;
    [SerializeField] TextMeshProUGUI p2ScoreText, p1ScoreTextShadow, p2ScoreTextShadow, victoryText, victoryShadow1, victoryShadow2;
    [SerializeField] ScoreManager scoreManager;
    int p1Score, p2Score;

    [Space]

    [Header("Scene Load")]
    [SerializeField] string [] sceneNames;
    [SerializeField] float sceneLoadDelay;

    void Awake()
    {
        p1Score = scoreManager.p1Score;
        p2Score = scoreManager.p2Score;
    }

    void FixedUpdate()
    {
        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();
        p1ScoreTextShadow.text = p1ScoreText.text;
        p2ScoreTextShadow.text = p2ScoreText.text;
    }

    public void IncreaseScore(int playerNum)
    {
        if (playerNum == 1)
        {
            p1Score++;
            scoreManager.p1Score = p1Score;
            victoryShadow1.text = $"Player {playerNum} wins!";
            victoryShadow1.gameObject.SetActive(true);
        }
        else
        if (playerNum == 2)
        {
            p2Score++;
            scoreManager.p2Score = p2Score;
            victoryShadow2.text = $"Player {playerNum} wins!";
            victoryShadow2.gameObject.SetActive(true);
        }
        victoryText.text = $"Player {playerNum} wins!";
        victoryText.gameObject.SetActive(true);
        Invoke("ChangeScene", sceneLoadDelay);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(sceneNames[Random.Range(0, sceneNames.Length)]);
    }
}