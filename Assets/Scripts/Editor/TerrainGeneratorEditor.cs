using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace TerrainGenerator
{
    public class TerrainGeneratorEditor : EditorWindow
    {
        static private int _minHeight = 300;
        static private int _minWidth = 300;

        bool isTextured = false;
        Gradient textureGradient = new Gradient();
        GradientColorKey[] colorKey = new GradientColorKey[3];
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[3];
        bool isUsing3Colors = false;
        Color gradientColor1 = Color.blue;
        Color gradientColor2 = Color.yellow;
        Color gradientColor3 = Color.green;

        int maxHeight = 60;
        int octaves = 15;
        float scale = 300;
        float persistance = 0.5f;
        float lacunarity = 2;

        bool isTrees = false;
        GameObject treePrefab;
        int treeMinHeight = 35;
        int treeMaxHeight = 55;

        [MenuItem("Window/Terrain Generator")]
        static void OpenWindow()
        {
            TerrainGeneratorEditor window = (TerrainGeneratorEditor)GetWindow(typeof(TerrainGeneratorEditor));

            window.minSize = new Vector2(_minWidth, _minHeight);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Terrain Generator", EditorStyles.largeLabel);

            EditorGUILayout.LabelField("Noise");
            maxHeight = EditorGUILayout.IntField("MaxHeight", maxHeight);
            octaves = EditorGUILayout.IntField("Number of Octaves", octaves);
            scale = EditorGUILayout.FloatField("Sclae", scale);
            persistance = EditorGUILayout.FloatField("Persistance", persistance);
            lacunarity = EditorGUILayout.FloatField("Lacunarity", lacunarity);

            EditorGUILayout.Space();
            isTextured = EditorGUILayout.Toggle("isTextured", isTextured);
            if (isTextured)
            {
                EditorGUILayout.LabelField("Texture");
                isUsing3Colors = EditorGUILayout.Toggle("Use 3 Colors for Gradient", isUsing3Colors);
                if (isUsing3Colors)
                {
                    gradientColor1 = EditorGUILayout.ColorField("Gradient Color 1", gradientColor1);
                    gradientColor2 = EditorGUILayout.ColorField("Gradient Color 2", gradientColor2);
                    gradientColor3 = EditorGUILayout.ColorField("Gradient Color 3", gradientColor3);
                    UpdateGradient(gradientColor1, gradientColor2, gradientColor3);
                }
                textureGradient = EditorGUILayout.GradientField("Gradient", textureGradient);
            }

            EditorGUILayout.Space();
            isTrees = EditorGUILayout.Toggle("is Planted with trees", isTrees);
            if (isTrees)
            {
                EditorGUILayout.LabelField("Forests");
                treePrefab = EditorGUILayout.ObjectField("Tree Prefab", treePrefab, typeof(GameObject), true) as GameObject;
                treeMaxHeight = EditorGUILayout.IntField("Max Tree Gen Height", treeMaxHeight);
                treeMinHeight = EditorGUILayout.IntField("Min Tree Gen Height", treeMinHeight);
            }


            if (GUILayout.Button("Generate"))
            {
                Generate();
            }
        }

        void Generate()
        {
            if (Selection.gameObjects.Length > 0)
            {
                GameObject selectedTerrain = Selection.gameObjects[0];
                Terrain terrain = selectedTerrain.GetComponent<Terrain>();
                if (terrain != null)
                {
                    float[,] noise = TerrainGenerator.GenerateTerrain(terrain, maxHeight, scale, octaves, persistance, lacunarity);
                    if(isTextured)
                    {
                        TerrainGenerator.GenerateTexture(terrain, textureGradient, noise);
                    }
                    if(isTrees)
                    {
                        int treeNoiseWidth = (int)terrain.terrainData.size.x;
                        int treeNoiseHeight = (int)terrain.terrainData.size.z;
                        float[,] treeNoiseMap = PerlinNoise.GenerateNoiseMap(treeNoiseWidth + 1, treeNoiseHeight + 1, scale / 2, octaves, persistance, lacunarity);
                        TerrainGenerator.GenerateTrees(terrain, treePrefab, treeNoiseMap, treeMaxHeight, treeMinHeight);
                    }
                }
                else
                {
                    Debug.LogWarning("Game object must be a Terrain!");
                }
            }
            else
            {
                Debug.LogWarning("Game object not selected!");
            }
        }

        private void UpdateGradient(Color color1, Color color2, Color color3)
        {
            colorKey[0].color = color1;
            colorKey[0].time = 0f;
            colorKey[1].color = color2;
            colorKey[1].time = 0.4f;
            colorKey[2].color = color3;
            colorKey[2].time = 1f;

            alphaKey[0].alpha = 1f;
            alphaKey[0].time = 0f;
            alphaKey[1].alpha = 1f;
            alphaKey[1].time = 0.4f;
            alphaKey[2].alpha = 1f;
            alphaKey[2].time = 1f;
            textureGradient.SetKeys(colorKey, alphaKey);
        }

    }
}
