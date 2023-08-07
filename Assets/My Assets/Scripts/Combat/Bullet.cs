using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float damage;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] string targetTag;
    [HideInInspector] public float knockback;
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
        Destroy(gameObject);
    }
}
