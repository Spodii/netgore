using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.Network;

namespace DemoGame.Client
{
    class GameplayScreenControls : GameControlCollection
    {
        readonly GameplayScreen _gameplayScreen;

        public GameplayScreen GameplayScreen
        {
            get { return _gameplayScreen; }
        }

        Map Map
        {
            get { return GameplayScreen.Map; }
        }

        ClientSockets Socket
        {
            get { return GameplayScreen.Socket; }
        }

        Character UserChar
        {
            get { return GameplayScreen.UserChar; }
        }

        public GameplayScreenControls(GameplayScreen gameplayScreen)
        {
            if (gameplayScreen == null)
                throw new ArgumentNullException("gameplayScreen");

            _gameplayScreen = gameplayScreen;

            const int minAttackRate = 150;
            const int minMoveRate = 150;
            const int minNPCChatRate = 150;
            const int minPickupRate = 150;
            const int minShopRate = 250;
            const int minUseRate = 250;

            CreateAndAdd("Jump", minMoveRate, () => UserChar.CanJump && CanUserMove(), HandleGameControl_Jump, Keys.Up, null,
                         null, null);

            CreateAndAdd("Move Left", minMoveRate, () => !UserChar.IsMovingLeft && CanUserMove(), HandleGameControl_MoveLeft,
                         Keys.Left, Keys.Right, null, null);

            CreateAndAdd("Move Right", minMoveRate, () => !UserChar.IsMovingRight && CanUserMove(),
                         HandleGameControl_MoveRight, Keys.Right, Keys.Left, null, null);

            CreateAndAdd("Attack", minAttackRate, CanUserMove, HandleGameControl_Attack, Keys.LeftControl, null, null, null);

            CreateAndAdd("Stop Moving", minMoveRate, () => UserChar.IsMoving, HandleGameControl_MoveStop, null,
                         new Keys[] { Keys.Left, Keys.Right }, null, null);

            CreateAndAdd("Use World", minUseRate, CanUserMove, HandleGameControl_Use, Keys.LeftAlt, null, null, null);

            CreateAndAdd("Use Shop", minShopRate, CanUserMove, HandleGameControl_Shop, Keys.LeftAlt, null, null, null);

            CreateAndAdd("Talk To NPC", minNPCChatRate, CanUserMove, HandleGameControl_TalkToNPC, Keys.LeftAlt, null, null,
                         null);

            CreateAndAdd("Pick Up", minPickupRate, CanUserMove, HandleGameControl_PickUp, Keys.Space, null, null, null);
        }

        /// <summary>
        /// Gets if the User character is allowed to move or perform actions. This only checks general
        /// conditions like if the User is chatting to a NPC or in some other state that does not
        /// allow movement of any kind.
        /// </summary>
        /// <returns>True if the User can move or perform actions; otherwise false.</returns>
        bool CanUserMove()
        {
            // Don't allow actions while chatting to NPCs
            if (!GameData.AllowMovementWhileChattingToNPC && GameplayScreen.ChatDialogForm.IsChatting)
                return false;

            // Movement is allowed
            return true;
        }

        /// <summary>
        /// Gets the closest valid shop owner to the given source Entity, or null if none found.
        /// </summary>
        /// <param name="source">The source Entity doing the shopping.</param>
        /// <returns>The closest valid shop owner to the given source Entity, or null if none found.</returns>
        DynamicEntity GetClosestValidShopOwner(Entity source)
        {
            var shopOwners = Map.DynamicEntities.OfType<CharacterEntity>().Where(x => x.HasShop);
            var validShopOwners = shopOwners.Where(x => GameData.IsValidDistanceToShop(source, x));

            // Return first if there is zero or one elements
            if (validShopOwners.Count() <= 1)
                return validShopOwners.FirstOrDefault();

            // Return closest
            return validShopOwners.MinElement(x => x.CB.GetDistance(source.CB));
        }

        void HandleGameControl_Attack(GameControl sender)
        {
            using (PacketWriter pw = ClientPacket.Attack())
            {
                Socket.Send(pw);
            }
        }

        void HandleGameControl_Jump(GameControl sender)
        {
            using (PacketWriter pw = ClientPacket.Jump())
            {
                Socket.Send(pw);
            }
        }

        void HandleGameControl_MoveLeft(GameControl sender)
        {
            using (PacketWriter pw = ClientPacket.MoveLeft())
            {
                Socket.Send(pw);
            }
        }

        void HandleGameControl_MoveRight(GameControl sender)
        {
            using (PacketWriter pw = ClientPacket.MoveRight())
            {
                Socket.Send(pw);
            }
        }

        void HandleGameControl_MoveStop(GameControl sender)
        {
            using (PacketWriter pw = ClientPacket.MoveStop())
            {
                Socket.Send(pw);
            }
        }

        void HandleGameControl_PickUp(GameControl sender)
        {
            Rectangle userRect = UserChar.CB.ToRectangle();
            ItemEntity pickupItem = Map.GetEntity<ItemEntity>(userRect);
            if (pickupItem == null)
                return;

            using (PacketWriter pw = ClientPacket.PickupItem(pickupItem.MapEntityIndex))
            {
                Socket.Send(pw);
            }
        }

        void HandleGameControl_Shop(GameControl sender)
        {
            DynamicEntity shopOwner = GetClosestValidShopOwner(UserChar);
            if (shopOwner == null)
                return;

            using (PacketWriter pw = ClientPacket.StartShopping(shopOwner.MapEntityIndex))
            {
                Socket.Send(pw);
            }
        }

        void HandleGameControl_TalkToNPC(GameControl sender)
        {
            CharacterEntity npc = Map.GetEntity<CharacterEntity>(UserChar.CB.ToRectangle(), x => x.HasChatDialog);
            if (npc == null)
                return;

            using (PacketWriter pw = ClientPacket.StartNPCChatDialog(npc.MapEntityIndex))
            {
                Socket.Send(pw);
            }
        }

        /// <summary>
        /// Filter used to find a DynamicEntity for the User to use.
        /// </summary>
        /// <param name="dynamicEntity">The User's character.</param>
        /// <returns>True if it can be used by the <paramref name="dynamicEntity"/>, else false.</returns>
        static bool UsableEntityFilter(DynamicEntity dynamicEntity)
        {
            // Make sure it is usable
            IUsableEntity asUsable = dynamicEntity as IUsableEntity;
            if (asUsable == null)
                return false;

            // Check that this DynamicEntity can use it
            return asUsable.CanUse(dynamicEntity);
        }

        void HandleGameControl_Use(GameControl sender)
        {
            DynamicEntity useEntity = Map.GetEntity<DynamicEntity>(UserChar.CB.ToRectangle(), UsableEntityFilter);
            if (useEntity == null)
                return;

            using (PacketWriter pw = ClientPacket.UseWorld(useEntity.MapEntityIndex))
            {
                Socket.Send(pw);
            }
        }
    }
}
