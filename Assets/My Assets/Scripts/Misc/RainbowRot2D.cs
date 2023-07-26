using UnityEngine;

public class RainbowRot2D : MonoBehaviour
{
    float hue;
    [SerializeField] float colourModifier;
    [SerializeField] float saturation;
    [SerializeField] float value;

    [Space]

    float rot;
    [SerializeField] float rotationModifier;

    [SerializeField] bool canColour;
    [SerializeField] bool canRot;

    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        hue = Random.Range(0f, 1f);
        rot = Random.Range(0f, 360f);
    }

    void Update()
    {
        if (canColour)
        {
            if (hue > 1f)
            {
                hue = 0;
            }
            else
            {
                hue += Time.deltaTime / colourModifier;
            }

            sr.color = Color.HSVToRGB(hue, saturation, value);
        }

        if (canRot)
        {
            if (rot > 1440)
            {
                rot = 0;
            }
            else
            {
                rot += Time.deltaTime / rotationModifier;
            }

            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
    }
}
