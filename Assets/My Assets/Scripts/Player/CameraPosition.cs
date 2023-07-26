using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
