using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Keeps track of which object(s) are currently selected and manages displaying and updating them.
    /// </summary>
    public class SelectedObjectsManager<T> where T : class
    {
        readonly PropertyGrid _propertyGrid;
        readonly EventHandler _selectedIndexChangedHandler;
        readonly ListBox _selectedListBox;
        readonly List<T> _selectedObjs = new List<T>();

        T _focused;

        /// <summary>
        /// Notifies listeners when the focused object has changed.
        /// </summary>
        public event SelectedObjectManagerEventHandler<T, T> FocusedChanged;

        /// <summary>
        /// Notifies listeners when the selected objects have changed.
        /// </summary>
        public event SelectedObjectManagerEventHandler<T> SelectedChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedObjectsManager{T}"/> class.
        /// </summary>
        /// <param name="propertyGrid">The <see cref="PropertyGrid"/> used to display the properties of the
        /// focused object.</param>
        /// <param name="selectedListBox">The <see cref="ListBox"/> used to display the selected objects.</param>
        public SelectedObjectsManager(PropertyGrid propertyGrid, ListBox selectedListBox)
        {
            if (propertyGrid == null)
                throw new ArgumentNullException("propertyGrid");
            if (selectedListBox == null)
                throw new ArgumentNullException("selectedListBox");

            _propertyGrid = propertyGrid;
            _selectedListBox = selectedListBox;

            _selectedIndexChangedHandler = SelectedListBox_SelectedIndexChanged;
            _selectedListBox.SelectedIndexChanged += _selectedIndexChangedHandler;
        }

        /// <summary>
        /// Gets the object that has the focus. This will only be null if no objects are in the selection.
        /// </summary>
        public T Focused
        {
            get { return _focused; }
            private set
            {
                if (_focused == value)
                    return;

                _focused = value;

                ChangeFocused();
                if (FocusedChanged != null)
                    FocusedChanged(this, _focused);
            }
        }

        /// <summary>
        /// Gets the <see cref="PropertyGrid"/> used to display the properties of the focused object.
        /// </summary>
        public PropertyGrid PropertyGrid
        {
            get { return _propertyGrid; }
        }

        /// <summary>
        /// Gets the <see cref="ListBox"/> used to display the selected objects.
        /// </summary>
        public ListBox SelectedListBox
        {
            get { return _selectedListBox; }
        }

        /// <summary>
        /// Gets all of the currently selected objects.
        /// </summary>
        public IEnumerable<T> SelectedObjects
        {
            get { return _selectedObjs; }
        }

        /// <summary>
        /// Adds a new object to the collection of selected objects.
        /// </summary>
        /// <param name="obj">The object to add.</param>
        public void AddSelected(T obj)
        {
            if (_selectedObjs.Contains(obj))
                return;

            _selectedObjs.Add(obj);
            UpdateSelection();
        }

        /// <summary>
        /// Handles when the focused object has changed.
        /// </summary>
        protected virtual void ChangeFocused()
        {
            SelectedListBox.SelectedIndexChanged -= _selectedIndexChangedHandler;
            SelectedListBox.SelectedItem = Focused;
            SelectedListBox.SelectedIndexChanged += _selectedIndexChangedHandler;

            PropertyGrid.SelectedObject = Focused;
        }

        /// <summary>
        /// Handles when the selected objects have changed.
        /// </summary>
        protected virtual void ChangeSelected()
        {
            SelectedListBox.SelectedIndexChanged -= _selectedIndexChangedHandler;

            SelectedListBox.SynchronizeItemList(SelectedObjects);
            SelectedListBox.SelectedItem = Focused;

            SelectedListBox.SelectedIndexChanged += _selectedIndexChangedHandler;
        }

        /// <summary>
        /// Clears all selected objects.
        /// </summary>
        public void Clear()
        {
            if (_selectedObjs.Count == 0)
                return;

            _selectedObjs.Clear();
            UpdateSelection();
        }

        /// <summary>
        /// Removes an object from the selected objects.
        /// </summary>
        /// <param name="obj">The object to remove.</param>
        public void Remove(T obj)
        {
            if (!_selectedObjs.Remove(obj))
                return;

            UpdateSelection();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the SelectedListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void SelectedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Focused = SelectedListBox.SelectedItem as T;
            if (Focused == null)
                return;

            if (SelectedListBox.SelectedItems.Count > 1)
            {
                List<T> objs = new List<T>(SelectedListBox.SelectedItems.Count);
                foreach (var item in 
                    SelectedListBox.SelectedItems)
                {
                    var o = item as T;
                    if (o != null)
                        objs.Add(o);
                }

                if (objs.Count > 0)
                    PropertyGrid.SelectedObjects = objs.ToArray();
            }
        }

        /// <summary>
        /// Sets the focused object.
        /// </summary>
        /// <param name="obj">The object to set as focused.</param>
        /// <returns>True if the <paramref name="obj"/> was successfully set as the focused object; otherwise
        /// false.</returns>
        public bool SetFocused(T obj)
        {
            return SetFocused(obj, false);
        }

        /// <summary>
        /// Sets the focused object.
        /// </summary>
        /// <param name="obj">The object to set as focused.</param>
        /// <param name="addIfMissing">If true, <paramref name="obj"/> will be added to the collection
        /// if it is not already in it.</param>
        /// <returns>True if the <paramref name="obj"/> was successfully set as the focused object; otherwise
        /// false.</returns>
        public bool SetFocused(T obj, bool addIfMissing)
        {
            if (!_selectedObjs.Contains(obj))
            {
                if (!addIfMissing)
                    return false;

                _selectedObjs.Add(obj);
            }

            Focused = obj;
            return true;
        }

        /// <summary>
        /// Gets the currently selected objects.
        /// </summary>
        /// <param name="selectedObjs">The currently selected objects.</param>
        public void SetManySelected(IEnumerable<T> selectedObjs)
        {
            // Check if to clear instead
            if (selectedObjs == null || selectedObjs.Count() == 0)
            {
                Clear();
                return;
            }

            selectedObjs = selectedObjs.Distinct();

            // Ignore if we already have this exact set as the current selection
            if (selectedObjs.ContainSameElements(_selectedObjs))
                return;

            // Set the new selected objects and update
            _selectedObjs.Clear();
            _selectedObjs.AddRange(selectedObjs);
            UpdateSelection();
        }

        /// <summary>
        /// Gets the currently selected object.
        /// </summary>
        /// <param name="selected">The currently selected object.</param>
        public void SetSelected(T selected)
        {
            // Check if to clear instead
            if (selected == null)
            {
                Clear();
                return;
            }

            // Ignore if we already have this exact set as the current selection
            if (_selectedObjs.Count == 1 && _selectedObjs[0] == selected)
                return;

            // Set the new selected objects and update
            _selectedObjs.Clear();
            _selectedObjs.Add(selected);
            UpdateSelection();
        }

        /// <summary>
        /// Handles when the selected objects have changed.
        /// </summary>
        void UpdateSelection()
        {
            // Notify that the collection has changed
            ChangeSelected();
            if (SelectedChanged != null)
                SelectedChanged(this);

            // Ensure the focused object is valid
            if (Focused == null || !_selectedObjs.Contains(Focused))
                Focused = _selectedObjs.FirstOrDefault();
        }
    }
}