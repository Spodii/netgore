using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using SFML;
using SFML.Graphics;

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
        static int _maxTextureSize = 1024;

        readonly List<AtlasTextureInfo> _atlasTextureInfos;

        bool _isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAtlas"/> class.
        /// </summary>
        /// <param name="atlasItems">The collection of items to place into the atlas.</param>
        public TextureAtlas(IEnumerable<ITextureAtlasable> atlasItems)
        {
            if (atlasItems == null || atlasItems.IsEmpty())
            {
                const string errmsg = "atlasItems is null or empty.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new ArgumentException(errmsg, "atlasItems");
            }

            UpdateMaxTextureSize();

            // Build the layout for all the items that will be in the atlas
            _atlasTextureInfos = Combine(atlasItems);

            // Build and apply each atlas texture
            foreach (var atlasTextureInfo in _atlasTextureInfos)
            {
                atlasTextureInfo.BuildTexture(Padding);
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
            var width = 0;
            var height = 0;
            while (workingList.Count > 0)
            {
                // If sizes are 0, find the starting size
                if (width == 0 || height == 0)
                    GetStartSize(workingList, MaxTextureSize, out width, out height);

                // Try to build the atlas
                var isMaxSize = (width == MaxTextureSize && height == MaxTextureSize);
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

            // Create the node stack for all set nodes
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

            var maxWidth = 0;
            var maxHeight = 0;
            var totalSize = 0;

            // Add the area of every AtlasItem along with find the largest width
            foreach (var item in items)
            {
                maxWidth = Math.Max(maxWidth, item.SourceRect.Width + Padding * 2);
                maxHeight = Math.Max(maxHeight, item.SourceRect.Height + Padding * 2);
                totalSize += item.SourceRect.Width * item.SourceRect.Height + Padding * 2;
            }

            // Get the guessed size to use
            var guessedSize = (int)Math.Sqrt(totalSize);

            // Check that the maxSize is able to fit the textures
            if (maxWidth > maxSize || maxHeight > maxSize)
            {
                const string errmsg = "One or more ITextureAtlases can not fit into the atlas with the given maximum texture size";
                throw new ArgumentException(errmsg, "items");
            }

            // Use the higher of the two values and round to the next power of 2
            width = BitOps.NextPowerOf2(Math.Max(guessedSize, maxWidth));
            height = BitOps.NextPowerOf2(Math.Max(guessedSize, maxHeight));

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

            var aSize = a.SourceRect.Height * a.SourceRect.Width;
            var bSize = b.SourceRect.Height * b.SourceRect.Width;
            return bSize.CompareTo(aSize);
        }

        /// <summary>
        /// Ensures that we have found the maximum allowed texture size.
        /// </summary>
        static void UpdateMaxTextureSize()
        {
            if (_maxTextureSize != int.MinValue)
                return;

            // FUTURE: Query the hardware for the actual _maxTextureSize value allowed by the system
            _maxTextureSize = 1024;

            // Ensure the size found is logical
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

            Image _atlasTexture;

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
            internal void BuildTexture(int padding)
            {
                // Dispose of the old texture if there is one
                if (_atlasTexture != null)
                    _atlasTexture.Dispose();

                // Draw the atlas (rebuilding the texture)
                IEnumerable<AtlasTextureItem> successful;
                _atlasTexture = DrawAtlas(padding, out successful);

                // Tell all the items that were successfully added to use the new atlas texture
                foreach (var node in successful)
                {
                    var r = new Rectangle(node.X + Padding, node.Y + Padding, node.Width - Padding * 2, node.Height - Padding * 2);
                    node.ITextureAtlasable.SetAtlas(_atlasTexture, r);
                }
            }

            /// <summary>
            /// Draws the <see cref="Image"/> for this atlas.
            /// </summary>
            /// <param name="padding">The amount to pad each item.</param>
            /// <param name="successfulItems">An IEnumerable of the <see cref="AtlasTextureItem"/>s that were
            /// successfully draw to the atlas.</param>
            /// <returns>A <see cref="Image"/> of the atlas.</returns>
            Image DrawAtlas(int padding, out IEnumerable<AtlasTextureItem> successfulItems)
            {
                // Create the list for successful items
                var successful = new List<AtlasTextureItem>();
                successfulItems = successful;

                // Try to create the atlas texture. If any exceptions are thrown when trying to create the texture,
                // do not use a texture atlas at all.
                const string errmsg = "Failed to create TextureAtlas texture. Exception: {0}";
                Image ret = null;
                try
                {
                    ret = new Image((uint)_width, (uint)_height, _backColor) { Smooth = false };
                    ret.CreateMaskFromColor(_backColor);
                    DrawAtlasDrawingHandler(ret, padding, successful);
                }
                catch (ObjectDisposedException ex)
                {
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);
                    Debug.Fail(string.Format(errmsg, ex));
                }

                // If we have a null Texture2D right here, it means we failed completely to create the atlas.
                // So clear the successful list completely, remove the atlas from all items, and return a null texture.
                if (ret == null)
                {
                    foreach (var node in Nodes)
                    {
                        node.ITextureAtlasable.RemoveAtlas();
                    }

                    successful.Clear();

                    return null;
                }

#pragma warning disable 162
                // Save the generated atlas
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (_saveGeneratedAtlasToTemp)
                    SaveTextureToTempFile(ret);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
#pragma warning restore 162

                return ret;
            }

            /// <summary>
            /// Handles the actual drawing of the atlas items onto a single <see cref="Image"/>.
            /// </summary>
            /// <param name="destImg">The <see cref="Image"/> to copy to.</param>
            /// <param name="padding">The amount of padding, in pixels, to place around each sprite in the atlas. If this
            /// values is greater than 0, all 4 sides of the atlas item will be copied one pixel outwards. This
            /// is to help prevent issues when using filtering with sprites.</param>
            /// <param name="successful">The <see cref="ICollection{T}"/> to populate with the 
            /// <see cref="AtlasTextureItem"/>s that were successfully drawn. Usually, an item will fail to draw
            /// when the item's texture cannot be loaded.</param>
            void DrawAtlasDrawingHandler(Image destImg, int padding, ICollection<AtlasTextureItem> successful)
            {
                // Draw every atlas item to the texture
                foreach (var item in Nodes)
                {
                    // Make sure this item is not already part of an atlas. While it is handy to have an atlas
                    // draw to another atlas, the benefits of that are likely minimal, and it is far more important
                    // to avoid having an invalid atlas (such as when the device is lost) drawing to another atlas
                    item.ITextureAtlasable.RemoveAtlas();

                    // Grab the texture and make sure it is valid
                    Image tex;
                    try
                    {
                        tex = item.ITextureAtlasable.Texture;
                        if (tex.IsDisposed)
                            tex = null;
                    }
                    catch (LoadingFailedException)
                    {
                        tex = null;
                    }

                    if (tex == null)
                    {
                        const string errmsg = "Failed to add item `{0}` to atlas - texture is null or disposed.";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, item);
                        continue;
                    }

                    // Draw the actual image (raw, no borders)
                    var srcRect = item.ITextureAtlasable.SourceRect;
                    var dest = new Vector2(item.Rect.X + padding, item.Y + padding);
                    var src = srcRect;
                    DrawToAtlas(destImg, tex, dest, src);

                    // Successfully drawn
                    successful.Add(item);

                    // Create the 1px borders only if padded
                    if (padding == 0)
                        continue;

                    // Left border
                    src.Width = 1;
                    dest.X -= 1;
                    DrawToAtlas(destImg, tex, dest, src);

                    // Right border
                    src.X += srcRect.Width - 1;
                    dest.X += srcRect.Width + 1;
                    DrawToAtlas(destImg, tex, dest, src);

                    // Top border
                    src = new Rectangle(srcRect.X, srcRect.Y, srcRect.Width, 1);
                    dest.X = item.X + padding;
                    dest.Y = item.Y + padding - 1;
                    DrawToAtlas(destImg, tex, dest, src);

                    // Bottom border
                    src.Y += srcRect.Height - 1;
                    dest.Y += srcRect.Height + 1;
                    DrawToAtlas(destImg, tex, dest, src);

                    // Top-left corner
                    src = new Rectangle(srcRect.X, srcRect.Y, 1, 1);
                    dest.X = item.X + padding - 1;
                    dest.Y = item.Y + padding - 1;
                    DrawToAtlas(destImg, tex, dest, src);

                    // Top-right corner
                    src.X += srcRect.Width - 1;
                    dest.X += srcRect.Width + 1;
                    DrawToAtlas(destImg, tex, dest, src);

                    // Bottom-right corner
                    src.Y += srcRect.Height - 1;
                    dest.Y += srcRect.Height + 1;
                    DrawToAtlas(destImg, tex, dest, src);

                    // Bottom-left corner
                    src.X -= srcRect.Width - 1;
                    dest.X -= srcRect.Width + 1;
                    DrawToAtlas(destImg, tex, dest, src);
                }
            }

            /// <summary>
            /// Draws a single item the atlas through a <see cref="ISpriteBatch"/>.
            /// </summary>
            /// <param name="destImg">The <see cref="Image"/> to copy to.</param>
            /// <param name="srcImg">The source <see cref="Image"/>.</param>
            /// <param name="dest">The drawing destination on the atlas.</param>
            /// <param name="src">The source rectangle to draw from the <paramref name="srcImg"/>.</param>
            static void DrawToAtlas(Image destImg, Image srcImg, Vector2 dest, Rectangle src)
            {
                destImg.Copy(srcImg, (uint)dest.X, (uint)dest.Y, (IntRect)src);
            }

            /// <summary>
            /// Saves the texture to a temp file so the generated atlases can be viewed. Only for when using
            /// debug mode.
            /// </summary>
            [Conditional("DEBUG")]
            static void SaveTextureToTempFile(Image texture)
            {
                var f = new TempFile();
                texture.SaveToFile(f.FilePath);
            }

            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                foreach (var node in Nodes)
                {
                    node.ITextureAtlasable.RemoveAtlas();
                }

                if (_atlasTexture != null && !_atlasTexture.IsDisposed)
                {
                    try
                    {
                        _atlasTexture.Dispose();
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to dispose atlas item: {0}";
                        if (log.IsWarnEnabled)
                            log.WarnFormat(errmsg, ex);
                        Debug.Fail(string.Format(errmsg, ex));
                    }
                }
            }

            #endregion
        }
    }
}