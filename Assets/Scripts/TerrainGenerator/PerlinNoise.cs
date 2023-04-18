using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;

namespace TerrainGenerator
{
    public static class PerlinNoise
    {

        public static float[,] GenerateNoiseMap(int terrainMapWidth, int terrainMapHeight, float scale, int octaves, float persistance, float lacunarity)
        {
            float[,] noiseMap = new float[terrainMapWidth, terrainMapHeight];

            Vector2[] octaveOffsets = new Vector2[octaves];

            for (int i = 0; i < octaves; i++)
            {
                float offsetX = Random.Range(-100000, 100000);
                float offsetY = Random.Range(-100000, 100000);
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            for (int y = 0; y < terrainMapHeight; y++)
            {
                for (int x = 0; x < terrainMapWidth; x++)
                {

                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - (terrainMapWidth / 2)) / scale * frequency + octaveOffsets[i].x;
                        float sampleY = (y - (terrainMapHeight / 2)) / scale * frequency + octaveOffsets[i].y;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    noiseMap[x, y] = noiseHeight;

                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }
                }
            }

            for (int y = 0; y < terrainMapHeight; y++)
            {
                for (int x = 0; x < terrainMapWidth; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }

            return noiseMap;
        }

    }
}