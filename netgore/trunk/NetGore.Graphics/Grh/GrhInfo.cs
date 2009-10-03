using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NetGore;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics
{
    public delegate void GrhDataEventHandler(GrhData grhData);

    /// <summary>
    /// Holds the GrhDatas and related methods.
    /// </summary>
    public static class GrhInfo
    {
        const string _animatedGrhDatasNodeName = "Animated";
        const string _nonAnimatedGrhDatasNodeName = "Stationary";
        const string _rootNodeName = "GrhDatas";

        /// <summary>
        /// Dictionary of categories, which contains a dictionary of all the names of the GrhDatas in
        /// that category, which contains the GrhData of that given name and category
        /// </summary>
        static readonly Dictionary<string, Dictionary<string, GrhData>> _catDic;

        /// <summary>
        /// The StringComparer used for the GrhData dictionaries. This must be used for all of the
        /// dictionaries created for the GrhData categorization.
        /// </summary>
        static readonly StringComparer _comparer = StringComparer.InvariantCultureIgnoreCase;

        /// <summary>
        /// List of all the GrhData where the array index is the GrhIndex
        /// </summary>
        static DArray<GrhData> _grhDatas;

        /// <summary>
        /// Notifies listeners when a GrhData has been added.
        /// </summary>
        public static event GrhDataEventHandler OnAdd;

        /// <summary>
        /// Notifies listeners when a GrhData has been removed.
        /// </summary>
        public static event GrhDataEventHandler OnRemove;

        /// <summary>
        /// Gets an IEnumerable of all of the GrhDatas.
        /// </summary>
        public static IEnumerable<GrhData> GrhDatas
        {
            get
            {
                if (_grhDatas == null)
                    return Enumerable.Empty<GrhData>();

                return _grhDatas;
            }
        }

        static GrhInfo()
        {
            _catDic = new Dictionary<string, Dictionary<string, GrhData>>(_comparer);
        }

        /// <summary>
        /// Adds a GrhData to the list of GrhDatas at the index assigned to it.
        /// </summary>
        /// <param name="gd">GrhData to add</param>
        internal static void AddGrhData(GrhData gd)
        {
            if (gd == null)
                throw new ArgumentNullException("gd");

            GrhIndex index = gd.GrhIndex;

#if DEBUG
            // Check if a GrhData will be overwritten
            if (_grhDatas.CanGet((int)index))
            {
                GrhData currentGD = GetData(index);
                if (currentGD != null && currentGD != gd)
                    Debug.Fail("Existing GrhData is going to be overwritten. This is likely not what was intended.");
            }
#endif

            _grhDatas[(int)index] = gd;

            // Make sure the GrhData is only in the list once
            Debug.Assert(GrhDatas.Where(x => x == gd).Count() == 1,
                         "The GrhData should be in the list only once. Somehow, its in there either more times, or not at all.");
        }

        /// <summary>
        /// Handles when a GrhData is added to the GrhDatas DArray
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        static void AddHandler(object sender, DArrayModifyEventArgs<GrhData> e)
        {
            GrhData gd = e.Item;
            if (gd == null)
            {
                Debug.Fail("gd is null.");
                return;
            }

            // Set the categorization event so we can keep up with any changes to the categorization.
            gd.OnChangeCategorization += ChangeCategorizationHandler;

            // If the title and category are not null, which would happen if the GrhData
            // was created and initialized, THEN added to the array, then add it to the dictionary
            if (gd.Category != null && gd.Title != null)
                AddToDictionary(gd);

            if (OnAdd != null)
                OnAdd(gd);
        }

        /// <summary>
        /// Adds a GrhData to the dictionary
        /// </summary>
        /// <param name="gd">GrhData to add to the dictionary</param>
        static void AddToDictionary(GrhData gd)
        {
            Dictionary<string, GrhData> titleDic;

            // Check if the category exists
            if (!_catDic.TryGetValue(gd.Category, out titleDic))
            {
                // Category does not exist, so create and add it
                titleDic = new Dictionary<string, GrhData>(_comparer);
                _catDic.Add(gd.Category, titleDic);
            }

            // Add the GrhData to the sub-dictionary by its title
            titleDic.Add(gd.Title, gd);
        }

        /// <summary>
        /// Handles when the category of a GrhData in the DArray changes
        /// </summary>
        static void ChangeCategorizationHandler(GrhData sender, string oldCategory, string oldTitle)
        {
            GrhData gd = sender;
            if (gd == null)
            {
                Debug.Fail("gd is null.");
                return;
            }

            RemoveFromDictionary(gd);
            AddToDictionary(gd);
        }

        public static GrhData CreateGrhData(GrhIndex[] frames, float speed, string category, string title)
        {
            if (category == null)
                category = string.Empty;
            if (title == null)
                throw new ArgumentNullException("title");

            category = SanitizeCategory(category);

            GrhIndex grhIndex = NextFreeIndex();
            GrhData gd = new GrhData();
            gd.Load(grhIndex, frames, speed, category, title);
            AddGrhData(gd);
            return gd;
        }

        public static GrhData CreateGrhData(ContentManager contentManager, string category, string title, string texture,
                                            Vector2 pos, Vector2 size)
        {
            return CreateGrhData(NextFreeIndex(), contentManager, category, title, texture, pos, size);
        }

        public static GrhData CreateGrhData(ContentManager contentManager, string category)
        {
            GrhIndex index = NextFreeIndex();
            string title = "tmp" + index;
            return CreateGrhData(index, contentManager, category, title, string.Empty, Vector2.Zero, Vector2.Zero);
        }

        static GrhData CreateGrhData(GrhIndex grhIndex, ContentManager contentManager, string category, string title,
                                     string texture, Vector2 pos, Vector2 size)
        {
            if (category == null)
                category = string.Empty;
            if (title == null)
                throw new ArgumentNullException("title");

            category = SanitizeCategory(category);

            GrhData gd = new GrhData();
            gd.Load(contentManager, grhIndex, texture, (int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y, category, title);
            AddGrhData(gd);
            return gd;
        }

        /// <summary>
        /// Deletes a GrhData.
        /// </summary>
        /// <param name="grhData">GrhData to delete.</param>
        public static void Delete(GrhData grhData)
        {
            if (grhData == null)
                throw new ArgumentNullException("grhData");

            Delete(grhData.GrhIndex);
        }

        /// <summary>
        /// Deletes a GrhData.
        /// </summary>
        /// <param name="grhIndex">Index of the GrhData to delete.</param>
        public static void Delete(GrhIndex grhIndex)
        {
            if (!_grhDatas.CanGet((int)grhIndex))
                return;

            _grhDatas.RemoveAt((int)grhIndex);
        }

        /// <summary>
        /// Finds all of the GrhDatas that reference a texture that does not exist.
        /// </summary>
        /// <returns>IEnumerable of all of the GrhDatas that reference a texture that does not exist.</returns>
        public static IEnumerable<GrhData> FindMissingTextures()
        {
            var nonanimated = GrhDatas.Where(x => !x.IsAnimated);
            var invalidTexture =
                nonanimated.Where(
                    x => string.IsNullOrEmpty(x.TextureName) || !File.Exists(ContentPaths.Build.Grhs.Join(x.TextureName) + ".xnb"));
            return invalidTexture;
        }

        /// <summary>
        /// Gets the GrhData by the given information.
        /// </summary>
        /// <param name="categoryAndTitle">Concatenated GrhData category and title.</param>
        /// <returns>GrhData matching the given information if found, or null if no matches.</returns>
        public static GrhData GetData(string categoryAndTitle)
        {
            string category, title;
            SplitCategoryAndTitle(categoryAndTitle, out category, out title);
            return GetData(category, title);
        }

        /// <summary>
        /// Gets the GrhData by the given information
        /// </summary>
        /// <param name="category">Category of the GrhData</param>
        /// <param name="title">Title of the GrhData</param>
        /// <returns>GrhData matching the given information if found, or null if no matches</returns>
        public static GrhData GetData(string category, string title)
        {
            if (string.IsNullOrEmpty(category))
                throw new ArgumentNullException("category");

            category = SanitizeCategory(category);

            // Get the dictionary for the category
            var titleDic = GetDatas(category);

            // If we failed to get it, return null
            if (titleDic == null)
                return null;

            // Try and get the GrhData for the given title, returning the GrhData
            // if it exists, or null if it does not exist in the dictionary
            GrhData ret;
            if (titleDic.TryGetValue(title, out ret))
                return ret;
            else
                return null;
        }

        /// <summary>
        /// Gets the GrhData by the given information
        /// </summary>
        /// <param name="grhIndex">GrhIndex of the GrhData</param>
        /// <returns>GrhData matching the given information if found, or null if no matches</returns>
        public static GrhData GetData(GrhIndex grhIndex)
        {
            if (_grhDatas.CanGet((int)grhIndex))
                return _grhDatas[(int)grhIndex];
            else
                return null;
        }

        /// <summary>
        /// Gets the GrhData by the given information
        /// </summary>
        /// <param name="category">Category of the GrhData</param>
        /// <returns>Dictionary of all GrhDatas from the given category with their Title as the key, or
        /// null if the category was invalid</returns>
        public static Dictionary<string, GrhData> GetDatas(string category)
        {
            if (string.IsNullOrEmpty(category))
                throw new ArgumentNullException("category");

            category = SanitizeCategory(category);

            Dictionary<string, GrhData> titleDic;

            // Return the whole dictionary for the given catgory, or null if it does not exist
            if (_catDic.TryGetValue(category, out titleDic))
                return titleDic;
            else
                return null;
        }

        /// <summary>
        /// Gets the absolute file path to the GrhData file.
        /// </summary>
        /// <param name="contentPath">ContentPath to use.</param>
        /// <returns>The absolute file path to the GrhData file.</returns>
        public static string GetGrhDataFilePath(ContentPaths contentPath)
        {
            return contentPath.Data.Join("grhdata.xml");
        }

        /// <summary>
        /// Checks if the specified Grh texture exists.
        /// </summary>
        /// <param name="textureName">Name of the Grh texture to check.</param>
        /// <returns>True if a texture with the specified Grh texture exists.</returns>
        public static bool GrhTextureExists(string textureName)
        {
            if (string.IsNullOrEmpty(textureName))
                throw new ArgumentNullException("textureName");

            string filePath = GrhTextureNameToFile(textureName);

            return File.Exists(filePath);
        }

        /// <summary>
        /// Gets the texture name from the corresponding file.
        /// </summary>
        /// <param name="filePath">Path of the file to get the texture name for.</param>
        /// <returns>The texture name from the corresponding file.</returns>
        public static string GrhTextureNameFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");

            string grhsPath = ContentPaths.Build.Grhs;

            // Trim off the start (absolute directory) and last 4 characters (file suffix: ".xnb")
            string textureName = filePath.Substring(grhsPath.Length, filePath.Length - grhsPath.Length - 4);
            textureName.Replace('\\', '/');
            return textureName;
        }

        /// <summary>
        /// Gets the file name for the corresponding Grh texture.
        /// </summary>
        /// <param name="textureName">Name of the Grh texture to get the file name for.</param>
        /// <returns>The file name for the corresponding Grh texture.</returns>
        public static string GrhTextureNameToFile(string textureName)
        {
            if (string.IsNullOrEmpty(textureName))
                throw new ArgumentNullException("textureName");

            textureName = textureName.Replace('/', Path.DirectorySeparatorChar);
            return ContentPaths.Build.Grhs.Join(textureName + ".xnb");
        }

        /// <summary>
        /// Loads the GrhDatas. This must be called before trying to access or use any GrhDatas.
        /// </summary>
        /// <param name="contentPath">The ContentPaths to load the GrhDatas from.</param>
        /// <param name="cm">The ContentManager to use for loaded GrhDatas.</param>
        public static void Load(ContentPaths contentPath, ContentManager cm)
        {
            string path = GetGrhDataFilePath(contentPath);

            if (cm == null)
                throw new ArgumentNullException("cm");

            if (!File.Exists(path))
                throw new FileNotFoundException("GrhData file not found.", path);

            // Create the GrhData DArray
            _grhDatas = new DArray<GrhData>(1024, false);
            _catDic.Clear();
            _grhDatas.OnAdd += AddHandler;
            _grhDatas.OnRemove += RemoveHandler;

            // Read the GrhDatas (non-aniamted first, followed by animated)
            XmlValueReader reader = new XmlValueReader(path, _rootNodeName);

            var nonAnimatedGrhDatas = reader.ReadManyNodes(_nonAnimatedGrhDatasNodeName, x => new GrhData(x, cm));
            foreach (GrhData gd in nonAnimatedGrhDatas)
            {
                _grhDatas[(int)gd.GrhIndex] = gd;
            }

            var animatedGrhDatas = reader.ReadManyNodes(_animatedGrhDatasNodeName, x => new GrhData(x, cm));
            foreach (GrhData gd in animatedGrhDatas)
            {
                _grhDatas[(int)gd.GrhIndex] = gd;
            }

            // Trim down the GrhData array, mainly for the client since it will never add/remove any GrhDatas
            // while in the Client, and the Client is what counts, baby!
            _grhDatas.Trim();
        }

        /// <summary>
        /// Finds the next free GrhData
        /// </summary>
        /// <returns>Next free GrhData index</returns>
        public static GrhIndex NextFreeIndex()
        {
            if (_grhDatas.TrackFree)
                return new GrhIndex(_grhDatas.NextFreeIndex());

            // Start at the first index
            int i = 1;

            // Just loop through the indicies until we find one thats not in use or
            // passes the length of the GrhDatas array (which means its obviously not in use)
            while (_grhDatas.CanGet(i) && _grhDatas[i] != null)
            {
                i++;
            }

            return new GrhIndex(i);
        }

        /// <summary>
        /// Removes a GrhData from the dictionary
        /// </summary>
        /// <param name="gd">GrhData to remove</param>
        static void RemoveFromDictionary(GrhData gd)
        {
            Dictionary<string, GrhData> titleDic;
            if (_catDic.TryGetValue(gd.Category, out titleDic))
                titleDic.Remove(gd.Title);
        }

        /// <summary>
        /// Handles when a GrhData is removed from the DArray
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        static void RemoveHandler(object sender, DArrayModifyEventArgs<GrhData> e)
        {
            RemoveFromDictionary(e.Item);

            if (OnRemove != null)
                OnRemove(e.Item);
        }

        public static string SanitizeCategory(string category)
        {
            return category.Replace('/', '.').Replace('\\', '.');
        }

        /// <summary>
        /// Saves all of the GrhData information to the specified file.
        /// </summary>
        /// <param name="contentPath">ContentPath to save the GrhData to.</param>
        public static void Save(ContentPaths contentPath)
        {
            string path = GetGrhDataFilePath(contentPath);

            string tempPath = path + ".temp";

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            // Organize the GrhDatas by if they are animated or not
            var nonNullGrhDatas = GrhDatas.Where(x => x != null);
            var nonAnimatedGrhDatas = nonNullGrhDatas.Where(x => !x.IsAnimated).ToArray();
            var animatedGrhDatas = nonNullGrhDatas.Where(x => x.IsAnimated).ToArray();

            // Write
            using (IValueWriter writer = new XmlValueWriter(tempPath, _rootNodeName))
            {
                writer.WriteManyNodes(_nonAnimatedGrhDatasNodeName, nonAnimatedGrhDatas, ((w, item) => item.Write(w)));
                writer.WriteManyNodes(_animatedGrhDatasNodeName, animatedGrhDatas, ((w, item) => item.Write(w)));
            }

            // Now that the temporary file has been successfully written, replace the existing file with it
            File.Delete(path);
            File.Move(tempPath, path);
        }

        public static void SplitCategoryAndTitle(string categoryAndTitle, out string category, out string title)
        {
            if (string.IsNullOrEmpty(categoryAndTitle))
                throw new ArgumentNullException("categoryAndTitle");

            categoryAndTitle = SanitizeCategory(categoryAndTitle);

            int lastSep = categoryAndTitle.LastIndexOf('.');
            if (lastSep == -1)
            {
                // If there is no seperator, we'll just assume there is no category and the whole thing is the title
                category = string.Empty;
                title = categoryAndTitle;
            }
            else
            {
                // Split at the last separator
                category = categoryAndTitle.Substring(0, lastSep);
                title = categoryAndTitle.Substring(lastSep + 1);
            }
        }
    }
}