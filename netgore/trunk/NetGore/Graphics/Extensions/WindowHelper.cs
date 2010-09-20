using System;
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
    /// Extension methods for the <see cref="Window"/> class.
    /// </summary>
    public static class WindowHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates a <see cref="RenderImage"/> for a <see cref="Window"/> that can be used as a buffer for the <see cref="Window"/>.
        /// </summary>
        /// <param name="w">The <see cref="Window"/> to create the <see cref="RenderImage"/> for.</param>
        /// <param name="ri">An optional, pre-existing <see cref="RenderImage"/> to try to use. If this <see cref="RenderImage"/>
        /// instance is already set up as needed, it will be used instead of creating a new <see cref="RenderImage"/>. This is
        /// primarily provided so you can pass a previous output from this method back into it.</param>
        /// <param name="disposeExisting">If <paramref name="ri"/> does not work for the <paramref name="w"/> and a new
        /// <see cref="RenderImage"/> has to be created, if this value is true, then the <paramref name="ri"/> will be
        /// disposed of. If false, it will not be disposed of.</param>
        /// <returns>
        /// A <see cref="RenderImage"/> that can be used as a buffer for the <paramref name="w"/>, or null if the
        /// <see cref="RenderImage"/> could needed to be recreated but could not be. Usually, it is okay to try again later
        /// (such as the next frame) if the <see cref="RenderImage"/> failed to be created.
        /// </returns>
        public static RenderImage CreateBufferRenderImage(this Window w, RenderImage ri = null, bool disposeExisting = true)
        {
            try
            {
                // Check if the provided RenderImage works for our needs
                var mustRecreate = false;
                try
                {
                    if (ri == null || ri.IsDisposed || ri.Width != w.Width || ri.Height != w.Height)
                        mustRecreate = true;
                }
                catch (InvalidOperationException)
                {
                    mustRecreate = true;
                }

                // Create the buffer RenderImage if needed
                if (!mustRecreate)
                    return ri;

                // If there is an old Image, make sure to dispose of it... or at least try to
                if (ri != null && disposeExisting)
                {
                    try
                    {
                        if (!ri.IsDisposed)
                            ri.Dispose();
                    }
                    catch (InvalidOperationException)
                    {
                        // Ignore failure to dispose
                    }
                }

                // Get the size to make the light map (same size of the window)
                int width;
                int height;
                try
                {
                    width = (int)w.Width;
                    height = (int)w.Height;
                }
                catch (InvalidOperationException ex)
                {
                    const string errmsg =
                        "Failed to create window buffer render image" +
                        " - failed to get Window width/height. Will attempt again next frame. Exception: {0}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, ex);
                    return null;
                }

                // Check for a valid RenderWindow size. These can be 0 when the RenderWindow has been minimized.
                if (width <= 0 || height <= 0)
                {
                    const string errmsg =
                        "Unable to create window buffer render image" +
                        " - invalid Width/Height ({0},{1}) returned from Window. Most likely, the form was minimized.";
                    if (log.IsInfoEnabled)
                        log.InfoFormat(errmsg, width, height);
                    return null;
                }

                // Create the new Image
                try
                {
                    ri = new RenderImage(w.Width, w.Height);
                }
                catch (LoadingFailedException ex)
                {
                    const string errmsg =
                        "Failed to create window buffer render image" + " - construction of Image failed. Exception: {0}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, ex);
                    return null;
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Completely unexpected exception in CreateBufferRenderImage. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
                return null;
            }

            return ri;
        }
    }
}