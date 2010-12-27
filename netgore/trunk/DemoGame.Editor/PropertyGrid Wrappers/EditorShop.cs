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
using NetGore.Features.Shops;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="Shop"/> that is to be used in editors in a <see cref="PropertyGrid"/>.
    /// </summary>
    public class EditorShop : IShopTable
    {
        const string _category = "Shop";
        readonly ShopID _id;

        List<ItemTemplateID> _items = new List<ItemTemplateID>();
        string _name = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorShop"/> class.
        /// </summary>
        /// <param name="id">The <see cref="ShopID"/>.</param>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        /// <exception cref="ArgumentException">No shop exists for the specified <paramref name="id"/>.</exception>
        public EditorShop(ShopID id, IDbController dbController)
        {
            _id = id;

            // Grab the general shop information
            var table = dbController.GetQuery<SelectShopQuery>().Execute(id);
            if (table == null)
            {
                const string errmsg = "No Shop with ID `{0}` exists.";
                throw new ArgumentException(string.Format(errmsg, id), "id");
            }

            Debug.Assert(id == table.ID);

            Name = table.Name;
            CanBuy = table.CanBuy;

            // Grab the items from the database and add them
            var dbItems = dbController.GetQuery<SelectShopItemsQuery>().Execute(id);
            _items = new List<ItemTemplateID>();
            if (dbItems != null)
                _items.AddRange(dbItems.Select(x => x.ItemTemplateID));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorShop"/> class.
        /// </summary>
        /// <param name="id">The <see cref="ShopID"/>.</param>
        /// <param name="existing">The <see cref="EditorShop"/> to copy the values from.</param>
        public EditorShop(ShopID id, EditorShop existing)
        {
            _id = id;

            Name = existing.Name;
            CanBuy = existing.CanBuy;

            _items = new List<ItemTemplateID>(existing._items);
        }

        /// <summary>
        /// Gets or sets the list of <see cref="ItemTemplateID"/>s of the items that this <see cref="Shop"/> sells.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The item template IDs of the items this shop sells.")]
        [Category(_category)]
        public List<ItemTemplateID> Items
        {
            get
            {
                _items.RemoveDuplicates((x, y) => x == y);
                return _items;
            }
            set
            {
                _items = value ?? new List<ItemTemplateID>();
                _items.RemoveDuplicates((x, y) => x == y);
            }
        }

        #region IShopTable Members

        /// <summary>
        /// Gets the value of the database column `can_buy`.
        /// </summary>
        [Browsable(true)]
        [Description("If this shop can buy items from shoppers. If false, this shop only sells items.")]
        [Category(_category)]
        public bool CanBuy { get; set; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        [Browsable(true)]
        [Description("The unique ID of this shop.")]
        [Category(_category)]
        public ShopID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        [Browsable(true)]
        [Description("The name of this shop. Does not have to be unique, but it is recommended to avoid confusion.")]
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
        /// <returns>A deep copy of this table.</returns>
        IShopTable IShopTable.DeepCopy()
        {
            return new ShopTable(this);
        }

        #endregion
    }
}