using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Db;
using NetGore.EditorTools;

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

            NetGore.EditorTools.CustomUITypeEditors.AddEditorsHelper(
                new EditorTypes(typeof(CharacterTemplateID), typeof(CharacterTemplateIDEditor)),
                new EditorTypes(typeof(ItemTemplateID), typeof(ItemTemplateIDEditor)),
                new EditorTypes(typeof(AllianceID), typeof(AllianceIDEditor)),
                new EditorTypes(typeof(MapIndex), typeof(MapIndexEditor)),
                new EditorTypes(typeof(IEnumerable<KeyValuePair<StatType, int>>), typeof(StatTypeConstDictionaryEditor)),
                new EditorTypes(typeof(StatTypeConstDictionary), typeof(StatTypeConstDictionaryEditor)));

            TypeDescriptor.AddAttributes(typeof(IEnumerable<KeyValuePair<StatType, int>>),
                                         new TypeConverterAttribute(typeof(StatTypeConstDictionaryTypeConverter)));

            NetGore.EditorTools.CustomUITypeEditors.AddEditors();
        }
    }
}