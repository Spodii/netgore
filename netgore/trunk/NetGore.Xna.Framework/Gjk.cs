using System;
using System.Linq;

namespace Microsoft.Xna.Framework
{
    [Serializable]
    class Gjk
    {
        // Fields
        static readonly int[] BitsToIndices = new int[]
        { 0, 1, 2, 0x11, 3, 0x19, 0x1a, 0xd1, 4, 0x21, 0x22, 0x111, 0x23, 0x119, 0x11a, 0x8d1 };

        readonly float[][] det = new float[0x10][];
        readonly float[][] edgeLengthSq = new float[][] { new float[4], new float[4], new float[4], new float[4] };
        readonly Vector3[][] edges = new Vector3[][] { new Vector3[4], new Vector3[4], new Vector3[4], new Vector3[4] };
        readonly Vector3[] y = new Vector3[4];
        readonly float[] yLengthSq = new float[4];
        Vector3 closestPoint;
        float maxLengthSq;
        int simplexBits;

        // Methods
        public Gjk()
        {
            for (int i = 0; i < 0x10; i++)
            {
                det[i] = new float[4];
            }
        }

        public Vector3 ClosestPoint
        {
            get { return closestPoint; }
        }

        public bool FullSimplex
        {
            get { return (simplexBits == 15); }
        }

        public float MaxLengthSquared
        {
            get { return maxLengthSq; }
        }

        public bool AddSupportPoint(ref Vector3 newPoint)
        {
            int index = (BitsToIndices[simplexBits ^ 15] & 7) - 1;
            y[index] = newPoint;
            yLengthSq[index] = newPoint.LengthSquared();
            for (int i = BitsToIndices[simplexBits]; i != 0; i = i >> 3)
            {
                int num2 = (i & 7) - 1;
                Vector3 vector = y[num2] - newPoint;
                edges[num2][index] = vector;
                edges[index][num2] = -(vector);
                edgeLengthSq[index][num2] = edgeLengthSq[num2][index] = vector.LengthSquared();
            }
            UpdateDeterminant(index);
            return UpdateSimplex(index);
        }

        Vector3 ComputeClosestPoint()
        {
            float num3 = 0f;
            Vector3 zero = Vector3.Zero;
            maxLengthSq = 0f;
            for (int i = BitsToIndices[simplexBits]; i != 0; i = i >> 3)
            {
                int index = (i & 7) - 1;
                float num4 = det[simplexBits][index];
                num3 += num4;
                zero += (y[index] * num4);
                maxLengthSq = MathHelper.Max(maxLengthSq, yLengthSq[index]);
            }
            return (zero / num3);
        }

        static float Dot(ref Vector3 a, ref Vector3 b)
        {
            return (((a.X * b.X) + (a.Y * b.Y)) + (a.Z * b.Z));
        }

        bool IsSatisfiesRule(int xBits, int yBits)
        {
            for (int i = BitsToIndices[yBits]; i != 0; i = i >> 3)
            {
                int index = (i & 7) - 1;
                int num3 = (1) << index;
                if ((num3 & xBits) != 0)
                {
                    if (det[xBits][index] <= 0f)
                        return false;
                }
                else if (det[xBits | num3][index] > 0f)
                    return false;
            }
            return true;
        }

        public void Reset()
        {
            simplexBits = 0;
            maxLengthSq = 0f;
        }

        void UpdateDeterminant(int xmIdx)
        {
            int index = (1) << xmIdx;
            det[index][xmIdx] = 1f;
            int num14 = BitsToIndices[simplexBits];
            int num8 = num14;
            for (int i = 0; num8 != 0; i++)
            {
                int num = (num8 & 7) - 1;
                int num12 = (1) << num;
                int num6 = num12 | index;
                det[num6][num] = Dot(ref edges[xmIdx][num], ref y[xmIdx]);
                det[num6][xmIdx] = Dot(ref edges[num][xmIdx], ref y[num]);
                int num11 = num14;
                for (int j = 0; j < i; j++)
                {
                    int num3 = (num11 & 7) - 1;
                    int num5 = (1) << num3;
                    int num9 = num6 | num5;
                    int num4 = (edgeLengthSq[num][num3] < edgeLengthSq[xmIdx][num3]) ? num : xmIdx;
                    det[num9][num3] = (det[num6][num] * Dot(ref edges[num4][num3], ref y[num])) +
                                      (det[num6][xmIdx] * Dot(ref edges[num4][num3], ref y[xmIdx]));
                    num4 = (edgeLengthSq[num3][num] < edgeLengthSq[xmIdx][num]) ? num3 : xmIdx;
                    det[num9][num] = (det[num5 | index][num3] * Dot(ref edges[num4][num], ref y[num3])) +
                                     (det[num5 | index][xmIdx] * Dot(ref edges[num4][num], ref y[xmIdx]));
                    num4 = (edgeLengthSq[num][xmIdx] < edgeLengthSq[num3][xmIdx]) ? num : num3;
                    det[num9][xmIdx] = (det[num12 | num5][num3] * Dot(ref edges[num4][xmIdx], ref y[num3])) +
                                       (det[num12 | num5][num] * Dot(ref edges[num4][xmIdx], ref y[num]));
                    num11 = num11 >> 3;
                }
                num8 = num8 >> 3;
            }
            if ((simplexBits | index) == 15)
            {
                int num2 = (edgeLengthSq[1][0] < edgeLengthSq[2][0])
                               ? ((edgeLengthSq[1][0] < edgeLengthSq[3][0]) ? 1 : 3)
                               : ((edgeLengthSq[2][0] < edgeLengthSq[3][0]) ? 2 : 3);
                det[15][0] = ((det[14][1] * Dot(ref edges[num2][0], ref y[1])) + (det[14][2] * Dot(ref edges[num2][0], ref y[2]))) +
                             (det[14][3] * Dot(ref edges[num2][0], ref y[3]));
                num2 = (edgeLengthSq[0][1] < edgeLengthSq[2][1])
                           ? ((edgeLengthSq[0][1] < edgeLengthSq[3][1]) ? 0 : 3)
                           : ((edgeLengthSq[2][1] < edgeLengthSq[3][1]) ? 2 : 3);
                det[15][1] = ((det[13][0] * Dot(ref edges[num2][1], ref y[0])) + (det[13][2] * Dot(ref edges[num2][1], ref y[2]))) +
                             (det[13][3] * Dot(ref edges[num2][1], ref y[3]));
                num2 = (edgeLengthSq[0][2] < edgeLengthSq[1][2])
                           ? ((edgeLengthSq[0][2] < edgeLengthSq[3][2]) ? 0 : 3)
                           : ((edgeLengthSq[1][2] < edgeLengthSq[3][2]) ? 1 : 3);
                det[15][2] = ((det[11][0] * Dot(ref edges[num2][2], ref y[0])) + (det[11][1] * Dot(ref edges[num2][2], ref y[1]))) +
                             (det[11][3] * Dot(ref edges[num2][2], ref y[3]));
                num2 = (edgeLengthSq[0][3] < edgeLengthSq[1][3])
                           ? ((edgeLengthSq[0][3] < edgeLengthSq[2][3]) ? 0 : 2)
                           : ((edgeLengthSq[1][3] < edgeLengthSq[2][3]) ? 1 : 2);
                det[15][3] = ((det[7][0] * Dot(ref edges[num2][3], ref y[0])) + (det[7][1] * Dot(ref edges[num2][3], ref y[1]))) +
                             (det[7][2] * Dot(ref edges[num2][3], ref y[2]));
            }
        }

        bool UpdateSimplex(int newIndex)
        {
            int yBits = simplexBits | ((1) << newIndex);
            int xBits = (1) << newIndex;
            for (int i = simplexBits; i != 0; i--)
            {
                if (((i & yBits) == i) && IsSatisfiesRule(i | xBits, yBits))
                {
                    simplexBits = i | xBits;
                    closestPoint = ComputeClosestPoint();
                    return true;
                }
            }
            bool flag = false;
            if (IsSatisfiesRule(xBits, yBits))
            {
                simplexBits = xBits;
                closestPoint = y[newIndex];
                maxLengthSq = yLengthSq[newIndex];
                flag = true;
            }
            return flag;
        }

        // Properties
    }
}