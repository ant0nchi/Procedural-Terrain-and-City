using UnityEngine;
using CityGenerator;
using UnityEditor;

namespace TerrainGenerator
{
    public static class TerrainGenerator
    {
        public static float[,] GenerateTerrain(Terrain terrain, int maxHeight, float scale, int octaves, float persistance, float lacunarity)
        {
            terrain.terrainData.size = new Vector3(terrain.terrainData.size.x, maxHeight, terrain.terrainData.size.z);
            //terrain.terrainData.heightmapResolution = [width, height];
            int height = terrain.terrainData.heightmapResolution;
            int width = terrain.terrainData.heightmapResolution;

            
            float[,] noiseMap = PerlinNoise.GenerateNoiseMap(width, height, scale, octaves, persistance, lacunarity);
            terrain.terrainData.SetHeights(0, 0, noiseMap);

            return noiseMap;
            /*
            GenerateTexture(terrain, gradient, noiseMap);

            int treeNoiseWidth = (int)terrain.terrainData.size.x;
            int treeNoiseHeight = (int)terrain.terrainData.size.z;
            float[,] treeNoiseMap = PerlinNoise.GenerateNoiseMap(treeNoiseWidth + 1, treeNoiseHeight + 1, scale/2, octaves, persistance, lacunarity);
            GenerateTrees(terrain, treePrefab, treeNoiseMap, maxTreeHeight, minTreeHeight);
            */
        }

        public static void GenerateTexture(Terrain terrain, Gradient gradient, float[,] heightMap)
        {
            var terrainData = terrain.terrainData;

            int height = terrainData.heightmapResolution;
            int width = terrainData.heightmapResolution;
            Texture2D texture = new Texture2D(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    texture.SetPixel(y, x, gradient.Evaluate(heightMap[x, y]));
                }
            }
            texture.Apply();

            TerrainLayer[] layersOld = terrainData.terrainLayers;
            TerrainLayer[] layersNew = new TerrainLayer[layersOld.Length + 1];
            System.Array.Copy(layersOld, layersNew, layersOld.Length);
            layersNew[layersNew.Length - 1] = new TerrainLayer();
            layersNew[layersNew.Length - 1].diffuseTexture = texture;
            layersNew[layersNew.Length - 1].tileOffset = new Vector2(0, 0);
            layersNew[layersNew.Length - 1].tileSize = new Vector2(terrainData.size.x, terrainData.size.z);
            layersNew[layersNew.Length - 1].diffuseTexture.Apply(true);

            terrainData.terrainLayers = layersNew;

        }

        public static void GenerateTrees(Terrain terrain, GameObject treePrefab, float[,] noiseMap, int maxHeight, int minHeight)
        {
            for (int x = 0; x < terrain.terrainData.size.x; x += 3)
            {
                for(int z = 0; z < terrain.terrainData.size.z; z += 3)
                {
                    if (noiseMap[x, z] > 0.6f)
                    {
                        float randomX = Random.Range(-1.5f, 1.5f);
                        float randomZ = Random.Range(-1.5f, 1.5f);
                        Vector3 treePosition = new Vector3(x + randomX, 0, z + randomZ);
                        Vector3 treePositionOnTerrain = CityGeneratorUtility.GetPointOnTerrain(treePosition, terrain);

                        if (treePositionOnTerrain.y > minHeight && treePositionOnTerrain.y < maxHeight)
                        {
                            GameObject tree = (GameObject)PrefabUtility.InstantiatePrefab(treePrefab);
                            tree.transform.position = treePositionOnTerrain;
                            float randomScaleModifier = Random.Range(1, 2);
                            tree.transform.localScale = tree.transform.localScale * randomScaleModifier;
                        }

                    }
                }
            }
        }
    }
}
