using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    [Serializable]
    internal class Gjk
    {
        // Fields
        private static int[] BitsToIndices = new int[] { 0, 1, 2, 0x11, 3, 0x19, 0x1a, 0xd1, 4, 0x21, 0x22, 0x111, 0x23, 0x119, 0x11a, 0x8d1 };
        private Vector3 closestPoint;
        private float[][] det = new float[0x10][];
        private float[][] edgeLengthSq = new float[][] { new float[4], new float[4], new float[4], new float[4] };
        private Vector3[][] edges = new Vector3[][] { new Vector3[4], new Vector3[4], new Vector3[4], new Vector3[4] };
        private float maxLengthSq;
        private int simplexBits;
        private Vector3[] y = new Vector3[4];
        private float[] yLengthSq = new float[4];

        // Methods
        public Gjk()
        {
            for (int i = 0; i < 0x10; i++)
            {
                this.det[i] = new float[4];
            }
        }

        public bool AddSupportPoint(ref Vector3 newPoint)
        {
            int index = (BitsToIndices[this.simplexBits ^ 15] & 7) - 1;
            this.y[index] = newPoint;
            this.yLengthSq[index] = newPoint.LengthSquared();
            for (int i = BitsToIndices[this.simplexBits]; i != 0; i = i >> 3)
            {
                int num2 = (i & 7) - 1;
                Vector3 vector = this.y[num2] - newPoint;
                this.edges[num2][index] = vector;
                this.edges[index][num2] = -(vector);
                this.edgeLengthSq[index][num2] = this.edgeLengthSq[num2][index] = vector.LengthSquared();
            }
            this.UpdateDeterminant(index);
            return this.UpdateSimplex(index);
        }

        private Vector3 ComputeClosestPoint()
        {
            float num3 = 0f;
            Vector3 zero = Vector3.Zero;
            this.maxLengthSq = 0f;
            for (int i = BitsToIndices[this.simplexBits]; i != 0; i = i >> 3)
            {
                int index = (i & 7) - 1;
                float num4 = this.det[this.simplexBits][index];
                num3 += num4;
                zero += (Vector3)(this.y[index] * num4);
                this.maxLengthSq = MathHelper.Max(this.maxLengthSq, this.yLengthSq[index]);
            }
            return (Vector3)(zero / num3);
        }

        private static float Dot(ref Vector3 a, ref Vector3 b)
        {
            return (((a.X * b.X) + (a.Y * b.Y)) + (a.Z * b.Z));
        }

        private bool IsSatisfiesRule(int xBits, int yBits)
        {
            for (int i = BitsToIndices[yBits]; i != 0; i = i >> 3)
            {
                int index = (i & 7) - 1;
                int num3 = ((int)1) << index;
                if ((num3 & xBits) != 0)
                {
                    if (this.det[xBits][index] <= 0f)
                    {
                        return false;
                    }
                }
                else if (this.det[xBits | num3][index] > 0f)
                {
                    return false;
                }
            }
            return true;
        }

        public void Reset()
        {
            this.simplexBits = 0;
            this.maxLengthSq = 0f;
        }

        private void UpdateDeterminant(int xmIdx)
        {
            int index = ((int)1) << xmIdx;
            this.det[index][xmIdx] = 1f;
            int num14 = BitsToIndices[this.simplexBits];
            int num8 = num14;
            for (int i = 0; num8 != 0; i++)
            {
                int num = (num8 & 7) - 1;
                int num12 = ((int)1) << num;
                int num6 = num12 | index;
                this.det[num6][num] = Dot(ref this.edges[xmIdx][num], ref this.y[xmIdx]);
                this.det[num6][xmIdx] = Dot(ref this.edges[num][xmIdx], ref this.y[num]);
                int num11 = num14;
                for (int j = 0; j < i; j++)
                {
                    int num3 = (num11 & 7) - 1;
                    int num5 = ((int)1) << num3;
                    int num9 = num6 | num5;
                    int num4 = (this.edgeLengthSq[num][num3] < this.edgeLengthSq[xmIdx][num3]) ? num : xmIdx;
                    this.det[num9][num3] = (this.det[num6][num] * Dot(ref this.edges[num4][num3], ref this.y[num])) + (this.det[num6][xmIdx] * Dot(ref this.edges[num4][num3], ref this.y[xmIdx]));
                    num4 = (this.edgeLengthSq[num3][num] < this.edgeLengthSq[xmIdx][num]) ? num3 : xmIdx;
                    this.det[num9][num] = (this.det[num5 | index][num3] * Dot(ref this.edges[num4][num], ref this.y[num3])) + (this.det[num5 | index][xmIdx] * Dot(ref this.edges[num4][num], ref this.y[xmIdx]));
                    num4 = (this.edgeLengthSq[num][xmIdx] < this.edgeLengthSq[num3][xmIdx]) ? num : num3;
                    this.det[num9][xmIdx] = (this.det[num12 | num5][num3] * Dot(ref this.edges[num4][xmIdx], ref this.y[num3])) + (this.det[num12 | num5][num] * Dot(ref this.edges[num4][xmIdx], ref this.y[num]));
                    num11 = num11 >> 3;
                }
                num8 = num8 >> 3;
            }
            if ((this.simplexBits | index) == 15)
            {
                int num2 = (this.edgeLengthSq[1][0] < this.edgeLengthSq[2][0]) ? ((this.edgeLengthSq[1][0] < this.edgeLengthSq[3][0]) ? 1 : 3) : ((this.edgeLengthSq[2][0] < this.edgeLengthSq[3][0]) ? 2 : 3);
                this.det[15][0] = ((this.det[14][1] * Dot(ref this.edges[num2][0], ref this.y[1])) + (this.det[14][2] * Dot(ref this.edges[num2][0], ref this.y[2]))) + (this.det[14][3] * Dot(ref this.edges[num2][0], ref this.y[3]));
                num2 = (this.edgeLengthSq[0][1] < this.edgeLengthSq[2][1]) ? ((this.edgeLengthSq[0][1] < this.edgeLengthSq[3][1]) ? 0 : 3) : ((this.edgeLengthSq[2][1] < this.edgeLengthSq[3][1]) ? 2 : 3);
                this.det[15][1] = ((this.det[13][0] * Dot(ref this.edges[num2][1], ref this.y[0])) + (this.det[13][2] * Dot(ref this.edges[num2][1], ref this.y[2]))) + (this.det[13][3] * Dot(ref this.edges[num2][1], ref this.y[3]));
                num2 = (this.edgeLengthSq[0][2] < this.edgeLengthSq[1][2]) ? ((this.edgeLengthSq[0][2] < this.edgeLengthSq[3][2]) ? 0 : 3) : ((this.edgeLengthSq[1][2] < this.edgeLengthSq[3][2]) ? 1 : 3);
                this.det[15][2] = ((this.det[11][0] * Dot(ref this.edges[num2][2], ref this.y[0])) + (this.det[11][1] * Dot(ref this.edges[num2][2], ref this.y[1]))) + (this.det[11][3] * Dot(ref this.edges[num2][2], ref this.y[3]));
                num2 = (this.edgeLengthSq[0][3] < this.edgeLengthSq[1][3]) ? ((this.edgeLengthSq[0][3] < this.edgeLengthSq[2][3]) ? 0 : 2) : ((this.edgeLengthSq[1][3] < this.edgeLengthSq[2][3]) ? 1 : 2);
                this.det[15][3] = ((this.det[7][0] * Dot(ref this.edges[num2][3], ref this.y[0])) + (this.det[7][1] * Dot(ref this.edges[num2][3], ref this.y[1]))) + (this.det[7][2] * Dot(ref this.edges[num2][3], ref this.y[2]));
            }
        }

        private bool UpdateSimplex(int newIndex)
        {
            int yBits = this.simplexBits | (((int)1) << newIndex);
            int xBits = ((int)1) << newIndex;
            for (int i = this.simplexBits; i != 0; i--)
            {
                if (((i & yBits) == i) && this.IsSatisfiesRule(i | xBits, yBits))
                {
                    this.simplexBits = i | xBits;
                    this.closestPoint = this.ComputeClosestPoint();
                    return true;
                }
            }
            bool flag = false;
            if (this.IsSatisfiesRule(xBits, yBits))
            {
                this.simplexBits = xBits;
                this.closestPoint = this.y[newIndex];
                this.maxLengthSq = this.yLengthSq[newIndex];
                flag = true;
            }
            return flag;
        }

        // Properties
        public Vector3 ClosestPoint
        {
            get
            {
                return this.closestPoint;
            }
        }

        public bool FullSimplex
        {
            get
            {
                return (this.simplexBits == 15);
            }
        }

        public float MaxLengthSquared
        {
            get
            {
                return this.maxLengthSq;
            }
        }
    }


}
