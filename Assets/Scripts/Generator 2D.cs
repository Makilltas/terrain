using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class Generator2D : MonoBehaviour
{
    public int width = 50;
    public int lenght = 50;

    public float seed;

    [Range(0, 1)]
    public float groundLimit = 0.5f;

    public float scale = 100;

    public GameObject GroundPrefab;
    public GameObject HousePrefab;  
    

    private float[,] grid;

    void Start()
    {
        grid = new float[width, lenght];
        GenerateNoise();
        BuildWorld();
    }

    void GenerateNoise()
    {
        seed = Random.Range(-1000, 1000);
        for (int y = 0; y < lenght; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var px = (x + seed) / scale;
                var py = (y + seed) / scale;

                grid[x, y] = (noise.snoise(new float2(px, py)) + 1) / 2;
            }
        }
    }

    void BuildWorld()
    {
        for (int y = 0; y < lenght; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] >= groundLimit)
                {
                   
                    GameObject ground = Instantiate(GroundPrefab, new Vector3(x, 0, y), Quaternion.identity);

                    
                    ChangeColorBasedOnNoise(ground, grid[x, y]);

                    
                    if (Random.value < 0.05f)  
                    {
                        Instantiate(HousePrefab, new Vector3(x, 0, y), Quaternion.identity);
                    }
                }
            }
        }
    }

    void ChangeColorBasedOnNoise(GameObject ground, float noiseValue)
    {
        Renderer groundRenderer = ground.GetComponent<Renderer>();

        if (groundRenderer != null)
        {

            if (noiseValue < 0.6f)
            {
                groundRenderer.material.color = Color.yellow;
            }
            else if (noiseValue < 0.9f)
            {
                groundRenderer.material.color = Color.green;
            }
            else
            {
                groundRenderer.material.color = Color.gray;
            }
        }
    }
}
