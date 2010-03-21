using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Helper methods for the <see cref="RenderTarget2D"/>.
    /// </summary>
    public static class RenderTarget2DHelper
    {
        /// <summary>
        /// Creates a <see cref="Texture2D"/> using a <see cref="RenderTarget2D"/> to draw the contents.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        /// <param name="width">The texture width.</param>
        /// <param name="height">The texture height.</param>
        /// <param name="backColor">The color to fill the texture with before drawing.</param>
        /// <param name="drawer">The <see cref="Action{T}"/> that does all the drawing using the given
        /// <see cref="ISpriteBatch"/>.</param>
        /// <returns>The created <see cref="Texture2D"/>.</returns>
        public static Texture2D CreateTexture2D(GraphicsDevice device, int width, int height, Color backColor,
                                                Action<ISpriteBatch> drawer)
        {
            const RenderTargetUsage usage = RenderTargetUsage.PreserveContents;
            const int mipMapLevels = 1;

            Texture2D ret;

            // Store the old graphics device values
            var oldDepthStencilBuffer = device.DepthStencilBuffer;
            var oldRenderTarget = device.GetRenderTarget(0) as RenderTarget2D;

            // If the old render target is (for some odd reason) disposed, use null instead
            if (oldRenderTarget != null && oldRenderTarget.IsDisposed)
                oldRenderTarget = null;

            // Same goes for if the depth stencil buffer is disposed
            if (oldDepthStencilBuffer != null && oldDepthStencilBuffer.IsDisposed)
                oldDepthStencilBuffer = null;

            // Grab the values to use to create the render target
            var pp = device.PresentationParameters;
            SurfaceFormat format = pp.BackBufferFormat;
            MultiSampleType sample = pp.MultiSampleType;
            int q = pp.MultiSampleQuality;

            // Create the render target
            using (var target = new RenderTarget2D(device, width, height, mipMapLevels, format, sample, q, usage))
            {
                // Set the render target to the texture and clear it
                device.DepthStencilBuffer = null;
                device.SetRenderTarget(0, target);
                device.Clear(ClearOptions.Target, backColor, 1.0f, 0);

                // Create the SpriteBatch
                using (var sb = new RoundedXnaSpriteBatch(device))
                {
                    // Do the drawing
                    drawer(sb);
                }

                // Restore the render target and grab the created texture
                device.SetRenderTarget(0, oldRenderTarget);
                ret = target.GetTexture();
            }

            // Restore the device's values
            device.DepthStencilBuffer = oldDepthStencilBuffer;

            return ret;
        }
    }
}