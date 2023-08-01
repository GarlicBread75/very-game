using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    public bool grounded;

    void Awake()
    {
        GetComponent<BoxCollider>().size = transform.lossyScale;
    }

    void FixedUpdate()
    {
        transform.position = target.position + offset;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
