using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float minVolume, maxVolume;
    [Range(0f, 3f)]
    public float minPitch, maxPitch;

    public bool loop;

    public AudioSource source;
}