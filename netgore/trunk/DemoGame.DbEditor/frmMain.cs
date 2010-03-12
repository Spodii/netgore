using System;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.EditorTools;
using DemoGame.Server.Queries;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        /// Handles the Click event of the btnItemTemplate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnItemTemplate_Click(object sender, EventArgs e)
        {
            using (var f = new ItemTemplateUITypeEditorForm(pgItemTemplate.SelectedObject))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                    pgItemTemplate.SelectedObject = f.SelectedItem;
            }
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
            if (!GrhInfo.IsLoaded && false) // TODO: !! Temp removal...
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

            // TODO: !! Temp populating...
            pgItemTemplate.SelectedObject = _dbController.GetQuery<SelectItemTemplateQuery>().Execute(new ItemTemplateID(1));
        }

        /// <summary>
        /// Handles the PropertyValueChanged event of the pgItemTemplate control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PropertyValueChangedEventArgs"/>
        /// instance containing the event data.</param>
        void pgItemTemplate_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var v = pgItemTemplate.SelectedObject as IItemTemplateTable;
            if (v == null)
                return;

            _dbController.GetQuery<ReplaceItemTemplateQuery>().Execute(v);
        }

        private void PropertyGrid_ShrinkColumns(object sender, EventArgs e)
        {
            var pg = sender as PropertyGrid;
            if (pg == null) return;

            // First time a valid object is set, shrink the PropertyGrid
            pgItemTemplate.ShrinkPropertiesColumn(10);

            // Remove this event hook from the PropertyGrid to make it only happen on the first call
            pg.SelectedObjectsChanged -= PropertyGrid_ShrinkColumns;
        }
    }
}