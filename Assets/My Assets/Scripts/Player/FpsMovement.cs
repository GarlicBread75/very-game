using UnityEngine;

public class FpsMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] Vector3 drag;
    [SerializeField] Transform orientation;
    Rigidbody rb;
    float inputX;
    float inputZ;

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float airMultiplier;
    bool jumpPressed;

    [Header("Ground Check")]
    [SerializeField] float height;
    [SerializeField] LayerMask ground;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        inputZ = Input.GetAxisRaw("Horizontal");
        inputX = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && Grounded())
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDir = (orientation.forward * inputX + orientation.right * inputZ).normalized;
        Vector3 force = new Vector3(moveDir.x, 0, moveDir.z) * speed;
        Vector3 draggedForce = new Vector3(force.x / (drag.x + 1), 0, force.z / (drag.z + 1));
        if (Grounded())
        {
            rb.AddForce(draggedForce, ForceMode.Force);
        }
        else
        {
            rb.AddForce(draggedForce * airMultiplier, ForceMode.Force);
        }    

        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (flatVelocity.magnitude > speed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * speed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }

        if (jumpPressed)
        {
            rb.velocity = new Vector3 (rb.velocity.x, jumpForce, rb.velocity.z);
            jumpPressed = false;
        }
        Debug.Log(rb.velocity);
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, ground);
    }
}
