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
    public static class WindowExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates a <see cref="RenderTexture"/> for a <see cref="Window"/> that can be used as a buffer for the <see cref="Window"/>.
        /// </summary>
        /// <param name="w">The <see cref="Window"/> to create the <see cref="RenderTexture"/> for.</param>
        /// <param name="ri">An optional, pre-existing <see cref="RenderTexture"/> to try to use. If this <see cref="RenderTexture"/>
        /// instance is already set up as needed, it will be used instead of creating a new <see cref="RenderTexture"/>. This is
        /// primarily provided so you can pass a previous output from this method back into it.</param>
        /// <param name="disposeExisting">If <paramref name="ri"/> does not work for the <paramref name="w"/> and a new
        /// <see cref="RenderTexture"/> has to be created, if this value is true, then the <paramref name="ri"/> will be
        /// disposed of. If false, it will not be disposed of.</param>
        /// <returns>
        /// A <see cref="RenderTexture"/> that can be used as a buffer for the <paramref name="w"/>, or null if the
        /// <see cref="RenderTexture"/> could needed to be recreated but could not be. Usually, it is okay to try again later
        /// (such as the next frame) if the <see cref="RenderTexture"/> failed to be created.
        /// </returns>
        public static RenderTexture CreateBufferRenderTexture(this Window w, RenderTexture ri = null, bool disposeExisting = true)
        {
            try
            {
                // Check if the provided RenderImage works for our needs
                var mustRecreate = false;
                try
                {
                    if (ri == null || ri.IsDisposed || ri.Size != w.Size)
                        mustRecreate = true;
                }
                catch (InvalidOperationException)
                {
                    // Common exception that occurs when the RenderImage is invalid or corrupt - recreate and do not report
                    mustRecreate = true;
                }
                catch (AccessViolationException)
                {
                    // Common exception that occurs when the RenderImage is invalid or corrupt - recreate and do not report
                    mustRecreate = true;
                }
                catch (Exception ex)
                {
                    // Recreate during any exception, but when its not one we know is fine to ignore, report it
                    const string errmsg = "Unexpected exception when reading properties of a RenderImage. Forcing recreation. Exception: {0}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, ex);

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
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to dispose RenderImage. This is usually not a concern. Exception: {0}";

                        // Ignore failure to dispose
                        if (log.IsInfoEnabled)
                            log.InfoFormat(errmsg, ex);
                    }
                }

                // Get the size to make the new RenderImage (same size of the window)
                uint width;
                uint height;
                try
                {
                    var size = w.Size;
                    width = (uint)size.X;
                    height = (uint)size.Y;
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

                // Check for a valid RenderWindow size. These can be 0 when the window has been minimized.
                if (width <= 0 || height <= 0)
                {
                    const string errmsg =
                        "Unable to create window buffer render image" +
                        " - invalid Width/Height ({0},{1}) returned from Window. Most likely, the form was minimized.";
                    if (log.IsInfoEnabled)
                        log.InfoFormat(errmsg, width, height);
                    return null;
                }

                // Create the new RenderImage
                try
                {
                    ri = new RenderTexture(width, height);
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