using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Server;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;

namespace DemoGame.Editor
{
    /// <summary>
    /// An <see cref="Alliance"/> that is to be used in editors in a <see cref="PropertyGrid"/>.
    /// </summary>
    public class EditorAlliance : IAllianceTable
    {
        const string _category = "Alliance";
        readonly AllianceID _id;

        List<AllianceID> _attackable;
        List<AllianceID> _hostile;
        string _name = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorAlliance"/> class.
        /// </summary>
        /// <param name="id">The <see cref="AllianceID"/>.</param>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        /// <exception cref="ArgumentException">No <see cref="Alliance"/> exists for the given <paramref name="id"/>.</exception>
        public EditorAlliance(AllianceID id, IDbController dbController)
        {
            _id = id;

            var table = dbController.GetQuery<SelectAllianceQuery>().Execute(id);
            if (table == null)
            {
                const string errmsg = "No Alliance with ID `{0}` exists.";
                throw new ArgumentException(string.Format(errmsg, id), "id");
            }

            Debug.Assert(id == table.ID);

            Name = table.Name;

            var attackable = dbController.GetQuery<SelectAllianceAttackableQuery>().Execute(id);
            _attackable = new List<AllianceID>(attackable.Select(x => x.AttackableID));

            var hostile = dbController.GetQuery<SelectAllianceHostileQuery>().Execute(id);
            _hostile = new List<AllianceID>(hostile.Select(x => x.HostileID));
        }

        /// <summary>
        /// Gets or sets the list of <see cref="AllianceID"/>s that this <see cref="Alliance"/> can attack.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The alliances that this alliance is allowed to attack.")]
        [Category(_category)]
        public List<AllianceID> Attackable
        {
            get
            {
                _attackable.RemoveDuplicates((x, y) => x == y);
                return _attackable;
            }
            set
            {
                _attackable = value ?? new List<AllianceID>();
                _attackable.RemoveDuplicates((x, y) => x == y);
            }
        }

        /// <summary>
        /// Gets or sets the list of <see cref="AllianceID"/>s that this <see cref="Alliance"/> is hostile towards.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The alliances that this alliance is hostile towards.")]
        [Category(_category)]
        public List<AllianceID> Hostile
        {
            get
            {
                _hostile.RemoveDuplicates((x, y) => x == y);
                return _hostile;
            }
            set
            {
                _hostile = value ?? new List<AllianceID>();
                _hostile.RemoveDuplicates((x, y) => x == y);
            }
        }

        #region IAllianceTable Members

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        [Browsable(true)]
        [Description("The unique ID of this alliance.")]
        [Category(_category)]
        public AllianceID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        [Browsable(true)]
        [Description("The name of this alliance. Does not have to be unique, but it is recommended to avoid confusion.")]
        [Category(_category)]
        public string Name
        {
            get { return _name; }
            set { _name = value ?? string.Empty; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IAllianceTable IAllianceTable.DeepCopy()
        {
            return new AllianceTable(this);
        }

        #endregion
    }
}