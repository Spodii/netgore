using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using NetGore.Db;

namespace DemoGame.EditorTools
{
    /// <summary>
    /// Helper methods for the custom <see cref="UITypeEditor"/>s.
    /// </summary>
    public static class CustomUITypeEditors
    {
        static bool _added = false;
        static IDbController _dbController;

        /// <summary>
        /// Gets the <see cref="IDbController"/> that was used when calling AddEditors.
        /// </summary>
        internal static IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Adds all of the custom <see cref="UITypeEditor"/>s.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        public static void AddEditors(IDbController dbController)
        {
            if (dbController != null)
                _dbController = dbController;

            if (_added)
                return;

            _added = true;

            AddEditorsHelper(new EditorTypes(typeof(CharacterTemplateID), typeof(CharacterTemplateIDEditor)),
                             new EditorTypes(typeof(ItemTemplateID), typeof(ItemTemplateIDEditor)));
        }

        /// <summary>
        /// Adds the <see cref="EditorAttribute"/>s for the given <see cref="Type"/>s.
        /// </summary>
        /// <param name="items">The <see cref="Type"/>s and <see cref="Type"/> of the editor.</param>
        static void AddEditorsHelper(params EditorTypes[] items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                var attrib = new EditorAttribute(item.EditorType, typeof(UITypeEditor));
                TypeDescriptor.AddAttributes(item.Type, attrib);
            }
        }

        /// <summary>
        /// A <see cref="Type"/> pair for a type and the type of editor.
        /// </summary>
        struct EditorTypes
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
}