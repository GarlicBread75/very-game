using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    #region Variables
    [SerializeField] Health hp;

    [Header("Gun Stats")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bullet;
    [SerializeField] float damage;
    [SerializeField] int bulletCount;
    [SerializeField] float minSpeed, maxSpeed, spread;
    public float fireRate;
    [SerializeField] bool automatic;
    [SerializeField] KeyCode shootKey;
    bool shootPressed, atkPressed, shooting;
    float shootCd;
    [HideInInspector] public float fireRateModifier, damageModifier, knockbackModifier, bulletKnockbackModifier;

    [Space]

    [Header("Muzzle Flash")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Gradient muzzleGradient;
    [SerializeField] SpriteRenderer[] muzzleRenderers;
    [SerializeField] Light muzzleLight;
    [SerializeField] float minMuzzleDelay, maxMuzzleDelay;
    [SerializeField] float muzzleScaleModifier;
    Vector3 muzzleInitialScale;

    [Space]

    [Header("Knockback")]
    [SerializeField] Rigidbody player;
    [SerializeField] float knockback;
    [SerializeField] float bulletKnockback;

    [Space]

    [Header("Slider")]
    public Slider slider;
    public Gradient gradient1;
    public Gradient gradient2;
    [SerializeField] Image fill;
    [SerializeField] Image background;
    [HideInInspector] public Gradient gr;
    #endregion

    void Awake()
    {
        slider.maxValue = fireRate;
        gr = gradient1;
        fireRateModifier = 1;
        damageModifier = 1;
        knockbackModifier = 1;
        bulletKnockbackModifier = 1;
        muzzleInitialScale = muzzleFlash.transform.localScale;
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

        if (shootCd < fireRate / fireRateModifier)
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
        fill.color = gr.Evaluate(slider.normalizedValue);
        background.color = new Color(fill.color.r / 5, fill.color.g / 5, fill.color.b / 5);

        if (shootCd < fireRate / fireRateModifier)
        {
            shootCd += Time.fixedDeltaTime;
        }

        if (atkPressed)
        {
            atkPressed = false;
            Shoot();
        }

        if (muzzleFlash.activeInHierarchy)
        {
            muzzleFlash.transform.localScale += new Vector3(muzzleScaleModifier * Time.deltaTime, muzzleScaleModifier * Time.deltaTime, muzzleScaleModifier * Time.deltaTime);
        }
    }

    void Shoot()
    {
        shooting = true;

        foreach (SpriteRenderer sr in muzzleRenderers)
        {
            sr.color = muzzleGradient.Evaluate(Random.Range(0f, 1f));
            sr.material.SetColor("_TintColor", muzzleGradient.Evaluate(Random.Range(0f, 1f)));
        }

        muzzleLight.intensity = Random.Range(1f, 25f);
        muzzleLight.color = muzzleGradient.Evaluate(Random.Range(0f, 1f));

        muzzleFlash.SetActive(true);

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject shotBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = shotBullet.GetComponent<Rigidbody>();
            shotBullet.GetComponent<Bullet>().damage = damage * damageModifier;
            shotBullet.GetComponent<Bullet>().knockback = bulletKnockback * bulletKnockbackModifier;
            Vector2 dir = transform.rotation * Vector2.right;
            Vector2 pDir = Vector2.Perpendicular(dir) * Random.Range(-spread, spread);
            bulletRb.velocity = (dir + pDir) * Random.Range(minSpeed, maxSpeed);
            player.AddForce(-transform.right * knockback * knockbackModifier, ForceMode.Impulse);
        }

        shootCd = 0;
        Invoke(nameof(MuzzleFlashOff), Random.Range(minMuzzleDelay, maxMuzzleDelay));
        shooting = false;
    }

    void MuzzleFlashOff()
    {
        muzzleFlash.SetActive(false);
        muzzleFlash.transform.localScale = muzzleInitialScale;
    }
}
