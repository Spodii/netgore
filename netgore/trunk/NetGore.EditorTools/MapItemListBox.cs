using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Special ListBox that is designed for displaying a list of items in a map.
    /// </summary>
    public abstract class MapItemListBox : ListBox
    {
        IMap _map;
        Timer _updateTimer;

        /// <summary>
        /// Gets or sets the Camera2D used to view the Map.
        /// </summary>
        public Camera2D Camera { get; set; }

        /// <summary>
        /// Gets or sets the Map containing the Entities being handled.
        /// </summary>
        public IMap IMap
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
        /// When overridden in the derived class, handles the "clone" menu button being pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        protected abstract void CloneHandler(object sender, EventArgs e);

        /// <summary>
        /// When overridden in the derived class, handles the "delete" menu button being pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        protected abstract void DeleteHandler(object sender, EventArgs e);

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control"/> and its child controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. 
        ///                 </param>
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
                            {
                                new MenuItem("Locate", LocateHandler), new MenuItem("Clone", CloneHandler),
                                new MenuItem("Delete", DeleteHandler),
                            };

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            ContextMenu = new ContextMenu(menuItems.ToArray());
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// When overridden in the derived class, handles the "locate" menu button being pressed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        protected abstract void LocateHandler(object sender, EventArgs e);

        /// <summary>
        /// Updates the list of items displayed.
        /// </summary>
        protected abstract void UpdateItems();

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

    /// <summary>
    /// Special ListBox that is designed for displaying a list of items in a map.
    /// </summary>
    /// <typeparam name="TMap">Type of Map.</typeparam>
    /// <typeparam name="TItem">Type of collection item.</typeparam>
    public abstract class MapItemListBox<TMap, TItem> : MapItemListBox where TMap : class, IMap where TItem : class
    {
        /// <summary>
        /// Gets the IMap as type <typeparam name="TMap">.
        /// </summary>
        public TMap Map
        {
            get { return IMap as TMap; }
        }

        /// <summary>
        /// Gets the selected item as type <typeparam name="TItem">.
        /// </summary>
        protected TItem SelectedTItem
        {
            get { return SelectedItem as TItem; }
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
        protected override void CloneHandler(object sender, EventArgs e)
        {
            TItem item = SelectedTItem;
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
        protected override void DeleteHandler(object sender, EventArgs e)
        {
            TItem item = SelectedTItem;
            if (item == null || Map == null || Camera == null)
                return;

            Delete(item);
            UpdateItems();
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of objects to be used in this MapItemListBox.
        /// </summary>
        /// <returns>An IEnumerable of objects to be used in this MapItemListBox.</returns>
        protected abstract IEnumerable<TItem> GetItems();

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
        protected override void LocateHandler(object sender, EventArgs e)
        {
            TItem item = SelectedTItem;
            if (item == null || Map == null || Camera == null)
                return;

            Locate(item);
        }

        /// <summary>
        /// Updates the list of items displayed.
        /// </summary>
        protected override void UpdateItems()
        {
            if (Map == null || Camera == null)
                return;

            var allItems = GetItems();
            if (allItems == null || allItems.Count() == 0)
                return;

            var existingItems = Items.OfType<TItem>();
            var toAdd = allItems.Except(existingItems).ToArray();
            var toRemove = allItems.Except(existingItems).ToArray();

            try
            {
                BeginUpdate();

                // Remove the extra items
                foreach (TItem item in toRemove)
                {
                    Items.Remove(item);
                }

                // Add the new items
                foreach (TItem item in toAdd)
                {
                    Items.Add(item);
                }

                // Refresh the items
                RefreshItems();
            }
            finally
            {
                EndUpdate();
            }
        }
    }
}