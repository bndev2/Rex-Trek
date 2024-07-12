using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Helpers
{
    public static class RandomExtensions
    {
        private static readonly List<string> firstNames = new List<string> {"Bob", "George", "Charlie", "David", "Eric", "Frank", "Grace", "Henry", "Isaac", "Jack",
            "Kevin", "Lack", "Marvin", "Oliver", "Paul", "Robin", "Stanley", "Todd", "Ulysses", "Victor", "William", "Zed", "Thomas", "James",
            "Andrew", "Bernard", "Charles", "Doris", "Edward", "Frank", "George", "Harry", "Ivan", "James", "Kevin", "Lawrence", "Mark", "Nathan",
            "Benjamin", "Richard", "Tyler"};

        private static readonly List<string> lastNames = new List<string> { "Smith", "Johnson", "Williams", "Brown", "Jones", "Wilson", "Moore", "Taylor", "Davis", "Martin", "Thomas" };

        private static readonly List<string> nouns = new List<string> { "Apple", "Ball", "Cat", "Dog", "Fish", "Mouse", "Rabbit", "Sheep", "Whale", "Zebra", "River", "Lake", "Sea", "Tree", "Car", "Photo"
    };

        private static T RandomElement<T>(List<T> list)
        {
            return list[new System.Random().Next(list.Count)];
        }

        //Lists 

        public static T DictionaryWeightedRandom<T>(this System.Random rnd, Dictionary<T, int> randomDictionary)
        {
            int totalWeight = randomDictionary.Values.Sum();
            if (totalWeight != 100)
            {
                Debug.LogError("The sum of weights must be 100");
                return default(T);
            }

            int randomValue = rnd.Next(totalWeight);
            foreach (KeyValuePair<T, int> kvp in randomDictionary)
            {
                if (randomValue < kvp.Value)
                {
                    return kvp.Key;
                }
                randomValue -= kvp.Value;
            }

            return default(T);
        }

        public static Dictionary<T, int> PrioritizeElementsByIndex<T>(List<T> list)
        {
            Dictionary<T, int> dictionary = new Dictionary<T, int>();

            int weightPool = 100;
            int subtract = weightPool / list.Count;

            for (int i = 0; i < list.Count; i++)
            {
                // Assign the remaining weight pool to the last element
                if (i == list.Count - 1)
                {
                    dictionary[list[i]] = weightPool;
                }
                else
                {
                    dictionary[list[i]] = subtract;
                    weightPool -= subtract;

                    // Recalculate the subtract value based on the remaining elements
                    if (i < list.Count - 2)
                    {
                        subtract = weightPool / (list.Count - i - 1);
                    }
                }
            }

            return dictionary;
        }


        public static List<T> ListShuffleList<T>(this System.Random rnd, List<T> list)
        {
            return list.OrderBy(x => rnd.Next()).ToList();
        }

        //  Bools

        public static bool BoolIsTrueByChance(this System.Random rnd, float chance)
        {
            if (chance > 100 || chance < 0)
            {
                Debug.LogError("The chance must be between 0 and 100");
                return false;
            }

            float randomNumber = (float)rnd.NextDouble() * 100;

            if (randomNumber <= chance)
            {
                return true;
            }

            return false;
        }


        public static bool BoolIsTrueByChance(this System.Random rnd, int chance)
        {
            if (chance > 100 || chance < 0)
            {
                Debug.LogError("The chance must be between 0 and 100");
                return false;
            }

            int randomNumber = rnd.Next(0, 101);

            if (randomNumber <= chance)
            {
                return true;
            }

            return false;
        }

        // ints

        public static int IntRandomFactor(int number, int minPower, int maxPower)
        {
            int randomFactor = UnityEngine.Random.Range(minPower, maxPower);

            return number ^ randomFactor;
        }

        // Strings

        public static string StringRandomNoun(this System.Random rnd, bool lowerCase = true)
        {
            string noun = RandomElement(nouns);

            if (!lowerCase)
            {
                return noun;
            }
            else
            {
                return noun.ToLower();
            }
        }

        public static string StringRandomName(this System.Random rnd, bool includeFirstName = true, bool includeLastName = true)
        {
            string firstName = includeFirstName ? RandomElement(firstNames) : "";
            string lastName = includeLastName ? RandomElement(lastNames) : "";

            if (!includeFirstName)
            {
                return lastName;
            }
            if (!includeLastName)
            {
                return firstName;
            }
            else
            {
                return firstName + " " + lastName;
            }
        }

        public static T RandomEnumValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            int index = UnityEngine.Random.Range(0, values.Length);
            return (T)values.GetValue(index);
        }

        // Vectors

        public static Vector3 Vector3RandomInsideRadius(Vector3 position, float radius)
        {
            float maxX = position.x + radius;
            float maxY = position.y + radius;
            float maxZ = position.z + radius;

            float minX = position.x - radius;
            float minY = position.y - radius;
            float minZ = position.z - radius;

            return new Vector3(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY), UnityEngine.Random.Range(minZ, maxZ));
        }

        public static Vector3 Vector3RandomBetweenPoints(Vector3 originPosition, Vector3 targetPosition)
        {
            Vector3 randomPoint = new Vector3();

            randomPoint.x = UnityEngine.Random.Range(Mathf.Min(originPosition.x, targetPosition.x), Mathf.Max(originPosition.x, targetPosition.x));
            randomPoint.y = UnityEngine.Random.Range(Mathf.Min(originPosition.y, targetPosition.y), Mathf.Max(originPosition.y, targetPosition.y));
            randomPoint.z = UnityEngine.Random.Range(Mathf.Min(originPosition.z, targetPosition.z), Mathf.Max(originPosition.z, targetPosition.z));

            return randomPoint;
        }

        // Colors 
        public static Color RandomColor()
        {
            return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        }

    }
}
