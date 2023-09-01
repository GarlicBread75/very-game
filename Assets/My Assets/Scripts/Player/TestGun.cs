using System.Collections;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    #region Variables

    [Header("Gun")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject shootExplosion;
    [SerializeField] int bulletCount;
    [SerializeField] float spread;
    public float fireRate;
    bool shooting;
    float shootCd;

    [Space]

    [Header("Bullet")]
    [SerializeField] GameObject bullet;
    [SerializeField] float minSpeed, maxSpeed, timeToDestroy;

    [Space]

    [Header("Muzzle Flash")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Gradient muzzleGradient;
    [SerializeField] SpriteRenderer[] muzzleRenderers;
    [SerializeField] Light muzzleLight;
    [SerializeField] float minMuzzleScale, maxMuzzleScale, minMuzzleFlashSpeed, maxMuzzleFlashSpeed, minMuzzleLifetime, maxMuzzleLifetime;
    float num, amplitude, multiplier, sin;
    #endregion

    void FixedUpdate()
    {
        if (shootCd < fireRate)
        {
            shootCd += Time.fixedDeltaTime;
        }
        else
        if (!shooting)
        {
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
        GameObject effect = Instantiate(shootExplosion, firePoint.position, Quaternion.identity);
        Destroy(effect, 2.1f);
        StartCoroutine(MuzzleFlash());
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject shotBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            Vector2 dir = transform.rotation * Vector2.right;
            Vector2 pDir = Vector2.Perpendicular(dir) * Random.Range(-spread, spread);
            shotBullet.GetComponent<Rigidbody>().velocity = (dir + pDir) * Random.Range(minSpeed, maxSpeed);
            if (timeToDestroy != 0)
            {
                Destroy(shotBullet, timeToDestroy);
            }
        }

        shootCd = 0;
        shooting = false;
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
