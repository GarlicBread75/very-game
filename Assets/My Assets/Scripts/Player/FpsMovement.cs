using UnityEngine;

public class FpsMovement : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] float walkAcceleration;
    [SerializeField] float maxWalkSpeed;
    [SerializeField] Transform orientation;
    Vector3 moveDir;
    float inputX;
    float inputZ;
    float acceleration;
    float maxSpeed;

    [Header("Sprinting")]
    [SerializeField] float sprintAcceleration;
    [SerializeField] float maxSprintSpeed;

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    bool jumpPressed;

    [Header("Crouching")]
    [SerializeField] float crouchAcceleration;
    [SerializeField] float maxCrouchSpeed;
    [SerializeField] float crouchHeight;
    float startHeight;

    [Header("Ground Check")]
    [SerializeField] LayerMask ground;
    [SerializeField] CapsuleCollider col;
    float height;

    [Header("Slope Check")]
    [SerializeField] float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;

    [Header("KeyBinds")]
    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode sprintKey;
    [SerializeField] KeyCode crouchKey;

    Rigidbody rb;
    ConstantForce cf;
    float originalGravity;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>();
        rb.freezeRotation = true;
        acceleration = walkAcceleration;
        maxSpeed = maxWalkSpeed;
        startHeight = transform.localScale.y;
        height = col.height;
        originalGravity = cf.force.y;
    }

    void Update()
    {
        PlayerInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
        SpeedLimit();
        Jumping();  
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, ground);
    }

    void PlayerInput()
    {
        // Movement Input
        inputZ = Input.GetAxisRaw("Horizontal");
        inputX = Input.GetAxisRaw("Vertical");

        // Jump Imput
        if (Input.GetKey(jumpKey) && Grounded())
        {
            jumpPressed = true;
        }

        // Sprinting Input
        if (Input.GetKeyDown(sprintKey))
        {
            acceleration = sprintAcceleration;
            maxSpeed = maxSprintSpeed;
        }
        else
        if (Input.GetKeyUp(sprintKey))
        {
            acceleration = walkAcceleration;
            maxSpeed = maxWalkSpeed;
        }

        // Crouch Input
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
            acceleration = crouchAcceleration;
            maxSpeed = maxCrouchSpeed;
        }
        else
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startHeight, transform.localScale.z);
            acceleration = walkAcceleration;
            maxSpeed = maxWalkSpeed;
        }
    }

    void MovePlayer()
    {
        moveDir = (orientation.forward * inputX + orientation.right * inputZ).normalized;
        moveDir.y = 0;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * acceleration, ForceMode.Force);
        }

        rb.AddForce(moveDir * acceleration, ForceMode.Force);
    }

    void SpeedLimit()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVel.magnitude > maxSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * maxSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    void Jumping()
    {
        if (jumpPressed)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            exitingSlope = true;
            jumpPressed = false;
        }
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, height * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        else
        {
            return false;
        }
    }

    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }
}
