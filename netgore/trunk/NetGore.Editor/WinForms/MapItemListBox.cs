using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore.World;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// Special <see cref="ListBox"/> that is designed for displaying a list of items in a map.
    /// </summary>
    /// <typeparam name="TMap">Type of Map.</typeparam>
    /// <typeparam name="TItem">Type of collection item.</typeparam>
    public abstract class MapItemListBox<TMap, TItem> : ListBox, IMapBoundControl where TMap : class, IMap where TItem : class
    {
        TMap _map;
        Timer _updateTimer;

        /// <summary>
        /// Gets or sets the Map.
        /// </summary>
        [Browsable(false)]
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
        /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control"/> and its child
        /// controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release
        /// only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

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
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl"/> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // Create the update timer
            if (_updateTimer == null)
            {
                _updateTimer = new Timer();
                _updateTimer.Tick += UpdateTimer_Tick;
                _updateTimer.Interval = 1000;
                _updateTimer.Start();
            }
        }

        /// <summary>
        /// Updates the list of items displayed.
        /// </summary>
        protected virtual void UpdateItems()
        {
            // If the map is not set, clear the list
            if (Map == null)
            {
                if (Items.Count > 0)
                    Items.Clear();
                return;
            }

            // Get all the items and, if there is none, clear the list
            var allItems = GetItems();
            if (allItems == null || allItems.IsEmpty())
            {
                if (Items.Count > 0)
                    Items.Clear();
                return;
            }

            // Find what items need to be added and removed to/from the current list
            var existingItems = Items.OfType<TItem>();
            var toAdd = allItems.Except(existingItems).ToArray();
            var toRemove = existingItems.Except(allItems).ToArray();

            // If nothing needs to change, return
            if (toAdd.IsEmpty() && toRemove.IsEmpty())
                return;

            // Remove the extra items and add the new ones
            try
            {
                BeginUpdate();

                // Remove the extra items
                foreach (var item in toRemove)
                {
                    Items.Remove(item);
                }

                // Add the new items
                Items.AddRange(toAdd.Cast<object>().ToArray());

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

        #region IMapBoundControl Members

        /// <summary>
        /// Gets or sets the IMap containing the objects being handled.
        /// </summary>
        [Browsable(false)]
        IMap IMapBoundControl.IMap
        {
            get { return Map; }
            set { Map = (TMap)value; }
        }

        #endregion
    }
}