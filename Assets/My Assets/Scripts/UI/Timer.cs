using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float time;

    void Start()
    {
        time = 0f;
    }

    void Update()
    {
        time += Time.deltaTime;
        timerText.text = time.ToString("F2");
    }
}
