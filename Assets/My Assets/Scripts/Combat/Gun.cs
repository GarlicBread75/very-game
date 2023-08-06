using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    #region Variables
    [SerializeField] Health hp;

    [Header("Gun Stats")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bullet;
    [SerializeField] int damage;
    [SerializeField] int bulletCount;
    [SerializeField] float speed, fireRate, spread;
    [SerializeField] bool automatic;
    [SerializeField] KeyCode shootKey;
    bool shootPressed, atkPressed, shooting;
    float shootCd;

    [Space]

    [Header("Knockback")]
    [SerializeField] Rigidbody player;
    [SerializeField] float knockback;
    [SerializeField] float bulletKnockback;

    [Space]

    [Header("Slider")]
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] Image background;
    #endregion

    void Awake()
    {
        slider.maxValue = fireRate;
    }

    void Update()
    {
        if (hp.dead)
        {
            return;
        }

        if (automatic)
        {
            shootPressed = Input.GetKey(shootKey);
        }
        else
        {
            shootPressed = Input.GetKeyDown(shootKey);
        }

        if (shootCd < fireRate)
        {
            return;
        }

        if (shootPressed && !shooting)
        {
            atkPressed = true;
        }
    }

    void FixedUpdate()
    {
        if (hp.dead)
        {
            return;
        }

        slider.value = shootCd;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        background.color = new Color(fill.color.r / 5, fill.color.g / 5, fill.color.b / 5);

        if (shootCd < fireRate)
        {
            shootCd += Time.fixedDeltaTime;
        }

        if (atkPressed)
        {
            atkPressed = false;
            Shoot();
        }
    }

    void Shoot()
    {
        shooting = true;
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject shotBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = shotBullet.GetComponent<Rigidbody>();
            shotBullet.GetComponent<Bullet>().damage = damage;
            shotBullet.GetComponent<Bullet>().knockback = bulletKnockback;
            Vector2 dir = transform.rotation * Vector2.right;
            Vector2 pDir = Vector2.Perpendicular(dir) * Random.Range(-spread, spread);
            bulletRb.velocity = (dir + pDir) * speed;
            player.AddForce(-transform.right * knockback, ForceMode.Impulse);
        }
        shootCd = 0;
        shooting = false;
    }
}
