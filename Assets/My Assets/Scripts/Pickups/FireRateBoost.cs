using System.Collections;
using UnityEngine;

public class FireRateBoost : MonoBehaviour
{
    [SerializeField] float duration, fireRateModifier;
    [SerializeField] Material outlineMaterial;
    [SerializeField] Vector3 particlesOffset;
    MeshRenderer thisRend, rend1, rend2;
    CapsuleCollider col;
    GameObject particles;
    ParticleSystem particlesSystem;
    ParticleSystem.EmissionModule em;
    Gun gun1, gun2;

    void Start()
    {
        rend1 = GameObject.Find("Outline 1").GetComponent<MeshRenderer>();
        rend2 = GameObject.Find("Outline 2").GetComponent<MeshRenderer>();
        gun1 = GameObject.Find("Gun 1").GetComponent<Gun>();
        gun2 = GameObject.Find("Gun 2").GetComponent<Gun>();
        thisRend = GetComponent<MeshRenderer>();
        col = GetComponent<CapsuleCollider>();
    }

    void Awake()
    {
        particles = GameObject.Find("Fire Rate Aura");
        particlesSystem = particles.GetComponent<ParticleSystem>();
        var emission = particlesSystem.emission;
        emission.enabled = true;
        for (int i = 0; i < particles.transform.childCount; i++)
        {
            em = particles.transform.GetChild(i).GetComponentInChildren<ParticleSystem>().emission;
            em.enabled = true;
        }
        particlesSystem.Play();
    }

    void Update()
    {
        particles.transform.position = transform.position + particlesOffset;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player 1"))
        {
            StartCoroutine(BoostFireRate(gun1, 1));
        }
        else
        if (collision.gameObject.CompareTag("Player 2"))
        {
            StartCoroutine(BoostFireRate(gun2, 2));
        }
    }

    IEnumerator BoostFireRate(Gun gun, int num)
    {
        if (num == 1)
        {
            rend1.material = outlineMaterial;
            rend1.enabled = true;
        }
        else
        if (num == 2)
        {
            rend2.material = outlineMaterial;
            rend2.enabled = true;
        }
        gun.slider.maxValue = gun.fireRate / fireRateModifier;
        gun.gr = gun.gradient2;
        gun.fireRateModifier = fireRateModifier;
        thisRend.enabled = false;
        var emission = particlesSystem.emission;
        emission.enabled = false;
        for (int i = 0; i < particles.transform.childCount; i++)
        {
            em = particles.transform.GetChild(i).GetComponentInChildren<ParticleSystem>().emission;
            em.enabled = false;
        }
        particlesSystem.Stop();
        particles.transform.position = new Vector3(0, -20, 0);
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
        gun.slider.maxValue = gun.fireRate;
        gun.gr = gun.gradient1;
        gun.fireRateModifier = 1;
        Destroy(gameObject);
    }
}