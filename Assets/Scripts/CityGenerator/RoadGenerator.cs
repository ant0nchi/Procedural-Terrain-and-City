using CityGenerator;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CityGenerator
{
    public class RoadGenerator
    {
        public LSystemGenerator lsystem;
        List<Vector3> positions = new List<Vector3>();
        //public GameObject prefab;
        public Material lineMaterial;

        private int length = 8;
        private float angle = 90;

        public int Length
        {
            get
            {
                if (length > 0)
                {
                    return length;
                }
                else
                {
                    return 1;
                }
            }
            set => length = value;
        }

        public void GenerateRoads(Terrain terrain, Vector3 startPosition, GameObject buildingPrefab)
        {
            var sequence = lsystem.GenerateSentence();
            VisualizeSequence(sequence, terrain, startPosition, buildingPrefab);
        }

        private void VisualizeSequence(string sequence, Terrain terrain, Vector3 startPosition, GameObject buildingPrefab)
        {
            Stack<Agent> savePoints = new Stack<Agent>();
            var currentPosition = startPosition;

            Vector3 direction = Vector3.forward;
            Vector3 tempPosition = startPosition;

            positions.Add(currentPosition);

            foreach (var letter in sequence)
            {
                EncodingLetters encoding = (EncodingLetters)letter;
                switch (encoding)
                {
                    case EncodingLetters.save:
                        savePoints.Push(new Agent
                        {
                            position = currentPosition,
                            direction = direction,
                            length = Length
                        });
                        break;
                    case EncodingLetters.load:
                        if (savePoints.Count > 0)
                        {
                            var agentParameter = savePoints.Pop();
                            currentPosition = agentParameter.position;
                            direction = agentParameter.direction;
                            Length = agentParameter.length;
                        }
                        else
                        {
                            throw new System.Exception("Dont have saved point in our stack");
                        }
                        break;
                    case EncodingLetters.generate:
                        tempPosition = currentPosition;
                        currentPosition += direction * length;

                        Vector2[] segmentPoints = CityGeneratorUtility.GenerateRoadSegmentPoints(new Vector2(tempPosition.x, tempPosition.z), new Vector2(currentPosition.x, currentPosition.z));
                        for (int i = 0; i < segmentPoints.Length; i++)
                        {
                            if(i > 0)
                            {
                                Vector3 thisPoint = CityGeneratorUtility.GetPointOnTerrain(new Vector3(segmentPoints[i].x, 0, segmentPoints[i].y), terrain);
                                Vector3 previousPoint = CityGeneratorUtility.GetPointOnTerrain(new Vector3(segmentPoints[i-1].x, 0, segmentPoints[i-1].y), terrain);

                                if (i < segmentPoints.Length - 1 && i > 1)
                                {
                                    BuildingGenerator.GenerateBuilding(thisPoint, direction, terrain, buildingPrefab);
                                }

                                DrawLine(previousPoint, thisPoint, Color.gray);
                            }
                        }
                        //Length -= 2;
                        positions.Add(currentPosition);
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                        break;
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                        break;
                    default:
                        break;
                }
            }

            /*
            foreach (var position in positions)
            {
                PrefabUtility.InstantiatePrefab(prefab, prefab.transform);
            }
            */

        }

        private void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            GameObject line = new GameObject("line");
            line.transform.position = start;
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
            lineRenderer.SetPosition(0, end + Vector3.up * 0.1f);
            lineRenderer.SetPosition(1, start + Vector3.up * 0.1f);
        }

        public enum EncodingLetters
        {
            save = '[',
            load = ']',
            generate = 'F',
            turnRight = '+',
            turnLeft = '-'
        }
    }
}