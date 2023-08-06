using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed;
    [SerializeField] Vector3 offset;

    void Update()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothPos;
    }
}
