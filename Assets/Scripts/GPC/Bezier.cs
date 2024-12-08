using UnityEngine;

using System.Collections.Generic;

namespace Beam.Utility
{
    class Bezier
    {
        //Get the point B as a function of the distance between A and C and the vector AB.
        public static Vector3 QuadraticApproximateB(Vector3 A, Vector3 C, Vector3 AB, float scale = 0.9f)
        {
            Vector3 AC = C - A;
            float ABMag = 1.0f / 2 * AC.magnitude * scale;
            return A + ABMag * AB.normalized;
        }

        //Gets the point along the bezier curve represented by A, B, and C: https://en.wikipedia.org/wiki/B%C3%A9zier_curve
        //At least I learned something from Prof. Rossignac.
        public static Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 d = Vector3.Lerp(a, b, t);
            Vector3 e = Vector3.Lerp(b, c, t);
            return Vector3.Lerp(d, e, t);
        }

        //Returns a list of points with length numSamples that are uniformly distributed along the bezier curve a, b, c.
        public static List<Vector3> QuadraticSample(Vector3 a, Vector3 b, Vector3 c, int numSamples)
        {
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < numSamples; i++)
            {
                positions.Add(QuadraticCurve(a, b, c, (float)i / (numSamples - 1)));
            }
            return positions;
        }
    }
}
