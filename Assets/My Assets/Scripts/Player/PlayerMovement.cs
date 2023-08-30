using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] float acceleration;
    [SerializeField] bool menu;
    Rigidbody rb;
    Health hp;
    Vector3 moveDir;
    float inputX, inputY;

    [Space]

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] string otherPlayerTag;
    [SerializeField] GameObject jumpDust;
    [SerializeField] LayerMask ground;
    [SerializeField] float distance;
    bool canJump, jumpPressed;

    [Space]

    [Header("Dashing")]
    [SerializeField] float dashPower;
    [SerializeField] float dashCooldown;
    [SerializeField] GameObject dashDust;
    [SerializeField] GameObject dashReadyOutline;
    Vector3 dashDir;
    bool dashPressed;
    float dashCd;

    [Space]

    [Header("KeyBinds")]
    [SerializeField] KeyBinding[] keys;

    [Space]

    [Header("Gun")]
    [SerializeField] Transform gunHolderTransform;
    [SerializeField] Transform gunTransform;
    [SerializeField] float rotSpeed;
    int gunRotX, gunHolderRotZ;

    [Space]

    [Header("Sounds")]
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource dashSound;
    [SerializeField] float minVolume, maxVolume, minPitch, maxPitch;
    BoxCollider col;

    [HideInInspector] public float speedModifier, jumpModifier, dashPowerModifier;

    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        if (menu)
        {
            return;
        }
        hp = GetComponent<Health>();
        dashCd = dashCooldown;
        speedModifier = 1;
        jumpModifier = 1;
        dashPowerModifier = 1;
        if (gameObject.name == "Body 2")
        {
            gunHolderRotZ = 180;
            gunRotX = 180;
        }
    }

    void Start()
    {
        gunTransform = gunHolderTransform.gameObject.GetComponent<GunHolder>().gunTransform;
    }

    void Update()
    {
        if (!menu)
        {
            if (hp.dead)
            {
                return;
            }
        }

        PlayerInput();
        moveDir = new Vector3(inputX, 0, 0).normalized;

        if (Input.GetKey(keys[0].keyCode) && canJump)
        {
            jumpPressed = true;
        }
        
        if (menu)
        {
            return;
        }

        dashDir = new Vector3(inputX, inputY, 0).normalized;

        if (Input.GetKeyDown(keys[4].keyCode) && dashCd <= 0 && dashDir != Vector3.zero)
        {
            dashPressed = true;
        }

    }

    void FixedUpdate()
    {
        if (!menu)
        {
            if (hp.dead)
            {
                return;
            }
        }

        rb.AddForce(moveDir * acceleration * speedModifier, ForceMode.Force);

        if (jumpPressed)
        {
            jumpPressed = false;
            PlaySound(jumpSound);
            GameObject thing = Instantiate(jumpDust, transform.position - new Vector3(0, 0.65f, 0), Quaternion.identity);
            Destroy(thing, 0.5f);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * jumpModifier, 0);
        }

        if (menu)
        {
            return;
        }

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

        if (dashPressed)
        {
            dashPressed = false;
            PlaySound(dashSound);
            GameObject thing = Instantiate(dashDust, transform.position - new Vector3(0, 0.65f, 0), Quaternion.identity);
            Destroy(thing, 0.5f);
            dashReadyOutline.SetActive(false);
            rb.AddForce(dashDir * dashPower * dashPowerModifier, ForceMode.Impulse);
            dashCd = dashCooldown;
        }

        GunRotation();
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag(otherPlayerTag)) && transform.position.y > collision.transform.position.y)
        {
            canJump = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag(otherPlayerTag))
        {
            canJump = false;
        }
    }

    void PlayerInput()
    {
        if (Input.GetKey(keys[0].keyCode))
        {
            inputY = 1;
        }
        else
        if (Input.GetKey(keys[1].keyCode))
        {
            inputY = -1;
        }
        else
        {
            inputY = 0;
        }

        if (Input.GetKey(keys[3].keyCode))
        {
            inputX = 1;
        }
        else
        if (Input.GetKey(keys[2].keyCode))
        {
            inputX = -1;
        }
        else
        {
            inputX = 0;
        }
    }

    void GunRotation()
    {
        switch (inputX)
        {
            case -1:
                switch (inputY)
                {
                    case -1:
                        gunHolderRotZ = 225;
                        gunRotX = 180;
                        break;

                    case 0:
                        gunHolderRotZ = 180;
                        gunRotX = 180;
                        break;

                    case 1:
                        gunHolderRotZ = 135;
                        gunRotX = 180;
                        break;
                }
                break;

            case 0:
                switch (inputY)
                {
                    case -1:
                        gunHolderRotZ = 270;
                        break;

                    case 1:
                        gunHolderRotZ = 90;
                        break;
                }
                break;

            case 1:
                switch (inputY)
                {
                    case -1:
                        gunHolderRotZ = 315;
                        gunRotX = 0;
                        break;

                    case 0:
                        gunHolderRotZ = 0;
                        gunRotX = 0;
                        break;

                    case 1:
                        gunHolderRotZ = 45;
                        gunRotX = 0;
                        break;
                }
                break;
        }
        gunHolderTransform.rotation = Quaternion.Lerp(gunHolderTransform.rotation, Quaternion.Euler(new Vector3(0, 0, gunHolderRotZ)), rotSpeed * Time.deltaTime);
        gunTransform.localRotation = Quaternion.Euler(new Vector3(gunRotX, 0, 0));
    }

    bool Grounded()
    {
        return Physics.BoxCast(transform.position, col.bounds.extents, Vector3.down, transform.rotation, distance, ground);
    }

    void PlaySound(AudioSource source)
    {
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        if (!source.isPlaying)
        {
            source.Play();
        }
    }
}