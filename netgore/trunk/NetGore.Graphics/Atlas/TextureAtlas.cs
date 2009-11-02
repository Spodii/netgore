using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;

namespace NetGore.Graphics
{
    /// <summary>
    /// Provides the ability to combine any class that supports the ITextureAtlas interface to
    /// be combined into a texture atlas which takes a collection of textures and combines just
    /// the parts of interest into one single texture. This provides the ability to store much more
    /// useful graphic data in memory at a lower cost and, due to being a single texture, increases
    /// performance by allowing larger batching. The TextureAtlas can also draw a 1 pixel border
    /// around all ITextureAtlas items which prevents texture filter issues when drawing.
    /// </summary>
    public class TextureAtlas : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Background color of the atlas (not like it matters).
        /// </summary>
        Color _backColor = new Color(255, 0, 255, 255);

        GraphicsDevice _device;
        bool _hasBeenBuilt = false;
        bool _isDisposed = false;

        /// <summary>
        /// Maximum size of the atlas texture (there is such thing as too big...).
        /// </summary>
        int _maxSize = 2048;

        /// <summary>
        /// Number of pixels to pad around each item in the atlas.
        /// </summary>
        int _padding = 1;

        /// <summary>
        /// Gets the <see cref="ITextureAtlas"/>es that this <see cref="TextureAtlas"/> was built with, or null
        /// if <see cref="HasBeenBuilt"/> is false.
        /// </summary>
        public IEnumerable<ITextureAtlas> AtlasItems
        {
            get
            {
                if (!HasBeenBuilt || _builtAtlasesInfos == null)
                    return null;

                return _builtAtlasesInfos.SelectMany(atlasInfo => atlasInfo.Nodes.Select(node => node.ITextureAtlas));
            }
        }

        /// <summary>
        /// Gets or sets the background color of the atlas texture.
        /// </summary>
        /// <exception cref="MethodAccessException"><see cref="HasBeenBuilt"/> is true.</exception>
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (HasBeenBuilt)
                    throw CreateExceptionHasBeenBuilt();

                _backColor = value;
            }
        }

        /// <summary>
        /// Gets if this <see cref="TextureAtlas"/> has already been built. If true, it cannot be edited or rebuilt,
        /// and any attempt to do so will result in a <see cref="MethodAccessException"/>.
        /// </summary>
        public bool HasBeenBuilt
        {
            get { return _hasBeenBuilt; }
        }

        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets or sets the maximum size of each atlas texture in pixels.
        /// </summary>
        /// <exception cref="MethodAccessException"><see cref="HasBeenBuilt"/> is true.</exception>
        public int MaxSize
        {
            get { return _maxSize; }
            set
            {
                if (HasBeenBuilt)
                    throw CreateExceptionHasBeenBuilt();

                if (!IsPowerOf2(value))
                    throw new Exception("Maximum size must be a power of 2");

                _maxSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the padding in pixels for each item in the atlas
        /// </summary>
        /// <exception cref="MethodAccessException"><see cref="HasBeenBuilt"/> is true.</exception>
        public int Padding
        {
            get { return _padding; }
            set
            {
                if (HasBeenBuilt)
                    throw CreateExceptionHasBeenBuilt();

                _padding = value;
            }
        }

        IEnumerable<TextureAtlasInfo> _builtAtlasesInfos;

        /// <summary>
        /// Builds the texture atlas or atlases for the given atlas items. This must only be called once.
        /// </summary>
        /// <param name="device">GraphicsDevice to build with.</param>
        /// <param name="atlasItems">The <see cref="ITextureAtlas"/>es to build the atlas with.</param>
        /// <returns>List of texture atlases used.</returns>
        /// <exception cref="MethodAccessException"><see cref="HasBeenBuilt"/> is true.</exception>
        /// <exception cref="ArgumentException">The <see cref="device"/> is null or disposed.</exception>
        public List<Texture2D> Build(GraphicsDevice device, IEnumerable<ITextureAtlas> atlasItems)
        {
            if (HasBeenBuilt)
                throw CreateExceptionHasBeenBuilt();

            if (device == null || device.IsDisposed)
            {
                const string errmsg = "device is null or invalid.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "device");
            }

            _hasBeenBuilt = true;

            _builtAtlasesInfos = Combine(device, atlasItems);

            // If the device is lost, we must rebuild it to make the atlas valid again
            _device = device;
            device.DeviceReset += HandleDeviceReset;

            return CreateAtlasTextures(device, _builtAtlasesInfos);
        }

        /// <summary>
        /// Combines a list of ITextureAtlases to an atlas.
        /// </summary>
        /// <param name="device">Device to use for creating the atlas.</param>
        /// <param name="atlasItems">The <see cref="ITextureAtlas"/>es to build the atlas with.</param>
        List<TextureAtlasInfo> Combine(GraphicsDevice device, IEnumerable<ITextureAtlas> atlasItems)
        {
            if (device == null || device.IsDisposed)
            {
                const string errmsg = "device is null or invalid.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "device");
            }

            var atlasInfos = new List<TextureAtlasInfo>();

            // Ensure we even have anything to work with
            if (atlasItems.Count() == 0)
                return atlasInfos;

            // Build the working list and sort it
            var workingList = new List<ITextureAtlas>(atlasItems);
            workingList.Sort(SizeCompare);

            // Loop until the list is empty
            int width = 0;
            int height = 0;
            while (workingList.Count > 0)
            {
                // If sizes are 0, find the starting size
                if (width == 0 || height == 0)
                    GetStartSize(workingList, _maxSize, out width, out height);

                // Try to build the atlas
                bool isMaxSize = (width == _maxSize && height == _maxSize);
                var combinedNodes = Combine(workingList, width, height, !isMaxSize);

                // If all items have been added to the atlas, we're done
                if (combinedNodes.Count == workingList.Count)
                {
                    atlasInfos.Add(new TextureAtlasInfo(combinedNodes, width, height));
                    break;
                }

                // We have remaining items
                if (isMaxSize)
                {
                    // We made it to the maximum size and still have items left - make a new atlas
                    while (combinedNodes.Count > 0)
                    {
                        workingList.Remove(combinedNodes.Pop().ITextureAtlas);
                    }
                    atlasInfos.Add(new TextureAtlasInfo(combinedNodes, width, height));
                    width = 0;
                    height = 0;
                }
                else
                {
                    // Increase the size to the next step and try again
                    GetNextSize(ref width, ref height);
                }
            }

            return atlasInfos;
        }

        /// <summary>
        /// Combines as many ITextureAtlas items as possible into an atlas.
        /// </summary>
        /// <param name="items">Collection of ITextureAtlas items to add to the atlas.</param>
        /// <param name="width">Width of the atlas.</param>
        /// <param name="height">Height of the atlas.</param>
        /// <param name="breakOnAddFail">If true, the method will return instantly after failing to add an item.</param>
        /// <returns>Stack containing all items that were successfully added to the atlas. If the
        /// count of this return value equals the count of the <paramref name="items"/> collection,
        /// all items were successfully added to the atlas of the specified size.</returns>
        Stack<AtlasNode> Combine(ICollection<ITextureAtlas> items, int width, int height, bool breakOnAddFail)
        {
            if (items == null)
            {
                const string errmsg = "items is null.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "items");
            }

            // Create the n stack for all set nodes
            var nodeStack = new Stack<AtlasNode>(items.Count);

            // Set the positions
            AtlasNode root = new AtlasNode(width, height);
            foreach (ITextureAtlas ta in items)
            {
                AtlasNode node = root.Insert(ta.SourceRect.Width + Padding * 2, ta.SourceRect.Height + Padding * 2);
                if (node != null)
                {
                    // Assign the TextureAtlas and push the node onto the stack
                    node.ITextureAtlas = ta;
                    nodeStack.Push(node);
                }
                else if (breakOnAddFail)
                {
                    // Node didn't fit - return
                    return nodeStack;
                }
            }

            return nodeStack;
        }

        /// <summary>
        /// Creates the atlas textures.
        /// </summary>
        /// <param name="device">GraphicsDevice to create the textures on.</param>
        /// <param name="atlasInfos">List of atlas creation information.</param>
        /// <returns>List of atlas textures.</returns>
        List<Texture2D> CreateAtlasTextures(GraphicsDevice device, IEnumerable<TextureAtlasInfo> atlasInfos)
        {
            if (device == null || device.IsDisposed)
            {
                const string errmsg = "device is null or invalid.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "device");
            }

            var ret = new List<Texture2D>();

            // Store the old depth stencil buffer
            DepthStencilBuffer oldDSB = device.DepthStencilBuffer;

            // Draw the atlas and update the altas items to use the atlas information
            foreach (TextureAtlasInfo atlasInfo in atlasInfos)
            {
                Texture2D tex = DrawAtlas(device, atlasInfo);
                ret.Add(tex);
                SetAtlas(tex, atlasInfo);
            }

            // Restore the old DSB
            device.DepthStencilBuffer = oldDSB;

            return ret;
        }

        /// <summary>
        /// Creates a DepthStencilBuffer.
        /// </summary>
        /// <param name="target">RenderTarget to create the DSB for.</param>
        /// <returns>A DSB sized to the RenderTarget.</returns>
        static DepthStencilBuffer CreateDSB(RenderTarget target)
        {
            if (target == null || target.IsDisposed)
            {
                const string errmsg = "target is null or invalid.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "target");
            }

            int w = target.Width;
            int h = target.Height;
            GraphicsDevice device = target.GraphicsDevice;

            if (w > device.DepthStencilBuffer.Width || h > device.DepthStencilBuffer.Height)
            {
                DepthFormat format = target.GraphicsDevice.DepthStencilBuffer.Format;
                MultiSampleType sample = target.MultiSampleType;
                int quality = target.MultiSampleQuality;
                return new DepthStencilBuffer(device, w, h, format, sample, quality);
            }
            else
                return device.DepthStencilBuffer;
        }

        static Exception CreateExceptionHasBeenBuilt()
        {
            return new MethodAccessException("Cannot rebuild or alter a method that has already been built.");
        }

        /// <summary>
        /// Draws a list of AtlasItems onto a texture to make the atlas.
        /// </summary>
        /// <param name="device">Device to use to create the atlas.</param>
        /// <param name="atlasInfo">Describes the atlas to draw.</param>
        Texture2D DrawAtlas(GraphicsDevice device, TextureAtlasInfo atlasInfo)
        {
            if (device == null || device.IsDisposed)
            {
                const string errmsg = "device is null or invalid.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "device");
            }

            if (atlasInfo == null)
            {
                const string errmsg = "atlasInfo is null.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentNullException("atlasInfo");
            }

            return DrawAtlas(device, atlasInfo.Nodes, atlasInfo.Width, atlasInfo.Height);
        }

        /// <summary>
        /// Draws a list of AtlasItems onto a texture to make the atlas.
        /// </summary>
        /// <param name="items">Items to set onto the atlas.</param>
        /// <param name="device">Device to use to create the atlas.</param>
        /// <param name="width">Width of the atlas.</param>
        /// <param name="height">Height of the atlas.</param>
        /// <returns>Texture2D atlas of all the given AtlasItems.</returns>
        Texture2D DrawAtlas(GraphicsDevice device, IEnumerable<AtlasNode> items, int width, int height)
        {
            if (device == null || device.IsDisposed)
            {
                const string errmsg = "device is null or invalid.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "device");
            }

            if (items == null)
            {
                const string errmsg = "items is null.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "items");
            }

            Texture2D ret;
            SurfaceFormat format = device.PresentationParameters.BackBufferFormat;
            MultiSampleType sample = device.PresentationParameters.MultiSampleType;
            int q = device.PresentationParameters.MultiSampleQuality;

            using (RenderTarget2D target = new RenderTarget2D(device, width, height, 1, format, sample, q))
            {
                // Set the render target to the texture and clear it
                device.DepthStencilBuffer = CreateDSB(target);
                device.SetRenderTarget(0, target);
                device.Clear(BackColor);

                using (SpriteBatch sb = new SpriteBatch(device))
                {
                    // Draw every atlas item to the texture
                    sb.BeginUnfiltered(SpriteBlendMode.None, SpriteSortMode.Texture, SaveStateMode.None);
                    foreach (AtlasNode item in items)
                    {
                        // Make sure we are not already using an atlas, otherwise we'll be drawing atlas -> atlas.
                        // In theory, this is a fine idea, and just improves performance of generating the new
                        // atlas. But in reality, we don't want to because we may either get artifacts from the other
                        // atlas or, even more likely, we are building the atlas because we lost the device in the first
                        // place. When we lose the device, the RenderTarget2D becomes forever invalid. So if we just
                        // lost the device and are restoring the atlas, trying to build the atlas again from the
                        // old atlas will just cause an exception.
                        item.ITextureAtlas.RemoveAtlas();

                        // Grab the texture and make sure it is valid
                        Texture2D tex = item.ITextureAtlas.Texture;
                        if (tex == null || tex.IsDisposed)
                        {
                            const string errmsg = "Failed to add item `{0}` to atlas - texture is null or disposed.";
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, item);
                            Debug.Fail(string.Format(errmsg, item));
                            continue;
                        }

                        // Draw the actual image (raw, no borders)
                        Rectangle srcRect = item.ITextureAtlas.SourceRect;
                        Vector2 dest = new Vector2(item.Rect.X + Padding, item.Y + Padding);
                        Rectangle src = srcRect;
                        sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        // Create the borders if padded
                        if (Padding == 0)
                            continue;

                        // Left border
                        src.Width = 1;
                        dest.X -= 1;
                        sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        // Right border
                        src.X += srcRect.Width - 1;
                        dest.X += srcRect.Width + 1;
                        sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        // Top border
                        src = new Rectangle(srcRect.X, srcRect.Y, srcRect.Width, 1);
                        dest.X = item.X + Padding;
                        dest.Y = item.Y + Padding - 1;
                        sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        // Bottom border
                        src.Y += srcRect.Height - 1;
                        dest.Y += srcRect.Height + 1;
                        sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        // Top-left corner
                        src = new Rectangle(srcRect.X, srcRect.Y, 1, 1);
                        dest.X = item.X + Padding - 1;
                        dest.Y = item.Y + Padding - 1;
                        sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        // Top-right corner
                        src.X += srcRect.Width - 1;
                        dest.X += srcRect.Width + 1;
                        sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        // Bottom-right corner
                        src.Y += srcRect.Height - 1;
                        dest.Y += srcRect.Height + 1;
                        sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        // Bottom-left corner
                        src.X -= srcRect.Width - 1;
                        dest.X -= srcRect.Width + 1;
                        sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    sb.End();
                }

                // Restore the render target and grab the created texture
                device.SetRenderTarget(0, null);
                ret = target.GetTexture();

                // Create a dummy array that will be used to grab some data from the return texture
                // This will force the texture to be read from, which for some reason is actually required to be
                // done for some systems... not sure why...
                int[] dummyIntArray = new int[1];
                ret.GetData(0, new Rectangle(0, 0, 1, 1), dummyIntArray, 0, 1);
            }

            ret.Name = "Texture Atlas";
            return ret;
        }

        /// <summary>
        /// Increases to the next texture size.
        /// </summary>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        static void GetNextSize(ref int width, ref int height)
        {
            if (height < width)
                height *= 2;
            else
                width *= 2;
        }

        /// <summary>
        /// Heuristic (aka shitty) guesses for what might be a good starting size.
        /// </summary>
        void GetStartSize(IEnumerable<ITextureAtlas> items, int maxSize, out int width, out int height)
        {
            if (items == null)
            {
                const string errmsg = "items is null.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "items");
            }

            int maxWidth = 0;
            int maxHeight = 0;
            int totalSize = 0;

            // Add the area of every AtlasItem along with find the largest width
            foreach (ITextureAtlas item in items)
            {
                maxWidth = Math.Max(maxWidth, item.SourceRect.Width + _padding * 2);
                maxHeight = Math.Max(maxHeight, item.SourceRect.Height + _padding * 2);
                totalSize += item.SourceRect.Width * item.SourceRect.Height + _padding * 2;
            }

            // Get the guessed size to use
            int guessedSize = (int)Math.Sqrt(totalSize);

            // Check that the maxSize is able to fit the textures
            if (maxWidth > maxSize || maxHeight > maxSize)
                throw new Exception("One or more ITextureAtlases can not fit into the atlas with the given maximum texture size");

            // Use the higher of the two values and round to the next power of 2
            width = NextPowerOf2(Math.Max(guessedSize, maxWidth));
            height = NextPowerOf2(Math.Max(guessedSize, maxHeight));

            // If possible, divide one of the sizes in half
            if (width / 2 > maxWidth)
                width /= 2;
            else if (height / 2 > maxHeight)
                height /= 2;

            // Finally, force below the maximum size
            width = Math.Min(width, maxSize);
            height = Math.Min(height, maxSize);
        }

        /// <summary>
        /// Handles when the <see cref="GraphicsDevice"/> is reset.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void HandleDeviceReset(object sender, EventArgs e)
        {
            foreach (var item in AtlasItems)
                item.RemoveAtlas();

            // TODO: CreateAtlasTextures(_device, _builtAtlasesInfos);
            // The above line WILL rebuild the atlases, but it doesn't want to work properly.
            // After a few resets and the program will crash again due to accessing invalid memory.
            // Should check the GrhDatas to see if they are being set back up properly when removing the atlas.
        }

        /// <summary>
        /// Checks if a value is a power of two.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if a power of 2, else false.</returns>
        static bool IsPowerOf2(int value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Finds the next highest power of 2 for a given value unless the value is already a power of 2.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Next highest power of 2 of the value.</returns>
        static int NextPowerOf2(int value)
        {
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return value + 1;
        }

        /// <summary>
        /// Sets the <see cref="ITextureAtlas"/>es of an atlas info to use a certain texture atlas.
        /// </summary>
        /// <param name="tex">Texture atlas for the <see cref="ITextureAtlas"/>es to use.</param>
        /// <param name="atlasInfo">Information describing the atlas.</param>
        void SetAtlas(Texture2D tex, TextureAtlasInfo atlasInfo)
        {
            if (tex == null || tex.IsDisposed)
            {
                const string errmsg = "device is null or invalid.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "tex");
            }

            if (atlasInfo == null)
            {
                const string errmsg = "atlasInfo is null.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentNullException("atlasInfo");
            }

            foreach (AtlasNode n in atlasInfo.Nodes)
            {
                Rectangle r = new Rectangle(n.X + Padding, n.Y + Padding, n.Width - Padding * 2, n.Height - Padding * 2);
                n.ITextureAtlas.SetAtlas(tex, r);
            }
        }

        /// <summary>
        /// Comparison function for sorting sprites by size.
        /// </summary>
        /// <returns>A signed number indicating the relative values of this instance and value.
        /// Less than zero means this instance is less than value. Zero means this instance is equal to value. 
        /// Greater than zero mean this instance is greater than value.</returns>
        static int SizeCompare(ITextureAtlas a, ITextureAtlas b)
        {
            if (a == null)
            {
                if (log.IsFatalEnabled)
                    log.Fatal("argument a is null.");
                throw new ArgumentNullException("a");
            }

            if (b == null)
            {
                if (log.IsFatalEnabled)
                    log.Fatal("argument b is null.");
                throw new ArgumentNullException("b");
            }

            int aSize = a.SourceRect.Height * a.SourceRect.Width;
            int bSize = b.SourceRect.Height * b.SourceRect.Width;
            return bSize.CompareTo(aSize);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            // Remove our event hook so we never rebuild this atlas when the device is lost
            if (HasBeenBuilt && _device != null && !_device.IsDisposed)
                _device.DeviceReset -= HandleDeviceReset;
        }

        #endregion
    }
}