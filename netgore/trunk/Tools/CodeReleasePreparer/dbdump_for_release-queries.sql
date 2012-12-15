TRUNCATE TABLE `account_ips`;
TRUNCATE TABLE `account_ban`;
TRUNCATE TABLE `active_trade_cash`;
TRUNCATE TABLE `active_trade_item`;
TRUNCATE TABLE `character_quest_status`;
TRUNCATE TABLE `character_quest_status_kills`;
TRUNCATE TABLE `character_status_effect`;
TRUNCATE TABLE `event_counters_guild`;
TRUNCATE TABLE `event_counters_item_template`;
TRUNCATE TABLE `event_counters_map`;
TRUNCATE TABLE `event_counters_npc`;
TRUNCATE TABLE `event_counters_quest`;
TRUNCATE TABLE `event_counters_shop`;
TRUNCATE TABLE `event_counters_user`;
TRUNCATE TABLE `guild_event`;
TRUNCATE TABLE `guild_member`;
DELETE FROM `guild`;
TRUNCATE TABLE `world_stats_count_consume_item`;
TRUNCATE TABLE `world_stats_count_item_buy`;
TRUNCATE TABLE `world_stats_count_item_create`;
TRUNCATE TABLE `world_stats_count_item_sell`;
TRUNCATE TABLE `world_stats_count_npc_kill_user`;
TRUNCATE TABLE `world_stats_count_shop_buy`;
TRUNCATE TABLE `world_stats_count_shop_sell`;
TRUNCATE TABLE `world_stats_count_user_consume_item`;
TRUNCATE TABLE `world_stats_count_user_kill_npc`;
TRUNCATE TABLE `world_stats_guild_user_change`;
TRUNCATE TABLE `world_stats_network`;
TRUNCATE TABLE `world_stats_npc_kill_user`;
TRUNCATE TABLE `world_stats_quest_accept`;
TRUNCATE TABLE `world_stats_quest_cancel`;
TRUNCATE TABLE `world_stats_quest_complete`;
TRUNCATE TABLE `world_stats_user_consume_item`;
TRUNCATE TABLE `world_stats_user_kill_npc`;
TRUNCATE TABLE `world_stats_user_level`;
TRUNCATE TABLE `world_stats_user_shopping`;

DELETE FROM `character_equipped` WHERE `character_id` != 1;
DELETE FROM `character_inventory` WHERE `character_id` != 1;

DELETE FROM `item` WHERE item.id NOT IN (
	SELECT `item_id` FROM `character_equipped` WHERE `character_id` = 1 UNION
	SELECT `item_id` FROM `character_inventory` WHERE `character_id` = 1);

DELETE FROM `character` WHERE `id` != 1;
DELETE FROM `account` WHERE `id` != 1;
