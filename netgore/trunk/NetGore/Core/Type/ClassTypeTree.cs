using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Describes an inheritance hierarchy for a set of class types.
    /// </summary>
    public class ClassTypeTree
    {
        readonly IEnumerable<ClassTypeTree> _children;
        readonly ClassTypeTree _parent;
        readonly Type _selfType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassTypeTree"/> class.
        /// </summary>
        /// <param name="types">The types to build the tree out of.</param>
        public ClassTypeTree(IEnumerable<Type> types)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor

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
                childList.Add(CreateNode(this, bt, remainingTypes));
            }

            // Add the wildcard for the base level
            childList.Add(CreateNode(this, null, null));

            // Store the children
            _children = FinalizeChildren(childList);

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassTypeTree"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="selfType">Type of the this node.</param>
        /// <param name="types">The possible child types.</param>
        protected ClassTypeTree(ClassTypeTree parent, Type selfType, IEnumerable<Type> types)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor

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
            if (!childTypes.IsEmpty())
            {
                var childList = new List<ClassTypeTree>();

                // Recursively build the children
                foreach (var bt in childTypes)
                {
                    childList.Add(CreateNode(this, bt, types));
                }

                // Add the wildcard that will catch anything that does not fit the other children
                childList.Add(CreateNode(this, null, null));

                _children = FinalizeChildren(childList);
            }

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Gets this node's child nodes.
        /// </summary>
        public IEnumerable<ClassTypeTree> Children
        {
            get
            {
                if (_children == null)
                    return Enumerable.Empty<ClassTypeTree>();

                return _children;
            }
        }

        /// <summary>
        /// Gets if this <see cref="ClassTypeTree"/> node is a leaf node (has no children).
        /// </summary>
        public bool IsLeaf
        {
            get { return _children == null; }
        }

        /// <summary>
        /// Gets this node's parent node. Only null if this is the root node.
        /// </summary>
        public ClassTypeTree Parent
        {
            get { return _parent; }
        }

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

        /// <summary>
        /// Gets the <see cref="Type"/> that this node contains. If null, this node is used for all <see cref="Type"/>s
        /// that match the node's parent, but not any of the parent's other children. That is, it is like a wildcard.
        /// </summary>
        public Type Type
        {
            get { return _selfType; }
        }

        /// <summary>
        /// Creates a <see cref="ClassTypeTree"/> using the given values. This allows derived types of the
        /// <see cref="ClassTypeTree"/> to have their derived type be used for every node in the tree.
        /// </summary>
        /// <param name="parent">The parent node.</param>
        /// <param name="selfType">The type of this node.</param>
        /// <param name="types">All of the possible child nodes.</param>
        /// <returns>A <see cref="ClassTypeTree"/> using the given values.</returns>
        protected virtual ClassTypeTree CreateNode(ClassTypeTree parent, Type selfType, IEnumerable<Type> types)
        {
            return new ClassTypeTree(parent, selfType, types);
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
        /// Finds the <see cref="ClassTypeTree"/> from this <see cref="ClassTypeTree"/> that best matches the
        /// given <paramref name="findType"/>. If the <paramref name="findType"/> is not a class, the returned
        /// <see cref="ClassTypeTree"/> will always be the root.
        /// </summary>
        /// <param name="findType">The <see cref="Type"/> to find the tree node for.</param>
        /// <returns>The <see cref="ClassTypeTree"/> from this <see cref="ClassTypeTree"/> that best matches the
        /// given <paramref name="findType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="findType"/> is null.</exception>
        public ClassTypeTree Find(Type findType)
        {
            if (findType == null)
                throw new ArgumentNullException("findType");

            if (!findType.IsClass)
                return Root;

            return InternalFind(findType);
        }

        /// <summary>
        /// Gets all of the nodes in the whole tree.
        /// </summary>
        /// <returns>All of the nodes in the whole tree.</returns>
        public IEnumerable<ClassTypeTree> GetAllNodes()
        {
            return Root.GetAllNodesInternal();
        }

        IEnumerable<ClassTypeTree> GetAllNodesInternal()
        {
            yield return this;

            foreach (var child in Children)
            {
                foreach (var c in child.GetAllNodesInternal())
                {
                    yield return c;
                }
            }
        }

        ClassTypeTree InternalFind(Type findType)
        {
            if (findType == Type)
                return this;

            // Ensure the type is assignable from this node
            // This little snippet is what allows us to use Find() from anywhere in the tree, not just from the root
            if (Type != null && !Type.IsAssignableFrom(findType))
                return Parent.InternalFind(findType);

            if (!IsLeaf)
            {
                // Grab the non-wildcard children since the last thing we want to match against is the wildcard
                var nonWildcardChildren = Children.Where(x => x.Type != null);

                // Check if any of the child types can be assigned from (or are equal to) the type to find
                foreach (var child in nonWildcardChildren)
                {
                    if (child.Type.IsAssignableFrom(findType))
                        return child.InternalFind(findType);
                }

                // The type to find didn't fit any of the children, which means that none of the children were equal to the
                // type to find nor can the type to find be casted to the child type (that is, they are on a different branch
                // on the type hierarchy, or they are too derived). So before we go straight for the wildcard node, we want to
                // check if the type to find CAN be in between this node and any of the children.
                if (nonWildcardChildren.Any(x => x.Type.IsSubclassOf(findType)))
                    return this;
                else
                    return _children.First(x => x.Type == null);
            }
            else
                return this;
        }
    }
}