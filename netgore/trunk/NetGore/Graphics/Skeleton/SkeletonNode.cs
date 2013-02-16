using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A node (joint) for a skeleton model
    /// </summary>
    public class SkeletonNode
    {
        /// <summary>
        /// Size of the joint
        /// </summary>
        public const float JointSize = 6;

        /// <summary>
        /// Half the size of the joint
        /// </summary>
        internal const float HalfJointSize = JointSize / 2.0f;

        const string _hasParentValueKey = "HasParent";
        const string _isModifierValueKey = "IsModifier";
        const string _nameValueKey = "Name";
        const string _parentNameValueKey = "ParentName";
        const string _positionValueKey = "Position";

        /// <summary>
        /// A vector representing half the size of the joint
        /// </summary>
        internal static readonly Vector2 HalfJointVector = new Vector2(HalfJointSize, HalfJointSize);

        /// <summary>
        /// Nodes that belong to this node
        /// </summary>
        readonly List<SkeletonNode> _nodes = new List<SkeletonNode>();

        /// <summary>
        /// Name of the node. The node name must be unique throughout the whole skeleton.
        /// </summary>
        string _name = "New node";

        SkeletonNode _parent;
        Vector2 _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonNode"/> class.
        /// </summary>
        /// <param name="position">The position of the node relative to the <see cref="Skeleton"/>.</param>
        public SkeletonNode(Vector2 position)
        {
            _parent = null;
            _position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonNode"/> class.
        /// </summary>
        /// <param name="parent">The parent <see cref="SkeletonNode"/>. If null, this will be treated as the root node in the
        /// <see cref="Skeleton"/>.</param>
        /// <param name="position">The position of the node relative to the <see cref="Skeleton"/>.</param>
        public SkeletonNode(SkeletonNode parent, Vector2 position)
        {
            _parent = parent;
            _parent._nodes.Add(this);
            _position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonNode"/> class.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <param name="parentName">Name of this SkeletonNode's parent, or null if the SkeletonNode
        /// has no parent. This value must be used to manually set the SkeletonNode's parent.</param>
        public SkeletonNode(IValueReader reader, out string parentName)
        {
            parentName = Read(reader);
        }

        /// <summary>
        /// Gets or sets if the node is used when the node is part of a <see cref="Skeleton"/> treated as an animation modifier.
        /// When this is true and the node is used as a modifier of another <see cref="Skeleton"/>, this node will be used
        /// instead of the corresponding node in the base <see cref="Skeleton"/>. If false, the node will be ignored.
        /// </summary>
        public bool IsModifier { get; set; }

        /// <summary>
        /// Gets or sets the name of the node. The node name must be unique throughout the whole skeleton.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets the nodes that belong to this node
        /// </summary>
        public IEnumerable<SkeletonNode> Nodes
        {
            get { return _nodes; }
        }

        /// <summary>
        /// Gets or sets the node that this node is attached to. When this is the root node in the <see cref="Skeleton"/>, this
        /// value will be null.
        /// </summary>
        public SkeletonNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the position of this <see cref="SkeletonNode"/> relative to the <see cref="Skeleton"/> that the node
        /// is part of.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Gets or sets the X position of the node.
        /// </summary>
        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        /// <summary>
        /// Gets or sets the Y position of the node.
        /// </summary>
        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        /// <summary>
        /// Gets the internal list of child <see cref="SkeletonNode"/>s in this <see cref="SkeletonNode"/>.
        /// </summary>
        internal List<SkeletonNode> internalNodes
        {
            get { return _nodes; }
        }

        /// <summary>
        /// Adds a child node to the node
        /// </summary>
        /// <param name="node">Child node to add</param>
        public void Add(SkeletonNode node)
        {
            if (node == null)
            {
                Debug.Fail("node is null.");
                return;
            }

            _nodes.Add(node);
            node.Parent = this;
        }

        /// <summary>
        /// Creates a deep-copy of the node and all its child nodes
        /// </summary>
        /// <returns>Duplicate copy of the node</returns>
        public SkeletonNode Duplicate()
        {
            return Duplicate(this);
        }

        /// <summary>
        /// Recursively creates a deep-copy of the node and all its child nodes
        /// </summary>
        /// <param name="root">Node to duplicate</param>
        /// <returns>Duplicate copy of the node</returns>
        static SkeletonNode Duplicate(SkeletonNode root)
        {
            if (root == null)
            {
                Debug.Fail("root is null.");
                return null;
            }

            var ret = new SkeletonNode(new Vector2(root.Position.X, root.Position.Y)) { Name = root.Name };
            foreach (var node in root.Nodes)
            {
                var newChild = Duplicate(node);
                newChild.Parent = ret;
                newChild.Name = node.Name;
                newChild.IsModifier = node.IsModifier;
                ret._nodes.Add(newChild);
            }
            return ret;
        }

        /// <summary>
        /// Recursively acquires all children nodes from a given root node
        /// </summary>
        /// <returns>A list containing this node and all of its children</returns>
        public ICollection<SkeletonNode> GetAllNodes()
        {
            var ret = GetNodes(this);
            ret.Add(this);
            return ret;
        }

        /// <summary>
        /// Finds the angle between two points
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <returns>Angle between the points</returns>
        public static float GetAngle(Vector2 p1, Vector2 p2)
        {
            float angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);

            // Clamp our angle good between -Pi and Pi
            angle = MathHelper.WrapAngle(angle);

            return angle;
        }

        /// <summary>
        /// Finds the angle between this node and another
        /// </summary>
        /// <param name="node">Node to compare to</param>
        /// <returns>Angle between the two nodes</returns>
        public float GetAngle(SkeletonNode node)
        {
            if (node == null)
                return 0f;

            return GetAngle(node.Position, Position);
        }

        /// <summary>
        /// Finds the angle between this node and its parent
        /// </summary>
        /// <returns>Angle between the two nodes</returns>
        public float GetAngle()
        {
            return GetAngle(Parent);
        }

        /// <summary>
        /// Finds the length of the bone between the node and its parent
        /// </summary>
        /// <returns>Length of the bone</returns>
        public float GetLength()
        {
            if (Parent == null)
                return 0f;

            return (float)Math.Round(Vector2.Distance(Position, Parent.Position), 4);
        }

        /// <summary>
        /// Used to assist the recursive node acquisition
        /// </summary>
        /// <param name="root">Root node to work from</param>
        /// <returns>Children of the root</returns>
        static List<SkeletonNode> GetNodes(SkeletonNode root)
        {
            if (root == null)
            {
                Debug.Fail("root is null.");
                return new List<SkeletonNode>();
            }

            var ret = new List<SkeletonNode>();
            foreach (var node in root.Nodes)
            {
                ret.Add(node);
                foreach (var subNode in GetNodes(node))
                {
                    ret.Add(subNode);
                }
            }
            return ret;
        }

        /// <summary>
        /// Checks if a given point hits the node
        /// </summary>
        /// <param name="camera">World camera (determines node size)</param>
        /// <param name="position">World position</param>
        /// <param name="node">Node to test against</param>
        /// <returns>True if the node was hit, else false</returns>
        public static bool HitTest(ICamera2D camera, Vector2 position, SkeletonNode node)
        {
            if (node == null)
            {
                Debug.Fail("node is null.");
                return false;
            }
            if (camera == null)
            {
                Debug.Fail("camera is null.");
                return false;
            }

            var size = Vector2.Divide(HalfJointVector, camera.Scale);
            var min = node.Position - size;
            var max = node.Position + size;
            return (position.X >= min.X && position.X <= max.X && position.Y >= min.Y && position.Y <= max.Y);
        }

        /// <summary>
        /// Checks if a given point hits the node
        /// </summary>
        /// <param name="camera">World camera (determines node size)</param>
        /// <param name="position">World position</param>
        /// <returns>True if the node was hit, else false</returns>
        public bool HitTest(ICamera2D camera, Vector2 position)
        {
            return HitTest(camera, position, this);
        }

        /// <summary>
        /// Move the node to a new position while retaining the structure of the child nodes
        /// </summary>
        /// <param name="newPosition">New position to move the node to</param>
        public void MoveTo(Vector2 newPosition)
        {
            // Apply the position difference to this node and all its children
            var diff = Position - newPosition;
            if (diff.X != 0 || diff.Y != 0)
                RecursiveMove(this, diff);
        }

        /// <summary>
        /// Reads the SkeletonNode's values from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>The name of the parent SkeletonNode, or null if the SkeletonNode has no parent (root node).
        /// The SkeletonNode's parent must be set manually using this value.</returns>
        public string Read(IValueReader reader)
        {
            Name = reader.ReadString(_nameValueKey);
            Position = reader.ReadVector2(_positionValueKey);
            IsModifier = reader.ReadBool(_isModifierValueKey);
            var hasParent = reader.ReadBool(_hasParentValueKey);

            string parentName = null;
            if (hasParent)
                parentName = reader.ReadString(_parentNameValueKey);

            return parentName;
        }

        /// <summary>
        /// Recursively moves a node and all its children by a given value
        /// </summary>
        /// <param name="node">Root node to move</param>
        /// <param name="diff">Distance to move the node</param>
        static void RecursiveMove(SkeletonNode node, Vector2 diff)
        {
            // Move the node
            node.Position -= diff;

            // Update the children
            foreach (var childNode in node.Nodes)
            {
                RecursiveMove(childNode, diff);
            }
        }

        /// <summary>
        /// Removes the node and all of its children from the skeleton
        /// </summary>
        public void Remove()
        {
            if (Parent != null)
            {
                Parent._nodes.Remove(this);
                Parent = null;
            }
        }

        /// <summary>
        /// Rotates all the child nodes of the node
        /// </summary>
        /// <param name="radians">Angle, in radians, of the initial joint</param>
        public void Rotate(float radians)
        {
            // Find the difference between the current angle and the new angle
            var diff = GetAngle() - radians;

            // Loop through the children, performing the same rotation task
            foreach (var node in Nodes)
            {
                node.Rotate(node.GetAngle() - diff);
            }

            // Finally set the angle. Due to the recursive structure, this will end up set
            // first on the lowest-level nodes, and last on the highest level node.
            SetAngle(radians);
        }

        /// <summary>
        /// Rotates all the child nodes of the node
        /// </summary>
        /// <param name="newPosition">New position to rotate to</param>
        public void Rotate(Vector2 newPosition)
        {
            if (Parent == null)
                return;

            // Rotate to match the new position
            Rotate(GetAngle(Parent.Position, newPosition));
        }

        /// <summary>
        /// Adjusts the node's position without changing the distance
        /// to match the angle of a given point. Node must contain a parent.
        /// </summary>
        /// <param name="target">Target point to face</param>
        public void SetAngle(Vector2 target)
        {
            if (Parent == null)
                return;

            SetAngle(GetAngle(Parent.Position, target));
        }

        /// <summary>
        /// Adjusts the node's position without changing the distanec to
        /// match the given angle. Node must contain a parent.
        /// </summary>
        /// <param name="radians">The angle, in radians, to create between the node and its parent</param>
        public void SetAngle(float radians)
        {
            // Find the distance between the node and its parent so we can preserve it
            var dist = Vector2.Distance(Position, Parent.Position);

            // Find the new position the node will be taking and move to it
            var newPos = new Vector2((float)Math.Cos(radians) * dist, (float)Math.Sin(radians) * dist);
            MoveTo(newPos + Parent.Position);
        }

        /// <summary>
        /// Sets the length of the bone (defined by the distance between a node and its parent)
        /// </summary>
        /// <param name="length">New length</param>
        public void SetLength(float length)
        {
            if (Parent == null)
                return;

            SetLength(length, GetAngle());
        }

        /// <summary>
        /// Sets the length of the bone (defined by the distance between a node and its parent)
        /// </summary>
        /// <param name="length">New length</param>
        /// <param name="radians">Angle of the node to its parent in radians</param>
        public void SetLength(float length, float radians)
        {
            var newPos = new Vector2((float)Math.Cos(radians) * length, (float)Math.Sin(radians) * length);
            MoveTo(newPos + Parent.Position);
        }

        /// <summary>
        /// Writes the SkeletonNode to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write(_nameValueKey, Name);
            writer.Write(_positionValueKey, Position);
            writer.Write(_isModifierValueKey, IsModifier);
            writer.Write(_hasParentValueKey, Parent != null);

            if (Parent != null)
                writer.Write(_parentNameValueKey, Parent.Name);
        }
    }
}