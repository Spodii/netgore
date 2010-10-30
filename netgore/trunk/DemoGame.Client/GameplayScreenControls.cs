using System;
using System.Linq;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.World;

namespace DemoGame.Client
{
    class GameplayScreenControls : GameControlCollection
    {
        readonly GameplayScreen _gameplayScreen;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameplayScreenControls"/> class.
        /// </summary>
        /// <param name="gameplayScreen">The <see cref="GameplayScreen"/>.</param>
        public GameplayScreenControls(GameplayScreen gameplayScreen)
        {
            if (gameplayScreen == null)
                throw new ArgumentNullException("gameplayScreen");

            _gameplayScreen = gameplayScreen;

            // Set some delay values for input handling
            const int minAttackRate = GameData.AttackTimeoutMin;
            const int minMoveRate = 150;
            const int minNPCChatRate = 150;
            const int minPickupRate = 150;
            const int minShopRate = 250;
            const int minUseRate = 250;
            const int minEmoteRate = 1000;
            const int minQuickBarRate = 200;

            // Set the handlers for all the different controls
#if !TOPDOWN
            CreateAndAdd(GameControlsKeys.Jump, minMoveRate, () => UserChar.CanJump && CanUserMove(), HandleGameControl_Jump);
#endif

#if TOPDOWN
            CreateAndAdd(GameControlsKeys.MoveUp, minMoveRate, () => !UserChar.IsMovingUp && CanUserMove(),
                         HandleGameControl_MoveUp);

            CreateAndAdd(GameControlsKeys.MoveDown, minMoveRate, () => !UserChar.IsMovingDown && CanUserMove(),
                         HandleGameControl_MoveDown);
#endif

            CreateAndAdd(GameControlsKeys.MoveLeft, minMoveRate, () => !UserChar.IsMovingLeft && CanUserMove(),
                         HandleGameControl_MoveLeft);

            CreateAndAdd(GameControlsKeys.MoveRight, minMoveRate, () => !UserChar.IsMovingRight && CanUserMove(),
                         HandleGameControl_MoveRight);

            CreateAndAdd(GameControlsKeys.Attack, minAttackRate, CanUserMove, HandleGameControl_Attack);

            CreateAndAdd(GameControlsKeys.MoveStop, minMoveRate, () => UserChar.IsMoving, HandleGameControl_MoveStop);

#if TOPDOWN
            CreateAndAdd(GameControlsKeys.MoveStopHorizontal, minMoveRate, () => UserChar.IsMovingLeft || UserChar.IsMovingRight, HandleGameControl_MoveStopHorizontal);

            CreateAndAdd(GameControlsKeys.MoveStopVertical, minMoveRate, () => UserChar.IsMovingUp || UserChar.IsMovingDown, HandleGameControl_MoveStopVertical);
#endif

            CreateAndAdd(GameControlsKeys.UseWorld, minUseRate, CanUserMove, HandleGameControl_Use);

            CreateAndAdd(GameControlsKeys.UseShop, minShopRate, CanUserMove, HandleGameControl_Shop);

            CreateAndAdd(GameControlsKeys.TalkToNPC, minNPCChatRate, CanUserMove, HandleGameControl_TalkToNPC);

            CreateAndAdd(GameControlsKeys.PickUp, minPickupRate, CanUserMove, HandleGameControl_PickUp);

            CreateAndAdd(GameControlsKeys.EmoteEllipsis, minEmoteRate, () => true, x => HandleGameControl_Emote(Emoticon.Ellipsis));
            CreateAndAdd(GameControlsKeys.EmoteExclamation, minEmoteRate, () => true,
                         x => HandleGameControl_Emote(Emoticon.Exclamation));
            CreateAndAdd(GameControlsKeys.EmoteHeartbroken, minEmoteRate, () => true,
                         x => HandleGameControl_Emote(Emoticon.Heartbroken));
            CreateAndAdd(GameControlsKeys.EmoteHearts, minEmoteRate, () => true, x => HandleGameControl_Emote(Emoticon.Hearts));
            CreateAndAdd(GameControlsKeys.EmoteMeat, minEmoteRate, () => true, x => HandleGameControl_Emote(Emoticon.Meat));
            CreateAndAdd(GameControlsKeys.EmoteQuestion, minEmoteRate, () => true, x => HandleGameControl_Emote(Emoticon.Question));
            CreateAndAdd(GameControlsKeys.EmoteSweat, minEmoteRate, () => true, x => HandleGameControl_Emote(Emoticon.Sweat));

            CreateAndAdd(GameControlsKeys.QuickBarItem0, minQuickBarRate, () => true, x => HandleGameControl_QuickBar(0));
            CreateAndAdd(GameControlsKeys.QuickBarItem1, minQuickBarRate, () => true, x => HandleGameControl_QuickBar(1));
            CreateAndAdd(GameControlsKeys.QuickBarItem2, minQuickBarRate, () => true, x => HandleGameControl_QuickBar(2));
            CreateAndAdd(GameControlsKeys.QuickBarItem3, minQuickBarRate, () => true, x => HandleGameControl_QuickBar(3));
            CreateAndAdd(GameControlsKeys.QuickBarItem4, minQuickBarRate, () => true, x => HandleGameControl_QuickBar(4));
            CreateAndAdd(GameControlsKeys.QuickBarItem5, minQuickBarRate, () => true, x => HandleGameControl_QuickBar(5));
            CreateAndAdd(GameControlsKeys.QuickBarItem6, minQuickBarRate, () => true, x => HandleGameControl_QuickBar(6));
            CreateAndAdd(GameControlsKeys.QuickBarItem7, minQuickBarRate, () => true, x => HandleGameControl_QuickBar(7));
            CreateAndAdd(GameControlsKeys.QuickBarItem8, minQuickBarRate, () => true, x => HandleGameControl_QuickBar(8));
            CreateAndAdd(GameControlsKeys.QuickBarItem9, minQuickBarRate, () => true, x => HandleGameControl_QuickBar(9));
        }

        /// <summary>
        /// Gets or sets the <see cref="MapEntityIndex"/> of the entity to use as the target for input controls
        /// that can utilize a target. If null, no target will be used.
        /// </summary>
        public MapEntityIndex? TargetIndex { get; set; }

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
            var area = GameData.GetValidShopArea(source);
            var validShopOwners = Map.Spatial.GetMany<CharacterEntity>(area, x => x.HasShop);

            // Return first if there is zero or one elements
            if (validShopOwners.Count() <= 1)
                return validShopOwners.FirstOrDefault();

            // Return closest
            return validShopOwners.MinElement(x => x.GetDistance(source));
        }

        void HandleGameControl_Attack(GameControl sender)
        {
            using (var pw = ClientPacket.Attack(TargetIndex))
            {
                Socket.Send(pw, ClientMessageType.CharacterAction);
            }
        }

        void HandleGameControl_Emote(Emoticon emoticon)
        {
            using (var pw = ClientPacket.Emoticon(emoticon))
            {
                Socket.Send(pw, ClientMessageType.CharacterEmote);
            }
        }

        void HandleGameControl_QuickBar(byte slot)
        {
            GameplayScreen.QuickBarForm.UseSlot(slot);
        }

#if !TOPDOWN
        void HandleGameControl_Jump(GameControl sender)
        {
            using (var pw = ClientPacket.Jump())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }
#endif

#if TOPDOWN
        void HandleGameControl_MoveDown(GameControl sender)
        {
            using (var pw = ClientPacket.MoveDown())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }
#endif

        void HandleGameControl_MoveLeft(GameControl sender)
        {
            using (var pw = ClientPacket.MoveLeft())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }

        void HandleGameControl_MoveRight(GameControl sender)
        {
            using (var pw = ClientPacket.MoveRight())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }

        void HandleGameControl_MoveStop(GameControl sender)
        {
            using (var pw = ClientPacket.MoveStop())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }

#if TOPDOWN
        void HandleGameControl_MoveStopHorizontal(GameControl sender)
        {
            using (var pw = ClientPacket.MoveStopHorizontal())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }
#endif

#if TOPDOWN
        void HandleGameControl_MoveStopVertical(GameControl sender)
        {
            using (var pw = ClientPacket.MoveStopVertical())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }
#endif

#if TOPDOWN
        void HandleGameControl_MoveUp(GameControl sender)
        {
            using (var pw = ClientPacket.MoveUp())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }
#endif

        void HandleGameControl_PickUp(GameControl sender)
        {
            var pickupItem = Map.Spatial.Get<ItemEntity>(GameData.GetPickupArea(UserChar));
            if (pickupItem == null)
                return;

            using (var pw = ClientPacket.PickupItem(pickupItem.MapEntityIndex))
            {
                Socket.Send(pw, ClientMessageType.CharacterInteract);
            }
        }

        void HandleGameControl_Shop(GameControl sender)
        {
            var shopOwner = GetClosestValidShopOwner(UserChar);
            if (shopOwner == null)
                return;

            using (var pw = ClientPacket.StartShopping(shopOwner.MapEntityIndex))
            {
                Socket.Send(pw, ClientMessageType.CharacterInteract);
            }
        }

        void HandleGameControl_TalkToNPC(GameControl sender)
        {
            var r = UserChar.ToRectangle(GameData.MaxNPCChatDistance);
            CharacterEntity npc = Map.Spatial.Get<Character>(r, x => x.HasChatDialog || !x.ProvidedQuests.IsEmpty());
            if (npc == null)
                return;

            using (var pw = ClientPacket.StartNPCChatDialog(npc.MapEntityIndex, false))
            {
                Socket.Send(pw, ClientMessageType.CharacterInteract);
            }
        }

        void HandleGameControl_Use(GameControl sender)
        {
            var useEntity = Map.Spatial.Get<DynamicEntity>(UserChar.ToRectangle(), UsableEntityFilter);
            if (useEntity == null)
                return;

            using (var pw = ClientPacket.UseWorld(useEntity.MapEntityIndex))
            {
                Socket.Send(pw, ClientMessageType.CharacterInteract);
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
            var asUsable = dynamicEntity as IUsableEntity;
            if (asUsable == null)
                return false;

            // Check that this DynamicEntity can use it
            return asUsable.CanUse(dynamicEntity);
        }
    }
}