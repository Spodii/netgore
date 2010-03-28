using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using NetGore.Xna.Framework.Design;

namespace NetGore.Xna.Framework
{
    [Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(RectangleConverter))]
    public struct Rectangle : IEquatable<Rectangle>
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        private static Rectangle _empty;
        public int Left
        {
            get
            {
                return this.X;
            }
        }
        public int Right
        {
            get
            {
                return (this.X + this.Width);
            }
        }
        public int Top
        {
            get
            {
                return this.Y;
            }
        }
        public int Bottom
        {
            get
            {
                return (this.Y + this.Height);
            }
        }
        public Point Location
        {
            get
            {
                return new Point(this.X, this.Y);
            }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }
        public Point Center
        {
            get
            {
                return new Point(this.X + (this.Width / 2), this.Y + (this.Height / 2));
            }
        }
        public static Rectangle Empty
        {
            get
            {
                return _empty;
            }
        }
        public bool IsEmpty
        {
            get
            {
                return ((((this.Width == 0) && (this.Height == 0)) && (this.X == 0)) && (this.Y == 0));
            }
        }
        public Rectangle(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public void Offset(Point amount)
        {
            this.X += amount.X;
            this.Y += amount.Y;
        }

        public void Offset(int offsetX, int offsetY)
        {
            this.X += offsetX;
            this.Y += offsetY;
        }

        public void Inflate(int horizontalAmount, int verticalAmount)
        {
            this.X -= horizontalAmount;
            this.Y -= verticalAmount;
            this.Width += horizontalAmount * 2;
            this.Height += verticalAmount * 2;
        }

        public bool Contains(int x, int y)
        {
            return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));
        }

        public bool Contains(Point value)
        {
            return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
        }

        public void Contains(ref Point value, out bool result)
        {
            result = (((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height));
        }

        public bool Contains(Rectangle value)
        {
            return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
        }

        public void Contains(ref Rectangle value, out bool result)
        {
            result = (((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height));
        }

        public bool Intersects(Rectangle value)
        {
            return ((((value.X < (this.X + this.Width)) && (this.X < (value.X + value.Width))) && (value.Y < (this.Y + this.Height))) && (this.Y < (value.Y + value.Height)));
        }

        public void Intersects(ref Rectangle value, out bool result)
        {
            result = (((value.X < (this.X + this.Width)) && (this.X < (value.X + value.Width))) && (value.Y < (this.Y + this.Height))) && (this.Y < (value.Y + value.Height));
        }

        public static Rectangle Intersect(Rectangle value1, Rectangle value2)
        {
            Rectangle rectangle;
            int num8 = value1.X + value1.Width;
            int num7 = value2.X + value2.Width;
            int num6 = value1.Y + value1.Height;
            int num5 = value2.Y + value2.Height;
            int num2 = (value1.X > value2.X) ? value1.X : value2.X;
            int num = (value1.Y > value2.Y) ? value1.Y : value2.Y;
            int num4 = (num8 < num7) ? num8 : num7;
            int num3 = (num6 < num5) ? num6 : num5;
            if ((num4 > num2) && (num3 > num))
            {
                rectangle.X = num2;
                rectangle.Y = num;
                rectangle.Width = num4 - num2;
                rectangle.Height = num3 - num;
                return rectangle;
            }
            rectangle.X = 0;
            rectangle.Y = 0;
            rectangle.Width = 0;
            rectangle.Height = 0;
            return rectangle;
        }

        public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
        {
            int num8 = value1.X + value1.Width;
            int num7 = value2.X + value2.Width;
            int num6 = value1.Y + value1.Height;
            int num5 = value2.Y + value2.Height;
            int num2 = (value1.X > value2.X) ? value1.X : value2.X;
            int num = (value1.Y > value2.Y) ? value1.Y : value2.Y;
            int num4 = (num8 < num7) ? num8 : num7;
            int num3 = (num6 < num5) ? num6 : num5;
            if ((num4 > num2) && (num3 > num))
            {
                result.X = num2;
                result.Y = num;
                result.Width = num4 - num2;
                result.Height = num3 - num;
            }
            else
            {
                result.X = 0;
                result.Y = 0;
                result.Width = 0;
                result.Height = 0;
            }
        }

        public static Rectangle Union(Rectangle value1, Rectangle value2)
        {
            Rectangle rectangle;
            int num6 = value1.X + value1.Width;
            int num5 = value2.X + value2.Width;
            int num4 = value1.Y + value1.Height;
            int num3 = value2.Y + value2.Height;
            int num2 = (value1.X < value2.X) ? value1.X : value2.X;
            int num = (value1.Y < value2.Y) ? value1.Y : value2.Y;
            int num8 = (num6 > num5) ? num6 : num5;
            int num7 = (num4 > num3) ? num4 : num3;
            rectangle.X = num2;
            rectangle.Y = num;
            rectangle.Width = num8 - num2;
            rectangle.Height = num7 - num;
            return rectangle;
        }

        public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
        {
            int num6 = value1.X + value1.Width;
            int num5 = value2.X + value2.Width;
            int num4 = value1.Y + value1.Height;
            int num3 = value2.Y + value2.Height;
            int num2 = (value1.X < value2.X) ? value1.X : value2.X;
            int num = (value1.Y < value2.Y) ? value1.Y : value2.Y;
            int num8 = (num6 > num5) ? num6 : num5;
            int num7 = (num4 > num3) ? num4 : num3;
            result.X = num2;
            result.Y = num;
            result.Width = num8 - num2;
            result.Height = num7 - num;
        }

        public bool Equals(Rectangle other)
        {
            return ((((this.X == other.X) && (this.Y == other.Y)) && (this.Width == other.Width)) && (this.Height == other.Height));
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Rectangle)
            {
                flag = this.Equals((Rectangle)obj);
            }
            return flag;
        }

        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1} Width:{2} Height:{3}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture), this.Width.ToString(currentCulture), this.Height.ToString(currentCulture) });
        }

        public override int GetHashCode()
        {
            return (((this.X.GetHashCode() + this.Y.GetHashCode()) + this.Width.GetHashCode()) + this.Height.GetHashCode());
        }

        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return ((((a.X == b.X) && (a.Y == b.Y)) && (a.Width == b.Width)) && (a.Height == b.Height));
        }

        public static bool operator !=(Rectangle a, Rectangle b)
        {
            if (((a.X == b.X) && (a.Y == b.Y)) && (a.Width == b.Width))
            {
                return (a.Height != b.Height);
            }
            return true;
        }

        static Rectangle()
        {
            _empty = new Rectangle();
        }
    }

 

}
