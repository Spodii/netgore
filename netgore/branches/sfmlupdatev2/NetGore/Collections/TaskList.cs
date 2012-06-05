using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// A very basic yet light-weight collection that only supports processing all items in the collection at once.
    /// The purpose of this list is for when you have a collection that has to process all items in the collection
    /// at once, and some or all of the items can be removed while processing.
    /// This collection is not thread-safe.
    /// </summary>
    /// <typeparam name="T">The type of value to store.</typeparam>
    public abstract class TaskList<T>
    {
        readonly Stack<TaskListNode> _pool = new Stack<TaskListNode>();

        /// <summary>
        /// The first node in the list, or null if the list is empty.
        /// </summary>
        TaskListNode _first = null;

        /// <summary>
        /// Adds a new item to this <see cref="TaskList{T}"/> at the head of the list.
        /// </summary>
        /// <param name="value">The value of the node to add.</param>
        public void Add(T value)
        {
            // Get the node object
            TaskListNode newHead;
            if (_pool.Count > 0)
                newHead = _pool.Pop();
            else
                newHead = new TaskListNode();

            // Set the node's values
            newHead.Value = value;
            newHead.Next = _first;

            // Set as the new head
            _first = newHead;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional processing after all tasks have been processed.
        /// </summary>
        protected virtual void PostProcess()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional processing before tasks are processed.
        /// </summary>
        protected virtual void PreProcess()
        {
        }

        /// <summary>
        /// Processes all of the tasks in the list.
        /// </summary>
        public void Process()
        {
            TaskListNode last = null;
            var current = _first;

            PreProcess();

            // Loop through the nodes until we hit null, indicating the end of the list
            while (current != null)
            {
                // Perform the func and get if we need to remove the node
                var remove = ProcessItem(current.Value);

                if (remove)
                {
                    // Remove the node
                    if (last != null)
                    {
                        // Skip over the node by setting the previous node's Next to the current node's Next
                        last.Next = current.Next;
                    }
                    else
                    {
                        // No previous node, so just set the next node as the head, bumping the current node into nothingness
                        _first = current.Next;
                    }

                    // Clear the value of the node to ensure we don't hold onto any references, then push the node
                    // back into the pool so it can be reused
                    current.Value = default(T);

                    var tmp = current;
                    current = current.Next;

                    tmp.Next = null;
                    _pool.Push(tmp);
                }
                else
                {
                    // No nodes removed, so just set the last node to the current
                    last = current;
                    current = current.Next;
                }
            }

            PostProcess();
        }

        /// <summary>
        /// When overridden in the derived class, handles processing the given task.
        /// </summary>
        /// <param name="item">The value of the task to process.</param>
        /// <returns>True if the <paramref name="item"/> is to be removed from the collection; otherwise false.</returns>
        protected abstract bool ProcessItem(T item);

        /// <summary>
        /// A <see cref="TaskList{T}"/> node that contains the value of the node, and the next node in the list.
        /// </summary>
        class TaskListNode
        {
            /// <summary>
            /// Gets or sets the next node in the list, or null if this is the last node in the list.
            /// </summary>
            public TaskListNode Next { get; set; }

            /// <summary>
            /// Gets or sets the value of the node.
            /// </summary>
            public T Value { get; set; }
        }
    }
}