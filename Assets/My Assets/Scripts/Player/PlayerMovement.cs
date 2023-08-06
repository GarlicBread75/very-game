//using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    Rigidbody rb;
    Health hp;
    Vector3 moveDir;
    float inputX;
    float inputY;

    [Space]

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    bool canJump;
    bool jumpPressed;

    [Space]

    /*[Header("Dashing")]
    [SerializeField] Vector3 power;
    [SerializeField] float duration;
    [SerializeField] float cooldown;
    TrailRenderer tr;
    Vector3 dashDir;
    bool dashPressed;
    bool dashing;
    float cd;
    
    [Space]*/

    [Header("KeyBinds")]
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    //[SerializeField] KeyCode dashKey;

    [Space]

    [Header("Gun")]
    [SerializeField] Transform gunHolder;
    [SerializeField] float rotSpeed;
    int rot;

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hp = GetComponent<Health>();
        //tr = GetComponent<TrailRenderer>();
        //cd = cooldown;
    }

    void Update()
    {
        if (hp.dead)
        {
            return;
        }
        PlayerInput();
        moveDir = new Vector3(inputX, 0, 0).normalized;
        //dashDir = new Vector3(inputX, inputY, 0);
    }

    void FixedUpdate()
    {
        if (hp.dead)
        {
            return;
        }
        /*if (cd > 0)
        {
            cd -= Time.fixedDeltaTime;
        }*/

        rb.AddForce(moveDir * acceleration, ForceMode.Force);

        if (jumpPressed)
        {
            jumpPressed = false;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
            canJump = false;
        }

        /*if (dashPressed)
        {
            dashPressed = false;
            StartCoroutine(Dashing());
        }*/

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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            canJump = true;
            //cd = 0;
        }    
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
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

        if (Input.GetKey(up) && canJump)
        {
            jumpPressed = true;
        }

        /*if (Input.GetKeyDown(dashKey) && cd <= 0 && dashDir != Vector3.zero)
        {
            dashPressed = true;
        }*/
    }

    /*IEnumerator Dashing()
    {
        dashing = true;
        tr.emitting = true;
        rb.AddForce(new Vector3(dashDir.x * power.x, dashDir.y * power.y, 0), ForceMode.Impulse);
        yield return new WaitForSeconds(duration);
        tr.emitting = false;
        dashing = false;
        cd = cooldown;
    }*/
}