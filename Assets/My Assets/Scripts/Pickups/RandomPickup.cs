using UnityEngine;

public class RandomPickup : MonoBehaviour
{
    [SerializeField] GameObject[] pickups;
    [SerializeField] Vector3 spawnPos;
    [SerializeField] float minSpawnDelay, maxSpawnDelay;
    float cd;

    void Awake()
    {
        cd = Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    void FixedUpdate()
    {
        if (cd > 0)
        {
            cd -= Time.fixedDeltaTime;
        }
        else
        {
            cd = Random.Range(minSpawnDelay, maxSpawnDelay);
            Instantiate(pickups[Random.Range(0, pickups.Length)], new Vector3(Random.Range(-spawnPos.x, spawnPos.x), spawnPos.y, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
        }
    }
}
