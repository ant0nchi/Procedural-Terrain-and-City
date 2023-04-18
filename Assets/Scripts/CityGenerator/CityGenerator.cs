using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace CityGenerator
{
    public static class CityGenerator
    {
        public static void GenerateCity(Vector3 startPoint, Terrain terrain, Rule rule, string rootSentence, int iterationLimit, Material roadMaterial, GameObject buildingPrefab)
        {
            LSystemGenerator lSystemGenerator = new LSystemGenerator();
            lSystemGenerator.rules = new Rule[] { rule };
            lSystemGenerator.rootSentence = rootSentence;
            lSystemGenerator.iterationLimit = iterationLimit;

            RoadGenerator roadGenerator = new RoadGenerator();
            roadGenerator.lsystem = lSystemGenerator;
            roadGenerator.lineMaterial = roadMaterial;
            roadGenerator.GenerateRoads(terrain, startPoint, buildingPrefab);

            Vector3 endPoint = startPoint + Vector3.forward * -30;
        }
    }
}