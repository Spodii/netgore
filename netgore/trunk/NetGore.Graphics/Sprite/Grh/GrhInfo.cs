using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
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
        const string _autoAnimatedGrhDatasNodeName = "AutomaticAnimated";
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
            Debug.Assert(EqualityComparer<SpriteCategory>.Default.Equals("asdf", "ASDF"),
                         "Sprite category must not be case sensitive.");
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
            Debug.Assert(e.Index != GrhIndex.Invalid);

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

        /// <summary>
        /// Creates a new <see cref="AutomaticAnimatedGrhData"/>. This should only be called from the
        /// AutomaticGrhDataUpdater.
        /// </summary>
        /// <param name="contentManager">The content manager.</param>
        /// <param name="categorization">The categorization for the <see cref="AutomaticAnimatedGrhData"/>.</param>
        /// <returns>The new <see cref="AutomaticAnimatedGrhData"/>, or null if none created.</returns>
        public static AutomaticAnimatedGrhData CreateAutomaticAnimatedGrhData(ContentManager contentManager,
                                                                              SpriteCategorization categorization)
        {
            // Check if the GrhData already exists
            if (GetData(categorization) != null)
                return null;

            var index = NextFreeIndex();
            var gd = new AutomaticAnimatedGrhData(contentManager, index, categorization);
            AddGrhData(gd);
            return gd;
        }

        public static AnimatedGrhData CreateGrhData(GrhIndex[] frames, float speed, SpriteCategorization categorization)
        {
            GrhIndex grhIndex = NextFreeIndex();
            var gd = new AnimatedGrhData(grhIndex, categorization);
            gd.SetSpeed(speed);
            gd.SetFrames(frames);
            AddGrhData(gd);
            return gd;
        }

        public static StationaryGrhData CreateGrhData(ContentManager contentManager, SpriteCategorization categorization,
                                                      string texture, Vector2 pos, Vector2 size)
        {
            return CreateGrhData(NextFreeIndex(), contentManager, categorization, texture, pos, size);
        }

        public static StationaryGrhData CreateGrhData(ContentManager contentManager, SpriteCategory category)
        {
            var index = NextFreeIndex();
            var title = GetUniqueTitle(category, "tmp" + index);
            var categorization = new SpriteCategorization(category, title);
            return CreateGrhData(index, contentManager, categorization, string.Empty, Vector2.Zero, Vector2.Zero);
        }

        static StationaryGrhData CreateGrhData(GrhIndex grhIndex, ContentManager contentManager,
                                               SpriteCategorization categorization, string texture, Vector2 pos, Vector2 size)
        {
            Debug.Assert(!grhIndex.IsInvalid);

            Rectangle source = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);

            var gd = new StationaryGrhData(contentManager, grhIndex, categorization);
            gd.ChangeTexture(texture, source);

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

            var grhIndex = grhData.GrhIndex;

            if (grhIndex.IsInvalid)
                return;

            // Insure the index is valid
            if (!_grhDatas.CanGet((int)grhIndex))
            {
                const string errmsg = "Attempted to delete GrhData `{0}`, but GrhIndex `{1}` could not be acquired.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, grhData, grhIndex);
                Debug.Fail(string.Format(errmsg, grhData, grhIndex));
                return;
            }
            
            // Make sure we are deleting the correct GrhData
            int i = (int)grhIndex;
            if (_grhDatas[i] != grhData)
            {
                const string errmsg = "Attempted to delete GrhData `{0}`, but GrhIndex `{1}` is already in use by" +
                    " a different GrhData `{2}`. Most likely, the GrhData we tried to delete is already deleted, and" +
                    " the GrhIndex has been recycled to a new GrhData. Stop trying to delete dead stuff, will ya!?";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, grhData, grhIndex, _grhDatas[i]);
                Debug.Fail(string.Format(errmsg, grhData, grhIndex, _grhDatas[i]));
                return;
            }

            _grhDatas.RemoveAt((int)grhIndex);
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Finds all of the <see cref="GrhData"/>s that reference a texture that does not exist.
        /// </summary>
        /// <returns>IEnumerable of all of the <see cref="GrhData"/>s that reference a texture that does not exist.</returns>
        public static IEnumerable<StationaryGrhData> FindMissingTextures()
        {
            var nonanimated = GrhDatas.OfType<StationaryGrhData>();
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
        /// Gets a <see cref="ContentManager"/>.
        /// </summary>
        /// <returns>A <see cref="ContentManager"/>.</returns>
        static ContentManager GetContentManager()
        {
            return GrhDatas.OfType<StationaryGrhData>().First(x => x.ContentManager != null).ContentManager;
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
            if (grhIndex.IsInvalid)
                return null;

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

            // Create a little delegate to add the GrhDatas by index to reduce code bloat without exposing this to other methods
            Action<IEnumerable<GrhData>> grhDataAdder = delegate(IEnumerable<GrhData> grhDatas)
            {
                foreach (var grhData in grhDatas)
                {
                    int index = (int)grhData.GrhIndex;
                    Debug.Assert(!_grhDatas.CanGet(index) || _grhDatas[index] == null, "Index already occupied!");

                    if (!grhData.GrhIndex.IsInvalid)
                    {
                        _grhDatas[index] = grhData;
                    }
                    else
                    {
                        const string errmsg = "Tried to add GrhData `{0}` which has an invalid GrhIndex.";
                        string err = string.Format(errmsg, grhData);
                        log.Fatal(err);
                        Debug.Fail(err);
                    }
                }
            };

            try
            {
                _isLoading = true;

                // Create the GrhData DArray
                if (_grhDatas == null)
                    _grhDatas = new DArray<GrhData>(256, false);
                else
                    _grhDatas.Clear();
                _catDic.Clear();
                _grhDatas.OnAdd += AddHandler;
                _grhDatas.OnRemove += RemoveHandler;

                // Read and add the GrhDatas in order by their type
                XmlValueReader reader = new XmlValueReader(path, _rootNodeName);

                grhDataAdder(reader.ReadManyNodes(_nonAnimatedGrhDatasNodeName, x => StationaryGrhData.Read(x, cm)));
                grhDataAdder(reader.ReadManyNodes(_animatedGrhDatasNodeName, x => AnimatedGrhData.Read(x)));
                grhDataAdder(reader.ReadManyNodes(_autoAnimatedGrhDatasNodeName, x => AutomaticAnimatedGrhData.Read(x, cm)));

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
            int i = GrhIndex.MinValue;

            // Just loop through the indicies until we find one thats not in use or
            // passes the length of the GrhDatas array (which means its obviously not in use)
            while (_grhDatas.CanGet(i) && _grhDatas[i] != null)
            {
                i++;
            }

            Debug.Assert(i != GrhIndex.Invalid);

            return new GrhIndex(i);
        }

        /// <summary>
        /// Removes a <see cref="GrhData"/> from the dictionary.
        /// </summary>
        /// <param name="gd"><see cref="GrhData"/> to remove.</param>
        static void RemoveFromDictionary(GrhData gd)
        {
            Dictionary<SpriteTitle, GrhData> titleDic;
            if (!_catDic.TryGetValue(gd.Categorization.Category, out titleDic))
            {
                const string errmsg = "Tried to remove GrhData `{0}` from the categorization dictionary with the" +
                    " categorization of `{1}`, but the whole category does not exist!" +
                    " This likely means some bad issue in the GrhInfo class...";
                string err = string.Format(errmsg, gd, gd.Categorization);
                log.Fatal(err);
                Debug.Fail(err);
                return;
            }

            if (!titleDic.Remove(gd.Categorization.Title))
            {
                const string errmsg = "Tried to remove GrhData `{0}` from the categorization dictionary with the" +
                    " categorization of `{1}`, but the GrhData did not exist in the title dictionary!" +
                    " This likely means some bad issue in the GrhInfo class...";
                string err = string.Format(errmsg, gd, gd.Categorization);
                log.Fatal(err);
                Debug.Fail(err);
            }
        }

        /// <summary>
        /// Handles when a GrhData is removed from the DArray
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        static void RemoveHandler(object sender, DArrayModifyEventArgs<GrhData> e)
        {
            Debug.Assert(e.Index != GrhIndex.Invalid);

            RemoveFromDictionary(e.Item);

            if (OnRemove != null)
                OnRemove(e.Item);
        }

        /// <summary>
        /// Replaces an existing GrhData with a new <see cref="AnimatedGrhData"/>.
        /// </summary>
        /// <param name="grhIndex">The index of the <see cref="GrhData"/> to convert.</param>
        /// <returns>The created <see cref="AnimatedGrhData"/>, or null if the replacement failed.</returns>
        public static AnimatedGrhData ReplaceExistingWithAnimated(GrhIndex grhIndex)
        {
            var gd = GetData(grhIndex);
            if (gd == null)
                return null;

            if (gd is AnimatedGrhData)
                return (AnimatedGrhData)gd;

            var newGD = new AnimatedGrhData(gd.GrhIndex, gd.Categorization);
            Delete(gd);
            AddGrhData(newGD);

            return newGD;
        }

        /// <summary>
        /// Replaces an existing GrhData with a new <see cref="StationaryGrhData"/>.
        /// </summary>
        /// <param name="grhIndex">The index of the <see cref="GrhData"/> to convert.</param>
        /// <returns>The created <see cref="StationaryGrhData"/>, or null if the replacement failed.</returns>
        public static StationaryGrhData ReplaceExistingWithStationary(GrhIndex grhIndex)
        {
            var gd = GetData(grhIndex);
            if (gd == null)
                return null;

            if (gd is StationaryGrhData)
                return (StationaryGrhData)gd;

            var newGD = new StationaryGrhData(GetContentManager(), gd.GrhIndex, gd.Categorization);
            Delete(gd);
            AddGrhData(newGD);

            return newGD;
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

            // Grab the GrhDatas by their type
            var gds = GrhDatas.Where(x => x != null).OrderBy(x => x.Categorization.ToString(), StringComparer.OrdinalIgnoreCase);
            var stationaryGrhDatas = gds.OfType<StationaryGrhData>().ToArray();
            var animatedGrhDatas = gds.OfType<AnimatedGrhData>().ToArray();
            var autoAnimatedGrhDatas = gds.OfType<AutomaticAnimatedGrhData>().ToArray();

            // Write
            using (IValueWriter writer = new XmlValueWriter(tempPath, _rootNodeName))
            {
                writer.WriteManyNodes(_nonAnimatedGrhDatasNodeName, stationaryGrhDatas, ((w, item) => item.Write(w)));
                writer.WriteManyNodes(_animatedGrhDatasNodeName, animatedGrhDatas, ((w, item) => item.Write(w)));
                writer.WriteManyNodes(_autoAnimatedGrhDatasNodeName, autoAnimatedGrhDatas, ((w, item) => item.Write(w)));
            }

            // Now that the temporary file has been successfully written, replace the existing file with it
            File.Delete(path);
            File.Move(tempPath, path);
        }
    }
}