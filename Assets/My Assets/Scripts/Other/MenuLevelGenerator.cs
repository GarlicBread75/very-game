using UnityEngine;

public class MenuLevelGenerator : MonoBehaviour
{
    [SerializeField] Texture2D[] map;
    [SerializeField] Color colour;
    [SerializeField] GameObject[] blocks;
    [SerializeField] bool menu;

    void Start()
    {
        if (menu)
        {
            GenerateMenuLevel();
        }
        else
        {
            GenerateLevel();
        }
    }

    void GenerateLevel()
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

    void GenerateMenuLevel()
    {
        for (int x = 0; x < 21; x++)
        {
            for (int y = -2; y < 12; y++)
            {
                if (y <= 3)
                {
                    Instantiate(blocks[Random.Range(0, blocks.Length)], new Vector3(-20 + x * 2, y * 2, 0), Quaternion.identity);
                }
            }
        }
    }
}