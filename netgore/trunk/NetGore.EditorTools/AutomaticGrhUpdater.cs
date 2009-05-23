using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Handles the automatic GrhData generation.
    /// </summary>
    public static class AutomaticGrhUpdater
    {
        /// <summary>
        /// The default speed of a new animation when no speed is specified.
        /// </summary>
        public const int DefaultAnimationSpeed = 400;

        /// <summary>
        /// Finds the directories used to store the frames for automatic animations.
        /// </summary>
        /// <param name="rootDir">Root directory to search from.</param>
        /// <returns>List of AnimationRegexInfos for each of the directories with the proper syntax for
        /// containing the frames of an animation.</returns>
        static List<AnimationRegexInfo> FindFrameDirs(string rootDir)
        {
            // The regex to match against the frames folders for automatic animations
            const string matchRegex = ".+_(?<Title>.+)_frames_?(?<Speed>\\d+)?";
            Regex r = new Regex(matchRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Get all directories
            var dirs = Directory.GetDirectories(rootDir, "*", SearchOption.AllDirectories);

            // Filter out only those that match our regex
            var ret = new List<AnimationRegexInfo>();
            foreach (string dir in dirs)
            {
                Match m = r.Match(dir);
                if (!m.Success)
                    continue;

                string title = m.Groups["Title"].Value;
                int speed = DefaultAnimationSpeed; // HACK: Default speed for new animations
                if (m.Groups["Speed"].Success)
                    speed = int.Parse(m.Groups["Speed"].Value);

                ret.Add(new AnimationRegexInfo(dir, title, speed));
            }

            return ret;
        }

        /// <summary>
        /// Finds the indices of the GrhDatas that use the textures in the specified directory.
        /// </summary>
        /// <param name="trimLen">Amount to trim off of the absolute path to get the relative
        /// path (use GetRelativeTrimLength()).</param>
        /// <param name="dir">Directory containing the textures to find the GrhDatas for.</param>
        /// <returns>Indices of the GrhDatas for the textures in the specified <paramref name="dir"/>, ordered
        /// by their filename parsed as an int.</returns>
        static ushort[] FindFrameIndices(int trimLen, string dir)
        {
            // Get all of the texture files
            var files = Directory.GetFiles(dir, "*.png", SearchOption.TopDirectoryOnly);
            if (files.Count() == 0)
                return null;

            // Take the file names and try to parse them as an int
            var fileInts = new List<KeyValuePair<int, string>>(files.Count());
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                int i;
                if (int.TryParse(fileName, out i))
                    fileInts.Add(new KeyValuePair<int, string>(i, file));
            }

            // Check that we even have anything still
            if (fileInts.Count == 0)
                return null;

            // Sort by the integer value, giving us our frame order, then grab just the path
            var goodFiles = fileInts.OrderBy(x => x.Key).Select(x => x.Value);

            // Find the corresponding GrhDatas
            var ret = new List<ushort>();

            foreach (string file in goodFiles)
            {
                string relativePath = TextureAbsoluteToRelativePath(trimLen, file);
                string category;
                string title;
                GrhInfo.SplitCategoryAndTitle(relativePath, out category, out title);

                // Because the frames should be automatically generated, we should know exactly where it is
                // just from the filepath and not have to search through the GrhDatas
                GrhData gd = GrhInfo.GetData(category, title);

                // Ensure the GrhData exists
                if (gd == null)
                {
                    Debug.Fail(string.Format("Failed to find frames for GrhData `{0}.{1}`", category, title));
                    return null;
                }

                ret.Add(gd.GrhIndex);
            }

            // Return the indices
            return ret.ToArray();
        }

        /// <summary>
        /// Finds all of the texture files from the root directory.
        /// </summary>
        /// <param name="rootDir">Root directory to search from.</param>
        /// <returns>List of the complete file paths from the <paramref name="rootDir"/>.</returns>
        static List<string> FindTextures(string rootDir)
        {
            var filePaths = Directory.GetFiles(rootDir, "*.png", SearchOption.AllDirectories);
            var ret = new List<string>(filePaths.Count());
            foreach (string filePath in filePaths)
            {
                ret.Add(filePath.Replace('\\', '/'));
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
            foreach (GrhData gd in GrhInfo.GrhDatas.Where(x => !x.IsAnimated))
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
        /// Updates all of the automatic GrhDatas.
        /// </summary>
        /// <param name="cm">ContentManager to use for new GrhDatas.</param>
        /// <param name="rootGrhDir">Root Grh texture directory.</param>
        /// <returns>IEnumerable of all of the new GrhDatas created.</returns>
        public static IEnumerable<GrhData> UpdateAll(ContentManager cm, string rootGrhDir)
        {
            return UpdateStationary(cm, rootGrhDir).Concat(UpdateAnimated(rootGrhDir));
        }

        /// <summary>
        /// Updates all of the automatic GrhDatas.
        /// </summary>
        /// <param name="rootGrhDir">Root Grh texture directory.</param>
        /// <returns>IEnumerable of all of the new GrhDatas created.</returns>
        public static IEnumerable<GrhData> UpdateAnimated(string rootGrhDir)
        {
            int trimLen = GetRelativeTrimLength(rootGrhDir);

            // Find the frame directories for auto-animations
            var frameDirInfos = FindFrameDirs(rootGrhDir);

            var ret = new List<GrhData>();
            foreach (AnimationRegexInfo frameDirInfo in frameDirInfos)
            {
                // Convert the parent path to the relative directory, then get the info from that
                DirectoryInfo parentDir = Directory.GetParent(frameDirInfo.Dir);
                if (parentDir == null)
                {
                    Debug.Fail(string.Format("Directory.GetParent() failed on `{0}`", frameDirInfo.Dir));
                    continue;
                }

                // Get the categorization
                string category = parentDir.FullName.Substring(trimLen);
                string title = frameDirInfo.Title;

                // Get the GrhIndices of the frames for the animation
                var indices = FindFrameIndices(trimLen, frameDirInfo.Dir);
                if (indices == null)
                    continue;

                // If the GrhData does not already exist, create it
                // If it does exist, update the frames and speed
                GrhData gd = GrhInfo.GetData(category, title);
                if (gd == null)
                {
                    // Create the new GrhData
                    gd = GrhInfo.CreateGrhData(indices, frameDirInfo.Speed, category, title);
                    gd.AutomaticSize = true;
                    ret.Add(gd);
                }
                else
                {
                    // Re-load the GrhData with the new values
                    gd.Load(gd.GrhIndex, indices, frameDirInfo.Speed, category, title);
                }
            }

            return ret;
        }

        /// <summary>
        /// Updates all of the automatic GrhDatas.
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
                string category;
                string title;
                GrhInfo.SplitCategoryAndTitle(relative, out category, out title);

                // Ensure the GrhData doesn't already exist
                if (GrhInfo.GetData(category, title) != null)
                    continue;

                // Load the texture size from the file
                Vector2 size = GetTextureSize(texture);

                // Create the GrhData
                GrhData gd = GrhInfo.CreateGrhData(cm, category, title, relative, Vector2.Zero, size);
                gd.AutomaticSize = true;
                ret.Add(gd);
            }

            return ret;
        }

        /// <summary>
        /// Structure describing a directory containing frames for an automatic animation.
        /// </summary>
        struct AnimationRegexInfo
        {
            /// <summary>
            /// Absolute path to the frames directory.
            /// </summary>
            public readonly string Dir;

            /// <summary>
            /// Speed to give the animation.
            /// </summary>
            public readonly int Speed;

            /// <summary>
            /// Title to give the animation.
            /// </summary>
            public readonly string Title;

            /// <summary>
            /// AnimationRegexInfo constructor.
            /// </summary>
            /// <param name="dir">Absolute path to the frames directory.</param>
            /// <param name="title">Title to give the animation.</param>
            /// <param name="speed">Speed to give the animation.</param>
            public AnimationRegexInfo(string dir, string title, int speed)
            {
                Dir = dir;
                Title = title;
                Speed = speed;
            }
        }
    }
}