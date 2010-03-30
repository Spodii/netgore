using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Design;

namespace Microsoft.Xna.Framework
{/// <summary>Defines an axis-aligned box-shaped 3D volume. Reference page contains links to related code samples.</summary>
[Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(BoundingBoxConverter))]
public struct BoundingBox : IEquatable<BoundingBox>
{
    /// <summary>Specifies the total number of corners (8) in the BoundingBox.</summary>
    public const int CornerCount = 8;
    /// <summary>The minimum point the BoundingBox contains.</summary>
    public Vector3 Min;
    /// <summary>The maximum point the BoundingBox contains.</summary>
    public Vector3 Max;
    /// <summary>Gets an array of points that make up the corners of the BoundingBox.</summary>
    public Vector3[] GetCorners()
    {
        return new Vector3[] { new Vector3(this.Min.X, this.Max.Y, this.Max.Z), new Vector3(this.Max.X, this.Max.Y, this.Max.Z), new Vector3(this.Max.X, this.Min.Y, this.Max.Z), new Vector3(this.Min.X, this.Min.Y, this.Max.Z), new Vector3(this.Min.X, this.Max.Y, this.Min.Z), new Vector3(this.Max.X, this.Max.Y, this.Min.Z), new Vector3(this.Max.X, this.Min.Y, this.Min.Z), new Vector3(this.Min.X, this.Min.Y, this.Min.Z) };
    }

    /// <summary>Gets the array of points that make up the corners of the BoundingBox.</summary>
    /// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingBox are written.</param>
    public void GetCorners(Vector3[] corners)
    {
        if (corners == null)
        {
            throw new ArgumentNullException("corners");
        }
        if (corners.Length < 8)
        {
            throw new ArgumentOutOfRangeException("corners", FrameworkResources.NotEnoughCorners);
        }
        corners[0].X = this.Min.X;
        corners[0].Y = this.Max.Y;
        corners[0].Z = this.Max.Z;
        corners[1].X = this.Max.X;
        corners[1].Y = this.Max.Y;
        corners[1].Z = this.Max.Z;
        corners[2].X = this.Max.X;
        corners[2].Y = this.Min.Y;
        corners[2].Z = this.Max.Z;
        corners[3].X = this.Min.X;
        corners[3].Y = this.Min.Y;
        corners[3].Z = this.Max.Z;
        corners[4].X = this.Min.X;
        corners[4].Y = this.Max.Y;
        corners[4].Z = this.Min.Z;
        corners[5].X = this.Max.X;
        corners[5].Y = this.Max.Y;
        corners[5].Z = this.Min.Z;
        corners[6].X = this.Max.X;
        corners[6].Y = this.Min.Y;
        corners[6].Z = this.Min.Z;
        corners[7].X = this.Min.X;
        corners[7].Y = this.Min.Y;
        corners[7].Z = this.Min.Z;
    }

    /// <summary>Creates an instance of BoundingBox. Reference page contains links to related code samples.</summary>
    /// <param name="min">The minimum point the BoundingBox includes.</param>
    /// <param name="max">The maximum point the BoundingBox includes.</param>
    public BoundingBox(Vector3 min, Vector3 max)
    {
        this.Min = min;
        this.Max = max;
    }

    /// <summary>Determines whether two instances of BoundingBox are equal.</summary>
    /// <param name="other">The BoundingBox to compare with the current BoundingBox.</param>
    public bool Equals(BoundingBox other)
    {
        return ((this.Min == other.Min) && (this.Max == other.Max));
    }

    /// <summary>Determines whether two instances of BoundingBox are equal.</summary>
    /// <param name="obj">The Object to compare with the current BoundingBox.</param>
    public override bool Equals(object obj)
    {
        bool flag = false;
        if (obj is BoundingBox)
        {
            flag = this.Equals((BoundingBox) obj);
        }
        return flag;
    }

    /// <summary>Gets the hash code for this instance.</summary>
    public override int GetHashCode()
    {
        return (this.Min.GetHashCode() + this.Max.GetHashCode());
    }

    /// <summary>Returns a String that represents the current BoundingBox.</summary>
    public override string ToString()
    {
        return string.Format(CultureInfo.CurrentCulture, "{{Min:{0} Max:{1}}}", new object[] { this.Min.ToString(), this.Max.ToString() });
    }

    /// <summary>Creates the smallest BoundingBox that contains the two specified BoundingBox instances.</summary>
    /// <param name="original">One of the BoundingBoxs to contain.</param>
    /// <param name="additional">One of the BoundingBoxs to contain.</param>
    public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
    {
        BoundingBox box;
        Vector3.Min(ref original.Min, ref additional.Min, out box.Min);
        Vector3.Max(ref original.Max, ref additional.Max, out box.Max);
        return box;
    }

    /// <summary>Creates the smallest BoundingBox that contains the two specified BoundingBox instances.</summary>
    /// <param name="original">One of the BoundingBox instances to contain.</param>
    /// <param name="additional">One of the BoundingBox instances to contain.</param>
    /// <param name="result">[OutAttribute] The created BoundingBox.</param>
    public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
    {
        Vector3 vector;
        Vector3 vector2;
        Vector3.Min(ref original.Min, ref additional.Min, out vector2);
        Vector3.Max(ref original.Max, ref additional.Max, out vector);
        result.Min = vector2;
        result.Max = vector;
    }

    /// <summary>Creates the smallest BoundingBox that will contain the specified BoundingSphere.</summary>
    /// <param name="sphere">The BoundingSphere to contain.</param>
    public static BoundingBox CreateFromSphere(BoundingSphere sphere)
    {
        BoundingBox box;
        box.Min.X = sphere.Center.X - sphere.Radius;
        box.Min.Y = sphere.Center.Y - sphere.Radius;
        box.Min.Z = sphere.Center.Z - sphere.Radius;
        box.Max.X = sphere.Center.X + sphere.Radius;
        box.Max.Y = sphere.Center.Y + sphere.Radius;
        box.Max.Z = sphere.Center.Z + sphere.Radius;
        return box;
    }

    /// <summary>Creates the smallest BoundingBox that will contain the specified BoundingSphere.</summary>
    /// <param name="sphere">The BoundingSphere to contain.</param>
    /// <param name="result">[OutAttribute] The created BoundingBox.</param>
    public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
    {
        result.Min.X = sphere.Center.X - sphere.Radius;
        result.Min.Y = sphere.Center.Y - sphere.Radius;
        result.Min.Z = sphere.Center.Z - sphere.Radius;
        result.Max.X = sphere.Center.X + sphere.Radius;
        result.Max.Y = sphere.Center.Y + sphere.Radius;
        result.Max.Z = sphere.Center.Z + sphere.Radius;
    }

    /// <summary>Creates the smallest BoundingBox that will contain a group of points.</summary>
    /// <param name="points">A list of points the BoundingBox should contain.</param>
    public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
    {
        if (points == null)
        {
            throw new ArgumentNullException();
        }
        bool flag = false;
        Vector3 vector3 = new Vector3(float.MaxValue);
        Vector3 vector2 = new Vector3(float.MinValue);
        foreach (Vector3 vector in points)
        {
            Vector3 vector4 = vector;
            Vector3.Min(ref vector3, ref vector4, out vector3);
            Vector3.Max(ref vector2, ref vector4, out vector2);
            flag = true;
        }
        if (!flag)
        {
            throw new ArgumentException(FrameworkResources.BoundingBoxZeroPoints);
        }
        return new BoundingBox(vector3, vector2);
    }

    /// <summary>Checks whether the current BoundingBox intersects another BoundingBox. Reference page contains links to related code samples.</summary>
    /// <param name="box">The BoundingBox to check for intersection with.</param>
    public bool Intersects(BoundingBox box)
    {
        if ((this.Max.X < box.Min.X) || (this.Min.X > box.Max.X))
        {
            return false;
        }
        if ((this.Max.Y < box.Min.Y) || (this.Min.Y > box.Max.Y))
        {
            return false;
        }
        return ((this.Max.Z >= box.Min.Z) && (this.Min.Z <= box.Max.Z));
    }

    /// <summary>Checks whether the current BoundingBox intersects another BoundingBox. Reference page contains links to related code samples.</summary>
    /// <param name="box">The BoundingBox to check for intersection with.</param>
    /// <param name="result">[OutAttribute] true if the BoundingBox instances intersect; false otherwise.</param>
    public void Intersects(ref BoundingBox box, out bool result)
    {
        result = false;
        if ((((this.Max.X >= box.Min.X) && (this.Min.X <= box.Max.X)) && ((this.Max.Y >= box.Min.Y) && (this.Min.Y <= box.Max.Y))) && ((this.Max.Z >= box.Min.Z) && (this.Min.Z <= box.Max.Z)))
        {
            result = true;
        }
    }

    /// <summary>Checks whether the current BoundingBox intersects a BoundingFrustum. Reference page contains links to related code samples.</summary>
    /// <param name="frustum">The BoundingFrustum to check for intersection with.</param>
    public bool Intersects(BoundingFrustum frustum)
    {
        if (null == frustum)
        {
            throw new ArgumentNullException("frustum", FrameworkResources.NullNotAllowed);
        }
        return frustum.Intersects(this);
    }

    /// <summary>Checks whether the current BoundingBox intersects a Plane. Reference page contains links to related code samples.</summary>
    /// <param name="plane">The Plane to check for intersection with.</param>
    public PlaneIntersectionType Intersects(Plane plane)
    {
        Vector3 vector;
        Vector3 vector2;
        vector2.X = (plane.Normal.X >= 0f) ? this.Min.X : this.Max.X;
        vector2.Y = (plane.Normal.Y >= 0f) ? this.Min.Y : this.Max.Y;
        vector2.Z = (plane.Normal.Z >= 0f) ? this.Min.Z : this.Max.Z;
        vector.X = (plane.Normal.X >= 0f) ? this.Max.X : this.Min.X;
        vector.Y = (plane.Normal.Y >= 0f) ? this.Max.Y : this.Min.Y;
        vector.Z = (plane.Normal.Z >= 0f) ? this.Max.Z : this.Min.Z;
        float num = ((plane.Normal.X * vector2.X) + (plane.Normal.Y * vector2.Y)) + (plane.Normal.Z * vector2.Z);
        if ((num + plane.D) > 0f)
        {
            return PlaneIntersectionType.Front;
        }
        num = ((plane.Normal.X * vector.X) + (plane.Normal.Y * vector.Y)) + (plane.Normal.Z * vector.Z);
        if ((num + plane.D) < 0f)
        {
            return PlaneIntersectionType.Back;
        }
        return PlaneIntersectionType.Intersecting;
    }

    /// <summary>Checks whether the current BoundingBox intersects a Plane. Reference page contains links to related code samples.</summary>
    /// <param name="plane">The Plane to check for intersection with.</param>
    /// <param name="result">[OutAttribute] An enumeration indicating whether the BoundingBox intersects the Plane.</param>
    public void Intersects(ref Plane plane, out PlaneIntersectionType result)
    {
        Vector3 vector;
        Vector3 vector2;
        vector2.X = (plane.Normal.X >= 0f) ? this.Min.X : this.Max.X;
        vector2.Y = (plane.Normal.Y >= 0f) ? this.Min.Y : this.Max.Y;
        vector2.Z = (plane.Normal.Z >= 0f) ? this.Min.Z : this.Max.Z;
        vector.X = (plane.Normal.X >= 0f) ? this.Max.X : this.Min.X;
        vector.Y = (plane.Normal.Y >= 0f) ? this.Max.Y : this.Min.Y;
        vector.Z = (plane.Normal.Z >= 0f) ? this.Max.Z : this.Min.Z;
        float num = ((plane.Normal.X * vector2.X) + (plane.Normal.Y * vector2.Y)) + (plane.Normal.Z * vector2.Z);
        if ((num + plane.D) > 0f)
        {
            result = PlaneIntersectionType.Front;
        }
        else
        {
            num = ((plane.Normal.X * vector.X) + (plane.Normal.Y * vector.Y)) + (plane.Normal.Z * vector.Z);
            if ((num + plane.D) < 0f)
            {
                result = PlaneIntersectionType.Back;
            }
            else
            {
                result = PlaneIntersectionType.Intersecting;
            }
        }
    }

    /// <summary>Checks whether the current BoundingBox intersects a Ray. Reference page contains links to related code samples.</summary>
    /// <param name="ray">The Ray to check for intersection with.</param>
    public float? Intersects(Ray ray)
    {
        float num = 0f;
        float maxValue = float.MaxValue;
        if (Math.Abs(ray.Direction.X) < 1E-06f)
        {
            if ((ray.Position.X < this.Min.X) || (ray.Position.X > this.Max.X))
            {
                return null;
            }
        }
        else
        {
            float num11 = 1f / ray.Direction.X;
            float num8 = (this.Min.X - ray.Position.X) * num11;
            float num7 = (this.Max.X - ray.Position.X) * num11;
            if (num8 > num7)
            {
                float num14 = num8;
                num8 = num7;
                num7 = num14;
            }
            num = MathHelper.Max(num8, num);
            maxValue = MathHelper.Min(num7, maxValue);
            if (num > maxValue)
            {
                return null;
            }
        }
        if (Math.Abs(ray.Direction.Y) < 1E-06f)
        {
            if ((ray.Position.Y < this.Min.Y) || (ray.Position.Y > this.Max.Y))
            {
                return null;
            }
        }
        else
        {
            float num10 = 1f / ray.Direction.Y;
            float num6 = (this.Min.Y - ray.Position.Y) * num10;
            float num5 = (this.Max.Y - ray.Position.Y) * num10;
            if (num6 > num5)
            {
                float num13 = num6;
                num6 = num5;
                num5 = num13;
            }
            num = MathHelper.Max(num6, num);
            maxValue = MathHelper.Min(num5, maxValue);
            if (num > maxValue)
            {
                return null;
            }
        }
        if (Math.Abs(ray.Direction.Z) < 1E-06f)
        {
            if ((ray.Position.Z < this.Min.Z) || (ray.Position.Z > this.Max.Z))
            {
                return null;
            }
        }
        else
        {
            float num9 = 1f / ray.Direction.Z;
            float num4 = (this.Min.Z - ray.Position.Z) * num9;
            float num3 = (this.Max.Z - ray.Position.Z) * num9;
            if (num4 > num3)
            {
                float num12 = num4;
                num4 = num3;
                num3 = num12;
            }
            num = MathHelper.Max(num4, num);
            maxValue = MathHelper.Min(num3, maxValue);
            if (num > maxValue)
            {
                return null;
            }
        }
        return new float?(num);
    }

    /// <summary>Checks whether the current BoundingBox intersects a Ray. Reference page contains links to related code samples.</summary>
    /// <param name="ray">The Ray to check for intersection with.</param>
    /// <param name="result">[OutAttribute] Distance at which the ray intersects the BoundingBox, or null if there is no intersection.</param>
    public void Intersects(ref Ray ray, out float? result)
    {
        result = 0;
        float num = 0f;
        float maxValue = float.MaxValue;
        if (Math.Abs(ray.Direction.X) < 1E-06f)
        {
            if ((ray.Position.X < this.Min.X) || (ray.Position.X > this.Max.X))
            {
                return;
            }
        }
        else
        {
            float num11 = 1f / ray.Direction.X;
            float num8 = (this.Min.X - ray.Position.X) * num11;
            float num7 = (this.Max.X - ray.Position.X) * num11;
            if (num8 > num7)
            {
                float num14 = num8;
                num8 = num7;
                num7 = num14;
            }
            num = MathHelper.Max(num8, num);
            maxValue = MathHelper.Min(num7, maxValue);
            if (num > maxValue)
            {
                return;
            }
        }
        if (Math.Abs(ray.Direction.Y) < 1E-06f)
        {
            if ((ray.Position.Y < this.Min.Y) || (ray.Position.Y > this.Max.Y))
            {
                return;
            }
        }
        else
        {
            float num10 = 1f / ray.Direction.Y;
            float num6 = (this.Min.Y - ray.Position.Y) * num10;
            float num5 = (this.Max.Y - ray.Position.Y) * num10;
            if (num6 > num5)
            {
                float num13 = num6;
                num6 = num5;
                num5 = num13;
            }
            num = MathHelper.Max(num6, num);
            maxValue = MathHelper.Min(num5, maxValue);
            if (num > maxValue)
            {
                return;
            }
        }
        if (Math.Abs(ray.Direction.Z) < 1E-06f)
        {
            if ((ray.Position.Z < this.Min.Z) || (ray.Position.Z > this.Max.Z))
            {
                return;
            }
        }
        else
        {
            float num9 = 1f / ray.Direction.Z;
            float num4 = (this.Min.Z - ray.Position.Z) * num9;
            float num3 = (this.Max.Z - ray.Position.Z) * num9;
            if (num4 > num3)
            {
                float num12 = num4;
                num4 = num3;
                num3 = num12;
            }
            num = MathHelper.Max(num4, num);
            maxValue = MathHelper.Min(num3, maxValue);
            if (num > maxValue)
            {
                return;
            }
        }
        result = new float?(num);
    }

    /// <summary>Checks whether the current BoundingBox intersects a BoundingSphere. Reference page contains links to related code samples.</summary>
    /// <param name="sphere">The BoundingSphere to check for intersection with.</param>
    public bool Intersects(BoundingSphere sphere)
    {
        float num;
        Vector3 vector;
        Vector3.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out vector);
        Vector3.DistanceSquared(ref sphere.Center, ref vector, out num);
        return (num <= (sphere.Radius * sphere.Radius));
    }

    /// <summary>Checks whether the current BoundingBox intersects a BoundingSphere. Reference page contains links to related code samples.</summary>
    /// <param name="sphere">The BoundingSphere to check for intersection with.</param>
    /// <param name="result">[OutAttribute] true if the BoundingBox and BoundingSphere intersect; false otherwise.</param>
    public void Intersects(ref BoundingSphere sphere, out bool result)
    {
        float num;
        Vector3 vector;
        Vector3.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out vector);
        Vector3.DistanceSquared(ref sphere.Center, ref vector, out num);
        result = num <= (sphere.Radius * sphere.Radius);
    }

    /// <summary>Tests whether the BoundingBox contains another BoundingBox.</summary>
    /// <param name="box">The BoundingBox to test for overlap.</param>
    public ContainmentType Contains(BoundingBox box)
    {
        if ((this.Max.X < box.Min.X) || (this.Min.X > box.Max.X))
        {
            return ContainmentType.Disjoint;
        }
        if ((this.Max.Y < box.Min.Y) || (this.Min.Y > box.Max.Y))
        {
            return ContainmentType.Disjoint;
        }
        if ((this.Max.Z < box.Min.Z) || (this.Min.Z > box.Max.Z))
        {
            return ContainmentType.Disjoint;
        }
        if ((((this.Min.X <= box.Min.X) && (box.Max.X <= this.Max.X)) && ((this.Min.Y <= box.Min.Y) && (box.Max.Y <= this.Max.Y))) && ((this.Min.Z <= box.Min.Z) && (box.Max.Z <= this.Max.Z)))
        {
            return ContainmentType.Contains;
        }
        return ContainmentType.Intersects;
    }

    /// <summary>Tests whether the BoundingBox contains a BoundingBox.</summary>
    /// <param name="box">The BoundingBox to test for overlap.</param>
    /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
    public void Contains(ref BoundingBox box, out ContainmentType result)
    {
        result = ContainmentType.Disjoint;
        if ((((this.Max.X >= box.Min.X) && (this.Min.X <= box.Max.X)) && ((this.Max.Y >= box.Min.Y) && (this.Min.Y <= box.Max.Y))) && ((this.Max.Z >= box.Min.Z) && (this.Min.Z <= box.Max.Z)))
        {
            result = ((((this.Min.X <= box.Min.X) && (box.Max.X <= this.Max.X)) && ((this.Min.Y <= box.Min.Y) && (box.Max.Y <= this.Max.Y))) && ((this.Min.Z <= box.Min.Z) && (box.Max.Z <= this.Max.Z))) ? ContainmentType.Contains : ContainmentType.Intersects;
        }
    }

    /// <summary>Tests whether the BoundingBox contains a BoundingFrustum.</summary>
    /// <param name="frustum">The BoundingFrustum to test for overlap.</param>
    public ContainmentType Contains(BoundingFrustum frustum)
    {
        if (null == frustum)
        {
            throw new ArgumentNullException("frustum", FrameworkResources.NullNotAllowed);
        }
        if (!frustum.Intersects(this))
        {
            return ContainmentType.Disjoint;
        }
        foreach (Vector3 vector in frustum.cornerArray)
        {
            if (this.Contains(vector) == ContainmentType.Disjoint)
            {
                return ContainmentType.Intersects;
            }
        }
        return ContainmentType.Contains;
    }

    /// <summary>Tests whether the BoundingBox contains a point.</summary>
    /// <param name="point">The point to test for overlap.</param>
    public ContainmentType Contains(Vector3 point)
    {
        if ((((this.Min.X <= point.X) && (point.X <= this.Max.X)) && ((this.Min.Y <= point.Y) && (point.Y <= this.Max.Y))) && ((this.Min.Z <= point.Z) && (point.Z <= this.Max.Z)))
        {
            return ContainmentType.Contains;
        }
        return ContainmentType.Disjoint;
    }

    /// <summary>Tests whether the BoundingBox contains a point.</summary>
    /// <param name="point">The point to test for overlap.</param>
    /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
    public void Contains(ref Vector3 point, out ContainmentType result)
    {
        result = ((((this.Min.X <= point.X) && (point.X <= this.Max.X)) && ((this.Min.Y <= point.Y) && (point.Y <= this.Max.Y))) && ((this.Min.Z <= point.Z) && (point.Z <= this.Max.Z))) ? ContainmentType.Contains : ContainmentType.Disjoint;
    }

    /// <summary>Tests whether the BoundingBox contains a BoundingSphere.</summary>
    /// <param name="sphere">The BoundingSphere to test for overlap.</param>
    public ContainmentType Contains(BoundingSphere sphere)
    {
        float num2;
        Vector3 vector;
        Vector3.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out vector);
        Vector3.DistanceSquared(ref sphere.Center, ref vector, out num2);
        float radius = sphere.Radius;
        if (num2 > (radius * radius))
        {
            return ContainmentType.Disjoint;
        }
        if (((((this.Min.X + radius) <= sphere.Center.X) && (sphere.Center.X <= (this.Max.X - radius))) && (((this.Max.X - this.Min.X) > radius) && ((this.Min.Y + radius) <= sphere.Center.Y))) && (((sphere.Center.Y <= (this.Max.Y - radius)) && ((this.Max.Y - this.Min.Y) > radius)) && ((((this.Min.Z + radius) <= sphere.Center.Z) && (sphere.Center.Z <= (this.Max.Z - radius))) && ((this.Max.X - this.Min.X) > radius))))
        {
            return ContainmentType.Contains;
        }
        return ContainmentType.Intersects;
    }

    /// <summary>Tests whether the BoundingBox contains a BoundingSphere.</summary>
    /// <param name="sphere">The BoundingSphere to test for overlap.</param>
    /// <param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
    public void Contains(ref BoundingSphere sphere, out ContainmentType result)
    {
        float num2;
        Vector3 vector;
        Vector3.Clamp(ref sphere.Center, ref this.Min, ref this.Max, out vector);
        Vector3.DistanceSquared(ref sphere.Center, ref vector, out num2);
        float radius = sphere.Radius;
        if (num2 > (radius * radius))
        {
            result = ContainmentType.Disjoint;
        }
        else
        {
            result = (((((this.Min.X + radius) <= sphere.Center.X) && (sphere.Center.X <= (this.Max.X - radius))) && (((this.Max.X - this.Min.X) > radius) && ((this.Min.Y + radius) <= sphere.Center.Y))) && (((sphere.Center.Y <= (this.Max.Y - radius)) && ((this.Max.Y - this.Min.Y) > radius)) && ((((this.Min.Z + radius) <= sphere.Center.Z) && (sphere.Center.Z <= (this.Max.Z - radius))) && ((this.Max.X - this.Min.X) > radius)))) ? ContainmentType.Contains : ContainmentType.Intersects;
        }
    }

    internal void SupportMapping(ref Vector3 v, out Vector3 result)
    {
        result.X = (v.X >= 0f) ? this.Max.X : this.Min.X;
        result.Y = (v.Y >= 0f) ? this.Max.Y : this.Min.Y;
        result.Z = (v.Z >= 0f) ? this.Max.Z : this.Min.Z;
    }

    /// <summary>Determines whether two instances of BoundingBox are equal.</summary>
    /// <param name="a">BoundingBox to compare.</param>
    /// <param name="b">BoundingBox to compare.</param>
    public static bool operator ==(BoundingBox a, BoundingBox b)
    {
        return a.Equals(b);
    }

    /// <summary>Determines whether two instances of BoundingBox are not equal.</summary>
    /// <param name="a">The object to the left of the inequality operator.</param>
    /// <param name="b">The object to the right of the inequality operator.</param>
    public static bool operator !=(BoundingBox a, BoundingBox b)
    {
        if (!(a.Min != b.Min))
        {
            return (a.Max != b.Max);
        }
        return true;
    }
}
}
