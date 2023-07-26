using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    TextMeshProUGUI counterText;
    int count = 0;

    void Start()
    {
        counterText = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        ShowCount();
    }

    void ShowCount()
    {
        counterText.text = count.ToString();
    }

    public void AddCount(int num)
    {
        count += num;
    }
}
