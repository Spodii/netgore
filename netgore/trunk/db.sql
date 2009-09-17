/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50138
Source Host           : localhost:3306
Source Database       : demogame

Target Server Type    : MYSQL
Target Server Version : 50138
File Encoding         : 65001

Date: 2009-09-17 11:29:22
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `account`
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `id` int(11) NOT NULL COMMENT 'The account ID.',
  `name` varchar(30) NOT NULL COMMENT 'The account name.',
  `password` varchar(30) NOT NULL COMMENT 'The account password.',
  `email` varchar(60) NOT NULL COMMENT 'The email address.',
  `time_created` datetime NOT NULL COMMENT 'The DateTime of when the account was created.',
  `time_last_login` datetime NOT NULL COMMENT 'The DateTime that the account was last logged in to.',
  `current_ip` int(10) unsigned DEFAULT NULL COMMENT 'IP address currently logged in to the account, or null if nobody is logged in.',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of account
-- ----------------------------
INSERT INTO `account` VALUES ('1', 'Spodi', 'qwerty123', 'spodi@vbgore.com', '2009-09-07 15:43:16', '2009-09-16 18:30:36', null);

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
  `chat_dialog` smallint(5) unsigned DEFAULT NULL,
  `x` float NOT NULL DEFAULT '100',
  `y` float NOT NULL DEFAULT '100',
  `respawn_map` smallint(5) unsigned DEFAULT NULL,
  `respawn_x` float NOT NULL DEFAULT '50',
  `respawn_y` float NOT NULL DEFAULT '50',
  `body_id` smallint(5) unsigned NOT NULL DEFAULT '1',
  `cash` int(10) unsigned NOT NULL DEFAULT '0',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `exp` int(10) unsigned NOT NULL DEFAULT '0',
  `statpoints` int(10) unsigned NOT NULL DEFAULT '0',
  `hp` smallint(6) NOT NULL DEFAULT '50',
  `mp` smallint(6) NOT NULL DEFAULT '50',
  `maxhp` smallint(6) NOT NULL DEFAULT '50',
  `maxmp` smallint(6) NOT NULL DEFAULT '50',
  `minhit` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `maxhit` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `acc` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `agi` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `armor` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `bra` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `defence` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `dex` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `evade` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `imm` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `int` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `perc` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `recov` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `regen` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `str` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `tact` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `ws` tinyint(3) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `template_id` (`character_template_id`),
  KEY `respawn_map` (`respawn_map`),
  KEY `character_ibfk_2` (`map_id`),
  KEY `idx_name` (`name`) USING BTREE,
  KEY `account_id` (`account_id`),
  CONSTRAINT `character_ibfk_2` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_3` FOREIGN KEY (`respawn_map`) REFERENCES `map` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_4` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_5` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character
-- ----------------------------
INSERT INTO `character` VALUES ('1', '1', null, 'Spodi', '2', null, '398', '338', '1', '500', '200', '1', '121', '21', '629', '84', '50', '50', '50', '50', '5', '11', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '2', '1', '1');
INSERT INTO `character` VALUES ('2', null, '1', 'Test A', '2', null, '800', '250', '2', '800', '250', '1', '3012', '12', '810', '527', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5');
INSERT INTO `character` VALUES ('3', null, '1', 'Test B', '2', null, '500', '250', '2', '500', '250', '1', '3012', '12', '810', '527', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5');
INSERT INTO `character` VALUES ('4', null, null, 'Talking Guy', '2', '0', '800', '500', '2', '800', '500', '1', '0', '1', '0', '0', '50', '50', '50', '50', '1', '1', '1', '1', '1', '1', '0', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1');

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
  `alliance_id` tinyint(3) unsigned NOT NULL,
  `name` varchar(50) NOT NULL DEFAULT 'New NPC',
  `ai` varchar(255) DEFAULT NULL,
  `body_id` smallint(5) unsigned NOT NULL DEFAULT '1',
  `respawn` smallint(5) unsigned NOT NULL DEFAULT '5',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `exp` int(10) unsigned NOT NULL,
  `statpoints` int(10) unsigned NOT NULL,
  `give_exp` smallint(5) unsigned NOT NULL DEFAULT '0',
  `give_cash` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxhp` smallint(5) unsigned NOT NULL DEFAULT '50',
  `maxmp` smallint(5) unsigned NOT NULL DEFAULT '50',
  `defence` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `minhit` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `maxhit` tinyint(3) unsigned NOT NULL DEFAULT '2',
  `acc` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `agi` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `armor` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `bra` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `dex` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `evade` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `imm` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `int` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `perc` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `recov` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `regen` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `str` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `tact` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `ws` tinyint(3) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `character_template_ibfk_2` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of character_template
-- ----------------------------
INSERT INTO `character_template` VALUES ('0', '0', 'User Template', null, '1', '5', '1', '0', '0', '0', '0', '50', '50', '0', '1', '2', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1');
INSERT INTO `character_template` VALUES ('1', '1', 'A Test NPC', 'TestAI', '1', '2', '0', '0', '0', '5', '5', '5', '5', '0', '0', '0', '0', '1', '0', '1', '1', '0', '0', '1', '0', '0', '0', '1', '0', '0');

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
INSERT INTO `character_template_equipped` VALUES ('0', '1', '5', '5000');
INSERT INTO `character_template_equipped` VALUES ('1', '1', '4', '5000');
INSERT INTO `character_template_equipped` VALUES ('2', '1', '3', '10000');

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
-- Table structure for `game_data`
-- ----------------------------
DROP TABLE IF EXISTS `game_data`;
CREATE TABLE `game_data` (
  `max_characters_per_account` tinyint(3) unsigned NOT NULL,
  `min_account_name_length` tinyint(3) unsigned NOT NULL,
  `max_account_name_length` tinyint(3) unsigned NOT NULL,
  `min_account_password_length` tinyint(3) unsigned NOT NULL,
  `max_account_password_length` tinyint(3) unsigned NOT NULL,
  `min_character_name_length` tinyint(3) unsigned NOT NULL,
  `max_character_name_length` tinyint(3) unsigned NOT NULL,
  `max_status_effect_power` smallint(5) unsigned NOT NULL,
  `screen_width` smallint(5) unsigned NOT NULL,
  `screen_height` smallint(5) unsigned NOT NULL,
  `server_ping_port` smallint(5) unsigned NOT NULL,
  `server_tcp_port` smallint(5) unsigned NOT NULL,
  `server_ip` varchar(15) NOT NULL,
  `world_physics_update_rate` smallint(5) unsigned NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- ----------------------------
-- Records of game_data
-- ----------------------------
INSERT INTO `game_data` VALUES ('10', '3', '30', '3', '30', '3', '15', '500', '800', '600', '44446', '44445', '127.0.0.1', '20');

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
  `agi` smallint(6) NOT NULL DEFAULT '0',
  `armor` smallint(5) unsigned NOT NULL DEFAULT '0',
  `bra` smallint(6) NOT NULL DEFAULT '0',
  `defence` smallint(6) NOT NULL DEFAULT '0',
  `dex` smallint(6) NOT NULL DEFAULT '0',
  `evade` smallint(6) NOT NULL DEFAULT '0',
  `hp` smallint(6) NOT NULL DEFAULT '0',
  `imm` smallint(6) NOT NULL DEFAULT '0',
  `int` smallint(6) NOT NULL DEFAULT '0',
  `maxhit` smallint(6) NOT NULL DEFAULT '0',
  `maxhp` smallint(6) NOT NULL DEFAULT '0',
  `maxmp` smallint(6) NOT NULL DEFAULT '0',
  `minhit` smallint(6) NOT NULL DEFAULT '0',
  `mp` smallint(6) NOT NULL DEFAULT '0',
  `perc` smallint(6) NOT NULL DEFAULT '0',
  `reqacc` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqagi` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqarmor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqbra` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqdex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqevade` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqimm` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqint` tinyint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of item
-- ----------------------------

-- ----------------------------
-- Table structure for `item_template`
-- ----------------------------
DROP TABLE IF EXISTS `item_template`;
CREATE TABLE `item_template` (
  `id` smallint(5) unsigned NOT NULL,
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `width` tinyint(3) unsigned NOT NULL,
  `height` tinyint(3) unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  `graphic` smallint(5) unsigned NOT NULL,
  `value` int(11) NOT NULL DEFAULT '0',
  `agi` smallint(6) NOT NULL DEFAULT '0',
  `armor` smallint(6) NOT NULL DEFAULT '0',
  `bra` smallint(6) NOT NULL DEFAULT '0',
  `defence` smallint(6) NOT NULL DEFAULT '0',
  `dex` smallint(6) NOT NULL DEFAULT '0',
  `evade` smallint(6) NOT NULL DEFAULT '0',
  `imm` smallint(6) NOT NULL DEFAULT '0',
  `int` smallint(6) NOT NULL DEFAULT '0',
  `hp` smallint(6) NOT NULL DEFAULT '0',
  `mp` smallint(6) NOT NULL DEFAULT '0',
  `minhit` smallint(6) NOT NULL DEFAULT '0',
  `maxhit` smallint(6) NOT NULL DEFAULT '0',
  `maxhp` smallint(6) NOT NULL DEFAULT '0',
  `maxmp` smallint(6) NOT NULL DEFAULT '0',
  `perc` smallint(6) NOT NULL DEFAULT '0',
  `reqacc` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqagi` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqarmor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqbra` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqdex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqevade` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqimm` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqint` tinyint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of item_template
-- ----------------------------
INSERT INTO `item_template` VALUES ('1', '1', '9', '16', 'Healing Potion', 'A healing potion', '127', '10', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('2', '1', '9', '16', 'Mana Potion', 'A mana potion', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('3', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('4', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('5', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');

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
INSERT INTO `map` VALUES ('1', '');
INSERT INTO `map` VALUES ('2', '');

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
INSERT INTO `map_spawn` VALUES ('12', '1', '1', '1', null, null, null, null);
INSERT INTO `map_spawn` VALUES ('13', '1', '1', '1', null, null, null, null);
INSERT INTO `map_spawn` VALUES ('14', '1', '1', '1', null, null, null, null);

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
INSERT INTO `server_time` VALUES ('2009-09-17 11:28:37');

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
-- View structure for `user_character`
-- ----------------------------
DROP VIEW IF EXISTS `user_character`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `user_character` AS select `character`.`id` AS `id`,`character`.`account_id` AS `account_id`,`character`.`character_template_id` AS `character_template_id`,`character`.`name` AS `name`,`character`.`map_id` AS `map_id`,`character`.`chat_dialog` AS `chat_dialog`,`character`.`x` AS `x`,`character`.`y` AS `y`,`character`.`respawn_map` AS `respawn_map`,`character`.`respawn_x` AS `respawn_x`,`character`.`respawn_y` AS `respawn_y`,`character`.`body_id` AS `body_id`,`character`.`cash` AS `cash`,`character`.`level` AS `level`,`character`.`exp` AS `exp`,`character`.`statpoints` AS `statpoints`,`character`.`hp` AS `hp`,`character`.`mp` AS `mp`,`character`.`maxhp` AS `maxhp`,`character`.`maxmp` AS `maxmp`,`character`.`minhit` AS `minhit`,`character`.`maxhit` AS `maxhit`,`character`.`acc` AS `acc`,`character`.`agi` AS `agi`,`character`.`armor` AS `armor`,`character`.`bra` AS `bra`,`character`.`defence` AS `defence`,`character`.`dex` AS `dex`,`character`.`evade` AS `evade`,`character`.`imm` AS `imm`,`character`.`int` AS `int`,`character`.`perc` AS `perc`,`character`.`recov` AS `recov`,`character`.`regen` AS `regen`,`character`.`str` AS `str`,`character`.`tact` AS `tact`,`character`.`ws` AS `ws` from `character` where (`character`.`account_id` is not null);

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
