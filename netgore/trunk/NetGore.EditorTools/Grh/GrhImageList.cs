using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using NetGore.EditorTools.Properties;
using NetGore.Graphics;

// FUTURE: Unload images that have not been used for an extended period of time

namespace NetGore.EditorTools
{
    /// <summary>
    /// Contains the <see cref="ImageList"/> used for the <see cref="GrhData"/>s to display the shrunken
    /// icon on the <see cref="GrhTreeView"/> or any other iconized <see cref="GrhData"/> preview.
    /// </summary>
    public class GrhImageList
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Image _closedFolder;
        static readonly Image _errorImage;
        static readonly GrhImageList _instance;
        static readonly Image _openFolder;

        readonly Dictionary<string, Image> _images = new Dictionary<string, Image>();
        readonly object _imagesSync = new object();

        /// <summary>
        /// The height of images in the <see cref="GrhImageList"/>.
        /// </summary>
        public const int ImageWidth = 16;

        /// <summary>
        /// The width of images in the <see cref="GrhImageList"/>.
        /// </summary>
        public const int ImageHeight = 16;

        /// <summary>
        /// Initializes the <see cref="GrhImageList"/> class.
        /// </summary>
        static GrhImageList()
        {
            // Load the folder images
            _openFolder = Resources.folderopen;
            _closedFolder = Resources.folder;

            // Create the error image
            var bmp = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
            using (var g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.DrawLine(Pens.Red, new Point(0, 0), new Point(bmp.Width, bmp.Height));
                g.DrawLine(Pens.Red, new Point(bmp.Width, 0), new Point(0, bmp.Height));
            }

            _errorImage = bmp;

            // Load the instance
            _instance = new GrhImageList();
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for a closed folder.
        /// </summary>
        public static Image ClosedFolder
        {
            get { return _closedFolder; }
        }

        /// <summary>
        /// Gets the <see cref="Image"/> to use for invalid values.
        /// </summary>
        public static Image ErrorImage
        {
            get { return _errorImage; }
        }

        /// <summary>
        /// Gets the <see cref="GrhImageList"/> instance.
        /// </summary>
        public static GrhImageList Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for an open folder.
        /// </summary>
        public static Image OpenFolder
        {
            get { return _openFolder; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhImageList"/> class.
        /// </summary>
        GrhImageList()
        {
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument, or null if the <see cref="Image"/> is not yet loaded.
        /// </summary>
        /// <param name="gd">The <see cref="GrhData"/> to get the <see cref="Image"/> for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="gd"/>.</returns>
        public Image GetImage(GrhData gd)
        {
            if (gd == null)
                return _errorImage;

            return GetImage(gd.GetFrame(0));
        }
        
        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument, or null if the <see cref="Image"/> is not yet loaded.
        /// </summary>
        /// <param name="obj">The object to get the <see cref="Image"/> for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="obj"/>.</returns>
        public Image TryGetImage(object obj)
        {
            if (obj is Grh)
                return GetImage((Grh)obj);
            if (obj is GrhData)
                return GetImage((GrhData)obj);
            if (obj is GrhIndex)
                return GetImage((GrhIndex)obj);

            return ErrorImage;
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument, or null if the <see cref="Image"/> is not yet loaded.
        /// </summary>
        /// <param name="grhIndex">The <see cref="GrhIndex"/> to get the <see cref="Image"/> for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="grhIndex"/>.</returns>
        public Image GetImage(GrhIndex grhIndex)
        {
            var gd = GrhInfo.GetData(grhIndex);
            return GetImage(gd);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument, or null if the <see cref="Image"/> is not yet loaded.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to get the <see cref="Image"/> for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="grh"/>.</returns>
        public Image GetImage(Grh grh)
        {
            if (grh == null)
                return _errorImage;

            return GetImage(grh.CurrentGrhData);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument, or null if the <see cref="Image"/> is not yet loaded.
        /// </summary>
        /// <param name="gd">The <see cref="StationaryGrhData"/> to get the <see cref="Image"/> for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="gd"/>.</returns>
        public Image GetImage(StationaryGrhData gd)
        {
            if (gd == null)
                return _errorImage;

            var key = GetImageKey(gd);

            Image img;
            lock (_imagesSync)
            {
                if (_images.TryGetValue(key, out img))
                    return img;

                img = gd.Texture.ToBitmap(gd.SourceRect, ImageWidth, ImageHeight);

                if (log.IsDebugEnabled)
                    log.DebugFormat("Created GrhImageList image for `{0}`.", img);

                _images.Add(key, img);
            }

            return img;
        }

        /// <summary>
        /// Gets the image key for a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> to get the image key for.</param>
        /// <returns>The image key for the <paramref name="grhData"/>.</returns>
        protected virtual string GetImageKey(StationaryGrhData grhData)
        {
            if (grhData == null)
                return string.Empty;

            if (!grhData.GrhIndex.IsInvalid)
            {
                // For normal GrhDatas, we return the unique GrhIndex
                var grhIndex = grhData.GrhIndex;

                if (grhIndex.IsInvalid)
                    return string.Empty;

                return grhIndex.ToString();
            }
            else
            {
                // When we have a frame for a GrhData with an invalid GrhIndex, we prefix a "_" and the use the texture name
                var textureName = grhData.TextureName != null ? grhData.TextureName.ToString() : null;
                if (string.IsNullOrEmpty(textureName))
                    return string.Empty;
                else
                    return "_" + textureName;
            }
        }
    }
}