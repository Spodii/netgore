using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Db.MySql;

namespace DemoGame.DbEditor
{
    public partial class frmMain : Form
    {
        IDbController _dbController;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Create the database connection
            DbConnectionSettings settings = new DbConnectionSettings();
            _dbController = new ServerDbController(settings.GetMySqlConnectionString());

            propertyGrid1.SelectedObject = _dbController.GetQuery<SelectItemTemplateQuery>().Execute(new ItemTemplateID(1));
        }
    }
}
