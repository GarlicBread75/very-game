using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] string targetTag;
    [SerializeField] GameObject hitEffect;
    [HideInInspector] public float damage, knockback;
    Health hp;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            hp = collision.GetComponent<Health>();
            hp.TakeDmg(damage);
            hp.hit = true;
            collision.GetComponent<Rigidbody>().AddForce(transform.right * knockback, ForceMode.Impulse);
        }
        else
        if (collision.gameObject.CompareTag("Pickup"))
        {
            collision.GetComponent<Rigidbody>().AddForce(transform.right * knockback, ForceMode.Impulse);
        }
        GameObject effect = Instantiate(hitEffect, transform.position,Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }
}
