using System.Diagnostics;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Assists in drawing the raw structure (joints and bones) of a single skeleton
    /// </summary>
    public static class SkeletonDrawer
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
        /// Sprite used for drawing the joints.
        /// </summary>
        static readonly ISprite _joint = null;

        static readonly Color _nodeColorRoot = new Color(0, 0, 0, 255);
        static readonly Color _nodeColorSelected = new Color(255, 255, 255, 255);

        /// <summary>
        /// Initializes the <see cref="SkeletonDrawer"/> class.
        /// </summary>
        static SkeletonDrawer()
        {
            _joint = SystemSprites.Joint;
        }

        /// <summary>
        /// Draws a <see cref="Skeleton"/>.
        /// </summary>
        /// <param name="skeleton">The <see cref="Skeleton"/> to draw.</param>
        /// <param name="camera">Camera to use.</param>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw with.</param>
        /// <param name="selectedNode">The <see cref="SkeletonNode"/> to draw as selected.</param>
        public static void Draw(Skeleton skeleton, ICamera2D camera, ISpriteBatch sb, SkeletonNode selectedNode = null)
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

            RecursiveDraw(camera, sb, selectedNode, skeleton.RootNode, 0);
        }

        /// <summary>
        /// Recursively draws the joints and bones of a skeleton.
        /// </summary>
        /// <param name="camera">Camera to use.</param>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="selectedNode">SpriteBatch to draw to.</param>
        /// <param name="node">Current node being drawn.</param>
        /// <param name="colorIndex">Index of the color to use from the ColorList.</param>
        static void RecursiveDraw(ICamera2D camera, ISpriteBatch sb, SkeletonNode selectedNode, SkeletonNode node, int colorIndex)
        {
            // Find the color of the joint
            var color = _colorList[colorIndex];
            if (node == selectedNode)
                color = _nodeColorSelected;
            else if (node.Parent == null)
                color = _nodeColorRoot;

            // Draw the joint
            var scale = 1f / camera.Scale;
            var origin = SkeletonNode.HalfJointVector;
            _joint.Draw(sb, node.Position, color, SpriteEffects.None, 0f, origin, scale);

            // Iterate through the children
            foreach (var child in node.Nodes)
            {
                colorIndex++;
                if (colorIndex == _colorList.Length)
                    colorIndex = 0;

                // Draw the bone to the child
                RenderLine.Draw(sb, node.Position, child.Position, _colorList[colorIndex], (1f / camera.Scale) * 2f);

                // Draw the child
                RecursiveDraw(camera, sb, selectedNode, child, colorIndex);
            }
        }
    }
}