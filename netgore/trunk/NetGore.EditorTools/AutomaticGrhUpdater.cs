using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    public static class AutomaticGrhUpdater
    {
        public static void UpdateAll(ContentManager cm, string rootDir)
        {
            UpdateStationary(cm, rootDir);
           // UpdateAnimated(rootDir);
        }

        static List<string> FindTextures(string rootDir)
        {
            return new List<string>(Directory.GetFiles(rootDir, "*.png", SearchOption.AllDirectories)).Select(x => x.Replace('\\', '/')).ToList();
        }

        static Dictionary<string, List<GrhData>> FindUsedTextures()
        {
            var ret = new Dictionary<string, List<GrhData>>(StringComparer.OrdinalIgnoreCase);

            // Loop through every stationary GrhData
            foreach (var gd in GrhInfo.GrhDatas.Where(x => !x.IsAnimated))
            {
                string texture = gd.TextureName;
                List<GrhData> dictList;

                // Get the existing list, or create a new one if the first entry
                if (!ret.TryGetValue(texture, out dictList))
                {
                    dictList = new List<GrhData>();
                    ret.Add(texture, dictList);
                }

                // Add the GrhData to the list
                dictList.Add(gd);
            }

            return ret;
        }

        static int GetRelativeTrimLength(string rootDir)
        {
            int len = rootDir.Length;
            if (!rootDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                len++;
            return len;
        }

        static string TextureAbsoluteToRelativePath(int trimLen, string absolute)
        {
            // Trim down to the relative path
            string rel = absolute.Substring(trimLen);

            // Remove the file suffix since we don't use that
            int lastPeriod = rel.LastIndexOf('.');
            rel = rel.Substring(0, lastPeriod);

            return rel;
        }

        static Vector2 GetTextureSize(string filePath)
        {
            var info = Texture2D.GetTextureInformation(filePath);
            return new Vector2(info.Width, info.Height);
        }

        public static IEnumerable<GrhData> UpdateStationary(ContentManager cm, string rootGrhDir)
        {
            // Get a List of all of the textures from the root directory
            var textures = FindTextures(rootGrhDir);

            // Get a List of all of the used textures
            var usedTextures = FindUsedTextures();

            // Grab the relative path instead of the complete file path since this
            // is how they are stored in the GrhData, then if it is in the usedTextures, remove it
            int trimLen = GetRelativeTrimLength(rootGrhDir);
            textures.RemoveAll(x => usedTextures.ContainsKey(TextureAbsoluteToRelativePath(trimLen, x)));

            // Check if there are any unused textures
            if (textures.Count == 0)
                return Enumerable.Empty<GrhData>();

            // Create the GrhDatas
            List<GrhData> ret = new List<GrhData>();
            foreach (var texture in textures)
            {
                // Load the texture size from the file
                Vector2 size = GetTextureSize(texture);

                // Go back to the relative path, and use it to figure out the categorization
                string relative = TextureAbsoluteToRelativePath(trimLen, texture);

                int lastSep = relative.LastIndexOf('/');
                string category = relative.Substring(0, lastSep);
                string title = relative.Substring(lastSep + 1);

                // Create the GrhData
                var gd = GrhInfo.CreateGrhData(cm, category, title, relative, Vector2.Zero, size);
                ret.Add(gd);
            }

            return ret;
        }

        public static void UpdateAnimated(string rootGrhDir)
        {
            throw new NotImplementedException();
        }
    }
}
