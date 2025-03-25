using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    [Header("World Settings")]
    public int sizeX = 50;
    public int sizeY = 20;
    public int sizeZ = 50;
    public float seed;

    [Header("Noise Settings")]
    public float scale = 25;

    [Header("Tiles")]
    public GameObject groundTile;

    private float[,] grid;

    void Start()
    {
        grid = new float[sizeX, sizeZ];
        GenerateNoise();
        BuildWorld();
    }

    void GenerateNoise()
    {
        seed = Random.Range(-1000, 1000);

        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var px = (x + seed) / scale;
                var pz = (z + seed) / scale;

                grid[x, z] = (noise.snoise(new float2(px, pz)) + 1) / 2;
            }
        }
    }

    void BuildWorld()
    {
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var height = grid[x, z] * sizeY;
                Instantiate(groundTile, new Vector3(x, height, z), Quaternion.identity);
            }
        }
    }
}