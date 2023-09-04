using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed;
    [SerializeField] Vector3 offset;

    [SerializeField] PlayerMovement pm;

    void Update()
    {
        Vector3 desiredPos = target.position + offset;
        if (pm != null)
        {
            desiredPos += new Vector3(0.35f * pm.inputX, 0.35f * pm.inputY, 0);
        }
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothPos;
    }
}
