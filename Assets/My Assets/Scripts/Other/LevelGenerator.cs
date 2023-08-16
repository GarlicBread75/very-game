using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Texture2D[] map;
    public ColourToPrefab[] mappings;

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
                        Instantiate(mapping.objectToSpawn, new Vector3(-20 + x * 2, y * 2, 0), Quaternion.identity);

                        if (mapping.name == "Player 1" || mapping.name == "Player 2")
                        {
                            Destroy(mapping.objectToSpawn);
                        }
                    }
                }
            }
        }
    }
}