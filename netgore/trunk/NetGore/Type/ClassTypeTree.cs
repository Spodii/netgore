using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// TODO: !! Use this to build the spatials and spatial aggregates automatically

namespace NetGore
{
    /// <summary>
    /// Describes an inheritance hierarchy for a set of class types.
    /// </summary>
    public class ClassTypeTree
    {
        readonly IEnumerable<ClassTypeTree> _children;
        readonly Type _selfType;
        readonly ClassTypeTree _parent;

        /// <summary>
        /// Gets the <see cref="Type"/> that this node contains. If null, this node is used for all <see cref="Type"/>s
        /// that match the node's parent, but not any of the parent's other children. That is, it is like a wildcard.
        /// </summary>
        public Type Type { get { return _selfType; } }

        /// <summary>
        /// Gets this node's parent node. Only null if this is the root node.
        /// </summary>
        public ClassTypeTree Parent { get { return _parent; } }
        
        /// <summary>
        /// Gets this node's child nodes.
        /// </summary>
        public IEnumerable<ClassTypeTree> Children { get {
            if (_children == null)
                return Enumerable.Empty<ClassTypeTree>();

            return _children; } }
        
        /// <summary>
        /// Gets the root node in the tree. This will always be the same node for each node in a tree.
        /// </summary>
        public ClassTypeTree Root
        {
            get
            {
                if (Parent == null)
                    return this;

                return Parent.Root;
            }
        }

        ClassTypeTree InternalFind(Type type)
        {
            if (type == _selfType)
                return this;

            // Ensure the type is assignable from this node
            // This little snippet is what allows us to use Find() from anywhere in the tree, not just from the root
            if (Type != null && !Type.IsAssignableFrom(type))
                return Parent.InternalFind(type);

            if (_children != null)
            {
                foreach (var child in Children)
                {
                    if (child.Type != null && child.Type.IsAssignableFrom(type))
                        return child.InternalFind(type);
                }

                return _children.First(x => x.Type == null);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Finds the <see cref="ClassTypeTree"/> from this <see cref="ClassTypeTree"/> that best matches the
        /// given <paramref name="type"/>. If the <paramref name="type"/> is not a class, the returned
        /// <see cref="ClassTypeTree"/> will always be the root.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to find the tree node for.</param>
        /// <returns>The <see cref="ClassTypeTree"/> from this <see cref="ClassTypeTree"/> that best matches the
        /// given <paramref name="type"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        public ClassTypeTree Find(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsClass)
                return Root;

            return InternalFind(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassTypeTree"/> class.
        /// </summary>
        /// <param name="types">The types to build the tree out of.</param>
        public ClassTypeTree(IEnumerable<Type> types)
        {
            var childList = new List<ClassTypeTree>();

            // Remove duplicates
            types = types.Where(x => x.IsClass).Distinct();

            // Grab the lowest-level types
            var baseTypes = types.Where(x => !types.Any(y => x != y && y.IsAssignableFrom(x)));

            // Grab the possible child types
            var remainingTypes = types.Except(baseTypes);
            foreach (var bt in baseTypes)
            {
                // Recursively build the tree
                childList.Add(new ClassTypeTree(this, bt, remainingTypes));
            }

            // Add the wildcard for the base level
            childList.Add(new ClassTypeTree(this, null, null));

            // Store the children
            _children = FinalizeChildren(childList);
        }

        /// <summary>
        /// Turns the temporary child list into the final version we want to store. This way we get nicely compacted and
        /// processed list to remove as much overhead as possible.
        /// </summary>
        /// <param name="childTypes">The list of child nodes.</param>
        /// <returns>Turns the temporary child list into the final version we want to store.</returns>
        static IEnumerable<ClassTypeTree> FinalizeChildren(IEnumerable<ClassTypeTree> childTypes)
        {
            return childTypes.OrderBy(x => x.Type != null ? x.Type.Name : string.Empty).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassTypeTree"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="selfType">Type of the this node.</param>
        /// <param name="types">The possible child types.</param>
        ClassTypeTree(ClassTypeTree parent, Type selfType, IEnumerable<Type> types)
        {
            _parent = parent;
            _selfType = selfType;
            
            // Abort if this is a wildcard leaf
            if (selfType == null || types == null)
                return;

            // Take all types that are not this type, and that can be assigned from this type
            types = types.Where(x => x != selfType && _selfType.IsAssignableFrom(x));

            // Only take the types that cannot be assigned by the other child types, giving us only the immediate
            // children instead of ALL children
            var childTypes = types.Where(x => !types.Any(y => x != y && x.IsSubclassOf(y)));

            // Check if we have any children
            if (childTypes.Count() > 0)
            {
                var childList = new List<ClassTypeTree>();

                // Recursively build the children
                foreach (var bt in childTypes)
                {
                    childList.Add(new ClassTypeTree(this, bt, types));
                }

                // Add the wildcard that will catch anything that does not fit the other children
                childList.Add(new ClassTypeTree(this, null, null));

                _children = FinalizeChildren(childList);
            }
        }
    }
}
