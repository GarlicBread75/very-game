using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Texture2D map;
    public ColourToPrefab[] mappings;

    void Start()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                Color pixelColour = map.GetPixel(x, y);

                foreach (ColourToPrefab mapping in mappings)
                {
                    if (mapping.colour == pixelColour)
                    {
                        Instantiate(mapping.objectToSpawn, new Vector3(-19 + x * 2, 1.5f + y * 2, 0), Quaternion.identity);

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