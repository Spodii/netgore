using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Content;
using NetGore.IO;
using SFML;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="GrhData"/> that only contains a single frame. This is the core of all <see cref="GrhData"/>s as it
    /// is also what each frame of an animated <see cref="GrhData"/> contains.
    /// </summary>
    public sealed class StationaryGrhData : GrhData, ITextureAtlasable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string _automaticSizeValueKey = "AutomaticSize";
        const string _textureNameValueKey = "Name";
        const string _textureNodeName = "Texture";
        const string _textureSourceValueKey = "Source";

        readonly IContentManager _cm;

        Rectangle _atlasSourceRect;
        bool _automaticSize = false;

        /// <summary>
        /// How many times the texture has failed to load in a row.
        /// </summary>
        byte _failedLoadAttempts = 0;

        bool _isUsingAtlas = false;

        /// <summary>
        /// The current time must be greater than or equal to this value for the texture to allow retrying to reload.
        /// </summary>
        TickCount _nextLoadAttemptTime = TickCount.MinValue;

        ushort _sourceHeight = 0;
        ushort _sourceWidth = 0;
        ushort _sourceX = 0;
        ushort _sourceY = 0;

        Texture _texture;
        TextureAssetName _textureName;

        /// <summary>
        /// Initializes a new instance of the <see cref="StationaryGrhData"/> class.
        /// </summary>
        /// <param name="cm">The <see cref="IContentManager"/>.</param>
        /// <param name="grhIndex">The <see cref="GrhIndex"/>.</param>
        /// <param name="cat">The <see cref="SpriteCategorization"/>.</param>
        /// <param name="textureName">Name of the texture.</param>
        /// <param name="textureSource">The area of the texture to use, or null for the whole texture.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cat"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="grhIndex"/> is equal to GrhIndex.Invalid.</exception>
        public StationaryGrhData(IContentManager cm, GrhIndex grhIndex, SpriteCategorization cat, TextureAssetName textureName,
                                 Rectangle? textureSource) : base(grhIndex, cat)
        {
            _cm = cm;
            _textureName = textureName;

            if (textureSource == null)
                AutomaticSize = true;
            else
                SetSourceRect(textureSource.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StationaryGrhData"/> class.
        /// </summary>
        /// <param name="autoGrhData">The <see cref="AutomaticAnimatedGrhData"/>.</param>
        /// <param name="assetName">Name of the asset.</param>
        internal StationaryGrhData(AutomaticAnimatedGrhData autoGrhData, TextureAssetName assetName)
            : base(autoGrhData.Categorization)
        {
            _cm = autoGrhData.ContentManager;
            _textureName = assetName;
            AutomaticSize = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StationaryGrhData"/> class.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/>.</param>
        /// <param name="cm">The <see cref="IContentManager"/>.</param>
        /// <param name="grhIndex">The <see cref="GrhIndex"/>.</param>
        /// <param name="cat">The <see cref="SpriteCategorization"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cat"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="grhIndex"/> is equal to GrhIndex.Invalid.</exception>
        StationaryGrhData(IValueReader r, IContentManager cm, GrhIndex grhIndex, SpriteCategorization cat) : base(grhIndex, cat)
        {
            _cm = cm;

            var automaticSize = r.ReadBool(_automaticSizeValueKey);
            var textureReader = r.ReadNode(_textureNodeName);
            var textureName = textureReader.ReadTextureAssetName(_textureNameValueKey);
            var textureSource = textureReader.ReadRectangle(_textureSourceValueKey);

            _textureName = textureName;
            SetSourceRect(textureSource);
            _automaticSize = automaticSize;
        }

        /// <summary>
        /// Notifies listeners when the <see cref="GrhData"/>'s texture has changed.
        /// </summary>
        public event TypedEventHandler<GrhData, EventArgs<ContentAssetName>> TextureChanged;

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
                    RemoveAtlas();
                    ValidateTexture();

                    if (Texture == null)
                    {
                        // Cannot set the sizes since the texture is invalid
                        SetSourceRect(0, 0, 0, 0);
                    }
                    else
                    {
                        // Use the correct sizes
                        var textureSize = Texture.Size;
                        SetSourceRect(0, 0, (int)textureSize.X, (int)textureSize.Y);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="IContentManager"/> used to load the content for this <see cref="GrhData"/>.
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
            get { return new StationaryGrhData[] { this }; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the number of frames in this <see cref="GrhData"/>. If this
        /// is not an animated <see cref="GrhData"/>, this value will always return 0.
        /// </summary>
        public override int FramesCount
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the pixel height for a single frame Grh (SourceRect.Height).
        /// </summary>
        public int Height
        {
            get
            {
                ValidateAutomaticSize();
                return _sourceHeight;
            }
        }

        /// <summary>
        /// Gets the source rectangle of the GrhData on the original texture. This value will remain the same even
        /// when a texture atlas is used.
        /// </summary>
        public Rectangle OriginalSourceRect
        {
            get
            {
                ValidateAutomaticSize();
                return new Rectangle(_sourceX, _sourceY, _sourceWidth, _sourceHeight);
            }
        }

        /// <summary>
        /// Gets the zero-base source pixel position (top-left corner) for a single frame GrhData.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                ValidateTexture();
                return _isUsingAtlas ? new Vector2(_atlasSourceRect.X, _atlasSourceRect.Y) : new Vector2(_sourceX, _sourceY);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the size of the <see cref="GrhData"/>'s sprite in pixels.
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                ValidateAutomaticSize();
                return new Vector2(_sourceWidth, _sourceHeight);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the speed multiplier of the <see cref="GrhData"/> animation where each
        /// frame lasts 1f/Speed milliseconds. For non-animated <see cref="GrhData"/>s, this value will always be 0.
        /// </summary>
        public override float Speed
        {
            get { return 0; }
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
            get
            {
                ValidateAutomaticSize();
                return _sourceWidth;
            }
        }

        /// <summary>
        /// Gets the pixel X coordinate for a single frame GrhData.
        /// </summary>
        public int X
        {
            get
            {
                ValidateTexture();
                return _isUsingAtlas ? _atlasSourceRect.X : _sourceX;
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
                return _isUsingAtlas ? _atlasSourceRect.Y : _sourceY;
            }
        }

        /// <summary>
        /// Changes the texture for a stationary <see cref="GrhData"/>.
        /// </summary>
        /// <param name="newTexture">Name of the new texture to use.</param>
        /// <param name="source">A <see cref="Rectangle"/> describing the source area of the texture to
        /// use for this <see cref="GrhData"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="newTexture" /> is <c>null</c>.</exception>
        public void ChangeTexture(TextureAssetName newTexture, Rectangle source)
        {
            if (newTexture == null)
                throw new ArgumentNullException("newTexture");

            // Check that the values have changed
            if (source == SourceRect && TextureName == newTexture)
                return;

            SetSourceRect(source);

            // Check that it is actually a different texture
            TextureAssetName oldTextureName = null;
            if (TextureName != newTexture)
                oldTextureName = _textureName;

            // Apply the new texture
            _texture = null;
            _isUsingAtlas = false;
            _textureName = newTexture;

            ValidateTexture();

            if (oldTextureName != null)
            {
                if (TextureChanged != null)
                    TextureChanged.Raise(this, EventArgsHelper.Create<ContentAssetName>(oldTextureName));
            }
        }

        /// <summary>
        /// Changes the texture for a stationary <see cref="GrhData"/>.
        /// </summary>
        /// <param name="newTexture">Name of the new texture to use.</param>
        public void ChangeTexture(TextureAssetName newTexture)
        {
            ChangeTexture(newTexture, OriginalSourceRect);
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
            var copy = new StationaryGrhData(ContentManager, newGrhIndex, newCategorization, TextureName,
                AutomaticSize ? (Rectangle?)null : SourceRect);
            return copy;
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
            return this;
        }

        /// <summary>
        /// Gets the timeout in milliseconds to wait before trying to load a texture again.
        /// </summary>
        /// <param name="failedLoadAttempts">The number of times the texture has failed to load.</param>
        /// <returns>The timeout in milliseconds to wait before trying to load a texture again.</returns>
        static int GetLoadTextureTimeout(int failedLoadAttempts)
        {
            // If 8 or more failed attempts, it is almost definite this texture isn't loading. However,
            // we will allow it to retry after 30 seconds... just in case it magically starts working again.
            if (failedLoadAttempts >= 8)
                return 1000 * 30;

            // Set the base delay to half a second, so we always wait at least half a second to try again
            var delay = 500;

            // If 3 or more failed load attempts, each failure results in another second being added
            if (failedLoadAttempts >= 3)
                delay += failedLoadAttempts * 1000;

            return delay;
        }

        /// <summary>
        /// Gets the original texture for this <see cref="StationaryGrhData"/>, bypassing any <see cref="TextureAtlas"/> being used.
        /// </summary>
        /// <returns>The original texture for this <see cref="StationaryGrhData"/>.</returns>
        public Texture GetOriginalTexture()
        {
            if (!_isUsingAtlas)
                return Texture;

            return _cm.LoadImage(_textureName, GrhInfo.ContentLevelDecider(this));
        }

        /// <summary>
        /// Reads a <see cref="GrhData"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> to read from.</param>
        /// <param name="cm">The <see cref="IContentManager"/> used to load content.</param>
        /// <returns>
        /// The <see cref="GrhData"/> read from the <see cref="IValueReader"/>.
        /// </returns>
        public static StationaryGrhData Read(IValueReader r, IContentManager cm)
        {
            GrhIndex grhIndex;
            SpriteCategorization categorization;
            ReadHeader(r, out grhIndex, out categorization);

            return new StationaryGrhData(r, cm, grhIndex, categorization);
        }

        /// <summary>
        /// Sets the source rectangle.
        /// </summary>
        /// <param name="r">The source <see cref="Rectangle"/>.</param>
        void SetSourceRect(Rectangle r)
        {
            SetSourceRect(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Sets the source rectangle.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="x"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="y"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="width"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="height"/> is less than zero.</exception>
        void SetSourceRect(int x, int y, int width, int height)
        {
            if (x < 0)
                throw new ArgumentOutOfRangeException("x");
            if (y < 0)
                throw new ArgumentOutOfRangeException("y");
            if (width < 0)
                throw new ArgumentOutOfRangeException("width");
            if (height < 0)
                throw new ArgumentOutOfRangeException("height");

            _sourceX = (ushort)x;
            _sourceY = (ushort)y;
            _sourceWidth = (ushort)width;
            _sourceHeight = (ushort)height;
        }

        /// <summary>
        /// Updates the size of a <see cref="StationaryGrhData"/> where <see cref="AutomaticSize"/> is set. This should only ever
        /// need to be called by editors, and never the client.
        /// </summary>
        /// <param name="newSize">The new, correct size of the texture used by the <see cref="StationaryGrhData"/>.</param>
        /// <exception cref="InvalidOperationException">This method can only be called when <see cref="AutomaticSize"/> is set.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutomaticSize")]
        public void UpdateAutomaticSize(Vector2 newSize)
        {
            if (!AutomaticSize)
                throw new InvalidOperationException("This method can only be called when AutomaticSize is set.");

            _sourceWidth = (ushort)newSize.X;
            _sourceHeight = (ushort)newSize.Y;
        }

        /// <summary>
        /// Validates the size of the <see cref="StationaryGrhData"/> that uses <see cref="AutomaticSize"/>.
        /// </summary>
        void ValidateAutomaticSize()
        {
            if (_sourceWidth > 0 || !AutomaticSize || Texture == null || _isUsingAtlas)
                return;

            var textureSize = Texture.Size;
            _sourceWidth = (ushort)textureSize.X;
            _sourceHeight = (ushort)textureSize.Y;
        }

        /// <summary>
        /// Ensures that the texture is properly loaded.
        /// </summary>
        void ValidateTexture()
        {
            // If the texture is not set or is disposed, request a new one
            if (_texture != null)
                return;

            // Check that enough time has elapsed to try and load the texture
            if (_failedLoadAttempts > 0 && _nextLoadAttemptTime > TickCount.Now)
                return;

            // Try to load the texture
            const string errmsg = "Failed to load texture `{0}` for GrhData `{1}`: {2}";
            try
            {
                _texture = _cm.LoadImage(_textureName, GrhInfo.ContentLevelDecider(this));
            }
            catch (LoadingFailedException ex)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _textureName, this, ex);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _textureName, this, ex);
                Debug.Fail(string.Format(errmsg, _textureName, this, ex));
            }

            // Update the failed loading information if the texture failed to load, or clear it if the texture
            // is valid
            if (_texture != null)
            {
                _failedLoadAttempts = 0;
                _nextLoadAttemptTime = TickCount.MinValue;
            }
            else
            {
                _failedLoadAttempts++;
                _nextLoadAttemptTime = (TickCount)(TickCount.Now + GetLoadTextureTimeout(_failedLoadAttempts));
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
            writer.Write(_textureSourceValueKey, OriginalSourceRect);
            writer.WriteEndNode(_textureNodeName);
        }

        #region ITextureAtlasable Members

        /// <summary>
        /// Gets the texture source <see cref="Rectangle"/> of the image.
        /// </summary>
        public Rectangle SourceRect
        {
            get
            {
                ValidateTexture();

                if (_isUsingAtlas)
                    return _atlasSourceRect;

                return new Rectangle(_sourceX, _sourceY, _sourceWidth, _sourceHeight);
            }
        }

        /// <summary>
        /// Gets the texture for a single frame Grh.
        /// </summary>
        public Texture Texture
        {
            get
            {
                ValidateTexture();
                return _texture;
            }
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

        /// <summary>
        /// Removes the atlas from the object and forces it to draw normally only if the given atlas
        /// is the atlas being used. If a different atlas is used, then it will not be removed.
        /// </summary>
        /// <param name="atlas">If the <see cref="ITextureAtlasable"/> is using this atlas, then the atlas
        /// should be removed.</param>
        void ITextureAtlasable.RemoveAtlas(Texture atlas)
        {
            if (_isUsingAtlas && _texture == atlas)
                RemoveAtlas();
        }

        /// <summary>
        /// Sets the atlas information.
        /// </summary>
        /// <param name="texture">Texture atlas.</param>
        /// <param name="atlasSourceRect">Source rectangle in the atlas.</param>
        /// <exception cref="ArgumentNullException"><paramref name="texture" /> is <c>null</c>.</exception>
        void ITextureAtlasable.SetAtlas(Texture texture, Rectangle atlasSourceRect)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");

            // Set the atlas usage values
            _atlasSourceRect = atlasSourceRect;
            _texture = texture;
            _isUsingAtlas = true;

            // Clear texture loading fail count
            _failedLoadAttempts = 0;
            _nextLoadAttemptTime = TickCount.MinValue;
        }

        #endregion
    }
}