using UnityEngine;

public class GunHolder : MonoBehaviour
{
    [SerializeField] GameObject[] guns;
    [HideInInspector] public Transform gunTransform;

    void Awake()
    {
        foreach (GameObject gun in guns)
        {
            if (gun.activeInHierarchy)
            {
                gun.SetActive(false);
            }
        }

        int num = Random.Range(0, guns.Length);
        gunTransform = guns[num].transform;
        guns[num].SetActive(true);
    }
}
