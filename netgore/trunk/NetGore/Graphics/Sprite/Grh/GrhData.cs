using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Base class for a <see cref="GrhData"/>, which is what describes how a <see cref="Grh"/> functions.
    /// </summary>
    public abstract class GrhData
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string _categorizationValueKey = "Categorization";
        const string _indexValueKey = "Index";

        readonly GrhIndex _grhIndex;
        SpriteCategorization _categorization;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhData"/> class.
        /// </summary>
        /// <param name="cat">The <see cref="SpriteCategorization"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cat"/> is null.</exception>
        protected GrhData(SpriteCategorization cat)
        {
            // This is the only way we allow GrhDatas with an invalid GrhIndex. It should only ever be called for
            // GrhDatas that will NOT persist, such as the AutomaticAnimatedGrhData's frames.
            _categorization = cat;
            _grhIndex = GrhIndex.Invalid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhData"/> class.
        /// </summary>
        /// <param name="grhIndex">The <see cref="GrhIndex"/>.</param>
        /// <param name="cat">The <see cref="SpriteCategorization"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cat"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="grhIndex"/> is equal to GrhIndex.Invalid.</exception>
        protected GrhData(GrhIndex grhIndex, SpriteCategorization cat)
        {
            if (cat == null)
                throw new ArgumentNullException("cat");

            if (grhIndex.IsInvalid)
            {
                const string errmsg =
                    "Failed to create GrhData with category `{0}`." +
                    " No GrhData may be created with a GrhIndex equal to GrhIndex.Invalid";
                var err = string.Format(errmsg, cat);
                log.Error(err);
                throw new ArgumentOutOfRangeException("grhIndex", err);
            }

            _categorization = cat;
            _grhIndex = grhIndex;
        }

        /// <summary>
        /// Notifies listeners when the <see cref="GrhData"/>'s categorization has changed.
        /// </summary>
        public event TypedEventHandler<GrhData, EventArgs<SpriteCategorization>> CategorizationChanged;

        /// <summary>
        /// Gets the <see cref="SpriteCategorization"/> for this <see cref="GrhData"/>.
        /// </summary>
        public SpriteCategorization Categorization
        {
            get { return _categorization; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the frames in an animated <see cref="GrhData"/>, or an
        /// IEnumerable containing a reference to its self if stationary.
        /// </summary>
        public abstract IEnumerable<StationaryGrhData> Frames { get; }

        /// <summary>
        /// When overridden in the derived class, gets the number of frames in this <see cref="GrhData"/>. If this
        /// is not an animated <see cref="GrhData"/>, this value will always return 0.
        /// </summary>
        public abstract int FramesCount { get; }

        /// <summary>
        /// Gets the index of the <see cref="GrhData"/>.
        /// </summary>
        public GrhIndex GrhIndex
        {
            get { return _grhIndex; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the size of the <see cref="GrhData"/>'s sprite in pixels.
        /// </summary>
        public abstract Vector2 Size { get; }

        /// <summary>
        /// When overridden in the derived class, gets the speed multiplier of the <see cref="GrhData"/> animation where each
        /// frame lasts 1f/Speed milliseconds. For non-animated <see cref="GrhData"/>s, this value will always be 0.
        /// </summary>
        public abstract float Speed { get; }

        /// <summary>
        /// When overridden in the derived class, creates a new <see cref="GrhData"/> equal to this <see cref="GrhData"/>
        /// except for the specified parameters.
        /// </summary>
        /// <param name="newCategorization">The <see cref="SpriteCategorization"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <param name="newGrhIndex">The <see cref="GrhIndex"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <returns>A deep copy of this <see cref="GrhData"/>.</returns>
        protected abstract GrhData DeepCopy(SpriteCategorization newCategorization, GrhIndex newGrhIndex);

        /// <summary>
        /// Creates a deep copy of the <see cref="GrhData"/>.
        /// </summary>
        /// <param name="newCategorization">Categorization for the duplicated GrhData. Must be unique.</param>
        /// <returns>Deep copy of the <see cref="GrhData"/> with the new categorization and its own
        /// unique <see cref="GrhIndex"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="newCategorization"/> is already in use.</exception>
        public GrhData Duplicate(SpriteCategorization newCategorization)
        {
            if (GrhInfo.GetData(newCategorization) != null)
                throw new ArgumentException("Category already in use.", "newCategorization");

            var index = GrhInfo.NextFreeIndex();
            Debug.Assert(GrhInfo.GetData(index) == null,
                "Slot to use is already in use! How the hell did this happen!? GrhInfo.NextFreeIndex() must be broken.");

            var dc = DeepCopy(newCategorization, index);

            GrhInfo.AddGrhData(dc);

            return dc;
        }

        /// <summary>
        /// When overridden in the derived class, gets the frame in an animated <see cref="GrhData"/> with the
        /// corresponding index, or null if the index is out of range. If stationary, this will always return
        /// a reference to its self, no matter what the index is.
        /// </summary>
        /// <param name="frameIndex">The index of the frame to get.</param>
        /// <returns>The frame with the given <paramref name="frameIndex"/>, or null if the <paramref name="frameIndex"/>
        /// is invalid, or a reference to its self if this is not an animated <see cref="GrhData"/>.</returns>
        public abstract StationaryGrhData GetFrame(int frameIndex);

        /// <summary>
        /// Gets the largest size of all the <see cref="GrhData"/>s.
        /// </summary>
        /// <param name="grhDatas">The <see cref="GrhData"/>s.</param>
        /// <returns>The largest size of all the <see cref="GrhData"/>s.</returns>
        protected static Vector2 GetMaxSize(IEnumerable<GrhData> grhDatas)
        {
            if (grhDatas == null || grhDatas.IsEmpty())
                return Vector2.Zero;

            var ret = Vector2.Zero;
            foreach (var f in grhDatas)
            {
                if (f.Size.X > ret.X)
                    ret.X = f.Size.X;
                if (f.Size.Y > ret.Y)
                    ret.Y = f.Size.Y;
            }

            return ret;
        }

        protected internal static void ReadHeader(IValueReader r, out GrhIndex grhIndex, out SpriteCategorization cat)
        {
            grhIndex = r.ReadGrhIndex(_indexValueKey);
            cat = r.ReadSpriteCategorization(_categorizationValueKey);
        }

        /// <summary>
        /// Sets the categorization for the <see cref="GrhData"/>.
        /// </summary>
        /// <param name="categorization">The new categorization.</param>
        /// <exception cref="ArgumentNullException"><paramref name="categorization" /> is <c>null</c>.</exception>
        public void SetCategorization(SpriteCategorization categorization)
        {
            if (categorization == null)
                throw new ArgumentNullException("categorization");

            // Check that either of the values are different
            if (_categorization == categorization)
                return;

            var oldCategorization = _categorization;
            _categorization = categorization;

            if (CategorizationChanged != null)
                CategorizationChanged.Raise(this, EventArgsHelper.Create(oldCategorization));
        }

        /// <summary>
        /// Returns a System.String that represents the <see cref="GrhData"/>.
        /// </summary>
        /// <returns>A System.String that represents the <see cref="GrhData"/>.</returns>
        public override string ToString()
        {
            return string.Format("[{0}] {1}", GrhIndex, Categorization);
        }

        /// <summary>
        /// Writes the <see cref="GrhData"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="w"><see cref="IValueWriter"/> to write to.</param>
        /// <exception cref="GrhDataException">The <see cref="GrhIndex"/> invalid.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="w" /> is <c>null</c>.</exception>
        public void Write(IValueWriter w)
        {
            // Check for valid data
            if (GrhIndex.IsInvalid)
                throw new GrhDataException("The GrhIndex invalid.");
            if (w == null)
                throw new ArgumentNullException("w");

            Debug.Assert(Categorization != null);

            // Write the index and category
            w.Write(_indexValueKey, GrhIndex);
            w.Write(_categorizationValueKey, Categorization);

            // Write the custom values
            WriteCustomValues(w);
        }

        /// <summary>
        /// When overridden in the derived class, writes the values unique to this derived type to the
        /// <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        protected abstract void WriteCustomValues(IValueWriter writer);

        public enum BoundWallType : byte
        {
            NoWall = 0,
            Solid = 1,
            Platform = 2,
        }

        /// <summary>
        /// The tags for a GrhData file. Defines certain attributes, like if the Grh should be covered by a wall, animation speed,
        /// default layer to be placed on, etc.
        /// </summary>
        public class FileTags
        {
            static readonly Regex _tagRegex = new Regex("\\[(?<tag>\\w)(?<value>[^\\]]+)\\]");
            static readonly Regex _tagWallDefinitionRegex = new Regex("(?<wallType>\\d+)x(?<x>.+)y(?<y>.+)w(?<w>.+)h(?<h>.+)");

            /// <summary>
            /// Gets the categorization title of the sprite.
            /// </summary>
            public string Title { get; private set; }

            /// <summary>
            /// Gets the animation speed, if specified.
            /// </summary>
            public ushort? AnimationSpeed { get; private set; }

            /// <summary>
            /// Gets the list of bound walls. Can be null. If non-null, the rectangle defines the position and size of the wall.
            /// </summary>
            public List<Tuple<BoundWallType, Rectangle?>> Walls { get; private set; }

            /// <summary>
            /// Gets the default layer to use, if specified.
            /// </summary>
            public MapRenderLayer? Layer { get; private set; }

            /// <summary>
            /// Creates a FileTags from a file name.
            /// </summary>
            public static FileTags Create(string fileNameWithoutExtension)
            {
                FileTags ret = new FileTags();

                // Go through each tag match, and apply it
                foreach (Match match in _tagRegex.Matches(fileNameWithoutExtension))
                {
                    ret.ApplyTag(match.Groups["tag"].Value, match.Groups["value"].Value, fileNameWithoutExtension);
                }

                // Get the title
                int braceIndex = fileNameWithoutExtension.IndexOf('[');
                if (braceIndex > 0)
                    ret.Title = fileNameWithoutExtension.Substring(0, braceIndex).Trim();
                else
                    ret.Title = fileNameWithoutExtension;

                if (ret.Title.Contains("[") || ret.Title.Contains("]"))
                {
                    throw new Exception("GrhData update failed for filename `" + fileNameWithoutExtension + "` because a [ or ] character was found in the sprite title." +
                        " Make sure your file name is correctly formed, and each [ has a matching ].");
                }

                return ret;
            }

            /// <summary>
            /// Apply a tag to this FileTags.
            /// </summary>
            /// <param name="tag">The tag character.</param>
            /// <param name="strValue">The corresponding tag value (unparsed string).</param>
            /// <param name="fileNameWithoutSuffix">The file name (for showing details in exception message).</param>
            void ApplyTag(string tag, string strValue, string fileNameWithoutSuffix)
            {
                tag = tag.ToLowerInvariant();

                if (tag == "s")
                {
                    // Animation speed
                    ushort val;
                    if (!ushort.TryParse(strValue, out val))
                        throw CreateApplyTagException(tag, strValue, fileNameWithoutSuffix, "Failed to parse the value as an ushort");

                    AnimationSpeed = val;
                }
                else if (tag == "w")
                {
                    // Wall
                    BoundWallType wall;
                    Rectangle? wallArea;

                    Match wallDefinitionMatch = _tagWallDefinitionRegex.Match(strValue);
                    if (wallDefinitionMatch.Success)
                    {
                        // Wall definition (doesn't cover whole sprite)
                        wall = (BoundWallType)int.Parse(wallDefinitionMatch.Groups["wallType"].Value);
                        wallArea = new Rectangle
                        {
                            X = int.Parse(wallDefinitionMatch.Groups["x"].Value),
                            Y = int.Parse(wallDefinitionMatch.Groups["y"].Value),
                            Width = int.Parse(wallDefinitionMatch.Groups["w"].Value),
                            Height = int.Parse(wallDefinitionMatch.Groups["h"].Value)
                        };
                    }
                    else
                    {
                        // Wall covers whole sprite
                        byte val;
                        if (!byte.TryParse(strValue, out val))
                            throw CreateApplyTagException(tag, strValue, fileNameWithoutSuffix, "Failed to parse the value as a byte");

                        wall = (BoundWallType)val;
                        wallArea = null;
                    }

                    if (!EnumHelper<BoundWallType>.IsDefined(wall))
                        throw CreateApplyTagException(tag, strValue, fileNameWithoutSuffix, string.Format("The value is not a valid BoundWallType value (use values {0} to {1})",
                            EnumHelper<BoundWallType>.MinValue, EnumHelper<BoundWallType>.MaxValue));

                    if (Walls == null)
                        Walls = new List<Tuple<BoundWallType, Rectangle?>>(1);

                    Walls.Add(new Tuple<BoundWallType, Rectangle?>(wall, wallArea));
                }
                else if (tag == "l")
                {
                    // Layer
                    byte val;
                    if (!byte.TryParse(strValue, out val))
                        throw CreateApplyTagException(tag, strValue, fileNameWithoutSuffix, "Failed to parse the value as a byte");

                    if (val == 0)
                        Layer = MapRenderLayer.SpriteBackground;
                    else if (val == 1)
                        Layer = MapRenderLayer.Dynamic;
                    else if (val == 2)
                        Layer = MapRenderLayer.SpriteForeground;
                    else
                    {
                        throw CreateApplyTagException(tag, strValue, fileNameWithoutSuffix, "The value is out of range (use 0 for background, 1 for dynamic, and 2 for foreground)");
                    }
                }
                else
                {
                    // Unknown/invalid
                    throw CreateApplyTagException(tag, strValue, fileNameWithoutSuffix, "The specified tag does not exist");
                }
            }

            static Exception CreateApplyTagException(string tag, string strValue, string fileNameWithoutSuffix, string reason)
            {
                return new Exception("GrhData update failed to parse the value `" + strValue + "` for tag `" + tag + "` on the file `" + fileNameWithoutSuffix +
                    "`. Reason: " + reason + ". Please correct the corresponding Grh file name under the Grh content directory.");
            }
        }
    }
}