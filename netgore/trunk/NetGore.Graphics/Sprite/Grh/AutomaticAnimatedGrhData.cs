using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

namespace NetGore.Graphics
{
    public sealed class AutomaticAnimatedGrhData : GrhData
    {
        float _speed;

        /// <summary>
        /// The delimiter used on the directory names for the <see cref="AutomaticAnimatedGrhData"/>.
        /// </summary>
        public const string DirectoryNameDelimiter = "_";

        readonly Vector2 _size;
        readonly StationaryGrhData[] _frames;
        readonly ContentManager _cm;

        AutomaticAnimatedGrhData(ContentManager cm, GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            var framesDir = GetFramesDirectory();
            if (framesDir == null)
                return;

            var animInfo = GrhInfo.GetAutomaticAnimationInfo(framesDir);
            _speed = animInfo.Speed;

            Debug.Assert(animInfo.Title == cat.Title);

            _cm = cm;
            _frames = CreateFrames(framesDir);
            _size = GetMaxSize(_frames);
        }

        string GetFramesDirectory()
        {
            string category = Categorization.Category.ToString();
            int lastSep = category.LastIndexOf(SpriteCategorization.Delimiter);
            string partialCategory;
            if (lastSep <= 0)
                partialCategory = string.Empty;
            else
                partialCategory = category.Substring(0, lastSep);

            var rootDir = ContentPaths.Build.Grhs.Join(partialCategory);

            var potentialDirs = Directory.GetDirectories(rootDir, string.Format("{0}{1}{0}frames{0}*", DirectoryNameDelimiter,
                Categorization.Title));

            if (potentialDirs.Count() > 1)
                throw new GrhDataException(this, "Multiple potential source directories found for the AutomatedGrhData. Make sure you don't have duplicate titles.");
            else if (potentialDirs.Count() == 0)
            {
                GrhInfo.Delete(this);
                return null;
            }

            return potentialDirs.First();
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

        public ContentManager ContentManager { get { return _cm; } }

        StationaryGrhData[] CreateFrames(string directory)
        {
            var allFiles = Directory.GetFiles(directory, "*." + ContentPaths.CompiledContentSuffix, SearchOption.TopDirectoryOnly);
            var files = SortAndFilterFrameFiles(allFiles);

            var frames = new StationaryGrhData[files.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                var contentAssetName = TextureAssetName.FromAbsoluteFilePath(files[i], ContentPaths.Build.Grhs).Value;
                var textureAssetName = new TextureAssetName(contentAssetName);
                frames[i] = new StationaryGrhData(this, textureAssetName);
            }

            return frames;
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
            get
            {
                return _speed;
            }
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
        /// When overridden in the derived class, writes the values unique to this derived type to the
        /// <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
        }
    }
}
