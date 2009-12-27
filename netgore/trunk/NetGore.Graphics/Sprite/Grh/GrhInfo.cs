using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Holds the <see cref="GrhData"/>s and related methods.
    /// </summary>
    public static class GrhInfo
    {
        const string _animatedGrhDatasNodeName = "Animated";
        const string _nonAnimatedGrhDatasNodeName = "Stationary";
        const string _rootNodeName = "GrhDatas";

        /// <summary>
        /// Dictionary of the <see cref="SpriteCategory"/>, and a dictionary of all the <see cref="SpriteTitle"/>s
        /// and the corresponding <see cref="GrhData"/> for that <see cref="SpriteTitle"/> under the
        /// <see cref="SpriteCategory"/>.
        /// </summary>
        static readonly Dictionary<SpriteCategory, Dictionary<SpriteTitle, GrhData>> _catDic;

        /// <summary>
        /// List of all the <see cref="GrhData"/> where the array index is the <see cref="GrhIndex"/>.
        /// </summary>
        static DArray<GrhData> _grhDatas;

        /// <summary>
        /// If the <see cref="GrhData"/>s are currently being loaded
        /// </summary>
        static bool _isLoading;

        /// <summary>
        /// Notifies listeners when a <see cref="GrhData"/> has been added. This includes
        /// <see cref="GrhData"/>s added from loading.
        /// </summary>
        public static event GrhDataEventHandler OnAdd;

        /// <summary>
        /// Notifies listeners when a new <see cref="GrhData"/> has been added. This does not include
        /// <see cref="GrhData"/>s added from loading.
        /// </summary>
        public static event GrhDataEventHandler OnAddNew;

        /// <summary>
        /// Notifies listeners when a <see cref="GrhData"/> has been removed.
        /// </summary>
        public static event GrhDataEventHandler OnRemove;

        /// <summary>
        /// Initializes the <see cref="GrhInfo"/> class.
        /// </summary>
        static GrhInfo()
        {
            _catDic = new Dictionary<SpriteCategory, Dictionary<SpriteTitle, GrhData>>();
            Debug.Assert(EqualityComparer<SpriteCategory>.Default.Equals("asdf", "ASDF"), "Sprite category must not be case sensitive.");
            Debug.Assert(EqualityComparer<SpriteTitle>.Default.Equals("asdf", "ASDF"), "Sprite title must not be case sensitive.");
        }

        /// <summary>
        /// Gets an IEnumerable of all of the <see cref="GrhData"/>s.
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

        /// <summary>
        /// Adds a <see cref="GrhData"/> to the list of <see cref="GrhData"/>s at the index assigned to it.
        /// </summary>
        /// <param name="gd"><see cref="GrhData"/> to add</param>
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
        /// Handles when a <see cref="GrhData"/> is added to the <see cref="GrhData"/>s DArray.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
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

            AddToDictionary(gd);

            if (OnAdd != null)
                OnAdd(gd);

            if (!_isLoading && OnAddNew != null)
                OnAddNew(gd);
        }

        /// <summary>
        /// Adds a <see cref="GrhData"/> to the dictionary.
        /// </summary>
        /// <param name="gd"><see cref="GrhData"/> to add to the dictionary.</param>
        static void AddToDictionary(GrhData gd)
        {
            Dictionary<SpriteTitle, GrhData> titleDic;

            // Check if the category exists
            if (!_catDic.TryGetValue(gd.Categorization.Category, out titleDic))
            {
                // Category does not exist, so create and add it
                titleDic = new Dictionary<SpriteTitle, GrhData>();
                _catDic.Add(gd.Categorization.Category, titleDic);
            }

            // Add the GrhData to the sub-dictionary by its title
            titleDic.Add(gd.Categorization.Title, gd);
        }

        /// <summary>
        /// Handles when the category of a <see cref="GrhData"/> in the DArray changes.
        /// </summary>
        static void ChangeCategorizationHandler(GrhData sender, SpriteCategorization oldCategorization)
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

        public static GrhData CreateGrhData(GrhIndex[] frames, float speed, SpriteCategorization categorization)
        {
            GrhIndex grhIndex = NextFreeIndex();
            GrhData gd = new GrhData();
            gd.Load(grhIndex, frames, speed, categorization);
            AddGrhData(gd);
            return gd;
        }

        public static GrhData CreateGrhData(ContentManager contentManager, SpriteCategorization categorization, string texture,
                                            Vector2 pos, Vector2 size)
        {
            return CreateGrhData(NextFreeIndex(), contentManager, categorization, texture, pos, size);
        }

        public static GrhData CreateGrhData(ContentManager contentManager, SpriteCategory category)
        {
            var index = NextFreeIndex();
            var title = GetUniqueTitle(category, "tmp" + index);
            var categorization = new SpriteCategorization(category, title);
            return CreateGrhData(index, contentManager, categorization, string.Empty, Vector2.Zero, Vector2.Zero);
        }

        static GrhData CreateGrhData(GrhIndex grhIndex, ContentManager contentManager, SpriteCategorization categorization,
                                     string texture, Vector2 pos, Vector2 size)
        {
            Rectangle source = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);

            GrhData gd = new GrhData();
            gd.Load(contentManager, grhIndex, texture, source, categorization);
            AddGrhData(gd);
            return gd;
        }

        /// <summary>
        /// Deletes a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData"><see cref="GrhData"/> to delete.</param>
        public static void Delete(GrhData grhData)
        {
            if (grhData == null)
                throw new ArgumentNullException("grhData");

            Delete(grhData.GrhIndex);
        }

        /// <summary>
        /// Deletes a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhIndex">Index of the <see cref="GrhData"/> to delete.</param>
        public static void Delete(GrhIndex grhIndex)
        {
            if (!_grhDatas.CanGet((int)grhIndex))
                return;

            _grhDatas.RemoveAt((int)grhIndex);
        }

        /// <summary>
        /// Finds all of the <see cref="GrhData"/>s that reference a texture that does not exist.
        /// </summary>
        /// <returns>IEnumerable of all of the <see cref="GrhData"/>s that reference a texture that does not exist.</returns>
        public static IEnumerable<GrhData> FindMissingTextures()
        {
            var nonanimated = GrhDatas.Where(x => !x.IsAnimated);
            var invalidTextures = nonanimated.Where(x => !x.TextureName.ContentExists());
            return invalidTextures;
        }

        /// <summary>
        /// Gets an IEnumerable of all the <see cref="GrhData"/> categories.
        /// </summary>
        /// <returns>An IEnumerable of all the <see cref="GrhData"/> categories.</returns>
        public static IEnumerable<SpriteCategory> GetCategories()
        {
            return _catDic.Keys;
        }

        /// <summary>
        /// Gets the <see cref="GrhData"/> by the given categorization information.
        /// </summary>
        /// <param name="category">Category of the <see cref="GrhData"/>.</param>
        /// <param name="title">Title of the <see cref="GrhData"/>.</param>
        /// <returns><see cref="GrhData"/> matching the given information if found, or null if no matches.</returns>
        public static GrhData GetData(SpriteCategory category, SpriteTitle title)
        {
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
        /// Gets the <see cref="GrhData"/> by the given categorization information.
        /// </summary>
        /// <param name="categorization">Categorization of the <see cref="GrhData"/>.</param>
        /// <returns><see cref="GrhData"/> matching the given information if found, or null if no matches.</returns>
        public static GrhData GetData(SpriteCategorization categorization)
        {
            return GetData(categorization.Category, categorization.Title);
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
        /// Gets the <see cref="GrhData"/>s by the given information.
        /// </summary>
        /// <param name="category">Category of the <see cref="GrhData"/>s.</param>
        /// <returns>All <see cref="GrhData"/>s from the given category with their Title as the key, or
        /// null if the <paramref name="category"/> was invalid.</returns>
        public static IDictionary<SpriteTitle, GrhData> GetDatas(SpriteCategory category)
        {
            Dictionary<SpriteTitle, GrhData> titleDic;

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
        /// Gets a unique category based off of an existing category.
        /// </summary>
        /// <param name="category">The category to base the new category name off of.</param>
        /// <returns>A unique, unused category.</returns>
        public static SpriteCategory GetUniqueCategory(SpriteCategory category)
        {
            return GetUniqueCategory(category.ToString());
        }

        /// <summary>
        /// Gets a unique category based off of an existing category.
        /// </summary>
        /// <param name="category">The category to base the new category name off of.</param>
        /// <returns>A unique, unused category.</returns>
        public static SpriteCategory GetUniqueCategory(string category)
        {
            int copyNum = 1;
            SpriteCategory newCategory;

            IDictionary<SpriteTitle, GrhData> coll;
            do
            {
                newCategory = new SpriteCategory(category + " (" + ++copyNum + ")");
            }
            while ((coll = GetDatas(newCategory)) != null && !coll.IsEmpty());

            return newCategory;
        }

        /// <summary>
        /// Gets a unique title for a <see cref="GrhData"/> in the given category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="title">The string to base the new title off of.</param>
        /// <returns>A unique title for the given <paramref name="category"/>.</returns>
        public static SpriteTitle GetUniqueTitle(SpriteCategory category, SpriteTitle title)
        {
            return GetUniqueTitle(category, title.ToString());
        }

        /// <summary>
        /// Gets a unique title for a <see cref="GrhData"/> in the given category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="title">The string to base the new title off of.</param>
        /// <returns>A unique title for the given <paramref name="category"/>.</returns>
        public static SpriteTitle GetUniqueTitle(SpriteCategory category, string title)
        {
            if (!string.IsNullOrEmpty(title))
                title = SpriteTitle.Sanitize(title);

            if (!SpriteTitle.IsValid(title))
                title = "tmp";

            // Check if the given title is already free
            var sTitle = new SpriteTitle(title);
            if (GetData(category, sTitle) == null)
                return sTitle;

            // Find next free number for the title suffix
            int copyNum = 1;
            SpriteTitle sNewTitle;
            do
            {
                copyNum++;
                sNewTitle = new SpriteTitle(title + " (" + copyNum + ")");
            }
            while (GetData(category, sNewTitle) != null);

            return sNewTitle;
        }

        /// <summary>
        /// Loads the <see cref="GrhData"/>s. This must be called before trying to access or use any
        /// <see cref="GrhData"/>s.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to load the <see cref="GrhData"/>s from.</param>
        /// <param name="cm">The <see cref="ContentManager"/> to use for loaded <see cref="GrhData"/>s.</param>
        public static void Load(ContentPaths contentPath, ContentManager cm)
        {
            string path = GetGrhDataFilePath(contentPath);

            if (cm == null)
                throw new ArgumentNullException("cm");

            if (!File.Exists(path))
                throw new FileNotFoundException("GrhData file not found.", path);

            try
            {
                _isLoading = true;

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
            finally
            {
                _isLoading = false;
            }
        }

        /// <summary>
        /// Finds the next free <see cref="GrhIndex"/>.
        /// </summary>
        /// <returns>The next free <see cref="GrhIndex"/>.</returns>
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
        /// Removes a <see cref="GrhData"/> from the dictionary.
        /// </summary>
        /// <param name="gd"><see cref="GrhData"/> to remove.</param>
        static void RemoveFromDictionary(GrhData gd)
        {
            Dictionary<SpriteTitle, GrhData> titleDic;
            if (_catDic.TryGetValue(gd.Categorization.Category, out titleDic))
                titleDic.Remove(gd.Categorization.Title);
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
    }
}