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
    MeshRenderer thisRend, rend1, rend2;
    CapsuleCollider col;
    GameObject particles;
    ParticleSystem particlesSystem;
    ParticleSystem.EmissionModule em;
    Gun gun1, gun2;
    int pickupNum;
    Rigidbody rb;
    #endregion

    void Start()
    {
        rend1 = GameObject.Find("Outline 1").GetComponent<MeshRenderer>();
        rend2 = GameObject.Find("Outline 2").GetComponent<MeshRenderer>();
        gun1 = GameObject.Find("Gun 1").GetComponent<Gun>();
        gun2 = GameObject.Find("Gun 2").GetComponent<Gun>();
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
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
                StartCoroutine(Buff(gun1, collision.gameObject.GetComponent<Health>(), 1));
            }
            else
            if (pickupNum == 3)
            {
                StartCoroutine(Buff(collision.gameObject.GetComponent<PlayerMovement>(), 1));
            }
            else
            if (pickupNum == 4)
            {
                StartCoroutine(Buff(collision.gameObject.GetComponent<Health>(), healAmount));
            }
        }
        else
        if (collision.gameObject.CompareTag("Player 2"))
        {
            if (pickupNum >= 0 && pickupNum <= 2)
            {
                StartCoroutine(Buff(gun2, collision.gameObject.GetComponent<Health>(), 2));
            }
            else
            if (pickupNum == 3)
            {
                StartCoroutine(Buff(collision.gameObject.GetComponent<PlayerMovement>(), 2));
            }
            else
            if (pickupNum == 4)
            {
                StartCoroutine(Buff(collision.gameObject.GetComponent<Health>(), healAmount));
            }
        }
    }

    IEnumerator Buff(Gun gun, Health hp, int num)
    {
        if (num == 1)
        {
            rend1.material = outlineMaterials[pickupNum];
            rend1.enabled = true;
        }
        else
        if (num == 2)
        {
            rend2.material = outlineMaterials[pickupNum];
            rend2.enabled = true;
        }

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
            rend1.enabled = false;
        }
        else
        if (num == 2)
        {
            rend2.enabled = false;
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

    IEnumerator Buff(PlayerMovement player, int num)
    {
        if (num == 1)
        {
            rend1.material = outlineMaterials[pickupNum];
            rend1.enabled = true;
        }
        else
        if (num == 2)
        {
            rend2.material = outlineMaterials[pickupNum];
            rend2.enabled = true;
        }

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
            rend1.enabled = false;
        }
        else
        if (num == 2)
        {
            rend2.enabled = false;
        }

        player.speedModifier = 1;
        player.jumpModifier = 1;
        player.dashPowerModifier = 1;
        Destroy(gameObject);
    }

    IEnumerator Buff(Health hp, float heal)
    {
        hp.Heal(heal);

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