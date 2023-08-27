using UnityEngine;

public class RisingLava : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float delay;

    void FixedUpdate()
    {
        if (delay > 0)
        {
            delay -= Time.fixedDeltaTime;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 50, 0), speed * Time.deltaTime);
        }
    }
}
