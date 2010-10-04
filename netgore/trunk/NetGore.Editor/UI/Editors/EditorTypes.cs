using System;
using System.Linq;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// A <see cref="Type"/> pair for a type and the type of editor.
    /// </summary>
    public struct EditorTypes
    {
        readonly Type _editorType;
        readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorTypes"/> struct.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="editorType">Type of the editor.</param>
        public EditorTypes(Type type, Type editorType)
        {
            _type = type;
            _editorType = editorType;
        }

        /// <summary>
        /// Gets the type of the editor.
        /// </summary>
        public Type EditorType
        {
            get { return _editorType; }
        }

        /// <summary>
        /// Gets the type to add to.
        /// </summary>
        public Type Type
        {
            get { return _type; }
        }
    }
}