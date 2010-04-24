using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace SFML
{
    namespace Graphics
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Font is the low-level class for loading and
        /// manipulating character fonts. This class is meant to
        /// be used by String2D
        /// </summary>
        ////////////////////////////////////////////////////////////
        public class Font : ObjectBase
        {
            ////////////////////////////////////////////////////////////
            static Font ourDefaultFont = null;

            #region Imports

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern IntPtr sfFont_Create();

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern IntPtr sfFont_CreateFromFile(string Filename, uint CharSize, IntPtr Charset);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern unsafe IntPtr sfFont_CreateFromMemory(char* Data, uint SizeInBytes, uint CharSize, IntPtr Charset);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern void sfFont_Destroy(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern uint sfFont_GetCharacterSize(IntPtr This);

            [DllImport("csfml-graphics", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern IntPtr sfFont_GetDefaultFont();

            #endregion

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Construct the font from a file, using custom size
            /// </summary>
            /// <param name="filename">Font file to load</param>
            /// <param name="charSize">Character size</param>
            /// <exception cref="LoadingFailedException" />
            ////////////////////////////////////////////////////////////
            public Font(string filename, uint charSize = 30u) : this(filename, charSize, string.Empty)
            {
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Construct the font from a file, using custom size and characters set
            /// </summary>
            /// <param name="filename">Font file to load</param>
            /// <param name="charSize">Character size</param>
            /// <param name="charset">Set of characters to generate</param>
            /// <exception cref="LoadingFailedException" />
            ////////////////////////////////////////////////////////////
            public Font(string filename, uint charSize, string charset) : base(IntPtr.Zero)
            {
                EnsureLoaded(filename, charSize, charset);
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Construct the font from a file in a stream
            /// </summary>
            /// <param name="stream">Stream containing the file contents</param>
            /// <param name="charSize">Character size</param>
            /// <param name="charset">Set of characters to generate</param>
            /// <exception cref="LoadingFailedException" />
            ////////////////////////////////////////////////////////////
            public Font(Stream stream, uint charSize = 30u, string charset = "") : base(IntPtr.Zero)
            {
                unsafe
                {
                    IntPtr ptr;
                    int size;
                    if (Int32.TryParse(charset, out size))
                        ptr = new IntPtr(&size);
                    else
                        ptr = IntPtr.Zero;

                    stream.Position = 0;
                    var StreamData = new byte[stream.Length];
                    var Read = (uint)stream.Read(StreamData, 0, StreamData.Length);
                    fixed (byte* dataPtr = StreamData)
                    {
                        SetThis(sfFont_CreateFromMemory((char*)dataPtr, Read, charSize, ptr));
                    }
                }

                // ReSharper disable DoNotCallOverridableMethodsInConstructor
                if (This == IntPtr.Zero)
                    throw new LoadingFailedException("font");
                // ReSharper restore DoNotCallOverridableMethodsInConstructor
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Image"/> class.
            /// </summary>
            protected internal Font() : base(IntPtr.Zero)
            {
            }

            /// <summary>
            /// Internal constructor
            /// </summary>
            /// <param name="thisPtr">Pointer to the object in C library</param>
            ////////////////////////////////////////////////////////////
            Font(IntPtr thisPtr) : base(thisPtr)
            {
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Base character size
            /// </summary>
            ////////////////////////////////////////////////////////////
            public uint CharacterSize
            {
                get { return sfFont_GetCharacterSize(This); }
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Default built-in font
            /// </summary>
            ////////////////////////////////////////////////////////////
            public static Font DefaultFont
            {
                get { return ourDefaultFont ?? (ourDefaultFont = new Font(sfFont_GetDefaultFont())); }
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Handle the destruction of the object
            /// </summary>
            /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
            ////////////////////////////////////////////////////////////
            protected override void Destroy(bool disposing)
            {
                if (this != ourDefaultFont)
                {
                    if (!disposing)
                        Context.Global.SetActive(true);

                    sfFont_Destroy(This);

                    if (!disposing)
                        Context.Global.SetActive(false);
                }
            }

            /// <summary>
            /// Reloads the asset from file if it is not loaded.
            /// </summary>
            /// <param name="filename">Font file to load</param>
            /// <param name="charSize">Character size</param>
            /// <param name="charset">Set of characters to generate</param>
            /// <returns>True if already loaded; false if it had to reload.</returns>
            /// <exception cref="LoadingFailedException"/>
            protected internal bool EnsureLoaded(string filename, uint charSize, string charset)
            {
                if (ThisRaw != IntPtr.Zero)
                    return true;

                unsafe
                {
                    IntPtr ptr;
                    int size;
                    if (Int32.TryParse(charset, out size))
                        ptr = new IntPtr(&size);
                    else
                        ptr = IntPtr.Zero;

                    SetThis(sfFont_CreateFromFile(filename, charSize, ptr));
                }

                if (ThisRaw == IntPtr.Zero)
                    throw new LoadingFailedException("font", filename);

                return false;
            }

            ////////////////////////////////////////////////////////////
        }
    }
}