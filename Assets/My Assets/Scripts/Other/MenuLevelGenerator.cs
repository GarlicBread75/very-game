using UnityEngine;

public class MenuLevelGenerator : MonoBehaviour
{
    [SerializeField] Texture2D[] map;
    [SerializeField] Color colour;
    [SerializeField] GameObject[] blocks;

    void Start()
    {   
        int num = Random.Range(0, map.Length);
        for (int x = 0; x < map[num].width; x++)
        {
            for (int y = 0; y < map[num].height; y++)
            {
                if (map[num].GetPixel(x, y) == colour)
                {
                    Instantiate(blocks[Random.Range(0, blocks.Length)], new Vector3(-20 + x * 2, y * 2, 0), Quaternion.identity);
                }
            }
        }
    }
}