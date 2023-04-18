using UnityEngine;

namespace CityGenerator
{
    [CreateAssetMenu(menuName = "ProceduralCity/Rule")]
    public class Rule : ScriptableObject
    {
        [SerializeField] public string letter;
        [SerializeField] private string[] results = null;

        public string GetResult()
        {
            if (results.Length > 1)
            {
                int randomIndex = Random.Range(0, results.Length);
                return results[randomIndex];
            }
            return results[0];
        }

    }
}
