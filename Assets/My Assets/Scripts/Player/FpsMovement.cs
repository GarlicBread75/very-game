using UnityEngine;

public class FpsMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float walkDrag;
    [SerializeField] float sprintDrag;
    [SerializeField] Transform orientation;
    Rigidbody rb;
    Vector3 moveDir;
    float inputX;
    float inputZ;
    float drag;

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
        drag = walkDrag;
    }

    void Update()
    {
        inputZ = Input.GetAxisRaw("Horizontal");
        inputX = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            drag = sprintDrag;
        }
        else
        {
            drag = walkDrag;
        }
        rb.drag = drag;

        if (Input.GetKey(KeyCode.Space) && Grounded())
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        moveDir = (orientation.forward * inputX + orientation.right * inputZ).normalized;
        if (Grounded())
        {
            rb.AddForce(moveDir * speed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDir * speed * airMultiplier, ForceMode.Force);
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
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, ground);
    }
}
