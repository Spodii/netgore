using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

namespace NetGore.Graphics
{
    public sealed class AutomaticAnimatedGrhData : GrhData
    {
        /// <summary>
        /// The delimiter used on the directory names for the <see cref="AutomaticAnimatedGrhData"/>.
        /// </summary>
        public const string DirectoryNameDelimiter = "_";

        static readonly Regex _automaticAnimationRegex = new Regex(".+_(?<Title>.+)_frames_(?<Speed>\\d+)",
                                                                   RegexOptions.Compiled | RegexOptions.IgnoreCase |
                                                                   RegexOptions.ExplicitCapture);

        readonly ContentManager _cm;
        readonly StationaryGrhData[] _frames;
        readonly Vector2 _size;
        readonly float _speed;

        internal AutomaticAnimatedGrhData(ContentManager cm, GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            var framesDir = GetFramesDirectory();
            if (framesDir == null)
                return;

            var animInfo = GetAutomaticAnimationInfo(framesDir);
            _speed = 1f / animInfo.Speed;

            Debug.Assert(animInfo.Title == cat.Title);

            _cm = cm;
            _frames = CreateFrames(framesDir);
            _size = GetMaxSize(_frames);
        }

        public ContentManager ContentManager
        {
            get { return _cm; }
        }

        /// <summary>
        /// Gets the frames in the <see cref="AutomaticAnimatedGrhData"/>.
        /// </summary>
        public IEnumerable<StationaryGrhData> Frames
        {
            get { return _frames; }
        }

        /// <summary>
        /// Gets the number of frames in this <see cref="AutomaticAnimatedGrhData"/>.
        /// </summary>
        public int FramesCount
        {
            get { return _frames.Length; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the size of the <see cref="GrhData"/>'s sprite in pixels.
        /// </summary>
        public override Vector2 Size
        {
            get { return _size; }
        }

        /// <summary>
        /// Gets or sets the speed multiplier of the Grh animation where each frame lasts 1f/Speed milliseconds.
        /// </summary>
        public float Speed
        {
            get { return _speed; }
        }

        StationaryGrhData[] CreateFrames(string directory)
        {
            var allFiles = Directory.GetFiles(directory, "*." + ContentPaths.CompiledContentSuffix, SearchOption.TopDirectoryOnly);
            var files = SortAndFilterFrameFiles(allFiles);

            var frames = new StationaryGrhData[files.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                var contentAssetName = TextureAssetName.FromAbsoluteFilePath(files[i], ContentPaths.Build.Grhs).Value;
                var textureAssetName = new TextureAssetName(contentAssetName);
                var frameGrhData = new StationaryGrhData(this, textureAssetName);
                frames[i] = frameGrhData;
            }

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
            Match match = _automaticAnimationRegex.Match(directory);
            if (!match.Success)
                return null;

            // Grab the different parts, and return the values
            string title = match.Groups["Title"].Value;
            int speed = defaultSpeed;
            if (match.Groups["Speed"].Success)
                speed = Parser.Invariant.ParseInt(match.Groups["Speed"].Value);

            var animationRegexInfo = new AutomaticAnimationInfo(directory, title, speed);
            return animationRegexInfo;
        }

        /// <summary>
        /// Gets the <see cref="StationaryGrhData"/> for the given frame.
        /// </summary>
        /// <param name="frameIndex">The index of the frame.</param>
        /// <returns>The <see cref="StationaryGrhData"/> for the given frame, or null if invalid.</returns>
        public StationaryGrhData GetFrame(int frameIndex)
        {
            if (frameIndex < 0 || frameIndex >= _frames.Length)
                return null;

            return _frames[frameIndex];
        }

        string GetFramesDirectory()
        {
            string category = Categorization.Category.ToString();
            string categoryAsFilePath = category.Replace(SpriteCategorization.Delimiter, Path.DirectorySeparatorChar.ToString());
            var rootDir = ContentPaths.Build.Grhs.Join(categoryAsFilePath);

            var potentialDirs = Directory.GetDirectories(rootDir,
                                                         string.Format("{0}{1}{0}frames{0}*", DirectoryNameDelimiter,
                                                                       Categorization.Title));

            if (potentialDirs.Count() > 1)
            {
                throw new GrhDataException(this,
                                           "Multiple potential source directories found for the AutomatedGrhData. Make sure you don't have duplicate titles.");
            }
            else if (potentialDirs.Count() == 0)
            {
                GrhInfo.Delete(this);
                return null;
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
            return _automaticAnimationRegex.IsMatch(directory);
        }

        /// <summary>
        /// Reads a <see cref="GrhData"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> to read from.</param>
        /// <param name="cm">The <see cref="ContentManager"/> that will contain the frames.</param>
        /// <returns>
        /// The <see cref="GrhData"/> read from the <see cref="IValueReader"/>.
        /// </returns>
        public static AutomaticAnimatedGrhData Read(IValueReader r, ContentManager cm)
        {
            GrhIndex grhIndex;
            SpriteCategorization categorization;
            ReadHeader(r, out grhIndex, out categorization);

            return new AutomaticAnimatedGrhData(cm, grhIndex, categorization);
        }

        static string[] SortAndFilterFrameFiles(IEnumerable<string> files)
        {
            List<KeyValuePair<int, string>> validFiles = new List<KeyValuePair<int, string>>(files.Count());

            // Parse and filter
            foreach (var file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                int o;
                if (!int.TryParse(fileName, out o))
                    continue;

                validFiles.Add(new KeyValuePair<int, string>(o, file));
            }

            // Sort and return
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