using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mathy
{
    public static Vector3 GetPointOnCircleEdgeByRadius(Vector3 _position, float radius, float angle, bool isXZPlane = true)
    {
        if (isXZPlane)
        {
            float x = _position.x + radius * Mathf.Cos(angle);
            float z = _position.z + radius * Mathf.Sin(angle);

            return new Vector3(x, _position.y, z);
        }
        else
        {
            float x = _position.x + radius * Mathf.Cos(angle);
            float y = _position.y + radius * Mathf.Sin(angle);

            return new Vector3(x, y, _position.z);
        }
    }

    public static Vector3 GetPointOnCircleEdgeByPercent(Vector3 _position, float radius, float percentageOfCircle, bool isXZPlane = true)
    {
        // Convert percentage of circle to angle in radians
        float angle = 2 * Mathf.PI * percentageOfCircle;

        if (isXZPlane)
        {
            float x = _position.x + radius * Mathf.Cos(angle);
            float z = _position.z + radius * Mathf.Sin(angle);

            return new Vector3(x, _position.y, z);
        }
        else
        {
            float x = _position.x + radius * Mathf.Cos(angle);
            float y = _position.y + radius * Mathf.Sin(angle);

            return new Vector3(x, y, _position.z);
        }
    }

    public static Vector3 GetPointOnTriangleEdge(Vector3 v0, Vector3 v1, Vector3 v2, int edgeNumber, float t)
    {
        // Ensure t is between 0 and 1
        t = Mathf.Clamp01(t);

        switch (edgeNumber)
        {
            case 0: // Edge from v0 to v1
                return Vector3.Lerp(v0, v1, t);
            case 1: // Edge from v1 to v2
                return Vector3.Lerp(v1, v2, t);
            case 2: // Edge from v2 to v0
                return Vector3.Lerp(v2, v0, t);
            default:
                Debug.LogError("Invalid edge number! Must be 0, 1, or 2.");
                return Vector3.zero;
        }
    }

    public static int GetHours(float seconds)
    {
        int hour = (int)(seconds / 3600);
        return hour;
    }

    public static int GetMinutes(float seconds)
    {
        int minutes = (int)((seconds % 3600) / 60);
        return minutes;
    }

    public static int GetSeconds(float seconds)
    {
        int remainingSeconds = (int)(seconds % 60);
        return remainingSeconds;
    }

    public static List<int> GetTime(float seconds)
    {
        int hours = GetHours(seconds);
        int minutes = GetMinutes(seconds);
        int remainingSeconds = GetSeconds(seconds);

        return new List<int> { hours, minutes, remainingSeconds };
    }

    public static float LogarithmicDecay(float x)
    {
        if (x == 0)
        {
            Debug.LogError("Input cannot be zero.");
            return float.NaN;
        }
        return Mathf.Log(1 / x);
    }

    public static float LogarithmicGrowth(float x)
    {
        if (x <= 0)
        {
            Debug.LogError("Input must be greater than zero.");
            return float.NaN;
        }
        return Mathf.Log(x);
    }


    public static float GetXZDistance(Vector3 start, Vector3 end)
    {
        return Vector3.Distance(new Vector3(start.x, 0, start.z), new Vector3(end.x, 0, end.z));
    }

    public static float GetSimilarity(Transform origin, Transform target)
    {
        // Calculate the dot product of the two forward vectors
        float dotProduct = Vector3.Dot(origin.forward.normalized, target.forward.normalized);

        // Convert the dot product to an angle
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

        return angle;
    }

    public static bool GetWithin(float startNumber, float comparedNumber, float tolerance)
    {
        if (comparedNumber >= startNumber - tolerance && comparedNumber <= startNumber + tolerance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
