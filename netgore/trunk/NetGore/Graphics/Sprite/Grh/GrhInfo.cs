using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.Content;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Holds the <see cref="GrhData"/>s and related methods.
    /// </summary>
    public static class GrhInfo
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

        static Func<GrhData, ContentLevel> _contentLevelDecider = DefaultContentLevelDecider;

        /// <summary>
        /// List of all the <see cref="GrhData"/> where the array index is the <see cref="GrhIndex"/>.
        /// </summary>
        static DArray<GrhData> _grhDatas;

        static bool _isLoaded = false;

        /// <summary>
        /// If the <see cref="GrhData"/>s are currently being loaded
        /// </summary>
        static bool _isLoading;

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
        /// Notifies listeners when a <see cref="GrhData"/> has been added. This includes
        /// <see cref="GrhData"/>s added from loading.
        /// </summary>
        public static event TypedEventHandler<GrhData> Added;

        /// <summary>
        /// Notifies listeners when a new <see cref="GrhData"/> has been added. This does not include
        /// <see cref="GrhData"/>s added from loading.
        /// </summary>
        public static event TypedEventHandler<GrhData> AddedNew;

        /// <summary>
        /// Notifies listeners when a <see cref="GrhData"/> has been removed.
        /// </summary>
        public static event TypedEventHandler<GrhData> Removed;

        /// <summary>
        /// Gets or sets a func used to determine the <see cref="ContentLevel"/>
        /// for a <see cref="GrhData"/>. This value should be set before calling <see cref="GrhInfo.Load"/>
        /// for best results.
        /// </summary>
        public static Func<GrhData, ContentLevel> ContentLevelDecider
        {
            get { return _contentLevelDecider; }
            set { _contentLevelDecider = value ?? DefaultContentLevelDecider; }
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// Default value is null.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

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
        /// Gets if the <see cref="GrhData"/>s have already been loaded.
        /// </summary>
        public static bool IsLoaded
        {
            get { return _isLoaded; }
        }

        /// <summary>
        /// Adds a <see cref="GrhData"/> to the list of <see cref="GrhData"/>s at the index assigned to it.
        /// </summary>
        /// <param name="gd"><see cref="GrhData"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="gd" /> is <c>null</c>.</exception>
        internal static void AddGrhData(GrhData gd)
        {
            if (gd == null)
                throw new ArgumentNullException("gd");

            var index = gd.GrhIndex;

            AssertGrhIndexIsFree(index, gd);

            _grhDatas[(int)index] = gd;

            // Make sure the GrhData is only in the list once
            Debug.Assert(GrhDatas.Count(x => x == gd) == 1,
                "The GrhData should be in the list only once. Somehow, its in there either more times, or not at all.");
        }

        /// <summary>
        /// Handles when a <see cref="GrhData"/> is added to the <see cref="GrhData"/>s DArray.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.Collections.DArrayEventArgs{T}"/> instance containing the event data.</param>
        static void AddHandler(DArray<GrhData> sender, DArrayEventArgs<GrhData> e)
        {
            Debug.Assert(e.Index != GrhIndex.Invalid);

            var gd = e.Item;
            if (gd == null)
            {
                Debug.Fail("gd is null.");
                return;
            }

            // Set the categorization event so we can keep up with any changes to the categorization.
            gd.CategorizationChanged -= ChangeCategorizationHandler;
            gd.CategorizationChanged += ChangeCategorizationHandler;

            AddToDictionary(gd);

            if (Added != null)
                Added.Raise(gd, EventArgs.Empty);

            if (!_isLoading)
            {
                if (AddedNew != null)
                    AddedNew.Raise(gd, EventArgs.Empty);
            }
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
        /// Asserts that a given <see cref="GrhIndex"/> is not already occupied.
        /// </summary>
        /// <param name="index">The <see cref="GrhIndex"/> to check.</param>
        /// <param name="ignoreWhen">When the <see cref="GrhData"/> at the <paramref name="index"/> is equal to this value,
        /// and this value is not null, no errors will be raised.</param>
        [Conditional("DEBUG")]
        static void AssertGrhIndexIsFree(GrhIndex index, GrhData ignoreWhen)
        {
            // Make sure we can even get the index
            if (!_grhDatas.CanGet((int)index))
                return;

            // Get the current GrhData
            var currentGD = GetData(index);

            // Check if occupied
            if (currentGD != null && (ignoreWhen == null || currentGD != ignoreWhen))
                Debug.Fail("Existing GrhData is going to be overwritten. This is likely not what was intended.");
        }

        /// <summary>
        /// Handles when the category of a <see cref="GrhData"/> in the DArray changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.EventArgs{SpriteCategorization}"/> instance containing the event data.</param>
        static void ChangeCategorizationHandler(GrhData sender, EventArgs<SpriteCategorization> e)
        {
            var gd = sender;
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
        /// <param name="index">If specified, the GrhIndex to use for the new GrhData.</param>
        /// <returns>The new <see cref="AutomaticAnimatedGrhData"/>, or null if none created.</returns>
        public static AutomaticAnimatedGrhData CreateAutomaticAnimatedGrhData(IContentManager contentManager, SpriteCategorization categorization, GrhIndex? index = null)
        {
            // Check if the GrhData already exists
            if (GetData(categorization) != null)
                return null;

            if (!index.HasValue)
                index = NextFreeIndex();

            var gd = new AutomaticAnimatedGrhData(contentManager, index.Value, categorization);
            AddGrhData(gd);
            return gd;
        }

        public static AnimatedGrhData CreateGrhData(IEnumerable<GrhIndex> frames, float speed, SpriteCategorization categorization)
        {
            var grhIndex = NextFreeIndex();
            var gd = new AnimatedGrhData(grhIndex, categorization);
            gd.SetSpeed(speed);
            gd.SetFrames(frames);
            AddGrhData(gd);
            return gd;
        }

        public static StationaryGrhData CreateGrhData(IContentManager contentManager, SpriteCategorization categorization,
                                                      string texture, Vector2 pos, Vector2 size)
        {
            return CreateGrhData(NextFreeIndex(), contentManager, categorization, texture, pos, size);
        }

        public static StationaryGrhData CreateGrhData(IContentManager contentManager, SpriteCategorization categorization,
                                                      string texture, GrhIndex? index = null)
        {
            return CreateGrhData(index ?? NextFreeIndex(), contentManager, categorization, texture, null, null);
        }

        public static StationaryGrhData CreateGrhData(IContentManager contentManager, SpriteCategory category)
        {
            var index = NextFreeIndex();
            var title = GetUniqueTitle(category, "tmp" + index);
            var categorization = new SpriteCategorization(category, title);
            return CreateGrhData(index, contentManager, categorization, string.Empty, Vector2.Zero, Vector2.Zero);
        }

        static StationaryGrhData CreateGrhData(GrhIndex grhIndex, IContentManager contentManager,
                                               SpriteCategorization categorization, string texture, Vector2? pos, Vector2? size)
        {
            Debug.Assert(!grhIndex.IsInvalid);

            Rectangle? source;

            if (pos == null || size == null)
                source = null;
            else
                source = new Rectangle(pos.Value.X, pos.Value.Y, size.Value.X, size.Value.Y);

            var gd = new StationaryGrhData(contentManager, grhIndex, categorization, texture, source);

            AddGrhData(gd);
            return gd;
        }

        /// <summary>
        /// Decides the <see cref="ContentLevel"/> to use for <see cref="GrhData"/>s.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> to get the <see cref="ContentLevel"/> for.</param>
        /// <returns>The <see cref="ContentLevel"/> for the <paramref name="grhData"/>.</returns>
        static ContentLevel DefaultContentLevelDecider(GrhData grhData)
        {
            return ContentLevel.Map;
        }

        /// <summary>
        /// Deletes a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData"><see cref="GrhData"/> to delete.</param>
        /// <exception cref="ArgumentNullException"><paramref name="grhData" /> is <c>null</c>.</exception>
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
            var i = (int)grhIndex;
            if (_grhDatas[i] != grhData)
            {
                const string errmsg =
                    "Attempted to delete GrhData `{0}`, but GrhIndex `{1}` is already in use by" +
                    " a different GrhData `{2}`. Most likely, the GrhData we tried to delete is already deleted, and" +
                    " the GrhIndex has been recycled to a new GrhData. Stop trying to delete dead stuff, will ya!?";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, grhData, grhIndex, _grhDatas[i]);
                Debug.Fail(string.Format(errmsg, grhData, grhIndex, _grhDatas[i]));
                return;
            }

            // Remove the GrhData from the collection
            _grhDatas.RemoveAt((int)grhIndex);

            // If a stationary GrhData and auto-size is set, delete the texture, too
            try
            {
                var sgd = grhData as StationaryGrhData;
                if (sgd != null && sgd.AutomaticSize)
                {
                    // Make sure no other GrhData is using the texture
                    var origTexture = sgd.GetOriginalTexture();
                    if (GrhDatas.OfType<StationaryGrhData>().All(x => origTexture != x.GetOriginalTexture()))
                    {
                        // Dispose of the texture then recycle the file
                        origTexture.Dispose();
                        try
                        {
                            sgd.TextureName.RecycleFile();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to recycle texture file for GrhData `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, grhData, ex);
                Debug.Fail(string.Format(errmsg, grhData, ex));
            }
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
        /// Gets a <see cref="IContentManager"/>.
        /// </summary>
        /// <returns>A <see cref="IContentManager"/>.</returns>
        static IContentManager GetContentManager()
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
            return contentPath.Data.Join("grhdata" + EngineSettings.DataFileSuffix);
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
            var copyNum = 1;
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
            var copyNum = 1;
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
        /// <param name="cm">The <see cref="IContentManager"/> to use for loaded <see cref="GrhData"/>s.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cm" /> is <c>null</c>.</exception>
        /// <exception cref="FileNotFoundException">GrhData data file not found.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GrhData")]
        public static void Load(ContentPaths contentPath, IContentManager cm)
        {
            if (IsLoaded)
                return;

            _isLoaded = true;

            var path = GetGrhDataFilePath(contentPath);

            if (cm == null)
                throw new ArgumentNullException("cm");

            if (!File.Exists(path))
                throw new FileNotFoundException("GrhData data file not found.", path);

            _isLoading = true;

            try
            {
                // Create the GrhData DArray
                if (_grhDatas == null)
                    _grhDatas = new DArray<GrhData>(256);
                else
                    _grhDatas.Clear();

                _catDic.Clear();

                _grhDatas.ItemAdded -= AddHandler;
                _grhDatas.ItemAdded += AddHandler;

                _grhDatas.ItemRemoved -= RemoveHandler;
                _grhDatas.ItemRemoved += RemoveHandler;

                // Read and add the GrhDatas in order by their type
                var reader = GenericValueReader.CreateFromFile(path, _rootNodeName);

                LoadGrhDatas(reader, _nonAnimatedGrhDatasNodeName, x => StationaryGrhData.Read(x, cm));
                LoadGrhDatas(reader, _animatedGrhDatasNodeName, AnimatedGrhData.Read);
                LoadGrhDatas(reader, _autoAnimatedGrhDatasNodeName, x => AutomaticAnimatedGrhData.Read(x, cm));

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
        /// Handles loading a group of <see cref="GrhData"/>s from an <see cref="IValueReader"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="GrhData"/> being loaded.</typeparam>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <param name="nodeName">The name of the node to read.</param>
        /// <param name="loader">The <see cref="ReadManyNodesHandler{T}"/> used to load each <see cref="GrhData"/>
        /// of type <typeparamref name="T"/>.</param>
        static void LoadGrhDatas<T>(IValueReader reader, string nodeName, ReadManyNodesHandler<T> loader) where T : GrhData
        {
            const string errmsgFailedLoads =
                "Failed to load the following indices from the GrhData file (\\DevContent\\Data\\grhdata.dat) for GrhData type `{2}`:{0}{1}{0}" +
                "Note that these indices are NOT the GrhIndex, but the index of the Xml node." +
                " Often times, this error means that you manually deleted one or more GrhDatas from this file," +
                "in which case you can ignore it.";

            // Create our "read key failed" handler for when a node for a GrhData is invalid
            var failedLoads = new List<KeyValuePair<int, Exception>>();
            Action<int, Exception> grhLoadFailHandler = (i, ex) => failedLoads.Add(new KeyValuePair<int, Exception>(i, ex));

            // Load the GrhDatas
            var loadedGrhDatas = reader.ReadManyNodes(nodeName, loader, grhLoadFailHandler);

            // Add the GrhDatas to the global collection
            foreach (var grhData in loadedGrhDatas)
            {
                if (grhData == null)
                    continue;

                var index = (int)grhData.GrhIndex;
                Debug.Assert(!_grhDatas.CanGet(index) || _grhDatas[index] == null, "Index already occupied!");

                if (!grhData.GrhIndex.IsInvalid)
                    _grhDatas[index] = grhData;
                else
                {
                    const string errmsg = "Tried to add GrhData `{0}` which has an invalid GrhIndex.";
                    var err = string.Format(errmsg, grhData);
                    log.Fatal(err);
                    Debug.Fail(err);
                }
            }

            // Notify when any of the nodes failed to load
            if (failedLoads.Count > 0)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsgFailedLoads, Environment.NewLine, failedLoads.Select(x => x.Key).Implode(), typeof(T).Name);
                Debug.Fail(string.Format(errmsgFailedLoads, Environment.NewLine, failedLoads.Select(x => x.Key).Implode(), typeof(T).Name));
                failedLoads.Clear();
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
            var i = GrhIndex.MinValue;

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
                const string errmsg =
                    "Tried to remove GrhData `{0}` from the categorization dictionary with the" +
                    " categorization of `{1}`, but the whole category does not exist!" +
                    " This likely means some bad issue in the GrhInfo class...";
                var err = string.Format(errmsg, gd, gd.Categorization);
                log.Fatal(err);
                Debug.Fail(err);
                return;
            }

            if (!titleDic.Remove(gd.Categorization.Title))
            {
                const string errmsg =
                    "Tried to remove GrhData `{0}` from the categorization dictionary with the" +
                    " categorization of `{1}`, but the GrhData did not exist in the title dictionary!" +
                    " This likely means some bad issue in the GrhInfo class...";
                var err = string.Format(errmsg, gd, gd.Categorization);
                log.Fatal(err);
                Debug.Fail(err);
            }
        }

        /// <summary>
        /// Handles when a GrhData is removed from the DArray
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.Collections.DArrayEventArgs{T}"/> instance containing the event data.</param>
        static void RemoveHandler(object sender, DArrayEventArgs<GrhData> e)
        {
            Debug.Assert(e.Index != GrhIndex.Invalid);

            RemoveFromDictionary(e.Item);

            if (Removed != null)
                Removed.Raise(e.Item, EventArgs.Empty);
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

            var newGD = new StationaryGrhData(GetContentManager(), gd.GrhIndex, gd.Categorization, null, null);

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
            // Grab the GrhDatas by their type
            var gds = GrhDatas.Where(x => x != null).OrderBy(x => x.Categorization.ToString(), StringComparer.OrdinalIgnoreCase);
            var stationaryGrhDatas = gds.OfType<StationaryGrhData>().ToImmutable();
            var animatedGrhDatas = gds.OfType<AnimatedGrhData>().ToImmutable();
            var autoAnimatedGrhDatas = gds.OfType<AutomaticAnimatedGrhData>().ToImmutable();

            // Write
            var path = GetGrhDataFilePath(contentPath);
            using (IValueWriter writer = GenericValueWriter.Create(path, _rootNodeName, EncodingFormat))
            {
                writer.WriteManyNodes(_nonAnimatedGrhDatasNodeName, stationaryGrhDatas, ((w, item) => item.Write(w)));
                writer.WriteManyNodes(_animatedGrhDatasNodeName, animatedGrhDatas, ((w, item) => item.Write(w)));
                writer.WriteManyNodes(_autoAnimatedGrhDatasNodeName, autoAnimatedGrhDatas, ((w, item) => item.Write(w)));
            }
        }
    }
}