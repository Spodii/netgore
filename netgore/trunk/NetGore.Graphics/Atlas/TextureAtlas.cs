using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// An texture atlas that contains multiple smaller textures combined into one or more larger textures.
    /// </summary>
    public class TextureAtlas : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The number of pixels to pad around each item inserted into the atlas.
        /// </summary>
        public const int Padding = 1;

        /// <summary>
        /// If the found texture size is less than this value, it will be assumed that the found value is wrong and
        /// this value will instead be used.
        /// </summary>
        const int _minAllowedTextureSize = 512;

        /// <summary>
        /// Background color of the atlas (not like it matters much as we should never see it unless there is
        /// a graphics issue or someone uses the wrong part of the atlas).
        /// </summary>
        static readonly Color _backColor = new Color(255, 0, 255, 255);

        /// <summary>
        /// Contains a cache of the maximum allowed texture size. If this value is equal to int.MinValue, it has yet
        /// to be found.
        /// </summary>
        static int _maxTextureSize = int.MinValue;

        readonly List<AtlasTextureInfo> _atlasTextureInfos;

        readonly GraphicsDevice _device;

        bool _isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAtlas"/> class.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        /// <param name="atlasItems">The collection of items to place into the atlas.</param>
        public TextureAtlas(GraphicsDevice device, IEnumerable<ITextureAtlasable> atlasItems)
        {
            if (device == null || device.IsDisposed)
            {
                const string errmsg = "device is null or invalid.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "device");
            }

            if (atlasItems == null || atlasItems.IsEmpty())
            {
                const string errmsg = "atlasItems is null or empty.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "atlasItems");
            }

            _device = device;
            UpdateMaxTextureSize(_device);

            // Create an event listener that will allow us to rebuild and reapply the atlas textures
            // whenever the device is lost
            _device.DeviceReset += HandleDeviceLost;

            // Build the layout for all the items that will be in the atlas
            _atlasTextureInfos = Combine(atlasItems);

            // Build and apply each atlas texture
            foreach (var atlasTextureInfo in _atlasTextureInfos)
            {
                atlasTextureInfo.BuildTexture(_device, Padding);
            }
        }

        /// <summary>
        /// Gets the items that this atlas is built out of.
        /// </summary>
        public IEnumerable<ITextureAtlasable> AtlasItems
        {
            get { return _atlasTextureInfos.SelectMany(x => x.Nodes).Select(x => x.ITextureAtlasable); }
        }

        /// <summary>
        /// Gets the <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> used by this
        /// <see cref="TextureAtlas"/>.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return _device; }
        }

        /// <summary>
        /// Gets if this <see cref="TextureAtlas"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets the cache of the maximum allowed texture size. If this value is equal to int.MinValue, it has yet
        /// to be found.
        /// </summary>
        static int MaxTextureSize
        {
            get
            {
                Debug.Assert(_maxTextureSize != int.MinValue, "MaxTextureSize was requested before it was calculated!");
                return _maxTextureSize;
            }
        }

        /// <summary>
        /// Combines the <see cref="ITextureAtlasable"/> items into one or more <see cref="AtlasTextureInfo"/>s. This
        /// is where all the arrangement magic takes place.
        /// </summary>
        /// <param name="atlasItems">The <see cref="ITextureAtlasable"/>es to build the atlas with.</param>
        static List<AtlasTextureInfo> Combine(IEnumerable<ITextureAtlasable> atlasItems)
        {
            var atlasInfos = new List<AtlasTextureInfo>();

            // Ensure we even have anything to work with
            if (atlasItems.Count() == 0)
                return atlasInfos;

            // Build the working list and sort it
            var workingList = new List<ITextureAtlasable>(atlasItems);
            workingList.Sort(SizeCompare);

            // Loop until the list is empty
            int width = 0;
            int height = 0;
            while (workingList.Count > 0)
            {
                // If sizes are 0, find the starting size
                if (width == 0 || height == 0)
                    GetStartSize(workingList, MaxTextureSize, out width, out height);

                // Try to build the atlas
                bool isMaxSize = (width == MaxTextureSize && height == MaxTextureSize);
                var combinedNodes = CombineSingleTexture(workingList, width, height, !isMaxSize);

                // If all items have been added to the atlas, we're done
                if (combinedNodes.Count == workingList.Count)
                {
                    atlasInfos.Add(new AtlasTextureInfo(width, height, combinedNodes));
                    break;
                }

                // We have remaining items
                if (isMaxSize)
                {
                    // We made it to the maximum size and still have items left - make a new atlas
                    while (combinedNodes.Count > 0)
                    {
                        workingList.Remove(combinedNodes.Pop().ITextureAtlasable);
                    }
                    atlasInfos.Add(new AtlasTextureInfo(width, height, combinedNodes));
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
        /// Combines as many ITextureAtlasable items as possible into an atlas.
        /// </summary>
        /// <param name="items">Collection of ITextureAtlasable items to add to the atlas.</param>
        /// <param name="width">Width of the atlas.</param>
        /// <param name="height">Height of the atlas.</param>
        /// <param name="breakOnAddFail">If true, the method will return instantly after failing to add an item.</param>
        /// <returns>Stack containing all items that were successfully added to the atlas. If the
        /// count of this return value equals the count of the <paramref name="items"/> collection,
        /// all items were successfully added to the atlas of the specified size.</returns>
        static Stack<AtlasTextureItem> CombineSingleTexture(ICollection<ITextureAtlasable> items, int width, int height,
                                                            bool breakOnAddFail)
        {
            if (items == null)
            {
                const string errmsg = "items is null.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "items");
            }

            // Create the n stack for all set nodes
            var nodeStack = new Stack<AtlasTextureItem>(items.Count);

            // Set the positions
            var root = new AtlasTreeNode(new AtlasTextureItem(width, height));
            foreach (var ta in items)
            {
                var node = root.Insert(ta.SourceRect.Width + Padding * 2, ta.SourceRect.Height + Padding * 2, ta);
                if (node != null)
                {
                    // Assign the TextureAtlas and push the node onto the stack
                    nodeStack.Push(node.AtlasNode);
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
        /// Heuristic guesses for what might be a good starting size.
        /// </summary>
        static void GetStartSize(IEnumerable<ITextureAtlasable> items, int maxSize, out int width, out int height)
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
            foreach (ITextureAtlasable item in items)
            {
                maxWidth = Math.Max(maxWidth, item.SourceRect.Width + Padding * 2);
                maxHeight = Math.Max(maxHeight, item.SourceRect.Height + Padding * 2);
                totalSize += item.SourceRect.Width * item.SourceRect.Height + Padding * 2;
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
        void HandleDeviceLost(object sender, EventArgs e)
        {
            foreach (var atlasTextureInfo in _atlasTextureInfos)
            {
                atlasTextureInfo.BuildTexture(_device, Padding);
            }
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
        /// Comparison function for sorting sprites by size.
        /// </summary>
        /// <returns>A signed number indicating the relative values of this instance and value.
        /// Less than zero means this instance is less than value. Zero means this instance is equal to value. 
        /// Greater than zero mean this instance is greater than value.</returns>
        static int SizeCompare(ITextureAtlasable a, ITextureAtlasable b)
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

        /// <summary>
        /// Ensures that we have found the maximum allowed texture size.
        /// </summary>
        /// <param name="device">The <see cref="GraphicsDevice"/>.</param>
        static void UpdateMaxTextureSize(GraphicsDevice device)
        {
            if (_maxTextureSize != int.MinValue)
                return;

            var gdCaps = device.GraphicsDeviceCapabilities;
            _maxTextureSize = Math.Min(gdCaps.MaxTextureWidth, gdCaps.MaxTextureHeight);

            if (_maxTextureSize < _minAllowedTextureSize)
            {
                const string errmsg = "Found maximum texture size was `{0}`, which seems way too low. Forcing to 512.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _maxTextureSize);

                _maxTextureSize = _minAllowedTextureSize;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            _isDisposed = true;

            GraphicsDevice.DeviceReset -= HandleDeviceLost;

            if (_atlasTextureInfos != null)
            {
                foreach (var item in _atlasTextureInfos)
                {
                    item.Dispose();
                }
            }
        }

        #endregion

        /// <summary>
        /// The texture for a single atlas texture. There will be one or more of these per <see cref="TextureAtlas"/>.
        /// </summary>
        class AtlasTextureInfo : IDisposable
        {
            /// <summary>
            /// If true, generated atlas textures will be saved to the temp files. Intended to be used for when tweaking
            /// the atlas generation code. Only possible in debug builds.
            /// </summary>
            const bool _saveGeneratedAtlasToTemp = false;

            readonly int _height;
            readonly IEnumerable<AtlasTextureItem> _nodes;
            readonly int _width;
            Texture2D _atlasTexture;

            /// <summary>
            /// Initializes a new instance of the <see cref="AtlasTextureInfo"/> class.
            /// </summary>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            /// <param name="nodes">The nodes.</param>
            public AtlasTextureInfo(int width, int height, IEnumerable<AtlasTextureItem> nodes)
            {
                _width = width;
                _height = height;

                // Since we won't be altering the collection, use the smallest footprint possible
                _nodes = nodes.Where(x => x.ITextureAtlasable != null).ToCompact();
            }

            /// <summary>
            /// Gets the <see cref="AtlasTextureItem"/>s that are in this atlas texture.
            /// </summary>
            public IEnumerable<AtlasTextureItem> Nodes
            {
                get { return _nodes; }
            }

            /// <summary>
            /// Builds the texture for this atlas item, or rebuilds the texture if one already existed.
            /// </summary>
            internal void BuildTexture(GraphicsDevice device, int padding)
            {
                // Dispose of the old texture if there is one
                if (_atlasTexture != null)
                    _atlasTexture.Dispose();

                // Store the old depth stencil buffer
                DepthStencilBuffer oldDSB = device.DepthStencilBuffer;

                // Draw the atlas (rebuilding the texture)
                _atlasTexture = DrawAtlas(device, padding);

                // Restore the old DSB
                device.DepthStencilBuffer = oldDSB;

                // Tell all the items to use the new atlas texture
                ReapplyAtlasTexture();
            }

            /// <summary>
            /// Draws the <see cref="Texture2D"/> for this atlas.
            /// </summary>
            /// <param name="device">Device to use to create the atlas.</param>
            /// <param name="padding">The amount to pad each item.</param>
            /// <returns>A <see cref="Texture2D"/> of the atlas.</returns>
            Texture2D DrawAtlas(GraphicsDevice device, int padding)
            {
                if (device == null || device.IsDisposed)
                {
                    const string errmsg = "device is null or invalid.";
                    if (log.IsFatalEnabled)
                        log.Fatal(errmsg);
                    throw new ArgumentException(errmsg, "device");
                }

                Texture2D ret;
                SurfaceFormat format = device.PresentationParameters.BackBufferFormat;
                MultiSampleType sample = device.PresentationParameters.MultiSampleType;
                int q = device.PresentationParameters.MultiSampleQuality;

                using (
                    RenderTarget2D target = new RenderTarget2D(device, _width, _height, 1, format, sample, q,
                                                               RenderTargetUsage.PreserveContents))
                {
                    // Set the render target to the texture and clear it
                    device.DepthStencilBuffer = null;
                    device.SetRenderTarget(0, target);
                    device.Clear(ClearOptions.Target, _backColor, 1.0f, 0);

                    using (SpriteBatch sb = new SpriteBatch(device))
                    {
                        // Draw every atlas item to the texture
                        sb.BeginUnfiltered(SpriteBlendMode.None, SpriteSortMode.Texture, SaveStateMode.SaveState);
                        foreach (AtlasTextureItem item in Nodes)
                        {
                            // Grab the texture and make sure it is valid
                            Texture2D tex = item.ITextureAtlasable.Texture;
                            if (tex == null || tex.IsDisposed)
                            {
                                // HACK: Even though we skip invalid textures, we still end up telling the item to use the atlas after it is made, which is obviously no good
                                const string errmsg = "Failed to add item `{0}` to atlas - texture is null or disposed.";
                                if (log.IsErrorEnabled)
                                    log.ErrorFormat(errmsg, item);
                                Debug.Fail(string.Format(errmsg, item));
                                continue;
                            }

                            // Draw the actual image (raw, no borders)
                            Rectangle srcRect = item.ITextureAtlasable.SourceRect;
                            Vector2 dest = new Vector2(item.Rect.X + padding, item.Y + padding);
                            Rectangle src = srcRect;
                            sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                            // Create the borders if padded
                            if (padding == 0)
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
                            dest.X = item.X + padding;
                            dest.Y = item.Y + padding - 1;
                            sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                            // Bottom border
                            src.Y += srcRect.Height - 1;
                            dest.Y += srcRect.Height + 1;
                            sb.Draw(tex, dest, src, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                            // Top-left corner
                            src = new Rectangle(srcRect.X, srcRect.Y, 1, 1);
                            dest.X = item.X + padding - 1;
                            dest.Y = item.Y + padding - 1;
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
                }

                // Save the generated atlas
#pragma warning disable 162
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (_saveGeneratedAtlasToTemp)
                    SaveTextureToTempFile(ret);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
#pragma warning restore 162

                ret.Name = "Atlas Texture";
                return ret;
            }

            /// <summary>
            /// Applies this atlas texture to all of the <see cref="ITextureAtlasable"/> items that the atlas texture
            /// contains.
            /// </summary>
            void ReapplyAtlasTexture()
            {
                foreach (AtlasTextureItem n in Nodes)
                {
                    Rectangle r = new Rectangle(n.X + Padding, n.Y + Padding, n.Width - Padding * 2, n.Height - Padding * 2);
                    n.ITextureAtlasable.SetAtlas(_atlasTexture, r);
                }
            }

            /// <summary>
            /// Saves the texture to a temp file.
            /// </summary>
            [Conditional("DEBUG")]
            static void SaveTextureToTempFile(Texture texture)
            {
                TempFile f = new TempFile();
                texture.Save(f.FilePath, ImageFileFormat.Bmp);
            }

            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_atlasTexture != null && !_atlasTexture.IsDisposed)
                    _atlasTexture.Dispose();
            }

            #endregion
        }
    }
}