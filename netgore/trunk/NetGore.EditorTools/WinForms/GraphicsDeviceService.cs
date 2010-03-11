using System;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

// The IGraphicsDeviceService interface requires a DeviceCreated event, but we
// always just create the device inside our constructor, so we have no place to
// raise that event. The C# compiler warns us that the event is never used, but
// we don't care so we just disable this warning.
#pragma warning disable 67

namespace NetGore.EditorTools
{
    /// <summary>
    /// Helper class responsible for creating and managing the GraphicsDevice.
    /// All GraphicsDeviceControl instances share the same GraphicsDeviceService,
    /// so even though there can be many controls, there will only ever be a single
    /// underlying GraphicsDevice. This implements the standard IGraphicsDeviceService
    /// interface, which provides notification events for when the device is reset
    /// or disposed.
    /// </summary>
    public class GraphicsDeviceService : IGraphicsDeviceService
    {
        /// <summary>
        /// Keeps track of how many controls are sharing the singletonInstance
        /// </summary>
        static int _referenceCount;

        /// <summary>
        /// Singleton device service instance
        /// </summary>
        static GraphicsDeviceService _instance;

        /// <summary>
        /// Current device settings
        /// </summary>
        readonly PresentationParameters _pp;

        /// <summary>
        /// Graphics device
        /// </summary>
        GraphicsDevice graphicsDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsDeviceService"/> class.
        /// </summary>
        /// <param name="windowHandle">The window handle.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        GraphicsDeviceService(IntPtr windowHandle, int width, int height)
        {
            _pp = new PresentationParameters
            {
                BackBufferWidth = Math.Max(width, 1),
                BackBufferHeight = Math.Max(height, 1),
                BackBufferFormat = SurfaceFormat.Color,
                EnableAutoDepthStencil = true,
                AutoDepthStencilFormat = DepthFormat.Depth24
            };

            graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, DeviceType.Hardware, windowHandle, _pp);
        }

        /// <summary>
        /// Gets a reference to the singleton instance.
        /// </summary>
        public static GraphicsDeviceService AddRef(IntPtr windowHandle, int width, int height)
        {
            // Increment the "how many controls sharing the device" reference count.
            if (Interlocked.Increment(ref _referenceCount) == 1)
            {
                // If this is the first control to start using the
                // device, we must create the singleton instance.
                _instance = new GraphicsDeviceService(windowHandle, width, height);
            }

            return _instance;
        }

        /// <summary>
        /// Releases a reference to the singleton instance.
        /// </summary>
        public void Release(bool disposing)
        {
            // Decrement the "how many controls sharing the device" reference count.
            if (Interlocked.Decrement(ref _referenceCount) != 0)
                return;

            // If this is the last control to finish using the
            // device, we should dispose the singleton instance.
            if (disposing)
            {
                if (DeviceDisposing != null)
                    DeviceDisposing(this, EventArgs.Empty);

                graphicsDevice.Dispose();
            }

            graphicsDevice = null;
        }

        /// <summary>
        /// Resets the graphics device to whichever is bigger out of the specified
        /// resolution or its current size. This behavior means the device will
        /// demand-grow to the largest of all its GraphicsDeviceControl clients.
        /// </summary>
        public void ResetDevice(int width, int height)
        {
            if (DeviceResetting != null)
                DeviceResetting(this, EventArgs.Empty);

            // Set the backbuffer size to the specified sizes (if they are larger than the current size)
            _pp.BackBufferWidth = Math.Max(_pp.BackBufferWidth, width);
            _pp.BackBufferHeight = Math.Max(_pp.BackBufferHeight, height);

            // Reset the GraphicsDevice
            graphicsDevice.Reset(_pp);

            if (DeviceReset != null)
                DeviceReset(this, EventArgs.Empty);
        }

        #region IGraphicsDeviceService Members

        /// <summary>
        /// The event that occurs when a graphics device is created.
        /// </summary>
        public event EventHandler DeviceCreated;

        /// <summary>
        /// The event that occurs when a graphics device is disposing.
        /// </summary>
        public event EventHandler DeviceDisposing;

        /// <summary>
        /// The event that occurs when a graphics device is reset.
        /// </summary>
        public event EventHandler DeviceReset;

        /// <summary>
        /// The event that occurs when a graphics device is in the process of resetting.
        /// </summary>
        public event EventHandler DeviceResetting;

        /// <summary>
        /// Gets the current graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        #endregion
    }
}