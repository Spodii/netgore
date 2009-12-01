using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Special <see cref="ListBox"/> that is designed for displaying a list of items in a map.
    /// </summary>
    /// <typeparam name="TMap">Type of Map.</typeparam>
    /// <typeparam name="TItem">Type of collection item.</typeparam>
    public abstract class MapItemListBox<TMap, TItem> : TypedListBox<TItem>, IMapItemListBox
        where TMap : class, IMap
        where TItem : class
    {
        TMap _map;
        Timer _updateTimer;

        /// <summary>
        /// Gets or sets the Camera2D used to view the Map.
        /// </summary>
        public Camera2D Camera { get; set; }

        /// <summary>
        /// Gets or sets the Map.
        /// </summary>
        public TMap Map
        {
            get { return _map; }
            set
            {
                if (_map == value)
                    return;

                _map = value;
                UpdateItems();
            }
        }

        /// <summary>
        /// Gets or sets the IMap containing the objects being handled.
        /// </summary>
        [Browsable(false)]
        IMap IMapItemListBox.IMap
        {
            get { return Map; }
            set
            {
                Map = (TMap)value;
            }
        }

        /// <summary>
        /// When overridden in the derived class, creates a clone of the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to clone.</param>
        protected abstract void Clone(TItem item);

        /// <summary>
        /// Handles the "clone" menu button being pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        protected virtual void CloneHandler(object sender, EventArgs e)
        {
            TItem item = TypedSelectedItem;
            if (item == null || Map == null || Camera == null)
                return;

            Clone(item);
            UpdateItems();
        }

        /// <summary>
        /// When overridden in the derived class, deletes the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to delete.</param>
        protected abstract void Delete(TItem item);

        /// <summary>
        /// Handles the "delete" menu button being pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        protected virtual void DeleteHandler(object sender, EventArgs e)
        {
            TItem item = TypedSelectedItem;
            if (item == null || Map == null || Camera == null)
                return;

            Delete(item);
            UpdateItems();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control"/> and its child
        /// controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release
        /// only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;

            // Dispose of the timer
            if (_updateTimer != null)
            {
                _updateTimer.Tick -= UpdateTimer_Tick;
                _updateTimer.Dispose();
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of objects to be used in this MapItemListBox.
        /// </summary>
        /// <returns>An IEnumerable of objects to be used in this MapItemListBox.</returns>
        protected abstract IEnumerable<TItem> GetItems();

        /// <summary>
        /// Called after the control has been added to another container.
        /// </summary>
        protected override void InitLayout()
        {
            base.InitLayout();

            // Create the update timer
            _updateTimer = new Timer();
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Interval = 1000;
            _updateTimer.Start();

            // Create the menu
            var menuItems = new List<MenuItem>
            { new MenuItem("Locate", LocateHandler), new MenuItem("Clone", CloneHandler), new MenuItem("Delete", DeleteHandler), };

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            ContextMenu = new ContextMenu(menuItems.ToArray());
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// When overridden in the derived class, centers the camera on the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Object to locate.</param>
        protected abstract void Locate(TItem item);

        /// <summary>
        /// Handles the "locate" menu button being pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        protected virtual void LocateHandler(object sender, EventArgs e)
        {
            TItem item = TypedSelectedItem;
            if (item == null || Map == null || Camera == null)
                return;

            Locate(item);
        }

        /// <summary>
        /// Updates the list of items displayed.
        /// </summary>
        protected virtual void UpdateItems()
        {
            if (Map == null || Camera == null)
            {
                if (Items.Count > 0)
                    Items.Clear();
                return;
            }

            var allItems = GetItems();
            if (allItems == null || allItems.Count() == 0)
            {
                if (Items.Count > 0)
                    Items.Clear();
                return;
            }

            var existingItems = Items.OfType<TItem>();
            var toAdd = allItems.Except(existingItems).ToArray();
            var toRemove = allItems.Except(existingItems).ToArray();

            try
            {
                BeginUpdate();

                // Remove the extra items
                foreach (TItem item in toRemove)
                {
                    this.RemoveItemAndReselect(item);
                }

                // Add the new items
                foreach (TItem item in toAdd)
                {
                    this.AddItemAndReselect(item);
                }

                // Refresh the items
                RefreshItems();
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Updates the ListBox items.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
        void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateItems();
        }
    }
}