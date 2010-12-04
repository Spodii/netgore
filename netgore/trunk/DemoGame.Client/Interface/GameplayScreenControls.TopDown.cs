using System.Linq;

#if !TOPDOWN
#pragma warning disable 1587
#endif

#if TOPDOWN

using System;
using System.Collections.Generic;
using System.Text;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    partial class GameplayScreenControls
    {
        /// <summary>
        /// Creates the controls specific to top-down builds.
        /// </summary>
        void CreateControlsForPerspective()
        {
            CreateAndAdd(GameControlsKeys.MoveUp, _minMoveRate, () => !UserChar.IsMovingUp && CanUserMove(),
                         HandleGameControl_MoveUp);

            CreateAndAdd(GameControlsKeys.MoveDown, _minMoveRate, () => !UserChar.IsMovingDown && CanUserMove(),
                         HandleGameControl_MoveDown);

            CreateAndAdd(GameControlsKeys.MoveStopHorizontal, _minMoveRate, () => UserChar.IsMovingLeft || UserChar.IsMovingRight, HandleGameControl_MoveStopHorizontal);

            CreateAndAdd(GameControlsKeys.MoveStopVertical, _minMoveRate, () => UserChar.IsMovingUp || UserChar.IsMovingDown, HandleGameControl_MoveStopVertical);
        }

        void HandleGameControl_MoveUp(GameControl sender, EventArgs e)
        {
            using (var pw = ClientPacket.MoveUp())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }

        void HandleGameControl_MoveStopVertical(GameControl sender, EventArgs e)
        {
            using (var pw = ClientPacket.MoveStopVertical())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }

        void HandleGameControl_MoveStopHorizontal(GameControl sender, EventArgs e)
        {
            using (var pw = ClientPacket.MoveStopHorizontal())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }

        void HandleGameControl_MoveDown(GameControl sender, EventArgs e)
        {
            using (var pw = ClientPacket.MoveDown())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }
    }
}

#endif