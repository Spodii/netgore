using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Helper members for stuff related to the editors.
    /// </summary>
    public static class EditorHelper
    {
        /// <summary>
        /// The fully qualified name for the ParticleModifierCollectionEditor.
        /// </summary>
        public const string ParticleModifierCollectionEditorTypeName =
            "NetGore.EditorTools.ParticleModifierCollectionEditor, NetGore.EditorTools";

        /// <summary>
        /// The fully qualified type name for the UITypeEditor.
        /// </summary>
        public const string UITypeEditorTypeName =
            "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

        /// <summary>
        /// The fully qualified type name for the XnaColorEditor.
        /// </summary>
        public const string XnaColorEditorTypeName = "NetGore.EditorTools.XnaColorEditor, NetGore.EditorTools";
    }
}