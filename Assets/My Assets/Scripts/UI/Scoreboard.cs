using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI p1ScoreText, p2ScoreText, p1ScoreTextShadow, p2ScoreTextShadow;
    int p1Score, p2Score;

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
        }
        else
        if (playerNum == 2)
        {
            p2Score++;
        }
    }
}
