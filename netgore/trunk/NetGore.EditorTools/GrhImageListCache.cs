using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    public static class GrhImageListCache
    {
        public static string FilePath { get { return ContentPaths.Build.Data.Join("grhimagelistcache.bin"); } }

        public static IEnumerable<GrhImageListCacheItem> Load()
        {
            GrhImageListCacheItem[] ret;

            if (!File.Exists(FilePath))
                return Enumerable.Empty<GrhImageListCacheItem>();


            using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192))
            {
                using (var r = new BinaryReader(stream))
                {
                    int count = r.ReadInt32();
                    ret = new GrhImageListCacheItem[count];

                    for (int i = 0; i < count; i++)
                    {
                        var item = GrhImageListCacheItem.Read(r);
                        ret[i] = item;
                    }
                }
            }

            return ret;
        }

        public static void Save()
        {
            var grhDatas = GrhInfo.GrhDatas.ToArray();
            Stack<GrhImageListCacheItem> validItems = new Stack<GrhImageListCacheItem>(grhDatas.Length);

            foreach (var gd in grhDatas)
            {
                var key = GrhImageList.GetImageKey(gd);
                var image = GrhImageList.ImageList.Images[key];

                if (image == null)
                    continue;

                GrhImageListCacheItem item = new GrhImageListCacheItem(gd.GrhIndex, image, gd.OriginalSourceRect);
                validItems.Push(item);
            }

            using (var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192))
            {
                using (var w = new BinaryWriter(stream))
                {
                    int count = validItems.Count;
                    w.Write(count);

                    foreach (var item in validItems)
                        item.Write(w);
                }
            }
        }
    }
}
