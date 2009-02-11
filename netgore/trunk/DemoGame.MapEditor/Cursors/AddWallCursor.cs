using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Extensions;
using Platyform;

namespace DemoGame.MapEditor
{
    class AddWallCursor : EditorCursorBase
    {
        public override void MouseDown(ScreenForm screen, MouseEventArgs e)
        {
            // Switch to the wall editing tool
            screen.toolBarItem_Click(screen.picToolWalls, null);

            // Create the new wall
            Wall w = Entity.Create<Wall>(screen.Camera.ToWorld(e.X, e.Y), 1f, 1f);
            screen.Map.AddEntity(w);
            w.CollisionType = (CollisionType)screen.cmbWallType.SelectedItem;
            if (screen.chkSnapWallGrid.Checked)
                screen.Grid.Align(w);

            // Create the transformation boxes for the wall and select the bottom/right one
            screen.TransBoxes.Clear();
            TransBox.SurroundEntity(w, screen.TransBoxes);
            foreach (TransBox tBox in screen.TransBoxes)
            {
                if (tBox.TransType == TransBoxType.BottomRight)
                {
                    screen.SelectedTransBox = tBox;
                    break;
                }
            }

            screen.UpdateSelectedWallsList(new List<WallEntity>(1) { w });
        }
    }
}