// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

using System.Diagnostics.CodeAnalysis;
using System.Linq;

[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources", Scope = "member",
        Target = "SFML.Graphics.Context.#myThis")]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member",
        Target = "SFML.Window.VideoMode.#FullscreenModes")]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#Samples")]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member",
        Target = "SFML.Graphics.Image.#Pixels")]
[assembly:
    SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#Initialize()")]
[assembly:
    SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#.ctor(System.UInt32,System.UInt32,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Scope = "type", Target = "SFML.Graphics.Text+Styles")]
[assembly: SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Scope = "type", Target = "SFML.Window.KeyCode")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Context.#sfContext_Create()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Context.#sfContext_SetActive(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Font.#sfFont_Copy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Font.#sfFont_CreateFromFile(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Font.#sfFont_CreateFromMemory(System.Char*,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Font.#sfFont_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Font.#sfFont_GetDefaultFont()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Font.#sfFont_GetGlyph(System.IntPtr,System.UInt32,System.UInt32,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Font.#sfFont_GetImage(System.IntPtr,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Font.#sfFont_GetKerning(System.IntPtr,System.UInt32,System.UInt32,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Font.#sfFont_GetLineSpacing(System.IntPtr,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_Bind(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_Copy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Image.#sfImage_CopyImage(System.IntPtr,System.IntPtr,System.UInt32,System.UInt32,SFML.Graphics.IntRect)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_CopyScreen(System.IntPtr,System.IntPtr,SFML.Graphics.IntRect)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_Create()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_CreateFromColor(System.UInt32,System.UInt32,SFML.Graphics.Color)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_CreateFromFile(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_CreateFromMemory(System.Char*,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_CreateFromPixels(System.UInt32,System.UInt32,System.Byte*)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_CreateMaskFromColor(System.IntPtr,SFML.Graphics.Color,System.Byte)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_GetHeight(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_GetPixel(System.IntPtr,System.UInt32,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_GetPixelsPtr(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_GetWidth(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_IsSmooth(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_SaveToFile(System.IntPtr,System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_SetPixel(System.IntPtr,System.UInt32,System.UInt32,SFML.Graphics.Color)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_SetSmooth(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Image.#sfImage_UpdatePixels(System.IntPtr,SFML.Graphics.Color*,SFML.Graphics.IntRect)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Input.#sfInput_GetJoystickAxis(System.IntPtr,System.UInt32,SFML.Window.JoyAxis)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Input.#sfInput_GetMouseX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Input.#sfInput_GetMouseY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Input.#sfInput_IsJoystickButtonDown(System.IntPtr,System.UInt32,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Input.#sfInput_IsKeyDown(System.IntPtr,SFML.Window.KeyCode)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Input.#sfInput_IsMouseButtonDown(System.IntPtr,SFML.Window.MouseButton)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Listener.#sfListener_GetDirection(System.Single&,System.Single&,System.Single&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Listener.#sfListener_GetGlobalVolume()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Listener.#sfListener_GetPosition(System.Single&,System.Single&,System.Single&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Listener.#sfListener_SetDirection(System.Single,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Listener.#sfListener_SetGlobalVolume(System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Listener.#sfListener_SetPosition(System.Single,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_CreateFromFile(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_CreateFromMemory(System.Char*,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetAttenuation(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetChannelsCount(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetDuration(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetLoop(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetMinDistance(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetPitch(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetPlayingOffset(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetPosition(System.IntPtr,System.Single&,System.Single&,System.Single&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetSampleRate(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetStatus(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_GetVolume(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_IsRelativeToListener(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_Pause(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_Play(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_SetAttenuation(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_SetLoop(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_SetMinDistance(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_SetPitch(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_SetPlayingOffset(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_SetPosition(System.IntPtr,System.Single,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_SetRelativeToListener(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_SetVolume(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Music.#sfMusic_Stop(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.PostFx.#sfPostFX_CanUsePostFX()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.PostFx.#sfPostFx_Create()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.PostFx.#sfPostFX_CreateFromFile(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.PostFx.#sfPostFX_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.PostFx.#sfPostFX_SetParameter1(System.IntPtr,System.String,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.PostFx.#sfPostFX_SetParameter2(System.IntPtr,System.String,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.PostFx.#sfPostFX_SetParameter3(System.IntPtr,System.String,System.Single,System.Single,System.Single)")
]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.PostFx.#sfPostFX_SetParameter4(System.IntPtr,System.String,System.Single,System.Single,System.Single,System.Single)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.PostFx.#sfPostFX_SetTexture(System.IntPtr,System.String,System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_Clear(System.IntPtr,SFML.Graphics.Color)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.RenderImage.#sfRenderImage_ConvertCoords(System.IntPtr,System.UInt32,System.UInt32,System.Single&,System.Single&,System.IntPtr)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_Create(System.UInt32,System.UInt32,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_Display(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_GetDefaultView(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_GetHeight(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_GetImage(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_GetViewport(System.IntPtr,System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_GetWidth(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_IsAvailable()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_RestoreGLStates(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_SaveGLStates(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_SetActive(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_SetView(System.IntPtr,System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_Clear(System.IntPtr,SFML.Graphics.Color)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_Close(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.RenderWindow.#sfRenderWindow_ConvertCoords(System.IntPtr,System.UInt32,System.UInt32,System.Single&,System.Single&,System.IntPtr)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.RenderWindow.#sfRenderWindow_Create(SFML.Window.VideoMode,System.String,SFML.Window.Styles,SFML.Window.ContextSettings&)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_CreateFromHandle(System.IntPtr,SFML.Window.ContextSettings&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_Display(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_EnableKeyRepeat(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_GetDefaultView(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_GetEvent(System.IntPtr,SFML.Window.Event&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_GetFrameTime(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_GetHeight(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_GetInput(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_GetSettings(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_GetSystemHandle(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_GetViewport(System.IntPtr,System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_GetWidth(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_IsOpened(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_RestoreGLStates(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_SaveGLStates(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_SetActive(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_SetCursorPosition(System.IntPtr,System.UInt32,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_SetFramerateLimit(System.IntPtr,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_SetIcon(System.IntPtr,System.UInt32,System.UInt32,System.Byte*)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_SetJoystickThreshold(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_SetPosition(System.IntPtr,System.Int32,System.Int32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_SetSize(System.IntPtr,System.UInt32,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_SetView(System.IntPtr,System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_Show(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_ShowMouseCursor(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_UseVerticalSync(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_WaitEvent(System.IntPtr,SFML.Window.Event&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_Bind(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_Copy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_Create()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_CreateFromFile(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_IsAvailable()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_SetParameter1(System.IntPtr,System.String,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_SetParameter2(System.IntPtr,System.String,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Shader.#sfShader_SetParameter3(System.IntPtr,System.String,System.Single,System.Single,System.Single)")
]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Shader.#sfShader_SetParameter4(System.IntPtr,System.String,System.Single,System.Single,System.Single,System.Single)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_SetTexture(System.IntPtr,System.String,System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_Unbind(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Shape.#sfShape_AddPoint(System.IntPtr,System.Single,System.Single,SFML.Graphics.Color,SFML.Graphics.Color)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_Copy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_Create()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Shape.#sfShape_CreateCircle(System.Single,System.Single,System.Single,SFML.Graphics.Color,System.Single,SFML.Graphics.Color)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Shape.#sfShape_CreateLine(System.Single,System.Single,System.Single,System.Single,System.Single,SFML.Graphics.Color,System.Single,SFML.Graphics.Color)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Shape.#sfShape_CreateRectangle(System.Single,System.Single,System.Single,System.Single,SFML.Graphics.Color,System.Single,SFML.Graphics.Color)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_EnableFill(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_EnableOutline(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetBlendMode(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetColor(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetOriginX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetOriginY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetOutlineWidth(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetPointColor(System.IntPtr,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetPointOutlineColor(System.IntPtr,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetPointPosition(System.IntPtr,System.UInt32,System.Single&,System.Single&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetPointsCount(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetRotation(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetScaleX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetScaleY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_GetY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_SetBlendMode(System.IntPtr,SFML.Graphics.BlendMode)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_SetColor(System.IntPtr,SFML.Graphics.Color)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_SetOrigin(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_SetOutlineWidth(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_SetPointColor(System.IntPtr,System.UInt32,SFML.Graphics.Color)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_SetPointOutlineColor(System.IntPtr,System.UInt32,SFML.Graphics.Color)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_SetPointPosition(System.IntPtr,System.UInt32,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_SetPosition(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_SetRotation(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shape.#sfShape_SetScale(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Shape.#sfShape_TransformToGlobal(System.IntPtr,System.Single,System.Single,System.Single&,System.Single&)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Shape.#sfShape_TransformToLocal(System.IntPtr,System.Single,System.Single,System.Single&,System.Single&)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_Copy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_Create()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_GetAttenuation(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_GetLoop(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_GetMinDistance(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_GetPitch(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_GetPlayingOffset(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_GetPosition(System.IntPtr,System.Single&,System.Single&,System.Single&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_GetStatus(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_GetVolume(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_IsRelativeToListener(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_Pause(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_Play(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_SetAttenuation(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_SetBuffer(System.IntPtr,System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_SetLoop(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_SetMinDistance(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_SetPitch(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_SetPlayingOffset(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_SetPosition(System.IntPtr,System.Single,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_SetRelativeToListener(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_SetVolume(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.Sound.#sfSound_Stop(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#sfSoundBuffer_Copy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#sfSoundBuffer_CreateFromFile(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#sfSoundBuffer_CreateFromMemory(System.Char*,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Audio.SoundBuffer.#sfSoundBuffer_CreateFromSamples(System.Int16*,System.UInt32,System.UInt32,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#sfSoundBuffer_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#sfSoundBuffer_GetChannelsCount(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#sfSoundBuffer_GetDuration(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#sfSoundBuffer_GetSampleRate(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#sfSoundBuffer_GetSamples(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#sfSoundBuffer_GetSamplesCount(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundBuffer.#sfSoundBuffer_SaveToFile(System.IntPtr,System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Audio.SoundRecorder.#sfSoundRecorder_Create(SFML.Audio.SoundRecorder+StartCallback,SFML.Audio.SoundRecorder+ProcessCallback,SFML.Audio.SoundRecorder+StopCallback,System.IntPtr)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundRecorder.#sfSoundRecorder_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundRecorder.#sfSoundRecorder_GetSampleRate(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundRecorder.#sfSoundRecorder_IsAvailable()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundRecorder.#sfSoundRecorder_Start(System.IntPtr,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundRecorder.#sfSoundRecorder_Stop(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Audio.SoundStream.#sfSoundStream_Create(SFML.Audio.SoundStream+GetDataCallbackType,SFML.Audio.SoundStream+SeekCallbackType,System.UInt32,System.UInt32,System.IntPtr)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_GetAttenuation(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_GetChannelsCount(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_GetLoop(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_GetMinDistance(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_GetPitch(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_GetPlayingOffset(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_GetPosition(System.IntPtr,System.Single&,System.Single&,System.Single&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_GetSampleRate(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_GetStatus(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_GetVolume(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_IsRelativeToListener(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_Pause(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_Play(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_SetAttenuation(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_SetLoop(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_SetMinDistance(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_SetPitch(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_SetPlayingOffset(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_SetPosition(System.IntPtr,System.Single,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_SetRelativeToListener(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_SetVolume(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Audio.SoundStream.#sfSoundStream_Stop(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_Copy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_Create()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_FlipX(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_FlipY(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetBlendMode(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetColor(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetHeight(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetOriginX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetOriginY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetPixel(System.IntPtr,System.UInt32,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetRotation(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetScaleX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetScaleY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetSubRect(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetWidth(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_GetY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_Resize(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_SetBlendMode(System.IntPtr,SFML.Graphics.BlendMode)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_SetColor(System.IntPtr,SFML.Graphics.Color)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_SetImage(System.IntPtr,System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_SetOrigin(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_SetPosition(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_SetRotation(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_SetScale(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Sprite.#sfSprite_SetSubRect(System.IntPtr,SFML.Graphics.IntRect)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Sprite.#sfSprite_TransformToGlobal(System.IntPtr,System.Single,System.Single,System.Single&,System.Single&)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Sprite.#sfSprite_TransformToLocal(System.IntPtr,System.Single,System.Single,System.Single&,System.Single&)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_Copy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_Create()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetBlendMode(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetCharacterPos(System.IntPtr,System.UInt32,System.Single&,System.Single&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetCharacterSize(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetColor(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetOriginX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetOriginY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetRect(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetRotation(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetScaleX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetScaleY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetString(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetStyle(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_GetY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_SetBlendMode(System.IntPtr,SFML.Graphics.BlendMode)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_SetCharacterSize(System.IntPtr,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_SetColor(System.IntPtr,SFML.Graphics.Color)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_SetFont(System.IntPtr,System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_SetOrigin(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_SetPosition(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_SetRotation(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_SetScale(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_SetString(System.IntPtr,System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Text.#sfText_SetStyle(System.IntPtr,SFML.Graphics.Text+Styles)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Text.#sfText_TransformToGlobal(System.IntPtr,System.Single,System.Single,System.Single&,System.Single&)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Graphics.Text.#sfText_TransformToLocal(System.IntPtr,System.Single,System.Single,System.Single&,System.Single&)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.VideoMode.#sfVideoMode_GetDesktopMode()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.VideoMode.#sfVideoMode_GetFullscreenModes(System.UInt32&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.VideoMode.#sfVideoMode_IsValid(SFML.Window.VideoMode)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_Copy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_Create()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_CreateFromRect(SFML.Graphics.FloatRect)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_GetCenterX(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_GetCenterY(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_GetHeight(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_GetRotation(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_GetViewport(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_GetWidth(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_Move(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_Reset(System.IntPtr,SFML.Graphics.FloatRect)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_Rotate(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_SetCenter(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_SetRotation(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_SetSize(System.IntPtr,System.Single,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_SetViewport(System.IntPtr,SFML.Graphics.FloatRect)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.View.#sfView_Zoom(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_Close(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "SFML.Window.Window.#sfWindow_Create(SFML.Window.VideoMode,System.String,SFML.Window.Styles,SFML.Window.ContextSettings&)"
        )]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_CreateFromHandle(System.IntPtr,SFML.Window.ContextSettings&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_Destroy(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_Display(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_EnableKeyRepeat(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_GetEvent(System.IntPtr,SFML.Window.Event&)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_GetFrameTime(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_GetHeight(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_GetInput(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_GetSettings(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_GetSystemHandle(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_GetWidth(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_IsOpened(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_SetActive(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_SetCursorPosition(System.IntPtr,System.UInt32,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_SetFramerateLimit(System.IntPtr,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_SetIcon(System.IntPtr,System.UInt32,System.UInt32,System.Byte*)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_SetJoystickThreshold(System.IntPtr,System.Single)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_SetPosition(System.IntPtr,System.Int32,System.Int32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_SetSize(System.IntPtr,System.UInt32,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_Show(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_ShowMouseCursor(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_UseVerticalSync(System.IntPtr,System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Window.Window.#sfWindow_WaitEvent(System.IntPtr,SFML.Window.Event&)")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target = "SFML.Graphics.Image.#CopyScreen(SFML.Graphics.RenderWindow,SFML.Graphics.IntRect)")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member",
        Target = "SFML.Graphics.Image.#Copy(SFML.Graphics.Image,System.UInt32,System.UInt32,SFML.Graphics.IntRect)")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1012:AbstractTypesShouldNotHaveConstructors", Scope = "type",
        Target = "SFML.Audio.SoundRecorder")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1012:AbstractTypesShouldNotHaveConstructors", Scope = "type",
        Target = "SFML.Audio.SoundStream")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Scope = "member",
        Target =
            "SFML.LoadingFailedException.#.ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type",
        Target = "SFML.Graphics.Text+Styles")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Scope = "type",
        Target = "NetGore.Content.ContentAssetName")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Scope = "type",
        Target = "NetGore.IO.PathString")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Scope = "type",
        Target = "NetGore.IO.SpriteCategory")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Scope = "type",
        Target = "NetGore.IO.SpriteTitle")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Scope = "type",
        Target = "NetGore.Stats.StatValueType")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.BoundingBox.#Max")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.BoundingBox.#Min")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.BoundingSphere.#Center")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.BoundingSphere.#Radius")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.ContextSettings.#AntialiasingLevel")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.ContextSettings.#DepthBits")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.ContextSettings.#MajorVersion")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.ContextSettings.#MinorVersion")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.ContextSettings.#StencilBits")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.Event.#JoyButton")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.Event.#JoyMove")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.Event.#Key")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.Event.#MouseButton")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.Event.#MouseMove")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.Event.#MouseWheel")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.Event.#Size")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.Event.#Text")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.Event.#Type")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.FloatRect.#Height")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.FloatRect.#Left")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.FloatRect.#Top")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.FloatRect.#Width")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Glyph.#Advance")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Glyph.#Rectangle")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Glyph.#TexCoords")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.IntRect.#Height")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.IntRect.#Left")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.IntRect.#Top")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.IntRect.#Width")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.JoyButtonEvent.#Button")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.JoyButtonEvent.#JoystickId")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.JoystickButtonEventArgs.#Button")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.JoystickButtonEventArgs.#JoystickId")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.JoyMoveEvent.#Axis")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.JoyMoveEvent.#JoystickId")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.JoyMoveEvent.#Position")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.JoystickMoveEventArgs.#Axis")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.JoystickMoveEventArgs.#JoystickId")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.JoystickMoveEventArgs.#Position")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.KeyEvent.#Alt")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.KeyEvent.#Code")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.KeyEvent.#Control")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.KeyEvent.#Shift")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.KeyEventArgs.#Alt")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.KeyEventArgs.#Code")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.KeyEventArgs.#Control")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.KeyEventArgs.#Shift")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Design.MathTypeConverter.#propertyDescriptions")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Design.MathTypeConverter.#supportStringConvert")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M11")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M12")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M13")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M14")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M21")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M22")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M23")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M24")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M31")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M32")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M33")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M34")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M41")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M42")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M43")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Matrix.#M44")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseButtonEvent.#Button")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseButtonEvent.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseButtonEvent.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseButtonEventArgs.#Button")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseButtonEventArgs.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseButtonEventArgs.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseMoveEvent.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseMoveEvent.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseMoveEventArgs.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseMoveEventArgs.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseWheelEvent.#Delta")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseWheelEvent.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseWheelEvent.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseWheelEventArgs.#Delta")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseWheelEventArgs.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.MouseWheelEventArgs.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Plane.#D")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Plane.#Normal")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Point.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Point.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Quaternion.#W")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Quaternion.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Quaternion.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Quaternion.#Z")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Ray.#Direction")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Ray.#Position")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Rectangle.#Height")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Rectangle.#Width")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Rectangle.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Rectangle.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.SizeEventArgs.#Width")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.SizeEventArgs.#Height")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.SizeEvent.#Width")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.SizeEvent.#Height")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.TextEvent.#Unicode")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.TextEventArgs.#Unicode")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Vector2.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Vector2.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Vector3.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Vector3.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Vector3.#Z")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Vector4.#W")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Vector4.#X")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Vector4.#Y")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Graphics.Vector4.#Z")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.VideoMode.#BitsPerPixel")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.VideoMode.#Height")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.VideoMode.#Width")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.Window.#myInput")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.WindowSettings.#AntialiasingLevel")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.WindowSettings.#DepthBits")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member",
        Target = "SFML.Window.WindowSettings.#StencilBits")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Scope = "type",
        Target = "SFML.Audio.Listener")]
[assembly:
    SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_Create()")]
[assembly:
    SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Scope = "member",
        Target = "SFML.Graphics.BoundingBox.#CreateFromPoints(System.Collections.Generic.IEnumerable`1<SFML.Graphics.Vector3>)")]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "0#", Scope = "member",
        Target = "SFML.Graphics.Image.#UpdatePixels(SFML.Graphics.Color[,])")]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "0#", Scope = "member",
        Target = "SFML.Graphics.Image.#UpdatePixels(SFML.Graphics.Color[,],System.UInt32,System.UInt32)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderImage.#sfRenderImage_GetView(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.RenderWindow.#sfRenderWindow_GetView(System.IntPtr)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target = "SFML.Graphics.Shader.#sfShader_CreateFromMemory(System.String)")]