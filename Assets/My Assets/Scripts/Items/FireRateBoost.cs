using System.Collections;
using UnityEngine;

public class FireRateBoost : MonoBehaviour
{
    [SerializeField] float duration, fireRateModifier;
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
            StartCoroutine(BoostFireRate(gun1, 1));
        }
        else
        if (collision.gameObject.CompareTag("Player 2"))
        {
            StartCoroutine(BoostFireRate(gun2, 2));
        }
    }

    IEnumerator BoostFireRate(Gun gun, int num)
    {
        if (num == 1)
        {
            GameObject.Find("Fire Rate Outline 1").GetComponent<MeshRenderer>().enabled = true;
        }
        else
        if (num == 2)
        {
            GameObject.Find("Fire Rate Outline 2").GetComponent<MeshRenderer>().enabled = true;
        }
        gun.gr = gun.gradient2;
        gun.fireRateModifier = fireRateModifier;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(duration);
        if (num == 1)
        {
            GameObject.Find("Fire Rate Outline 1").GetComponent<MeshRenderer>().enabled = false;
        }
        else
        if (num == 2)
        {
            GameObject.Find("Fire Rate Outline 2").GetComponent<MeshRenderer>().enabled = false;
        }
        gun.gr = gun.gradient1;
        gun.fireRateModifier = 1;
        Destroy(gameObject);
    }
}