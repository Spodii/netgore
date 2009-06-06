using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Holds information representing how a Grh is displayed and functions
    /// </summary>
    public class GrhData : ITextureAtlas
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        Rectangle _atlasSourceRect;
        bool _automaticSize = false;
        string _category;
        ContentManager _cm;
        GrhData[] _frames;
        ushort _grhIndex;
        bool _isUsingAtlas;
        Rectangle _sourceRect;
        float _speed;
        Texture2D _texture;
        string _textureName;
        string _title;

        /// <summary>
        /// Notifies when either the category or title have been changed
        /// </summary>
        public event ChangeCategorizationHandler OnChangeCategorization;

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
                    _sourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
                }
            }
        }

        /// <summary>
        /// Gets the category of the GrhData (delimited by a period). If the category has not yet
        /// been set (Load() has not been called on this GrhData), this value will be null. Other
        /// than that, this value will never be null.
        /// </summary>
        public string Category
        {
            get { return _category; }
        }

        /// <summary>
        /// Gets the ContentManager used by the GrhData
        /// </summary>
        public ContentManager ContentManager
        {
            get { return _cm; }
            set { _cm = value; }
        }

        /// <summary>
        /// Gets the frames in the GrhData
        /// </summary>
        public GrhData[] Frames
        {
            get { return _frames; }
        }

        /// <summary>
        /// GrhIndex of the Grh. If stationary, this is the same Grh as the first frame. If animated,
        /// this is the index that represents the series of Grhs under it. This is the same index as
        /// used in the GrhInfo.Data[] array.
        /// </summary>
        public ushort GrhIndex
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
            get { return _frames.Length > 1 && _frames[0] != this; }
        }

        /// <summary>
        /// Gets the zero-base source pixel position (top-left corner) for a single frame GrhData.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                ValidateTexture();
                return new Vector2(_sourceRect.Left, _sourceRect.Top);
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
        /// Gets the speed multiplier of the Grh animation where each frame lasts "Speed" milliseconds.
        /// </summary>
        public float Speed
        {
            get { return _speed; }
        }

        /// <summary>
        /// Gets the name of the texture used by the GrhData. Use instead of Texture.Name
        /// when possible to prevent needless overhead and texture loading.
        /// </summary>
        public string TextureName
        {
            get { return _textureName; }
        }

        /// <summary>
        /// Gets the title of the GrhData. If the category has not yet been set (Load() has not been 
        /// called on this GrhData), this value will be null. Other than that, this value will never be null.
        /// </summary>
        public string Title
        {
            get { return _title; }
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

        internal GrhData()
        {
            // Restrict construction to internal only
        }

        /// <summary>
        /// Changes the texture for a stationary GrhData.
        /// </summary>
        /// <param name="newTexture">Name of the new texture to use.</param>
        public void ChangeTexture(string newTexture)
        {
            if (string.IsNullOrEmpty(newTexture))
                throw new ArgumentNullException("newTexture");

            // Ensure this is not an animated GrhData
            if (IsAnimated)
                throw new MemberAccessException("Cannot change the texture of an animated GrhData.");

            // Check that it is actually a different texture
            if (TextureName == newTexture)
                return;

            // Apply the new texture
            _texture = null;
            _isUsingAtlas = false;
            _textureName = newTexture;

            ValidateTexture();
        }

        /// <summary>
        /// Creates a duplicate (deep copy) of the GrhData, using the specified GrhIndex. Either one or both the
        /// category and name must be different from the original GrhData, since duplicate names and categories
        /// will raise an exception.
        /// </summary>
        /// <param name="newCategory">Category for the duplicate GrhData</param>
        /// <param name="newTitle">Title for the duplicate GrhData</param>
        /// <returns>Deep copy of the GrhData</returns>
        public GrhData Duplicate(string newCategory, string newTitle)
        {
            ushort index = GrhInfo.NextFreeIndex();

            Debug.Assert(GrhInfo.GetDatas(index) == null, "Slot to use is already in use!");

            GrhData gd = new GrhData
                         {
                             _category = newCategory, _cm = _cm, _frames = _frames, _grhIndex = index, _sourceRect = _sourceRect,
                             _speed = _speed, _texture = _texture, _textureName = _textureName, _title = newTitle,
                             _isUsingAtlas = _isUsingAtlas, _atlasSourceRect = _atlasSourceRect
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
        /// Sets the data for a single GrhData (no animation)
        /// </summary>
        /// <param name="cm">ContentManager used by this texture</param>
        /// <param name="grhIndex">Index of the Grh</param>
        /// <param name="textureName">Path and name of the texture relative to the Content/Grh/ folder</param>
        /// <param name="x">Pixel x coordinate of the source texture</param>
        /// <param name="y">Pixel y coordinate of the source texture</param>
        /// <param name="width">Pixel width of the source texture</param>
        /// <param name="height">Pixel height of the source texture</param>
        /// <param name="category">Period-delimited category</param>
        /// <param name="title">Title of the GrhData. Must be unique for the supplied category.</param>
        public void Load(ContentManager cm, ushort grhIndex, string textureName, int x, int y, int width, int height,
                         string category, string title)
        {
            if (cm == null)
            {
                Debug.Fail("cm is null.");
                if (log.IsFatalEnabled)
                    log.Fatal("Parameter `cm`, ContentManager, is null.");
                throw new ArgumentNullException("cm");
            }

            // We only have one frame, this one
            _frames = new GrhData[1];
            _frames[0] = this;

            // Store some values and references
            _cm = cm;
            _textureName = textureName;
            _grhIndex = grhIndex;
            _sourceRect = new Rectangle(x, y, width, height);

            // Set the categorization
            SetCategorization(category, title);
        }

        /// <summary>
        /// Sets the data for an animated GrhData
        /// </summary>
        /// <param name="grhIndex">Index of the Grh</param>
        /// <param name="frames">Total number of frames</param>
        /// <param name="speed">Array where the indices are the frames and elements are the GrhIndexes</param>
        /// <param name="category">Period-delimited category</param>
        /// <param name="title">Title of the GrhData. Must be unique for the supplied category.</param>
        public void Load(ushort grhIndex, ushort[] frames, float speed, string category, string title)
        {
            // Create the frames
            _frames = new GrhData[frames.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                _frames[i] = GrhInfo.GetDatas(frames[i]);
            }

            // Store some values and references
            _grhIndex = grhIndex;
            _speed = speed;
            if (_speed > 1f)
                _speed = 1f / _speed;

            // Animated GrhDatas don't have a single texture, so there can't be a texture name
            _textureName = null;

            // Set the categorization
            SetCategorization(category, title);
        }

        /// <summary>
        /// Writes the XML information for a single GrhData to a file
        /// </summary>
        /// <param name="w">XmlWriter to write with</param>
        public virtual void Save(XmlWriter w)
        {
            // Check for valid data
            if (GrhIndex <= 0)
                throw new Exception("GrhIndex invalid.");
            if (w == null)
                throw new ArgumentNullException("w");
            if (w.WriteState == WriteState.Error || w.WriteState == WriteState.Closed)
                throw new ArgumentException("XmlWriter's state is invalid.", "w");

            if (string.IsNullOrEmpty(Category))
                throw new NullReferenceException("Category is null or invalid.");
            if (string.IsNullOrEmpty(Title))
                throw new NullReferenceException("Title is null or invalid.");

            // Header
            w.WriteStartElement("Grh");
            w.WriteAttributeString("Index", GrhIndex.ToString());

            // Single frame
            if (Frames == null || Frames.Length == 1)
            {
                if (string.IsNullOrEmpty(TextureName))
                    throw new NullReferenceException("TextureName is null or invalid for a non-animation.");
                if (SourceRect.Width <= 0)
                    throw new Exception("SourceRect.Width must be > 0.");
                if (SourceRect.Height <= 0)
                    throw new Exception("SourceRect.Height must be > 0.");

                w.WriteElementString("AutomaticSize", AutomaticSize.ToString());

                w.WriteStartElement("Frames");
                w.WriteAttributeString("Count", "1");
                w.WriteEndElement();

                w.WriteStartElement("Texture");
                Rectangle r = GetOriginalSource();
                w.WriteElementString("File", TextureName);
                w.WriteElementString("X", r.X.ToString());
                w.WriteElementString("Y", r.Y.ToString());
                w.WriteElementString("W", r.Width.ToString());
                w.WriteElementString("H", r.Height.ToString());
                w.WriteEndElement();
            }
            else
            {
                // Animated
                w.WriteStartElement("Frames");
                w.WriteAttributeString("Count", Frames.Length.ToString());
                for (int i = 0; i < Frames.Length; i++)
                {
                    w.WriteAttributeString("F" + (i + 1), Frames[i].GrhIndex.ToString());
                }
                w.WriteEndElement();

                w.WriteStartElement("Anim");
                w.WriteAttributeString("Speed", (1f / Speed).ToString());
                w.WriteEndElement();
            }

            // Category
            w.WriteStartElement("Category");
            w.WriteElementString("Name", Category);
            w.WriteElementString("Title", Title);
            w.WriteEndElement();

            // End <Grh> element
            w.WriteEndElement();
        }

        /// <summary>
        /// Sets the category and title for the GrhData and raises an OnChangeCategorization event if
        /// either of the values are different from the current
        /// </summary>
        /// <param name="category">New category (may not be null or else it will change to a String.Empty)</param>
        /// <param name="title">New title (may not be null or else it will change to a String.Empty)</param>
        public void SetCategorization(string category, string title)
        {
            Debug.Assert(category != null, "category is null.");
            Debug.Assert(title != null, "title is null.");

            // If given a null value, change it to a String.Empty so we can at least use it still
            if (_category == null)
                _category = string.Empty;
            if (_title == null)
                _title = string.Empty;

            // Check that either of the values are different
            if (_category == category && _title == title)
                return;

            string oldCategory = _category;
            string oldTitle = _title;

            // Set the new values
            _category = category;
            _title = title;

            // Raise the event
            if (OnChangeCategorization != null)
                OnChangeCategorization(this, oldCategory, oldTitle);
        }

        /// <summary>
        /// Returns a System.String that represents the GrhData.
        /// </summary>
        /// <returns>A System.String that represents the GrhData.</returns>
        public override string ToString()
        {
            // Add the frame count if animated, or the texture if not animated
            string extra;
            if (_frames.Length > 1)
                extra = _frames.Length + " frames";
            else
                extra = _textureName;

            // Create the categorization display, and add the extra info
            return string.Format("[{0}] {1}.{2} - {3}", _grhIndex, _category, _title, extra);
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
                    _texture = _cm.Load<Texture2D>("Grh/" + _textureName);
                }
                catch (ContentLoadException ex)
                {
                    const string errmsg = "Failed to load GrhData `{0}.{1}` [{2}] : {3}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, Category, Title, GrhIndex, ex);
                }

                // If we were using an atlas, we'll have to remove it because the texture was reloaded
                _isUsingAtlas = false;
            }
        }

        #region ITextureAtlas Members

        /// <summary>
        /// Gets the texture source rectangle in pixels for a single frame Grh
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
        void ITextureAtlas.SetAtlas(Texture2D texture, Rectangle atlasSourceRect)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");

            // An atlas may only be used on a frame, not an animation
            if (IsAnimated)
            {
                const string errmsg = "An atlas may only be applied on a frame, not an animation!";
                Debug.Fail(errmsg);
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                return;
            }

            // Set the atlas usage values
            _atlasSourceRect = atlasSourceRect;
            _texture = texture;
            _isUsingAtlas = true;
        }

        #endregion
    }
}