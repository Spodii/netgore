using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.EditorTools;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Db;
using NetGore.Db.MySql;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.IO;
using CustomUITypeEditors=DemoGame.EditorTools.CustomUITypeEditors;

namespace DemoGame.DbEditor
{
    public partial class frmMain : Form
    {
        IDbController _dbController;

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMain"/> class.
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
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
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    var item = f.SelectedItem;
                    pgCharacterTemplate.SelectedObject = item;
                }
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

            bool clone = false;
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

                _dbController.GetQuery<ReplaceCharacterTemplateQuery>().Execute(newItem);
            }
            else
            {
                // Create new template using the default table values (the row already exists since we reserved it).
                newItem = (CharacterTemplateTable)_dbController.GetQuery<SelectCharacterTemplateQuery>().Execute(freeID);
            }

            // Select the new item
            pgCharacterTemplate.SelectedObject = newItem;
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
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    var item = f.SelectedItem;
                    pgItemTemplate.SelectedObject = item;
                }
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

            bool clone = false;
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

                _dbController.GetQuery<ReplaceItemTemplateQuery>().Execute(newItem);
            }
            else
            {
                // Create new template using the default table values (the row already exists since we reserved it).
                newItem = (ItemTemplateTable)_dbController.GetQuery<SelectItemTemplateQuery>().Execute(freeID);
            }

            // Select the new item
            pgItemTemplate.SelectedObject = newItem;
        }

        void frmMain_Load(object sender, EventArgs e)
        {
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
            int idBase = 0;
            T freeIDAsT;

            // Loop until we successfully find an ID that has no template
            bool success = false;
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
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Ensure the GrhImageList's cache is updated
            GrhImageList.Save();

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

            // If the GrhDatas have no been loaded, we will have to load them. Otherwise, we won't get our
            // pretty little pictures. :(
            if (!GrhInfo.IsLoaded)
            {
                var gdService = GraphicsDeviceService.AddRef(Handle, 640, 480);
                var serviceContainer = new ServiceContainer();
                serviceContainer.AddService<IGraphicsDeviceService>(gdService);
                ContentManager cm = new ContentManager(serviceContainer, ContentPaths.Build.Root);
                GrhInfo.Load(ContentPaths.Dev, cm);
            }

            // Create the database connection
            DbConnectionSettings settings = new DbConnectionSettings();
            _dbController = new ServerDbController(settings.GetMySqlConnectionString());

            // Load the custom UITypeEditors
            CustomUITypeEditors.AddEditors(_dbController);

            // Hook PropertyGrid_ShrinkColumns to all PropertyGrids
            foreach (var pg in this.GetControls().OfType<PropertyGrid>())
            {
                pg.SelectedObjectsChanged += PropertyGrid_ShrinkColumns;
            }
        }

        /// <summary>
        /// Handles the PropertyValueChanged event of the pgCharacterTemplate control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PropertyValueChangedEventArgs"/>
        /// instance containing the event data.</param>
        void pgCharacterTemplate_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label != null && e.ChangedItem.Label.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("This value cannot be changed.");
                return;
            }

            var v = pgCharacterTemplate.SelectedObject as ICharacterTemplateTable;
            if (v == null)
                return;

            _dbController.GetQuery<ReplaceCharacterTemplateQuery>().Execute(v);
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
        /// Handles the PropertyValueChanged event of the pgItemTemplate control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PropertyValueChangedEventArgs"/>
        /// instance containing the event data.</param>
        void pgItemTemplate_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label != null && e.ChangedItem.Label.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("This value cannot be changed.");
                return;
            }

            var v = pgItemTemplate.SelectedObject as IItemTemplateTable;
            if (v == null)
                return;

            _dbController.GetQuery<ReplaceItemTemplateQuery>().Execute(v);
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
        /// When attached to the <see cref="PropertyGrid.SelectedObjectsChanged"/> event on a <see cref="PropertyGrid"/>,
        /// shrinks the <see cref="PropertyGrid"/>'s left column to fit the first item added to it, then detatches
        /// itself from the event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void PropertyGrid_ShrinkColumns(object sender, EventArgs e)
        {
            var pg = sender as PropertyGrid;
            if (pg == null)
                return;

            // First time a valid object is set, shrink the PropertyGrid
            pgItemTemplate.ShrinkPropertiesColumn(10);

            // Remove this event hook from the PropertyGrid to make it only happen on the first call
            pg.SelectedObjectsChanged -= PropertyGrid_ShrinkColumns;
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

            return GetFreeID(dbController, true, t => new CharacterTemplateID(t), x => (int)x, getUsedQuery.Execute,
                             x => selectQuery.Execute(x), x => insertByIDQuery.Execute(x));
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

            return GetFreeID(dbController, true, t => new ItemTemplateID(t), x => (int)x, getUsedQuery.Execute,
                             x => selectQuery.Execute(x), x => insertByIDQuery.Execute(x));
        }
    }
}