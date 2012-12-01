using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore.World;

namespace NetGore.Editor
{
    /// <summary>
    /// Keeps track of which object(s) are currently selected and manages displaying and updating them.
    /// </summary>
    public class SelectedObjectsManager<T> where T : class
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly TypedEventHandler<Entity> _disposedListenerEntity;
        readonly EventHandler _selectedIndexChangedHandler;
        readonly List<T> _selectedObjs = new List<T>();
        T _focused;

        PropertyGrid _propertyGrid;
        ListBox _selectedListBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedObjectsManager{T}"/> class.
        /// </summary>
        public SelectedObjectsManager()
        {
            _disposedListenerEntity = DisposedListener_Entity;

            _selectedIndexChangedHandler = SelectedListBox_SelectedIndexChanged;
        }

        /// <summary>
        /// Notifies listeners when the focused object has changed.
        /// </summary>
        public event TypedEventHandler<SelectedObjectsManager<T>, EventArgs<T>> FocusedChanged;

        /// <summary>
        /// Notifies listeners when an object has been added to this collection.
        /// </summary>
        public event TypedEventHandler<SelectedObjectsManager<T>, EventArgs<T>> ObjectAdded;

        /// <summary>
        /// Notifies listeners when an object has been removed from this collection.
        /// </summary>
        public event TypedEventHandler<SelectedObjectsManager<T>, EventArgs<T>> ObjectRemoved;

        /// <summary>
        /// Notifies listeners when the selected objects have changed.
        /// </summary>
        public event TypedEventHandler<SelectedObjectsManager<T>> SelectedChanged;

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
                    FocusedChanged.Raise(this, EventArgsHelper.Create(_focused));
            }
        }

        /// <summary>
        /// Gets the <see cref="PropertyGrid"/> used to display the properties of the focused object.
        /// </summary>
        public PropertyGrid PropertyGrid
        {
            get { return _propertyGrid; }
            set
            {
                if (_propertyGrid == value)
                    return;

                _propertyGrid = value;

                if (_propertyGrid != null)
                {
                    // Update the PropertyGrid's selection
                    if (SelectedListBox != null && SelectedListBox.SelectedItems.Count > 1)
                    {
                        // Select many
                        var objs = new List<T>(SelectedListBox.SelectedItems.OfType<T>());
                        if (objs.Count > 0)
                            PropertyGrid.SelectedObjects = objs.Cast<object>().ToArray();
                    }
                    else
                    {
                        // Select only the focused
                        _propertyGrid.SelectedObject = Focused;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="ListBox"/> used to display the selected objects.
        /// </summary>
        public ListBox SelectedListBox
        {
            get { return _selectedListBox; }
            set
            {
                if (_selectedListBox == value)
                    return;

                _selectedListBox = value;

                if (_selectedListBox != null)
                {
                    // Update the SelectedListBox's items
                    ChangeSelected();
                }
            }
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
        public void Add(T obj)
        {
            if (obj == null)
                return;

            if (_selectedObjs.Contains(obj))
                return;

            _selectedObjs.Add(obj);

            OnObjectAdded(obj);
            if (ObjectAdded != null)
                ObjectAdded.Raise(this, EventArgsHelper.Create(obj));

            UpdateSelection();
        }

        /// <summary>
        /// Call when the focused object has been updated so the properties in the UI are refreshed.
        /// </summary>
        public void UpdateFocused()
        {
            if (_focused != null)
            {
                try
                {
                    PropertyGrid.Refresh();
                    if (FocusedChanged != null)
                        FocusedChanged.Raise(this, EventArgsHelper.Create(_focused));
                }
                catch (NullReferenceException)
                {
                }
            }
        }

        /// <summary>
        /// Handles when the focused object has changed.
        /// </summary>
        protected virtual void ChangeFocused()
        {
            if (SelectedListBox != null)
            {
                SelectedListBox.SelectedIndexChanged -= _selectedIndexChangedHandler;
                SelectedListBox.SelectedItem = Focused;
                SelectedListBox.SelectedIndexChanged += _selectedIndexChangedHandler;
            }

            if (PropertyGrid != null)
            {
                try
                {
                    PropertyGrid.SelectedObject = Focused;
                }
                catch (NullReferenceException)
                {
                }
            }
        }

        /// <summary>
        /// Handles when the selected objects have changed.
        /// </summary>
        protected virtual void ChangeSelected()
        {
            if (SelectedListBox != null)
            {
                SelectedListBox.SelectedIndexChanged -= _selectedIndexChangedHandler;

                SelectedListBox.SynchronizeItemList(SelectedObjects);
                SelectedListBox.SelectedItem = Focused;

                SelectedListBox.SelectedIndexChanged += _selectedIndexChangedHandler;
            }
        }

        /// <summary>
        /// Clears all selected objects.
        /// </summary>
        public void Clear()
        {
            if (_selectedObjs.Count == 0)
                return;

            while (_selectedObjs.Count > 0)
            {
                var obj = _selectedObjs[_selectedObjs.Count - 1];
                _selectedObjs.RemoveAt(_selectedObjs.Count - 1);

                OnObjectRemoved(obj);
                if (ObjectRemoved != null)
                    ObjectRemoved.Raise(this, EventArgsHelper.Create(obj));
            }

            Debug.Assert(_selectedObjs.Count == 0);

            UpdateSelection();
        }

        /// <summary>
        /// Handles when an <see cref="Entity"/> in this <see cref="SelectedObjectsManager{T}"/> is disposed.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void DisposedListener_Entity(Entity entity, EventArgs e)
        {
            Remove(entity as T);
        }

        /// <summary>
        /// Occurs when an object is added to the collection.
        /// </summary>
        /// <param name="obj">The object that was added.</param>
        protected virtual void OnObjectAdded(T obj)
        {
            var asEntity = obj as Entity;
            if (asEntity != null)
            {
                asEntity.Disposed -= _disposedListenerEntity;
                asEntity.Disposed += _disposedListenerEntity;
            }
        }

        /// <summary>
        /// Occurs when an object is removed from the collection.
        /// </summary>
        /// <param name="obj">The object that was removed.</param>
        protected virtual void OnObjectRemoved(T obj)
        {
            var asEntity = obj as Entity;
            if (asEntity != null)
            {
                asEntity.Disposed -= _disposedListenerEntity;
            }
        }

        /// <summary>
        /// Removes an object from the selected objects.
        /// </summary>
        /// <param name="obj">The object to remove.</param>
        public void Remove(T obj)
        {
            if (obj == null)
                return;

            if (!_selectedObjs.Remove(obj))
                return;

            OnObjectRemoved(obj);
            if (ObjectRemoved != null)
                ObjectRemoved.Raise(this, EventArgsHelper.Create(obj));

            UpdateSelection();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the SelectedListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void SelectedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedListBox == null)
                return;

            Debug.Assert(sender == SelectedListBox);

            Focused = SelectedListBox.SelectedItem as T;
            if (Focused == null)
                return;

            if (SelectedListBox.SelectedItems.Count > 1)
            {
                var objs = new List<T>(SelectedListBox.SelectedItems.OfType<T>());
                if (objs.Count > 0)
                    PropertyGrid.SelectedObjects = objs.Cast<object>().ToArray();
            }
        }

        /// <summary>
        /// Sets the focused object.
        /// </summary>
        /// <param name="obj">The object to set as focused.</param>
        /// <param name="addIfMissing">If true, <paramref name="obj"/> will be added to the collection
        /// if it is not already in it.</param>
        /// <returns>True if the <paramref name="obj"/> was successfully set as the focused object; otherwise
        /// false.</returns>
        public bool SetFocused(T obj, bool addIfMissing = false)
        {
            if (obj == null)
                return false;

            if (!_selectedObjs.Contains(obj))
            {
                if (!addIfMissing)
                    return false;

                _selectedObjs.Add(obj);

                OnObjectAdded(obj);
                if (ObjectAdded != null)
                    ObjectAdded.Raise(this, EventArgsHelper.Create(obj));
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
            if (selectedObjs == null || selectedObjs.IsEmpty())
            {
                Clear();
                return;
            }

            selectedObjs = selectedObjs.Distinct().ToImmutable();

            // Ignore if we already have this exact set as the current selection
            if (selectedObjs.ContainSameElements(_selectedObjs))
                return;

            // Set the new selected objects and update
            Clear();

            foreach (var obj in selectedObjs)
            {
                _selectedObjs.Add(obj);
                OnObjectAdded(obj);
                if (ObjectAdded != null)
                    ObjectAdded.Raise(this, EventArgsHelper.Create(obj));
            }

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
            Clear();

            _selectedObjs.Add(selected);

            OnObjectAdded(selected);
            if (ObjectAdded != null)
                ObjectAdded.Raise(this, EventArgsHelper.Create(selected));

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
                SelectedChanged.Raise(this, EventArgs.Empty);

            // Ensure the focused object is valid
            if (Focused == null || !_selectedObjs.Contains(Focused))
                Focused = _selectedObjs.FirstOrDefault();
        }
    }
}