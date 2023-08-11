using UnityEngine;

public class SineWaves: MonoBehaviour
{
    [SerializeField] Vector3 num, waveSpeed, waveAmplitude;
    [SerializeField] bool timeX, timeY, timeZ;
    [SerializeField] Vector3 offset;
    MeshRenderer rend;
    Vector3 sin;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (timeX)
        {
            num.x += Time.deltaTime;
        }

        if (timeY)
        {
            num.y += Time.deltaTime;
        }

        if (timeZ)
        {
            num.z += Time.deltaTime;
        }

        sin.x = Mathf.Sin(num.x * waveSpeed.x);
        sin.y = Mathf.Sin(num.y * waveSpeed.y);
        sin.z = Mathf.Sin(num.z * waveSpeed.z);

        rend.material.SetColor("_BaseColor", new Color(Mathf.Abs(sin.x), Mathf.Abs(sin.y), Mathf.Abs(sin.z)));

        transform.position = offset + new Vector3(waveAmplitude.x * sin.x, waveAmplitude.y * sin.y, waveAmplitude.z * sin.z);

        transform.rotation = Quaternion.Euler(sin.x * 360, sin.y * 360, sin.z * 360);

        transform.localScale = new Vector3(waveAmplitude.x * sin.x, waveAmplitude.y * sin.y, waveAmplitude.z * sin.z);
    }
}
