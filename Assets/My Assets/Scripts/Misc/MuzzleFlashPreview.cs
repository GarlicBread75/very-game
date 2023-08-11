using UnityEngine;

public class MuzzleFlashPreview : MonoBehaviour
{
    [SerializeField] GameObject[] muzzleFlashes;
    [SerializeField] float delay;
    [SerializeField] string prefabName;
    [SerializeField] Transform target;
    float cd;
    int i;

    void Start()
    {
        cd = delay;
    }

    void FixedUpdate()
    {
        if (cd > 0)
        {
            cd -= Time.fixedDeltaTime;
        }
        else
        {
            cd = delay;
            GameObject thing = Instantiate(muzzleFlashes[i], target.position, Quaternion.identity, target);
            prefabName = muzzleFlashes[i].name;
            Destroy(thing, delay);
            i++;
        }

        if (i >= muzzleFlashes.Length)
        {
            i = 0;
        }
    }
}
