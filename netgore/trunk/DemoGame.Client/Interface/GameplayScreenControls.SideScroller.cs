#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    partial class GameplayScreenControls
    {
        /// <summary>
        /// Creates the controls specific to side-scroller builds.
        /// </summary>
        void CreateControlsForPerspective()
        {
            CreateAndAdd(GameControlsKeys.Jump, _minMoveRate, () => UserChar.CanJump && CanUserMove(), HandleGameControl_Jump);
        }

        void HandleGameControl_Jump(GameControl sender)
        {
            using (var pw = ClientPacket.Jump())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }
    }
}

#endif