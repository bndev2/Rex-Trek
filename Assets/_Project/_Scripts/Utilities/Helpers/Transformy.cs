using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Helpers
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Predicts the future position of an object given its current position, velocity, and the time step.
        /// </summary>
        /// <param name="position">The current position of the object.</param>
        /// <param name="velocity">The velocity of the object.</param>
        /// <param name="t">The time step.</param>
        /// <returns>The predicted future position of the object.</returns>
        public static Vector3 PredictPosition(Vector3 position, Vector3 velocity, float t)
        {
            // The future position is calculated by adding the displacement (velocity * time) to the current position
            return position + velocity * t;
        }

    }
}