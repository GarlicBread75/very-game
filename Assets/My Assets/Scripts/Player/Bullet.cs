using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string targetTag;
    [SerializeField] GameObject hitEffect;
    public AudioSource impactSound;
    [SerializeField] float minVolume, maxVolume, minPitch, maxPitch;
    float damage, knockback;
    Health hp;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            PlaySound(impactSound);
            hp = collision.GetComponent<Health>();
            hp.TakeDmg(damage);
            hp.hit = true;
            collision.GetComponent<Rigidbody>().AddForce(transform.right * knockback, ForceMode.Impulse);
        }
        else
        if (collision.gameObject.CompareTag("Pickup") || collision.gameObject.CompareTag("Ground"))
        {
            collision.GetComponent<Rigidbody>().AddForce(transform.right * knockback, ForceMode.Impulse);
        }
        GameObject effect = Instantiate(hitEffect, transform.position,Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
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
}
