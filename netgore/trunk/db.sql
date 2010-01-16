/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50138
Source Host           : localhost:3306
Source Database       : demogame

Target Server Type    : MYSQL
Target Server Version : 50138
File Encoding         : 65001

Date: 2010-01-16 01:04:12
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `account`
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `id` int(11) NOT NULL COMMENT 'The account ID.',
  `name` varchar(30) NOT NULL COMMENT 'The account name.',
  `password` varchar(40) NOT NULL COMMENT 'The account password.',
  `email` varchar(60) NOT NULL COMMENT 'The email address.',
  `time_created` datetime NOT NULL COMMENT 'The DateTime of when the account was created.',
  `time_last_login` datetime NOT NULL COMMENT 'The DateTime that the account was last logged in to.',
  `creator_ip` int(10) unsigned NOT NULL COMMENT 'The IP address that created the account.',
  `current_ip` int(10) unsigned DEFAULT NULL COMMENT 'IP address currently logged in to the account, or null if nobody is logged in.',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of account
-- ----------------------------
INSERT INTO `account` VALUES ('1', 'Spodi', '3fc0a7acf087f549ac2b266baf94b8b1', 'spodi@vbgore.com', '2009-09-07 15:43:16', '2010-01-15 21:27:15', '16777343', null);

-- ----------------------------
-- Table structure for `account_ips`
-- ----------------------------
DROP TABLE IF EXISTS `account_ips`;
CREATE TABLE `account_ips` (
  `account_id` int(11) NOT NULL COMMENT 'The ID of the account.',
  `ip` int(10) unsigned NOT NULL COMMENT 'The IP that logged into the account.',
  `time` datetime NOT NULL COMMENT 'When this IP last logged into this account.',
  PRIMARY KEY (`account_id`,`time`),
  KEY `account_id` (`account_id`,`ip`),
  CONSTRAINT `account_ips_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of account_ips
-- ----------------------------
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 09:56:47');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 10:07:26');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 12:48:29');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 12:54:25');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 12:55:43');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 12:57:52');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 13:10:33');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 13:18:55');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 13:33:47');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 13:35:42');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 13:50:43');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-07 21:17:13');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-08 15:44:00');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-09 13:34:23');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 09:48:59');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 10:43:24');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 21:20:21');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 21:20:50');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 21:21:31');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 21:22:37');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 21:23:57');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 21:24:42');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 21:28:29');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:00:48');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:02:12');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:08:53');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:09:44');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:10:07');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:11:56');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:16:50');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:20:31');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:26:27');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:31:24');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:32:38');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:33:44');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-11 22:44:05');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-12 00:24:48');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-12 00:25:29');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-12 00:45:32');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-12 00:52:11');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-13 15:16:15');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-14 12:56:06');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-14 13:09:55');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-14 13:09:56');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-14 13:09:58');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-14 14:36:49');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-14 14:44:59');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-14 16:18:31');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-14 16:26:53');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-14 17:27:08');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-15 19:34:53');
INSERT INTO `account_ips` VALUES ('1', '16777343', '2010-01-15 21:27:15');

-- ----------------------------
-- Table structure for `alliance`
-- ----------------------------
DROP TABLE IF EXISTS `alliance`;
CREATE TABLE `alliance` (
  `id` tinyint(3) unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of alliance
-- ----------------------------
INSERT INTO `alliance` VALUES ('0', 'user');
INSERT INTO `alliance` VALUES ('1', 'monster');

-- ----------------------------
-- Table structure for `alliance_attackable`
-- ----------------------------
DROP TABLE IF EXISTS `alliance_attackable`;
CREATE TABLE `alliance_attackable` (
  `alliance_id` tinyint(3) unsigned NOT NULL,
  `attackable_id` tinyint(3) unsigned NOT NULL,
  `placeholder` tinyint(3) unsigned DEFAULT NULL COMMENT 'Unused placeholder column - please do not remove',
  PRIMARY KEY (`alliance_id`,`attackable_id`),
  KEY `attackable_id` (`attackable_id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `alliance_attackable_ibfk_3` FOREIGN KEY (`attackable_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `alliance_attackable_ibfk_4` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of alliance_attackable
-- ----------------------------
INSERT INTO `alliance_attackable` VALUES ('0', '1', null);
INSERT INTO `alliance_attackable` VALUES ('1', '0', null);

-- ----------------------------
-- Table structure for `alliance_hostile`
-- ----------------------------
DROP TABLE IF EXISTS `alliance_hostile`;
CREATE TABLE `alliance_hostile` (
  `alliance_id` tinyint(3) unsigned NOT NULL,
  `hostile_id` tinyint(3) unsigned NOT NULL,
  `placeholder` tinyint(3) unsigned DEFAULT NULL COMMENT 'Unused placeholder column - please do not remove',
  PRIMARY KEY (`alliance_id`,`hostile_id`),
  KEY `hostile_id` (`hostile_id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `alliance_hostile_ibfk_3` FOREIGN KEY (`hostile_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `alliance_hostile_ibfk_4` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of alliance_hostile
-- ----------------------------
INSERT INTO `alliance_hostile` VALUES ('0', '1', '0');
INSERT INTO `alliance_hostile` VALUES ('1', '0', '0');

-- ----------------------------
-- Table structure for `character`
-- ----------------------------
DROP TABLE IF EXISTS `character`;
CREATE TABLE `character` (
  `id` int(11) NOT NULL,
  `account_id` int(11) DEFAULT NULL,
  `character_template_id` smallint(5) unsigned DEFAULT NULL,
  `name` varchar(30) NOT NULL,
  `map_id` smallint(5) unsigned NOT NULL DEFAULT '1',
  `shop_id` smallint(5) unsigned DEFAULT NULL,
  `chat_dialog` smallint(5) unsigned DEFAULT NULL,
  `ai_id` smallint(5) unsigned DEFAULT NULL,
  `x` float NOT NULL DEFAULT '100',
  `y` float NOT NULL DEFAULT '100',
  `respawn_map` smallint(5) unsigned DEFAULT NULL,
  `respawn_x` float NOT NULL DEFAULT '50',
  `respawn_y` float NOT NULL DEFAULT '50',
  `body_id` smallint(5) unsigned NOT NULL DEFAULT '1',
  `move_speed` smallint(5) unsigned NOT NULL DEFAULT '1800',
  `cash` int(11) NOT NULL DEFAULT '0',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `exp` int(11) NOT NULL DEFAULT '0',
  `statpoints` int(11) NOT NULL DEFAULT '0',
  `hp` smallint(6) NOT NULL DEFAULT '50',
  `mp` smallint(6) NOT NULL DEFAULT '50',
  `stat_maxhp` smallint(6) NOT NULL DEFAULT '50',
  `stat_maxmp` smallint(6) NOT NULL DEFAULT '50',
  `stat_minhit` smallint(6) NOT NULL DEFAULT '1',
  `stat_maxhit` smallint(6) NOT NULL DEFAULT '1',
  `stat_defence` smallint(6) NOT NULL DEFAULT '1',
  `stat_agi` smallint(6) NOT NULL DEFAULT '1',
  `stat_int` smallint(6) NOT NULL DEFAULT '1',
  `stat_str` smallint(6) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `template_id` (`character_template_id`),
  KEY `respawn_map` (`respawn_map`),
  KEY `character_ibfk_2` (`map_id`),
  KEY `idx_name` (`name`) USING BTREE,
  KEY `account_id` (`account_id`),
  KEY `shop_id` (`shop_id`),
  CONSTRAINT `character_ibfk_2` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_3` FOREIGN KEY (`respawn_map`) REFERENCES `map` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_4` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_5` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_6` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character
-- ----------------------------
INSERT INTO `character` VALUES ('1', '1', null, 'Spodi', '1', null, null, null, '559.201', '498', '1', '500', '200', '1', '1800', '2241', '93', '2769', '438', '47', '50', '50', '50', '7', '12', '0', '1', '1', '2');
INSERT INTO `character` VALUES ('2', null, '1', 'Test A', '2', null, null, '1', '800', '250', '2', '800', '250', '1', '1800', '3012', '12', '810', '527', '5', '5', '5', '5', '5', '5', '0', '5', '5', '5');
INSERT INTO `character` VALUES ('3', null, '1', 'Test B', '2', null, null, '1', '500', '250', '2', '500', '250', '1', '1800', '3012', '12', '810', '527', '5', '5', '5', '5', '5', '5', '0', '5', '5', '5');
INSERT INTO `character` VALUES ('4', null, null, 'Talking Guy', '2', null, '0', null, '800', '530', '2', '800', '530', '1', '1800', '0', '1', '0', '0', '50', '50', '50', '50', '1', '1', '0', '1', '1', '1');
INSERT INTO `character` VALUES ('5', null, null, 'Shopkeeper', '2', '0', null, null, '600', '530', '2', '600', '530', '1', '1800', '0', '1', '0', '0', '50', '50', '50', '50', '1', '1', '0', '1', '1', '1');
INSERT INTO `character` VALUES ('6', null, null, 'Vending Machine', '2', '1', null, null, '500', '530', '2', '500', '530', '1', '1800', '0', '1', '0', '0', '50', '50', '50', '50', '1', '1', '0', '1', '1', '1');

-- ----------------------------
-- Table structure for `character_equipped`
-- ----------------------------
DROP TABLE IF EXISTS `character_equipped`;
CREATE TABLE `character_equipped` (
  `character_id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `slot` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`character_id`,`slot`),
  KEY `item_id` (`item_id`),
  CONSTRAINT `character_equipped_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_equipped_ibfk_4` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character_equipped
-- ----------------------------
INSERT INTO `character_equipped` VALUES ('3', '332', '2');

-- ----------------------------
-- Table structure for `character_inventory`
-- ----------------------------
DROP TABLE IF EXISTS `character_inventory`;
CREATE TABLE `character_inventory` (
  `character_id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `slot` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`character_id`,`slot`),
  KEY `item_id` (`item_id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `character_inventory_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_inventory_ibfk_4` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character_inventory
-- ----------------------------
INSERT INTO `character_inventory` VALUES ('1', '299', '2');
INSERT INTO `character_inventory` VALUES ('1', '304', '0');
INSERT INTO `character_inventory` VALUES ('1', '305', '1');
INSERT INTO `character_inventory` VALUES ('1', '313', '4');
INSERT INTO `character_inventory` VALUES ('1', '314', '3');

-- ----------------------------
-- Table structure for `character_status_effect`
-- ----------------------------
DROP TABLE IF EXISTS `character_status_effect`;
CREATE TABLE `character_status_effect` (
  `id` int(11) NOT NULL COMMENT 'Unique ID of the status effect instance.',
  `character_id` int(11) NOT NULL COMMENT 'ID of the Character that the status effect is on.',
  `status_effect_id` tinyint(3) unsigned NOT NULL COMMENT 'ID of the status effect that this effect is for. This corresponds to the StatusEffectType enum''s value.',
  `power` smallint(5) unsigned NOT NULL COMMENT 'The power of this status effect instance.',
  `time_left_secs` smallint(5) unsigned NOT NULL COMMENT 'The amount of time remaining for this status effect in seconds.',
  PRIMARY KEY (`id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `character_status_effect_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character_status_effect
-- ----------------------------

-- ----------------------------
-- Table structure for `character_template`
-- ----------------------------
DROP TABLE IF EXISTS `character_template`;
CREATE TABLE `character_template` (
  `id` smallint(5) unsigned NOT NULL,
  `alliance_id` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `name` varchar(50) NOT NULL DEFAULT 'New NPC',
  `ai_id` smallint(5) unsigned DEFAULT NULL,
  `shop_id` smallint(5) unsigned DEFAULT NULL,
  `body_id` smallint(5) unsigned NOT NULL DEFAULT '1',
  `move_speed` smallint(5) unsigned NOT NULL DEFAULT '1800',
  `respawn` smallint(5) unsigned NOT NULL DEFAULT '5',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `exp` int(11) NOT NULL DEFAULT '0',
  `statpoints` int(11) NOT NULL DEFAULT '0',
  `give_exp` smallint(5) unsigned NOT NULL DEFAULT '0',
  `give_cash` smallint(5) unsigned NOT NULL DEFAULT '0',
  `stat_maxhp` smallint(6) NOT NULL DEFAULT '50',
  `stat_maxmp` smallint(6) NOT NULL DEFAULT '50',
  `stat_minhit` smallint(6) NOT NULL DEFAULT '1',
  `stat_maxhit` smallint(6) NOT NULL DEFAULT '1',
  `stat_defence` smallint(6) NOT NULL DEFAULT '1',
  `stat_agi` smallint(6) NOT NULL DEFAULT '1',
  `stat_int` smallint(6) NOT NULL DEFAULT '1',
  `stat_str` smallint(6) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `alliance_id` (`alliance_id`),
  KEY `shop_id` (`shop_id`),
  CONSTRAINT `character_template_ibfk_2` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_ibfk_3` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character_template
-- ----------------------------
INSERT INTO `character_template` VALUES ('0', '0', 'User Template', null, null, '1', '1800', '5', '1', '0', '0', '0', '0', '50', '50', '1', '2', '0', '1', '1', '1');
INSERT INTO `character_template` VALUES ('1', '1', 'A Test NPC', '1', null, '2', '1800', '2', '0', '0', '0', '5', '5', '5', '5', '0', '0', '0', '1', '1', '1');

-- ----------------------------
-- Table structure for `character_template_equipped`
-- ----------------------------
DROP TABLE IF EXISTS `character_template_equipped`;
CREATE TABLE `character_template_equipped` (
  `id` int(11) NOT NULL,
  `character_template_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  `chance` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `item_id` (`item_template_id`),
  KEY `character_id` (`character_template_id`),
  CONSTRAINT `character_template_equipped_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_equipped_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character_template_equipped
-- ----------------------------
INSERT INTO `character_template_equipped` VALUES ('0', '1', '5', '3000');
INSERT INTO `character_template_equipped` VALUES ('1', '1', '4', '3000');
INSERT INTO `character_template_equipped` VALUES ('2', '1', '3', '60000');

-- ----------------------------
-- Table structure for `character_template_inventory`
-- ----------------------------
DROP TABLE IF EXISTS `character_template_inventory`;
CREATE TABLE `character_template_inventory` (
  `id` int(11) NOT NULL,
  `character_template_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  `min` tinyint(3) unsigned NOT NULL,
  `max` tinyint(3) unsigned NOT NULL,
  `chance` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `item_id` (`item_template_id`),
  KEY `character_id` (`character_template_id`),
  CONSTRAINT `character_template_inventory_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_inventory_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character_template_inventory
-- ----------------------------
INSERT INTO `character_template_inventory` VALUES ('0', '1', '5', '0', '2', '10000');
INSERT INTO `character_template_inventory` VALUES ('1', '1', '4', '1', '2', '5000');
INSERT INTO `character_template_inventory` VALUES ('2', '1', '3', '1', '1', '5000');
INSERT INTO `character_template_inventory` VALUES ('3', '1', '2', '0', '1', '10000');
INSERT INTO `character_template_inventory` VALUES ('4', '1', '1', '0', '5', '10000');

-- ----------------------------
-- Table structure for `game_constant`
-- ----------------------------
DROP TABLE IF EXISTS `game_constant`;
CREATE TABLE `game_constant` (
  `max_characters_per_account` tinyint(3) unsigned NOT NULL,
  `min_account_name_length` tinyint(3) unsigned NOT NULL,
  `max_account_name_length` tinyint(3) unsigned NOT NULL,
  `min_account_password_length` tinyint(3) unsigned NOT NULL,
  `max_account_password_length` tinyint(3) unsigned NOT NULL,
  `min_character_name_length` tinyint(3) unsigned NOT NULL,
  `max_character_name_length` tinyint(3) unsigned NOT NULL,
  `max_shop_items` tinyint(3) unsigned NOT NULL,
  `max_status_effect_power` smallint(5) unsigned NOT NULL,
  `screen_width` smallint(5) unsigned NOT NULL,
  `screen_height` smallint(5) unsigned NOT NULL,
  `server_ping_port` smallint(5) unsigned NOT NULL,
  `server_tcp_port` smallint(5) unsigned NOT NULL,
  `server_ip` varchar(15) NOT NULL,
  `world_physics_update_rate` smallint(5) unsigned NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- ----------------------------
-- Records of game_constant
-- ----------------------------
INSERT INTO `game_constant` VALUES ('10', '3', '30', '3', '30', '3', '15', '0', '500', '1024', '768', '44446', '44445', '127.0.0.1', '20');

-- ----------------------------
-- Table structure for `item`
-- ----------------------------
DROP TABLE IF EXISTS `item`;
CREATE TABLE `item` (
  `id` int(11) NOT NULL,
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `width` tinyint(3) unsigned NOT NULL DEFAULT '16',
  `height` tinyint(3) unsigned NOT NULL DEFAULT '16',
  `name` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  `amount` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `graphic` smallint(5) unsigned NOT NULL DEFAULT '0',
  `value` int(11) NOT NULL DEFAULT '0',
  `hp` smallint(6) NOT NULL DEFAULT '0',
  `mp` smallint(6) NOT NULL DEFAULT '0',
  `stat_agi` smallint(6) NOT NULL DEFAULT '0',
  `stat_int` smallint(6) NOT NULL DEFAULT '0',
  `stat_str` smallint(6) NOT NULL DEFAULT '0',
  `stat_minhit` smallint(6) NOT NULL DEFAULT '0',
  `stat_maxhit` smallint(6) NOT NULL DEFAULT '0',
  `stat_maxhp` smallint(6) NOT NULL DEFAULT '0',
  `stat_maxmp` smallint(6) NOT NULL DEFAULT '0',
  `stat_defence` smallint(6) NOT NULL DEFAULT '0',
  `stat_req_agi` smallint(6) NOT NULL DEFAULT '0',
  `stat_req_int` smallint(6) NOT NULL DEFAULT '0',
  `stat_req_str` smallint(6) NOT NULL DEFAULT '0',
  `equipped_body` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of item
-- ----------------------------
INSERT INTO `item` VALUES ('0', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('1', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('2', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('3', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('4', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('5', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('6', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('7', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('8', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('9', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('10', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('11', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('12', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('13', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('14', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('15', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('16', '1', '9', '16', 'Healing Potion', 'A healing potion', '3', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('17', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('18', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('19', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('20', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('21', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('22', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('23', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('24', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('25', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('26', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('27', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('28', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('29', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('30', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('31', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('32', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('33', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('34', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('35', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('36', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('37', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('38', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('39', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('40', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('41', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('42', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('43', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('44', '1', '9', '16', 'Healing Potion', 'A healing potion', '3', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('45', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('46', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('47', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('48', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('49', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('50', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('51', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('52', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('53', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('54', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('55', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('56', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('57', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('58', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('59', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('60', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('61', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('62', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('63', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('64', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('65', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('66', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('67', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('68', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('69', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('70', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('71', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('72', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('73', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('74', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('75', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('76', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('77', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('78', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('79', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('80', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('81', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('82', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('83', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('84', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('85', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('86', '1', '9', '16', 'Healing Potion', 'A healing potion', '3', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('87', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('88', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('89', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('90', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('91', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('92', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('93', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('94', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('95', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('96', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('97', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('98', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('99', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('100', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('101', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('102', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('103', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('104', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('105', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('106', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('107', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('108', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('109', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('110', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('111', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('112', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('113', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('114', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('115', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('116', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('117', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('118', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('119', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('120', '1', '9', '16', 'Healing Potion', 'A healing potion', '5', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('121', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('122', '1', '9', '16', 'Healing Potion', 'A healing potion', '3', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('123', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('124', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('125', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('126', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('127', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('128', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('129', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('130', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('131', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('132', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('133', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('134', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('135', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('136', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('137', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('138', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('139', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('140', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('141', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('142', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('143', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('144', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('145', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('146', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('147', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('148', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('149', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('150', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('151', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('152', '1', '9', '16', 'Healing Potion', 'A healing potion', '4', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('153', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('154', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('155', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('156', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('157', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('158', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('159', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('160', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('161', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('162', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('163', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('164', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('165', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('166', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('167', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('168', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('169', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('170', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('171', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('172', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('173', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('174', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('175', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('176', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('177', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('178', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('179', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('180', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('181', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('182', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('183', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('184', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('185', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('186', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('187', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('188', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('189', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('190', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('191', '1', '9', '16', 'Healing Potion', 'A healing potion', '3', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('192', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('193', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('194', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('195', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('196', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('197', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('198', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('199', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('200', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('201', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('202', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('203', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('204', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('205', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('206', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('207', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('208', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('209', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('210', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('211', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('212', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('213', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('214', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('215', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('216', '1', '9', '16', 'Healing Potion', 'A healing potion', '4', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('217', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('218', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('219', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('220', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('221', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('222', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('223', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('224', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('225', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('226', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('227', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('228', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('229', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('230', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('231', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('232', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('233', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('234', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('235', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('236', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('237', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('238', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('239', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('240', '1', '9', '16', 'Healing Potion', 'A healing potion', '5', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('241', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('242', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('243', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('244', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('245', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('246', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('247', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('248', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('249', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('250', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('251', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('252', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('253', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('254', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('255', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('256', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('257', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('258', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('259', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('260', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('261', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('262', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('263', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('264', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('265', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('266', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('267', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('268', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('269', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('270', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('271', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('272', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('273', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('274', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('275', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('276', '1', '9', '16', 'Healing Potion', 'A healing potion', '4', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('277', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('278', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('279', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('280', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('281', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('282', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('283', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('284', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('285', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('286', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('287', '1', '9', '16', 'Healing Potion', 'A healing potion', '5', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('288', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('289', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('290', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('291', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('292', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('293', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('294', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('295', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('296', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('297', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('298', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('299', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('300', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('301', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('302', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('303', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('304', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('305', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('306', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('307', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('308', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('309', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('310', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('311', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('312', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('313', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('314', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('315', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('316', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('317', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('318', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('319', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item` VALUES ('320', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('321', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('322', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('323', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('324', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('325', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('326', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('327', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('328', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('329', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('330', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('331', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('332', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('333', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');
INSERT INTO `item` VALUES ('334', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('335', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('336', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item` VALUES ('337', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');

-- ----------------------------
-- Table structure for `item_template`
-- ----------------------------
DROP TABLE IF EXISTS `item_template`;
CREATE TABLE `item_template` (
  `id` smallint(5) unsigned NOT NULL,
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `width` tinyint(3) unsigned NOT NULL DEFAULT '16',
  `height` tinyint(3) unsigned NOT NULL DEFAULT '16',
  `name` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  `graphic` smallint(5) unsigned NOT NULL DEFAULT '0',
  `value` int(11) NOT NULL DEFAULT '0',
  `hp` smallint(6) NOT NULL DEFAULT '0',
  `mp` smallint(6) NOT NULL DEFAULT '0',
  `stat_agi` smallint(6) NOT NULL DEFAULT '0',
  `stat_int` smallint(6) NOT NULL DEFAULT '0',
  `stat_str` smallint(6) NOT NULL DEFAULT '0',
  `stat_minhit` smallint(6) NOT NULL DEFAULT '0',
  `stat_maxhit` smallint(6) NOT NULL DEFAULT '0',
  `stat_maxhp` smallint(6) NOT NULL DEFAULT '0',
  `stat_maxmp` smallint(6) NOT NULL DEFAULT '0',
  `stat_defence` smallint(6) NOT NULL DEFAULT '0',
  `stat_req_agi` smallint(6) NOT NULL DEFAULT '0',
  `stat_req_int` smallint(6) NOT NULL DEFAULT '0',
  `stat_req_str` smallint(6) NOT NULL DEFAULT '0',
  `equipped_body` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of item_template
-- ----------------------------
INSERT INTO `item_template` VALUES ('1', '1', '9', '16', 'Healing Potion', 'A healing potion', '94', '10', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item_template` VALUES ('2', '1', '9', '16', 'Mana Potion', 'A mana potion', '95', '10', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item_template` VALUES ('3', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '96', '100', '0', '0', '0', '0', '0', '5', '10', '0', '0', '0', '0', '0', '0', null);
INSERT INTO `item_template` VALUES ('4', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '99', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '5', '0', '0', '0', 'crystal body');
INSERT INTO `item_template` VALUES ('5', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '97', '50', '0', '0', '0', '0', '0', '0', '0', '0', '0', '2', '0', '0', '0', 'crystal helmet');

-- ----------------------------
-- Table structure for `map`
-- ----------------------------
DROP TABLE IF EXISTS `map`;
CREATE TABLE `map` (
  `id` smallint(5) unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of map
-- ----------------------------
INSERT INTO `map` VALUES ('1', 'INSERT VALUE');
INSERT INTO `map` VALUES ('2', 'INSERT VALUE');

-- ----------------------------
-- Table structure for `map_spawn`
-- ----------------------------
DROP TABLE IF EXISTS `map_spawn`;
CREATE TABLE `map_spawn` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `map_id` smallint(5) unsigned NOT NULL,
  `character_template_id` smallint(5) unsigned NOT NULL,
  `amount` tinyint(3) unsigned NOT NULL,
  `x` smallint(5) unsigned DEFAULT NULL,
  `y` smallint(5) unsigned DEFAULT NULL,
  `width` smallint(5) unsigned DEFAULT NULL,
  `height` smallint(5) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `character_id` (`character_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `map_spawn_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `map_spawn_ibfk_2` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of map_spawn
-- ----------------------------
INSERT INTO `map_spawn` VALUES ('12', '1', '1', '5', null, null, null, null);
INSERT INTO `map_spawn` VALUES ('13', '1', '1', '1', null, null, null, null);
INSERT INTO `map_spawn` VALUES ('14', '1', '1', '1', null, null, null, null);

-- ----------------------------
-- Table structure for `server_setting`
-- ----------------------------
DROP TABLE IF EXISTS `server_setting`;
CREATE TABLE `server_setting` (
  `motd` varchar(250) NOT NULL DEFAULT '' COMMENT 'The message of the day.'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of server_setting
-- ----------------------------
INSERT INTO `server_setting` VALUES ('Welcome to NetGore! Use the arrow keys to move, control to attack, alt to talk to NPCs and use world entities, and space to pick up items.');

-- ----------------------------
-- Table structure for `server_time`
-- ----------------------------
DROP TABLE IF EXISTS `server_time`;
CREATE TABLE `server_time` (
  `server_time` datetime NOT NULL,
  PRIMARY KEY (`server_time`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of server_time
-- ----------------------------
INSERT INTO `server_time` VALUES ('2010-01-15 21:27:28');

-- ----------------------------
-- Table structure for `shop`
-- ----------------------------
DROP TABLE IF EXISTS `shop`;
CREATE TABLE `shop` (
  `id` smallint(5) unsigned NOT NULL,
  `name` varchar(60) NOT NULL,
  `can_buy` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of shop
-- ----------------------------
INSERT INTO `shop` VALUES ('0', 'Test Shop', '1');
INSERT INTO `shop` VALUES ('1', 'Soda Vending Machine', '0');

-- ----------------------------
-- Table structure for `shop_item`
-- ----------------------------
DROP TABLE IF EXISTS `shop_item`;
CREATE TABLE `shop_item` (
  `shop_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`shop_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `shop_item_ibfk_1` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `shop_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of shop_item
-- ----------------------------
INSERT INTO `shop_item` VALUES ('0', '1');
INSERT INTO `shop_item` VALUES ('1', '1');
INSERT INTO `shop_item` VALUES ('0', '2');
INSERT INTO `shop_item` VALUES ('1', '2');
INSERT INTO `shop_item` VALUES ('0', '3');
INSERT INTO `shop_item` VALUES ('0', '4');
INSERT INTO `shop_item` VALUES ('0', '5');

-- ----------------------------
-- View structure for `npc_character`
-- ----------------------------
DROP VIEW IF EXISTS `npc_character`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `npc_character` AS select `character`.`id` AS `id`,`character`.`account_id` AS `account_id`,`character`.`character_template_id` AS `character_template_id`,`character`.`name` AS `name`,`character`.`map_id` AS `map_id`,`character`.`shop_id` AS `shop_id`,`character`.`chat_dialog` AS `chat_dialog`,`character`.`ai_id` AS `ai_id`,`character`.`x` AS `x`,`character`.`y` AS `y`,`character`.`respawn_map` AS `respawn_map`,`character`.`respawn_x` AS `respawn_x`,`character`.`respawn_y` AS `respawn_y`,`character`.`body_id` AS `body_id`,`character`.`cash` AS `cash`,`character`.`level` AS `level`,`character`.`exp` AS `exp`,`character`.`statpoints` AS `statpoints`,`character`.`hp` AS `hp`,`character`.`mp` AS `mp`,`character`.`stat_maxhp` AS `stat_maxhp`,`character`.`stat_maxmp` AS `stat_maxmp`,`character`.`stat_minhit` AS `stat_minhit`,`character`.`stat_maxhit` AS `stat_maxhit`,`character`.`stat_defence` AS `stat_defence`,`character`.`stat_agi` AS `stat_agi`,`character`.`stat_int` AS `stat_int`,`character`.`stat_str` AS `stat_str` from `character` where isnull(`character`.`account_id`);

-- ----------------------------
-- View structure for `user_character`
-- ----------------------------
DROP VIEW IF EXISTS `user_character`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `user_character` AS select `character`.`id` AS `id`,`character`.`account_id` AS `account_id`,`character`.`character_template_id` AS `character_template_id`,`character`.`name` AS `name`,`character`.`map_id` AS `map_id`,`character`.`shop_id` AS `shop_id`,`character`.`chat_dialog` AS `chat_dialog`,`character`.`ai_id` AS `ai_id`,`character`.`x` AS `x`,`character`.`y` AS `y`,`character`.`respawn_map` AS `respawn_map`,`character`.`respawn_x` AS `respawn_x`,`character`.`respawn_y` AS `respawn_y`,`character`.`body_id` AS `body_id`,`character`.`cash` AS `cash`,`character`.`level` AS `level`,`character`.`exp` AS `exp`,`character`.`statpoints` AS `statpoints`,`character`.`hp` AS `hp`,`character`.`mp` AS `mp`,`character`.`stat_maxhp` AS `stat_maxhp`,`character`.`stat_maxmp` AS `stat_maxmp`,`character`.`stat_minhit` AS `stat_minhit`,`character`.`stat_maxhit` AS `stat_maxhit`,`character`.`stat_defence` AS `stat_defence`,`character`.`stat_agi` AS `stat_agi`,`character`.`stat_int` AS `stat_int`,`character`.`stat_str` AS `stat_str` from `character` where (`character`.`account_id` is not null);

-- ----------------------------
-- Procedure structure for `Rebuild_View_NPC_Character`
-- ----------------------------
DROP PROCEDURE IF EXISTS `Rebuild_View_NPC_Character`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `Rebuild_View_NPC_Character`()
BEGIN
	
	DROP VIEW IF EXISTS `npc_character`;
	CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `npc_character` AS SELECT *FROM `character` WHERE `account_id` IS NULL;
    
END;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `Rebuild_Views`
-- ----------------------------
DROP PROCEDURE IF EXISTS `Rebuild_Views`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `Rebuild_Views`()
BEGIN
	
	CALL Rebuild_View_NPC_Character();
	CALL Rebuild_View_User_Character();
    
END;;
DELIMITER ;

-- ----------------------------
-- Function structure for `CreateUserOnAccount`
-- ----------------------------
DROP FUNCTION IF EXISTS `CreateUserOnAccount`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `CreateUserOnAccount`(accountID INT, characterName VARCHAR(30), characterID INT) RETURNS varchar(100) CHARSET latin1
BEGIN
		
		DECLARE character_count INT DEFAULT 0;
		DECLARE max_character_count INT DEFAULT 3;
		DECLARE is_id_free INT DEFAULT 0;
		DECLARE is_name_free INT DEFAULT 0;
		DECLARE errorMsg VARCHAR(100) DEFAULT "";

		SELECT COUNT(*) 
			INTO character_count
			FROM `character`
			WHERE account_id = accountID;
					
			SELECT `max_characters_per_account` 
			INTO max_character_count
			FROM `game_data`;
					
			IF character_count > max_character_count THEN
				SET errorMsg = "No free character slots available in the account.";
			ELSE
				SELECT COUNT(*)
				INTO is_id_free
				FROM `character`
				WHERE `id` = characterID;
				
				IF is_id_free > 0 THEN
					SET errorMsg = "The specified CharacterID is not available for use.";
				ELSE
					SELECT COUNT(*)
					INTO is_name_free
					FROM `user_character`
					WHERE `name` = characterName;
						
					IF is_name_free > 0 THEN
						SET errorMsg = "The specified character name is not available for use.";
					ELSE
						INSERT INTO `character` SET 
						`id`			= 	characterID,
						`name`			= 	characterName,
						`account_id`	= 	accountID;
					END IF;
				END IF;
			END IF;
				
		RETURN errorMsg;
    
END;;
DELIMITER ;
