using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Holds information representing how a Grh is displayed and functions
    /// </summary>
    public class GrhData : ITextureAtlasable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const string _automaticSizeValueKey = "AutomaticSize";
        const string _categoryNameValueKey = "Name";
        const string _categoryNodeName = "Category";
        const string _categoryTitleValueKey = "Title";
        const string _framesNodeName = "Frames";
        const string _indexKey = "Index";
        const string _speedValueKey = "Speed";
        const string _textureNameValueKey = "Name";
        const string _textureNodeName = "Texture";
        const string _textureSourceValueKey = "Source";

        Rectangle _atlasSourceRect;
        bool _automaticSize = false;
        SpriteCategorization _categorization;
        ContentManager _cm;
        ImmutableArray<GrhData> _frames;
        GrhIndex _grhIndex;
        bool _isUsingAtlas;
        Rectangle _sourceRect;
        float _speed;
        Texture2D _texture;
        TextureAssetName _textureName;

        /// <summary>
        /// Notifies listeners when the <see cref="GrhData"/>'s categorization has changed.
        /// </summary>
        public event GrhDataChangeCategorizationHandler OnChangeCategorization;

        /// <summary>
        /// Notifies listeners when the <see cref="GrhData"/>'s texture has changed.
        /// </summary>
        public event GrhDataChangeTextureHandler OnChangeTexture;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhData"/> class.
        /// </summary>
        /// <param name="r">IValueReader to read the values from.</param>
        /// <param name="cm">ContentManager to use.</param>
        internal GrhData(IValueReader r, ContentManager cm)
        {
            Read(r, cm);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhData"/> class.
        /// </summary>
        internal GrhData()
        {
            // Restrict construction to internal only
        }

        /// <summary>
        /// Gets or sets if this GrhData automatically finds the Size by using the whole source texture. Only applies to
        /// stationary GrhDatas.
        /// </summary>
        public bool AutomaticSize
        {
            get { return _automaticSize; }
            set
            {
                if (IsAnimated || AutomaticSize == value)
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
        /// Gets the <see cref="SpriteCategorization"/> for this <see cref="GrhData"/>.
        /// </summary>
        public SpriteCategorization Categorization
        {
            get { return _categorization; }
        }

        /// <summary>
        /// Gets the <see cref="ContentManager"/> used to load the content for this <see cref="GrhData"/>.
        /// </summary>
        public ContentManager ContentManager
        {
            get { return _cm; }
        }

        /// <summary>
        /// Gets the frames in the GrhData
        /// </summary>
        public ImmutableArray<GrhData> Frames
        {
            get { return _frames; }
        }

        /// <summary>
        /// Gets the index of the GrhData. If stationary, this is the same GrhIndex as the first frame. If animated,
        /// this is the index that represents the series of Grhs under it. This is the same index as
        /// used in the GrhInfo.Data[] array.
        /// </summary>
        public GrhIndex GrhIndex
        {
            get { return _grhIndex; }
        }

        /// <summary>
        /// Gets the pixel height for a single frame Grh (SourceRect.Height)
        /// </summary>
        public int Height
        {
            get { return _sourceRect.Height; }
        }

        /// <summary>
        /// Gets if this GrhData is animated. If false, this is a stationary GrhData.
        /// </summary>
        public bool IsAnimated
        {
            get { return _frames.Length > 1; }
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

        /// <summary>
        /// Gets the source pixel size for a single frame GrhData.
        /// </summary>
        public Vector2 Size
        {
            get { return new Vector2(_sourceRect.Width, _sourceRect.Height); }
        }

        /// <summary>
        /// Gets or sets the speed multiplier of the Grh animation where each frame lasts "Speed" milliseconds.
        /// </summary>
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
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
        public void ChangeTexture(TextureAssetName newTexture)
        {
            if (newTexture == null)
                throw new ArgumentNullException("newTexture");

            // Ensure this is not an animated GrhData
            if (IsAnimated)
                throw new MemberAccessException("Cannot change the texture of an animated GrhData.");

            // Check that it is actually a different texture
            if (TextureName == newTexture)
                return;

            var oldTextureName = _textureName;

            // Apply the new texture
            _texture = null;
            _isUsingAtlas = false;
            _textureName = newTexture;

            ValidateTexture();

            if (OnChangeTexture != null)
                OnChangeTexture(this, oldTextureName);
        }

        /// <summary>
        /// Creates a duplicate of the <see cref="GrhData"/>.
        /// </summary>
        /// <param name="newCategorization">Categorization for the duplicated GrhData. Must be unique.</param>
        /// <returns>Deep copy of the <see cref="GrhData"/> with the new categorization and its own
        /// unique <see cref="GrhIndex"/>.</returns>
        public GrhData Duplicate(SpriteCategorization newCategorization)
        {
            GrhIndex index = GrhInfo.NextFreeIndex();
            Debug.Assert(GrhInfo.GetData(index) == null,
                         "Slot to use is already in use! How the hell did this happen!? GrhInfo.NextFreeIndex() must be broken.");

            if (GrhInfo.GetData(newCategorization) != null)
                throw new ArgumentException("Category already in use.", "newCategorization");

            GrhData gd = new GrhData
            {
                _categorization = newCategorization,
                _cm = _cm,
                _frames = _frames,
                _grhIndex = index,
                _sourceRect = _sourceRect,
                _speed = _speed,
                _texture = _texture,
                _textureName = _textureName,
                _isUsingAtlas = _isUsingAtlas,
                _atlasSourceRect = _atlasSourceRect
            };

            GrhInfo.AddGrhData(gd);

            return gd;
        }

        /// <summary>
        /// Gets the original source rectangle, bypassing any applied atlas
        /// </summary>
        public Rectangle GetOriginalSource()
        {
            return _sourceRect;
        }

        /// <summary>
        /// Sets the data for a single <see cref="GrhData"/> (no animation).
        /// </summary>
        /// <param name="cm">ContentManager used by this texture.</param>
        /// <param name="grhIndex">Index of the <see cref="Grh"/>.</param>
        /// <param name="textureName">Name of the texture asset.</param>
        /// <param name="x">Pixel x coordinate of the source texture.</param>
        /// <param name="y">Pixel y coordinate of the source texture.</param>
        /// <param name="width">Pixel width of the source texture.</param>
        /// <param name="height">Pixel height of the source texture.</param>
        /// <param name="categorization">Unique categorization.</param>
        public void Load(ContentManager cm, GrhIndex grhIndex, string textureName, int x, int y, int width, int height,
                         SpriteCategorization categorization)
        {
            if (cm == null)
            {
                Debug.Fail("cm is null.");
                if (log.IsFatalEnabled)
                    log.Fatal("Parameter `cm`, ContentManager, is null.");
                throw new ArgumentNullException("cm");
            }

            if (GrhInfo.GetData(categorization) != null)
                throw new ArgumentException("Category already in use.", "categorization");

            // We only have one frame, this one
            var frames = new GrhData[1];
            frames[0] = this;
            _frames = new ImmutableArray<GrhData>(frames);

            // Store some values and references
            _cm = cm;
            _textureName = textureName;
            _grhIndex = grhIndex;
            _sourceRect = new Rectangle(x, y, width, height);

            // Set the categorization
            SetCategorization(categorization);
        }

        ImmutableArray<GrhData> CreateFrames(GrhIndex[] frameIndices)
        {
            var frames = new GrhData[frameIndices.Length];
            for (int i = 0; i < frameIndices.Length; i++)
            {
                frames[i] = GrhInfo.GetData(frameIndices[i]);
                if (frames[i] == null)
                {
                    const string errmsg =
                        "Failed to load GrhData `{0}`. GrhData `{1}` needs it for frame index `{2}` (0-based), out of `{3}` frames total.";
                    string err = string.Format(errmsg, frames[i], this, i, frameIndices.Length);
                    throw new Exception(err);
                }
            }

            return new ImmutableArray<GrhData>(frames);
        }

        /// <summary>
        /// Sets the <see cref="Frames"/> for an animated <see cref="GrhData"/>.
        /// </summary>
        /// <param name="frameIndices">The indices of the <see cref="GrhData"/> frames to build the animation from.</param>
        /// <exception cref="MethodAccessException">IsAnimated is not set.</exception>
        public void SetFrames(GrhIndex[] frameIndices)
        {
            if (!IsAnimated)
                throw new MethodAccessException("Method may only be accessed when IsAnimated is not set.");

            _frames = CreateFrames(frameIndices);
        }

        /// <summary>
        /// Sets the data for an animated <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhIndex">Index of the <see cref="GrhData"/>.</param>
        /// <param name="frameIndices">The indices of the <see cref="GrhData"/> frames to build the animation from.</param>
        /// <param name="speed">The speed of the animation.</param>
        /// <param name="categorization">The unique categorization..</param>
        public void Load(GrhIndex grhIndex, GrhIndex[] frameIndices, float speed, SpriteCategorization categorization)
        {
            if (GrhInfo.GetData(categorization) != null)
                throw new ArgumentException("Category already in use.", "categorization");

            // Create the frames
            _frames = CreateFrames(frameIndices);

            // Store some values and references
            _grhIndex = grhIndex;
            _speed = speed;
            if (_speed > 1f)
                _speed = 1f / _speed;

            // Animated GrhDatas don't have a single texture, so there can't be a texture name
            _textureName = null;

            // Set the categorization
            SetCategorization(categorization);
        }

        /// <summary>
        /// Reads and loads the GrhData from an IValueReader.
        /// </summary>
        /// <param name="r">IValueReader to read the values from.</param>
        /// <param name="cm">ContentManager to use.</param>
        public void Read(IValueReader r, ContentManager cm)
        {
            GrhIndex grhIndex = r.ReadGrhIndex(_indexKey);
            var frames = r.ReadMany(_framesNodeName, ((reader, name) => reader.ReadGrhIndex(name)));

            IValueReader categoryNodeReader = r.ReadNode(_categoryNodeName);
            string categoryName = categoryNodeReader.ReadString(_categoryNameValueKey);
            string categoryTitle = categoryNodeReader.ReadString(_categoryTitleValueKey);
            var categorization = new SpriteCategorization(categoryName, categoryTitle);

            if (frames.Length <= 1)
            {
                // Single frame
                bool automaticSize = r.ReadBool(_automaticSizeValueKey);

                IValueReader textureNodeReader = r.ReadNode(_textureNodeName);
                string textureName = textureNodeReader.ReadString(_textureNameValueKey);
                Rectangle source = textureNodeReader.ReadRectangle(_textureSourceValueKey);

                Load(cm, grhIndex, textureName, source.X, source.Y, source.Width, source.Height, categorization);
                AutomaticSize = automaticSize;
            }
            else
            {
                // Animated
                float speed = r.ReadFloat(_speedValueKey);

                Load(grhIndex, frames, speed, categorization);
            }
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
            // Add the frame count if animated, or the texture if not animated
            string extra;
            if (_frames.Length > 1)
                extra = _frames.Length + " frames";
            else
                extra = _textureName.ToString();

            // Create the categorization display, and add the extra info
            return string.Format("{0} - {1} [{2}]", _categorization, extra, _grhIndex);
        }

        /// <summary>
        /// Ensures that the texture is properly loaded.
        /// </summary>
        void ValidateTexture()
        {
            if (IsAnimated)
                return;

            // If the texture is not set or is disposed, request a new one
            if (_texture == null || _texture.IsDisposed)
            {
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

            // Header
            w.Write(_indexKey, GrhIndex);
            w.WriteMany(_framesNodeName, Frames.Select(x => x.GrhIndex).ToArray(), w.Write);

            // Category
            w.WriteStartNode(_categoryNodeName);
            {
                w.Write(_categoryNameValueKey, Categorization.Category);
                w.Write(_categoryTitleValueKey, Categorization.Title);
            }
            w.WriteEndNode(_categoryNodeName);

            if (!IsAnimated)
            {
                // Single frame
                Debug.Assert(TextureName != null, "TextureName is null or invalid for a non-animation.");
                Debug.Assert(SourceRect.Width > 0, "SourceRect.Width must be > 0.");
                Debug.Assert(SourceRect.Height > 0, "SourceRect.Height must be > 0.");

                w.Write(_automaticSizeValueKey, AutomaticSize);

                w.WriteStartNode(_textureNodeName);
                {
                    w.Write(_textureNameValueKey, TextureName);
                    w.Write(_textureSourceValueKey, GetOriginalSource());
                }
                w.WriteEndNode(_textureNodeName);
            }
            else
            {
                // Animated
                w.Write(_speedValueKey, (1f / Speed));
            }
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

            // An atlas may only be used on a frame, not an animation
            if (IsAnimated)
            {
                const string errmsg = "An atlas may only be applied on a frame, not an animation!";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                Debug.Fail(errmsg);
                return;
            }

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