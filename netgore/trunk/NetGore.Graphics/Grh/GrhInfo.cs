using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NetGore.Collections;
using NetGore.Globalization;

namespace NetGore.Graphics
{
    public delegate void GrhDataEventHandler(GrhData grhData);

    /// <summary>
    /// Holds the GrhDatas and related methods.
    /// </summary>
    public static class GrhInfo
    {
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
        /// Loads all of the GrhData information
        /// </summary>
        /// <param name="path">Path to the file containing the GrhData information</param>
        /// <param name="cm">ContentManager to apply to the created GrhDatas</param>
        public static void Load(string path, ContentManager cm)
        {
            if (cm == null)
                throw new ArgumentNullException("cm");

            if (!File.Exists(path))
                throw new ArgumentException("Specified file path does not exist.", "path");

            // Create the GrhData DArray
            _grhDatas = new DArray<GrhData>(1024, false);
            _catDic.Clear();
            _grhDatas.OnAdd += AddHandler;
            _grhDatas.OnRemove += RemoveHandler;

            // Load the GrhData from the file
            var grhDataFile = XmlInfoReader.ReadFile(path, true);
            if (grhDataFile.Count <= 0)
                throw new Exception("Error loading GrhData information from the specified file.");

            var animatedGrhDatas = new Stack<GrhData>(128);

            // Load the information into the objects
            foreach (var dic in grhDataFile)
            {
                GrhIndex currGrhIndex = Parser.Invariant.ParseGrhIndex(dic["Grh.Index"]);
                GrhData currGrh = new GrhData();
                int numFrames = dic.AsInt("Grh.Frames.Count");

                // Categorization
                string category = dic["Grh.Category.Name"];
                string title = dic["Grh.Category.Title"];

                if (numFrames == 1)
                {
                    // Single frame
                    string file = dic["Grh.Texture.File"];
                    int x = dic.AsInt("Grh.Texture.X");
                    int y = dic.AsInt("Grh.Texture.Y");
                    int w = dic.AsInt("Grh.Texture.W");
                    int h = dic.AsInt("Grh.Texture.H");
                    currGrh.Load(cm, currGrhIndex, file, x, y, w, h, category, title);

                    currGrh.AutomaticSize = dic.AsBool("Grh.AutomaticResize", false);

                    // Add to the collection
                    _grhDatas[(int)currGrh.GrhIndex] = currGrh;
                }
                else
                {
                    // Multiple frames
                    float speed = dic.AsFloat("Grh.Anim.Speed");
                    var frames = new GrhIndex[numFrames];
                    for (int i = 0; i < frames.Length; i++)
                    {
                        frames[i] = Parser.Invariant.ParseGrhIndex(dic["Grh.Frames.F" + (i + 1)]);
                    }
                    currGrh.Load(currGrhIndex, frames, 1f / speed, category, title);

                    // Add to the stack for now
                    animatedGrhDatas.Push(currGrh);
                }
            }

            // Add all the animated GrhDatas to the collection now that we have the frames in
            foreach (GrhData currGrh in animatedGrhDatas)
            {
                _grhDatas[(int)currGrh.GrhIndex] = currGrh;
            }

            // Trim down the GrhData array, mainly for the client since it will never add/remove any GrhDatas
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
        /// <param name="path">Path of the file to save the GrhData information to.</param>
        public static void Save(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            string tempPath = path + ".temp";

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            // Open the FileStream
            using (FileStream stream = new FileStream(tempPath, FileMode.Create))
            {
                // Create the XmlWriter and settings
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };
                using (XmlWriter w = XmlWriter.Create(stream, settings))
                {
                    if (w == null)
                        throw new Exception("Failed to create XmlWriter for saving GrhInfo.");

                    // Begin
                    w.WriteStartDocument();
                    w.WriteStartElement("Grhs");

                    // Get all of the single and multiple frame GrhDatas
                    var nonNullDatas = GrhDatas.Where(gd => gd != null);
                    var singleFrames = nonNullDatas.Where(gd => gd.Frames == null);
                    var multiFrames = nonNullDatas.Where(gd => gd.Frames != null);

                    // Save the single frame GrhDatas first, then the animated GrhDatas
                    foreach (GrhData grhData in singleFrames)
                    {
                        grhData.Save(w);
                    }
                    foreach (GrhData grhData in multiFrames)
                    {
                        grhData.Save(w);
                    }

                    // End
                    w.WriteEndElement();
                    w.WriteEndDocument();
                    w.Flush();
                }
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