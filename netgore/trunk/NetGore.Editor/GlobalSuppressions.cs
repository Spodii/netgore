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
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "pane", Scope = "member",
        Target =
            "NetGore.Editor.Docking.DockContentHandler.#DockTo(NetGore.Editor.Docking.DockPanel,System.Windows.Forms.DockStyle)")]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "pane", Scope = "member",
        Target = "NetGore.Editor.Docking.DockContentHandler.#FloatAt(System.Drawing.Rectangle)")]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "activeContent", Scope = "member",
        Target = "NetGore.Editor.Docking.DockPane.#RestoreToPanel()")]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "content", Scope = "member",
        Target = "NetGore.Editor.Docking.DockPanel.#DocumentsCount")]
[assembly:
    SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastDocument", Scope = "member",
        Target =
            "NetGore.Editor.Docking.DockPanel+Persistor.#LoadFromXml(NetGore.Editor.Docking.DockPanel,System.IO.Stream,NetGore.Editor.Docking.DeserializeDockContent,System.Boolean)"
        )]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type",
        Target = "NetGore.Editor.Docking.DockPane+AppearanceStyle")]