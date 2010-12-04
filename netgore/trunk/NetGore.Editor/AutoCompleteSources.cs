using System.IO;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.Editor
{
    /// <summary>
    /// Contains different AutoComplete sources.
    /// </summary>
    public static class AutoCompleteSources
    {
        /// <summary>
        /// The default <see cref="AutoCompleteMode"/> to use for controls.
        /// </summary>
        public const AutoCompleteMode DefaultAutoCompleteMode = AutoCompleteMode.Suggest;

        /// <summary>
        /// The maximum rate at which the <see cref="_categories"/> may be updated. Any attempt to update
        /// more frequenlty than this rate will be ignored. This is mostly to prevent from massive stalls
        /// when adding/removing tons of <see cref="GrhData"/>s at once.
        /// </summary>
        const int _categoriesMaxUpdateRate = 500;

        static readonly AutoCompleteStringCollection _categories;
        static readonly AutoCompleteStringCollection _textures;

        /// <summary>
        /// The tick count at which the <see cref="_categories"/> was last updated.
        /// </summary>
        static TickCount _categoriesLastUpdateTime = TickCount.MinValue;

        /// <summary>
        /// Initializes the <see cref="AutoCompleteSources"/> class.
        /// </summary>
        static AutoCompleteSources()
        {
            // Textures source
            _textures = new AutoCompleteStringCollection();
            PrepareTextures();

            // Categories source
            _categories = new AutoCompleteStringCollection();
            PrepareCategories();
        }

        /// <summary>
        /// Gets the <see cref="AutoCompleteStringCollection"/> for the <see cref="GrhData"/> categories.
        /// </summary>
        public static AutoCompleteStringCollection Categories
        {
            get
            {
                UpdateCategories();
                return _categories;
            }
        }

        /// <summary>
        /// Gets the <see cref="AutoCompleteStringCollection"/> for the textures.
        /// </summary>
        public static AutoCompleteStringCollection Textures
        {
            get { return _textures; }
        }

        /// <summary>
        /// Forces the <see cref="Categories"/> collection to be updated.
        /// </summary>
        public static void ForceUpdateCategories()
        {
            UpdateCategories();
        }

        /// <summary>
        /// Sets up the <see cref="_categories"/> collection.
        /// </summary>
        static void PrepareCategories()
        {
            // Force update categorization when any GrhInfos are added or removed
            GrhInfo.Added += (sender, e) => UpdateCategories();
            GrhInfo.Removed += delegate { UpdateCategories(); };

            // Auto-update every 30 seconds so any unexpected synchronization problems self-resolve eventually
            // without any noticeable hits to performance
            var t = new Timer { Interval = 30000 };
            t.Tick += delegate { UpdateCategories(); };

            // Perform initial population
            UpdateCategories();
        }

        /// <summary>
        /// Sets up the <see cref="_textures"/> collection.
        /// </summary>
        static void PrepareTextures()
        {
            // Get all the content files
            var files = Directory.GetFiles(ContentPaths.Build.Grhs, "*" + ContentPaths.ContentFileSuffix,
                SearchOption.AllDirectories);

            // Cache the amount we need to remove from the start and end of each string
            var start = ContentPaths.Build.Grhs.ToString().Length + 1;
            var trimEnd = ContentPaths.ContentFileSuffix.Length;

            // Remove from the start so we only have the relative path, and remove the suffix from the end
            files =
                files.Select(x => x.Substring(start, x.Length - start - trimEnd).Replace(Path.DirectorySeparatorChar, '/')).
                    ToArray();

            // Add
            _textures.AddRange(files);
        }

        /// <summary>
        /// Updates the <see cref="_categories"/> collection.
        /// </summary>
        static void UpdateCategories()
        {
            if (_categoriesLastUpdateTime + _categoriesMaxUpdateRate > TickCount.Now)
                return;

            _categoriesLastUpdateTime = TickCount.Now;

            // Grab the array first before clearing just so there is less time when the collection is empty
            var items = GrhInfo.GetCategories().Select(x => x.ToString()).ToArray();

            _categories.Clear();
            _categories.AddRange(items);
        }
    }
}