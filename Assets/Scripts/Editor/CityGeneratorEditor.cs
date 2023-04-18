using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CityGenerator
{
    public class CityGeneratorEditor : EditorWindow
    {
        static private int _minHeight = 300;
        static private int _minWidth = 300;

        Material roadMaterial;
        Rule rule;
        string rootSentence = "F";
        [Range(0, 10)]
        int iterationLimit = 3;

        Transform startPoint;
        Terrain terrain;

        GameObject buildingPrefab;

        [MenuItem("Window/City Generator")]
        static void OpenWindow()
        {
            CityGeneratorEditor window = (CityGeneratorEditor)GetWindow(typeof(CityGeneratorEditor));
            window.minSize = new Vector2(_minWidth, _minHeight);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("City Generator", EditorStyles.largeLabel);

            startPoint = EditorGUILayout.ObjectField("Start Point", startPoint, typeof(Transform), true) as Transform;
            terrain = EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true) as Terrain;

            EditorGUILayout.LabelField("L-Systems");
            rule = EditorGUILayout.ObjectField("Rule", rule, typeof(Rule), true) as Rule;
            rootSentence = EditorGUILayout.TextField("Root Sentence", rootSentence);
            iterationLimit = EditorGUILayout.IntField("Number of Iterations", iterationLimit);

            EditorGUILayout.LabelField("Roads and Buildings");
            roadMaterial = EditorGUILayout.ObjectField("Road Material", roadMaterial, typeof(Material), true) as Material;
            buildingPrefab = EditorGUILayout.ObjectField("Building Prefab", buildingPrefab, typeof(GameObject), true) as GameObject;

            if (GUILayout.Button("Generate"))
            {
                Generate();
            }
        }

        void Generate()
        {
            if (terrain != null && startPoint != null)
            {
                CityGenerator.GenerateCity(startPoint.position, terrain, rule, rootSentence, iterationLimit, roadMaterial, buildingPrefab);
            }
        }
    }
}
