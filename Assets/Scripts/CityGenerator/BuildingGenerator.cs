using UnityEditor;
using UnityEngine;

namespace CityGenerator
{
    public static class BuildingGenerator
    {
        public static void GenerateBuilding(Vector3 roadPoint, Vector3 direction, Terrain terrain, GameObject buildingPrefab)
        {
            Vector3 left = Vector3.Cross(direction, Vector3.up).normalized;
            Vector3 right = -left;

            Vector3 placementLeftPoint = roadPoint + left * 1.2f;
            Vector3 placementRightPoint = roadPoint + right * 1.2f;
            placementLeftPoint = CityGeneratorUtility.GetPointOnTerrain(placementLeftPoint, terrain);
            placementRightPoint = CityGeneratorUtility.GetPointOnTerrain(placementRightPoint, terrain);

            PlaceBuilding(placementLeftPoint, buildingPrefab);
            PlaceBuilding(placementRightPoint, buildingPrefab);

        }
        static void PlaceBuilding(Vector3 placementPoint, GameObject buildingPrefab)
        {
            bool isPlaceable = true;
            Collider[] hitColliders = Physics.OverlapSphere(placementPoint, 0.9f);
            int i = 0;
            Debug.Log(hitColliders);
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Building")
                {
                    isPlaceable = false;
                    break;
                }
                i++;
            }

            if (isPlaceable)
            {
                float randomHeight = Random.Range(1, 5);
                //GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
                GameObject building = (GameObject)PrefabUtility.InstantiatePrefab(buildingPrefab);
                building.transform.position = placementPoint;
                building.transform.localScale = new Vector3(building.transform.localScale.x, building.transform.localScale.y * randomHeight, building.transform.localScale.z);
                building.AddComponent<BoxCollider>();
                building.tag = "Building";
            }
        }
    }
}
