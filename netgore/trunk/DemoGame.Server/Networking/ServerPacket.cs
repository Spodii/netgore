using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.DbObjs;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Audio;
using NetGore.Features.Emoticons;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Network;
using NetGore.Stats;

namespace DemoGame.Server
{
    /// <summary>
    /// Packets going out from the server / in to the client.
    /// </summary>
    static class ServerPacket
    {
        static readonly PacketWriterPool _writerPool = new PacketWriterPool();

        public static void SetProvidedQuests(PacketWriter pw, MapEntityIndex mapEntityIndex, IEnumerable<QuestID> quests)
        {
            pw.WriteEnum(ServerPacketID.SetProvidedQuests);
            pw.Write(mapEntityIndex);
            pw.Write((byte)quests.Count());
            foreach (var q in quests)
                pw.Write(q);
        }

        public static PacketWriter AcceptQuestReply(QuestID questID, bool successful)
        {
            var pw = GetWriter(ServerPacketID.AcceptQuestReply);
            pw.Write(questID);
            pw.Write(successful);
            return pw;
        }

        public static PacketWriter AddStatusEffect(StatusEffectType statusEffectType, ushort power, int timeLeft)
        {
            ushort secsLeft = (ushort)((timeLeft / 1000).Clamp(ushort.MinValue, ushort.MaxValue));

            PacketWriter pw = GetWriter(ServerPacketID.AddStatusEffect);
            pw.WriteEnum(statusEffectType);
            pw.Write(power);
            pw.Write(secsLeft);
            return pw;
        }

        public static PacketWriter CharAttack(MapEntityIndex mapEntityIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.CharAttack);
            pw.Write(mapEntityIndex);
            return pw;
        }

        public static PacketWriter CharDamage(MapEntityIndex mapEntityIndex, int damage)
        {
            PacketWriter pw = GetWriter(ServerPacketID.CharDamage);
            pw.Write(mapEntityIndex);
            pw.Write(damage);
            return pw;
        }

        public static PacketWriter Chat(string text)
        {
            PacketWriter pw = GetWriter(ServerPacketID.Chat);
            pw.Write(text, GameData.MaxServerSayLength);
            return pw;
        }

        public static PacketWriter ChatSay(string name, MapEntityIndex mapEntityIndex, string text)
        {
            PacketWriter pw = GetWriter(ServerPacketID.ChatSay);
            pw.Write(name, GameData.MaxServerSayNameLength);
            pw.Write(mapEntityIndex);
            pw.Write(text, GameData.MaxServerSayLength);
            return pw;
        }

        public static PacketWriter CreateAccount(bool successful, string failureMessage)
        {
            var pw = GetWriter(ServerPacketID.CreateAccount);
            pw.Write(successful);
            if (!successful)
                pw.Write(failureMessage);
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
            DynamicEntityFactory.Instance.Write(pw, dynamicEntity);
        }

        public static PacketWriter CreateDynamicEntity(DynamicEntity dynamicEntity)
        {
            PacketWriter pw = GetWriter();
            CreateDynamicEntity(pw, dynamicEntity);
            return pw;
        }

        public static PacketWriter Emote(MapEntityIndex mapEntityIndex, Emoticon emoticon)
        {
            PacketWriter pw = GetWriter(ServerPacketID.Emote);
            pw.Write(mapEntityIndex);
            pw.WriteEnum(emoticon);
            return pw;
        }

        public static PacketWriter EndChatDialog()
        {
            return GetWriter(ServerPacketID.EndChatDialog);
        }

        /// <summary>
        /// Gets a PacketWriter to use from the internal pool. It is important that this
        /// PacketWriter is disposed of properly when done.
        /// </summary>
        /// <returns>PacketWriter to use.</returns>
        public static PacketWriter GetWriter()
        {
            return _writerPool.Acquire();
        }

        /// <summary>
        /// Gets a PacketWriter to use from the internal pool. It is important that this
        /// PacketWriter is disposed of properly when done.
        /// </summary>
        /// <param name="id">ServerPacketID that this PacketWriter will be writing.</param>
        /// <returns>PacketWriter to use.</returns>
        static PacketWriter GetWriter(ServerPacketID id)
        {
            PacketWriter pw = _writerPool.Acquire();
            pw.Write(id);
            return pw;
        }

        public static PacketWriter GroupInfo(Action<PacketWriter> populate)
        {
            var ret = GetWriter(ServerPacketID.GroupInfo);
            populate(ret);
            return ret;
        }

        public static PacketWriter QuestInfo(Action<PacketWriter> populate)
        {
            var ret = GetWriter(ServerPacketID.QuestInfo);
            populate(ret);
            return ret;
        }

        public static PacketWriter GuildInfo(Action<PacketWriter> populate)
        {
            var ret = GetWriter(ServerPacketID.GuildInfo);
            populate(ret);
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
            PacketWriter pw = GetWriter(ServerPacketID.LoginUnsuccessful);
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
            PacketWriter pw = GetWriter(ServerPacketID.LoginUnsuccessful);
            pw.Write(gameMessage, p);
            return pw;
        }

        public static PacketWriter NotifyExpCash(int exp, int cash)
        {
            PacketWriter pw = GetWriter(ServerPacketID.NotifyExpCash);
            pw.Write(exp);
            pw.Write(cash);
            return pw;
        }

        public static PacketWriter NotifyGetItem(string name, byte amount)
        {
            PacketWriter pw = GetWriter(ServerPacketID.NotifyGetItem);
            pw.Write(name);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter NotifyLevel(MapEntityIndex mapEntityIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.NotifyLevel);
            pw.Write(mapEntityIndex);
            return pw;
        }

        public static PacketWriter Ping()
        {
            return GetWriter(ServerPacketID.Ping);
        }

        public static PacketWriter PlaySound(SoundID sound)
        {
            PacketWriter pw = GetWriter(ServerPacketID.PlaySound);
            pw.Write(sound);
            return pw;
        }

        public static PacketWriter PlaySoundAt(SoundID sound, Vector2 position)
        {
            PacketWriter pw = GetWriter(ServerPacketID.PlaySoundAt);
            pw.Write(sound);
            pw.Write(position);
            return pw;
        }

        public static PacketWriter PlaySoundAtEntity(SoundID sound, MapEntityIndex mapEntityIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.PlaySoundAtEntity);
            pw.Write(sound);
            pw.Write(mapEntityIndex);
            return pw;
        }

        public static PacketWriter RemoveDynamicEntity(DynamicEntity dynamicEntity)
        {
            PacketWriter pw = GetWriter(ServerPacketID.RemoveDynamicEntity);
            pw.Write(dynamicEntity.MapEntityIndex);
            return pw;
        }

        public static PacketWriter RemoveStatusEffect(StatusEffectType statusEffectType)
        {
            PacketWriter pw = GetWriter(ServerPacketID.RemoveStatusEffect);
            pw.WriteEnum(statusEffectType);
            return pw;
        }

        public static PacketWriter SendAccountCharacters(AccountCharacterInfo[] charInfos)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SendAccountCharacters);

            if (charInfos == null || charInfos.Length == 0)
                pw.Write((byte)0);
            else
            {
                pw.Write((byte)charInfos.Length);
                for (int i = 0; i < charInfos.Length; i++)
                {
                    pw.Write(charInfos[i]);
                }
            }

            return pw;
        }

        public static PacketWriter SendEquipmentItemInfo(EquipmentSlot slot, ItemEntity item)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SendEquipmentItemInfo);
            pw.WriteEnum(slot);
            new ItemTable(item).WriteState(pw);
            return pw;
        }

        public static PacketWriter SendInventoryItemInfo(InventorySlot slot, ItemEntity item)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SendInventoryItemInfo);
            pw.Write(slot);
            new ItemTable(item).WriteState(pw);
            return pw;
        }

        public static PacketWriter SendMessage(GameMessage gameMessage)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SendMessage);
            pw.Write(gameMessage);
            return pw;
        }

        public static PacketWriter SendMessage(GameMessage gameMessage, params object[] p)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SendMessage);
            pw.Write(gameMessage, p);
            return pw;
        }

        public static PacketWriter SetCash(int cash)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetCash);
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
            PacketWriter pw = GetWriter();
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
            PacketWriter pw = GetWriter();
            SetCharacterMPPercent(pw, mapEntityIndex, mpPercent);
            return pw;
        }

        public static PacketWriter SetCharacterPaperDoll(MapEntityIndex mapEntityIndex, IEnumerable<string> layers)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetCharacterPaperDoll);
            pw.Write(mapEntityIndex);

            pw.Write((byte)layers.Count());
            foreach (var layer in layers)
            {
                pw.Write(layer);
            }

            return pw;
        }

        public static PacketWriter SetChatDialogPage(ushort pageIndex, IEnumerable<byte> responsesToSkip)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetChatDialogPage);
            pw.Write(pageIndex);

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
                foreach (byte responseToSkip in responsesToSkip)
                {
                    pw.Write(responseToSkip);
                }
            }

            return pw;
        }

        public static PacketWriter SetExp(int exp)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetExp);
            pw.Write(exp);
            return pw;
        }

        public static void SetHP(PacketWriter pw, SPValueType hp)
        {
            pw.Write(ServerPacketID.SetHP);
            pw.Write(hp);
        }

        public static PacketWriter SetHP(SPValueType hp)
        {
            PacketWriter pw = GetWriter();
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
            PacketWriter pw = GetWriter();
            SetInventorySlot(pw, slot, graphic, amount);
            return pw;
        }

        public static PacketWriter SetLevel(byte level)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetLevel);
            pw.Write(level);
            return pw;
        }

        public static void SetMap(PacketWriter pw, MapIndex mapIndex)
        {
            pw.Write(ServerPacketID.SetMap);
            pw.Write(mapIndex);
        }

        public static PacketWriter SetMap(MapIndex mapIndex)
        {
            PacketWriter pw = GetWriter();
            SetMap(pw, mapIndex);
            return pw;
        }

        public static void SetMP(PacketWriter pw, SPValueType mp)
        {
            pw.Write(ServerPacketID.SetMP);
            pw.Write(mp);
        }

        public static PacketWriter SetMP(SPValueType mp)
        {
            PacketWriter pw = GetWriter();
            SetMP(pw, mp);
            return pw;
        }

        public static PacketWriter SetSkillGroupCooldown(byte skillGroup, ushort cooldownTime)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetSkillGroupCooldown);
            pw.Write(skillGroup);
            pw.Write(cooldownTime);
            return pw;
        }

        public static PacketWriter SetStatPoints(int statPoints)
        {
            PacketWriter pw = GetWriter(ServerPacketID.SetStatPoints);
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
            PacketWriter pw = GetWriter();
            SetUserChar(pw, mapEntityIndex);
            return pw;
        }

        public static PacketWriter StartCastingSkill(SkillType skillType, ushort castTime)
        {
            PacketWriter pw = GetWriter(ServerPacketID.StartCastingSkill);
            pw.WriteEnum(skillType);
            pw.Write(castTime);
            return pw;
        }

        public static PacketWriter StartQuestChatDialog(MapEntityIndex npcIndex, QuestID[] availableQuests)
        {
            PacketWriter pw = GetWriter(ServerPacketID.StartQuestChatDialog);

            pw.Write(npcIndex);

            if (availableQuests != null)
            {
                Debug.Assert(availableQuests.Length <= byte.MaxValue);
                pw.Write((byte)availableQuests.Length);
                foreach (var q in availableQuests)
                    pw.Write(q);
            }
            else
            {
                pw.Write((byte)0);
            }

            return pw;
        }

        public static PacketWriter StartChatDialog(MapEntityIndex npcIndex, ushort dialogIndex)
        {
            PacketWriter pw = GetWriter(ServerPacketID.StartChatDialog);
            pw.Write(npcIndex);
            pw.Write(dialogIndex);
            return pw;
        }

        public static PacketWriter StartShopping(MapEntityIndex shopOwnerIndex, IShop<ShopItem> shop)
        {
            PacketWriter pw = GetWriter(ServerPacketID.StartShopping);
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
            pw.Write(ServerPacketID.CreateDynamicEntity);
            pw.Write(dynamicEntity.MapEntityIndex);
            dynamicEntity.Serialize(pw);
        }

        public static PacketWriter SynchronizeDynamicEntity(DynamicEntity dynamicEntity)
        {
            PacketWriter pw = GetWriter();
            SynchronizeDynamicEntity(pw, dynamicEntity);
            return pw;
        }

        public static PacketWriter UpdateEquipmentSlot(EquipmentSlot slot, GrhIndex? graphic)
        {
            PacketWriter pw = GetWriter(ServerPacketID.UpdateEquipmentSlot);
            pw.WriteEnum(slot);
            pw.Write(graphic.HasValue);

            if (graphic.HasValue)
                pw.Write(graphic.Value);

            return pw;
        }

        public static void UpdateStat(PacketWriter pw, IStat<StatType> stat, StatCollectionType statCollectionType)
        {
            bool isBaseStat = (statCollectionType == StatCollectionType.Base);

            pw.Write(ServerPacketID.UpdateStat);
            pw.Write(isBaseStat);
            pw.WriteEnum(stat.StatType);
            stat.Write(pw);
        }

        public static PacketWriter UpdateStat(IStat<StatType> stat, StatCollectionType statCollectionType)
        {
            PacketWriter pw = GetWriter();
            UpdateStat(pw, stat, statCollectionType);
            return pw;
        }

        public static void UpdateVelocityAndPosition(PacketWriter pw, DynamicEntity dynamicEntity, int currentTime)
        {
            pw.Write(ServerPacketID.UpdateVelocityAndPosition);
            pw.Write(dynamicEntity.MapEntityIndex);
            dynamicEntity.SerializePositionAndVelocity(pw, currentTime);
        }

        public static PacketWriter UpdateVelocityAndPosition(DynamicEntity dynamicEntity, int currentTime)
        {
            PacketWriter pw = GetWriter();
            UpdateVelocityAndPosition(pw, dynamicEntity, currentTime);
            return pw;
        }

        public static PacketWriter UseEntity(MapEntityIndex usedEntity, MapEntityIndex usedBy)
        {
            PacketWriter pw = GetWriter(ServerPacketID.UseEntity);
            pw.Write(usedEntity);
            pw.Write(usedBy);
            return pw;
        }

        public static PacketWriter UseSkill(MapEntityIndex user, MapEntityIndex? target, SkillType skillType)
        {
            PacketWriter pw = GetWriter(ServerPacketID.UseSkill);
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
    }
}