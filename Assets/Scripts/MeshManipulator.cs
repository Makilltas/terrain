using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Terrain))]

public class MeshManipulator : MonoBehaviour
{
    public Vector3 size = new Vector3(100, 30, 100);
    public float seed;

    [Header("Noise Settings")]
    public float scale = 100;
    public int octaves = 4;


    private Terrain terrain;
    private TerrainData terrainData;
    private int heightMapResolution;
    private float[,] map;
    float[,,] alphaData;


    private const int SAND = 0;
    private const int STONE = 1;
    private const int GRASS = 2;


    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        heightMapResolution = terrainData.heightmapResolution;
        map = new float[heightMapResolution, heightMapResolution];

        GenerateTerrain();
        PaintTerrain();
    }

    void GenerateTerrain()
    {
        seed = Random.Range(-1000, 1000);

        for (int z = 0; z < heightMapResolution; z++)
        {
            for (int x = 0; x < heightMapResolution; x++)
            {

                for (int o = 0; o < octaves; o++)
                {
                    var px = (x + seed) / scale * Mathf.Pow(2, o);
                    var pz = (z + seed) / scale * Mathf.Pow(2, o);

                    var sign = o % 2 == 0 ? 1 : -1;

                    var noiseValue = (noise.snoise(new float2(px, pz)) + 1) / 2 / Mathf.Pow(2, o) * sign;

                    map[x, z] += noiseValue;
                }


            }
        }

        terrainData.SetHeights(0, 0, map);
        terrain.Flush();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateTerrain();
            PaintTerrain();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearMap();
        }
        
    }

    void ClearMap()
    {
        for (int z = 0; z < heightMapResolution; z++)
        {
            for (int x = 0; x < heightMapResolution; x++)
            {
                map[x, z] = 0f;
            }
        }

        terrainData.SetHeights(0, 0, map);
        terrain.Flush();
    }

    void PaintTerrain()
    {
        alphaData =terrainData.GetAlphamaps(0,0,terrainData.alphamapWidth,terrainData.alphamapHeight);

        var height = terrainData.alphamapHeight;
        var width = terrainData.alphamapWidth;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                alphaData[x, y, SAND] = 0;
                alphaData[x, y, GRASS] = 0;
                alphaData[x, y, STONE] = 0;

                if (map[x, y] < 0.4f)
                {
                    alphaData[x, y, SAND] = 1;
                }
                else if (map[x, y] < 0.6f)
                {
                    alphaData[x, y, GRASS] = 1;
                }
                else
                {
                    alphaData[x, y , STONE] = 1;
                }

            }
        }
        terrainData.SetAlphamaps(0, 0, alphaData);
        terrain.Flush();
    }


}
