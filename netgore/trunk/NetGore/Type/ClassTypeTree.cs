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
        readonly List<ClassTypeTree> _children;
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

        public ClassTypeTree(IEnumerable<Type> types)
        {
            _children = new List<ClassTypeTree>();

            types = types.Where(x => x.IsClass).Distinct();

            var baseTypes = types.Where(x => !types.Any(y => x != y && y.IsAssignableFrom(x)));

            var remainingTypes = types.Except(baseTypes);
            foreach (var bt in baseTypes)
            {
                _children.Add(new ClassTypeTree(this, bt, remainingTypes));
            }

            _children.Add(new ClassTypeTree(this, null, null));

            _children.TrimExcess();
        }

        ClassTypeTree(ClassTypeTree parent, Type selfType, IEnumerable<Type> types)
        {
            _parent = parent;
            _selfType = selfType;

            if (selfType == null || types == null)
                return;

            types = types.Where(x => x != selfType && _selfType.IsAssignableFrom(x));
            var childTypes = types.Where(x => !types.Any(y => x != y && x.IsSubclassOf(y)));

            if (childTypes.Count() > 0)
            {
                _children = new List<ClassTypeTree>();

                foreach (var bt in childTypes)
                {
                    _children.Add(new ClassTypeTree(this, bt, types));
                }

                _children.Add(new ClassTypeTree(this, null, null));

                _children.TrimExcess();
            }
        }
    }
}
