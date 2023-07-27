using UnityEngine;

public class FpsMovement : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] float walkAcceleration;
    [SerializeField] float maxWalkSpeed;
    [SerializeField] Transform orientation;
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
    [SerializeField] float height;
    [SerializeField] LayerMask ground;

    [Header("KeyBinds")]
    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode sprintKey;
    [SerializeField] KeyCode crouchKey;

    [Header("Movement State")]
    [SerializeField] States state;
    enum States { walking, sprinting, air, crouching }

    Rigidbody rb;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startHeight = transform.localScale.y;
    }

    void Update()
    {
        PlayerInput();
        StateManager();
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

        // Sprint Input
        if (Input.GetKey(sprintKey))
        {
            acceleration = sprintAcceleration;
            maxSpeed = maxSprintSpeed;
        }
        else
        {
            acceleration = walkAcceleration;
            maxSpeed = maxWalkSpeed;
        }

        // Crouch Input
        if (Input.GetKey(crouchKey))
        {
            acceleration = crouchAcceleration;
            maxSpeed = maxCrouchSpeed;
        }
        else
        {
            acceleration = walkAcceleration;
            maxSpeed = maxWalkSpeed;
        }
    }

    void MovePlayer()
    {
        Vector3 moveDir = (orientation.forward * inputX + orientation.right * inputZ).normalized;
        moveDir.y = 0;
        rb.AddForce(moveDir * acceleration, ForceMode.Force);
    }

    void SpeedLimit()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void Jumping()
    {
        if (jumpPressed)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpPressed = false;
        }
    }

    void StateManager()
    {
        if (Grounded())
        {
            if (Input.GetKey(sprintKey))
            {
                state = States.sprinting;
            }
            else
            if (Input.GetKey(crouchKey))
            {
                state = States.crouching;
            }
            else
            {
                state = States.walking;
            }
        }
        else
        {
            state = States.crouching;
        }
    }
}
