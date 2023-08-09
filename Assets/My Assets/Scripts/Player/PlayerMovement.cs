using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] float acceleration;
    Rigidbody rb;
    Health hp;
    Vector3 moveDir;
    float inputX, inputY;

    [Space]

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    bool canJump, jumpPressed;

    [Space]

    [Header("Dashing")]
    [SerializeField] float dashPower;
    [SerializeField] float dashCooldown;
    [SerializeField] GameObject dashReadyOutline;
    Vector3 dashDir;
    bool dashPressed;
    float dashCd;

    [Space]

    [Header("KeyBinds")]
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode dashKey;

    [Space]

    [Header("Gun")]
    [SerializeField] Transform gunHolder;
    [SerializeField] float rotSpeed;
    int rot;

    [HideInInspector] public float speedModifier, jumpModifier, dashPowerModifier;

    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hp = GetComponent<Health>();
        dashCd = dashCooldown;
        speedModifier = 1;
        jumpModifier = 1;
        dashPowerModifier = 1;
    }

    void Update()
    {
        if (hp.dead)
        {
            return;
        }

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

        if (Input.GetKey(up) && canJump)
        {
            jumpPressed = true;
        }

        if (Input.GetKeyDown(dashKey) && dashCd <= 0 && dashDir != Vector3.zero)
        {
            dashPressed = true;
        }

        moveDir = new Vector3(inputX, 0, 0).normalized;
        dashDir = new Vector3(inputX, inputY, 0).normalized;
    }

    void FixedUpdate()
    {
        if (hp.dead)
        {
            return;
        }

        rb.AddForce(moveDir * acceleration * speedModifier, ForceMode.Force);

        if (dashCd > 0)
        {
            dashCd -= Time.fixedDeltaTime;
        }
        else
        {
            if (!dashReadyOutline.activeInHierarchy)
            {
                dashReadyOutline.SetActive(true);
            }
        }

        if (jumpPressed)
        {
            jumpPressed = false;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * jumpModifier, 0);
            canJump = false;
        }

        if (dashPressed)
        {
            dashPressed = false;
            dashReadyOutline.SetActive(false);
            rb.AddForce(dashDir * dashPower * dashPowerModifier, ForceMode.Impulse);
            dashCd = dashCooldown;
        }

        if (inputX == 1)
        {
            if (inputY == 0)
            {
                rot = 0;
            }
            else
            if (inputY == 1)
            {
                rot = 45;
            }
            else
            if (inputY == -1)
            {
                rot = 315;
            }
            gunHolder.rotation = Quaternion.Lerp(gunHolder.rotation, Quaternion.Euler(new Vector3(0, 0, rot)), rotSpeed * Time.deltaTime);
        }
        else
        if (inputX == -1)
        {
            if (inputY == 0)
            {
                rot = 180;
            }
            else
            if (inputY == 1)
            {
                rot = 135;
            }
            else
            if (inputY == -1)
            {
                rot = 225;
            }
            gunHolder.rotation = Quaternion.Lerp(gunHolder.rotation, Quaternion.Euler(new Vector3(0, 0, rot)), rotSpeed * Time.deltaTime);
        }
        else
        if (inputX == 0)
        {
            if (inputY == 0)
            {

            }
            else
            if (inputY == 1)
            {
                rot = 90;
            }
            else
            if (inputY == -1)
            {
                rot = 270;
            }
            gunHolder.rotation = Quaternion.Lerp(gunHolder.rotation, Quaternion.Euler(new Vector3(0, 0, rot)), rotSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }
}