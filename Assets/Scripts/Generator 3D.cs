using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class Generator3D : MonoBehaviour
{
    [Header("World Settings")]
    public int sizeX = 50;
    public int sizeY = 50;
    public int sizeZ = 50;
    public float seed;
    [Range(0, 1)] public float groundLimit = 0.5f;

    [Header("Noise Settings")]
    public float scale = 25;

    [Header("Tiles")]
    public GameObject groundTile;

    private float[,,] grid;

    void Start()
    {
        grid = new float[sizeX, sizeY, sizeZ];
        GenerateNoise();
        BuildWorld();
    }

    void GenerateNoise()
    {
        seed = Random.Range(-1000, 1000);
        for (int y = 0; y < sizeY; y++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    var px = (x + seed) / scale;
                    var py = (y + seed) / scale;
                    var pz = (z + seed) / scale;
                    grid[x, y, z] = (noise.snoise(new float3(px, py, pz)) + 1) / 2;
                }
            }
        }
    }

    void BuildWorld()
    {
        for (int y = 0; y < sizeY; y++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (grid[x, y, z] >= groundLimit && !IsCompletelySurrounded(x, y, z))
                    {
                        Instantiate(groundTile, new Vector3(x, y, z), Quaternion.identity);
                    }
                }
            }
        }
    }

    bool IsCompletelySurrounded(int x, int y, int z)
    {
        int[][] directions = new int[][]
        {
            new int[] {1, 0, 0}, new int[] {-1, 0, 0}, 
            new int[] {0, 1, 0}, new int[] {0, -1, 0}, 
            new int[] {0, 0, 1}, new int[] {0, 0, -1}   
        };

        foreach (var dir in directions)
        {
            int nx = x + dir[0];
            int ny = y + dir[1];
            int nz = z + dir[2];

            if (nx < 0 || nx >= sizeX || ny < 0 || ny >= sizeY || nz < 0 || nz >= sizeZ || grid[nx, ny, nz] < groundLimit)
            {
                return false; 
            }
        }
        return true; 
    }
}
