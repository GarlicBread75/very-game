using System.Collections;
using UnityEngine;

public class MovementBoost : MonoBehaviour
{
    [SerializeField] float speedModifier, jumpModifier, dashPowerModifier, duration;

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
            GameObject.Find("Speed Outline 1").GetComponent<MeshRenderer>().enabled = true;
        }
        else
        if (num == 2)
        {
            GameObject.Find("Speed Outline 2").GetComponent<MeshRenderer>().enabled = true;
        }
        player.speedModifier = speedModifier;
        player.jumpModifier = jumpModifier;
        player.dashPowerModifier = dashPowerModifier;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(duration);
        if (num == 1)
        {
            GameObject.Find("Speed Outline 1").GetComponent<MeshRenderer>().enabled = false;
        }
        else
        if (num == 2)
        {
            GameObject.Find("Speed Outline 2").GetComponent<MeshRenderer>().enabled = false;
        }
        player.speedModifier = 1;
        player.jumpModifier = 1;
        player.dashPowerModifier = 1;
        Destroy(gameObject);
    }
}
