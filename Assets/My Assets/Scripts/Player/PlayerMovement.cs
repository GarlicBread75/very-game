using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    Rigidbody rb;
    Vector3 moveDir;
    float inputX;
    float inputY;

    [Space]

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 size;
    [SerializeField] float distance;
    [SerializeField] LayerMask ground;
    BoxCollider col;
    bool grounded;
    bool jumpPressed;

    [Space]

    [Header("Dashing")]
    [SerializeField] Vector3 power;
    [SerializeField] float duration;
    [SerializeField] float cooldown;
    TrailRenderer tr;
    Vector3 dashDir;
    bool dashPressed;
    bool dashing;
    float cd;

    [Header("KeyBinds")]
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode dashKey;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
        col = GetComponent<BoxCollider>();
        cd = cooldown;
    }

    void Update()
    {
        PlayerInput();
        moveDir = new Vector3(inputX, 0, 0).normalized;
        dashDir = new Vector3(inputX, inputY, 0);
        Debug.Log(Grounded());
    }

    void FixedUpdate()
    {
        if (cd > 0)
        {
            cd -= Time.fixedDeltaTime;
        }

        rb.AddForce(moveDir * acceleration, ForceMode.Force);
        SpeedLimit();

        if (jumpPressed)
        {
            jumpPressed = false;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        }

        if (dashPressed)
        {
            dashPressed = false;
            StartCoroutine(Dashing());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }    
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(col.bounds.center + offset, size);
    }

    bool Grounded()
    {
        return Physics.BoxCast(col.bounds.center + offset, size, Vector3.down, Quaternion.Euler(0, 0, 0), distance, ground);
    }

    void PlayerInput()
    {
        if (Input.GetKey(up))
        {
            inputY = 1;
        }
        else
        if (Input.GetKey(down))
        {
            inputY = -1;
        }
        else
        {
            inputY = 0;
        }

        if (Input.GetKey(right))
        {
            inputX = 1;
        }
        else
        if (Input.GetKey(left))
        {
            inputX = -1;
        }
        else
        {
            inputX = 0;
        }

        if (Input.GetKey(jumpKey) && grounded)
        {
            jumpPressed = true;
        }

        if (Input.GetKeyDown(dashKey) && cd <= 0 && dashDir != Vector3.zero)
        {
            dashPressed = true;
        }
    }

    void SpeedLimit()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, 0);
        if (flatVel.magnitude > maxSpeed)
        {
            int thing = 1;
            if (grounded)
            {
                thing = 1;
            }
            else
            {
                thing = 2;
            }
            Vector3 limitedVel = flatVel.normalized * maxSpeed * thing;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, 0);
        }
    }

    IEnumerator Dashing()
    {
        dashing = true;
        tr.emitting = true;
        rb.AddForce(new Vector3(dashDir.x * power.x, dashDir.y * power.y, 0), ForceMode.Impulse);
        yield return new WaitForSeconds(duration);
        tr.emitting = false;
        dashing = false;
        cd = cooldown;
    }
}