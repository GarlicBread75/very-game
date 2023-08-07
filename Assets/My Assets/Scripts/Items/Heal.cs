using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] float healAmount;
    [SerializeField] float duration;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player 1") || collision.gameObject.CompareTag("Player 2"))
        {
            collision.gameObject.GetComponent<Health>().Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
