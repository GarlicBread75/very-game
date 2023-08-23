using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    #region Variables
    [SerializeField] string[] pickupNames;
    [SerializeField] Material[] materials;
    [SerializeField] Material[] outlineColours;
    [SerializeField] GameObject[] effect;
    [SerializeField] GameObject[] dissapearEffect;

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
    GameObject particles, angry1, angry2;
    ParticleSystem.EmissionModule em;
    ParticleSystem particlesSystem;
    PickupSpawner spawner;
    CapsuleCollider col;
    MeshRenderer rend, o1, o2;
    Gun gun1, gun2;
    int pickupNum;
    Rigidbody rb;
    Health hp1, hp2;
    public PlayerMovement pm1, pm2;
    #endregion

    void Awake()
    {
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        pickupNum = Random.Range(0, pickupNames.Length);
        spawner = GameObject.Find("Pickup Spawner").GetComponent<PickupSpawner>();
        o1 = spawner.o1;
        o2 = spawner.o2;
        hp1 = spawner.hp1;
        hp2 = spawner.hp2;
        if (pickupNum >= 0 && pickupNum <= 2)
        {
            buffSound = spawner.buffSound;
            gun1 = spawner.gun1;
            gun2 = spawner.gun2;
            angry1 = spawner.angry1;
            angry2 = spawner.angry2;
        }
        else
        if (pickupNum == 3)
        {
            buffSound = spawner.buffSound;
            pm1 = spawner.pm1;
            pm2 = spawner.pm2;
        }
        else
        if (pickupNum == 4)
        {
            healSound = spawner.healSound;
        }
        rend = GetComponent<MeshRenderer>();
        rend.material = materials[pickupNum];
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
        if (collision.gameObject.CompareTag("Void"))
        {
            Invoke("Dissapear", 5);
            return;
        }

        if (collision.gameObject.CompareTag("Player 1"))
        {
            if (hp1.dead)
            {
                return;
            }

            if (pickupNum >= 0 && pickupNum <= 2)
            {
                StartCoroutine(GunBuff(gun1, 1));
            }
            else
            if (pickupNum == 3)
            {
                StartCoroutine(MovementBuff(pm1, 1));
            }
            else
            if (pickupNum == 4)
            {
                StartCoroutine(Heal(hp1));
            }
        }
        else
        if (collision.gameObject.CompareTag("Player 2"))
        {
            if (hp2.dead)
            {
                return;
            }

            if (pickupNum >= 0 && pickupNum <= 2)
            {
                StartCoroutine(GunBuff(gun2, 2));
            }
            else
            if (pickupNum == 3)
            {
                StartCoroutine(MovementBuff(pm2, 2));
            }
            else
            if (pickupNum == 4)
            {
                StartCoroutine(Heal(hp2));
            }
        }
    }

    void PlaySound(AudioSource source)
    {
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(source.clip);
    }

    void Dissapear()
    {
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
        Destroy(gameObject);
    }

    IEnumerator GunBuff(Gun gun, int num)
    {
        if (num == 1)
        {
            o1.material = outlineColours[pickupNum];
            o1.enabled = true;
        }
        else
        if (num == 2)
        {
            o2.material = outlineColours[pickupNum];
            o2.enabled = true;
        }
        PlaySound(buffSound);
        switch (pickupNum)
        {
            case 0:
                if (num == 1)
                {
                    spawner.damageActive1 = true;
                    angry1.SetActive(true);
                }
                else
                if (num == 2)
                {
                    spawner.damageActive2 = true;
                    angry2.SetActive(true);
                }
                gun.damageModifier = damageModifier;
                break;

            case 1:
                if (num == 1)
                {
                    spawner.fireRateActive1 = true;
                }
                else
                if (num == 2)
                {
                    spawner.fireRateActive2 = true;
                }
                gun.slider.maxValue = gun.fireRate / fireRateModifier;
                gun.gr = gun.gradient2;
                gun.fireRateModifier = fireRateModifier;
                break;

            case 2:
                if (num == 1)
                {
                    spawner.knockbackActive1 = true;
                }
                else
                if (num == 2)
                {
                    spawner.knockbackActive2 = true;
                }
                gun.knockbackModifier = knockbackModifier;
                gun.bulletKnockbackModifier = bulletKnockbackModifier;
                break;
        }
        rend.enabled = false;
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
        GameObject thing = Instantiate(dissapearEffect[pickupNum], transform.position, Quaternion.identity);
        Destroy(thing, 1);
        yield return new WaitForSeconds(duration);
        switch (pickupNum)
        {
            case 0:
                gun.damageModifier = 1;
                if (num == 1)
                {
                    spawner.damageActive1 = false;
                    angry1.SetActive(false);
                    if (!spawner.movementActive1 && !spawner.fireRateActive1 && !spawner.knockbackActive1)
                    {
                        o1.enabled = false;
                    }
                }
                else
                if (num == 2)
                {
                    spawner.damageActive2 = false;
                    angry2.SetActive(false);
                    if (!spawner.movementActive2 && !spawner.fireRateActive2 && !spawner.knockbackActive2)
                    {
                        o2.enabled = false;
                    }
                }
                break;

            case 1:
                gun.slider.maxValue = gun.fireRate;
                gun.gr = gun.gradient1;
                gun.fireRateModifier = 1;
                if (num == 1)
                {
                    spawner.fireRateActive1 = false;
                    if (!spawner.movementActive1 && !spawner.damageActive1 && !spawner.knockbackActive1)
                    {
                        o1.enabled = false;
                    }
                }
                else
                if (num == 2)
                {
                    spawner.fireRateActive2 = false;
                    if (!spawner.movementActive2 && !spawner.damageActive2 && !spawner.knockbackActive2)
                    {
                        o2.enabled = false;
                    }
                }
                break;

            case 2:
                gun.knockbackModifier = 1;
                gun.bulletKnockbackModifier = 1;
                if (num == 1)
                {
                    spawner.knockbackActive1 = false;
                    if (!spawner.movementActive1 && !spawner.damageActive1 && !spawner.fireRateActive1)
                    {
                        o1.enabled = false;
                    }
                }
                else
                if (num == 2)
                {
                    spawner.knockbackActive2 = false;
                    if (!spawner.movementActive2 && !spawner.damageActive2 && !spawner.fireRateActive2)
                    {
                        o2.enabled = false;
                    }
                }
                break;
        }
        Destroy(gameObject);
    }

    IEnumerator MovementBuff(PlayerMovement player, int num)
    {
        if (num == 1)
        {
            spawner.movementActive1 = true;
            o1.material = outlineColours[pickupNum];
            o1.enabled = true;
        }
        else
        if (num == 2)
        {
            spawner.movementActive2 = true;
            o2.material = outlineColours[pickupNum];
            o2.enabled = true;
        }

        PlaySound(buffSound);
        player.speedModifier = speedModifier;
        player.jumpModifier = jumpModifier;
        player.dashPowerModifier = dashPowerModifier;
        rend.enabled = false;
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
        GameObject thing = Instantiate(dissapearEffect[pickupNum], transform.position, Quaternion.identity);
        Destroy(thing, 1);
        yield return new WaitForSeconds(duration);
        if (num == 1)
        {
            spawner.movementActive1 = false;
            if (!spawner.damageActive1 && !spawner.fireRateActive1 && !spawner.knockbackActive1)
            {
                o1.enabled = false;
            }
        }
        else
        if (num == 2)
        {
            spawner.movementActive2 = false;
            if (!spawner.damageActive2 && !spawner.fireRateActive2 && !spawner.knockbackActive2)
            {
                o2.enabled = false;
            }
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
        rend.enabled = false;
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
        GameObject thing = Instantiate(dissapearEffect[pickupNum], transform.position, Quaternion.identity);
        Destroy(thing, 1);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}