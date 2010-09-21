using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace SFML
{
    namespace Graphics
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Simple wrapper for Window that allows easy
        /// 2D rendering
        /// </summary>
        ////////////////////////////////////////////////////////////
        public class RenderImage : ObjectBase, RenderTarget
        {
            ////////////////////////////////////////////////////////////
            readonly View myDefaultView = null;
            readonly Image myImage = null;
            View myCurrentView = null;

            #region Imports

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern void sfRenderImage_Clear(IntPtr This, Color ClearColor);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern void sfRenderImage_ConvertCoords(IntPtr This, uint WindowX, uint WindowY, out float ViewX,
                                                           out float ViewY, IntPtr TargetView);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern IntPtr sfRenderImage_Create(uint Width, uint Height, bool DepthBuffer);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern void sfRenderImage_Destroy(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern bool sfRenderImage_Display(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern IntPtr sfRenderImage_GetDefaultView(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern uint sfRenderImage_GetHeight(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern IntPtr sfRenderImage_GetImage(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern IntPtr sfRenderImage_GetView(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern IntRect sfRenderImage_GetViewport(IntPtr This, IntPtr TargetView);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern uint sfRenderImage_GetWidth(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern bool sfRenderImage_IsAvailable();

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern bool sfRenderImage_RestoreGLStates(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern bool sfRenderImage_SaveGLStates(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern bool sfRenderImage_SetActive(IntPtr This, bool Active);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern void sfRenderImage_SetView(IntPtr This, IntPtr View);

            #endregion

            /// <summary>
            /// Create the render image with the given dimensions
            /// </summary>
            /// <param name="width">Width of the render image</param>
            /// <param name="height">Height of the render image</param>
            ////////////////////////////////////////////////////////////
            public RenderImage(uint width, uint height) : this(width, height, false)
            {
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Create the render image with the given dimensions and
            /// an optional depth-buffer attached
            /// </summary>
            /// <param name="width">Width of the render image</param>
            /// <param name="height">Height of the render image</param>
            /// <param name="depthBuffer">Do you want a depth-buffer attached?</param>
            ////////////////////////////////////////////////////////////
            public RenderImage(uint width, uint height, bool depthBuffer) : base(sfRenderImage_Create(width, height, depthBuffer))
            {
                myDefaultView = new View(sfRenderImage_GetDefaultView(This));
                myImage = new Image(sfRenderImage_GetImage(This));
                myCurrentView = myDefaultView;
                GC.SuppressFinalize(myDefaultView);
                GC.SuppressFinalize(myImage);
            }

            /// <summary>
            /// Target image of the render image
            /// </summary>
            ////////////////////////////////////////////////////////////
            public Image Image
            {
                get { return myImage; }
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Tell whether or not the system supports render images
            /// </summary>
            ////////////////////////////////////////////////////////////
            public static bool IsAvailable
            {
                get { return sfRenderImage_IsAvailable(); }
            }

            /// <summary>
            /// Handle the destruction of the object
            /// </summary>
            /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
            ////////////////////////////////////////////////////////////
            protected override void Destroy(bool disposing)
            {
                if (!disposing)
                    Context.Global.SetActive(true);

                sfRenderImage_Destroy(This);

                if (disposing)
                {
                    // NOTE: myDefaultView disposal removed since it seemed to keep causing an AccessViolationException when calling Dipose(true)
                    //myDefaultView.Dispose();
                    myImage.Dispose();
                }

                if (!disposing)
                    Context.Global.SetActive(false);
            }

            /// <summary>
            /// Update the contents of the target image
            /// </summary>
            ////////////////////////////////////////////////////////////
            public void Display()
            {
                sfRenderImage_Display(This);
            }

            ////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Activate of deactivate the render image as the current target
            /// for rendering
            /// </summary>
            /// <param name="active">True to activate, false to deactivate (true by default)</param>
            /// <returns>True if operation was successful, false otherwise</returns>
            ////////////////////////////////////////////////////////////
            public bool SetActive(bool active)
            {
                return sfRenderImage_SetActive(This, active);
            }

            /// <summary>
            /// Provide a string describing the object
            /// </summary>
            /// <returns>String description of the object</returns>
            ////////////////////////////////////////////////////////////
            public override string ToString()
            {
                return "[RenderImage]" + " Width(" + Width + ")" + " Height(" + Height + ")" + " Image(" + Image + ")" +
                       " DefaultView(" + DefaultView + ")" + " CurrentView(" + CurrentView + ")";
            }

            ////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////

            #region RenderTarget Members

            /// <summary>
            /// Current view active in the render image
            /// </summary>
            ////////////////////////////////////////////////////////////
            public View CurrentView
            {
                get { return myCurrentView; }
                set
                {
                    myCurrentView = value;
                    sfRenderImage_SetView(This, value.This);
                }
            }

            /// <summary>
            /// Default view of the render image
            /// </summary>
            ////////////////////////////////////////////////////////////
            public View DefaultView
            {
                get { return myDefaultView; }
            }

            /// <summary>
            /// Height of the rendering region of the image
            /// </summary>
            ////////////////////////////////////////////////////////////
            public uint Height
            {
                get { return sfRenderImage_GetHeight(This); }
            }

            /// <summary>
            /// Width of the rendering region of the image
            /// </summary>
            ////////////////////////////////////////////////////////////
            public uint Width
            {
                get { return sfRenderImage_GetWidth(This); }
            }

            ////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Clear the entire render image with black color
            /// </summary>
            ////////////////////////////////////////////////////////////
            public void Clear()
            {
                sfRenderImage_Clear(This, Color.Black);
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Clear the entire render image with a single color
            /// </summary>
            /// <param name="color">Color to use to clear the image</param>
            ////////////////////////////////////////////////////////////
            public void Clear(Color color)
            {
                sfRenderImage_Clear(This, color);
            }

            /// <summary>
            /// Convert a point in target coordinates into view coordinates
            /// This version uses the current view of the window
            /// </summary>
            /// <param name="x">X coordinate of the point to convert, relative to the target</param>
            /// <param name="y">Y coordinate of the point to convert, relative to the target</param>
            /// <returns>Converted point</returns>
            ///
            ////////////////////////////////////////////////////////////
            public Vector2 ConvertCoords(uint x, uint y)
            {
                return ConvertCoords(x, y, CurrentView);
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Convert a point in target coordinates into view coordinates
            /// This version uses the given view
            /// </summary>
            /// <param name="x">X coordinate of the point to convert, relative to the target</param>
            /// <param name="y">Y coordinate of the point to convert, relative to the target</param>
            /// <param name="view">Target view to convert the point to</param>
            /// <returns>Converted point</returns>
            ///
            ////////////////////////////////////////////////////////////
            public Vector2 ConvertCoords(uint x, uint y, View view)
            {
                Vector2 point;
                sfRenderImage_ConvertCoords(This, x, y, out point.X, out point.Y, view.This);

                return point;
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Draw something into the render image
            /// </summary>
            /// <param name="objectToDraw">Object to draw</param>
            ////////////////////////////////////////////////////////////
            public void Draw(Drawable objectToDraw)
            {
                objectToDraw.Render(this, null);
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Draw something into the render image with a shader
            /// </summary>
            /// <param name="objectToDraw">Object to draw</param>
            /// <param name="shader">Shader to apply</param>
            ////////////////////////////////////////////////////////////
            public void Draw(Drawable objectToDraw, Shader shader)
            {
                objectToDraw.Render(this, shader);
            }

            /// <summary>
            /// Get the viewport of a view applied to this target
            /// </summary>
            /// <param name="view">Target view</param>
            /// <returns>Viewport rectangle, expressed in pixels in the current target</returns>
            ////////////////////////////////////////////////////////////
            public IntRect GetViewport(View view)
            {
                return sfRenderImage_GetViewport(This, view.This);
            }

            ////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Restore the previously saved OpenGL render states and matrices
            /// </summary>
            ////////////////////////////////////////////////////////////
            public void RestoreGLStates()
            {
                sfRenderImage_RestoreGLStates(This);
            }

            /// <summary>
            /// Save the current OpenGL render states and matrices
            /// </summary>
            ////////////////////////////////////////////////////////////
            public void SaveGLStates()
            {
                sfRenderImage_SaveGLStates(This);
            }

            #endregion

            ////////////////////////////////////////////////////////////
        }
    }
}