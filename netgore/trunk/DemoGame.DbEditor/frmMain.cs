using System;
using System.Linq;
using System.Windows.Forms;
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
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

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

            pgItemTemplate.SelectedObject = _dbController.GetQuery<SelectItemTemplateQuery>().Execute(new ItemTemplateID(1));
        }
    }
}