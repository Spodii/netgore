using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics
{
    public abstract class GrhData
    {
        const string _categorizationValueKey = "Categorization";
        const string _indexValueKey = "Index";
        readonly GrhIndex _grhIndex;
        SpriteCategorization _categorization;

        /// <summary>
        /// Notifies listeners when the <see cref="GrhData"/>'s categorization has changed.
        /// </summary>
        public event GrhDataChangeCategorizationHandler OnChangeCategorization;

        protected GrhData(GrhIndex grhIndex, SpriteCategorization cat)
        {
            _categorization = cat;
            _grhIndex = grhIndex;
        }

        /// <summary>
        /// Gets the <see cref="SpriteCategorization"/> for this <see cref="GrhData"/>.
        /// </summary>
        public SpriteCategorization Categorization
        {
            get { return _categorization; }
        }

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

            GrhIndex index = GrhInfo.NextFreeIndex();
            Debug.Assert(GrhInfo.GetData(index) == null,
                         "Slot to use is already in use! How the hell did this happen!? GrhInfo.NextFreeIndex() must be broken.");

            var dc = DeepCopy(newCategorization, index);

            GrhInfo.AddGrhData(dc);

            return dc;
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
        public void SetCategorization(SpriteCategorization categorization)
        {
            if (categorization == null)
                throw new ArgumentNullException("categorization");

            // Check that either of the values are different
            if (_categorization == categorization)
                return;

            var oldCategorization = _categorization;
            _categorization = categorization;

            if (OnChangeCategorization != null)
                OnChangeCategorization(this, oldCategorization);
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
        public void Write(IValueWriter w)
        {
            // Check for valid data
            if (GrhIndex <= 0)
                throw new Exception("GrhIndex invalid.");
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
    }

    public sealed class AnimatedGrhData : GrhData
    {
        const string _framesNodeName = "Frames";
        const string _speedValueKey = "Speed";

        StationaryGrhData[] _frames;
        Vector2 _size;
        float _speed;

        public AnimatedGrhData(GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            _frames = new StationaryGrhData[0];
            _speed = 1f / 300f;
        }

        internal AnimatedGrhData(IValueReader r, GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            var speed = r.ReadInt(_speedValueKey);
            var frames = r.ReadMany(_framesNodeName, (xreader, xname) => xreader.ReadGrhIndex(xname));

            _speed = 1f / speed;
            _frames = CreateFrames(frames);
            _size = GetMaxSize(_frames);
        }

        /// <summary>
        /// Gets the frames in the GrhData
        /// </summary>
        public StationaryGrhData[] Frames
        {
            get { return _frames; }
        }

        /// <summary>
        /// Gets the number of frames in this <see cref="AnimatedGrhData"/>.
        /// </summary>
        public int FramesCount
        {
            get { return _frames.Length; }
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

        /// <summary>
        /// When overridden in the derived class, gets the size of the <see cref="GrhData"/>'s sprite in pixels.
        /// </summary>
        /// <value></value>
        public override Vector2 Size
        {
            get { return _size; }
        }

        /// <summary>
        /// Gets or sets the speed multiplier of the Grh animation where each frame lasts "Speed" milliseconds.
        /// </summary>
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        StationaryGrhData[] CreateFrames(GrhIndex[] frameIndices)
        {
            var frames = new StationaryGrhData[frameIndices.Length];
            for (int i = 0; i < frameIndices.Length; i++)
            {
                frames[i] = GrhInfo.GetData(frameIndices[i]) as StationaryGrhData;
                if (frames[i] == null)
                {
                    const string errmsg =
                        "Failed to load GrhData `{0}`. GrhData `{1}` needs it for frame index `{2}` (0-based), out of `{3}` frames total.";
                    string err = string.Format(errmsg, frames[i], this, i, frameIndices.Length);
                    throw new Exception(err);
                }
            }

            return frames;
        }

        protected override GrhData DeepCopy(SpriteCategorization newCategorization, GrhIndex newGrhIndex)
        {
            var copyArray = new StationaryGrhData[_frames.Length];
            Array.Copy(_frames, copyArray, _frames.Length);

            var copy = new AnimatedGrhData(newGrhIndex, newCategorization) { _frames = copyArray, Speed = Speed };

            return copy;
        }

        /// <summary>
        /// Gets the largest size of all the <see cref="GrhData"/>s.
        /// </summary>
        /// <param name="grhDatas">The <see cref="GrhData"/>s.</param>
        /// <returns>The largest size of all the <see cref="GrhData"/>s.</returns>
        static Vector2 GetMaxSize(IEnumerable<GrhData> grhDatas)
        {
            if (grhDatas == null || grhDatas.Count() == 0)
                return Vector2.Zero;

            Vector2 ret = Vector2.Zero;
            foreach (var f in grhDatas)
            {
                if (f.Size.X > ret.X)
                    ret.X = f.Size.X;
                if (f.Size.Y > ret.Y)
                    ret.Y = f.Size.Y;
            }

            return ret;
        }

        /// <summary>
        /// Reads a <see cref="GrhData"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>
        /// The <see cref="GrhData"/> read from the <see cref="IValueReader"/>.
        /// </returns>
        public static AnimatedGrhData Read(IValueReader r)
        {
            GrhIndex grhIndex;
            SpriteCategorization categorization;
            ReadHeader(r, out grhIndex, out categorization);

            return new AnimatedGrhData(r, grhIndex, categorization);
        }

        public void SetFrames(IEnumerable<GrhIndex> frameIndices)
        {
            SetFrames(CreateFrames(frameIndices.ToArray()));
        }

        public void SetFrames(IEnumerable<StationaryGrhData> frames)
        {
            _frames = frames.ToArray();
            _size = GetMaxSize(_frames);
        }

        /// <summary>
        /// When overridden in the derived class, writes the values unique to this derived type to the
        /// <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            var frameIndices = _frames.Select(x => x.GrhIndex).ToArray();

            writer.Write(_speedValueKey, (int)(1f / Speed));
            writer.WriteMany(_framesNodeName, frameIndices, writer.Write);
        }
    }

    public sealed class StationaryGrhData : GrhData, ITextureAtlasable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string _automaticSizeValueKey = "AutomaticSize";
        const string _textureNameValueKey = "Name";
        const string _textureNodeName = "Texture";
        const string _textureSourceValueKey = "Source";

        readonly ContentManager _cm;
        Rectangle _atlasSourceRect;
        bool _automaticSize = false;
        bool _isUsingAtlas;
        Rectangle _sourceRect;
        Texture2D _texture;
        TextureAssetName _textureName;

        /// <summary>
        /// Notifies listeners when the <see cref="GrhData"/>'s texture has changed.
        /// </summary>
        public event GrhDataChangeTextureHandler OnChangeTexture;

        public StationaryGrhData(ContentManager cm, GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            _automaticSize = true;
            _cm = cm;
            _isUsingAtlas = false;
        }

        internal StationaryGrhData(IValueReader r, ContentManager cm, GrhIndex grhIndex, SpriteCategorization cat)
            : base(grhIndex, cat)
        {
            _cm = cm;

            var automaticSize = r.ReadBool(_automaticSizeValueKey);
            var textureReader = r.ReadNode(_textureNodeName);
            var textureName = textureReader.ReadTextureAssetName(_textureNameValueKey);
            var textureSource = textureReader.ReadRectangle(_textureSourceValueKey);

            _textureName = textureName;
            _automaticSize = automaticSize;
            _sourceRect = textureSource;
        }

        /// <summary>
        /// Gets or sets if this GrhData automatically finds the Size by using the whole source texture.
        /// </summary>
        public bool AutomaticSize
        {
            get { return _automaticSize; }
            set
            {
                if (AutomaticSize == value)
                    return;

                _automaticSize = value;

                if (AutomaticSize)
                {
                    _isUsingAtlas = false;
                    _texture = null;
                    ValidateTexture();

                    if (Texture == null || Texture.IsDisposed)
                    {
                        const string errmsg = "GrhData `{0}` cannot be automatically sized since the texture is null or disposed!";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, this);
                        _sourceRect = new Rectangle(0, 0, 1, 1);
                    }
                    else
                        _sourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="ContentManager"/> used to load the content for this <see cref="GrhData"/>.
        /// </summary>
        public ContentManager ContentManager
        {
            get { return _cm; }
        }

        /// <summary>
        /// Gets the pixel height for a single frame Grh (SourceRect.Height)
        /// </summary>
        public int Height
        {
            get { return _sourceRect.Height; }
        }

        /// <summary>
        /// Gets the source rectangle of the GrhData on the original texture. This value will remain the same even
        /// when a texture atlas is used.
        /// </summary>
        public Rectangle OriginalSourceRect
        {
            get { return _sourceRect; }
        }

        /// <summary>
        /// Gets the zero-base source pixel position (top-left corner) for a single frame GrhData.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                ValidateTexture();
                return _isUsingAtlas
                           ? new Vector2(_atlasSourceRect.X, _atlasSourceRect.Y) : new Vector2(_sourceRect.X, _sourceRect.Y);
            }
        }

        public override Vector2 Size
        {
            get { return new Vector2(_sourceRect.Width, _sourceRect.Height); }
        }

        /// <summary>
        /// Gets the name of the texture used by the GrhData.
        /// </summary>
        public TextureAssetName TextureName
        {
            get { return _textureName; }
        }

        /// <summary>
        /// Gets the pixel width for a single frame GrhData.
        /// </summary>
        public int Width
        {
            get { return _sourceRect.Width; }
        }

        /// <summary>
        /// Gets the pixel X coordinate for a single frame GrhData.
        /// </summary>
        public int X
        {
            get
            {
                ValidateTexture();
                return _isUsingAtlas ? _atlasSourceRect.X : _sourceRect.X;
            }
        }

        /// <summary>
        /// Gets the pixel Y coordinate for a single frame GrhData.
        /// </summary>
        public int Y
        {
            get
            {
                ValidateTexture();
                return _isUsingAtlas ? _atlasSourceRect.Y : _sourceRect.Y;
            }
        }

        /// <summary>
        /// Changes the texture for a stationary <see cref="GrhData"/>.
        /// </summary>
        /// <param name="newTexture">Name of the new texture to use.</param>
        /// <param name="source">A <see cref="Rectangle"/> describing the source area of the texture to
        /// use for this <see cref="GrhData"/>.</param>
        public void ChangeTexture(TextureAssetName newTexture, Rectangle source)
        {
            if (newTexture == null)
                throw new ArgumentNullException("newTexture");

            // Check that the values have changed
            if (source == _sourceRect && TextureName == newTexture)
                return;

            _sourceRect = source;

            // Check that it is actually a different texture
            TextureAssetName oldTextureName = null;
            if (TextureName != newTexture)
                oldTextureName = _textureName;

            // Apply the new texture
            _texture = null;
            _isUsingAtlas = false;
            _textureName = newTexture;

            ValidateTexture();

            if (oldTextureName != null && OnChangeTexture != null)
                OnChangeTexture(this, oldTextureName);
        }

        /// <summary>
        /// Changes the texture for a stationary <see cref="GrhData"/>.
        /// </summary>
        /// <param name="newTexture">Name of the new texture to use.</param>
        public void ChangeTexture(TextureAssetName newTexture)
        {
            ChangeTexture(newTexture, GetOriginalSource());
        }

        /// <summary>
        /// When overridden in the derived class, creates a new <see cref="GrhData"/> equal to this <see cref="GrhData"/>
        /// except for the specified parameters.
        /// </summary>
        /// <param name="newCategorization">The <see cref="SpriteCategorization"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <param name="newGrhIndex">The <see cref="GrhIndex"/> to give to the new
        /// <see cref="GrhData"/>.</param>
        /// <returns>
        /// A deep copy of this <see cref="GrhData"/>.
        /// </returns>
        protected override GrhData DeepCopy(SpriteCategorization newCategorization, GrhIndex newGrhIndex)
        {
            StationaryGrhData copy = new StationaryGrhData(ContentManager, newGrhIndex, newCategorization)
            { _textureName = TextureName, _sourceRect = _sourceRect, _automaticSize = _automaticSize };

            return copy;
        }

        /// <summary>
        /// Gets the original source rectangle, bypassing any applied atlas
        /// </summary>
        public Rectangle GetOriginalSource()
        {
            return _sourceRect;
        }

        /// <summary>
        /// Reads a <see cref="GrhData"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> to read from.</param>
        /// <param name="cm">The <see cref="ContentManager"/> used to load content.</param>
        /// <returns>
        /// The <see cref="GrhData"/> read from the <see cref="IValueReader"/>.
        /// </returns>
        public static StationaryGrhData Read(IValueReader r, ContentManager cm)
        {
            GrhIndex grhIndex;
            SpriteCategorization categorization;
            ReadHeader(r, out grhIndex, out categorization);

            return new StationaryGrhData(r, cm, grhIndex, categorization);
        }

        /// <summary>
        /// Ensures that the texture is properly loaded.
        /// </summary>
        void ValidateTexture()
        {
            // If the texture is not set or is disposed, request a new one
            if (_texture != null && !_texture.IsDisposed)
                return;

            // Try to load the texture
            try
            {
                _texture = _cm.Load<Texture2D>(_textureName);
            }
            catch (ContentLoadException ex)
            {
                const string errmsg = "Failed to load texture `{0}` for GrhData `{1}`: {2}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _textureName, this, ex);
            }

            // If we were using an atlas, we'll have to remove it because the texture was reloaded
            _isUsingAtlas = false;
        }

        /// <summary>
        /// When overridden in the derived class, writes the values unique to this derived type to the
        /// <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            writer.Write(_automaticSizeValueKey, AutomaticSize);
            writer.WriteStartNode(_textureNodeName);
            writer.Write(_textureNameValueKey, TextureName);
            writer.Write(_textureSourceValueKey, GetOriginalSource());
            writer.WriteEndNode(_textureNodeName);
        }

        #region ITextureAtlasable Members

        /// <summary>
        /// Gets the texture source <see cref="Rectangle"/> of the original image.
        /// </summary>
        public Rectangle SourceRect
        {
            get
            {
                ValidateTexture();
                return _isUsingAtlas ? _atlasSourceRect : _sourceRect;
            }
        }

        /// <summary>
        /// Gets the texture for a single frame Grh.
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                ValidateTexture();
                return _texture;
            }
        }

        /// <summary>
        /// Sets the atlas information.
        /// </summary>
        /// <param name="texture">Texture atlas.</param>
        /// <param name="atlasSourceRect">Source rectangle in the atlas.</param>
        void ITextureAtlasable.SetAtlas(Texture2D texture, Rectangle atlasSourceRect)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");

            // Set the atlas usage values
            _atlasSourceRect = atlasSourceRect;
            _texture = texture;
            _isUsingAtlas = true;
        }

        /// <summary>
        /// Removes the atlas from the object and forces it to draw normally.
        /// </summary>
        public void RemoveAtlas()
        {
            if (!_isUsingAtlas)
                return;

            _isUsingAtlas = false;
            _texture = null;
        }

        #endregion
    }
}