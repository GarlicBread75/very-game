using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] pickups;
    [SerializeField] Vector3 spawnPos;
    [SerializeField] float minSpawnDelay, maxSpawnDelay;
    public AudioSource healSound, buffSound;
    [HideInInspector] public bool damageActive1, fireRateActive1, knockbackActive1, movementActive1, damageActive2, fireRateActive2, knockbackActive2, movementActive2;
    float cd;

    [Space]

    public Health hp1;
    public Health hp2;
    public PlayerMovement pm1, pm2;
    public GameObject p1, p2, angry1, angry2;
    public MeshRenderer o1, o2;
    public Gun gun1, gun2;

    void Awake()
    {
        cd = Random.Range(minSpawnDelay, maxSpawnDelay);
        Invoke("GetPlayerComponents", 0.5f);
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

    void GetPlayerComponents()
    {
        p1 = GameObject.Find("Body 1");
        p2 = GameObject.Find("Body 2");
        hp1 = p1.GetComponent<Health>();
        hp2 = p2.GetComponent<Health>();
        pm1 = p1.GetComponent<PlayerMovement>();
        pm2 = p2.GetComponent<PlayerMovement>();

        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("Gun 1"))
        {
            if (thing.activeInHierarchy)
            {
                gun1 = thing.GetComponent<Gun>();
            }
        }

        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("Gun 2"))
        {
            if (thing.activeInHierarchy)
            {
                gun2 = thing.GetComponent<Gun>();
            }
        }

        o1 = GameObject.Find("Outline 1").GetComponent<MeshRenderer>();
        o2 = GameObject.Find("Outline 2").GetComponent<MeshRenderer>();
        angry1 = GameObject.Find("Angry 1");
        angry1.GetComponent<MeshRenderer>().enabled = true;
        angry1.SetActive(false);
        angry2 = GameObject.Find("Angry 2");
        angry2.GetComponent<MeshRenderer>().enabled = true;
        angry2.SetActive(false);
    }
}