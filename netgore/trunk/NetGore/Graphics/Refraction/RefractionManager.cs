using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics
{
    /// <summary>
    /// Manages multiple <see cref="IRefractionEffect"/>s and the generation of the refraction map.
    /// </summary>
    public class RefractionManager : OffscreenRenderBufferManagerBase, IRefractionManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The default refraction map requires be cleared with the RGB channels at 127 since that is the color used
        /// to indicate to the shader that no refraction.
        /// </summary>
        static readonly Color _defaultClearColor = new Color(127, 127, 127, 0);

        static readonly Shader _defaultShader;

        readonly List<IRefractionEffect> _list = new List<IRefractionEffect>();
        Texture _colorMap;

        bool _isEnabled;

        /// <summary>
        /// Initializes the <see cref="RefractionManager"/> class.
        /// </summary>
        static RefractionManager()
        {
            // Check if shaders are supported
            if (!Shader.IsAvailable)
            {
                const string msg =
                    "Unable to construct shader for `RefractionManager` - shaders are not supported on this system.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(msg);
                return;
            }

            const string defaultShaderCode =
                @"
/*
	This effect uses the R, G, and A channels to perform a reflection distortion. Color channels are used in the following ways:
		R: The amount to translate on the X axis (< BaseOffset for right, > BaseOffset for left).
		G: The amount to translate on the Y axis (< BaseOffset for down, > BaseOffset for up).
		A: The alpha value of the reflected image. 0.0 shows nothing, while 1.0 shows only the reflection.
*/

// The distance multiplier to apply to the values unpacked from channels to get the offset. This decreases our resolution,
// giving us a choppier image, but increases our range. Lower values give higher resolution but require smaller distances.
// This MUST be the same in all the refraction effects!
const float DistanceMultiplier = 2.0;

// The value of the reflection channels that will be used to not perform any reflection. Having this non-zero allows us to
// reflect in both directions instead of just borrowing pixels in one direction. Of course, this also halves our max distance.
// Logically, this value is 1/2. However, a slightly different number is used due to the behavior of floating-point numbers.
// This MUST be the same in all the refraction effects!
const float BaseOffset = 0.4981;

// The texture that contains the colors to use. Typically, a copy of the screen to be distorted.
uniform sampler2D ColorMap;

// The texture containing the noise that will be used to distort the ColorMap.
uniform sampler2D NoiseMap;

void main (void)
{
	vec4 noiseVec;
	vec4 colorReflected;
	vec4 colorOriginal;

	// Get the noise vector from the noise map.
	noiseVec = texture2D(NoiseMap, gl_TexCoord[0].st).rgba;

	// Get the original texel color, which is just the texel at the ColorMap at the same position as the NoiseMap.
	colorOriginal = texture2D(ColorMap, gl_TexCoord[0].st);

	// Using the noise vector to offset the position, find the corresponding reflected color on the color map.
	colorReflected = texture2D(ColorMap, gl_TexCoord[0].st + ((noiseVec.xy - BaseOffset) * DistanceMultiplier));

	// Mix the reflected and original texels together by the alpha of the noise vector to get the final pixel color.
	gl_FragColor = mix(colorOriginal, colorReflected, noiseVec.a);
}";
            // Try to create the default shader
            try
            {
                _defaultShader = ShaderExtensions.LoadFromMemory(defaultShaderCode);
            }
            catch (LoadingFailedException ex)
            {
                const string errmsg = "Failed to load the default Shader for the RefractionManager. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefractionManager"/> class.
        /// </summary>
        public RefractionManager()
        {
            DrawToTargetShader = DefaultShader;
            BufferClearColor = _defaultClearColor;

            // Try to enable
            IsEnabled = true;
        }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to use when clearing the buffer.
        /// </summary>
        public Color ClearColor
        {
            get { return BufferClearColor; }
            set { BufferClearColor = value; }
        }

        /// <summary>
        /// Gets the default <see cref="Shader"/> used by the <see cref="RefractionManager"/> for drawing the refraction map.
        /// </summary>
        public static Shader DefaultShader
        {
            get { return _defaultShader; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the buffer.
        /// </summary>
        /// <param name="rt">The <see cref="RenderTarget"/> to draw to.</param>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw to the <paramref name="rt"/>. The derived class
        /// is required to handle making Begin()/End() calls on it.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> to use when drawing.</param>
        /// <returns>
        /// True if the drawing was successful; false if there were any errors while drawing.
        /// </returns>
        protected override bool HandleDrawBuffer(RenderTarget rt, ISpriteBatch sb, ICamera2D camera)
        {
            sb.Begin(BlendMode.Add, camera);

            try
            {
                // Sort through the effects, grabbing those in view, then ordering by their drawing priority
                foreach (var effect in this.Where(camera.InView).OrderBy(x => x.DrawPriority))
                {
                    try
                    {
                        // Draw the effect
                        effect.Draw(sb);
                    }
                    catch (Exception ex)
                    {
                        // A single effect failed
                        const string errmsg =
                            "Error while drawing IRefractionEffect `{0}` for RefractionManager `{1}`. Exception: {2}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, effect, this, ex);
                        Debug.Fail(string.Format(errmsg, effect, this, ex));
                    }
                }
            }
            catch (Exception ex)
            {
                // Failed somewhere other than effect.Draw(), causing the whole drawing to fail
                const string errmsg = "Error while drawing IRefractionEffects for RefractionManager `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
                Debug.Fail(string.Format(errmsg, this, ex));
                return false;
            }
            finally
            {
                sb.End();
            }

            return true;
        }

        /// <summary>
        /// Handles the actual drawing of the buffer to a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="buffer">The <see cref="Texture"/> of the buffer that is to be drawn to the <paramref name="target"/>.</param>
        /// <param name="sprite">The <see cref="SFML.Graphics.Sprite"/> set up to draw the <paramref name="buffer"/>.</param>
        /// <param name="target">The <see cref="RenderTarget"/> to draw the <paramref name="buffer"/> to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that was used during the creation of the buffer.</param>
        protected override void HandleDrawBufferToTarget(Texture buffer, SFML.Graphics.Sprite sprite, RenderTarget target, ICamera2D camera, RenderStates renderStates)
        {
            renderStates.BlendMode = BlendMode.None;

            // Set up the shader
            DrawToTargetShader.SetParameter("ColorMap", _colorMap);
            DrawToTargetShader.SetParameter("NoiseMap", buffer);

            base.HandleDrawBufferToTarget(buffer, sprite, target, camera, renderStates);
        }

        #region IRefractionManager Members

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the
        /// <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/>
        /// is read-only.
        /// </exception>
        public IRefractionEffect this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Gets or sets if this <see cref="IRefractionManager"/> is enabled.
        /// If <see cref="SFML.Graphics.Shader.IsAvailable"/> is false, this will always be false.
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value)
                    return;

                // When going from disabled to enabled, make sure that shaders are supported
                if (value && !Shader.IsAvailable)
                {
                    const string errmsg = "Cannot enable IRefractionMap since Shader.IsAvailable returned false.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg);
                    return;
                }

                _isEnabled = value;
            }
        }

        /// <summary>
        /// Gets if the <see cref="IRefractionManager"/> has been initialized.
        /// </summary>
        public bool IsInitialized
        {
            get { return IsBufferInitialized; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<IRefractionEffect>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Add(IRefractionEffect item)
        {
            _list.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(IRefractionEffect item)
        {
            return _list.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>,
        /// starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.
        /// -or-
        /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the
        /// available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// -or-
        /// Type <see cref="IRefractionEffect"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(IRefractionEffect[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Draws all of the reflection effects in this <see cref="IRefractionManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <returns>
        /// The <see cref="Texture"/> containing the reflection map. If the reflection map failed to be generated
        /// for whatever reason, a null value will be returned instead.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="IRefractionManager.IsInitialized"/> is false.</exception>
        public virtual Texture Draw(ICamera2D camera)
        {
            return GetBuffer(camera);
        }

        /// <summary>
        /// Draws the refraction map to a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <param name="target">The <see cref="RenderTarget"/> to draw the refraction map to.</param>
        /// <param name="colorMap">The <see cref="Texture"/> to get the colors from. Typically, this is an <see cref="Texture"/> of
        /// the fully drawn game scene to apply refractions to.</param>
        public virtual void DrawToTarget(ICamera2D camera, RenderTarget target, Texture colorMap)
        {
            _colorMap = colorMap;
            DrawBufferToTarget(target, camera);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IRefractionEffect> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(IRefractionEffect item)
        {
            return _list.IndexOf(item);
        }

        /// <summary>
        /// Initializes the <see cref="IRefractionManager"/> so it can be drawn. This must be called before any drawing
        /// can take place, but does not need to be drawn before <see cref="IRefractionEffect"/> are added to or removed
        /// from the collection.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> is null.</exception>
        public virtual void Initialize(Window window)
        {
            InitializeRenderBuffer(window);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the
        /// <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        /// </exception>
        public void Insert(int index, IRefractionEffect item)
        {
            _list.Insert(index, item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>;
        /// otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public bool Remove(IRefractionEffect item)
        {
            return _list.Remove(item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in
        /// the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        /// </exception>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <summary>
        /// Updates the <see cref="IDrawingManager"/> and all components inside of it.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public virtual void Update(TickCount currentTime)
        {
            foreach (var fx in this)
            {
                fx.Update(currentTime);
            }
        }

        #endregion
    }
}