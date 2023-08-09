using System.Collections;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] float healAmount, duration;
    [SerializeField] Vector3 particlesOffset;
    MeshRenderer thisRend;
    CapsuleCollider col;
    GameObject particles;
    ParticleSystem particlesSystem;
    ParticleSystem.EmissionModule em;

    void Start()
    {
        thisRend = GetComponent<MeshRenderer>();
        col = GetComponent<CapsuleCollider>();
    }

    void Awake()
    {
        particles = GameObject.Find("Heal Aura");
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
        if (collision.gameObject.CompareTag("Player 1") || collision.gameObject.CompareTag("Player 2"))
        {
            StartCoroutine(HealPlayer(collision.gameObject.GetComponent<Health>(), healAmount));
        }
    }

    IEnumerator HealPlayer(Health hp, float heal)
    {
        hp.Heal(heal);
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
        Destroy(gameObject);
    }
}
