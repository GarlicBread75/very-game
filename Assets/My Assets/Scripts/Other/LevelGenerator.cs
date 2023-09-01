using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Texture2D[] map;
    public ColourToPrefab[] mappings;
    GameObject thing;

    void Start()
    {   
        int num = Random.Range(0, map.Length);
        for (int x = 0; x < map[num].width; x++)
        {
            for (int y = 0; y < map[num].height; y++)
            {
                Color pixelColour = map[num].GetPixel(x, y);

                foreach (ColourToPrefab mapping in mappings)
                {
                    if (mapping.colour == pixelColour)
                    {
                        thing = Instantiate(mapping.objectToSpawn, new Vector3(-20 + x * 2, y * 2, 0), Quaternion.identity);
                        thing.AddComponent<DestroyBlock>();
                        thing.GetComponent<DestroyBlock>().timer = 5;

                        if (y == 0)
                        {
                            thing = Instantiate(mapping.objectToSpawn, new Vector3(-20 + x * 2, -2, 0), Quaternion.identity);
                            thing.tag = "Block Support";
                            thing.AddComponent<DestroyBlock>();
                            thing.GetComponent<DestroyBlock>().timer = 10;

                            thing = Instantiate(mapping.objectToSpawn, new Vector3(-20 + x * 2, -4, 0), Quaternion.identity);
                            thing.tag = "Block Support";
                        }

                        if (mapping.name == "Player 1" || mapping.name == "Player 2")
                        {
                            Destroy(mapping.objectToSpawn);
                        }
                    }
                }
            }
        }
        thing = null;
    }
}