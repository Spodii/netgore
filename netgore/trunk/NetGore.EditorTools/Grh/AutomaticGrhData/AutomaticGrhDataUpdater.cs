using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Handles the automatic GrhData generation.
    /// </summary>
    public static class AutomaticGrhDataUpdater
    {
        /// <summary>
        /// Finds all of the texture files from the root directory.
        /// </summary>
        /// <param name="rootDir">Root directory to search from.</param>
        /// <returns>List of the complete file paths from the <paramref name="rootDir"/>.</returns>
        static List<string> FindTextures(string rootDir)
        {
            var ret = new List<string>();
            var dirs = GetStationaryDirectories(rootDir);

            foreach (var dir in dirs)
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    if (!file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        continue;

                    ret.Add(file.Replace('\\', '/'));
                }
            }

            return ret;
        }

        /// <summary>
        /// Finds all of the used textures.
        /// </summary>
        /// <returns>Dictionary where the key is the virtual texture name (relative path minus the file extension) and
        /// the value is a list of GrhDatas that use that texture.</returns>
        static Dictionary<string, List<GrhData>> FindUsedTextures()
        {
            var ret = new Dictionary<string, List<GrhData>>(StringComparer.OrdinalIgnoreCase);

            // Loop through every stationary GrhData
            foreach (var gd in GrhInfo.GrhDatas.OfType<StationaryGrhData>())
            {
                var textureName = gd.TextureName.ToString();
                List<GrhData> dictList;

                // Get the existing list, or create a new one if the first entry
                if (!ret.TryGetValue(textureName, out dictList))
                {
                    dictList = new List<GrhData>();
                    ret.Add(textureName, dictList);
                }

                // Add the GrhData to the list
                dictList.Add(gd);
            }

            return ret;
        }

        /// <summary>
        /// Gets the directories that are for automatic animations.
        /// </summary>
        /// <param name="rootDir">The root directory.</param>
        /// <returns>The directories that contain textures and are for automatic animations.</returns>
        static IEnumerable<string> GetAnimatedDirectories(string rootDir)
        {
            if (AutomaticAnimatedGrhData.IsAutomaticAnimatedGrhDataDirectory(rootDir))
                yield return rootDir;
            else
            {
                foreach (var dir in Directory.GetDirectories(rootDir))
                {
                    foreach (var dir2 in GetAnimatedDirectories(dir))
                    {
                        yield return dir2;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the amount to trim off of a directory to make it relative to the specified <paramref name="rootDir"/>.
        /// </summary>
        /// <param name="rootDir">Root directory to make other directories relative to.</param>
        /// <returns>The amount to trim off of a directory to make it relative to the specified
        /// <paramref name="rootDir"/>.</returns>
        static int GetRelativeTrimLength(string rootDir)
        {
            int len = rootDir.Length;
            if (!rootDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                len++;
            return len;
        }

        /// <summary>
        /// Gets the directories that are not for automatic animations.
        /// </summary>
        /// <param name="rootDir">The root directory.</param>
        /// <returns>The directories that contain textures that are not for automatic animations.</returns>
        static IEnumerable<string> GetStationaryDirectories(string rootDir)
        {
            yield return rootDir;

            foreach (var dir in Directory.GetDirectories(rootDir))
            {
                if (AutomaticAnimatedGrhData.IsAutomaticAnimatedGrhDataDirectory(dir))
                    continue;

                foreach (var dir2 in GetStationaryDirectories(dir))
                {
                    yield return dir2;
                }
            }
        }

        /// <summary>
        /// Gets the size of a texture.
        /// </summary>
        /// <param name="filePath">Absolute file path to the texture.</param>
        /// <returns>Size of the texture.</returns>
        static Vector2 GetTextureSize(string filePath)
        {
            TextureInformation info = Texture2D.GetTextureInformation(filePath);
            return new Vector2(info.Width, info.Height);
        }

        /// <summary>
        /// Converts an absolute path to a virtual texture path (relative path minus the extension).
        /// </summary>
        /// <param name="trimLen">Amount to trim off to make the path relative.</param>
        /// <param name="absolute">Absolute path to find the relative path of.</param>
        /// <returns>The relative path of the specified absolute path.</returns>
        static string TextureAbsoluteToRelativePath(int trimLen, string absolute)
        {
            absolute = absolute.Replace('\\', '/');

            // Trim down to the relative path
            string rel = absolute.Substring(trimLen);

            // Remove the file suffix since we don't use that
            int lastPeriod = rel.LastIndexOf('.');
            rel = rel.Substring(0, lastPeriod);

            return rel;
        }

        /// <summary>
        /// Updates all of the automaticly added GrhDatas.
        /// </summary>
        /// <param name="cm">ContentManager to use for new GrhDatas.</param>
        /// <param name="rootGrhDir">Root Grh texture directory.</param>
        /// <returns>IEnumerable of all of the new GrhDatas created.</returns>
        public static IEnumerable<GrhData> UpdateAll(ContentManager cm, string rootGrhDir)
        {
            return UpdateStationary(cm, rootGrhDir).Concat(UpdateAnimated(cm, rootGrhDir));
        }

        /// <summary>
        /// Updates the animated automaticly added GrhDatas.
        /// </summary>
        /// <param name="cm">ContentManager to use for new GrhDatas.</param>
        /// <param name="rootGrhDir">Root Grh texture directory.</param>
        /// <returns>IEnumerable of all of the new GrhDatas created.</returns>
        public static IEnumerable<GrhData> UpdateAnimated(ContentManager cm, string rootGrhDir)
        {
            List<GrhData> ret = new List<GrhData>();

            // Find all directories that match the needed pattern
            var dirs = GetAnimatedDirectories(rootGrhDir);

            foreach (var dir in dirs)
            {
                // Grab the animation info from the directory
                var animInfo = AutomaticAnimatedGrhData.GetAutomaticAnimationInfo(dir);
                if (animInfo == null)
                    continue;

                // Get the virtual directory (remove the root)
                var partialDir = dir.Substring(rootGrhDir.Length);
                if (partialDir.StartsWith(Path.DirectorySeparatorChar.ToString()) ||
                    partialDir.StartsWith(Path.AltDirectorySeparatorChar.ToString()))
                    partialDir = partialDir.Substring(1);

                // Get the categorization
                string categoryStr;
                int lastDirSep = partialDir.LastIndexOf(Path.DirectorySeparatorChar);
                if (lastDirSep < 0)
                    categoryStr = string.Empty;
                else
                    categoryStr = partialDir.Substring(0, lastDirSep);

                var categorization = new SpriteCategorization(new SpriteCategory(categoryStr), new SpriteTitle(animInfo.Title));

                // Create the AutomaticAnimatedGrhData
                var gd = GrhInfo.CreateAutomaticAnimatedGrhData(cm, categorization);
                if (gd != null)
                    ret.Add(gd);
            }

            return ret;
        }

        /// <summary>
        /// Updates the stationary automaticly added GrhDatas.
        /// </summary>
        /// <param name="cm">ContentManager to use for new GrhDatas.</param>
        /// <param name="rootGrhDir">Root Grh texture directory.</param>
        /// <returns>IEnumerable of all of the new GrhDatas created.</returns>
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
            var ret = new List<GrhData>();
            foreach (string texture in textures)
            {
                // Go back to the relative path, and use it to figure out the categorization
                string relative = TextureAbsoluteToRelativePath(trimLen, texture);
                var categorization = SpriteCategorization.SplitCategoryAndTitle(relative);

                // Ensure the GrhData doesn't already exist
                if (GrhInfo.GetData(categorization) != null)
                    continue;

                // Read the texture size from the file
                Vector2 size = GetTextureSize(texture);

                // Create the GrhData
                var gd = GrhInfo.CreateGrhData(cm, categorization, relative, Vector2.Zero, size);
                gd.AutomaticSize = true;
                ret.Add(gd);
            }

            return ret;
        }
    }
}