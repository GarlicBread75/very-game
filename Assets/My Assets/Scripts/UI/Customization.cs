using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customization : MonoBehaviour
{
    #region Variables
    [SerializeField] Material mat;
    [SerializeField] Material deadMat;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Gradient gradient;
    GradientColorKey[] colourKeys = new GradientColorKey[2];
    GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
    [SerializeField] bool eye;
    [SerializeField] string nameExtension;

    [Space]

    [SerializeField] Slider red;
    [SerializeField] Slider green;
    [SerializeField] Slider blue;
    #endregion

    void Awake()
    {
        colourKeys[0].color = Color.white;
        colourKeys[1].time = 1;
        alphaKeys[0].alpha = 1;
        alphaKeys[0].time = 0;
        alphaKeys[1].alpha = 1;
        alphaKeys[1].time = 1;
        GetData();
    }

    void FixedUpdate()
    {
        SetColour();

        if (!eye)
        {
            colourKeys[1].color = mat.GetColor("_BaseColor");
            gradient.SetKeys(colourKeys, alphaKeys);
            deadMat.SetColor("_BaseColor", gradient.Evaluate(0.5f));
            text.color = gradient.Evaluate(0.25f);
        }
        SaveData();
    }

    void SetColour()
    {
        mat.SetColor("_BaseColor", new Color(red.value, green.value, blue.value));
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat($"red{nameExtension}", red.value);
        PlayerPrefs.SetFloat($"green{nameExtension}", green.value);
        PlayerPrefs.SetFloat($"blue{nameExtension}", blue.value);
    }

    public void GetData()
    {
        red.value = PlayerPrefs.GetFloat($"red{nameExtension}");
        green.value = PlayerPrefs.GetFloat($"green{nameExtension}");
        blue.value = PlayerPrefs.GetFloat($"blue{nameExtension}");
    }
}