using System.Collections;
using UnityEngine;

public class DamageBoost : MonoBehaviour
{
    [SerializeField] float duration, damageModifier;
    Gun gun1, gun2;

    private void Awake()
    {
        gun1 = GameObject.Find("Gun 1").GetComponent<Gun>();
        gun2 = GameObject.Find("Gun 2").GetComponent<Gun>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player 1"))
        {
            StartCoroutine(BoostDamage(gun1, 1));
        }
        else
        if (collision.gameObject.CompareTag("Player 2"))
        {
            StartCoroutine(BoostDamage(gun2, 2));
        }
    }

    IEnumerator BoostDamage(Gun gun, int num)
    {
        if (num == 1)
        {
            GameObject.Find("Damage Outline 1").GetComponent<MeshRenderer>().enabled = true;
        }
        else
        if (num == 2)
        {
            GameObject.Find("Damage Outline 2").GetComponent<MeshRenderer>().enabled = true;
        }
        gun.damageModifier = damageModifier;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(duration);
        if (num == 1)
        {
            GameObject.Find("Damage Outline 1").GetComponent<MeshRenderer>().enabled = false;
        }
        else
        if (num == 2)
        {
            GameObject.Find("Damage Outline 2").GetComponent<MeshRenderer>().enabled = false;
        }
        gun.damageModifier = 1;
        Destroy(gameObject);
    }
}