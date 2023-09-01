using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    public float timer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Void"))
        {
            Destroy(gameObject, timer);
        }
    }
}
