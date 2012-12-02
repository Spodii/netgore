using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using NetGore.Editor.Properties;
using NetGore.Graphics;
using SFML;

// FUTURE: Unload images that have not been used for an extended period of time

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// Contains the <see cref="ImageList"/> used for the <see cref="GrhData"/>s to display the shrunken
    /// icon on the <see cref="GrhTreeView"/> or any other iconized <see cref="GrhData"/> preview.
    /// </summary>
    public class GrhImageList
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The width of images in the <see cref="GrhImageList"/>.
        /// </summary>
        public const int ImageHeight = 16;

        /// <summary>
        /// The height of images in the <see cref="GrhImageList"/>.
        /// </summary>
        public const int ImageWidth = 16;

        /// <summary>
        /// How long to wait while spin-waiting for an image to finish being generated. The longer the delay, the longer it will take
        /// for the spin-wait to finish, but the less overhead there will be.
        /// </summary>
        const int _spinWaitSleepTime = 200;

        static readonly Image _closedFolder;
        static readonly Image _errorImage;
        static readonly GrhImageList _instance;
        static readonly Image _openFolder;

        /// <summary>
        /// The <see cref="Image"/> to place in the <see cref="_images"/> collection when we are creating the actual
        /// <see cref="Image"/>. Since async mode returns null when an <see cref="Image"/> is not already created and
        /// non-async mode either spin-waits or creates the <see cref="Image"/>, this should never actually end up
        /// leaving this class. Still, its easiest and less confusing to use an actual valid <see cref="Image"/>
        /// instead of null or something.
        /// </summary>
        static readonly Image _placeholder;

        readonly Dictionary<string, Image> _images = new Dictionary<string, Image>();
        readonly object _imagesSync = new object();
        readonly WaitCallback _threadPoolCallback;

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

            // Create the placeholder image
            _placeholder = ImageHelper.CreateSolid(1, 1, Color.White);

            // Load the instance
            _instance = new GrhImageList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhImageList"/> class.
        /// </summary>
        GrhImageList()
        {
            _threadPoolCallback = ThreadPoolCallback;
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
        /// Creates an <see cref="Image"/> for a <see cref="StationaryGrhData"/>, adds it to the cache, and returns it.
        /// Only call this if the image actually needs to be created, and never call this while in the <see cref="_imagesSync"/>
        /// lock!
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="gd">The <see cref="StationaryGrhData"/>.</param>
        /// <returns>The <see cref="Image"/>.</returns>
        Image CreateAndInsertImage(string key, StationaryGrhData gd)
        {
            Image img;

            // The image was null, so we are going to be the ones to create it
            try
            {
                // Try to create the image
                var tex = gd.GetOriginalTexture();
                if (tex == null)
                    img = ErrorImage;
                else
                    img = tex.ToBitmap(gd.OriginalSourceRect, ImageWidth, ImageHeight);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to create GrhImageList image for `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, gd, ex);
                if (!(ex is LoadingFailedException))
                    Debug.Fail(string.Format(errmsg, gd, ex));
                img = _errorImage;
            }

            if (log.IsDebugEnabled)
                log.DebugFormat("Created GrhImageList image for `{0}`.", img);

            // If we for some reason have the _placeholder or null image by this point, there is an error in the logic above. But to
            // avoid a deadlock (even though this should never happen), we will just set it to the ErrorImage if it somehow does.
            if (img == null || img == _placeholder)
            {
                const string errmsg = "Created image was either null or the placeholder image, which should never happen.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                img = _errorImage;
            }

            // Add the image to the cache (it will either be the correct image, or the ErrorImage if it failed). Remember that we
            // have already inserted the placeholder image at the key. So we're just replacing the placeholder with the actual image.
            lock (_imagesSync)
            {
                Debug.Assert(_images[key] == _placeholder);
                _images[key] = img;
            }

            // Return the generated image
            return img;
        }

        /// <summary>
        /// Scales the unscaled <see cref="Bitmap"/> for a <see cref="StationaryGrhData"/>.
        /// </summary>
        /// <param name="unscaled">The unscaled <see cref="Image"/>.</param>
        /// <returns>The scaled <see cref="Bitmap"/> scaled to the size for this image list, or null if an error occured.</returns>
        static Bitmap CreateScaledBitmap(Image unscaled)
        {
            try
            {
                return unscaled.CreateScaled(ImageWidth, ImageHeight, true, null, null);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to scale bitmap down to size. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
                return null;
            }
        }

        /// <summary>
        /// Creates an unscaled <see cref="Bitmap"/> of a <see cref="StationaryGrhData"/>.
        /// </summary>
        /// <param name="gd">The <see cref="StationaryGrhData"/>.</param>
        /// <returns>The unscaled <see cref="Bitmap"/>, or null if an error occured.</returns>
        static Bitmap CreateUnscaledBitmap(StationaryGrhData gd)
        {
            Bitmap img;

            // The image was null, so we are going to be the ones to create it
            try
            {
                // Try to create the image
                var tex = gd.GetOriginalTexture();
                if (tex == null)
                    img = null;
                else
                    img = tex.ToBitmap(gd.OriginalSourceRect);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to create GrhImageList image for `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, gd, ex);
                if (!(ex is LoadingFailedException) && !(ex is OutOfMemoryException))
                    Debug.Fail(string.Format(errmsg, gd, ex));
                img = null;
            }

            return img;
        }

        /// <summary>
        /// Executes a <see cref="ThreadPoolAsyncCallbackState"/> on the <see cref="ThreadPool"/> by passing it to the
        /// <see cref="ThreadPoolCallback"/> method. If it fails to be added to the <see cref="ThreadPool"/> for whatever
        /// reason, it will execute synchronously.
        /// </summary>
        /// <param name="state">The <see cref="ThreadPoolAsyncCallbackState"/> describing the state and operation to perform.</param>
        void ExecuteOnThreadPool(ThreadPoolAsyncCallbackState state)
        {
            bool wasEnqueued;

            try
            {
                // Try to add to the thread pool
                wasEnqueued = ThreadPool.QueueUserWorkItem(_threadPoolCallback, state);

                if (!wasEnqueued)
                {
                    const string errmsg =
                        "Failed to create thread to generate image for GrhData `{0}`" +
                        " - running synchronously instead. ThreadPool.QueueWorkItem() returned false.";
                    if (log.IsInfoEnabled)
                        log.InfoFormat(errmsg, state.GrhData);
                }
            }
            catch (OutOfMemoryException ex)
            {
                wasEnqueued = false;

                if (!wasEnqueued)
                {
                    const string errmsg =
                        "Failed to create thread to generate image for GrhData `{0}`" +
                        " - running synchronously instead. Exception: {1}";
                    if (log.IsInfoEnabled)
                        log.InfoFormat(errmsg, state.GrhData, ex);
                }
            }
            catch (ApplicationException ex)
            {
                wasEnqueued = false;

                const string errmsg =
                    "Failed to create thread to generate image for GrhData `{0}`" +
                    " - running synchronously instead. Exception: {1}";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, state.GrhData, ex);
            }

            // If we failed to enqueue it on the thread pool, run it synchronously
            if (!wasEnqueued)
                ThreadPoolCallback(state);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument.
        /// </summary>
        /// <param name="gd">The <see cref="GrhData"/> to get the <see cref="Image"/> for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="gd"/>.</returns>
        public Image GetImage(GrhData gd)
        {
            if (gd == null)
                return _errorImage;

            return GetImage(gd.GetFrame(0), false, null, null);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument.
        /// </summary>
        /// <param name="grhIndex">The <see cref="GrhIndex"/> to get the <see cref="Image"/> for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="grhIndex"/>.</returns>
        public Image GetImage(GrhIndex grhIndex)
        {
            var gd = GrhInfo.GetData(grhIndex);
            return GetImage(gd);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to get the <see cref="Image"/> for.</param>
        /// <returns>The <see cref="Image"/> for the <paramref name="grh"/>.</returns>
        public Image GetImage(Grh grh)
        {
            if (grh == null)
                return _errorImage;

            return GetImage(grh.CurrentGrhData, false, null, null);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument.
        /// </summary>
        /// <param name="gd">The <see cref="StationaryGrhData"/> to get the <see cref="Image"/> for.</param>
        /// <returns>
        /// The <see cref="Image"/> for the <paramref name="gd"/>.
        /// </returns>
        public Image GetImage(StationaryGrhData gd)
        {
            return GetImage(gd, false, null, null);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument.
        /// </summary>
        /// <param name="gd">The <see cref="StationaryGrhData"/> to get the <see cref="Image"/> for.</param>
        /// <param name="async">If true, asynchronous mode will be used. This will return null immediately if the desired
        /// <see cref="Image"/> has not yet been created.</param>
        /// <param name="callback">When <see cref="async"/> is false, contains the callback method to invoke when the <see cref="Image"/>
        /// has been created.</param>
        /// <param name="userState">The optional user state object to pass to the <paramref name="callback"/>.</param>
        /// <returns>
        /// The <see cref="Image"/> for the <paramref name="gd"/>, or null if <paramref name="async"/> is set.
        /// </returns>
        Image GetImage(StationaryGrhData gd, bool async, GrhImageListAsyncCallback callback, object userState)
        {
            if (gd == null)
            {
                if (!async)
                {
                    // Return the ErrorImage directly
                    return ErrorImage;
                }
                else
                {
                    // Raise the callback and pass the ErrorImage
                    if (callback != null)
                        callback(this, gd, ErrorImage, userState);
                    return null;
                }
            }

            // Get the key
            var key = GetImageKey(gd);

            // Get the image from the cache
            Image img;
            lock (_imagesSync)
            {
                // Check if the image already exists
                if (!_images.TryGetValue(key, out img))
                {
                    // Image does not exist, so add the placeholder since we are about to create it. Placing the placeholder
                    // in there will make sure that no other threads try to create it at the same time.
                    img = null;
                    _images.Add(key, _placeholder);
                }
            }

            if (!async)
            {
                if (img != null)
                {
                    if (img == _placeholder)
                    {
                        // If we got the placeholder image, do a spin-wait until we get the actual image, then return the image. This will
                        // happen when another thread is creating the image.
                        return SpinWaitForImage(key);
                    }
                    else
                    {
                        // Any other non-null image means that the image was already created, so we can just return it immediately
                        return img;
                    }
                }
                else
                {
                    // Create it on this thread and return it when its done
                    return CreateAndInsertImage(key, gd);
                }
            }
            else
            {
                if (img != null)
                {
                    // When we get the placeholder image while in async mode, this is slightly more annoying since we have
                    // to create another thread to spin-wait on
                    if (img == _placeholder)
                    {
                        // Create the thread to spin-wait on... though only if we were given a callback method. Obviously does no
                        // good to wait for the image when there is no callback method.
                        if (callback != null)
                        {
                            var tpacs = new ThreadPoolAsyncCallbackState(gd, callback, userState, true, null);
                            ExecuteOnThreadPool(tpacs);
                        }
                    }
                    else
                    {
                        // But when we get the actual image, we can just invoke the callback directly from this thread. This is the
                        // once scenario where no threads are created in async mode.
                        if (callback != null)
                            callback(this, gd, img, userState);
                    }
                }
                else
                {
                    // NOTE: The asynchronous aspect is less than optimal due to this.
                    // When originally designing this, I was working under the assumption that SFML would be able to deal with the threading
                    // better. Turns out I was wrong. There are probably some other threading issues that would have to be taken into
                    // account, too, like that content can be disposed on the main thread. I could offload more onto the worker thread
                    // than just the rescaling, such as the generation of the original unscaled bitmap, but the biggest gains come from
                    // the ability to offload the actual image loading. I guess its helpful to have at least a little work offloaded
                    // than to be completely synchronous since that does mean multi-core CPUs can load a bit faster.
                    var bmp = CreateUnscaledBitmap(gd);
                    if (bmp == null)
                    {
                        // If the bitmap failed to be created for whatever reason, use the ErrorImage
                        if (callback != null)
                            callback(this, gd, ErrorImage, userState);

                        lock (_imagesSync)
                        {
                            Debug.Assert(_images[key] == _placeholder);
                            _images[key] = ErrorImage;
                        }
                    }
                    else
                    {
                        // Add the Image creation job to the thread pool
                        var tpacs = new ThreadPoolAsyncCallbackState(gd, callback, userState, false, bmp);
                        ExecuteOnThreadPool(tpacs);
                    }
                }

                // Async always returns null
                return null;
            }
        }

        /// <summary>
        /// Asynchronously gets the <see cref="Image"/> for the given argument.
        /// </summary>
        /// <param name="gd">The <see cref="GrhData"/> to get the <see cref="Image"/> for.</param>
        /// <param name="callback">The <see cref="GrhImageListAsyncCallback"/> to invoke when the operation has finished.</param>
        /// <param name="userState">The optional user state object to pass to the <paramref name="callback"/>.</param>
        public void GetImageAsync(GrhData gd, GrhImageListAsyncCallback callback, object userState)
        {
            if (gd == null)
            {
                if (callback != null)
                    callback(this, null, ErrorImage, userState);
            }
            else
                GetImage(gd.GetFrame(0), true, callback, userState);
        }

        /// <summary>
        /// Asynchronously gets the <see cref="Image"/> for the given argument.
        /// </summary>
        /// <param name="grhIndex">The <see cref="GrhIndex"/> to get the <see cref="Image"/> for.</param>
        /// <param name="callback">The <see cref="GrhImageListAsyncCallback"/> to invoke when the operation has finished.</param>
        /// <param name="userState">The optional user state object to pass to the <paramref name="callback"/>.</param>
        public void GetImageAsync(GrhIndex grhIndex, GrhImageListAsyncCallback callback, object userState)
        {
            var gd = GrhInfo.GetData(grhIndex);
            GetImageAsync(gd, callback, userState);
        }

        /// <summary>
        /// Asynchronously gets the <see cref="Image"/> for the given argument.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to get the <see cref="Image"/> for.</param>
        /// <param name="callback">The <see cref="GrhImageListAsyncCallback"/> to invoke when the operation has finished.</param>
        /// <param name="userState">The optional user state object to pass to the <paramref name="callback"/>.</param>
        public void GetImageAsync(Grh grh, GrhImageListAsyncCallback callback, object userState)
        {
            if (grh == null)
            {
                if (callback != null)
                    callback(this, null, ErrorImage, userState);
            }
            else
                GetImage(grh.CurrentGrhData, true, callback, userState);
        }

        /// <summary>
        /// Asynchronously gets the <see cref="Image"/> for the given argument.
        /// </summary>
        /// <param name="gd">The <see cref="StationaryGrhData"/> to get the <see cref="Image"/> for.</param>
        /// <param name="callback">The <see cref="GrhImageListAsyncCallback"/> to invoke when the operation has finished.</param>
        /// <param name="userState">The optional user state object to pass to the <paramref name="callback"/>.</param>
        public void GetImageAsync(StationaryGrhData gd, GrhImageListAsyncCallback callback, object userState)
        {
            GetImage(gd, true, callback, userState);
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

        /// <summary>
        /// Does a spin-wait while waiting for the image for a key. Use this when there is the <see cref="_placeholder"/> image
        /// is at a key and the calling method wants to wait for the real image to be generated.
        /// </summary>
        /// <param name="key">The key of the image to wait for.</param>
        /// <returns>The <see cref="Image"/> for the given <paramref name="key"/>.</returns>
        Image SpinWaitForImage(string key)
        {
            Image ret;
            do
            {
                lock (_imagesSync)
                {
                    ret = _images[key];
                }

                Thread.Sleep(_spinWaitSleepTime);
            }
            while (ret == _placeholder);

            return ret;
        }

        /// <summary>
        /// The callback for when invoking the an operation to either spin-wait for or create an <see cref="Image"/>
        /// asynchronously.
        /// </summary>
        /// <param name="state">The state object (contains the <see cref="ThreadPoolAsyncCallbackState"/>).</param>
        void ThreadPoolCallback(object state)
        {
            var s = (ThreadPoolAsyncCallbackState)state;
            var key = GetImageKey(s.GrhData);

            Image img;

            if (s.Wait)
            {
                // Wait for the image to be created
                img = SpinWaitForImage(key);
            }
            else
            {
                // Create the image
                img = CreateScaledBitmap(s.Bitmap) ?? ErrorImage;

                lock (_imagesSync)
                {
                    Debug.Assert(_images[key] == _placeholder);
                    _images[key] = img;
                }

                // Dispose of our temporary bitmap containing the unscaled image
                if (s.Bitmap != img)
                    s.Bitmap.Dispose();
            }

            // Invoke the callback method
            if (s.Callback != null)
                s.Callback(this, s.GrhData, img, s.UserState);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> for the given argument.
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
        /// Asynchronously gets the <see cref="Image"/> for the given argument.
        /// </summary>
        /// <param name="obj">The object to get the <see cref="Image"/> for.</param>
        /// <param name="callback">The <see cref="GrhImageListAsyncCallback"/> to invoke when the operation has finished.</param>
        /// <param name="userState">The optional user state object to pass to the <paramref name="callback"/>.</param>
        public void TryGetImageAsync(object obj, GrhImageListAsyncCallback callback, object userState)
        {
            if (obj is Grh)
            {
                // Grh
                GetImageAsync((Grh)obj, callback, userState);
            }
            else if (obj is GrhData)
            {
                // GrhData
                GetImageAsync((GrhData)obj, callback, userState);
            }
            else if (obj is GrhIndex)
            {
                // Grhindex
                GetImageAsync((GrhIndex)obj, callback, userState);
            }
            else
            {
                // Invalid type
                if (callback != null)
                    callback(this, null, ErrorImage, userState);
            }
        }

        /// <summary>
        /// Describes the information when making a call to the <see cref="ThreadPool"/> to perform asynchronous <see cref="Image"/>
        /// acquisition for when the <see cref="Image"/> has not yet been created.
        /// </summary>
        class ThreadPoolAsyncCallbackState
        {
            readonly Bitmap _bitmap;
            readonly GrhImageListAsyncCallback _callback;
            readonly StationaryGrhData _grhData;
            readonly object _userState;
            readonly bool _wait;

            /// <summary>
            /// Initializes a new instance of the <see cref="ThreadPoolAsyncCallbackState"/> class.
            /// </summary>
            /// <param name="grhData">The <see cref="StationaryGrhData"/>.</param>
            /// <param name="callback">The callback to invoke when complete.</param>
            /// <param name="userState">The user state object.</param>
            /// <param name="wait">If true, performs a spin-wait. If false, generates the <see cref="Image"/> on the thread.</param>
            /// <param name="bitmap">The unscaled <see cref="Bitmap"/>. Only needed when <paramref name="wait"/> is false.</param>
            public ThreadPoolAsyncCallbackState(StationaryGrhData grhData, GrhImageListAsyncCallback callback, object userState,
                                                bool wait, Bitmap bitmap)
            {
                _grhData = grhData;
                _callback = callback;
                _userState = userState;
                _wait = wait;
                _bitmap = bitmap;
            }

            /// <summary>
            /// Gets the unscaled <see cref="Bitmap"/> to scale down.
            /// </summary>
            public Bitmap Bitmap
            {
                get { return _bitmap; }
            }

            /// <summary>
            /// Gets the callback to invoke when done.
            /// </summary>
            public GrhImageListAsyncCallback Callback
            {
                get { return _callback; }
            }

            /// <summary>
            /// Gets the <see cref="StationaryGrhData"/> to generate the <see cref="Image"/> for.
            /// </summary>
            public StationaryGrhData GrhData
            {
                get { return _grhData; }
            }

            /// <summary>
            /// Gets the user state object to pass to the <see cref="Callback"/>.
            /// </summary>
            public object UserState
            {
                get { return _userState; }
            }

            /// <summary>
            /// Gets if the <see cref="Image"/> is to be generanted on the thread or spin-wait. If false, the <see cref="Image"/> is generated
            /// on this thread. If true, perform a spin-wait.
            /// </summary>
            public bool Wait
            {
                get { return _wait; }
            }
        }
    }
}