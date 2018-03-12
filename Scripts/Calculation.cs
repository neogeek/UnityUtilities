using UnityEngine;

namespace CandyCoded
{

    public static class Calculation
    {

        /// <summary>
        /// Generates a bounds object based on the position and size of the child GameObjects.
        /// </summary>
        /// <param name="gameObject">Parent GameObject to run calculation on.</param>
        /// <returns>Bounds</returns>
        public static Bounds ParentBounds(GameObject gameObject)
        {

            Vector3 center = Vector3.zero;
            Vector3 min = Vector3.zero;
            Vector3 max = Vector3.zero;

            Bounds bounds = new Bounds();

            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {

                Bounds childBounds = renderer.bounds;

                min = Vector3.Min(min, childBounds.min);
                max = Vector3.Max(max, childBounds.max);

                center = max - min;

            }

            bounds.SetMinMax(min, max);

            return bounds;

        }

        /// <summary>
        /// Generates a bounds object based on the position and size of the child GameObjects.
        /// </summary>
        /// <param name="transform">Parent transform to run calculation on.</param>
        /// <returns>Bounds</returns>
        public static Bounds ParentBounds(Transform transform)
        {

            return ParentBounds(transform.gameObject);

        }

    }

}