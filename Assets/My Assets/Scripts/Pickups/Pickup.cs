using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    #region Variables
    [SerializeField] string[] pickupNames;
    [SerializeField] Material[] materials;
    [SerializeField] Material[] outlineMaterials;
    [SerializeField] GameObject[] effect;

    [Space]

    [SerializeField] float duration;
    [SerializeField] float damageModifier, fireRateModifier, knockbackModifier, bulletKnockbackModifier;

    [Space]

    [SerializeField] float speedModifier;
    [SerializeField] float jumpModifier, dashPowerModifier;

    [Space]

    [SerializeField] float healAmount;

    [Space]

    [SerializeField] Vector3 particlesOffset;
    [SerializeField] float maxFallSpeed;
    [SerializeField] AudioSource healSound, buffSound;
    [SerializeField] float minVolume, maxVolume, minPitch, maxPitch;
    MeshRenderer thisRend, buffOutline1, buffOutline2;
    CapsuleCollider col;
    PickupSpawner spawner;
    GameObject particles;
    ParticleSystem particlesSystem;
    ParticleSystem.EmissionModule em;
    Gun gun1, gun2;
    int pickupNum;
    Rigidbody rb;
    Events thing;
    #endregion

    void Start()
    {
        buffOutline1 = GameObject.Find("Outline 1").GetComponent<MeshRenderer>();
        buffOutline2 = GameObject.Find("Outline 2").GetComponent<MeshRenderer>();
        gun1 = GameObject.Find("Gun 1").GetComponent<Gun>();
        gun2 = GameObject.Find("Gun 2").GetComponent<Gun>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        spawner = GameObject.Find("Pickup Spawner").GetComponent<PickupSpawner>();
        healSound = spawner.healSound;
        buffSound = spawner.buffSound;
        thisRend = GetComponent<MeshRenderer>();
        pickupNum = Random.Range(0, pickupNames.Length);
        thisRend.material = materials[pickupNum];
        particles = Instantiate(effect[pickupNum], transform.position, Quaternion.identity);
        particlesSystem = particles.GetComponent<ParticleSystem>();
        em = particlesSystem.emission;
        em.enabled = true;

        for (int i = 0; i < particles.transform.childCount; i++)
        {
            em = particles.transform.GetChild(i).GetComponentInChildren<ParticleSystem>().emission;
            em.enabled = true;
        }

        particlesSystem.Play();

        if (pickupNum == 5)
        {
            particlesOffset.y += 1;
        }
    }

    void Update()
    {
        if (particles != null)
        {
            particles.transform.position = transform.position + particlesOffset;
        }
    }

    void FixedUpdate()
    {
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxFallSpeed, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player 1"))
        {
            if (pickupNum >= 0 && pickupNum <= 2)
            {
                StartCoroutine(GunBuff(gun1, collision.gameObject.GetComponent<Health>(), 1));
            }
            else
            if (pickupNum == 3)
            {
                StartCoroutine(MovementBuff(collision.gameObject.GetComponent<PlayerMovement>(), 1));
            }
            else
            if (pickupNum == 4)
            {
                StartCoroutine(Heal(collision.gameObject.GetComponent<Health>()));
            }
        }
        else
        if (collision.gameObject.CompareTag("Player 2"))
        {
            if (pickupNum >= 0 && pickupNum <= 2)
            {
                StartCoroutine(GunBuff(gun2, collision.gameObject.GetComponent<Health>(), 2));
            }
            else
            if (pickupNum == 3)
            {
                StartCoroutine(MovementBuff(collision.gameObject.GetComponent<PlayerMovement>(), 2));
            }
            else
            if (pickupNum == 4)
            {
                StartCoroutine(Heal(collision.gameObject.GetComponent<Health>()));
            }
        }
    }

    void PlaySound(AudioSource source)
    {
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(source.clip);
    }

    IEnumerator GunBuff(Gun gun, Health hp, int num)
    {
        if (num == 1)
        {
            buffOutline1.material = outlineMaterials[pickupNum];
            buffOutline1.enabled = true;
        }
        else
        if (num == 2)
        {
            buffOutline2.material = outlineMaterials[pickupNum];
            buffOutline2.enabled = true;
        }
        PlaySound(buffSound);
        switch (pickupNum)
        {
            case 0:
                gun.damageModifier = damageModifier;
                hp.ToggleAngry(true);
                break;

            case 1:
                gun.slider.maxValue = gun.fireRate / fireRateModifier;
                gun.gr = gun.gradient2;
                gun.fireRateModifier = fireRateModifier;
                break;

            case 2:
                gun.knockbackModifier = knockbackModifier;
                gun.bulletKnockbackModifier = bulletKnockbackModifier;
                break;
        }
        thisRend.enabled = false;
        em = particlesSystem.emission;
        em.enabled = false;
        for (int i = 0; i < particles.transform.childCount; i++)
        {
            em = particles.transform.GetChild(i).GetComponentInChildren<ParticleSystem>().emission;
            em.enabled = false;
        }
        particlesSystem.Stop();
        Destroy(particles);
        particlesSystem = null;
        col.enabled = false;
        yield return new WaitForSeconds(duration);
        if (num == 1)
        {
            buffOutline1.enabled = false;
        }
        else
        if (num == 2)
        {
            buffOutline2.enabled = false;
        }
        switch (pickupNum)
        {
            case 0:
                gun.damageModifier = 1;
                hp.ToggleAngry(false);
                break;

            case 1:
                gun.slider.maxValue = gun.fireRate;
                gun.gr = gun.gradient1;
                gun.fireRateModifier = 1;
                break;

            case 2:
                gun.knockbackModifier = 1;
                gun.bulletKnockbackModifier = 1;
                break;
        }
        Destroy(gameObject);
    }

    IEnumerator MovementBuff(PlayerMovement player, int num)
    {
        if (num == 1)
        {
            buffOutline1.material = outlineMaterials[pickupNum];
            buffOutline1.enabled = true;
        }
        else
        if (num == 2)
        {
            buffOutline2.material = outlineMaterials[pickupNum];
            buffOutline2.enabled = true;
        }
        PlaySound(buffSound);
        player.speedModifier = speedModifier;
        player.jumpModifier = jumpModifier;
        player.dashPowerModifier = dashPowerModifier;
        thisRend.enabled = false;
        em = particlesSystem.emission;
        em.enabled = false;
        for (int i = 0; i < particles.transform.childCount; i++)
        {
            em = particles.transform.GetChild(i).GetComponentInChildren<ParticleSystem>().emission;
            em.enabled = false;
        }
        particlesSystem.Stop();
        Destroy(particles);
        particlesSystem = null;
        col.enabled = false;
        yield return new WaitForSeconds(duration);
        if (num == 1)
        {
            buffOutline1.enabled = false;
        }
        else
        if (num == 2)
        {
            buffOutline2.enabled = false;
        }
        player.speedModifier = 1;
        player.jumpModifier = 1;
        player.dashPowerModifier = 1;
        Destroy(gameObject);
    }

    IEnumerator Heal(Health hp)
    {
        PlaySound(healSound);
        hp.Heal(healAmount);
        thisRend.enabled = false;
        em = particlesSystem.emission;
        em.enabled = false;
        for (int i = 0; i < particles.transform.childCount; i++)
        {
            em = particles.transform.GetChild(i).GetComponentInChildren<ParticleSystem>().emission;
            em.enabled = false;
        }
        particlesSystem.Stop();
        Destroy(particles);
        particlesSystem = null;
        col.enabled = false;
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}