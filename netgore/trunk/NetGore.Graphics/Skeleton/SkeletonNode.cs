using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// A n (joint) for a skeleton model
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
        /// Nodes that belong to this n
        /// </summary>
        readonly List<SkeletonNode> _nodes = new List<SkeletonNode>();

        /// <summary>
        /// Name of the n. The n name must be unique throughout the whole skeleton.
        /// </summary>
        string _name = "New n";

        /// <summary>
        /// Node that this n is attached to (null if none)
        /// </summary>
        SkeletonNode _parent;

        /// <summary>
        /// Absolute position of the n
        /// </summary>
        Vector2 _position;

        /// <summary>
        /// Gets or sets if the n is used when the n is part of a skeleton treated as an animation modifier
        /// </summary>
        public bool IsModifier { get; set; }

        /// <summary>
        /// Gets or sets the name of the n. The n name must be unique throughout the whole skeleton.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets the nodes that belong to this n
        /// </summary>
        public List<SkeletonNode> Nodes
        {
            get { return _nodes; }
        }

        /// <summary>
        /// Gets or sets the n that this n is attached to (null if none)
        /// </summary>
        public SkeletonNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the absolute position of the n
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Gets or sets the X position of the n (equal to Position.X)
        /// </summary>
        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        /// <summary>
        /// Gets or sets the Y position of the n (equal to Position.Y)
        /// </summary>
        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        public SkeletonNode(Vector2 position)
        {
            _parent = null;
            _position = position;
        }

        public SkeletonNode(SkeletonNode parent, Vector2 position)
        {
            _parent = parent;
            _parent.Nodes.Add(this);
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
        /// Adds a child n to the n
        /// </summary>
        /// <param name="n">Child n to add</param>
        public void Add(SkeletonNode node)
        {
            if (node == null)
            {
                Debug.Fail("n is null.");
                return;
            }

            _nodes.Add(node);
            node.Parent = this;
        }

        /// <summary>
        /// Creates a deep-copy of the n and all its child nodes
        /// </summary>
        /// <returns>Duplicate copy of the n</returns>
        public SkeletonNode Duplicate()
        {
            return Duplicate(this);
        }

        /// <summary>
        /// Recursively creates a deep-copy of the n and all its child nodes
        /// </summary>
        /// <param name="root">Node to duplicate</param>
        /// <returns>Duplicate copy of the n</returns>
        static SkeletonNode Duplicate(SkeletonNode root)
        {
            if (root == null)
            {
                Debug.Fail("root is null.");
                return null;
            }

            SkeletonNode ret = new SkeletonNode(new Vector2(root.Position.X, root.Position.Y)) { Name = root.Name };
            foreach (SkeletonNode n in root.Nodes)
            {
                SkeletonNode newChild = Duplicate(n);
                newChild.Parent = ret;
                newChild.Name = n.Name;
                newChild.IsModifier = n.IsModifier;
                ret.Nodes.Add(newChild);
            }
            return ret;
        }

        /// <summary>
        /// Recursively acquires all children nodes from a given root n
        /// </summary>
        /// <returns>A list containing this n and all of its children</returns>
        public List<SkeletonNode> GetAllNodes()
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
            return (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }

        /// <summary>
        /// Finds the angle between this n and another
        /// </summary>
        /// <param name="n">Node to compare to</param>
        /// <returns>Angle between the two nodes</returns>
        public float GetAngle(SkeletonNode node)
        {
            if (node == null)
                return 0f;

            return GetAngle(node.Position, Position);
        }

        /// <summary>
        /// Finds the angle between this n and its parent
        /// </summary>
        /// <returns>Angle between the two nodes</returns>
        public float GetAngle()
        {
            return GetAngle(Parent);
        }

        /// <summary>
        /// Finds the length of the bone between the n and its parent
        /// </summary>
        /// <returns>Length of the bone</returns>
        public float GetLength()
        {
            if (Parent == null)
                return 0f;

            return (float)Math.Round(Vector2.Distance(Position, Parent.Position), 4);
        }

        /// <summary>
        /// Used to assist the recursive n acquisition
        /// </summary>
        /// <param name="root">Root n to work from</param>
        /// <returns>Children of the root</returns>
        static List<SkeletonNode> GetNodes(SkeletonNode root)
        {
            if (root == null)
            {
                Debug.Fail("root is null.");
                return new List<SkeletonNode>();
            }

            var ret = new List<SkeletonNode>();
            foreach (SkeletonNode node in root.Nodes)
            {
                ret.Add(node);
                foreach (SkeletonNode subNode in GetNodes(node))
                {
                    ret.Add(subNode);
                }
            }
            return ret;
        }

        /// <summary>
        /// Checks if a given point hits the n
        /// </summary>
        /// <param name="camera">World camera (determines n size)</param>
        /// <param name="position">World position</param>
        /// <param name="n">Node to test against</param>
        /// <returns>True if the n was hit, else false</returns>
        public static bool HitTest(Camera2D camera, Vector2 position, SkeletonNode node)
        {
            if (node == null)
            {
                Debug.Fail("n is null.");
                return false;
            }
            if (camera == null)
            {
                Debug.Fail("camera is null.");
                return false;
            }

            Vector2 size = Vector2.Divide(HalfJointVector, camera.Scale);
            Vector2 min = node.Position - size;
            Vector2 max = node.Position + size;
            return (position.X >= min.X && position.X <= max.X && position.Y >= min.Y && position.Y <= max.Y);
        }

        /// <summary>
        /// Checks if a given point hits the n
        /// </summary>
        /// <param name="camera">World camera (determines n size)</param>
        /// <param name="position">World position</param>
        /// <returns>True if the n was hit, else false</returns>
        public bool HitTest(Camera2D camera, Vector2 position)
        {
            return HitTest(camera, position, this);
        }

        /// <summary>
        /// Move the n to a new position while retaining the structure of the child nodes
        /// </summary>
        /// <param name="newPosition">New position to move the n to</param>
        public void MoveTo(Vector2 newPosition)
        {
            // Apply the position difference to this n and all its children
            Vector2 diff = Position - newPosition;
            if (diff.X != 0 || diff.Y != 0)
                RecursiveMove(this, diff);
        }

        /// <summary>
        /// Reads the SkeletonNode's values from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>The name of the parent SkeletonNode, or null if the SkeletonNode has no parent (root n).
        /// The SkeletonNode's parent must be set manually using this value.</returns>
        public string Read(IValueReader reader)
        {
            Name = reader.ReadString(_nameValueKey);
            Position = reader.ReadVector2(_positionValueKey);
            IsModifier = reader.ReadBool(_isModifierValueKey);
            bool hasParent = reader.ReadBool(_hasParentValueKey);

            string parentName = null;
            if (hasParent)
                parentName = reader.ReadString(_parentNameValueKey);

            return parentName;
        }

        /// <summary>
        /// Recursively moves a n and all its children by a given value
        /// </summary>
        /// <param name="n">Root n to move</param>
        /// <param name="diff">Distance to move the n</param>
        static void RecursiveMove(SkeletonNode node, Vector2 diff)
        {
            // Move the n
            node.Position -= diff;

            // Update the children
            foreach (SkeletonNode n in node.Nodes)
            {
                RecursiveMove(n, diff);
            }
        }

        /// <summary>
        /// Removes the n and all of its children from the skeleton
        /// </summary>
        public void Remove()
        {
            if (Parent != null)
            {
                Parent.Nodes.Remove(this);
                Parent = null;
            }
        }

        /// <summary>
        /// Rotates all the child nodes of the n
        /// </summary>
        /// <param name="radians">Angle, in radians, of the initial joint</param>
        public void Rotate(float radians)
        {
            // Find the difference between the current angle and the new angle
            float diff = GetAngle() - radians;

            // Loop through the children, performing the same rotation task
            foreach (SkeletonNode node in Nodes)
            {
                node.Rotate(node.GetAngle() - diff);
            }

            // Finally set the angle. Due to the recursive structure, this will end up set
            // first on the lowest-level nodes, and last on the highest level n.
            SetAngle(radians);
        }

        /// <summary>
        /// Rotates all the child nodes of the n
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
        /// Adjusts the n's position without changing the distance
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
        /// Adjusts the n's position without changing the distanec to
        /// match the given angle. Node must contain a parent.
        /// </summary>
        /// <param name="radians">The angle, in radians, to create between the n and its parent</param>
        public void SetAngle(float radians)
        {
            // Find the distance between the n and its parent so we can preserve it
            float dist = Vector2.Distance(Position, Parent.Position);

            // Find the new position the n will be taking and move to it
            Vector2 newPos = new Vector2((float)Math.Cos(radians) * dist, (float)Math.Sin(radians) * dist);
            MoveTo(newPos + Parent.Position);
        }

        /// <summary>
        /// Sets the length of the bone (defined by the distance between a n and its parent)
        /// </summary>
        /// <param name="length">New length</param>
        public void SetLength(float length)
        {
            if (Parent == null)
                return;

            SetLength(length, GetAngle());
        }

        /// <summary>
        /// Sets the length of the bone (defined by the distance between a n and its parent)
        /// </summary>
        /// <param name="length">New length</param>
        /// <param name="radians">Angle of the n to its parent in radians</param>
        public void SetLength(float length, float radians)
        {
            Vector2 newPos = new Vector2((float)Math.Cos(radians) * length, (float)Math.Sin(radians) * length);
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