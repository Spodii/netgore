using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Platyform.Extensions;

namespace Platyform.EditorTools
{
    /// <summary>
    /// Applies undo and redo operations on objects
    /// </summary>
    /// <typeparam name="T">Type of object to perform the undo operations on</typeparam>
    public class Undoer<T>
    {
        /// <summary>
        /// Stack used for redo operations
        /// </summary>
        protected Stack<ObjectVersion<T>> redoStack = new Stack<ObjectVersion<T>>();

        /// <summary>
        /// Dictionary of objects currently being tracked and a list of their values when tracking started
        /// </summary>
        protected Dictionary<T, List<PropertyInfoValue>> tracking = new Dictionary<T, List<PropertyInfoValue>>();

        /// <summary>
        /// Stack used for undo operations
        /// </summary>
        protected Stack<ObjectVersion<T>> undoStack = new Stack<ObjectVersion<T>>();

        /// <summary>
        /// Gets if a redo operation can be performed
        /// </summary>
        public bool CanRedo
        {
            get { return redoStack.Count > 0; }
        }

        /// <summary>
        /// Gets if an undo operation can be performed
        /// </summary>
        public bool CanUndo
        {
            get { return undoStack.Count > 0; }
        }

        /// <summary>
        /// Applies the differences to an item
        /// </summary>
        /// <param name="item">UndoItem information to apply to</param>
        protected static void ApplyDifferences(ObjectVersion<T> item)
        {
            // Go through and apply all changes
            foreach (PropertyInfoValue piv in item.Changes)
            {
                piv.PropertyInfo.SetValue(item.Object, piv.Value, null);
            }
        }

        /// <summary>
        /// Begins tracking an object for changes. Items must be tracked to use Update() on, but
        /// do not need to be tracked for Undo() and Redo().
        /// </summary>
        /// <param name="item">Object to track</param>
        public void BeginTrack(T item)
        {
            // Check if the item is already being tracked
            if (tracking.ContainsKey(item))
                return;

            // Add the item and its properties to the tracker
            tracking.Add(item, GetAllValues(item));
        }

        /// <summary>
        /// Ends tracking an object for changes and updates any changes made
        /// </summary>
        /// <param name="item">Object to track</param>
        public void EndTrack(T item)
        {
            // Store the differences
            StoreDifferences(item);

            // Remove the object from the tracking list
            tracking.Remove(item);
        }

        /// <summary>
        /// Creates a list of properties and values for an item
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>List of PropertyInfoValues for the item</returns>
        protected List<PropertyInfoValue> GetAllValues(object item)
        {
            Type type = item.GetType();
            var props = new List<PropertyInfoValue>();

            // For each property, add the PropertyInfo and its value to the return list
            foreach (PropertyInfo p in type.GetProperties())
            {
                if (IsValidProperty(p))
                    props.Add(new PropertyInfoValue(p, p.GetValue(item, null)));
            }

            return props;
        }

        /// <summary>
        /// Finds all the changed values between the cached and current object state
        /// </summary>
        /// <param name="item">Tracked object to check</param>
        /// <returns>List of all changes</returns>
        protected List<PropertyInfoValue> GetDifferences(T item)
        {
            var ret = new List<PropertyInfoValue>();

            // Get the old values
            List<PropertyInfoValue> oldValues;
            if (!tracking.TryGetValue(item, out oldValues))
                return ret;

            // Check all the values for a change and return them if there was one
            foreach (PropertyInfoValue piv in oldValues)
            {
                if (piv.Value != piv.PropertyInfo.GetValue(item, null))
                    ret.Add(piv);
            }

            return ret;
        }

        /// <summary>
        /// Checks if a property is a valid one to track
        /// </summary>
        /// <param name="p">Property information in question</param>
        /// <returns>True if valid, else false</returns>
        protected virtual bool IsValidProperty(PropertyInfo p)
        {
            // Must be able to read and write. Indexers not supported.
            return p.CanRead && p.CanWrite;
        }

        /// <summary>
        /// Performs a redo operation
        /// </summary>
        /// <returns>Object that the redo operation was performed on. default(T) if no items to redo.</returns>
        public T Redo()
        {
            // Check for an item to redo
            if (!CanRedo)
                return default(T);

            // Get the redo item
            var item = redoStack.Pop();

            // Push the current value of the item onto the undo stack
            undoStack.Push(new ObjectVersion<T>(item.Object, GetAllValues(item.Object)));

            // Apply the differences
            ApplyDifferences(item);

            return item.Object;
        }

        /// <summary>
        /// Finds and stores the changes in an object into the stack using GetDifferences()
        /// </summary>
        /// <param name="item">Tracked object to check</param>
        protected void StoreDifferences(T item)
        {
            // Get the list of changes
            var diffs = GetDifferences(item);

            // Check that there was even any changes
            if (diffs.Count == 0)
                return;

            // Add the changes and the item to the undo stack
            undoStack.Push(new ObjectVersion<T>(item, diffs));
        }

        /// <summary>
        /// Performs an undo operation
        /// </summary>
        /// <returns>Object that the undo operation was performed on. default(T) if no items to undo.</returns>
        public T Undo()
        {
            // Check for items in the stack
            if (!CanUndo)
                return default(T);

            // Get the next item
            var item = undoStack.Pop();

            // Add the item to the redo stack
            redoStack.Push(new ObjectVersion<T>(item.Object, GetAllValues(item.Object)));

            // Roll back
            ApplyDifferences(item);

            // Return the UndoItem
            return item.Object;
        }

        /// <summary>
        /// Notifies the tracker that an object has been updated but does not remove it from tracking
        /// </summary>
        /// <param name="item">Tracked object to update</param>
        public void Update(T item)
        {
            EndTrack(item);
            BeginTrack(item);
        }
    }
}