using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// Helper methods for the <see cref="PropertyGrid"/>.
    /// </summary>
    public static class PropertyGridHelper
    {
        static readonly SelectedGridItemChangedEventHandler _refresher;
        static readonly EventHandler _shrinker;

        /// <summary>
        /// Initializes the <see cref="PropertyGridHelper"/> class.
        /// </summary>
        static PropertyGridHelper()
        {
            _shrinker = ((pg, o) => ShrinkColumnsHandler(pg));
            _refresher = ((pg, o) => RefreshHandler(pg));
        }

        /// <summary>
        /// Gets the event handler for refreshing a <see cref="PropertyGrid"/> when the selected grid item changes.
        /// This is usually attached to the <see cref="PropertyGrid.SelectedGridItemChanged"/> event.
        /// </summary>
        public static SelectedGridItemChangedEventHandler RefresherEventHandler
        {
            get { return _refresher; }
        }

        /// <summary>
        /// Gets the event handler for shrinking the left column of a <see cref="PropertyGrid"/>
        /// when the selected value changes for the first time, then removes itself from the event.
        /// This should be attached to the <see cref="PropertyGrid.SelectedObjectsChanged"/> event.
        /// </summary>
        public static EventHandler ShrinkerEventHandler
        {
            get { return _shrinker; }
        }

        /// <summary>
        /// Attaches the <see cref="RefresherEventHandler"/> to a <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="pg">The <see cref="PropertyGrid"/> to attach the event to.</param>
        public static void AttachRefresherEventHandler(PropertyGrid pg)
        {
            if (pg == null)
                return;

            pg.SelectedGridItemChanged -= RefresherEventHandler;
            pg.SelectedGridItemChanged += RefresherEventHandler;
        }

        /// <summary>
        /// Attaches the <see cref="ShrinkerEventHandler"/> to a <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="pg">The <see cref="PropertyGrid"/> to attach the event to.</param>
        public static void AttachShrinkerEventHandler(PropertyGrid pg)
        {
            if (pg == null)
                return;

            pg.SelectedObjectsChanged -= ShrinkerEventHandler;
            pg.SelectedObjectsChanged += ShrinkerEventHandler;
        }

        /// <summary>
        /// Handles the <see cref="PropertyGrid.SelectedGridItemChanged"/> event of the <see cref="PropertyGrid"/> control.
        /// This method is bound to all <see cref="PropertyGrid"/>s automatically at runtime.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        static void RefreshHandler(object sender)
        {
            var pg = sender as PropertyGrid;
            if (pg != null)
                pg.Refresh();
        }

        /// <summary>
        /// Adds the <see cref="GeneralPropertyGridContextMenu"/> to a <see cref="PropertyGrid"/> if there is
        /// no context menu already on the <see cref="PropertyGrid"/>.
        /// </summary>
        /// <param name="pg">The <see cref="PropertyGrid"/> to attach the menu to.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetContextMenuIfNone(PropertyGrid pg)
        {
            if (pg == null)
                return;

            // Add the context menu (only if one wasn't already provided)
            if (pg.ContextMenu == null && pg.ContextMenuStrip == null)
                pg.ContextMenu = new GeneralPropertyGridContextMenu();
        }

        /// <summary>
        /// When attached to the <see cref="PropertyGrid.SelectedObjectsChanged"/> event on a <see cref="PropertyGrid"/>,
        /// shrinks the <see cref="PropertyGrid"/>'s left column to fit the first item added to it, then detatches
        /// itself from the event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        static void ShrinkColumnsHandler(object sender)
        {
            var pg = sender as PropertyGrid;
            if (pg == null)
                return;

            // First time a valid object is set, shrink the PropertyGrid
            pg.ShrinkPropertiesColumn(20);

            // Remove this event hook from the PropertyGrid to make it only happen on the first call
            pg.SelectedObjectsChanged -= _shrinker;
        }
    }
}