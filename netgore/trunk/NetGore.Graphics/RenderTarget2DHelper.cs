using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    public static class RenderTarget2DHelper
    {
        public static Texture2D CreateTexture2D(GraphicsDevice device, int width, int height, Color backColor,
                                                Action<SpriteBatch> drawer)
        {
            const RenderTargetUsage usage = RenderTargetUsage.PreserveContents;
            const int mipMapLevels = 1;

            Texture2D ret;

            // Store the old graphics device values
            var oldDepthStencilBuffer = device.DepthStencilBuffer;
            var oldRenderTarget = device.GetRenderTarget(0) as RenderTarget2D;

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
                using (var sb = new SpriteBatch(device))
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