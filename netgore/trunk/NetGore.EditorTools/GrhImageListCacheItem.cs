using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Rectangle=Microsoft.Xna.Framework.Rectangle;

namespace NetGore.EditorTools
{
    public struct GrhImageListCacheItem
    {
        public readonly Rectangle SourceRect;
        public readonly GrhIndex GrhIndex;
        public readonly Image Image;

        public GrhImageListCacheItem(GrhIndex grhIndex, Image image, Rectangle sourceRect)
        {
            GrhIndex = grhIndex;
            Image = image;
            SourceRect = sourceRect;
        }

        public void Write(BinaryWriter w)
        {
            w.Write(SourceRect.X);
            w.Write(SourceRect.Y);
            w.Write(SourceRect.Width);
            w.Write(SourceRect.Height);
            w.Write((int)GrhIndex);

            byte[] asArray;
            using (MemoryStream ms = new MemoryStream())
            {
                Image.Save(ms, ImageFormat.Png);
                asArray = ms.ToArray();
            }

            w.Write(asArray.Length);
            w.Write(asArray);
        }

        public static GrhImageListCacheItem Read(BinaryReader r)
        {
            int x = r.ReadInt32();
            int y = r.ReadInt32();
            int w = r.ReadInt32();
            int h = r.ReadInt32();
            int i = r.ReadInt32();
            
            int l = r.ReadInt32();
            byte[] b = new byte[l];
            r.Read(b, 0, l);

            Image img;
            using (MemoryStream ms = new MemoryStream(b))
            {
                img = Image.FromStream(ms);
            }

            return new GrhImageListCacheItem(new GrhIndex(i), img, new Rectangle(x, y, w, h));
        }
    }
}
