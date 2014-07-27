using System;
using System.Linq;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.World;

namespace DemoGame.Client
{
    partial class GameplayScreenControls : GameControlCollection
    {
        const int _minAttackRate = GameData.AttackTimeoutMin;
        const int _minEmoteRate = 1000;
        const int _minMoveRate = 150;
        const int _minNPCChatRate = 150;
        const int _minPickupRate = 150;
        const int _minQuickBarRate = 200;
        const int _minShopRate = 250;
        const int _minUseRate = 250;
        const int _minWindowRate = 5;

        readonly GameplayScreen _gameplayScreen;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameplayScreenControls"/> class.
        /// </summary>
        /// <param name="gameplayScreen">The <see cref="GameplayScreen"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="gameplayScreen" /> is <c>null</c>.</exception>
        public GameplayScreenControls(GameplayScreen gameplayScreen)
        {
            if (gameplayScreen == null)
                throw new ArgumentNullException("gameplayScreen");

            _gameplayScreen = gameplayScreen;

            CreateControls();
        }

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

        /// <summary>
        /// Gets or sets the <see cref="MapEntityIndex"/> of the entity to use as the target for input controls
        /// that can utilize a target. If null, no target will be used.
        /// </summary>
        public MapEntityIndex? TargetIndex { get; set; }

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

            // Don't allow movement while typing/editing text
            if (GameplayScreen.IsChatFormInputFocused)
                return false;

            // Movement is allowed
            return true;
        }

        /// <summary>
        /// Creates all of the controls for the <see cref="GameplayScreen"/> not specific to a single perspective.
        /// </summary>
        void CreateControls()
        {
            CreateAndAdd(GameControlsKeys.MoveLeft, _minMoveRate, () => !UserChar.IsMovingLeft && CanUserMove(),
                HandleGameControl_MoveLeft);

            CreateAndAdd(GameControlsKeys.MoveRight, _minMoveRate, () => !UserChar.IsMovingRight && CanUserMove(),
                HandleGameControl_MoveRight);

            CreateAndAdd(GameControlsKeys.Attack, _minAttackRate, CanUserMove, HandleGameControl_Attack);

            CreateAndAdd(GameControlsKeys.MoveStop, _minMoveRate, () => UserChar.IsMoving, HandleGameControl_MoveStop);

            CreateAndAdd(GameControlsKeys.UseWorld, _minUseRate, CanUserMove, HandleGameControl_Use);

            CreateAndAdd(GameControlsKeys.UseShop, _minShopRate, CanUserMove, HandleGameControl_Shop);

            CreateAndAdd(GameControlsKeys.TalkToNPC, _minNPCChatRate, CanUserMove, HandleGameControl_TalkToNPC);

            CreateAndAdd(GameControlsKeys.PickUp, _minPickupRate, CanUserMove, HandleGameControl_PickUp);

            CreateAndAdd(GameControlsKeys.EmoteEllipsis, _minEmoteRate, () => true,
                (x, e) => HandleGameControl_Emote(Emoticon.Ellipsis));
            CreateAndAdd(GameControlsKeys.EmoteExclamation, _minEmoteRate, () => true,
                (x, e) => HandleGameControl_Emote(Emoticon.Exclamation));
            CreateAndAdd(GameControlsKeys.EmoteHeartbroken, _minEmoteRate, () => true,
                (x, e) => HandleGameControl_Emote(Emoticon.Heartbroken));
            CreateAndAdd(GameControlsKeys.EmoteHearts, _minEmoteRate, () => true,
                (x, e) => HandleGameControl_Emote(Emoticon.Hearts));
            CreateAndAdd(GameControlsKeys.EmoteMeat, _minEmoteRate, () => true, (x, e) => HandleGameControl_Emote(Emoticon.Meat));
            CreateAndAdd(GameControlsKeys.EmoteQuestion, _minEmoteRate, () => true,
                (x, e) => HandleGameControl_Emote(Emoticon.Question));
            CreateAndAdd(GameControlsKeys.EmoteSweat, _minEmoteRate, () => true, (x, e) => HandleGameControl_Emote(Emoticon.Sweat));

            CreateAndAdd(GameControlsKeys.QuickBarItem0, _minQuickBarRate, () => true, (x, e) => HandleGameControl_QuickBar(0));
            CreateAndAdd(GameControlsKeys.QuickBarItem1, _minQuickBarRate, () => true, (x, e) => HandleGameControl_QuickBar(1));
            CreateAndAdd(GameControlsKeys.QuickBarItem2, _minQuickBarRate, () => true, (x, e) => HandleGameControl_QuickBar(2));
            CreateAndAdd(GameControlsKeys.QuickBarItem3, _minQuickBarRate, () => true, (x, e) => HandleGameControl_QuickBar(3));
            CreateAndAdd(GameControlsKeys.QuickBarItem4, _minQuickBarRate, () => true, (x, e) => HandleGameControl_QuickBar(4));
            CreateAndAdd(GameControlsKeys.QuickBarItem5, _minQuickBarRate, () => true, (x, e) => HandleGameControl_QuickBar(5));
            CreateAndAdd(GameControlsKeys.QuickBarItem6, _minQuickBarRate, () => true, (x, e) => HandleGameControl_QuickBar(6));
            CreateAndAdd(GameControlsKeys.QuickBarItem7, _minQuickBarRate, () => true, (x, e) => HandleGameControl_QuickBar(7));
            CreateAndAdd(GameControlsKeys.QuickBarItem8, _minQuickBarRate, () => true, (x, e) => HandleGameControl_QuickBar(8));
            CreateAndAdd(GameControlsKeys.QuickBarItem9, _minQuickBarRate, () => true, (x, e) => HandleGameControl_QuickBar(9));

            CreateAndAdd(GameControlsKeys.WindowEquipped, _minWindowRate, () => true,
                (x, e) => HandleGameControl_ToggleWindow(GameplayScreen.EquippedForm));
            CreateAndAdd(GameControlsKeys.WindowGuild, _minWindowRate, () => true,
                (x, e) => HandleGameControl_ToggleWindow(GameplayScreen.GuildForm));
            CreateAndAdd(GameControlsKeys.WindowInventory, _minWindowRate, () => true,
                (x, e) => HandleGameControl_ToggleWindow(GameplayScreen.InventoryForm));
            CreateAndAdd(GameControlsKeys.WindowSkills, _minWindowRate, () => true,
                (x, e) => HandleGameControl_ToggleWindow(GameplayScreen.SkillsForm));
            CreateAndAdd(GameControlsKeys.WindowStats, _minWindowRate, () => true,
                (x, e) => HandleGameControl_ToggleWindow(GameplayScreen.StatsForm));

            // Create the controls specific to a perspective
            CreateControlsForPerspective();
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

        void HandleGameControl_Attack(GameControl sender, EventArgs e)
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

        void HandleGameControl_MoveLeft(GameControl sender, EventArgs e)
        {
            using (var pw = ClientPacket.MoveLeft())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }

        void HandleGameControl_MoveRight(GameControl sender, EventArgs e)
        {
            using (var pw = ClientPacket.MoveRight())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }

        void HandleGameControl_MoveStop(GameControl sender, EventArgs e)
        {
            using (var pw = ClientPacket.MoveStop())
            {
                Socket.Send(pw, ClientMessageType.CharacterMove);
            }
        }

        void HandleGameControl_PickUp(GameControl sender, EventArgs e)
        {
            var pickupItem = Map.Spatial.Get<ItemEntity>(GameData.GetPickupArea(UserChar));
            if (pickupItem == null)
                return;

            using (var pw = ClientPacket.PickupItem(pickupItem.MapEntityIndex))
            {
                Socket.Send(pw, ClientMessageType.CharacterInteract);
            }
        }

        void HandleGameControl_QuickBar(byte slot)
        {
            GameplayScreen.QuickBarForm.UseSlot(slot);
        }

        void HandleGameControl_Shop(GameControl sender, EventArgs e)
        {
            var shopOwner = GetClosestValidShopOwner(UserChar);
            if (shopOwner == null)
                return;

            using (var pw = ClientPacket.StartShopping(shopOwner.MapEntityIndex))
            {
                Socket.Send(pw, ClientMessageType.CharacterInteract);
            }
        }

        void HandleGameControl_TalkToNPC(GameControl sender, EventArgs e)
        {
            var r = UserChar.ToRectangle().Inflate(GameData.MaxNPCChatDistance);
            CharacterEntity npc = Map.Spatial.Get<Character>(r, x => x.HasChatDialog || !x.ProvidedQuests.IsEmpty());
            if (npc == null)
                return;

            using (var pw = ClientPacket.StartNPCChatDialog(npc.MapEntityIndex, false))
            {
                Socket.Send(pw, ClientMessageType.CharacterInteract);
            }
        }

        void HandleGameControl_ToggleWindow(Control c)
        {
            c.IsVisible = !c.IsVisible;
        }

        void HandleGameControl_Use(GameControl sender, EventArgs e)
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