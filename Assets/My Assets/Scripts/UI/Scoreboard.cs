using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] TextMeshProUGUI p1ScoreText;
    [SerializeField] TextMeshProUGUI p2ScoreText, victoryText, victoryShadow1, victoryShadow2;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] Material p1, p2, p1Font, p2Font;
    [SerializeField] Color c;
    [SerializeField] float sceneLoadDelay;
    int p1Score, p2Score;
    [HideInInspector] public bool alreadyScored;

    void Awake()
    {
        p1Score = scoreManager.p1Score;
        p2Score = scoreManager.p2Score;
        p1Font.SetColor("_UnderlayColor", p1.GetColor("_BaseColor"));
        p2Font.SetColor("_UnderlayColor", p2.GetColor("_BaseColor"));
    }

    void FixedUpdate()
    {
        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();
    }

    public void IncreaseScore(int playerNum)
    {
        if (alreadyScored)
        {
            return;
        }

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
        Invoke("RoundOver", sceneLoadDelay);
    }

    void RoundOver()
    {
        SceneManager.LoadScene("Level");
    }

    public void AlreadyScored()
    {
        alreadyScored = true;
    }
}
