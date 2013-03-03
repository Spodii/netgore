using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SFML.Graphics;
using log4net;
using NetGore.Content;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// Handles the automatic GrhData generation.
    /// </summary>
    public static class AutomaticGrhDataUpdater
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly string[] _graphicFileSuffixes = {
            ".bmp", ".jpg", ".jpeg", ".dds", ".psd", ".png", ".gif", ".tga", ".hdr"
        };

        /// <summary>
        /// Updates all of the automaticly added GrhDatas.
        /// </summary>
        /// <param name="cm"><see cref="IContentManager"/> to use for new GrhDatas.</param>
        /// <param name="rootGrhDir">Root Grh texture directory.</param>
        /// <param name="added">The GrhDatas that were added (empty if none were added).</param>
        /// <param name="deleted">The GrhDatas that were deleted (empty if none were added).</param>
        /// <param name="grhDataFileTags">The file tags for the corresponding GrhDatas.</param>
        /// <returns>
        /// IEnumerable of all of the new GrhDatas created.
        /// </returns>
        public static void Update(IContentManager cm, string rootGrhDir, out GrhData[] added, out GrhData[] deleted, out Dictionary<GrhData, GrhData.FileTags> grhDataFileTags)
        {
            if (!rootGrhDir.EndsWith("\\") && !rootGrhDir.EndsWith("/"))
                rootGrhDir += "/";

            // Clear the temporary content to make sure we have plenty of working memory
            cm.Unload(ContentLevel.Temporary, true);

            // Get the relative file path for all files up-front (only do this once since it doesn't scale well)
            var relFilePaths = Directory.GetFiles(rootGrhDir, "*", SearchOption.AllDirectories)
                .Select(x => x.Replace('\\', '/').Substring(rootGrhDir.Length))
                .ToArray();

            // Also grab the existing GrhDatas
            var existingGrhDatas = GrhInfo.GrhDatas.ToDictionary(x => x.Categorization.ToString(), x => x);

            // Go through each file and do the adds
            grhDataFileTags = new Dictionary<GrhData, GrhData.FileTags>();
            HashSet<GrhData> addedGrhDatas = new HashSet<GrhData>();
            HashSet<GrhData> deletedGrhDatas = new HashSet<GrhData>();
            HashSet<GrhData> grhDatasToDelete = new HashSet<GrhData>(existingGrhDatas.Values);
            HashSet<string> checkedAnimationRelDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var relFilePath in relFilePaths)
            {
                // Before doing anything else, ensure it is a valid file type to handle
                string fileExtension = Path.GetExtension(relFilePath);
                if (!_graphicFileSuffixes.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                    continue;

                string absFilePath = rootGrhDir + relFilePath;

                // Grab some stuff based on the file path
                string absDir = Path.GetDirectoryName(absFilePath);
                if (rootGrhDir.Length >= absDir.Length)
                    continue;

                string relDir = absDir.Substring(rootGrhDir.Length);
                string parentDirName = absDir.Substring(Path.GetDirectoryName(absDir).Length + 1);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(relFilePath);

                bool isAnimationFrame = parentDirName.StartsWith("_");

                if (!isAnimationFrame)
                {
                    // ** Stationary **

                    var fileTags = GrhData.FileTags.Create(fileNameWithoutExtension);

                    // Build the categorization info
                    string category = relDir.Replace("/", SpriteCategorization.Delimiter).Replace("\\", SpriteCategorization.Delimiter);
                    SpriteCategorization cat = new SpriteCategorization(category, fileTags.Title);

                    // Get existing
                    GrhIndex? grhIndex = null;
                    GrhData grhData;
                    if (existingGrhDatas.TryGetValue(cat.ToString(), out grhData))
                    {
                        grhDatasToDelete.Remove(grhData);
                    }

                    // If already exists as animated, delete first
                    if (grhData != null && (grhData is AnimatedGrhData || grhData is AutomaticAnimatedGrhData))
                    {
                        grhIndex = grhData.GrhIndex; // We will re-use this GrhIndex
                        GrhInfo.Delete(grhData);
                        deletedGrhDatas.Add(grhData);
                        grhData = null;
                    }

                    // Add new
                    string texturePath = "/" + relFilePath.Substring(0, relFilePath.Length - Path.GetExtension(absFilePath).Length);
                    if (grhData == null)
                    {
                        grhData = GrhInfo.CreateGrhData(cm, cat, texturePath, grhIndex);
                        addedGrhDatas.Add(grhData);
                    }
                    else
                    {
                        // Make sure the texture is correct
                        string currTextureName = "/" + ((StationaryGrhData)grhData).TextureName.ToString().TrimStart('/', '\\');
                        if (currTextureName != texturePath)
                        {
                            ((StationaryGrhData)grhData).ChangeTexture(texturePath);
                        }
                    }

                    // Ensure set to auto-size
                    StationaryGrhData stationaryGrhData = (StationaryGrhData)grhData;
                    if (!stationaryGrhData.AutomaticSize)
                    {
                        stationaryGrhData.AutomaticSize = true;
                    }

                    // Add to GrhDataFileTags
                    if (grhDataFileTags.ContainsKey(grhData))
                    {
                        throw new GrhDataException(grhData, string.Format("Found more than one stationary GrhData with the categorization: `{0}`." +
                            " Make sure that you do not have multiple image files in /DevContent/ in the same folder with the same name but different extensions (e.g. Sprite.png and Sprite.jpg).",
                            grhData.Categorization));
                    }

                    grhDataFileTags.Add(grhData, fileTags);
                }
                else
                {
                    // ** Animated **

                    // Make sure we only handle each animation once (since this will get called for each frame since we're looping over the files)
                    if (!checkedAnimationRelDirs.Add(relDir))
                        continue;

                    var fileTags = GrhData.FileTags.Create(parentDirName.Substring(1)); // Remove the _ prefix from directory name

                    // Build the categorization
                    string category = Path.GetDirectoryName(absDir).Substring(rootGrhDir.Length)
                        .Replace("/", SpriteCategorization.Delimiter).Replace("\\", SpriteCategorization.Delimiter);
                    SpriteCategorization cat = new SpriteCategorization(category, fileTags.Title);

                    // Get existing
                    GrhIndex? grhIndex = null;
                    GrhData grhData;
                    if (existingGrhDatas.TryGetValue(cat.ToString(), out grhData))
                    {
                        grhDatasToDelete.Remove(grhData);

                        // If already exists as stationary, delete first
                        if (grhData is StationaryGrhData)
                        {
                            grhIndex = grhData.GrhIndex; // We will re-use this GrhIndex
                            GrhInfo.Delete(grhData);
                            deletedGrhDatas.Add(grhData);
                            grhData = null;
                        }
                    }

                    // Add new
                    if (grhData == null)
                    {
                        grhData = GrhInfo.CreateAutomaticAnimatedGrhData(cm, cat, grhIndex);
                        addedGrhDatas.Add(grhData);
                    }

                    // Add to GrhDataFileTags
                    if (grhDataFileTags.ContainsKey(grhData))
                    {
                        throw new GrhDataException(grhData, string.Format("Found more than one animated GrhData with the categorization: `{0}`." +
                            " Make sure that you do not have multiple sub-folders in /DevContent/ in the same folder with the same name but different folder tags (e.g. /_Sprite[s100]/ and /_Sprite[s200]/).",
                            grhData.Categorization));
                    }

                    grhDataFileTags.Add(grhData, fileTags);
                }
            }

            // Now check if there are any GrhDatas to be deleted by taking existing GrhDatas, getting their relative path, and
            // see if that exists in our relative path list we built earlier against the file system
            foreach (var toDelete in grhDatasToDelete)
            {
                GrhInfo.Delete(toDelete);
                deletedGrhDatas.Add(toDelete);
            }

            if (log.IsInfoEnabled)
                log.WarnFormat("Automatic GrhData creation update resulted in `{0}` new GrhData(s) and `{1}` deleted GrhData(s).", addedGrhDatas.Count, deletedGrhDatas.Count);

            added = addedGrhDatas.ToArray();
            deleted = deletedGrhDatas.ToArray();
        }
    }
}