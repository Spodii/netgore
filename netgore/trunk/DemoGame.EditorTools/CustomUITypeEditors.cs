using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using DemoGame.Client.NPCChat;
using DemoGame.DbObjs;
using DemoGame.Server;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;
using NetGore.EditorTools;
using NetGore.Features.Shops;

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
        /// Adds the advanced class type converters and sets 
        /// </summary>
        static void AddAdvancedClassTypeConverters()
        {
            // Add the types we want to have use the AdvancedClassTypeConverter. This is purely just for
            // a better PropertyGrid-based editing experience. Since it doesn't really matter if we add too many
            // types (since it is only really for the PropertyGrid), we just add EVERY table from the DbObjs.
            var filterCreator = new TypeFilterCreator
            { IsClass = true, CustomFilter = (x => x.Name.EndsWith("Table") && x.Namespace.Contains("DbObjs")) };
            var filter = filterCreator.GetFilter();

            var dbObjsTypes = TypeHelper.FindTypes(filter, Type.EmptyTypes);

            AdvancedClassTypeConverter.AddTypes(dbObjsTypes.ToArray());

            // Set the properties we want to force being readonly in the PropertyGrid
            AdvancedClassTypeConverter.SetForceReadOnlyProperties(typeof(CharacterTemplateTable), "ID");
            AdvancedClassTypeConverter.SetForceReadOnlyProperties(typeof(ItemTemplateTable), "ID");

            // Set the UITypeEditor for specific properties on classes instead of every property with a certain type
            AdvancedClassTypeConverter.SetForceEditor(typeof(ItemTemplateTable),
                                                      new KeyValuePair<string, UITypeEditor>("EquippedBody",
                                                                                             new BodyPaperDollTypeEditor()));
        }

        /// <summary>
        /// Adds all of the custom <see cref="UITypeEditor"/>s.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        public static void AddEditors(IDbController dbController)
        {
            // Set the db controller only if we weren't given a null one
            if (dbController != null)
                _dbController = dbController;

            // If we already added the custom editors, don't add them again
            if (_added)
                return;

            _added = true;

            // Add all of our custom UITypeEditors. Note that for value types, we have to make a call for both the
            // regular type and nullable type if we want to support nullable types. It is important that the
            // UITypeEditor, too, also supports the nullable type.
            NetGore.EditorTools.CustomUITypeEditors.AddEditorsHelper(
                new EditorTypes(typeof(CharacterTemplateID), typeof(CharacterTemplateIDEditor)),
                new EditorTypes(typeof(CharacterTemplateID?), typeof(CharacterTemplateIDEditor)),
                new EditorTypes(typeof(ItemTemplateID), typeof(ItemTemplateIDEditor)),
                new EditorTypes(typeof(ItemTemplateID?), typeof(ItemTemplateIDEditor)),
                new EditorTypes(typeof(AllianceID), typeof(AllianceIDEditor)),
                new EditorTypes(typeof(AllianceID?), typeof(AllianceIDEditor)),
                new EditorTypes(typeof(BodyIndex), typeof(BodyIndexEditor)),
                new EditorTypes(typeof(BodyIndex?), typeof(BodyIndexEditor)), new EditorTypes(typeof(BodyInfo), typeof(BodyInfo)),
                new EditorTypes(typeof(MapIndex), typeof(MapIndexEditor)),
                new EditorTypes(typeof(MapIndex?), typeof(MapIndexEditor)), new EditorTypes(typeof(ShopID), typeof(ShopIDEditor)),
                new EditorTypes(typeof(ShopID?), typeof(ShopIDEditor)),
                new EditorTypes(typeof(IEnumerable<KeyValuePair<StatType, int>>), typeof(StatTypeConstDictionaryEditor)),
                new EditorTypes(typeof(StatTypeConstDictionary), typeof(StatTypeConstDictionaryEditor)));

            // Add a TypeConverter for the type:
            //      IEnumerable<KeyValuePair<StatType, int>>
            // We just assume that this type will be for a StatTypeConstDictionary. This is what lets us actually
            // display the stats text. The editing of the valeus is done in the chunk above with the AddEditorsHelper().
            TypeDescriptor.AddAttributes(typeof(IEnumerable<KeyValuePair<StatType, int>>),
                                         new TypeConverterAttribute(typeof(StatTypeConstDictionaryTypeConverter)));

            // Add the custom UITypeEditors defined by the base engine
            NetGore.EditorTools.CustomUITypeEditors.AddEditors();
            NetGore.EditorTools.CustomUITypeEditors.AddNPCChatDialogEditor(NPCChatManager.Instance);
            NetGore.EditorTools.CustomUITypeEditors.AddAIIDEditor(AIFactory.Instance);

            AddAdvancedClassTypeConverters();
        }
    }
}