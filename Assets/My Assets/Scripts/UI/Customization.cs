using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customization : MonoBehaviour
{
    #region Variables
    [SerializeField] Material mat, deadMat;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image list, image;
    [SerializeField] Gradient gradient;
    [SerializeField] UIGradient saturationGradient, valueGradient;
    [SerializeField] bool eye;
    [SerializeField] string nameExtension;

    [SerializeField] Slider hue;
    [SerializeField] Slider saturation;
    [SerializeField] Slider value;

    GradientColorKey[] colourKeys = new GradientColorKey[2];
    GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
    int hueNum;
    #endregion

    void Start()
    {
        GetData();
    }

    void Awake()
    {
        colourKeys[0].color = Color.white;
        colourKeys[1].time = 1;
        alphaKeys[0].alpha = 1;
        alphaKeys[0].time = 0;
        alphaKeys[1].alpha = 1;
        alphaKeys[1].time = 1;
    }

    void FixedUpdate()
    {
        SetColour();

        colourKeys[1].color = mat.GetColor("_BaseColor");
        gradient.SetKeys(colourKeys, alphaKeys);

        image.color = gradient.Evaluate(1);
        text.color = gradient.Evaluate(0.25f);
        list.color = gradient.Evaluate(0.1f);

        if (!eye)
        {
            deadMat.SetColor("_BaseColor", gradient.Evaluate(0.5f));
            saturationGradient.thing1 = gradient.Evaluate(0.99f);
            valueGradient.m_color2 = gradient.Evaluate(1);
        }
        SaveData();
    }

    void SetColour()
    {
        mat.SetColor("_BaseColor", Color.HSVToRGB(hue.value, saturation.value, value.value));
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat($"hue{nameExtension}", hue.value);
        PlayerPrefs.SetFloat($"saturation{nameExtension}", saturation.value);
        PlayerPrefs.SetFloat($"value{nameExtension}", value.value);
    }

    public void GetData()
    {
        hue.value = PlayerPrefs.GetFloat($"hue{nameExtension}");
        saturation.value = PlayerPrefs.GetFloat($"saturation{nameExtension}");
        value.value = PlayerPrefs.GetFloat($"value{nameExtension}");
    }
}