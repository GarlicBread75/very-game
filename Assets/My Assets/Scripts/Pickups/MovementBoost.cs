using System.Collections;
using UnityEngine;

public class MovementBoost : MonoBehaviour
{
    [SerializeField] float speedModifier, jumpModifier, dashPowerModifier, duration;
    [SerializeField] Material outlineMaterial;
    [SerializeField] Vector3 particlesOffset;
    MeshRenderer thisRend, rend1, rend2;
    CapsuleCollider col;
    GameObject particles;
    ParticleSystem particlesSystem;
    ParticleSystem.EmissionModule em;

    void Start()
    {
        rend1 = GameObject.Find("Outline 1").GetComponent<MeshRenderer>();
        rend2 = GameObject.Find("Outline 2").GetComponent<MeshRenderer>();
        thisRend = GetComponent<MeshRenderer>();
        col = GetComponent<CapsuleCollider>();
    }

    void Awake()
    {
        particles = GameObject.Find("Movement Aura");
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
            StartCoroutine(BoostSpeed(collision.gameObject.GetComponent<PlayerMovement>(), 1));
        }
        else
        if (collision.gameObject.CompareTag("Player 2"))
        {
            StartCoroutine(BoostSpeed(collision.gameObject.GetComponent<PlayerMovement>(), 2));
        }
    }

    IEnumerator BoostSpeed(PlayerMovement player, int num)
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
        player.speedModifier = speedModifier;
        player.jumpModifier = jumpModifier;
        player.dashPowerModifier = dashPowerModifier;
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
        player.speedModifier = 1;
        player.jumpModifier = 1;
        player.dashPowerModifier = 1;
        Destroy(gameObject);
    }
}