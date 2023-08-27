using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [SerializeField] GameObject[] guns;

    void Awake()
    {
        guns[Random.Range(0, guns.Length)].SetActive(true);
    }
}
