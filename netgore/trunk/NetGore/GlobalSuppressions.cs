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