using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Design;

namespace Microsoft.Xna.Framework
{/// <summary>Defines a vector with three components.</summary>
[Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(Vector3Converter))]
public struct Vector3 : IEquatable<Vector3>
{
    /// <summary>Gets or sets the x-component of the vector.</summary>
    public float X;
    /// <summary>Gets or sets the y-component of the vector.</summary>
    public float Y;
    /// <summary>Gets or sets the z-component of the vector.</summary>
    public float Z;
    private static Vector3 _zero;
    private static Vector3 _one;
    private static Vector3 _unitX;
    private static Vector3 _unitY;
    private static Vector3 _unitZ;
    private static Vector3 _up;
    private static Vector3 _down;
    private static Vector3 _right;
    private static Vector3 _left;
    private static Vector3 _forward;
    private static Vector3 _backward;
    /// <summary>Returns a Vector3 with all of its components set to zero.</summary>
    public static Vector3 Zero
    {
        get
        {
            return _zero;
        }
    }
    /// <summary>Returns a Vector3 with ones in all of its components.</summary>
    public static Vector3 One
    {
        get
        {
            return _one;
        }
    }
    /// <summary>Returns the x unit Vector3 (1, 0, 0).</summary>
    public static Vector3 UnitX
    {
        get
        {
            return _unitX;
        }
    }
    /// <summary>Returns the y unit Vector3 (0, 1, 0).</summary>
    public static Vector3 UnitY
    {
        get
        {
            return _unitY;
        }
    }
    /// <summary>Returns the z unit Vector3 (0, 0, 1).</summary>
    public static Vector3 UnitZ
    {
        get
        {
            return _unitZ;
        }
    }
    /// <summary>Returns a unit vector designating up (0, 1, 0).</summary>
    public static Vector3 Up
    {
        get
        {
            return _up;
        }
    }
    /// <summary>Returns a unit Vector3 designating down (0, −1, 0).</summary>
    public static Vector3 Down
    {
        get
        {
            return _down;
        }
    }
    /// <summary>Returns a unit Vector3 pointing to the right (1, 0, 0).</summary>
    public static Vector3 Right
    {
        get
        {
            return _right;
        }
    }
    /// <summary>Returns a unit Vector3 designating left (−1, 0, 0).</summary>
    public static Vector3 Left
    {
        get
        {
            return _left;
        }
    }
    /// <summary>Returns a unit Vector3 designating forward in a right-handed coordinate system(0, 0, −1).</summary>
    public static Vector3 Forward
    {
        get
        {
            return _forward;
        }
    }
    /// <summary>Returns a unit Vector3 designating backward in a right-handed coordinate system (0, 0, 1).</summary>
    public static Vector3 Backward
    {
        get
        {
            return _backward;
        }
    }
    /// <summary>Initializes a new instance of Vector3.</summary>
    /// <param name="x">Initial value for the x-component of the vector.</param>
    /// <param name="y">Initial value for the y-component of the vector.</param>
    /// <param name="z">Initial value for the z-component of the vector.</param>
    public Vector3(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    /// <summary>Creates a new instance of Vector3.</summary>
    /// <param name="value">Value to initialize each component to.</param>
    public Vector3(float value)
    {
        this.X = this.Y = this.Z = value;
    }

    /// <summary>Initializes a new instance of Vector3.</summary>
    /// <param name="value">A vector containing the values to initialize x and y components with.</param>
    /// <param name="z">Initial value for the z-component of the vector.</param>
    public Vector3(Vector2 value, float z)
    {
        this.X = value.X;
        this.Y = value.Y;
        this.Z = z;
    }

    /// <summary>Retrieves a string representation of the current object.</summary>
    public override string ToString()
    {
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture), this.Z.ToString(currentCulture) });
    }

    /// <summary>Determines whether the specified Object is equal to the Vector3.</summary>
    /// <param name="other">The Vector3 to compare with the current Vector3.</param>
    public bool Equals(Vector3 other)
    {
        return (((this.X == other.X) && (this.Y == other.Y)) && (this.Z == other.Z));
    }

    /// <summary>Returns a value that indicates whether the current instance is equal to a specified object.</summary>
    /// <param name="obj">Object to make the comparison with.</param>
    public override bool Equals(object obj)
    {
        bool flag = false;
        if (obj is Vector3)
        {
            flag = this.Equals((Vector3) obj);
        }
        return flag;
    }

    /// <summary>Gets the hash code of the vector object.</summary>
    public override int GetHashCode()
    {
        return ((this.X.GetHashCode() + this.Y.GetHashCode()) + this.Z.GetHashCode());
    }

    /// <summary>Calculates the length of the vector.</summary>
    public float Length()
    {
        float num = ((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z);
        return (float) Math.Sqrt((double) num);
    }

    /// <summary>Calculates the length of the vector squared.</summary>
    public float LengthSquared()
    {
        return (((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z));
    }

    /// <summary>Calculates the distance between two vectors.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static float Distance(Vector3 value1, Vector3 value2)
    {
        float num3 = value1.X - value2.X;
        float num2 = value1.Y - value2.Y;
        float num = value1.Z - value2.Z;
        float num4 = ((num3 * num3) + (num2 * num2)) + (num * num);
        return (float) Math.Sqrt((double) num4);
    }

    /// <summary>Calculates the distance between two vectors.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    /// <param name="result">[OutAttribute] The distance between the vectors.</param>
    public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
    {
        float num3 = value1.X - value2.X;
        float num2 = value1.Y - value2.Y;
        float num = value1.Z - value2.Z;
        float num4 = ((num3 * num3) + (num2 * num2)) + (num * num);
        result = (float) Math.Sqrt((double) num4);
    }

    /// <summary>Calculates the distance between two vectors squared.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static float DistanceSquared(Vector3 value1, Vector3 value2)
    {
        float num3 = value1.X - value2.X;
        float num2 = value1.Y - value2.Y;
        float num = value1.Z - value2.Z;
        return (((num3 * num3) + (num2 * num2)) + (num * num));
    }

    /// <summary>Calculates the distance between two vectors squared.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    /// <param name="result">[OutAttribute] The distance between the two vectors squared.</param>
    public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out float result)
    {
        float num3 = value1.X - value2.X;
        float num2 = value1.Y - value2.Y;
        float num = value1.Z - value2.Z;
        result = ((num3 * num3) + (num2 * num2)) + (num * num);
    }

    /// <summary>Calculates the dot product of two vectors. If the two vectors are unit vectors, the dot product returns a floating point value between -1 and 1 that can be used to determine some properties of the angle between two vectors. For example, it can show whether the vectors are orthogonal, parallel, or have an acute or obtuse angle between them.</summary>
    /// <param name="vector1">Source vector.</param>
    /// <param name="vector2">Source vector.</param>
    public static float Dot(Vector3 vector1, Vector3 vector2)
    {
        return (((vector1.X * vector2.X) + (vector1.Y * vector2.Y)) + (vector1.Z * vector2.Z));
    }

    /// <summary>Calculates the dot product of two vectors and writes the result to a user-specified variable. If the two vectors are unit vectors, the dot product returns a floating point value between -1 and 1 that can be used to determine some properties of the angle between two vectors. For example, it can show whether the vectors are orthogonal, parallel, or have an acute or obtuse angle between them.</summary>
    /// <param name="vector1">Source vector.</param>
    /// <param name="vector2">Source vector.</param>
    /// <param name="result">[OutAttribute] The dot product of the two vectors.</param>
    public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out float result)
    {
        result = ((vector1.X * vector2.X) + (vector1.Y * vector2.Y)) + (vector1.Z * vector2.Z);
    }

    /// <summary>Turns the current vector into a unit vector. The result is a vector one unit in length pointing in the same direction as the original vector.</summary>
    public void Normalize()
    {
        float num2 = ((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z);
        float num = 1f / ((float) Math.Sqrt((double) num2));
        this.X *= num;
        this.Y *= num;
        this.Z *= num;
    }

    /// <summary>Creates a unit vector from the specified vector. The result is a vector one unit in length pointing in the same direction as the original vector.</summary>
    /// <param name="value">The source Vector3.</param>
    public static Vector3 Normalize(Vector3 value)
    {
        Vector3 vector;
        float num2 = ((value.X * value.X) + (value.Y * value.Y)) + (value.Z * value.Z);
        float num = 1f / ((float) Math.Sqrt((double) num2));
        vector.X = value.X * num;
        vector.Y = value.Y * num;
        vector.Z = value.Z * num;
        return vector;
    }

    /// <summary>Creates a unit vector from the specified vector, writing the result to a user-specified variable. The result is a vector one unit in length pointing in the same direction as the original vector.</summary>
    /// <param name="value">Source vector.</param>
    /// <param name="result">[OutAttribute] The normalized vector.</param>
    public static void Normalize(ref Vector3 value, out Vector3 result)
    {
        float num2 = ((value.X * value.X) + (value.Y * value.Y)) + (value.Z * value.Z);
        float num = 1f / ((float) Math.Sqrt((double) num2));
        result.X = value.X * num;
        result.Y = value.Y * num;
        result.Z = value.Z * num;
    }

    /// <summary>Calculates the cross product of two vectors.</summary>
    /// <param name="vector1">Source vector.</param>
    /// <param name="vector2">Source vector.</param>
    public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
    {
        Vector3 vector;
        vector.X = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y);
        vector.Y = (vector1.Z * vector2.X) - (vector1.X * vector2.Z);
        vector.Z = (vector1.X * vector2.Y) - (vector1.Y * vector2.X);
        return vector;
    }

    /// <summary>Calculates the cross product of two vectors.</summary>
    /// <param name="vector1">Source vector.</param>
    /// <param name="vector2">Source vector.</param>
    /// <param name="result">[OutAttribute] The cross product of the vectors.</param>
    public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
    {
        float num3 = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y);
        float num2 = (vector1.Z * vector2.X) - (vector1.X * vector2.Z);
        float num = (vector1.X * vector2.Y) - (vector1.Y * vector2.X);
        result.X = num3;
        result.Y = num2;
        result.Z = num;
    }

    /// <summary>Returns the reflection of a vector off a surface that has the specified normal.  Reference page contains code sample.</summary>
    /// <param name="vector">Source vector.</param>
    /// <param name="normal">Normal of the surface.</param>
    public static Vector3 Reflect(Vector3 vector, Vector3 normal)
    {
        Vector3 vector2;
        float num = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);
        vector2.X = vector.X - ((2f * num) * normal.X);
        vector2.Y = vector.Y - ((2f * num) * normal.Y);
        vector2.Z = vector.Z - ((2f * num) * normal.Z);
        return vector2;
    }

    /// <summary>Returns the reflection of a vector off a surface that has the specified normal.  Reference page contains code sample.</summary>
    /// <param name="vector">Source vector.</param>
    /// <param name="normal">Normal of the surface.</param>
    /// <param name="result">[OutAttribute] The reflected vector.</param>
    public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
    {
        float num = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);
        result.X = vector.X - ((2f * num) * normal.X);
        result.Y = vector.Y - ((2f * num) * normal.Y);
        result.Z = vector.Z - ((2f * num) * normal.Z);
    }

    /// <summary>Returns a vector that contains the lowest value from each matching pair of components.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static Vector3 Min(Vector3 value1, Vector3 value2)
    {
        Vector3 vector;
        vector.X = (value1.X < value2.X) ? value1.X : value2.X;
        vector.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
        vector.Z = (value1.Z < value2.Z) ? value1.Z : value2.Z;
        return vector;
    }

    /// <summary>Returns a vector that contains the lowest value from each matching pair of components.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    /// <param name="result">[OutAttribute] The minimized vector.</param>
    public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
        result.X = (value1.X < value2.X) ? value1.X : value2.X;
        result.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
        result.Z = (value1.Z < value2.Z) ? value1.Z : value2.Z;
    }

    /// <summary>Returns a vector that contains the highest value from each matching pair of components.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static Vector3 Max(Vector3 value1, Vector3 value2)
    {
        Vector3 vector;
        vector.X = (value1.X > value2.X) ? value1.X : value2.X;
        vector.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
        vector.Z = (value1.Z > value2.Z) ? value1.Z : value2.Z;
        return vector;
    }

    /// <summary>Returns a vector that contains the highest value from each matching pair of components.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    /// <param name="result">[OutAttribute] The maximized vector.</param>
    public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
        result.X = (value1.X > value2.X) ? value1.X : value2.X;
        result.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
        result.Z = (value1.Z > value2.Z) ? value1.Z : value2.Z;
    }

    /// <summary>Restricts a value to be within a specified range.</summary>
    /// <param name="value1">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
    {
        Vector3 vector;
        float x = value1.X;
        x = (x > max.X) ? max.X : x;
        x = (x < min.X) ? min.X : x;
        float y = value1.Y;
        y = (y > max.Y) ? max.Y : y;
        y = (y < min.Y) ? min.Y : y;
        float z = value1.Z;
        z = (z > max.Z) ? max.Z : z;
        z = (z < min.Z) ? min.Z : z;
        vector.X = x;
        vector.Y = y;
        vector.Z = z;
        return vector;
    }

    /// <summary>Restricts a value to be within a specified range.</summary>
    /// <param name="value1">The value to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="result">[OutAttribute] The clamped value.</param>
    public static void Clamp(ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result)
    {
        float x = value1.X;
        x = (x > max.X) ? max.X : x;
        x = (x < min.X) ? min.X : x;
        float y = value1.Y;
        y = (y > max.Y) ? max.Y : y;
        y = (y < min.Y) ? min.Y : y;
        float z = value1.Z;
        z = (z > max.Z) ? max.Z : z;
        z = (z < min.Z) ? min.Z : z;
        result.X = x;
        result.Y = y;
        result.Z = z;
    }

    /// <summary>Performs a linear interpolation between two vectors.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
    public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
    {
        Vector3 vector;
        vector.X = value1.X + ((value2.X - value1.X) * amount);
        vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
        vector.Z = value1.Z + ((value2.Z - value1.Z) * amount);
        return vector;
    }

    /// <summary>Performs a linear interpolation between two vectors.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
    /// <param name="result">[OutAttribute] The result of the interpolation.</param>
    public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
    {
        result.X = value1.X + ((value2.X - value1.X) * amount);
        result.Y = value1.Y + ((value2.Y - value1.Y) * amount);
        result.Z = value1.Z + ((value2.Z - value1.Z) * amount);
    }

    /// <summary>Returns a Vector3 containing the 3D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 3D triangle.</summary>
    /// <param name="value1">A Vector3 containing the 3D Cartesian coordinates of vertex 1 of the triangle.</param>
    /// <param name="value2">A Vector3 containing the 3D Cartesian coordinates of vertex 2 of the triangle.</param>
    /// <param name="value3">A Vector3 containing the 3D Cartesian coordinates of vertex 3 of the triangle.</param>
    /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param>
    /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
    public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
    {
        Vector3 vector;
        vector.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
        vector.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
        vector.Z = (value1.Z + (amount1 * (value2.Z - value1.Z))) + (amount2 * (value3.Z - value1.Z));
        return vector;
    }

    /// <summary>Returns a Vector3 containing the 3D Cartesian coordinates of a point specified in barycentric (areal) coordinates relative to a 3D triangle.</summary>
    /// <param name="value1">A Vector3 containing the 3D Cartesian coordinates of vertex 1 of the triangle.</param>
    /// <param name="value2">A Vector3 containing the 3D Cartesian coordinates of vertex 2 of the triangle.</param>
    /// <param name="value3">A Vector3 containing the 3D Cartesian coordinates of vertex 3 of the triangle.</param>
    /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in value2).</param>
    /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in value3).</param>
    /// <param name="result">[OutAttribute] The 3D Cartesian coordinates of the specified point are placed in this Vector3 on exit.</param>
    public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result)
    {
        result.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
        result.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
        result.Z = (value1.Z + (amount1 * (value2.Z - value1.Z))) + (amount2 * (value3.Z - value1.Z));
    }

    /// <summary>Interpolates between two values using a cubic equation.</summary>
    /// <param name="value1">Source value.</param>
    /// <param name="value2">Source value.</param>
    /// <param name="amount">Weighting value.</param>
    public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, float amount)
    {
        Vector3 vector;
        amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
        amount = (amount * amount) * (3f - (2f * amount));
        vector.X = value1.X + ((value2.X - value1.X) * amount);
        vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
        vector.Z = value1.Z + ((value2.Z - value1.Z) * amount);
        return vector;
    }

    /// <summary>Interpolates between two values using a cubic equation.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    /// <param name="amount">Weighting value.</param>
    /// <param name="result">[OutAttribute] The interpolated value.</param>
    public static void SmoothStep(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
    {
        amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
        amount = (amount * amount) * (3f - (2f * amount));
        result.X = value1.X + ((value2.X - value1.X) * amount);
        result.Y = value1.Y + ((value2.Y - value1.Y) * amount);
        result.Z = value1.Z + ((value2.Z - value1.Z) * amount);
    }

    /// <summary>Performs a Catmull-Rom interpolation using the specified positions.</summary>
    /// <param name="value1">The first position in the interpolation.</param>
    /// <param name="value2">The second position in the interpolation.</param>
    /// <param name="value3">The third position in the interpolation.</param>
    /// <param name="value4">The fourth position in the interpolation.</param>
    /// <param name="amount">Weighting factor.</param>
    public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
    {
        Vector3 vector;
        float num = amount * amount;
        float num2 = amount * num;
        vector.X = 0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) + (((((2f * value1.X) - (5f * value2.X)) + (4f * value3.X)) - value4.X) * num)) + ((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * num2));
        vector.Y = 0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + (((((2f * value1.Y) - (5f * value2.Y)) + (4f * value3.Y)) - value4.Y) * num)) + ((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * num2));
        vector.Z = 0.5f * ((((2f * value2.Z) + ((-value1.Z + value3.Z) * amount)) + (((((2f * value1.Z) - (5f * value2.Z)) + (4f * value3.Z)) - value4.Z) * num)) + ((((-value1.Z + (3f * value2.Z)) - (3f * value3.Z)) + value4.Z) * num2));
        return vector;
    }

    /// <summary>Performs a Catmull-Rom interpolation using the specified positions.</summary>
    /// <param name="value1">The first position in the interpolation.</param>
    /// <param name="value2">The second position in the interpolation.</param>
    /// <param name="value3">The third position in the interpolation.</param>
    /// <param name="value4">The fourth position in the interpolation.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <param name="result">[OutAttribute] A vector that is the result of the Catmull-Rom interpolation.</param>
    public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result)
    {
        float num = amount * amount;
        float num2 = amount * num;
        result.X = 0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) + (((((2f * value1.X) - (5f * value2.X)) + (4f * value3.X)) - value4.X) * num)) + ((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * num2));
        result.Y = 0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + (((((2f * value1.Y) - (5f * value2.Y)) + (4f * value3.Y)) - value4.Y) * num)) + ((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * num2));
        result.Z = 0.5f * ((((2f * value2.Z) + ((-value1.Z + value3.Z) * amount)) + (((((2f * value1.Z) - (5f * value2.Z)) + (4f * value3.Z)) - value4.Z) * num)) + ((((-value1.Z + (3f * value2.Z)) - (3f * value3.Z)) + value4.Z) * num2));
    }

    /// <summary>Performs a Hermite spline interpolation.</summary>
    /// <param name="value1">Source position vector.</param>
    /// <param name="tangent1">Source tangent vector.</param>
    /// <param name="value2">Source position vector.</param>
    /// <param name="tangent2">Source tangent vector.</param>
    /// <param name="amount">Weighting factor.</param>
    public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
    {
        Vector3 vector;
        float num = amount * amount;
        float num2 = amount * num;
        float num6 = ((2f * num2) - (3f * num)) + 1f;
        float num5 = (-2f * num2) + (3f * num);
        float num4 = (num2 - (2f * num)) + amount;
        float num3 = num2 - num;
        vector.X = (((value1.X * num6) + (value2.X * num5)) + (tangent1.X * num4)) + (tangent2.X * num3);
        vector.Y = (((value1.Y * num6) + (value2.Y * num5)) + (tangent1.Y * num4)) + (tangent2.Y * num3);
        vector.Z = (((value1.Z * num6) + (value2.Z * num5)) + (tangent1.Z * num4)) + (tangent2.Z * num3);
        return vector;
    }

    /// <summary>Performs a Hermite spline interpolation.</summary>
    /// <param name="value1">Source position vector.</param>
    /// <param name="tangent1">Source tangent vector.</param>
    /// <param name="value2">Source position vector.</param>
    /// <param name="tangent2">Source tangent vector.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <param name="result">[OutAttribute] The result of the Hermite spline interpolation.</param>
    public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
    {
        float num = amount * amount;
        float num2 = amount * num;
        float num6 = ((2f * num2) - (3f * num)) + 1f;
        float num5 = (-2f * num2) + (3f * num);
        float num4 = (num2 - (2f * num)) + amount;
        float num3 = num2 - num;
        result.X = (((value1.X * num6) + (value2.X * num5)) + (tangent1.X * num4)) + (tangent2.X * num3);
        result.Y = (((value1.Y * num6) + (value2.Y * num5)) + (tangent1.Y * num4)) + (tangent2.Y * num3);
        result.Z = (((value1.Z * num6) + (value2.Z * num5)) + (tangent1.Z * num4)) + (tangent2.Z * num3);
    }

    /// <summary>Transforms a 3D vector by the given matrix.</summary>
    /// <param name="position">The source vector.</param>
    /// <param name="matrix">The transformation matrix.</param>
    public static Vector3 Transform(Vector3 position, Matrix matrix)
    {
        Vector3 vector;
        float num3 = (((position.X * matrix.M11) + (position.Y * matrix.M21)) + (position.Z * matrix.M31)) + matrix.M41;
        float num2 = (((position.X * matrix.M12) + (position.Y * matrix.M22)) + (position.Z * matrix.M32)) + matrix.M42;
        float num = (((position.X * matrix.M13) + (position.Y * matrix.M23)) + (position.Z * matrix.M33)) + matrix.M43;
        vector.X = num3;
        vector.Y = num2;
        vector.Z = num;
        return vector;
    }

    /// <summary>Transforms a Vector3 by the given Matrix.</summary>
    /// <param name="position">The source Vector3.</param>
    /// <param name="matrix">The transformation Matrix.</param>
    /// <param name="result">[OutAttribute] The transformed vector.</param>
    public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector3 result)
    {
        float num3 = (((position.X * matrix.M11) + (position.Y * matrix.M21)) + (position.Z * matrix.M31)) + matrix.M41;
        float num2 = (((position.X * matrix.M12) + (position.Y * matrix.M22)) + (position.Z * matrix.M32)) + matrix.M42;
        float num = (((position.X * matrix.M13) + (position.Y * matrix.M23)) + (position.Z * matrix.M33)) + matrix.M43;
        result.X = num3;
        result.Y = num2;
        result.Z = num;
    }

    /// <summary>Transforms a 3D vector normal by a matrix.</summary>
    /// <param name="normal">The source vector.</param>
    /// <param name="matrix">The transformation matrix.</param>
    public static Vector3 TransformNormal(Vector3 normal, Matrix matrix)
    {
        Vector3 vector;
        float num3 = ((normal.X * matrix.M11) + (normal.Y * matrix.M21)) + (normal.Z * matrix.M31);
        float num2 = ((normal.X * matrix.M12) + (normal.Y * matrix.M22)) + (normal.Z * matrix.M32);
        float num = ((normal.X * matrix.M13) + (normal.Y * matrix.M23)) + (normal.Z * matrix.M33);
        vector.X = num3;
        vector.Y = num2;
        vector.Z = num;
        return vector;
    }

    /// <summary>Transforms a vector normal by a matrix.</summary>
    /// <param name="normal">The source vector.</param>
    /// <param name="matrix">The transformation Matrix.</param>
    /// <param name="result">[OutAttribute] The Vector3 resulting from the transformation.</param>
    public static void TransformNormal(ref Vector3 normal, ref Matrix matrix, out Vector3 result)
    {
        float num3 = ((normal.X * matrix.M11) + (normal.Y * matrix.M21)) + (normal.Z * matrix.M31);
        float num2 = ((normal.X * matrix.M12) + (normal.Y * matrix.M22)) + (normal.Z * matrix.M32);
        float num = ((normal.X * matrix.M13) + (normal.Y * matrix.M23)) + (normal.Z * matrix.M33);
        result.X = num3;
        result.Y = num2;
        result.Z = num;
    }

    /// <summary>Transforms a Vector3 by a specified Quaternion rotation.</summary>
    /// <param name="value">The Vector3 to rotate.</param>
    /// <param name="rotation">The Quaternion rotation to apply.</param>
    public static Vector3 Transform(Vector3 value, Quaternion rotation)
    {
        Vector3 vector;
        float num12 = rotation.X + rotation.X;
        float num2 = rotation.Y + rotation.Y;
        float num = rotation.Z + rotation.Z;
        float num11 = rotation.W * num12;
        float num10 = rotation.W * num2;
        float num9 = rotation.W * num;
        float num8 = rotation.X * num12;
        float num7 = rotation.X * num2;
        float num6 = rotation.X * num;
        float num5 = rotation.Y * num2;
        float num4 = rotation.Y * num;
        float num3 = rotation.Z * num;
        float num15 = ((value.X * ((1f - num5) - num3)) + (value.Y * (num7 - num9))) + (value.Z * (num6 + num10));
        float num14 = ((value.X * (num7 + num9)) + (value.Y * ((1f - num8) - num3))) + (value.Z * (num4 - num11));
        float num13 = ((value.X * (num6 - num10)) + (value.Y * (num4 + num11))) + (value.Z * ((1f - num8) - num5));
        vector.X = num15;
        vector.Y = num14;
        vector.Z = num13;
        return vector;
    }

    /// <summary>Transforms a Vector3 by a specified Quaternion rotation.</summary>
    /// <param name="value">The Vector3 to rotate.</param>
    /// <param name="rotation">The Quaternion rotation to apply.</param>
    /// <param name="result">[OutAttribute] An existing Vector3 filled in with the results of the rotation.</param>
    public static void Transform(ref Vector3 value, ref Quaternion rotation, out Vector3 result)
    {
        float num12 = rotation.X + rotation.X;
        float num2 = rotation.Y + rotation.Y;
        float num = rotation.Z + rotation.Z;
        float num11 = rotation.W * num12;
        float num10 = rotation.W * num2;
        float num9 = rotation.W * num;
        float num8 = rotation.X * num12;
        float num7 = rotation.X * num2;
        float num6 = rotation.X * num;
        float num5 = rotation.Y * num2;
        float num4 = rotation.Y * num;
        float num3 = rotation.Z * num;
        float num15 = ((value.X * ((1f - num5) - num3)) + (value.Y * (num7 - num9))) + (value.Z * (num6 + num10));
        float num14 = ((value.X * (num7 + num9)) + (value.Y * ((1f - num8) - num3))) + (value.Z * (num4 - num11));
        float num13 = ((value.X * (num6 - num10)) + (value.Y * (num4 + num11))) + (value.Z * ((1f - num8) - num5));
        result.X = num15;
        result.Y = num14;
        result.Z = num13;
    }

    /// <summary>Transforms a source array of Vector3s by a specified Matrix and writes the results to an existing destination array.</summary>
    /// <param name="sourceArray">The source array.</param>
    /// <param name="matrix">The transform Matrix to apply.</param>
    /// <param name="destinationArray">An existing destination array into which the transformed Vector3s are written.</param>
    public static void Transform(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
    {
        if (sourceArray == null)
        {
            throw new ArgumentNullException("sourceArray");
        }
        if (destinationArray == null)
        {
            throw new ArgumentNullException("destinationArray");
        }
        if (destinationArray.Length < sourceArray.Length)
        {
            throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        }
        for (int i = 0; i < sourceArray.Length; i++)
        {
            float x = sourceArray[i].X;
            float y = sourceArray[i].Y;
            float z = sourceArray[i].Z;
            destinationArray[i].X = (((x * matrix.M11) + (y * matrix.M21)) + (z * matrix.M31)) + matrix.M41;
            destinationArray[i].Y = (((x * matrix.M12) + (y * matrix.M22)) + (z * matrix.M32)) + matrix.M42;
            destinationArray[i].Z = (((x * matrix.M13) + (y * matrix.M23)) + (z * matrix.M33)) + matrix.M43;
        }
    }

    /// <summary>Applies a specified transform Matrix to a specified range of an array of Vector3s and writes the results into a specified range of a destination array.</summary>
    /// <param name="sourceArray">The source array.</param>
    /// <param name="sourceIndex">The index in the source array at which to start.</param>
    /// <param name="matrix">The transform Matrix to apply.</param>
    /// <param name="destinationArray">The existing destination array.</param>
    /// <param name="destinationIndex">The index in the destination array at which to start.</param>
    /// <param name="length">The number of Vector3s to transform.</param>
    public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
    {
        if (sourceArray == null)
        {
            throw new ArgumentNullException("sourceArray");
        }
        if (destinationArray == null)
        {
            throw new ArgumentNullException("destinationArray");
        }
        if (sourceArray.Length < (sourceIndex + length))
        {
            throw new ArgumentException(FrameworkResources.NotEnoughSourceSize);
        }
        if (destinationArray.Length < (destinationIndex + length))
        {
            throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        }
        while (length > 0)
        {
            float x = sourceArray[sourceIndex].X;
            float y = sourceArray[sourceIndex].Y;
            float z = sourceArray[sourceIndex].Z;
            destinationArray[destinationIndex].X = (((x * matrix.M11) + (y * matrix.M21)) + (z * matrix.M31)) + matrix.M41;
            destinationArray[destinationIndex].Y = (((x * matrix.M12) + (y * matrix.M22)) + (z * matrix.M32)) + matrix.M42;
            destinationArray[destinationIndex].Z = (((x * matrix.M13) + (y * matrix.M23)) + (z * matrix.M33)) + matrix.M43;
            sourceIndex++;
            destinationIndex++;
            length--;
        }
    }

    /// <summary>Transforms an array of 3D vector normals by a specified Matrix.</summary>
    /// <param name="sourceArray">The array of Vector3 normals to transform.</param>
    /// <param name="matrix">The transform matrix to apply.</param>
    /// <param name="destinationArray">An existing Vector3 array into which the results of the transforms are written.</param>
    public static void TransformNormal(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
    {
        if (sourceArray == null)
        {
            throw new ArgumentNullException("sourceArray");
        }
        if (destinationArray == null)
        {
            throw new ArgumentNullException("destinationArray");
        }
        if (destinationArray.Length < sourceArray.Length)
        {
            throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        }
        for (int i = 0; i < sourceArray.Length; i++)
        {
            float x = sourceArray[i].X;
            float y = sourceArray[i].Y;
            float z = sourceArray[i].Z;
            destinationArray[i].X = ((x * matrix.M11) + (y * matrix.M21)) + (z * matrix.M31);
            destinationArray[i].Y = ((x * matrix.M12) + (y * matrix.M22)) + (z * matrix.M32);
            destinationArray[i].Z = ((x * matrix.M13) + (y * matrix.M23)) + (z * matrix.M33);
        }
    }

    /// <summary>Transforms a specified range in an array of 3D vector normals by a specified Matrix and writes the results to a specified range in a destination array.</summary>
    /// <param name="sourceArray">The source array of Vector3 normals.</param>
    /// <param name="sourceIndex">The starting index in the source array.</param>
    /// <param name="matrix">The transform Matrix to apply.</param>
    /// <param name="destinationArray">The destination Vector3 array.</param>
    /// <param name="destinationIndex">The starting index in the destination array.</param>
    /// <param name="length">The number of vectors to transform.</param>
    public static void TransformNormal(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
    {
        if (sourceArray == null)
        {
            throw new ArgumentNullException("sourceArray");
        }
        if (destinationArray == null)
        {
            throw new ArgumentNullException("destinationArray");
        }
        if (sourceArray.Length < (sourceIndex + length))
        {
            throw new ArgumentException(FrameworkResources.NotEnoughSourceSize);
        }
        if (destinationArray.Length < (destinationIndex + length))
        {
            throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        }
        while (length > 0)
        {
            float x = sourceArray[sourceIndex].X;
            float y = sourceArray[sourceIndex].Y;
            float z = sourceArray[sourceIndex].Z;
            destinationArray[destinationIndex].X = ((x * matrix.M11) + (y * matrix.M21)) + (z * matrix.M31);
            destinationArray[destinationIndex].Y = ((x * matrix.M12) + (y * matrix.M22)) + (z * matrix.M32);
            destinationArray[destinationIndex].Z = ((x * matrix.M13) + (y * matrix.M23)) + (z * matrix.M33);
            sourceIndex++;
            destinationIndex++;
            length--;
        }
    }

    /// <summary>Transforms a source array of Vector3s by a specified Quaternion rotation and writes the results to an existing destination array.</summary>
    /// <param name="sourceArray">The source array.</param>
    /// <param name="rotation">The Quaternion rotation to apply.</param>
    /// <param name="destinationArray">An existing destination array into which the transformed Vector3s are written.</param>
    public static void Transform(Vector3[] sourceArray, ref Quaternion rotation, Vector3[] destinationArray)
    {
        if (sourceArray == null)
        {
            throw new ArgumentNullException("sourceArray");
        }
        if (destinationArray == null)
        {
            throw new ArgumentNullException("destinationArray");
        }
        if (destinationArray.Length < sourceArray.Length)
        {
            throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        }
        float num16 = rotation.X + rotation.X;
        float num6 = rotation.Y + rotation.Y;
        float num2 = rotation.Z + rotation.Z;
        float num15 = rotation.W * num16;
        float num14 = rotation.W * num6;
        float num13 = rotation.W * num2;
        float num12 = rotation.X * num16;
        float num11 = rotation.X * num6;
        float num10 = rotation.X * num2;
        float num9 = rotation.Y * num6;
        float num8 = rotation.Y * num2;
        float num7 = rotation.Z * num2;
        float num25 = (1f - num9) - num7;
        float num24 = num11 - num13;
        float num23 = num10 + num14;
        float num22 = num11 + num13;
        float num21 = (1f - num12) - num7;
        float num20 = num8 - num15;
        float num19 = num10 - num14;
        float num18 = num8 + num15;
        float num17 = (1f - num12) - num9;
        for (int i = 0; i < sourceArray.Length; i++)
        {
            float x = sourceArray[i].X;
            float y = sourceArray[i].Y;
            float z = sourceArray[i].Z;
            destinationArray[i].X = ((x * num25) + (y * num24)) + (z * num23);
            destinationArray[i].Y = ((x * num22) + (y * num21)) + (z * num20);
            destinationArray[i].Z = ((x * num19) + (y * num18)) + (z * num17);
        }
    }

    /// <summary>Applies a specified Quaternion rotation to a specified range of an array of Vector3s and writes the results into a specified range of a destination array.</summary>
    /// <param name="sourceArray">The source array.</param>
    /// <param name="sourceIndex">The index in the source array at which to start.</param>
    /// <param name="rotation">The Quaternion rotation to apply.</param>
    /// <param name="destinationArray">The existing destination array.</param>
    /// <param name="destinationIndex">The index in the destination array at which to start.</param>
    /// <param name="length">The number of Vector3s to transform.</param>
    public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector3[] destinationArray, int destinationIndex, int length)
    {
        if (sourceArray == null)
        {
            throw new ArgumentNullException("sourceArray");
        }
        if (destinationArray == null)
        {
            throw new ArgumentNullException("destinationArray");
        }
        if (sourceArray.Length < (sourceIndex + length))
        {
            throw new ArgumentException(FrameworkResources.NotEnoughSourceSize);
        }
        if (destinationArray.Length < (destinationIndex + length))
        {
            throw new ArgumentException(FrameworkResources.NotEnoughTargetSize);
        }
        float num15 = rotation.X + rotation.X;
        float num5 = rotation.Y + rotation.Y;
        float num = rotation.Z + rotation.Z;
        float num14 = rotation.W * num15;
        float num13 = rotation.W * num5;
        float num12 = rotation.W * num;
        float num11 = rotation.X * num15;
        float num10 = rotation.X * num5;
        float num9 = rotation.X * num;
        float num8 = rotation.Y * num5;
        float num7 = rotation.Y * num;
        float num6 = rotation.Z * num;
        float num24 = (1f - num8) - num6;
        float num23 = num10 - num12;
        float num22 = num9 + num13;
        float num21 = num10 + num12;
        float num20 = (1f - num11) - num6;
        float num19 = num7 - num14;
        float num18 = num9 - num13;
        float num17 = num7 + num14;
        float num16 = (1f - num11) - num8;
        while (length > 0)
        {
            float x = sourceArray[sourceIndex].X;
            float y = sourceArray[sourceIndex].Y;
            float z = sourceArray[sourceIndex].Z;
            destinationArray[destinationIndex].X = ((x * num24) + (y * num23)) + (z * num22);
            destinationArray[destinationIndex].Y = ((x * num21) + (y * num20)) + (z * num19);
            destinationArray[destinationIndex].Z = ((x * num18) + (y * num17)) + (z * num16);
            sourceIndex++;
            destinationIndex++;
            length--;
        }
    }

    /// <summary>Returns a vector pointing in the opposite direction.</summary>
    /// <param name="value">Source vector.</param>
    public static Vector3 Negate(Vector3 value)
    {
        Vector3 vector;
        vector.X = -value.X;
        vector.Y = -value.Y;
        vector.Z = -value.Z;
        return vector;
    }

    /// <summary>Returns a vector pointing in the opposite direction.</summary>
    /// <param name="value">Source vector.</param>
    /// <param name="result">[OutAttribute] Vector pointing in the opposite direction.</param>
    public static void Negate(ref Vector3 value, out Vector3 result)
    {
        result.X = -value.X;
        result.Y = -value.Y;
        result.Z = -value.Z;
    }

    /// <summary>Adds two vectors.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static Vector3 Add(Vector3 value1, Vector3 value2)
    {
        Vector3 vector;
        vector.X = value1.X + value2.X;
        vector.Y = value1.Y + value2.Y;
        vector.Z = value1.Z + value2.Z;
        return vector;
    }

    /// <summary>Adds two vectors.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    /// <param name="result">[OutAttribute] Sum of the source vectors.</param>
    public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
        result.X = value1.X + value2.X;
        result.Y = value1.Y + value2.Y;
        result.Z = value1.Z + value2.Z;
    }

    /// <summary>Subtracts a vector from a vector.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static Vector3 Subtract(Vector3 value1, Vector3 value2)
    {
        Vector3 vector;
        vector.X = value1.X - value2.X;
        vector.Y = value1.Y - value2.Y;
        vector.Z = value1.Z - value2.Z;
        return vector;
    }

    /// <summary>Subtracts a vector from a vector.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    /// <param name="result">[OutAttribute] The result of the subtraction.</param>
    public static void Subtract(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
        result.X = value1.X - value2.X;
        result.Y = value1.Y - value2.Y;
        result.Z = value1.Z - value2.Z;
    }

    /// <summary>Multiplies the components of two vectors by each other.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static Vector3 Multiply(Vector3 value1, Vector3 value2)
    {
        Vector3 vector;
        vector.X = value1.X * value2.X;
        vector.Y = value1.Y * value2.Y;
        vector.Z = value1.Z * value2.Z;
        return vector;
    }

    /// <summary>Multiplies the components of two vectors by each other.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    /// <param name="result">[OutAttribute] The result of the multiplication.</param>
    public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
        result.X = value1.X * value2.X;
        result.Y = value1.Y * value2.Y;
        result.Z = value1.Z * value2.Z;
    }

    /// <summary>Multiplies a vector by a scalar value.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    public static Vector3 Multiply(Vector3 value1, float scaleFactor)
    {
        Vector3 vector;
        vector.X = value1.X * scaleFactor;
        vector.Y = value1.Y * scaleFactor;
        vector.Z = value1.Z * scaleFactor;
        return vector;
    }

    /// <summary>Multiplies a vector by a scalar value.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <param name="result">[OutAttribute] The result of the multiplication.</param>
    public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
    {
        result.X = value1.X * scaleFactor;
        result.Y = value1.Y * scaleFactor;
        result.Z = value1.Z * scaleFactor;
    }

    /// <summary>Divides the components of a vector by the components of another vector.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Divisor vector.</param>
    public static Vector3 Divide(Vector3 value1, Vector3 value2)
    {
        Vector3 vector;
        vector.X = value1.X / value2.X;
        vector.Y = value1.Y / value2.Y;
        vector.Z = value1.Z / value2.Z;
        return vector;
    }

    /// <summary>Divides the components of a vector by the components of another vector.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">The divisor.</param>
    /// <param name="result">[OutAttribute] The result of the division.</param>
    public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
    {
        result.X = value1.X / value2.X;
        result.Y = value1.Y / value2.Y;
        result.Z = value1.Z / value2.Z;
    }

    /// <summary>Divides a vector by a scalar value.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">The divisor.</param>
    public static Vector3 Divide(Vector3 value1, float value2)
    {
        Vector3 vector;
        float num = 1f / value2;
        vector.X = value1.X * num;
        vector.Y = value1.Y * num;
        vector.Z = value1.Z * num;
        return vector;
    }

    /// <summary>Divides a vector by a scalar value.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">The divisor.</param>
    /// <param name="result">[OutAttribute] The result of the division.</param>
    public static void Divide(ref Vector3 value1, float value2, out Vector3 result)
    {
        float num = 1f / value2;
        result.X = value1.X * num;
        result.Y = value1.Y * num;
        result.Z = value1.Z * num;
    }

    /// <summary>Returns a vector pointing in the opposite direction.</summary>
    /// <param name="value">Source vector.</param>
    public static Vector3 operator -(Vector3 value)
    {
        Vector3 vector;
        vector.X = -value.X;
        vector.Y = -value.Y;
        vector.Z = -value.Z;
        return vector;
    }

    /// <summary>Tests vectors for equality.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static bool operator ==(Vector3 value1, Vector3 value2)
    {
        return (((value1.X == value2.X) && (value1.Y == value2.Y)) && (value1.Z == value2.Z));
    }

    /// <summary>Tests vectors for inequality.</summary>
    /// <param name="value1">Vector to compare.</param>
    /// <param name="value2">Vector to compare.</param>
    public static bool operator !=(Vector3 value1, Vector3 value2)
    {
        if ((value1.X == value2.X) && (value1.Y == value2.Y))
        {
            return (value1.Z != value2.Z);
        }
        return true;
    }

    /// <summary>Adds two vectors.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static Vector3 operator +(Vector3 value1, Vector3 value2)
    {
        Vector3 vector;
        vector.X = value1.X + value2.X;
        vector.Y = value1.Y + value2.Y;
        vector.Z = value1.Z + value2.Z;
        return vector;
    }

    /// <summary>Subtracts a vector from a vector.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static Vector3 operator -(Vector3 value1, Vector3 value2)
    {
        Vector3 vector;
        vector.X = value1.X - value2.X;
        vector.Y = value1.Y - value2.Y;
        vector.Z = value1.Z - value2.Z;
        return vector;
    }

    /// <summary>Multiplies the components of two vectors by each other.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Source vector.</param>
    public static Vector3 operator *(Vector3 value1, Vector3 value2)
    {
        Vector3 vector;
        vector.X = value1.X * value2.X;
        vector.Y = value1.Y * value2.Y;
        vector.Z = value1.Z * value2.Z;
        return vector;
    }

    /// <summary>Multiplies a vector by a scalar value.</summary>
    /// <param name="value">Source vector.</param>
    /// <param name="scaleFactor">Scalar value.</param>
    public static Vector3 operator *(Vector3 value, float scaleFactor)
    {
        Vector3 vector;
        vector.X = value.X * scaleFactor;
        vector.Y = value.Y * scaleFactor;
        vector.Z = value.Z * scaleFactor;
        return vector;
    }

    /// <summary>Multiplies a vector by a scalar value.</summary>
    /// <param name="scaleFactor">Scalar value.</param>
    /// <param name="value">Source vector.</param>
    public static Vector3 operator *(float scaleFactor, Vector3 value)
    {
        Vector3 vector;
        vector.X = value.X * scaleFactor;
        vector.Y = value.Y * scaleFactor;
        vector.Z = value.Z * scaleFactor;
        return vector;
    }

    /// <summary>Divides the components of a vector by the components of another vector.</summary>
    /// <param name="value1">Source vector.</param>
    /// <param name="value2">Divisor vector.</param>
    public static Vector3 operator /(Vector3 value1, Vector3 value2)
    {
        Vector3 vector;
        vector.X = value1.X / value2.X;
        vector.Y = value1.Y / value2.Y;
        vector.Z = value1.Z / value2.Z;
        return vector;
    }

    /// <summary>Divides a vector by a scalar value.</summary>
    /// <param name="value">Source vector.</param>
    /// <param name="divider">The divisor.</param>
    public static Vector3 operator /(Vector3 value, float divider)
    {
        Vector3 vector;
        float num = 1f / divider;
        vector.X = value.X * num;
        vector.Y = value.Y * num;
        vector.Z = value.Z * num;
        return vector;
    }

    static Vector3()
    {
        _zero = new Vector3();
        _one = new Vector3(1f, 1f, 1f);
        _unitX = new Vector3(1f, 0f, 0f);
        _unitY = new Vector3(0f, 1f, 0f);
        _unitZ = new Vector3(0f, 0f, 1f);
        _up = new Vector3(0f, 1f, 0f);
        _down = new Vector3(0f, -1f, 0f);
        _right = new Vector3(1f, 0f, 0f);
        _left = new Vector3(-1f, 0f, 0f);
        _forward = new Vector3(0f, 0f, -1f);
        _backward = new Vector3(0f, 0f, 1f);
    }
}

}
