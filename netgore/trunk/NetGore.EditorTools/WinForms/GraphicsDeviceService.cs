using System;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using NetGore;

// The IGraphicsDeviceService interface requires a DeviceCreated event, but we
// always just create the device inside our constructor, so we have no place to
// raise that event. The C# compiler warns us that the event is never used, but
// we don't care so we just disable this warning.
#pragma warning disable 67

namespace NetGore.EditorTools.WinForms
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
        static int referenceCount;

        /// <summary>
        /// Singleton device service instance
        /// </summary>
        static GraphicsDeviceService singletonInstance;

        /// <summary>
        /// Current device settings
        /// </summary>
        readonly PresentationParameters pp;

        /// <summary>
        /// Graphics device
        /// </summary>
        GraphicsDevice graphicsDevice;

        /// <summary>
        /// Constructor is private, because this is a singleton class:
        /// client controls should use the public AddRef method instead.
        /// </summary>
        GraphicsDeviceService(IntPtr windowHandle, int width, int height)
        {
            pp = new PresentationParameters();

            pp.BackBufferWidth = Math.Max(width, 1);
            pp.BackBufferHeight = Math.Max(height, 1);
            pp.BackBufferFormat = SurfaceFormat.Color;

            pp.EnableAutoDepthStencil = true;
            pp.AutoDepthStencilFormat = DepthFormat.Depth24;

            graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, DeviceType.Hardware, windowHandle, pp);
        }

        /// <summary>
        /// Gets a reference to the singleton instance.
        /// </summary>
        public static GraphicsDeviceService AddRef(IntPtr windowHandle, int width, int height)
        {
            // Increment the "how many controls sharing the device" reference count.
            if (Interlocked.Increment(ref referenceCount) == 1)
            {
                // If this is the first control to start using the
                // device, we must create the singleton instance.
                singletonInstance = new GraphicsDeviceService(windowHandle, width, height);
            }
            return singletonInstance;
        }

        /// <summary>
        /// Releases a reference to the singleton instance.
        /// </summary>
        public void Release(bool disposing)
        {
            // Decrement the "how many controls sharing the device" reference count.
            if (Interlocked.Decrement(ref referenceCount) == 0)
            {
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
            pp.BackBufferWidth = Math.Max(pp.BackBufferWidth, width);
            pp.BackBufferHeight = Math.Max(pp.BackBufferHeight, height);

            // Reset the GraphicsDevice
            graphicsDevice.Reset(pp);

            if (DeviceReset != null)
                DeviceReset(this, EventArgs.Empty);
        }

        #region IGraphicsDeviceService Members

        public event EventHandler DeviceCreated;
        public event EventHandler DeviceDisposing;
        public event EventHandler DeviceReset;
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