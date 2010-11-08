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
    SuppressMessage("Microsoft.Usage", "CA2215:Dispose methods should call base class dispose", Scope = "member",
        Target = "NetGore.Editor.Docking.DockPanel+FocusManagerImpl.#Dispose(System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources", Scope = "member",
        Target = "NetGore.Editor.Docking.DockPanel+FocusManagerImpl+LocalWindowsHook.#m_hHook")]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Scope = "member",
        Target = "NetGore.Editor.Docking.DockPanel+MdiClientController.#Dispose(System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Scope = "member",
        Target = "NetGore.Editor.Docking.DockPanel+FocusManagerImpl.#Dispose(System.Boolean)")]
[assembly:
    SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Scope = "member",
        Target = "NetGore.Editor.Docking.DockPanel+DragHandlerBase.#BeginDrag()")]