using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    public static class GrhImageListCache
    {
        public static string FilePath
        {
            get { return ContentPaths.Build.Data.Join("grhimagelistcache.bin"); }
        }

        public static IEnumerable<GrhImageListCacheItem> Load()
        {
            GrhImageListCacheItem[] ret;

            if (!File.Exists(FilePath))
                return Enumerable.Empty<GrhImageListCacheItem>();

            using (FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    int count = r.ReadInt32();
                    ret = new GrhImageListCacheItem[count];

                    for (int i = 0; i < count; i++)
                    {
                        GrhImageListCacheItem item = GrhImageListCacheItem.Read(r);
                        ret[i] = item;
                    }
                }
            }

            return ret;
        }

        public static void Save()
        {
            var grhDatas = GrhInfo.GrhDatas.ToArray();
            var validItems = new Stack<GrhImageListCacheItem>(grhDatas.Length);

            foreach (GrhData gd in grhDatas)
            {
                string key = GrhImageList.GetImageKey(gd);
                Image image = GrhImageList.ImageList.Images[key];

                if (image == null)
                    continue;

                GrhImageListCacheItem item = new GrhImageListCacheItem(gd.GrhIndex, image, gd.OriginalSourceRect);
                validItems.Push(item);
            }

            using (FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192))
            {
                using (BinaryWriter w = new BinaryWriter(stream))
                {
                    int count = validItems.Count;
                    w.Write(count);

                    foreach (GrhImageListCacheItem item in validItems)
                    {
                        item.Write(w);
                    }
                }
            }
        }
    }
}