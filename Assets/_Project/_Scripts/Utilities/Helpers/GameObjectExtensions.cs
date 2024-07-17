using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static T OverwriteComponent<T>(this GameObject gameObject) where T : Component
    {
        // Check if the component already exists
        T existingComponent = gameObject.GetComponent<T>();

        // If it does, remove it
        if (existingComponent != null)
        {
            GameObject.Destroy(existingComponent);
        }

        // Add and return the new component
        return gameObject.AddComponent<T>();
    }
}
