using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string targetTag, notTargetTag;
    [SerializeField] GameObject hitEffect;
    public AudioSource impactSound;
    [SerializeField] float destroyTimer, minVolume, maxVolume, minPitch, maxPitch;
    float damage, knockback;
    Health hp;

    [Space]

    [Header("Explosive")]
    [SerializeField] bool explosive;
    [SerializeField] float explosionDelay, blastRadius;
    Rigidbody hitRb;

    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag(targetTag) || collision.gameObject.CompareTag("Pickup") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Block Support"))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, destroyTimer);

            if (explosive)
            {
                Invoke("Explode", explosionDelay);
                return;
            }
            else
            {
                if (collision.TryGetComponent(out hp))
                {
                    PlaySound(impactSound);
                    hp.TakeDmg(damage);
                    hp.hit = true;
                }

                if (collision.TryGetComponent(out hitRb))
                {
                    hitRb.AddForce(transform.right * knockback, ForceMode.Impulse);
                }
            }
            Destroy(gameObject);
        }
    }

    void PlaySound(AudioSource source)
    {
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(source.clip);
    }

    public void SetStats(float dmg, float force)
    {
        damage = dmg;
        knockback = force;
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out hitRb))
            {
                if (!hitRb.gameObject.CompareTag(notTargetTag) && !hitRb.gameObject.CompareTag(targetTag))
                {
                    hitRb.AddExplosionForce(knockback, transform.position, blastRadius, 1, ForceMode.Impulse);
                }
                else
                {
                    hitRb.velocity = Vector3.zero;
                }
            }

            if (col.TryGetComponent(out hp))
            {
                if (!hp.gameObject.CompareTag(notTargetTag))
                {
                    PlaySound(impactSound);
                    hp.TakeDmg(damage);
                    hp.hit = true;
                }
            }
        }
        Destroy(gameObject);
    }
}
