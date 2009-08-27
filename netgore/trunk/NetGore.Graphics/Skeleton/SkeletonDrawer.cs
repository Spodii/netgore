using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Assists in drawing the raw structure (joints and bones) of a single skeleton
    /// </summary>
    public class SkeletonDrawer
    {
        /// <summary>
        /// List of colors to draw with
        /// </summary>
        static readonly Color[] _colorList = new[]
        {
            new Color(0, 255, 255), new Color(255, 64, 160), new Color(255, 128, 64), new Color(0, 255, 0), new Color(255, 255, 0),
            new Color(35, 25, 255)
        };

        /// <summary>
        /// XNALine for drawing lines. Because the points and color are always set before
        /// using this line, it can be static.
        /// </summary>
        static readonly XNALine _line = new XNALine();

        static readonly Color _nodeColorRoot = new Color(0, 0, 0, 255);
        static readonly Color _nodeColorSelected = new Color(255, 255, 255, 255);

        /// <summary>
        /// Grh used for drawing the joints
        /// </summary>
        static Grh _joint = null;

        /// <summary>
        /// Draws a skeleton
        /// </summary>
        /// <param name="skeleton">Skeleton to draw</param>
        /// <param name="camera">Camera to use</param>
        /// <param name="sb">SpriteBatch to draw to</param>
        public void Draw(Skeleton skeleton, Camera2D camera, SpriteBatch sb)
        {
            Draw(skeleton, camera, sb, null);
        }

        /// <summary>
        /// Draws a skeleton
        /// </summary>
        /// <param name="skeleton">Skeleton to draw</param>
        /// <param name="camera">Camera to use</param>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="selectedNode">SpriteBatch to draw to</param>
        public void Draw(Skeleton skeleton, Camera2D camera, SpriteBatch sb, SkeletonNode selectedNode)
        {
            if (skeleton == null)
            {
                Debug.Fail("skeleton is null.");
                return;
            }
            if (skeleton.RootNode == null)
            {
                Debug.Fail("skeleton contains no root node.");
                return;
            }
            if (sb == null)
            {
                Debug.Fail("sb is null.");
                return;
            }
            if (sb.IsDisposed)
            {
                Debug.Fail("sb is disposed.");
                return;
            }
            if (camera == null)
            {
                Debug.Fail("camera is null.");
                return;
            }

            LoadStaticGrhs();
            if (_joint == null)
                return;

            _line.Thickness = 1f / camera.Scale;
            RecursiveDraw(camera, sb, selectedNode, skeleton.RootNode, 0);
        }

        /// <summary>
        /// Loads the static Grhs if needed
        /// </summary>
        static void LoadStaticGrhs()
        {
            if (_joint == null)
            {
                GrhData gd = GrhInfo.GetData("System", "Joint");
                if (gd == null)
                    throw new Exception("Failed to load GrhData System.Joint.");
                _joint = new Grh(gd);
            }
        }

        /// <summary>
        /// Recursively draws the joints and bones of a skeleton
        /// </summary>
        /// <param name="camera">Camera to use</param>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="selectedNode">SpriteBatch to draw to</param>
        /// <param name="node">Current node being drawn</param>
        /// <param name="colorIndex">Index of the color to use from the ColorList</param>
        static void RecursiveDraw(Camera2D camera, SpriteBatch sb, SkeletonNode selectedNode, SkeletonNode node, int colorIndex)
        {
            // Find the color of the joint
            Color color = _colorList[colorIndex];
            if (node == selectedNode)
                color = _nodeColorSelected;
            else if (node.Parent == null)
                color = _nodeColorRoot;

            // Draw the joint
            float scale = 1f / camera.Scale;
            Vector2 origin = SkeletonNode.HalfJointVector;
            _joint.Draw(sb, node.Position, color, SpriteEffects.None, 0f, origin, scale);

            // Iterate through the children
            foreach (SkeletonNode child in node.Nodes)
            {
                colorIndex++;
                if (colorIndex == _colorList.Length)
                    colorIndex = 0;

                // Draw the bone to the child
                _line.SetPoints(node.Position, child.Position);
                _line.Draw(sb, _colorList[colorIndex]);

                // Draw the child
                RecursiveDraw(camera, sb, selectedNode, child, colorIndex);
            }
        }
    }
}