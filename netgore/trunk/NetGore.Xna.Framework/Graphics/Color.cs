using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>Represents a color using Red, Green, Blue, and Alpha values.</summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(ColorConverter))]
    public struct Color : IPackedVector<uint>, IPackedVector, IEquatable<Color>
    {
        uint packedValue;

        Color(uint packedValue)
        {
            this.packedValue = packedValue;
        }

        /// <summary>Initializes a new instance of Color.</summary>
        /// <param name="r">The red component of a color, between 0 and 255.</param>
        /// <param name="g">The green component of a color, between 0 and 255.</param>
        /// <param name="b">The blue component of a color, between 0 and 255.</param>
        public Color(byte r, byte g, byte b)
        {
            packedValue = PackHelper(r, g, b, 0xff);
        }

        /// <summary>Initializes a new instance of Color.</summary>
        /// <param name="r">The red component of a color, between 0 and 255.</param>
        /// <param name="g">The green component of a color, between 0 and 255.</param>
        /// <param name="b">The blue component of a color, between 0 and 255.</param>
        /// <param name="a">The alpha component of a color, between 0 and 255.</param>
        public Color(byte r, byte g, byte b, byte a)
        {
            packedValue = PackHelper(r, g, b, a);
        }

        /// <summary>Initializes a new instance of Color.</summary>
        /// <param name="r">The red component of a color, between 0 and 1.0.</param>
        /// <param name="g">The green component of a color, between 0 and 1.0.</param>
        /// <param name="b">The blue component of a color, between 0 and 1.0.</param>
        public Color(float r, float g, float b)
        {
            packedValue = PackHelper(r, g, b, 1f);
        }

        /// <summary>Initializes a new instance of Color.</summary>
        /// <param name="r">The red component of a color, between 0 and 1.0.</param>
        /// <param name="g">The green component of a color, between 0 and 1.0.</param>
        /// <param name="b">The blue component of a color, between 0 and 1.0.</param>
        /// <param name="a">The alpha component of a color, between 0 and 1.0.</param>
        public Color(float r, float g, float b, float a)
        {
            packedValue = PackHelper(r, g, b, a);
        }

        /// <summary>Initializes a new instance of Color.</summary>
        /// <param name="vector">A Vector3 containing the Red, Green, and Blue values defining a color.</param>
        public Color(Vector3 vector)
        {
            packedValue = PackHelper(vector.X, vector.Y, vector.Z, 1f);
        }

        /// <summary>Initializes a new instance of Color.</summary>
        /// <param name="vector">A Vector4 containing the Red, Green, Blue, and Alpha values defining a color.</param>
        public Color(Vector4 vector)
        {
            packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        /// <summary>Initializes a new instance of Color.</summary>
        /// <param name="rgb">A Color specifying the red, green, and blue components of a color.</param>
        /// <param name="a">The alpha component of a color, between 0 and 255.</param>
        public Color(Color rgb, byte a)
        {
            uint num2 = rgb.packedValue & 0xffffff;
            uint num = (uint)(a << 0x18);
            packedValue = num2 | num;
        }

        /// <summary>Initializes a new instance of Color.</summary>
        /// <param name="rgb">A Color specifying the red, green, and blue components of a color.</param>
        /// <param name="a">The alpha component of a color, between 0 and 1.0.</param>
        public Color(Color rgb, float a)
        {
            uint num2 = rgb.packedValue & 0xffffff;
            uint num = PackUtils.PackUNorm(255f, a) << 0x18;
            packedValue = num2 | num;
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            packedValue = PackHelper(vector.X, vector.Y, vector.Z, vector.W);
        }

        static uint PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW)
        {
            uint num4 = PackUtils.PackUNorm(255f, vectorX) << 0x10;
            uint num3 = PackUtils.PackUNorm(255f, vectorY) << 8;
            uint num2 = PackUtils.PackUNorm(255f, vectorZ);
            uint num = PackUtils.PackUNorm(255f, vectorW) << 0x18;
            return (((num4 | num3) | num2) | num);
        }

        static uint PackHelper(byte r, byte g, byte b, byte a)
        {
            uint num4 = (uint)(r << 0x10);
            uint num3 = (uint)(g << 8);
            uint num2 = b;
            uint num = (uint)(a << 0x18);
            return (((num4 | num3) | num2) | num);
        }

        /// <summary>Returns the current color as a Vector3.</summary>
        public Vector3 ToVector3()
        {
            Vector3 vector;
            vector.X = PackUtils.UnpackUNorm(0xff, packedValue >> 0x10);
            vector.Y = PackUtils.UnpackUNorm(0xff, packedValue >> 8);
            vector.Z = PackUtils.UnpackUNorm(0xff, packedValue);
            return vector;
        }

        /// <summary>Returns the current color as a Vector4.</summary>
        public Vector4 ToVector4()
        {
            Vector4 vector;
            vector.X = PackUtils.UnpackUNorm(0xff, packedValue >> 0x10);
            vector.Y = PackUtils.UnpackUNorm(0xff, packedValue >> 8);
            vector.Z = PackUtils.UnpackUNorm(0xff, packedValue);
            vector.W = PackUtils.UnpackUNorm(0xff, packedValue >> 0x18);
            return vector;
        }

        /// <summary>Gets or sets the red component value of this Color.</summary>
        public byte R
        {
            get { return (byte)(packedValue >> 0x10); }
            set { packedValue = (packedValue & 0xff00ffff) | ((uint)(value << 0x10)); }
        }

        /// <summary>Gets or sets the green component value of this Color.</summary>
        public byte G
        {
            get { return (byte)(packedValue >> 8); }
            set { packedValue = (packedValue & 0xffff00ff) | ((uint)(value << 8)); }
        }

        /// <summary>Gets or sets the blue component value of this Color.</summary>
        public byte B
        {
            get { return (byte)packedValue; }
            set { packedValue = (packedValue & 0xffffff00) | value; }
        }

        /// <summary>Gets or sets the alpha component value.</summary>
        public byte A
        {
            get { return (byte)(packedValue >> 0x18); }
            set { packedValue = (packedValue & 0xffffff) | ((uint)(value << 0x18)); }
        }

        /// <summary>Gets the current color as a packed value.</summary>
        [CLSCompliant(false)]
        public uint PackedValue
        {
            get { return packedValue; }
            set { packedValue = value; }
        }

        /// <summary>Linearly interpolates between two colors.</summary>
        /// <param name="value1">Source Color.</param>
        /// <param name="value2">Source Color.</param>
        /// <param name="amount">A value between 0 and 1.0 indicating the weight of value2.</param>
        public static Color Lerp(Color value1, Color value2, float amount)
        {
            Color color;
            uint packedValue = value1.packedValue;
            uint num2 = value2.packedValue;
            int num7 = (byte)(packedValue >> 0x10);
            int num6 = (byte)(packedValue >> 8);
            int num5 = (byte)packedValue;
            int num4 = (byte)(packedValue >> 0x18);
            int num15 = (byte)(num2 >> 0x10);
            int num14 = (byte)(num2 >> 8);
            int num13 = (byte)num2;
            int num12 = (byte)(num2 >> 0x18);
            int num = (int)PackUtils.PackUNorm(65536f, amount);
            int num11 = num7 + (((num15 - num7) * num) >> 0x10);
            int num10 = num6 + (((num14 - num6) * num) >> 0x10);
            int num9 = num5 + (((num13 - num5) * num) >> 0x10);
            int num8 = num4 + (((num12 - num4) * num) >> 0x10);
            color.packedValue = (uint)((((num11 << 0x10) | (num10 << 8)) | num9) | (num8 << 0x18));
            return color;
        }

        /// <summary>Gets a string representation of this object.</summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{R:{0} G:{1} B:{2} A:{3}}}", new object[] { R, G, B, A });
        }

        /// <summary>Gets the hash code for this instance.</summary>
        public override int GetHashCode()
        {
            return packedValue.GetHashCode();
        }

        /// <summary>Returns a value that indicates whether the current instance is equal to a specified object.</summary>
        /// <param name="obj">The Object to compare with the current Color.</param>
        public override bool Equals(object obj)
        {
            return ((obj is Color) && Equals((Color)obj));
        }

        /// <summary>Returns a value that indicates whether the current instance is equal to a specified object.</summary>
        /// <param name="other">The Color to compare with the current Color.</param>
        public bool Equals(Color other)
        {
            return packedValue.Equals(other.packedValue);
        }

        /// <summary>Compares two objects to determine whether they are the same.</summary>
        /// <param name="a">The object to the left of the equality operator.</param>
        /// <param name="b">The object to the right of the equality operator.</param>
        public static bool operator ==(Color a, Color b)
        {
            return a.Equals(b);
        }

        /// <summary>Compares two objects to determine whether they are different.</summary>
        /// <param name="a">The object to the left of the equality operator.</param>
        /// <param name="b">The object to the right of the equality operator.</param>
        public static bool operator !=(Color a, Color b)
        {
            return !a.Equals(b);
        }

        /// <summary>Gets a system-defined color with the value R:0 G:0 B:0 A:0.</summary>
        public static Color TransparentBlack
        {
            get { return new Color(0); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:255 B:255 A:0.</summary>
        public static Color TransparentWhite
        {
            get { return new Color(0xffffff); }
        }

        /// <summary>Gets a system-defined color with the value R:240 G:248 B:255 A:255.</summary>
        public static Color AliceBlue
        {
            get { return new Color(0xfff0f8ff); }
        }

        /// <summary>Gets a system-defined color with the value R:250 G:235 B:215 A:255.</summary>
        public static Color AntiqueWhite
        {
            get { return new Color(0xfffaebd7); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:255 B:255 A:255.</summary>
        public static Color Aqua
        {
            get { return new Color(0xff00ffff); }
        }

        /// <summary>Gets a system-defined color with the value R:127 G:255 B:212 A:255.</summary>
        public static Color Aquamarine
        {
            get { return new Color(0xff7fffd4); }
        }

        /// <summary>Gets a system-defined color with the value R:240 G:255 B:255 A:255.</summary>
        public static Color Azure
        {
            get { return new Color(0xfff0ffff); }
        }

        /// <summary>Gets a system-defined color with the value R:245 G:245 B:220 A:255.</summary>
        public static Color Beige
        {
            get { return new Color(0xfff5f5dc); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:228 B:196 A:255.</summary>
        public static Color Bisque
        {
            get { return new Color(0xffffe4c4); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:0 B:0 A:255.</summary>
        public static Color Black
        {
            get { return new Color(0xff000000); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:235 B:205 A:255.</summary>
        public static Color BlanchedAlmond
        {
            get { return new Color(0xffffebcd); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:0 B:255 A:255.</summary>
        public static Color Blue
        {
            get { return new Color(0xff0000ff); }
        }

        /// <summary>Gets a system-defined color with the value R:138 G:43 B:226 A:255.</summary>
        public static Color BlueViolet
        {
            get { return new Color(0xff8a2be2); }
        }

        /// <summary>Gets a system-defined color with the value R:165 G:42 B:42 A:255.</summary>
        public static Color Brown
        {
            get { return new Color(0xffa52a2a); }
        }

        /// <summary>Gets a system-defined color with the value R:222 G:184 B:135 A:255.</summary>
        public static Color BurlyWood
        {
            get { return new Color(0xffdeb887); }
        }

        /// <summary>Gets a system-defined color with the value R:95 G:158 B:160 A:255.</summary>
        public static Color CadetBlue
        {
            get { return new Color(0xff5f9ea0); }
        }

        /// <summary>Gets a system-defined color with the value R:127 G:255 B:0 A:255.</summary>
        public static Color Chartreuse
        {
            get { return new Color(0xff7fff00); }
        }

        /// <summary>Gets a system-defined color with the value R:210 G:105 B:30 A:255.</summary>
        public static Color Chocolate
        {
            get { return new Color(0xffd2691e); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:127 B:80 A:255.</summary>
        public static Color Coral
        {
            get { return new Color(0xffff7f50); }
        }

        /// <summary>Gets a system-defined color with the value R:100 G:149 B:237 A:255.</summary>
        public static Color CornflowerBlue
        {
            get { return new Color(0xff6495ed); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:248 B:220 A:255.</summary>
        public static Color Cornsilk
        {
            get { return new Color(0xfffff8dc); }
        }

        /// <summary>Gets a system-defined color with the value R:220 G:20 B:60 A:255.</summary>
        public static Color Crimson
        {
            get { return new Color(0xffdc143c); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:255 B:255 A:255.</summary>
        public static Color Cyan
        {
            get { return new Color(0xff00ffff); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:0 B:139 A:255.</summary>
        public static Color DarkBlue
        {
            get { return new Color(0xff00008b); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:139 B:139 A:255.</summary>
        public static Color DarkCyan
        {
            get { return new Color(0xff008b8b); }
        }

        /// <summary>Gets a system-defined color with the value R:184 G:134 B:11 A:255.</summary>
        public static Color DarkGoldenrod
        {
            get { return new Color(0xffb8860b); }
        }

        /// <summary>Gets a system-defined color with the value R:169 G:169 B:169 A:255.</summary>
        public static Color DarkGray
        {
            get { return new Color(0xffa9a9a9); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:100 B:0 A:255.</summary>
        public static Color DarkGreen
        {
            get { return new Color(0xff006400); }
        }

        /// <summary>Gets a system-defined color with the value R:189 G:183 B:107 A:255.</summary>
        public static Color DarkKhaki
        {
            get { return new Color(0xffbdb76b); }
        }

        /// <summary>Gets a system-defined color with the value R:139 G:0 B:139 A:255.</summary>
        public static Color DarkMagenta
        {
            get { return new Color(0xff8b008b); }
        }

        /// <summary>Gets a system-defined color with the value R:85 G:107 B:47 A:255.</summary>
        public static Color DarkOliveGreen
        {
            get { return new Color(0xff556b2f); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:140 B:0 A:255.</summary>
        public static Color DarkOrange
        {
            get { return new Color(0xffff8c00); }
        }

        /// <summary>Gets a system-defined color with the value R:153 G:50 B:204 A:255.</summary>
        public static Color DarkOrchid
        {
            get { return new Color(0xff9932cc); }
        }

        /// <summary>Gets a system-defined color with the value R:139 G:0 B:0 A:255.</summary>
        public static Color DarkRed
        {
            get { return new Color(0xff8b0000); }
        }

        /// <summary>Gets a system-defined color with the value R:233 G:150 B:122 A:255.</summary>
        public static Color DarkSalmon
        {
            get { return new Color(0xffe9967a); }
        }

        /// <summary>Gets a system-defined color with the value R:143 G:188 B:139 A:255.</summary>
        public static Color DarkSeaGreen
        {
            get { return new Color(0xff8fbc8b); }
        }

        /// <summary>Gets a system-defined color with the value R:72 G:61 B:139 A:255.</summary>
        public static Color DarkSlateBlue
        {
            get { return new Color(0xff483d8b); }
        }

        /// <summary>Gets a system-defined color with the value R:47 G:79 B:79 A:255.</summary>
        public static Color DarkSlateGray
        {
            get { return new Color(0xff2f4f4f); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:206 B:209 A:255.</summary>
        public static Color DarkTurquoise
        {
            get { return new Color(0xff00ced1); }
        }

        /// <summary>Gets a system-defined color with the value R:148 G:0 B:211 A:255.</summary>
        public static Color DarkViolet
        {
            get { return new Color(0xff9400d3); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:20 B:147 A:255.</summary>
        public static Color DeepPink
        {
            get { return new Color(0xffff1493); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:191 B:255 A:255.</summary>
        public static Color DeepSkyBlue
        {
            get { return new Color(0xff00bfff); }
        }

        /// <summary>Gets a system-defined color with the value R:105 G:105 B:105 A:255.</summary>
        public static Color DimGray
        {
            get { return new Color(0xff696969); }
        }

        /// <summary>Gets a system-defined color with the value R:30 G:144 B:255 A:255.</summary>
        public static Color DodgerBlue
        {
            get { return new Color(0xff1e90ff); }
        }

        /// <summary>Gets a system-defined color with the value R:178 G:34 B:34 A:255.</summary>
        public static Color Firebrick
        {
            get { return new Color(0xffb22222); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:250 B:240 A:255.</summary>
        public static Color FloralWhite
        {
            get { return new Color(0xfffffaf0); }
        }

        /// <summary>Gets a system-defined color with the value R:34 G:139 B:34 A:255.</summary>
        public static Color ForestGreen
        {
            get { return new Color(0xff228b22); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:0 B:255 A:255.</summary>
        public static Color Fuchsia
        {
            get { return new Color(0xffff00ff); }
        }

        /// <summary>Gets a system-defined color with the value R:220 G:220 B:220 A:255.</summary>
        public static Color Gainsboro
        {
            get { return new Color(0xffdcdcdc); }
        }

        /// <summary>Gets a system-defined color with the value R:248 G:248 B:255 A:255.</summary>
        public static Color GhostWhite
        {
            get { return new Color(0xfff8f8ff); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:215 B:0 A:255.</summary>
        public static Color Gold
        {
            get { return new Color(0xffffd700); }
        }

        /// <summary>Gets a system-defined color with the value R:218 G:165 B:32 A:255.</summary>
        public static Color Goldenrod
        {
            get { return new Color(0xffdaa520); }
        }

        /// <summary>Gets a system-defined color with the value R:128 G:128 B:128 A:255.</summary>
        public static Color Gray
        {
            get { return new Color(0xff808080); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:128 B:0 A:255.</summary>
        public static Color Green
        {
            get { return new Color(0xff008000); }
        }

        /// <summary>Gets a system-defined color with the value R:173 G:255 B:47 A:255.</summary>
        public static Color GreenYellow
        {
            get { return new Color(0xffadff2f); }
        }

        /// <summary>Gets a system-defined color with the value R:240 G:255 B:240 A:255.</summary>
        public static Color Honeydew
        {
            get { return new Color(0xfff0fff0); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:105 B:180 A:255.</summary>
        public static Color HotPink
        {
            get { return new Color(0xffff69b4); }
        }

        /// <summary>Gets a system-defined color with the value R:205 G:92 B:92 A:255.</summary>
        public static Color IndianRed
        {
            get { return new Color(0xffcd5c5c); }
        }

        /// <summary>Gets a system-defined color with the value R:75 G:0 B:130 A:255.</summary>
        public static Color Indigo
        {
            get { return new Color(0xff4b0082); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:255 B:240 A:255.</summary>
        public static Color Ivory
        {
            get { return new Color(0xfffffff0); }
        }

        /// <summary>Gets a system-defined color with the value R:240 G:230 B:140 A:255.</summary>
        public static Color Khaki
        {
            get { return new Color(0xfff0e68c); }
        }

        /// <summary>Gets a system-defined color with the value R:230 G:230 B:250 A:255.</summary>
        public static Color Lavender
        {
            get { return new Color(0xffe6e6fa); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:240 B:245 A:255.</summary>
        public static Color LavenderBlush
        {
            get { return new Color(0xfffff0f5); }
        }

        /// <summary>Gets a system-defined color with the value R:124 G:252 B:0 A:255.</summary>
        public static Color LawnGreen
        {
            get { return new Color(0xff7cfc00); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:250 B:205 A:255.</summary>
        public static Color LemonChiffon
        {
            get { return new Color(0xfffffacd); }
        }

        /// <summary>Gets a system-defined color with the value R:173 G:216 B:230 A:255.</summary>
        public static Color LightBlue
        {
            get { return new Color(0xffadd8e6); }
        }

        /// <summary>Gets a system-defined color with the value R:240 G:128 B:128 A:255.</summary>
        public static Color LightCoral
        {
            get { return new Color(0xfff08080); }
        }

        /// <summary>Gets a system-defined color with the value R:224 G:255 B:255 A:255.</summary>
        public static Color LightCyan
        {
            get { return new Color(0xffe0ffff); }
        }

        /// <summary>Gets a system-defined color with the value R:250 G:250 B:210 A:255.</summary>
        public static Color LightGoldenrodYellow
        {
            get { return new Color(0xfffafad2); }
        }

        /// <summary>Gets a system-defined color with the value R:144 G:238 B:144 A:255.</summary>
        public static Color LightGreen
        {
            get { return new Color(0xff90ee90); }
        }

        /// <summary>Gets a system-defined color with the value R:211 G:211 B:211 A:255.</summary>
        public static Color LightGray
        {
            get { return new Color(0xffd3d3d3); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:182 B:193 A:255.</summary>
        public static Color LightPink
        {
            get { return new Color(0xffffb6c1); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:160 B:122 A:255.</summary>
        public static Color LightSalmon
        {
            get { return new Color(0xffffa07a); }
        }

        /// <summary>Gets a system-defined color with the value R:32 G:178 B:170 A:255.</summary>
        public static Color LightSeaGreen
        {
            get { return new Color(0xff20b2aa); }
        }

        /// <summary>Gets a system-defined color with the value R:135 G:206 B:250 A:255.</summary>
        public static Color LightSkyBlue
        {
            get { return new Color(0xff87cefa); }
        }

        /// <summary>Gets a system-defined color with the value R:119 G:136 B:153 A:255.</summary>
        public static Color LightSlateGray
        {
            get { return new Color(0xff778899); }
        }

        /// <summary>Gets a system-defined color with the value R:176 G:196 B:222 A:255.</summary>
        public static Color LightSteelBlue
        {
            get { return new Color(0xffb0c4de); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:255 B:224 A:255.</summary>
        public static Color LightYellow
        {
            get { return new Color(0xffffffe0); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:255 B:0 A:255.</summary>
        public static Color Lime
        {
            get { return new Color(0xff00ff00); }
        }

        /// <summary>Gets a system-defined color with the value R:50 G:205 B:50 A:255.</summary>
        public static Color LimeGreen
        {
            get { return new Color(0xff32cd32); }
        }

        /// <summary>Gets a system-defined color with the value R:250 G:240 B:230 A:255.</summary>
        public static Color Linen
        {
            get { return new Color(0xfffaf0e6); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:0 B:255 A:255.</summary>
        public static Color Magenta
        {
            get { return new Color(0xffff00ff); }
        }

        /// <summary>Gets a system-defined color with the value R:128 G:0 B:0 A:255.</summary>
        public static Color Maroon
        {
            get { return new Color(0xff800000); }
        }

        /// <summary>Gets a system-defined color with the value R:102 G:205 B:170 A:255.</summary>
        public static Color MediumAquamarine
        {
            get { return new Color(0xff66cdaa); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:0 B:205 A:255.</summary>
        public static Color MediumBlue
        {
            get { return new Color(0xff0000cd); }
        }

        /// <summary>Gets a system-defined color with the value R:186 G:85 B:211 A:255.</summary>
        public static Color MediumOrchid
        {
            get { return new Color(0xffba55d3); }
        }

        /// <summary>Gets a system-defined color with the value R:147 G:112 B:219 A:255.</summary>
        public static Color MediumPurple
        {
            get { return new Color(0xff9370db); }
        }

        /// <summary>Gets a system-defined color with the value R:60 G:179 B:113 A:255.</summary>
        public static Color MediumSeaGreen
        {
            get { return new Color(0xff3cb371); }
        }

        /// <summary>Gets a system-defined color with the value R:123 G:104 B:238 A:255.</summary>
        public static Color MediumSlateBlue
        {
            get { return new Color(0xff7b68ee); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:250 B:154 A:255.</summary>
        public static Color MediumSpringGreen
        {
            get { return new Color(0xff00fa9a); }
        }

        /// <summary>Gets a system-defined color with the value R:72 G:209 B:204 A:255.</summary>
        public static Color MediumTurquoise
        {
            get { return new Color(0xff48d1cc); }
        }

        /// <summary>Gets a system-defined color with the value R:199 G:21 B:133 A:255.</summary>
        public static Color MediumVioletRed
        {
            get { return new Color(0xffc71585); }
        }

        /// <summary>Gets a system-defined color with the value R:25 G:25 B:112 A:255.</summary>
        public static Color MidnightBlue
        {
            get { return new Color(0xff191970); }
        }

        /// <summary>Gets a system-defined color with the value R:245 G:255 B:250 A:255.</summary>
        public static Color MintCream
        {
            get { return new Color(0xfff5fffa); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:228 B:225 A:255.</summary>
        public static Color MistyRose
        {
            get { return new Color(0xffffe4e1); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:228 B:181 A:255.</summary>
        public static Color Moccasin
        {
            get { return new Color(0xffffe4b5); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:222 B:173 A:255.</summary>
        public static Color NavajoWhite
        {
            get { return new Color(0xffffdead); }
        }

        /// <summary>Gets a system-defined color R:0 G:0 B:128 A:255.</summary>
        public static Color Navy
        {
            get { return new Color(0xff000080); }
        }

        /// <summary>Gets a system-defined color with the value R:253 G:245 B:230 A:255.</summary>
        public static Color OldLace
        {
            get { return new Color(0xfffdf5e6); }
        }

        /// <summary>Gets a system-defined color with the value R:128 G:128 B:0 A:255.</summary>
        public static Color Olive
        {
            get { return new Color(0xff808000); }
        }

        /// <summary>Gets a system-defined color with the value R:107 G:142 B:35 A:255.</summary>
        public static Color OliveDrab
        {
            get { return new Color(0xff6b8e23); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:165 B:0 A:255.</summary>
        public static Color Orange
        {
            get { return new Color(0xffffa500); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:69 B:0 A:255.</summary>
        public static Color OrangeRed
        {
            get { return new Color(0xffff4500); }
        }

        /// <summary>Gets a system-defined color with the value R:218 G:112 B:214 A:255.</summary>
        public static Color Orchid
        {
            get { return new Color(0xffda70d6); }
        }

        /// <summary>Gets a system-defined color with the value R:238 G:232 B:170 A:255.</summary>
        public static Color PaleGoldenrod
        {
            get { return new Color(0xffeee8aa); }
        }

        /// <summary>Gets a system-defined color with the value R:152 G:251 B:152 A:255.</summary>
        public static Color PaleGreen
        {
            get { return new Color(0xff98fb98); }
        }

        /// <summary>Gets a system-defined color with the value R:175 G:238 B:238 A:255.</summary>
        public static Color PaleTurquoise
        {
            get { return new Color(0xffafeeee); }
        }

        /// <summary>Gets a system-defined color with the value R:219 G:112 B:147 A:255.</summary>
        public static Color PaleVioletRed
        {
            get { return new Color(0xffdb7093); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:239 B:213 A:255.</summary>
        public static Color PapayaWhip
        {
            get { return new Color(0xffffefd5); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:218 B:185 A:255.</summary>
        public static Color PeachPuff
        {
            get { return new Color(0xffffdab9); }
        }

        /// <summary>Gets a system-defined color with the value R:205 G:133 B:63 A:255.</summary>
        public static Color Peru
        {
            get { return new Color(0xffcd853f); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:192 B:203 A:255.</summary>
        public static Color Pink
        {
            get { return new Color(0xffffc0cb); }
        }

        /// <summary>Gets a system-defined color with the value R:221 G:160 B:221 A:255.</summary>
        public static Color Plum
        {
            get { return new Color(0xffdda0dd); }
        }

        /// <summary>Gets a system-defined color with the value R:176 G:224 B:230 A:255.</summary>
        public static Color PowderBlue
        {
            get { return new Color(0xffb0e0e6); }
        }

        /// <summary>Gets a system-defined color with the value R:128 G:0 B:128 A:255.</summary>
        public static Color Purple
        {
            get { return new Color(0xff800080); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:0 B:0 A:255.</summary>
        public static Color Red
        {
            get { return new Color(0xffff0000); }
        }

        /// <summary>Gets a system-defined color with the value R:188 G:143 B:143 A:255.</summary>
        public static Color RosyBrown
        {
            get { return new Color(0xffbc8f8f); }
        }

        /// <summary>Gets a system-defined color with the value R:65 G:105 B:225 A:255.</summary>
        public static Color RoyalBlue
        {
            get { return new Color(0xff4169e1); }
        }

        /// <summary>Gets a system-defined color with the value R:139 G:69 B:19 A:255.</summary>
        public static Color SaddleBrown
        {
            get { return new Color(0xff8b4513); }
        }

        /// <summary>Gets a system-defined color with the value R:250 G:128 B:114 A:255.</summary>
        public static Color Salmon
        {
            get { return new Color(0xfffa8072); }
        }

        /// <summary>Gets a system-defined color with the value R:244 G:164 B:96 A:255.</summary>
        public static Color SandyBrown
        {
            get { return new Color(0xfff4a460); }
        }

        /// <summary>Gets a system-defined color with the value R:46 G:139 B:87 A:255.</summary>
        public static Color SeaGreen
        {
            get { return new Color(0xff2e8b57); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:245 B:238 A:255.</summary>
        public static Color SeaShell
        {
            get { return new Color(0xfffff5ee); }
        }

        /// <summary>Gets a system-defined color with the value R:160 G:82 B:45 A:255.</summary>
        public static Color Sienna
        {
            get { return new Color(0xffa0522d); }
        }

        /// <summary>Gets a system-defined color with the value R:192 G:192 B:192 A:255.</summary>
        public static Color Silver
        {
            get { return new Color(0xffc0c0c0); }
        }

        /// <summary>Gets a system-defined color with the value R:135 G:206 B:235 A:255.</summary>
        public static Color SkyBlue
        {
            get { return new Color(0xff87ceeb); }
        }

        /// <summary>Gets a system-defined color with the value R:106 G:90 B:205 A:255.</summary>
        public static Color SlateBlue
        {
            get { return new Color(0xff6a5acd); }
        }

        /// <summary>Gets a system-defined color with the value R:112 G:128 B:144 A:255.</summary>
        public static Color SlateGray
        {
            get { return new Color(0xff708090); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:250 B:250 A:255.</summary>
        public static Color Snow
        {
            get { return new Color(0xfffffafa); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:255 B:127 A:255.</summary>
        public static Color SpringGreen
        {
            get { return new Color(0xff00ff7f); }
        }

        /// <summary>Gets a system-defined color with the value R:70 G:130 B:180 A:255.</summary>
        public static Color SteelBlue
        {
            get { return new Color(0xff4682b4); }
        }

        /// <summary>Gets a system-defined color with the value R:210 G:180 B:140 A:255.</summary>
        public static Color Tan
        {
            get { return new Color(0xffd2b48c); }
        }

        /// <summary>Gets a system-defined color with the value R:0 G:128 B:128 A:255.</summary>
        public static Color Teal
        {
            get { return new Color(0xff008080); }
        }

        /// <summary>Gets a system-defined color with the value R:216 G:191 B:216 A:255.</summary>
        public static Color Thistle
        {
            get { return new Color(0xffd8bfd8); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:99 B:71 A:255.</summary>
        public static Color Tomato
        {
            get { return new Color(0xffff6347); }
        }

        /// <summary>Gets a system-defined color with the value R:64 G:224 B:208 A:255.</summary>
        public static Color Turquoise
        {
            get { return new Color(0xff40e0d0); }
        }

        /// <summary>Gets a system-defined color with the value R:238 G:130 B:238 A:255.</summary>
        public static Color Violet
        {
            get { return new Color(0xffee82ee); }
        }

        /// <summary>Gets a system-defined color with the value R:245 G:222 B:179 A:255.</summary>
        public static Color Wheat
        {
            get { return new Color(0xfff5deb3); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:255 B:255 A:255.</summary>
        public static Color White
        {
            get { return new Color(uint.MaxValue); }
        }

        /// <summary>Gets a system-defined color with the value R:245 G:245 B:245 A:255.</summary>
        public static Color WhiteSmoke
        {
            get { return new Color(0xfff5f5f5); }
        }

        /// <summary>Gets a system-defined color with the value R:255 G:255 B:0 A:255.</summary>
        public static Color Yellow
        {
            get { return new Color(0xffffff00); }
        }

        /// <summary>Gets a system-defined color with the value R:154 G:205 B:50 A:255.</summary>
        public static Color YellowGreen
        {
            get { return new Color(0xff9acd32); }
        }
    }
}