using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CityGenerator
{
    public static class CityGeneratorUtility
    {
        public static Vector2[] GenerateRoadSegmentPoints(Vector2 start, Vector2 end)
        {
            float diffX = start.x - end.x;
            float diffY = start.y - end.y;

            float distance = Vector2.Distance(start, end);
            int numberOfPoints = (int)distance + 3;

            float intervalX = diffX / numberOfPoints;
            float intervalY = diffY / numberOfPoints;

            Vector2[] result = new Vector2[numberOfPoints];
            result[0] = start;

            for (int i = 1; i < numberOfPoints - 1; i++)
            {
                result[i] = new Vector2(start.x - intervalX * i, start.y - intervalY * i);
            }

            result[numberOfPoints - 1] = end;

            return result;
        }

        public static Vector3 GetPointOnTerrain(Vector3 position, Terrain terrain)
        {
            position.y = terrain.SampleHeight(position);
            return position;
        }

        static Quaternion GetPointRotationOnTerrain(Vector3 position, Terrain terrain)
        {
            Quaternion rotation = Quaternion.identity;
            RaycastHit hit;
            var ray = new Ray(position + Vector3.up, Vector3.down);
            if (terrain.GetComponent<Collider>().Raycast(ray, out hit, 3))
            {
                rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }

            return rotation;
        }
    }
}

