using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Audio;
using NetGore.Features.ActionDisplays;
using NetGore.Features.NPCChat;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Network;
using NetGore.Stats;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// Packets going out from the server / in to the client.
    /// </summary>
    static class ServerPacket
    {
        static readonly PacketWriterPool _writerPool = new PacketWriterPool();

        public static PacketWriter AcceptOrTurnInQuestReply(QuestID questID, bool successful, bool accepted)
        {
            var pw = GetWriter(ServerPacketID.AcceptOrTurnInQuestReply);
            pw.Write(questID);
            pw.Write(successful);
            pw.Write(accepted);
            return pw;
        }

        public static PacketWriter AddStatusEffect(StatusEffectType statusEffectType, ushort power, uint timeLeft)
        {
            var secsLeft = (ushort)((timeLeft / 1000).Clamp(ushort.MinValue, ushort.MaxValue));

            var pw = GetWriter(ServerPacketID.AddStatusEffect);
            pw.WriteEnum(statusEffectType);
            pw.Write(power);
            pw.Write(secsLeft);
            return pw;
        }

        public static PacketWriter CharAttack(MapEntityIndex attacker, MapEntityIndex? attacked = null, ActionDisplayID? actionDisplay = null)
        {
            var pw = GetWriter(ServerPacketID.CharAttack);
            pw.Write(attacker);

            pw.Write(attacked.HasValue);
            if (attacked.HasValue)
                pw.Write(attacked.Value);

            pw.Write(actionDisplay.HasValue);
            if (actionDisplay.HasValue)
                pw.Write(actionDisplay.Value);

            return pw;
        }

        public static PacketWriter CharDamage(MapEntityIndex mapEntityIndex, int damage)
        {
            var pw = GetWriter(ServerPacketID.CharDamage);
            pw.Write(mapEntityIndex);
            pw.Write(damage);
            return pw;
        }

        public static PacketWriter Chat(string text)
        {
            var pw = GetWriter(ServerPacketID.Chat);
            pw.Write(text, GameData.MaxServerSayLength);
            return pw;
        }

        public static PacketWriter ChatSay(string name, MapEntityIndex mapEntityIndex, string text)
        {
            var pw = GetWriter(ServerPacketID.ChatSay);
            pw.Write(name, GameData.MaxServerSayNameLength);
            pw.Write(mapEntityIndex);
            pw.Write(text, GameData.MaxServerSayLength);
            return pw;
        }

        public static PacketWriter CreateAccount(bool successful, GameMessage failureReason)
        {
            var pw = GetWriter(ServerPacketID.CreateAccount);
            pw.Write(successful);
            if (!successful)
                pw.WriteEnum(failureReason);
            return pw;
        }

        public static PacketWriter CreateAccountCharacter(bool successful, string failureMessage)
        {
            var pw = GetWriter(ServerPacketID.CreateAccountCharacter);
            pw.Write(successful);
            if (!successful)
                pw.Write(failureMessage);
            return pw;
        }

        public static void CreateDynamicEntity(PacketWriter pw, DynamicEntity dynamicEntity)
        {
            pw.Write(ServerPacketID.CreateDynamicEntity);
            pw.Write(dynamicEntity.MapEntityIndex);
            DynamicEntityFactory.Instance.Write(pw, dynamicEntity, true);
        }

        public static PacketWriter CreateDynamicEntity(DynamicEntity dynamicEntity)
        {
            var pw = GetWriter();
            CreateDynamicEntity(pw, dynamicEntity);
            return pw;
        }

        public static PacketWriter Emote(MapEntityIndex mapEntityIndex, Emoticon emoticon)
        {
            var pw = GetWriter(ServerPacketID.Emote);
            pw.Write(mapEntityIndex);
            pw.WriteEnum(emoticon);
            return pw;
        }

        public static PacketWriter EndChatDialog()
        {
            return GetWriter(ServerPacketID.EndChatDialog);
        }

        /// <summary>
        /// Gets a <see cref="PacketWriter"/> to use from the internal pool. It is important that this
        /// <see cref="PacketWriter"/> is disposed of properly when done.
        /// </summary>
        /// <returns>The <see cref="PacketWriter"/> to use.</returns>
        public static PacketWriter GetWriter()
        {
            return _writerPool.Acquire();
        }

        /// <summary>
        /// Gets a <see cref="PacketWriter"/> to use from the internal pool. It is important that this
        /// <see cref="PacketWriter"/> is disposed of properly when done.
        /// </summary>
        /// <param name="id">The <see cref="ServerPacketID"/> that this <see cref="PacketWriter"/> will be writing.</param>
        /// <returns>The <see cref="PacketWriter"/> to use.</returns>
        public static PacketWriter GetWriter(ServerPacketID id)
        {
            var pw = _writerPool.Acquire();
            Debug.Assert(pw.LengthBits == 0);
            pw.Write(id);
            return pw;
        }

        public static PacketWriter GroupInfo(Action<PacketWriter> populate)
        {
            var ret = GetWriter(ServerPacketID.GroupInfo);
            populate(ret);
            return ret;
        }

        public static PacketWriter GuildInfo(Action<PacketWriter> populate)
        {
            var ret = GetWriter(ServerPacketID.GuildInfo);
            populate(ret);
            return ret;
        }

        public static PacketWriter HasQuestFinishRequirements(QuestID questID, bool hasRequirements)
        {
            var ret = GetWriter(ServerPacketID.HasQuestFinishRequirementsReply);
            ret.Write(questID);
            ret.Write(hasRequirements);
            return ret;
        }

        public static PacketWriter HasQuestStartRequirements(QuestID questID, bool hasRequirements)
        {
            var ret = GetWriter(ServerPacketID.HasQuestStartRequirementsReply);
            ret.Write(questID);
            ret.Write(hasRequirements);
            return ret;
        }

        /// <summary>
        /// Tells the user their login attempt was successful.
        /// </summary>
        public static PacketWriter LoginSuccessful()
        {
            return GetWriter(ServerPacketID.LoginSuccessful);
        }

        /// <summary>
        /// Tells the user their login attempt was unsuccessful.
        /// </summary>
        /// <param name="gameMessage">GameMessage for explaining why the login was unsuccessful.</param>
        public static PacketWriter LoginUnsuccessful(GameMessage gameMessage)
        {
            var pw = GetWriter(ServerPacketID.LoginUnsuccessful);
            pw.Write(gameMessage);
            return pw;
        }

        /// <summary>
        /// Tells the user their login attempt was unsuccessful.
        /// </summary>
        /// <param name="gameMessage">GameMessage for explaining why the login was unsuccessful.</param>
        /// <param name="p">Arguments for the GameMessage.</param>
        public static PacketWriter LoginUnsuccessful(GameMessage gameMessage, params object[] p)
        {
            var pw = GetWriter(ServerPacketID.LoginUnsuccessful);
            pw.Write(gameMessage, p);
            return pw;
        }

        public static PacketWriter NotifyExpCash(int exp, int cash)
        {
            var pw = GetWriter(ServerPacketID.NotifyExpCash);
            pw.Write(exp);
            pw.Write(cash);
            return pw;
        }

        public static PacketWriter NotifyGetItem(string name, byte amount)
        {
            var pw = GetWriter(ServerPacketID.NotifyGetItem);
            pw.Write(name);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter NotifyLevel(MapEntityIndex mapEntityIndex)
        {
            var pw = GetWriter(ServerPacketID.NotifyLevel);
            pw.Write(mapEntityIndex);
            return pw;
        }

        public static PacketWriter PlaySound(SoundID sound)
        {
            var pw = GetWriter(ServerPacketID.PlaySound);
            pw.Write(sound);
            return pw;
        }

        public static PacketWriter PlaySoundAt(SoundID sound, Vector2 position)
        {
            var pw = GetWriter(ServerPacketID.PlaySoundAt);
            pw.Write(sound);
            pw.Write(position);
            return pw;
        }

        public static PacketWriter PlaySoundAtEntity(SoundID sound, MapEntityIndex mapEntityIndex)
        {
            var pw = GetWriter(ServerPacketID.PlaySoundAtEntity);
            pw.Write(sound);
            pw.Write(mapEntityIndex);
            return pw;
        }

        public static PacketWriter QuestInfo(Action<PacketWriter> populate)
        {
            var ret = GetWriter(ServerPacketID.QuestInfo);
            populate(ret);
            return ret;
        }

        public static PacketWriter RemoveDynamicEntity(MapEntityIndex mei)
        {
            var pw = GetWriter(ServerPacketID.RemoveDynamicEntity);
            pw.Write(mei);
            return pw;
        }

        public static PacketWriter RemoveStatusEffect(StatusEffectType statusEffectType)
        {
            var pw = GetWriter(ServerPacketID.RemoveStatusEffect);
            pw.WriteEnum(statusEffectType);
            return pw;
        }

        public static PacketWriter SendAccountCharacters(AccountCharacterInfo[] charInfos)
        {
            var pw = GetWriter(ServerPacketID.SendAccountCharacters);

            if (charInfos == null || charInfos.Length == 0)
                pw.Write((byte)0);
            else
            {
                pw.Write((byte)charInfos.Length);
                for (var i = 0; i < charInfos.Length; i++)
                {
                    pw.Write(charInfos[i]);
                }
            }

            return pw;
        }

        public static PacketWriter SendEquipmentItemInfo(EquipmentSlot slot, ItemEntity item)
        {
            var pw = GetWriter(ServerPacketID.SendEquipmentItemInfo);
            pw.WriteEnum(slot);
            new ItemTable(item).WriteState(pw);
            return pw;
        }

        public static PacketWriter SendInventoryItemInfo(InventorySlot slot, ItemEntity item)
        {
            var pw = GetWriter(ServerPacketID.SendInventoryItemInfo);
            pw.Write(slot);
            new ItemTable(item).WriteState(pw);
            return pw;
        }

        public static PacketWriter SendMessage(GameMessage gameMessage)
        {
            var pw = GetWriter(ServerPacketID.SendMessage);
            pw.Write(gameMessage);
            return pw;
        }

        public static PacketWriter SendMessage(GameMessage gameMessage, params object[] p)
        {
            var pw = GetWriter(ServerPacketID.SendMessage);
            pw.Write(gameMessage, p);
            return pw;
        }

        public static PacketWriter SetCash(int cash)
        {
            var pw = GetWriter(ServerPacketID.SetCash);
            pw.Write(cash);
            return pw;
        }

        public static void SetCharacterHPPercent(PacketWriter pw, MapEntityIndex mapEntityIndex, byte hpPercent)
        {
            pw.Write(ServerPacketID.SetCharacterHPPercent);
            pw.Write(mapEntityIndex);
            pw.Write(hpPercent);
        }

        public static PacketWriter SetCharacterHPPercent(MapEntityIndex mapEntityIndex, byte hpPercent)
        {
            var pw = GetWriter();
            SetCharacterHPPercent(pw, mapEntityIndex, hpPercent);
            return pw;
        }

        public static void SetCharacterMPPercent(PacketWriter pw, MapEntityIndex mapEntityIndex, byte mpPercent)
        {
            pw.Write(ServerPacketID.SetCharacterMPPercent);
            pw.Write(mapEntityIndex);
            pw.Write(mpPercent);
        }

        public static PacketWriter SetCharacterMPPercent(MapEntityIndex mapEntityIndex, byte mpPercent)
        {
            var pw = GetWriter();
            SetCharacterMPPercent(pw, mapEntityIndex, mpPercent);
            return pw;
        }

        public static PacketWriter SetCharacterPaperDoll(MapEntityIndex mapEntityIndex, IEnumerable<string> layers)
        {
            var pw = GetWriter(ServerPacketID.SetCharacterPaperDoll);
            pw.Write(mapEntityIndex);

            pw.Write((byte)layers.Count());
            foreach (var layer in layers)
            {
                pw.Write(layer);
            }

            return pw;
        }

        public static PacketWriter SetChatDialogPage(NPCChatDialogItemID pageID, IEnumerable<byte> responsesToSkip)
        {
            var pw = GetWriter(ServerPacketID.SetChatDialogPage);
            pw.Write(pageID);

            // Get the number of responses to skip
            byte skipCount;
            if (responsesToSkip == null)
                skipCount = 0;
            else
                skipCount = (byte)responsesToSkip.Count();

            pw.Write(skipCount);

            // List off the responses to skip, if any
            if (skipCount > 0)
            {
                Debug.Assert(responsesToSkip != null);
                foreach (var responseToSkip in responsesToSkip)
                {
                    pw.Write(responseToSkip);
                }
            }

            return pw;
        }

        public static PacketWriter SetClickWarpMode(bool enabled)
        {
            var pw = GetWriter(ServerPacketID.SetClickWarpMode);
            pw.Write(enabled);
            return pw;
        }

        public static PacketWriter SetExp(int exp)
        {
            var pw = GetWriter(ServerPacketID.SetExp);
            pw.Write(exp);
            return pw;
        }

        public static PacketWriter SetGameTime(DateTime serverTime)
        {
            var pw = GetWriter(ServerPacketID.SetGameTime);
            pw.Write(serverTime.ToBinary());
            return pw;
        }

        public static void SetHP(PacketWriter pw, SPValueType hp)
        {
            pw.Write(ServerPacketID.SetHP);
            pw.Write(hp);
        }

        public static PacketWriter SetHP(SPValueType hp)
        {
            var pw = GetWriter();
            SetHP(pw, hp);
            return pw;
        }

        public static void SetInventorySlot(PacketWriter pw, InventorySlot slot, GrhIndex graphic, byte amount)
        {
            pw.Write(ServerPacketID.SetInventorySlot);
            pw.Write(slot);

            if (graphic.IsInvalid)
                pw.Write(false);
            else
            {
                pw.Write(true);
                pw.Write(graphic);
            }

            pw.Write(amount);
        }

        public static PacketWriter SetInventorySlot(InventorySlot slot, GrhIndex graphic, byte amount)
        {
            var pw = GetWriter();
            SetInventorySlot(pw, slot, graphic, amount);
            return pw;
        }

        public static PacketWriter SetLevel(short level)
        {
            var pw = GetWriter(ServerPacketID.SetLevel);
            pw.Write(level);
            return pw;
        }

        public static void SetMP(PacketWriter pw, SPValueType mp)
        {
            pw.Write(ServerPacketID.SetMP);
            pw.Write(mp);
        }

        public static PacketWriter SetMP(SPValueType mp)
        {
            var pw = GetWriter();
            SetMP(pw, mp);
            return pw;
        }

        public static void SetMap(PacketWriter pw, MapID mapID)
        {
            pw.Write(ServerPacketID.SetMap);
            pw.Write(mapID);
        }

        public static PacketWriter SetMap(MapID mapID)
        {
            var pw = GetWriter();
            SetMap(pw, mapID);
            return pw;
        }

        public static void SetProvidedQuests(PacketWriter pw, MapEntityIndex mapEntityIndex, IEnumerable<QuestID> quests)
        {
            pw.WriteEnum(ServerPacketID.SetProvidedQuests);
            pw.Write(mapEntityIndex);
            pw.Write((byte)quests.Count());
            foreach (var q in quests)
            {
                pw.Write(q);
            }
        }

        public static PacketWriter SetStatPoints(int statPoints)
        {
            var pw = GetWriter(ServerPacketID.SetStatPoints);
            pw.Write(statPoints);
            return pw;
        }

        public static void SetUserChar(PacketWriter pw, MapEntityIndex mapEntityIndex)
        {
            pw.Write(ServerPacketID.SetUserChar);
            pw.Write(mapEntityIndex);
        }

        /// <summary>
        /// Sets which map character is the one controlled by the user
        /// </summary>
        /// <param name="mapEntityIndex">Map character index controlled by the user</param>
        public static PacketWriter SetUserChar(MapEntityIndex mapEntityIndex)
        {
            var pw = GetWriter();
            SetUserChar(pw, mapEntityIndex);
            return pw;
        }

        public static PacketWriter SkillSetGroupCooldown(byte skillGroup, ushort cooldownTime)
        {
            var pw = GetWriter(ServerPacketID.SkillSetGroupCooldown);
            pw.Write(skillGroup);
            pw.Write(cooldownTime);
            return pw;
        }

        public static PacketWriter SkillSetKnown(SkillType skillType, bool isKnown)
        {
            var pw = GetWriter(ServerPacketID.SkillSetKnown);
            pw.WriteEnum(skillType);
            pw.Write(isKnown);
            return pw;
        }

        public static PacketWriter SkillSetKnownAll(IEnumerable<SkillType> knownSkills)
        {
            var pw = GetWriter(ServerPacketID.SkillSetKnownAll);
            var count = knownSkills == null ? 0 : knownSkills.Count();

            Debug.Assert(count <= byte.MaxValue,
                "Expected 255 or less known skills... will have to change packet to use a ushort for length.");
            Debug.Assert(count >= 0, "How do we have a negative count?");

            pw.Write((byte)count);

            if (count > 0)
            {
                foreach (var ks in knownSkills)
                {
                    pw.WriteEnum(ks);
                }
            }

            return pw;
        }

        public static PacketWriter SkillStartCasting_ToMap(MapEntityIndex entityIndex, SkillType skillType)
        {
            var pw = GetWriter(ServerPacketID.SkillStartCasting_ToMap);
            pw.Write(entityIndex);
            pw.WriteEnum(skillType);
            return pw;
        }

        public static PacketWriter CreateActionDisplayAtEntity(ActionDisplayID actionDisplayId, MapEntityIndex sourceEntityIndex, MapEntityIndex? targetEntityIndex = null)
        {
            var pw = GetWriter(ServerPacketID.CreateActionDisplayAtEntity);
            pw.Write(actionDisplayId);
            pw.Write(sourceEntityIndex);

            pw.Write(targetEntityIndex.HasValue);
            if (targetEntityIndex.HasValue)
                pw.Write(targetEntityIndex.Value);

            return pw;
        }

        public static PacketWriter SkillStartCasting_ToUser(SkillType skillType, ushort castTime)
        {
            var pw = GetWriter(ServerPacketID.SkillStartCasting_ToUser);
            pw.WriteEnum(skillType);
            pw.Write(castTime);
            return pw;
        }

        public static PacketWriter SkillStopCasting_ToMap(MapEntityIndex entityIndex)
        {
            var pw = GetWriter(ServerPacketID.SkillStopCasting_ToMap);
            pw.Write(entityIndex);
            return pw;
        }

        public static PacketWriter SkillStopCasting_ToUser()
        {
            var pw = GetWriter(ServerPacketID.SkillStopCasting_ToUser);
            return pw;
        }

        public static PacketWriter SkillUse(MapEntityIndex user, MapEntityIndex? target, SkillType skillType)
        {
            var pw = GetWriter(ServerPacketID.SkillUse);
            pw.Write(user);

            if (target.HasValue)
            {
                pw.Write(true);
                pw.Write(target.Value);
            }
            else
                pw.Write(false);

            pw.WriteEnum(skillType);

            return pw;
        }

        public static PacketWriter StartChatDialog(MapEntityIndex npcIndex, NPCChatDialogID dialogID)
        {
            var pw = GetWriter(ServerPacketID.StartChatDialog);
            pw.Write(npcIndex);
            pw.Write(dialogID);
            return pw;
        }

        public static PacketWriter StartQuestChatDialog(MapEntityIndex npcIndex, IEnumerable<QuestID> availableQuests,
                                                        IEnumerable<QuestID> turnInQuests)
        {
            var pw = GetWriter(ServerPacketID.StartQuestChatDialog);

            pw.Write(npcIndex);

            // Write the list of available quests
            if (availableQuests != null)
            {
                var values = availableQuests.ToImmutable();
                Debug.Assert(values.Count() <= byte.MaxValue);
                pw.Write((byte)values.Count());
                foreach (var q in values)
                {
                    pw.Write(q);
                }
            }
            else
                pw.Write((byte)0);

            // Write the list of quests that can be turned in
            if (turnInQuests != null)
            {
                var values = turnInQuests.ToImmutable();
                Debug.Assert(values.Count() <= byte.MaxValue);
                pw.Write((byte)values.Count());
                foreach (var q in values)
                {
                    pw.Write(q);
                }
            }
            else
                pw.Write((byte)0);

            return pw;
        }

        public static PacketWriter StartShopping(MapEntityIndex shopOwnerIndex, IShop<ShopItem> shop)
        {
            var pw = GetWriter(ServerPacketID.StartShopping);
            pw.Write(shopOwnerIndex);
            pw.Write(shop.CanBuy);
            pw.Write(shop.Name);
            shop.WriteShopItems(pw);
            return pw;
        }

        public static PacketWriter StopShopping()
        {
            return GetWriter(ServerPacketID.StopShopping);
        }

        public static void SynchronizeDynamicEntity(PacketWriter pw, DynamicEntity dynamicEntity)
        {
            pw.Write(ServerPacketID.SynchronizeDynamicEntity);
            pw.Write(dynamicEntity.MapEntityIndex);
            dynamicEntity.Serialize(pw);
        }

        public static PacketWriter SynchronizeDynamicEntity(DynamicEntity dynamicEntity)
        {
            var pw = GetWriter();
            SynchronizeDynamicEntity(pw, dynamicEntity);
            return pw;
        }

        public static PacketWriter UpdateEquipmentSlot(EquipmentSlot slot, GrhIndex? graphic)
        {
            var pw = GetWriter(ServerPacketID.UpdateEquipmentSlot);
            pw.WriteEnum(slot);
            pw.Write(graphic.HasValue);

            if (graphic.HasValue)
                pw.Write(graphic.Value);

            return pw;
        }

        public static void UpdateStat(PacketWriter pw, Stat<StatType> stat, StatCollectionType statCollectionType)
        {
            var isBaseStat = (statCollectionType == StatCollectionType.Base);

            pw.Write(ServerPacketID.UpdateStat);
            pw.Write(isBaseStat);
            pw.Write(stat);
        }

        public static PacketWriter UpdateStat(Stat<StatType> stat, StatCollectionType statCollectionType)
        {
            var pw = GetWriter();
            UpdateStat(pw, stat, statCollectionType);
            return pw;
        }

        public static void UpdateVelocityAndPosition(PacketWriter pw, DynamicEntity dynamicEntity, TickCount currentTime)
        {
            pw.Write(ServerPacketID.UpdateVelocityAndPosition);
            pw.Write(dynamicEntity.MapEntityIndex);
            dynamicEntity.SerializePositionAndVelocity(pw, currentTime);
        }

        public static PacketWriter UpdateVelocityAndPosition(DynamicEntity dynamicEntity, TickCount currentTime)
        {
            var pw = GetWriter();
            UpdateVelocityAndPosition(pw, dynamicEntity, currentTime);
            return pw;
        }

        public static PacketWriter UseEntity(MapEntityIndex usedEntity, MapEntityIndex usedBy)
        {
            var pw = GetWriter(ServerPacketID.UseEntity);
            pw.Write(usedEntity);
            pw.Write(usedBy);
            return pw;
        }

        public static PacketWriter ReceiveFriends(string onlineFriends, string friendsMap, string friendsList)
        {
            var pw = GetWriter(ServerPacketID.ReceiveFriends);
            pw.Write(onlineFriends);
            pw.Write(friendsMap);
            pw.Write(friendsList);
            return pw;
        }

        public static PacketWriter ReceivePrivateMessage(string message)
        {
            var pw = GetWriter(ServerPacketID.ReceivePrivateMessage);
            pw.Write(message);
            return pw;
        }

        public static PacketWriter ReceiveAllUsers(string onlineUsers)
        {
            var pw = GetWriter(ServerPacketID.ReceiveOnlineUsers);
            pw.Write(onlineUsers);
            return pw;
        }
    }
}