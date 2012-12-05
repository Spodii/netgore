using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Editor.TypeConverters;
using DemoGame.Editor.UITypeEditors;
using DemoGame.Server;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.AI;
using NetGore.Db;
using NetGore.Editor;
using NetGore.Editor.UI;
using NetGore.Features.ActionDisplays;
using NetGore.Features.NPCChat;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Graphics;
using NetGore.World;

namespace DemoGame.Editor
{
    /// <summary>
    /// Helper methods for the custom <see cref="UITypeEditor"/>s. Call <see cref="CustomUITypeEditors.AddEditors"/> once at the
    /// start of a project to add all of the custom <see cref="UITypeEditor"/>s.
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

        static void AddAdvancedClassTypeConverters()
        {
            // Add the types we want to have use the AdvancedClassTypeConverter. This is purely just for
            // a better PropertyGrid-based editing experience. Since it doesn't really matter if we add too many
            // types (since it is only really for the PropertyGrid), we just add EVERY table from the DbObjs.
            var filterCreator = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                CustomFilter = (x => x.Name.EndsWith("Table") && (x.Namespace ?? string.Empty).Contains("DbObjs"))
            };
            var filter = filterCreator.GetFilter();

            var typesToAdd = TypeHelper.FindTypes(filter, null);
            AdvancedClassTypeConverter.AddTypes(typesToAdd.ToArray());

            // Also automatically add all the instantiable types that derive from some of our base classes
            var baseTypes = new Type[] { typeof(Entity), typeof(MapBase) };
            filterCreator = new TypeFilterCreator { IsClass = true, IsAbstract = false, CustomFilter = (x => baseTypes.Any(x.IsSubclassOf)) };
            filter = filterCreator.GetFilter();

            typesToAdd = TypeHelper.FindTypes(filter, null);
            AdvancedClassTypeConverter.AddTypes(typesToAdd.ToArray());

            // Manually add some other types we want to have use the AdvancedClassTypeConverter. Again, doesn't
            // really matter what you add here. In general, you should just add to this when you notice that
            // a PropertyGrid isn't using the AdvancedClassTypeConverter.
            AdvancedClassTypeConverter.AddTypes(typeof(MutablePair<ItemTemplateID, byte>),
                typeof(MutablePair<CharacterTemplateID, ushort>), typeof(EditorQuest), typeof(EditorAlliance), typeof(EditorShop),
                typeof(EditorCharacterTemplate));

            // Set the properties we want to force being readonly in the PropertyGrid
            AdvancedClassTypeConverter.SetForceReadOnlyProperties(typeof(CharacterTemplateTable), "ID");
            AdvancedClassTypeConverter.SetForceReadOnlyProperties(typeof(EditorCharacterTemplate), "ID");
            AdvancedClassTypeConverter.SetForceReadOnlyProperties(typeof(ItemTemplateTable), "ID");

#if !TOPDOWN
            // Set the UITypeEditor for specific properties on classes instead of every property with a certain type
            AdvancedClassTypeConverter.SetForceEditor(typeof(ItemTemplateTable),
                new KeyValuePair<string, UITypeEditor>("EquippedBody", new BodyPaperDollTypeEditor()));
#endif
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
            NetGore.Editor.UI.CustomUITypeEditors.AddEditorsHelper(
                new EditorTypes(typeof(ActionDisplayID), typeof(ActionDisplayIDEditor)),
                new EditorTypes(typeof(ActionDisplayID?), typeof(ActionDisplayIDEditor)),
                new EditorTypes(typeof(CharacterTemplateID), typeof(CharacterTemplateIDEditor)),
                new EditorTypes(typeof(CharacterTemplateID?), typeof(CharacterTemplateIDEditor)),
                new EditorTypes(typeof(ItemTemplateID), typeof(ItemTemplateIDEditor)),
                new EditorTypes(typeof(ItemTemplateID?), typeof(ItemTemplateIDEditor)),
                new EditorTypes(typeof(AllianceID), typeof(AllianceIDEditor)),
                new EditorTypes(typeof(AllianceID?), typeof(AllianceIDEditor)),
                new EditorTypes(typeof(BodyID), typeof(BodyIDEditor)), new EditorTypes(typeof(BodyID?), typeof(BodyIDEditor)),
                new EditorTypes(typeof(BodyInfo), typeof(BodyInfo)), new EditorTypes(typeof(MapID), typeof(MapIDEditor)),
                new EditorTypes(typeof(MapID?), typeof(MapIDEditor)), new EditorTypes(typeof(ShopID), typeof(ShopIDEditor)),
                new EditorTypes(typeof(ShopID?), typeof(ShopIDEditor)),
                new EditorTypes(typeof(IEnumerable<KeyValuePair<StatType, int>>), typeof(StatTypeConstDictionaryEditor)),
                new EditorTypes(typeof(List<MutablePair<ItemTemplateID, byte>>), typeof(ItemTemplateAndAmountEditor)),
                new EditorTypes(typeof(List<QuestID>), typeof(QuestIDListEditor)),
                new EditorTypes(typeof(List<SkillType>), typeof(SkillTypeListEditor)),
                new EditorTypes(typeof(List<AllianceID>), typeof(AllianceIDListEditor)),
                new EditorTypes(typeof(List<ItemTemplateID>), typeof(ItemTemplateIDListEditor)),
                new EditorTypes(typeof(List<MutablePair<CharacterTemplateID, ushort>>), typeof(CharacterTemplateAndAmountEditor)),
                new EditorTypes(typeof(List<CharacterTemplateEquippedItem>), typeof(CharacterTemplateEquippedItemEditor)),
                new EditorTypes(typeof(List<CharacterTemplateInventoryItem>), typeof(CharacterTemplateInventoryItemEditor)),
                new EditorTypes(typeof(StatTypeConstDictionary), typeof(StatTypeConstDictionaryEditor)));

            // Add a TypeConverter for the type:
            //      IEnumerable<KeyValuePair<StatType, int>>
            // We just assume that this type will be for a StatTypeConstDictionary. This is what lets us actually
            // display the stats text. The editing of the valeus is done in the chunk above with the AddEditorsHelper().
            TypeDescriptor.AddAttributes(typeof(IEnumerable<KeyValuePair<StatType, int>>),
                new TypeConverterAttribute(typeof(StatTypeConstDictionaryTypeConverter)));

            // Add some other custom TypeConverters
            TypeDescriptor.AddAttributes(typeof(IEnumerable<MutablePair<CharacterTemplateID, ushort>>),
                new TypeConverterAttribute(typeof(CharacterTemplateAndAmountListTypeConverter)));

            TypeDescriptor.AddAttributes(typeof(IEnumerable<MutablePair<ItemTemplateID, byte>>),
                new TypeConverterAttribute(typeof(ItemTemplateAndAmountListTypeConverter)));

            TypeDescriptor.AddAttributes(typeof(IEnumerable<ItemTemplateID>),
                new TypeConverterAttribute(typeof(ItemTemplateListTypeConverter)));

            TypeDescriptor.AddAttributes(typeof(IEnumerable<CharacterTemplateEquippedItem>),
                new TypeConverterAttribute(typeof(CharacterTemplateEquippedItemListTypeConverter)));

            TypeDescriptor.AddAttributes(typeof(IEnumerable<CharacterTemplateInventoryItem>),
                new TypeConverterAttribute(typeof(CharacterTemplateInventoryItemListTypeConverter)));

            TypeDescriptor.AddAttributes(typeof(IEnumerable<QuestID>),
                new TypeConverterAttribute(typeof(QuestIDListTypeConverter)));

            TypeDescriptor.AddAttributes(typeof(IEnumerable<SkillType>),
                new TypeConverterAttribute(typeof(SkillTypeListTypeConverter)));

            TypeDescriptor.AddAttributes(typeof(IEnumerable<AllianceID>),
                new TypeConverterAttribute(typeof(AllianceIDListTypeConverter)));

            // Add the custom UITypeEditors defined by the base engine
            NetGore.Editor.UI.CustomUITypeEditors.AddEditors();
            NetGore.Editor.UI.CustomUITypeEditors.AddNPCChatDialogEditor(EditorNPCChatManager.Instance);
            NetGore.Editor.UI.CustomUITypeEditors.AddAIIDEditor(AIFactory.Instance);

            AddAdvancedClassTypeConverters();
            AddExtraTextProviders();
        }

        /// <summary>
        /// Adds all the extra text providers for the <see cref="AdvancedPropertyDescriptor"/>s.
        /// </summary>
        static void AddExtraTextProviders()
        {
            AdvancedPropertyDescriptor.SetExtraTextProvider<GrhIndex>(ExtraTextProvider_GrhIndex);
            AdvancedPropertyDescriptor.SetExtraTextProvider<AIID>(ExtraTextProvider_AIID);
            AdvancedPropertyDescriptor.SetExtraTextProvider<IAI>(ExtraTextProvider_IAI);
            AdvancedPropertyDescriptor.SetExtraTextProvider<AllianceID>(ExtraTextProvider_AllianceID);
            AdvancedPropertyDescriptor.SetExtraTextProvider<Alliance>(ExtraTextProvider_Alliance);
            AdvancedPropertyDescriptor.SetExtraTextProvider<BodyID>(ExtraTextProvider_BodyID);
            AdvancedPropertyDescriptor.SetExtraTextProvider<NPCChatDialogID>(ExtraTextProvider_NPCChatDialogID);
            AdvancedPropertyDescriptor.SetExtraTextProvider<ShopID>(ExtraTextProvider_ShopID);
            AdvancedPropertyDescriptor.SetExtraTextProvider<ItemTemplateID>(ExtraTextProvider_ItemTemplateID);
            AdvancedPropertyDescriptor.SetExtraTextProvider<CharacterTemplateID>(ExtraTextProvider_CharacterTemplateID);
            AdvancedPropertyDescriptor.SetExtraTextProvider<CharacterTemplateEquippedItem>(ExtraTextProvider_CharacterTemplateEquippedItem);
            AdvancedPropertyDescriptor.SetExtraTextProvider<CharacterTemplateInventoryItem>(ExtraTextProvider_CharacterTemplateInventoryItem);
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="AIID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_AIID(AIID v)
        {
            return AIFactory.Instance.GetAIName(v);
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="Alliance"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_Alliance(Alliance v)
        {
            if (v == null)
                return null;

            return v.Name;
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="AllianceID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_AllianceID(AllianceID v)
        {
            var alliance = AllianceManager.Instance[v];
            if (alliance == null)
                return null;

            return alliance.Name;
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="BodyID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_BodyID(BodyID v)
        {
            var body = BodyInfoManager.Instance.GetBody(v);
            if (body == null)
                return null;

            return body.Body + " - Size: " + body.Size.X + "x" + body.Size.Y;
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="CharacterTemplateEquippedItem"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_CharacterTemplateEquippedItem(CharacterTemplateEquippedItem v)
        {
            var item = ItemTemplateManager.Instance[v.ID];
            if (item == null)
                return null;

            var chance = Math.Round(v.Chance.Percentage * 100f, 0) + "%";

            return "[" + chance + "] " + item.Name;
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="CharacterTemplateID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_CharacterTemplateID(CharacterTemplateID v)
        {
            var character = CharacterTemplateManager.Instance[v];
            if (character == null || character.TemplateTable == null)
                return null;

            return character.TemplateTable.Name;
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="CharacterTemplateEquippedItem"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_CharacterTemplateInventoryItem(CharacterTemplateInventoryItem v)
        {
            var item = ItemTemplateManager.Instance[v.ID];
            if (item == null)
                return null;

            var chance = Math.Round(v.Chance.Percentage * 100f, 0) + "%";

            return "[" + chance + "] " + item.Name;
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="GrhIndex"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_GrhIndex(GrhIndex v)
        {
            var grhData = GrhInfo.GetData(v);
            if (grhData == null)
                return null;

            return grhData.Categorization.ToString();
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="IAI"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_IAI(IAI v)
        {
            if (v == null)
                return null;

            return AIFactory.Instance.GetAIName(v.ID);
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="ItemTemplateID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_ItemTemplateID(ItemTemplateID v)
        {
            var item = ItemTemplateManager.Instance[v];
            if (item == null)
                return null;

            return item.Name;
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="NPCChatDialogID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_NPCChatDialogID(NPCChatDialogID v)
        {
            var dialog = ClientNPCChatManager.Instance[v];
            if (dialog == null)
                return null;

            return dialog.Title;
        }

        /// <summary>
        /// Provides the extra text for the <see cref="AdvancedPropertyDescriptor"/> for a
        /// <see cref="ShopID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The extra text to display.</returns>
        static string ExtraTextProvider_ShopID(ShopID v)
        {
            var shop = ShopManager.Instance[v];
            if (shop == null)
                return null;

            return shop.Name;
        }
    }
}