using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Editor.UITypeEditors;
using DemoGame.Server;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using DemoGame.Server.Quests;
using NetGore;
using NetGore.Content;
using NetGore.Db;
using NetGore.Db.MySql;
using NetGore.Editor;
using WeifenLuo.WinFormsUI.Docking;
using NetGore.Editor.WinForms;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.Editor
{
    public partial class DbEditorForm : DockContent
    {
        IDbController _dbController;
        GameMessage? _editingGameMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbEditorForm"/> class.
        /// </summary>
        public DbEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Changes the selected game messages language.
        /// </summary>
        /// <param name="newLanguage">The new language name. Can be null or empty to select no language.</param>
        void ChangeGameMessagesLanguage(string newLanguage)
        {
            txtMessages.Text = newLanguage;

            // Clear old values
            lstMessages.Items.Clear();
            lstMissingMessages.Items.Clear();
            _editingGameMessage = null;
            txtSelectedMessage.Text = string.Empty;

            // Set new values
            if (!string.IsNullOrEmpty(newLanguage))
            {
                // Existing messages
                var existing = GameMessageCollection.LoadRawMessages(newLanguage).OrderBy(x => x.Key.ToString(),
                    NaturalStringComparer.Instance);
                lstMessages.Items.AddRange(existing.Cast<object>().ToArray());

                // Missing messages
                var missing = EnumHelper<GameMessage>.Values.Except(existing.Select(x => x.Key));
                lstMissingMessages.Items.AddRange(missing.Cast<object>().ToArray());
            }
        }

        /// <summary>
        /// Gets the next free <see cref="ItemTemplateID"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TTemplate">The type of the template.</typeparam>
        /// <param name="dbController">The db controller.</param>
        /// <param name="reserve">If true, the index will be reserved by inserting the default
        /// values for the column at the given index.</param>
        /// <param name="intToT">The int to T.</param>
        /// <param name="tToInt">The t to int.</param>
        /// <param name="getUsed">The get used.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="inserter">The inserter.</param>
        /// <returns>
        /// The next free <see cref="ItemTemplateID"/>.
        /// </returns>
        public static T GetFreeID<T, TTemplate>(IDbController dbController, bool reserve, Func<int, T> intToT, Func<T, int> tToInt,
                                                Func<IEnumerable<T>> getUsed, Func<T, TTemplate> selector, Action<T> inserter)
            where TTemplate : class
        {
            // Get the used IDs as ints
            var usedIDs = getUsed().Select(tToInt).ToImmutable();

            // Start with ID 0
            var idBase = 0;
            T freeIDAsT;

            // Loop until we successfully find an ID that has no template
            var success = false;
            do
            {
                // Get the next free value
                var freeID = usedIDs.NextFreeValue(idBase);
                freeIDAsT = intToT(freeID);

                // Increase the base so that way, if it fails, we are forced to check a higher value
                idBase = freeID + 1;

                // Ensure the ID is free
                if (selector(freeIDAsT) != null)
                    continue;

                // Reserve the ID if needed
                if (reserve)
                {
                    try
                    {
                        inserter(freeIDAsT);
                    }
                    catch (DuplicateKeyException)
                    {
                        // Someone just grabbed the key - those jerks!
                        continue;
                    }
                }

                success = true;
            }
            while (!success);

            // Return the value
            return freeIDAsT;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DesignMode)
            {
                base.OnClosing(e);
                return;
            }

            var quitMsg = "If you quit without saving, changes will be lost. Are you sure you wish to quit?" + Environment.NewLine +
                          " Press No if you wish to go back and save anything that needs to be saved.";

            if (MessageBox.Show(quitMsg, "Quit?", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Don't try to load stuff when in design mode
            if (DesignMode)
                return;

            Show();
            Refresh();

            // Create the database connection
            _dbController = GlobalState.Instance.DbController;

            // If the GrhDatas have no been loaded, we will have to load them. Otherwise, we won't get our
            // pretty little pictures. :(
            if (!GrhInfo.IsLoaded)
            {
                var cm = ContentManager.Create();
                GrhInfo.Load(ContentPaths.Dev, cm);
            }

            if (_dbController == null)
            {
                Close();
                return;
            }

            // Process all the PropertyGrids
            foreach (var pg in this.GetControls().OfType<PropertyGrid>())
            {
                PropertyGridHelper.AttachRefresherEventHandler(pg);
                PropertyGridHelper.AttachShrinkerEventHandler(pg);
                PropertyGridHelper.SetContextMenuIfNone(pg);
            }
        }

        /// <summary>
        /// Gets and reserves the next free <see cref="AllianceID"/>.
        /// </summary>
        /// <param name="dbController">The db controller.</param>
        /// <returns>The next free <see cref="AllianceID"/>.</returns>
        public static AllianceID ReserveFreeAllianceID(IDbController dbController)
        {
            var getUsedQuery = dbController.GetQuery<SelectAllianceIDsQuery>();
            var selectQuery = dbController.GetQuery<SelectAllianceQuery>();
            var insertByIDQuery = dbController.GetQuery<InsertAllianceQuery>();

            return GetFreeID(dbController, true, t => new AllianceID(t), x => (int)x, getUsedQuery.Execute, selectQuery.Execute,
                x => insertByIDQuery.Execute(new AllianceTable { ID = x, Name = string.Empty }));
        }

        /// <summary>
        /// Gets and reserves the next free <see cref="CharacterTemplateID"/>.
        /// </summary>
        /// <param name="dbController">The db controller.</param>
        /// <returns>The next free <see cref="CharacterTemplateID"/>.</returns>
        public static CharacterTemplateID ReserveFreeCharacterTemplateID(IDbController dbController)
        {
            var getUsedQuery = dbController.GetQuery<SelectCharacterTemplateIDsQuery>();
            var selectQuery = dbController.GetQuery<SelectCharacterTemplateQuery>();
            var insertByIDQuery = dbController.GetQuery<InsertCharacterTemplateIDOnlyQuery>();

            return GetFreeID(dbController, true, t => new CharacterTemplateID(t), x => (int)x, getUsedQuery.Execute, selectQuery.Execute, insertByIDQuery.Execute);
        }

        /// <summary>
        /// Gets and reserves the next free <see cref="ItemTemplateID"/>.
        /// </summary>
        /// <param name="dbController">The db controller.</param>
        /// <returns>The next free <see cref="ItemTemplateID"/>.</returns>
        public static ItemTemplateID ReserveFreeItemTemplateID(IDbController dbController)
        {
            var getUsedQuery = dbController.GetQuery<SelectItemTemplateIDsQuery>();
            var selectQuery = dbController.GetQuery<SelectItemTemplateQuery>();
            var insertByIDQuery = dbController.GetQuery<InsertItemTemplateIDOnlyQuery>();

            return GetFreeID(dbController, true, t => new ItemTemplateID(t), x => (int)x, getUsedQuery.Execute, selectQuery.Execute, insertByIDQuery.Execute);
        }

        /// <summary>
        /// Gets and reserves the next free <see cref="QuestID"/>.
        /// </summary>
        /// <param name="dbController">The db controller.</param>
        /// <returns>The next free <see cref="QuestID"/>.</returns>
        public static QuestID ReserveFreeQuestID(IDbController dbController)
        {
            var getUsedQuery = dbController.GetQuery<SelectQuestIDsQuery>();
            var selectQuery = dbController.GetQuery<SelectQuestQuery>();
            var insertByIDQuery = dbController.GetQuery<InsertQuestQuery>();

            return GetFreeID(dbController, true, t => new QuestID(t), x => (int)x, getUsedQuery.Execute, selectQuery.Execute,
                x => insertByIDQuery.Execute(new QuestTable { ID = x }));
        }

        /// <summary>
        /// Gets and reserves the next free <see cref="ShopID"/>.
        /// </summary>
        /// <param name="dbController">The db controller.</param>
        /// <returns>The next free <see cref="ShopID"/>.</returns>
        public static ShopID ReserveFreeShopID(IDbController dbController)
        {
            var getUsedQuery = dbController.GetQuery<SelectShopIDsQuery>();
            var selectQuery = dbController.GetQuery<SelectShopQuery>();
            var insertByIDQuery = dbController.GetQuery<InsertUpdateShopQuery>();

            return GetFreeID(dbController, true, t => new ShopID(t), x => (int)x, getUsedQuery.Execute, selectQuery.Execute,
                x => insertByIDQuery.Execute(new ShopTable { ID = x, Name = string.Empty }));
        }

        /// <summary>
        /// Sets the current quest.
        /// </summary>
        /// <param name="questID">The quest ID.</param>
        /// <param name="isNew">Set to new if this is a new quest; otherwise set as false.</param>
        void SetQuest(QuestID questID, bool isNew)
        {
            pgQuest.SelectedObject = new EditorQuest(questID, _dbController);

            if (isNew)
            {
                var qdc = (QuestDescriptionCollection)QuestDescriptionCollection.Create(ContentPaths.Dev);
                var qd = qdc[questID];

                if (qd != null)
                {
                    qdc.Remove(qd);
                    qdc.Save();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAllianceDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAllianceDelete_Click(object sender, EventArgs e)
        {
            var a = pgAlliance.SelectedObject as EditorAlliance;
            if (a == null)
                return;

            const string confirmMsg = "Are you sure you wish to delete the alliance `{0}` [ID: {1}]?";
            if (MessageBox.Show(string.Format(confirmMsg, a.Name, a.ID), "Delete?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            _dbController.GetQuery<DeleteAllianceQuery>().Execute(a.ID);

            pgAlliance.SelectedObject = null;

            MessageBox.Show(string.Format("Alliance `{0}` [ID: {1}] successfully deleted!", a.Name, a.ID));
        }

        /// <summary>
        /// Handles the Click event of the btnAllianceNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAllianceNew_Click(object sender, EventArgs e)
        {
            const string confirmMsg = "Are you sure you wish to create a new alliance?";
            if (MessageBox.Show(confirmMsg, "Create new?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Get the next free ID
            var id = ReserveFreeAllianceID(_dbController);

            // Set the new alliance
            pgAlliance.SelectedObject = new EditorAlliance(id, _dbController);
        }

        /// <summary>
        /// Handles the Click event of the btnAllianceSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAllianceSave_Click(object sender, EventArgs e)
        {
            var v = pgAlliance.SelectedObject as EditorAlliance;
            if (v == null)
                return;

            // Main values
            _dbController.GetQuery<InsertUpdateAllianceQuery>().Execute(v);

            // Attackable/hostile lists
            _dbController.GetQuery<InsertAllianceAttackableQuery>().Execute(v.ID, v.Attackable);
            _dbController.GetQuery<InsertAllianceHostileQuery>().Execute(v.ID, v.Hostile);

            // Reload from the database
            AllianceManager.Instance.Reload(v.ID);

            // Refresh the selected object
            pgAlliance.SelectedObject = null;
            pgAlliance.SelectedObject = v;

            MessageBox.Show("Alliance " + v.Name + " successfully saved!");
        }

        /// <summary>
        /// Handles the Click event of the btnAlliance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAlliance_Click(object sender, EventArgs e)
        {
            using (var f = new AllianceUITypeEditorForm(pgAlliance.SelectedObject))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var item = f.SelectedItem;
                pgAlliance.SelectedObject = new EditorAlliance(item.ID, _dbController);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCharacterTemplateDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCharacterTemplateDelete_Click(object sender, EventArgs e)
        {
            var item = pgCharacterTemplate.SelectedObject as ICharacterTemplateTable;
            if (item == null)
                return;

            const string confirmMsg = "Are you sure you wish to delete the character template {0} [ID: {1}]?";
            if (MessageBox.Show(string.Format(confirmMsg, item.Name, item.ID), "Delete?", MessageBoxButtons.YesNo) ==
                DialogResult.No)
                return;

            _dbController.GetQuery<DeleteCharacterTemplateQuery>().Execute(item.ID);

            MessageBox.Show(string.Format("Character template {0} [ID: {1}] successfully deleted.", item.Name, item.ID));

            pgCharacterTemplate.SelectedObject = null;
        }

        /// <summary>
        /// Handles the Click event of the btnCharacterTemplateNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCharacterTemplateNew_Click(object sender, EventArgs e)
        {
            const string confirmMsg = "Are you sure you wish to create a new character template?";
            if (MessageBox.Show(confirmMsg, "Create new?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var clone = false;
            if (pgCharacterTemplate.SelectedObject != null)
            {
                const string cloneMsg = "Do you wish to copy the values from the currently selected character template?";
                clone = MessageBox.Show(cloneMsg, "Clone?", MessageBoxButtons.YesNo) == DialogResult.Yes;
            }

            // Get the free ID
            var freeID = ReserveFreeCharacterTemplateID(_dbController);

            // Create the template
            CharacterTemplateTable newItem;
            if (clone)
            {
                // Clone from existing template
                try
                {
                    newItem = (CharacterTemplateTable)((ICharacterTemplateTable)pgCharacterTemplate.SelectedObject).DeepCopy();
                    newItem.ID = freeID;
                }
                catch (Exception)
                {
                    // If cloning fails, make sure we delete the ID we reserved before returning the exception
                    _dbController.GetQuery<DeleteCharacterTemplateQuery>().Execute(freeID);
                    throw;
                }

                _dbController.GetQuery<InsertUpdateCharacterTemplateQuery>().Execute(newItem);
            }
            else
            {
                // Create new template using the default table values (the row already exists since we reserved it).
                newItem = (CharacterTemplateTable)_dbController.GetQuery<SelectCharacterTemplateQuery>().Execute(freeID);
            }

            // Select the new item
            pgCharacterTemplate.SelectedObject = new EditorCharacterTemplate(newItem, _dbController);
        }

        /// <summary>
        /// Handles the Click event of the btnCharacterTemplateSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCharacterTemplateSave_Click(object sender, EventArgs e)
        {
            // Check for a valid object being edited
            var v = pgCharacterTemplate.SelectedObject as EditorCharacterTemplate;
            if (v == null)
                return;

            // Save
            _dbController.GetQuery<InsertUpdateCharacterTemplateQuery>().Execute(v);

            // Equipped items (delete old, then add new)
            _dbController.GetQuery<DeleteCharacterTemplateEquippedForCharacterQuery>().Execute(v.ID);

            var equippedQuery = _dbController.GetQuery<InsertCharacterTemplateEquippedQuery>();
            foreach (var item in v.Equipped)
            {
                var freeRow = IDCreatorHelper.GetNextFreeID(_dbController.ConnectionPool, CharacterTemplateEquippedTable.TableName,
                    "id");
                var queryArg = item.ToTableRow(v.ID, freeRow);
                equippedQuery.Execute(queryArg);
            }

            // Inventory items (delete old, then add new)
            _dbController.GetQuery<DeleteCharacterTemplateInventoryForCharacterQuery>().Execute(v.ID);

            var inventoryQuery = _dbController.GetQuery<InsertCharacterTemplateInventoryQuery>();
            foreach (var item in v.Inventory)
            {
                var freeRow = IDCreatorHelper.GetNextFreeID(_dbController.ConnectionPool,
                    CharacterTemplateInventoryTable.TableName, "id");
                var queryArg = item.ToTableRow(v.ID, freeRow);
                inventoryQuery.Execute(queryArg);
            }

            // Known Skills (delete old, then add new)
            _dbController.GetQuery<DeleteCharacterTemplateSkillsQuery>().Execute(v.ID);

            var knownSkillsQuery = _dbController.GetQuery<InsertCharacterTemplateSkillQuery>();
            foreach (var skill in v.KnownSkills)
            {
                knownSkillsQuery.Execute(v.ID, skill);
            }

            // Quests (delete old, then add new)
            _dbController.GetQuery<DeleteCharacterTemplateQuestProviderForCharacterQuery>().Execute(v.ID);

            var questQuery = _dbController.GetQuery<InsertCharacterTemplateQuestProviderQuery>();
            foreach (var item in v.GiveQuests)
            {
                var queryArg = new CharacterTemplateQuestProviderTable(characterTemplateID: v.ID, questID: item);
                questQuery.Execute(queryArg);
            }

            // Reload from the database
            CharacterTemplateManager.Instance.Reload(v.ID);

            // Refresh the selected object
            pgCharacterTemplate.SelectedObject = null;
            pgCharacterTemplate.SelectedObject = v;

            MessageBox.Show(v.Name + " successfully saved!");
        }

        /// <summary>
        /// Handles the Click event of the btnCharacterTemplate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCharacterTemplate_Click(object sender, EventArgs e)
        {
            using (var f = new CharacterTemplateUITypeEditorForm(pgCharacterTemplate.SelectedObject))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var item = f.SelectedItem;
                pgCharacterTemplate.SelectedObject = new EditorCharacterTemplate(item, _dbController);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnItemTemplateDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnItemTemplateDelete_Click(object sender, EventArgs e)
        {
            var item = pgItemTemplate.SelectedObject as IItemTemplateTable;
            if (item == null)
                return;

            const string confirmMsg = "Are you sure you wish to delete the item template {0} [ID: {1}]?";
            if (MessageBox.Show(string.Format(confirmMsg, item.Name, item.ID), "Delete?", MessageBoxButtons.YesNo) ==
                DialogResult.No)
                return;

            _dbController.GetQuery<DeleteItemTemplateQuery>().Execute(item.ID);

            MessageBox.Show(string.Format("Item template {0} [ID: {1}] successfully deleted.", item.Name, item.ID));

            pgItemTemplate.SelectedObject = null;
        }

        /// <summary>
        /// Handles the Click event of the btnItemTemplateNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnItemTemplateNew_Click(object sender, EventArgs e)
        {
            const string confirmMsg = "Are you sure you wish to create a new item template?";
            if (MessageBox.Show(confirmMsg, "Create new?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var clone = false;
            if (pgItemTemplate.SelectedObject != null)
            {
                const string cloneMsg = "Do you wish to copy the values from the currently selected item template?";
                clone = MessageBox.Show(cloneMsg, "Clone?", MessageBoxButtons.YesNo) == DialogResult.Yes;
            }

            // Get the free ID
            var freeID = ReserveFreeItemTemplateID(_dbController);

            // Create the template
            ItemTemplateTable newItem;
            if (clone)
            {
                // Clone from existing template
                try
                {
                    newItem = (ItemTemplateTable)((IItemTemplateTable)pgItemTemplate.SelectedObject).DeepCopy();
                    newItem.ID = freeID;
                }
                catch (Exception)
                {
                    // If cloning fails, make sure we delete the ID we reserved before returning the exception
                    _dbController.GetQuery<DeleteItemTemplateQuery>().Execute(freeID);
                    throw;
                }

                _dbController.GetQuery<InsertUpdateItemTemplateQuery>().Execute(newItem);
            }
            else
            {
                // Create new template using the default table values (the row already exists since we reserved it).
                newItem = (ItemTemplateTable)_dbController.GetQuery<SelectItemTemplateQuery>().Execute(freeID);
            }

            // Select the new item
            pgItemTemplate.SelectedObject = newItem;
        }

        /// <summary>
        /// Handles the Click event of the btnItemTemplateSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnItemTemplateSave_Click(object sender, EventArgs e)
        {
            // Check for a valid object being edited
            var v = pgItemTemplate.SelectedObject as IItemTemplateTable;
            if (v == null)
                return;

            // Save
            _dbController.GetQuery<InsertUpdateItemTemplateQuery>().Execute(v);

            // Reload from the database
            ItemTemplateManager.Instance.Reload(v.ID);

            // Refresh the selected object
            pgItemTemplate.SelectedObject = null;
            pgItemTemplate.SelectedObject = v;

            MessageBox.Show(v.Name + " successfully saved!");
        }

        /// <summary>
        /// Handles the Click event of the btnItemTemplate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnItemTemplate_Click(object sender, EventArgs e)
        {
            using (var f = new ItemTemplateUITypeEditorForm(pgItemTemplate.SelectedObject))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var item = f.SelectedItem;
                pgItemTemplate.SelectedObject = item;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnMessagesDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnMessagesDelete_Click(object sender, EventArgs e)
        {
            var lang = txtMessages.Text;
            if (string.IsNullOrEmpty(lang))
                return;

            // Confirm the deletion
            const string confirmMsg =
                "Are you sure you wish to delete the messages for the language `{0}`? This cannot be undone!";
            if (MessageBox.Show(string.Format(confirmMsg, lang), "Delete language message?", MessageBoxButtons.YesNo) ==
                DialogResult.No)
                return;

            // Clear screen
            ChangeGameMessagesLanguage(null);

            // Delete
            GameMessageCollection.DeleteLanguageFiles(lang);
        }

        /// <summary>
        /// Handles the Click event of the btnMessagesNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnMessagesNew_Click(object sender, EventArgs e)
        {
            // Confirm
            const string confirmMsg = "Are you sure you wish to create a new game messages language?";
            if (MessageBox.Show(confirmMsg, "Create new language?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Get the name to use
            var name = InputBox.Show("New language name", "Enter the name of the new language");
            if (string.IsNullOrEmpty(name))
                return;

            // Create the new language files, and select it
            var langFile = GameMessageCollection.GetLanguageFile(ContentPaths.Dev, name);
            if (!File.Exists(langFile))
                File.WriteAllText(langFile, string.Empty);

            ChangeGameMessagesLanguage(name);
        }

        /// <summary>
        /// Handles the Click event of the btnMessagesSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnMessagesSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessages.Text))
                return;

            GameMessageCollection.SaveRawMessages(txtMessages.Text, lstMessages.Items.OfType<KeyValuePair<GameMessage, string>>());

            MessageBox.Show(string.Format("The game messages for language `{0}` have been successfully saved.", txtMessages.Text), "Saved!");
        }

        /// <summary>
        /// Handles the Click event of the btnMessages control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnMessages_Click(object sender, EventArgs e)
        {
            using (var f = new GameMessageCollectionLanguageEditorUITypeEditorForm(null))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                ChangeGameMessagesLanguage(f.SelectedItem);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnQuestDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnQuestDelete_Click(object sender, EventArgs e)
        {
            var quest = pgQuest.SelectedObject as EditorQuest;
            if (quest == null)
                return;

            const string confirmMsg = "Are you sure you wish to delete the quest `{0}` [ID: {1}]?";
            if (MessageBox.Show(string.Format(confirmMsg, quest.Name, quest.ID), "Delete?", MessageBoxButtons.YesNo) ==
                DialogResult.No)
                return;

            // Delete from database
            _dbController.GetQuery<DeleteQuestQuery>().Execute(quest.ID);

            // Delete descriptions
            var qdc = (QuestDescriptionCollection)QuestDescriptionCollection.Create(ContentPaths.Dev);
            var qd = qdc[quest.ID];
            if (qd != null)
            {
                qdc.Remove(qd);
                qdc.Save();
            }

            MessageBox.Show(string.Format("Quest `{0}` [ID: {1}] successfully deleted.", quest.Name, quest.ID));

            pgQuest.SelectedObject = null;
        }

        /// <summary>
        /// Handles the Click event of the btnQuestNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnQuestNew_Click(object sender, EventArgs e)
        {
            const string confirmMsg = "Are you sure you wish to create a new quest?";
            if (MessageBox.Show(confirmMsg, "Create new?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Get the next free ID
            var id = ReserveFreeQuestID(_dbController);

            // Set the new quest
            SetQuest(id, true);
        }

        /// <summary>
        /// Handles the Click event of the btnQuestSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnQuestSave_Click(object sender, EventArgs e)
        {
            var v = pgQuest.SelectedObject as EditorQuest;
            if (v == null)
                return;

            // Description
            var qdc = (QuestDescriptionCollection)QuestDescriptionCollection.Create(ContentPaths.Dev);
            var qd = qdc[v.ID] as QuestDescription;
            if (qd == null)
            {
                // Remove an existing IQuestDescription that is not of type QuestDescription, so we can write to it
                var rawQD = qdc[v.ID];
                if (rawQD != null)
                    qdc.Remove(rawQD);

                // Add the new QuestDescription type
                qd = new QuestDescription { QuestID = v.ID };
                qdc.Add(qd);
            }

            qd.Name = v.Name;
            qd.Description = v.Description;

            qdc.Save();

            // Main quest table values
            _dbController.GetQuery<InsertUpdateQuestQuery>().Execute(v);

            // DELETE then INSERT to ensure everything updates correctly
            // You cannot just simply delete the quest as this will
            // delete all links in other tables due to the DB key relationship

            // Required items to start/finish
            _dbController.GetQuery<DeleteQuestRequireStartItemQuery>().Execute(v.ID);
            _dbController.GetQuery<InsertQuestRequireStartItemQuery>().Execute(v.ID, v.StartItems.Select(x => (KeyValuePair<ItemTemplateID, byte>)x));
            _dbController.GetQuery<DeleteQuestRequireFinishItemQuery>().Execute(v.ID);
            _dbController.GetQuery<InsertQuestRequireFinishItemQuery>().Execute(v.ID, v.FinishItems.Select(x => (KeyValuePair<ItemTemplateID, byte>)x));

            // Required quests to start/finish
            _dbController.GetQuery<DeleteQuestRequireStartQuestQuery>().Execute(v.ID);
            _dbController.GetQuery<InsertQuestRequireStartQuestQuery>().Execute(v.ID, v.StartQuests);
            _dbController.GetQuery<DeleteQuestRequireFinishQuestQuery>().Execute(v.ID);
            _dbController.GetQuery<InsertQuestRequireFinishQuestQuery>().Execute(v.ID, v.FinishQuests);

            // Other requirements
            _dbController.GetQuery<DeleteQuestRequireKillQuery>().Execute(v.ID);
            _dbController.GetQuery<InsertQuestRequireKillQuery>().Execute(v.ID, v.Kills.Select(x => (KeyValuePair<CharacterTemplateID, ushort>)x));

            // Rewards
            _dbController.GetQuery<DeleteQuestRewardItemQuery>().Execute(v.ID);
            _dbController.GetQuery<InsertQuestRewardItemQuery>().Execute(v.ID, v.RewardItems.Select(x => (KeyValuePair<ItemTemplateID, byte>)x));

            SetQuest(v.ID, false);

            QuestManager.Instance.Reload(v.ID);

            pgQuest.SelectedObject = null;
            pgQuest.SelectedObject = v;

            MessageBox.Show("Quest ID `" + v.ID + "` successfully saved!");
        }

        /// <summary>
        /// Handles the Click event of the btnQuest control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnQuest_Click(object sender, EventArgs e)
        {
            using (var f = new QuestUITypeEditorForm(null))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var item = f.SelectedItem;
                SetQuest(item.ID, false);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnShopDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnShopDelete_Click(object sender, EventArgs e)
        {
            var a = pgShop.SelectedObject as EditorShop;
            if (a == null)
                return;

            const string confirmMsg = "Are you sure you wish to delete the shop `{0}` [ID: {1}]?";
            if (MessageBox.Show(string.Format(confirmMsg, a.Name, a.ID), "Delete?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            _dbController.GetQuery<DeleteShopQuery>().Execute(a.ID);

            pgShop.SelectedObject = null;

            MessageBox.Show(string.Format("Shop `{0}` [ID: {1}] successfully deleted!", a.Name, a.ID));
        }

        /// <summary>
        /// Handles the Click event of the btnShopNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnShopNew_Click(object sender, EventArgs e)
        {
            const string confirmMsg = "Are you sure you wish to create a new shop?";
            if (MessageBox.Show(confirmMsg, "Create new?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Ask if they want to clone the selected object
            var clone = false;
            if (pgShop.SelectedObject != null)
            {
                const string cloneMsg = "Do you wish to copy the values from the currently selected shop?";
                clone = MessageBox.Show(cloneMsg, "Clone?", MessageBoxButtons.YesNo) == DialogResult.Yes;
            }

            // Get the next free ID
            var id = ReserveFreeShopID(_dbController);

            // Create the template
            EditorShop newShop;
            if (clone)
            {
                // Clone from existing template
                try
                {
                    newShop = new EditorShop(id, (EditorShop)pgShop.SelectedObject);
                }
                catch (Exception)
                {
                    // If cloning fails, make sure we delete the ID we reserved before returning the exception
                    _dbController.GetQuery<DeleteShopQuery>().Execute(id);
                    throw;
                }
            }
            else
            {
                // Create new template using the default table values (the row already exists since we reserved it)
                newShop = new EditorShop(id, _dbController);
            }

            // Set the new shop
            pgShop.SelectedObject = newShop;
        }

        /// <summary>
        /// Handles the Click event of the btnShopSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnShopSave_Click(object sender, EventArgs e)
        {
            var v = pgShop.SelectedObject as EditorShop;
            if (v == null)
                return;

            // Main values
            _dbController.GetQuery<InsertUpdateShopQuery>().Execute(v);

            // Items
            _dbController.GetQuery<DeleteShopItemsQuery>().Execute(v.ID);
            _dbController.GetQuery<InsertShopItemQuery>().Execute(v.ID, v.Items);

            // Reload from the database
            ShopManager.Instance.Reload(v.ID);

            // Refresh the selected object
            pgShop.SelectedObject = null;
            pgShop.SelectedObject = v;

            MessageBox.Show("Shop " + v.Name + " successfully saved!");
        }

        /// <summary>
        /// Handles the Click event of the btnShops control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnShops_Click(object sender, EventArgs e)
        {
            using (var f = new ShopUITypeEditorForm(pgShop.SelectedObject))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                var item = f.SelectedItem;
                pgShop.SelectedObject = new EditorShop(item.ID, _dbController);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstMessages control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMessages.SelectedItem == null || !(lstMessages.SelectedItem is KeyValuePair<GameMessage, string>))
            {
                _editingGameMessage = null;
                return;
            }

            var sel = (KeyValuePair<GameMessage, string>)lstMessages.SelectedItem;
            _editingGameMessage = sel.Key;
            txtSelectedMessage.Text = sel.Value;
        }

        /// <summary>
        /// Handles the DoubleClick event of the lstMissingMessages control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstMissingMessages_DoubleClick(object sender, EventArgs e)
        {
            if (lstMissingMessages.SelectedItem == null)
                return;

            var msg = (GameMessage)lstMissingMessages.SelectedItem;

            // Remove item from this list
            lstMissingMessages.Items.RemoveAt(lstMissingMessages.SelectedIndex);

            // Do not add if already exists in the added list
            if (lstMessages.Items.OfType<KeyValuePair<GameMessage, string>>().Any(x => x.Key == msg))
                return;

            // Add and re-sort
            var newItem = new KeyValuePair<GameMessage, string>(msg, "\"\"");
            var msgs =
                lstMessages.Items.OfType<KeyValuePair<GameMessage, string>>().Concat(new KeyValuePair<GameMessage, string>[]
                { newItem }).ToImmutable();
            lstMessages.Items.Clear();
            lstMessages.Items.AddRange(
                msgs.OrderBy(x => x.Key.ToString(), NaturalStringComparer.Instance).Cast<object>().ToArray());

            // Select the new item
            lstMessages.SelectedIndex = lstMessages.Items.IndexOf(newItem);
        }

        /// <summary>
        /// Handles the SelectedObjectsChanged event of the pgAlliance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void pgAlliance_SelectedObjectsChanged(object sender, EventArgs e)
        {
            var c = pgAlliance.SelectedObject as EditorAlliance;
            if (c == null)
                txtAlliance.Text = string.Empty;
            else
                txtAlliance.Text = string.Format("{0}. {1}", c.ID, c.Name);
        }

        /// <summary>
        /// Handles the SelectedObjectsChanged event of the pgCharacterTemplate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void pgCharacterTemplate_SelectedObjectsChanged(object sender, EventArgs e)
        {
            var item = pgCharacterTemplate.SelectedObject as ICharacterTemplateTable;
            if (item == null)
                txtCharacterTemplate.Text = string.Empty;
            else
                txtCharacterTemplate.Text = string.Format("{0}. {1}", item.ID, item.Name);
        }

        /// <summary>
        /// Handles the SelectedObjectsChanged event of the pgItemTemplate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void pgItemTemplate_SelectedObjectsChanged(object sender, EventArgs e)
        {
            var item = pgItemTemplate.SelectedObject as IItemTemplateTable;
            if (item == null)
                txtItemTemplate.Text = string.Empty;
            else
                txtItemTemplate.Text = string.Format("{0}. {1}", item.ID, item.Name);
        }

        /// <summary>
        /// Handles the SelectedObjectsChanged event of the pgQuest control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void pgQuest_SelectedObjectsChanged(object sender, EventArgs e)
        {
            var q = pgQuest.SelectedObject as EditorQuest;
            if (q == null)
                txtQuest.Text = string.Empty;
            else
                txtQuest.Text = string.Format("{0}. {1}", q.ID, q.Name);
        }

        /// <summary>
        /// Handles the SelectedObjectsChanged event of the pgShop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void pgShop_SelectedObjectsChanged(object sender, EventArgs e)
        {
            var c = pgShop.SelectedObject as EditorShop;
            if (c == null)
                txtShop.Text = string.Empty;
            else
                txtShop.Text = string.Format("{0}. {1}", c.ID, c.Name);
        }

        /// <summary>
        /// Handles the KeyDown event of the txtSelectedMessage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void txtSelectedMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return || _editingGameMessage == null)
                return;

            txtSelectedMessage_Leave(sender, null);
        }

        /// <summary>
        /// Handles the Leave event of the txtSelectedMessage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtSelectedMessage_Leave(object sender, EventArgs e)
        {
            var v = _editingGameMessage;
            _editingGameMessage = null;

            KeyValuePair<GameMessage, string> original;
            try
            {
                original = lstMessages.Items.OfType<KeyValuePair<GameMessage, string>>().First(x => x.Key == v);
            }
            catch (InvalidOperationException)
            {
                return;
            }

            if (original.Value == txtSelectedMessage.Text)
                return;

            var oldSelectedIndex = lstMessages.SelectedIndex;
            var msgIndex = lstMessages.Items.IndexOf(original);

            lstMessages.Items[msgIndex] = new KeyValuePair<GameMessage, string>(original.Key, txtSelectedMessage.Text);

            lstMessages.SelectedIndex = oldSelectedIndex;
        }
    }
}