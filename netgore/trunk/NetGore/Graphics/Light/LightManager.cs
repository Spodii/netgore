using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics
{
    public class LightManager : OffscreenRenderBufferManagerBase, ILightManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly List<ILight> _list = new List<ILight>();

        Grh _defaultSprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightManager"/> class.
        /// </summary>
        public LightManager()
        {
            Ambient = Color.White;

            // Try to enable
            IsEnabled = true;
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the buffer.
        /// </summary>
        /// <param name="rt">The <see cref="RenderTarget"/> to draw to.</param>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw to the <paramref name="rt"/>. The derived class
        /// is required to handle making Begin()/End() calls on it.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> to use when drawing.</param>
        /// <returns>True if the drawing was successful; false if there were any errors while drawing.</returns>
        protected override bool HandleDrawBuffer(RenderTarget rt, ISpriteBatch sb, ICamera2D camera)
        {
            // Draw the lights
            sb.Begin(BlendMode.Add, camera);

            foreach (var light in this)
            {
                if (camera.InView(light))
                    light.Draw(sb);
            }

            sb.End();

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
            renderStates.BlendMode = BlendMode.Multiply;
            base.HandleDrawBufferToTarget(buffer, sprite, target, camera, renderStates);
        }

        #region ILightManager Members

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
        public ILight this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        /// <summary>
        /// Gets or sets the ambient light color. The alpha value has no affect and will always be set to 255.
        /// </summary>
        public Color Ambient
        {
            get { return BufferClearColor; }
            set { BufferClearColor = new Color(value, byte.MaxValue); }
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
        /// Gets or sets the default sprite to use for all lights added to this <see cref="ILightManager"/>.
        /// This only applies to new lights added to this <see cref="ILightManager"/> where no sprite is
        /// specified. Existing lights are never affected by this property.
        /// </summary>
        public Grh DefaultSprite
        {
            get { return _defaultSprite; }
            set
            {
                if (_defaultSprite == value)
                    return;

                _defaultSprite = value;
            }
        }

        /// <summary>
        /// Gets or sets if this <see cref="ILightManager"/> is enabled.
        /// If <see cref="SFML.Graphics.Shader.IsAvailable"/> is false, this will always be false.
        /// </summary>
        public bool IsEnabled { get; set; }

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
        bool ICollection<ILight>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Add(ILight item)
        {
            if (item.Sprite == null)
            {
                if (DefaultSprite != null)
                {
                    // Use a copy of the DefaultSprite
                    item.Sprite = DefaultSprite.DeepCopy();
                }
                else
                {
                    // No sprite was given, and no DefaultSprite to be used
                    const string errmsg =
                        "Added light `{0}` to `{1}` with no sprite, but couldn't use LightManager.DefaultSprite" +
                        " because the property is null.";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, item, this);
                    Debug.Fail(string.Format(errmsg, item, this));
                }
            }

            if (!_list.Contains(item))
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
        public bool Contains(ILight item)
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
        /// Type <see cref="ILight"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(ILight[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Draws all of the lights in this <see cref="ILightManager"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <returns>
        /// The <see cref="Texture"/> containing the light map. If the light map failed to be generated
        /// for whatever reason, a null value will be returned instead.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="ILightManager.IsInitialized"/> is false.</exception>
        public Texture Draw(ICamera2D camera)
        {
            return GetBuffer(camera);
        }

        /// <summary>
        /// Draws the light map to a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="camera">The camera describing the current view.</param>
        /// <param name="target">The <see cref="RenderTarget"/> to draw the light map to.</param>
        public void DrawToTarget(ICamera2D camera, RenderTarget target)
        {
            DrawBufferToTarget(target, camera);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ILight> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
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
        public int IndexOf(ILight item)
        {
            return _list.IndexOf(item);
        }

        /// <summary>
        /// Initializes the <see cref="ILightManager"/> so it can be drawn. This must be called before any drawing
        /// can take place, but does not need to be drawn before <see cref="ILight"/> are added to or removed
        /// from the collection.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        public void Initialize(Window window)
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
        public void Insert(int index, ILight item)
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
        public bool Remove(ILight item)
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
        /// Updates all of the lights in this <see cref="ILightManager"/>, along with the <see cref="ILightManager"/> itself.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public void Update(TickCount currentTime)
        {
            // Update the default sprite
            var ds = DefaultSprite;
            if (ds != null)
                ds.Update(currentTime);

            // Update the individual lights
            foreach (var light in this)
            {
                light.Update(currentTime);
            }
        }

        #endregion
    }
}