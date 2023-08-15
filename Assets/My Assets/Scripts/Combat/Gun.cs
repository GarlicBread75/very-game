using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    #region Variables
    [SerializeField] Health hp;

    [Header("Gun")]
    [SerializeField] Transform firePoint;
    [SerializeField] int bulletCount;
    [SerializeField] float spread;
    public float fireRate;
    [SerializeField] bool automatic;
    [SerializeField] KeyCode shootKey;
    bool shootPressed, atkPressed, shooting;
    float shootCd;
    [HideInInspector] public float fireRateModifier, damageModifier, knockbackModifier, bulletKnockbackModifier;

    [Space]

    [Header("Bullet")]
    [SerializeField] GameObject bullet;
    [SerializeField] float damage, minSpeed, maxSpeed, timeToDestroy;

    [Space]

    [Header("Muzzle Flash")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Gradient muzzleGradient;
    [SerializeField] SpriteRenderer[] muzzleRenderers;
    [SerializeField] Light muzzleLight;
    [SerializeField] float minMuzzleScale, maxMuzzleScale, minMuzzleFlashSpeed, maxMuzzleFlashSpeed, minMuzzleLifetime, maxMuzzleLifetime;
    float num, amplitude, multiplier, sin;

    [Space]

    [Header("Knockback")]
    [SerializeField] Rigidbody player;
    [SerializeField] float knockback;
    [SerializeField] float bulletKnockback;

    [Space]

    [Header("Gun Cooldown Slider")]
    public Slider slider;
    public Gradient gradient1;
    public Gradient gradient2;
    [SerializeField] Image fill;
    [SerializeField] Image background;
    [HideInInspector] public Gradient gr;

    [Space]

    [Header("Sounds")]
    [SerializeField] AudioSource shootSound;
    [SerializeField] AudioSource bulletImpactSound;
    [SerializeField] float minVolume, maxVolume, minPitch, maxPitch;
    #endregion

    void Awake()
    {
        slider.maxValue = fireRate;
        gr = gradient1;
        fireRateModifier = 1;
        damageModifier = 1;
        knockbackModifier = 1;
        bulletKnockbackModifier = 1;
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
            num += Time.deltaTime;
            sin = Mathf.Abs(amplitude * Mathf.Sin(num * multiplier));
            muzzleFlash.transform.localScale = new Vector3(sin, sin, sin);
        }
    }

    void Shoot()
    {
        shooting = true;
        PlaySound(shootSound);
        StartCoroutine(MuzzleFlash());
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject shotBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            Bullet bulllet = shotBullet.GetComponent<Bullet>();
            bulllet.SetStats(damage * damageModifier, bulletKnockback * bulletKnockbackModifier);
            bulllet.impactSound = bulletImpactSound;
            Vector2 dir = transform.rotation * Vector2.right;
            Vector2 pDir = Vector2.Perpendicular(dir) * Random.Range(-spread, spread);
            shotBullet.GetComponent<Rigidbody>().velocity = (dir + pDir) * Random.Range(minSpeed, maxSpeed);
            if (timeToDestroy != 0)
            {
                Destroy(shotBullet, timeToDestroy);
            }
        }
        player.AddForce(-transform.right * knockback * knockbackModifier, ForceMode.Impulse);

        shootCd = 0;
        shooting = false;
    }

    void PlaySound(AudioSource source)
    {
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(source.clip);
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlash.SetActive(false);
        muzzleFlash.transform.localScale = Vector3.one / 100;
        num = 0;
        sin = 0;

        foreach (SpriteRenderer sr in muzzleRenderers)
        {
            sr.color = muzzleGradient.Evaluate(Random.Range(0f, 1f));
            sr.material.SetColor("_TintColor", muzzleGradient.Evaluate(Random.Range(0f, 1f)));
        }

        muzzleLight.intensity = Random.Range(1f, 25f);
        muzzleLight.color = muzzleGradient.Evaluate(Random.Range(0f, 1f));

        amplitude = Random.Range(minMuzzleScale, maxMuzzleScale);
        multiplier = Random.Range(minMuzzleFlashSpeed, maxMuzzleFlashSpeed);

        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(Random.Range(minMuzzleLifetime, maxMuzzleLifetime));

        muzzleFlash.SetActive(false);
        muzzleFlash.transform.localScale = Vector3.one / 100;
        num = 0;
        sin = 0;
    }
}
