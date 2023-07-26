using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    [SerializeField] private int maxFps;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = maxFps;
    }
}
