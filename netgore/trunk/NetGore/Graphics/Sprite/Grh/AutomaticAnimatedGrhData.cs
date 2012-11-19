using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using NetGore.Content;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="GrhData"/> that is animated, and the frames are automatically created at run-time from the appropriate
    /// directory based off of the <see cref="AutomaticAnimatedGrhData"/>'s categorization.
    /// </summary>
    public sealed class AutomaticAnimatedGrhData : GrhData
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The delimiter used on the directory names for the <see cref="AutomaticAnimatedGrhData"/>.
        /// </summary>
        public const string DirectoryNameDelimiter = "_";

        /// <summary>
        /// The Regex used to match folders used for <see cref="AutomaticAnimatedGrhData"/>s.
        /// </summary>
        static readonly Regex _aaFolderRegex;

        readonly IContentManager _cm;
        readonly StationaryGrhData[] _frames;
        readonly float _speed;

        ushort _sizeX = 0;
        ushort _sizeY = 0;

        /// <summary>
        /// Initializes the <see cref="AutomaticAnimatedGrhData"/> class.
        /// </summary>
        static AutomaticAnimatedGrhData()
        {
            // Build the folder-matching Regex. The second parameter ({1}) is the set of possible directory separators.
            // The only real confusing bit is: ([{1}][^{1}]*)?$
            // This basically just says that the only non-digits that may be before the end of the string are either nothing,
            // or a single path separator and some characters. This allows us to also match when the path of a frame
            // is passed.
            var regexStr = string.Format(@"[{1}]{0}(?<Title>[^{1}]+?){0}frames{0}(?<Speed>\d+)([{1}][^{1}{0}]*)?$",
                DirectoryNameDelimiter, @"\\/");
            _aaFolderRegex = new Regex(regexStr, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticAnimatedGrhData"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="IContentManager"/> used for creating the frames.</param>
        /// <param name="grhIndex">The <see cref="GrhIndex"/>.</param>
        /// <param name="cat">The <see cref="SpriteCategorization"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cat"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="grhIndex"/> is equal to GrhIndex.Invalid.</exception>
        internal AutomaticAnimatedGrhData(IContentManager cm, GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            _cm = cm;

            try
            {
                var framesDir = GetFramesDirectory();
                var framesDirName = Path.GetFileName(framesDir).Substring(1); // Get dir name only, and skip the _ at the start

                var fileTags = FileTags.Create(framesDirName);
                _speed = 1f / fileTags.AnimationSpeed.Value;

                Debug.Assert(fileTags.Title == cat.Title);

                _frames = CreateFrames(framesDir);
            }
            catch
            {
                _speed = 1f;
                _frames = new StationaryGrhData[0];
            }
        }

        /// <summary>
        /// Gets the <see cref="IContentManager"/> used by the <see cref="AutomaticAnimatedGrhData"/>.
        /// </summary>
        public IContentManager ContentManager
        {
            get { return _cm; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the frames in an animated <see cref="GrhData"/>, or an
        /// IEnumerable containing a reference to its self if stationary.
        /// </summary>
        public override IEnumerable<StationaryGrhData> Frames
        {
            get { return _frames; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the number of frames in this <see cref="GrhData"/>. If this
        /// is not an animated <see cref="GrhData"/>, this value will always return 0.
        /// </summary>
        public override int FramesCount
        {
            get { return _frames.Length; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the size of the <see cref="GrhData"/>'s sprite in pixels.
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                LoadSizeIfNeeded();
                return new Vector2(_sizeX, _sizeY);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the speed multiplier of the <see cref="GrhData"/> animation where each
        /// frame lasts 1f/Speed milliseconds. For non-animated <see cref="GrhData"/>s, this value will always be 0.
        /// </summary>
        public override float Speed
        {
            get { return _speed; }
        }

        /// <summary>
        /// Creates the <see cref="StationaryGrhData"/> frames for this <see cref="AutomaticAnimatedGrhData"/>.
        /// </summary>
        /// <param name="directory">The directory containing the frames.</param>
        /// <returns>An array of the <see cref="StationaryGrhData"/> frames.</returns>
        StationaryGrhData[] CreateFrames(string directory)
        {
            // Find all the valid content files
            var allFiles = Directory.GetFiles(directory, "*" + ContentPaths.ContentFileSuffix, SearchOption.TopDirectoryOnly);

            // Filter out the files with invalid naming conventions, and sort them so we animate in the correct order
            var files = SortAndFilterFrameFiles(allFiles);

            // Build the individual frames
            var frames = new StationaryGrhData[files.Length];
            for (var i = 0; i < frames.Length; i++)
            {
                var contentAssetName = ContentAssetName.FromAbsoluteFilePath(files[i], ContentPaths.Build.Grhs).Value;
                var textureAssetName = new TextureAssetName(contentAssetName);
                var frameGrhData = new StationaryGrhData(this, textureAssetName);
                frames[i] = frameGrhData;
            }

            if (frames.Length == 0)
                throw new Exception("Animation has no frames");

            return frames;
        }

        /// <summary>
        /// When overridden in the derived class, creates a new <see cref="GrhData"/> equal to this <see cref="GrhData"/>
        /// except for the specified parameters.
        /// </summary>
        /// <param name="newCategorization">The <see cref="SpriteCategorization"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <param name="newGrhIndex">The <see cref="GrhData.GrhIndex"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <returns>A deep copy of this <see cref="GrhData"/>.</returns>
        /// <exception cref="NotSupportedException">Cannot make a copy of an <see cref="AutomaticAnimatedGrhData"/>.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutomaticAnimatedGrhData"
            )]
        protected override GrhData DeepCopy(SpriteCategorization newCategorization, GrhIndex newGrhIndex)
        {
            throw new NotSupportedException("Cannot make a copy of an AutomaticAnimatedGrhData.");
        }

        /// <summary>
        /// Attempts to get the <see cref="AutomaticAnimationInfo"/> for the <paramref name="directory"/>.
        /// </summary>
        /// <param name="directory">The absolute directory path to try to get the <see cref="AutomaticAnimationInfo"/>
        /// for.</param>
        /// <returns>The <see cref="AutomaticAnimationInfo"/> for the <paramref name="directory"/>, or null if
        /// the <paramref name="directory"/> is invalid or does not match the pattern for an automatic animation.</returns>
        public static AutomaticAnimationInfo GetAutomaticAnimationInfo(string directory)
        {
            const int defaultSpeed = 400;

            // Get the regex match
            var match = _aaFolderRegex.Match(directory);
            if (!match.Success)
                return null;

            // Grab the different parts, and return the values
            var title = match.Groups["Title"].Value;
            var speed = defaultSpeed;
            if (match.Groups["Speed"].Success)
                speed = Parser.Invariant.ParseInt(match.Groups["Speed"].Value);

            var animationRegexInfo = new AutomaticAnimationInfo(directory, title, speed);
            return animationRegexInfo;
        }

        /// <summary>
        /// When overridden in the derived class, gets the frame in an animated <see cref="GrhData"/> with the
        /// corresponding index, or null if the index is out of range. If stationary, this will always return
        /// a reference to its self, no matter what the index is.
        /// </summary>
        /// <param name="frameIndex">The index of the frame to get.</param>
        /// <returns>
        /// The frame with the given <paramref name="frameIndex"/>, or null if the <paramref name="frameIndex"/>
        /// is invalid, or a reference to its self if this is not an animated <see cref="GrhData"/>.
        /// </returns>
        public override StationaryGrhData GetFrame(int frameIndex)
        {
            if (frameIndex < 0 || frameIndex >= _frames.Length)
                return null;

            return _frames[frameIndex];
        }

        /// <summary>
        /// Gets the directory that contains the frames for this <see cref="AutomaticAnimatedGrhData"/>. The directory
        /// is scanned for on-demand based off of the categorization.
        /// </summary>
        /// <returns>The directory that contains the frames for this <see cref="AutomaticAnimatedGrhData"/>, or null
        /// if the directory could not be found.</returns>
        /// <exception cref="GrhDataException">Multiple potential source directories found for the <see cref="AutomaticAnimatedGrhData"/>.
        /// Make sure you don't have duplicate titles.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutomatedGrhData")]
        string GetFramesDirectory()
        {
            var category = Categorization.Category.ToString();
            var categoryAsFilePath = category.Replace(SpriteCategorization.Delimiter, Path.DirectorySeparatorChar.ToString());
            var rootDir = ContentPaths.Build.Grhs.Join(categoryAsFilePath);

            // Create the filter that will be used to find the directory containing the frames
            var dirNameFilter = "_" + Categorization.Title + "*";

            // Try to find the directory
            string[] potentialDirs;
            try
            {
                potentialDirs = Directory.GetDirectories(rootDir, dirNameFilter);
            }
            catch (DirectoryNotFoundException)
            {
                potentialDirs = null;
            }

            // Ensure we only have one valid directory
            if (potentialDirs == null || potentialDirs.Length == 0)
            {
                throw new GrhDataException(this, string.Format("Could not find the directory for AutomaticAnimatedGrhData `{0}`.", Categorization));
            }

            if (potentialDirs.Length > 1)
            {
                const string errmsg =
                    "Multiple potential source directories found for the AutomaticAnimatedGrhData. Make sure you don't have duplicate titles.";
                throw new GrhDataException(this, errmsg);
            }

            return potentialDirs.First();
        }

        /// <summary>
        /// Checks if the given <paramref name="directory"/> is for an <see cref="AutomaticAnimatedGrhData"/>.
        /// </summary>
        /// <param name="directory">The absolute directory path.</param>
        /// <returns>If the given <paramref name="directory"/> is for an <see cref="AutomaticAnimatedGrhData"/>.</returns>
        public static bool IsAutomaticAnimatedGrhDataDirectory(string directory)
        {
            return _aaFolderRegex.IsMatch(directory);
        }

        void LoadSizeIfNeeded()
        {
            if (_sizeX != 0)
                return;

            var maxSize = GetMaxSize(_frames);

            _sizeX = (ushort)maxSize.X;
            if (_sizeX < 1)
                _sizeX = 1;

            _sizeY = (ushort)maxSize.Y;
            if (_sizeY < 1)
                _sizeY = 1;
        }

        /// <summary>
        /// Reads a <see cref="GrhData"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> to read from.</param>
        /// <param name="cm">The <see cref="IContentManager"/> that will contain the frames.</param>
        /// <returns>
        /// The <see cref="GrhData"/> read from the <see cref="IValueReader"/>.
        /// </returns>
        public static AutomaticAnimatedGrhData Read(IValueReader r, IContentManager cm)
        {
            GrhIndex grhIndex;
            SpriteCategorization categorization;
            ReadHeader(r, out grhIndex, out categorization);

            return new AutomaticAnimatedGrhData(cm, grhIndex, categorization);
        }

        /// <summary>
        /// Filters out the invalid frame files and sorts them by name.
        /// </summary>
        /// <param name="files">The paths to the files that are to be used as the frames.</param>
        /// <returns>The files that have a valid name, ordered numerically by file name.</returns>
        static string[] SortAndFilterFrameFiles(IEnumerable<string> files)
        {
            var validFiles = new List<KeyValuePair<int, string>>(files.Count());

            // Parse and filter
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                // If we can't parse it as an int, then we won't include it
                int o;
                if (!Parser.Invariant.TryParse(fileName, out o))
                    continue;

                // Add the parsed value and the original name to the list of valid files to prevent having to re-parse
                // the name as an int again
                validFiles.Add(new KeyValuePair<int, string>(o, file));
            }

            // Sort by the parsed int and return the sorted file names
            return validFiles.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
        }

        /// <summary>
        /// When overridden in the derived class, writes the values unique to this derived type to the
        /// <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
        }
    }
}