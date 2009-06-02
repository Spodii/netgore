/*
MySQL Data Transfer
Source Host: localhost
Source Database: demogame
Target Host: localhost
Target Database: demogame
Date: 6/1/2009 5:03:54 PM
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for alliances
-- ----------------------------
CREATE TABLE `alliances` (
  `name` varchar(255) NOT NULL,
  `hostile` text NOT NULL,
  `attackable` text NOT NULL,
  PRIMARY KEY (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for item_templates
-- ----------------------------
CREATE TABLE `item_templates` (
  `guid` smallint(5) unsigned NOT NULL,
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `width` tinyint(3) unsigned NOT NULL,
  `height` tinyint(3) unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  `graphic` smallint(5) unsigned NOT NULL,
  `value` int(11) NOT NULL DEFAULT '0',
  `agi` smallint(5) unsigned NOT NULL DEFAULT '0',
  `armor` smallint(5) unsigned NOT NULL DEFAULT '0',
  `bra` smallint(5) unsigned NOT NULL DEFAULT '0',
  `defence` smallint(5) unsigned NOT NULL DEFAULT '0',
  `dex` smallint(5) unsigned NOT NULL DEFAULT '0',
  `evade` smallint(5) unsigned NOT NULL DEFAULT '0',
  `imm` smallint(5) unsigned NOT NULL DEFAULT '0',
  `int` smallint(5) unsigned NOT NULL DEFAULT '0',
  `hp` smallint(6) NOT NULL DEFAULT '0',
  `mp` smallint(6) NOT NULL DEFAULT '0',
  `minhit` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxhit` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxhp` smallint(6) NOT NULL DEFAULT '0',
  `maxmp` smallint(6) NOT NULL DEFAULT '0',
  `perc` smallint(5) unsigned NOT NULL DEFAULT '0',
  `reqacc` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqagi` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqarmor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqbra` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqdex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqevade` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqimm` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqint` tinyint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for items
-- ----------------------------
CREATE TABLE `items` (
  `guid` int(10) unsigned NOT NULL,
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `width` tinyint(3) unsigned NOT NULL DEFAULT '16',
  `height` tinyint(3) unsigned NOT NULL DEFAULT '16',
  `name` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  `amount` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `graphic` smallint(5) unsigned NOT NULL DEFAULT '0',
  `value` int(11) NOT NULL DEFAULT '0',
  `agi` smallint(5) unsigned NOT NULL DEFAULT '0',
  `armor` smallint(5) unsigned NOT NULL DEFAULT '0',
  `bra` smallint(5) unsigned NOT NULL DEFAULT '0',
  `defence` smallint(5) unsigned NOT NULL DEFAULT '0',
  `dex` smallint(5) unsigned NOT NULL DEFAULT '0',
  `evade` smallint(5) unsigned NOT NULL DEFAULT '0',
  `hp` smallint(6) NOT NULL DEFAULT '0',
  `imm` smallint(5) unsigned NOT NULL DEFAULT '0',
  `int` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxhit` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxhp` smallint(6) NOT NULL DEFAULT '0',
  `maxmp` smallint(6) NOT NULL DEFAULT '0',
  `minhit` smallint(5) unsigned NOT NULL DEFAULT '0',
  `mp` smallint(6) NOT NULL DEFAULT '0',
  `perc` smallint(5) unsigned NOT NULL DEFAULT '0',
  `reqacc` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqagi` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqarmor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqbra` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqdex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqevade` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqimm` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `reqint` tinyint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for npc_drops
-- ----------------------------
CREATE TABLE `npc_drops` (
  `guid` smallint(5) unsigned NOT NULL,
  `item_guid` smallint(5) unsigned NOT NULL,
  `min` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `max` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `chance` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`guid`),
  KEY `fk_npc_drops_item_guid` (`item_guid`),
  CONSTRAINT `fk_npc_drops_item_guid` FOREIGN KEY (`item_guid`) REFERENCES `item_templates` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for npc_templates
-- ----------------------------
CREATE TABLE `npc_templates` (
  `guid` smallint(5) unsigned NOT NULL,
  `alliance` varchar(255) NOT NULL,
  `name` varchar(50) NOT NULL DEFAULT 'New NPC',
  `ai` varchar(255) NOT NULL,
  `body` smallint(5) unsigned NOT NULL DEFAULT '1',
  `respawn` smallint(5) unsigned NOT NULL DEFAULT '5',
  `drops` varchar(255) NOT NULL,
  `give_exp` smallint(5) unsigned NOT NULL DEFAULT '0',
  `give_cash` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxhp` smallint(6) NOT NULL DEFAULT '50',
  `maxmp` smallint(6) NOT NULL DEFAULT '50',
  `str` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `agi` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `dex` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `int` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `bra` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `acc` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `armor` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `evade` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `imm` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `perc` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `recov` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `regen` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `tact` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `ws` tinyint(3) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`guid`),
  KEY `fk_npc_templates_alliance` (`alliance`),
  CONSTRAINT `fk_npc_templates_alliance` FOREIGN KEY (`alliance`) REFERENCES `alliances` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for user_equipped
-- ----------------------------
CREATE TABLE `user_equipped` (
  `item_guid` int(10) unsigned NOT NULL,
  `user_guid` mediumint(8) unsigned NOT NULL,
  `slot` tinyint(3) unsigned NOT NULL,
  KEY `fk_user_equipped_user_guid` (`user_guid`),
  KEY `fk_user_equipped_item_guid` (`item_guid`),
  CONSTRAINT `fk_user_equipped_user_guid` FOREIGN KEY (`user_guid`) REFERENCES `users` (`guid`),
  CONSTRAINT `fk_user_equipped_item_guid` FOREIGN KEY (`item_guid`) REFERENCES `items` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for user_inventory
-- ----------------------------
CREATE TABLE `user_inventory` (
  `item_guid` int(10) unsigned NOT NULL,
  `user_guid` mediumint(8) unsigned NOT NULL,
  PRIMARY KEY (`item_guid`,`user_guid`),
  KEY `fk_user_inventory_user_guid` (`user_guid`),
  CONSTRAINT `fk_user_inventory_item_guid` FOREIGN KEY (`item_guid`) REFERENCES `items` (`guid`),
  CONSTRAINT `fk_user_inventory_user_guid` FOREIGN KEY (`user_guid`) REFERENCES `users` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for users
-- ----------------------------
CREATE TABLE `users` (
  `guid` mediumint(8) unsigned NOT NULL,
  `name` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `map` smallint(5) unsigned NOT NULL DEFAULT '1',
  `x` float NOT NULL DEFAULT '10',
  `y` float NOT NULL DEFAULT '10',
  `body` smallint(5) unsigned NOT NULL DEFAULT '0',
  `cash` int(11) NOT NULL DEFAULT '0',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `exp` int(11) NOT NULL DEFAULT '0',
  `expspent` int(11) NOT NULL DEFAULT '0',
  `hp` smallint(6) NOT NULL DEFAULT '50',
  `maxhp` smallint(6) NOT NULL DEFAULT '50',
  `mp` smallint(6) NOT NULL DEFAULT '50',
  `maxmp` smallint(6) NOT NULL DEFAULT '50',
  `str` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `agi` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `dex` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `int` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `bra` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `ws` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `armor` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `acc` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `evade` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `perc` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `regen` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `recov` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `tact` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `imm` tinyint(3) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`guid`),
  UNIQUE KEY `idx_name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `alliances` VALUES ('monster', 'user', 'user');
INSERT INTO `alliances` VALUES ('user', 'monster', 'monster');
INSERT INTO `item_templates` VALUES ('1', '1', '9', '16', 'Healing Potion', 'A healing potion', '127', '10', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_templates` VALUES ('2', '1', '9', '16', 'Mana Potion', 'A mana potion', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_templates` VALUES ('3', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_templates` VALUES ('4', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_templates` VALUES ('5', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('0', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('1', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '3', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('2', '1', '9', '16', 'Healing Potion', 'A healing potion', '12', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('3', '1', '9', '16', 'Mana Potion', 'A mana potion', '20', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('4', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('5', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('6', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '0', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('7', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('8', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('9', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('10', '1', '9', '16', 'Mana Potion', 'A mana potion', '4', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('11', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('12', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '0', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('13', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('14', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('15', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('16', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('17', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('18', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('19', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('20', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('21', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('22', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('23', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('24', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('25', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('26', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('27', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('28', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('29', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '0', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('30', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('31', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('32', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('33', '1', '9', '16', 'Mana Potion', 'A mana potion', '5', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('34', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('35', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('36', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('37', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('38', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('39', '1', '9', '16', 'Mana Potion', 'A mana potion', '4', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('40', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('41', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('42', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('43', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('44', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('45', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('46', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('47', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('48', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('49', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('50', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('51', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('52', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '0', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('53', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('54', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('55', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '0', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('56', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '0', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('57', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '0', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('58', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '0', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('59', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '0', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('60', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '0', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('61', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '0', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('62', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('63', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('64', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('65', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('66', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('67', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('68', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('69', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('70', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('71', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('72', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('73', '1', '9', '16', 'Mana Potion', 'A mana potion', '5', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('74', '1', '9', '16', 'Mana Potion', 'A mana potion', '4', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('75', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('76', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('77', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('78', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('79', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('80', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('81', '1', '9', '16', 'Mana Potion', 'A mana potion', '5', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('82', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('83', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('84', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('85', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('86', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('87', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('88', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('89', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('90', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('91', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('92', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('93', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('94', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('95', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('96', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('97', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('98', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('99', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('100', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('101', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('102', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('103', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('104', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('105', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('106', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('107', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('108', '1', '9', '16', 'Mana Potion', 'A mana potion', '4', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('109', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('110', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('111', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('112', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('113', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('114', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('115', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('116', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('117', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('118', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('119', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('120', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('121', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('122', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('123', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('124', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('125', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('126', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('127', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('128', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('129', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('130', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('131', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('132', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('133', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('134', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('135', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('136', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('137', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('138', '1', '9', '16', 'Healing Potion', 'A healing potion', '28', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('139', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('140', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('141', '1', '9', '16', 'Mana Potion', 'A mana potion', '4', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('142', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('143', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('144', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('145', '1', '9', '16', 'Mana Potion', 'A mana potion', '16', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('146', '1', '9', '16', 'Mana Potion', 'A mana potion', '31', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('147', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('148', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('149', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('150', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '2', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('151', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('152', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('153', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('154', '1', '9', '16', 'Mana Potion', 'A mana potion', '5', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('155', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('156', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('157', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('158', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('159', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('160', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('161', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('162', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('163', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('164', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('166', '1', '9', '16', 'Mana Potion', 'A mana potion', '2', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('167', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('168', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('169', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('170', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('171', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('172', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('173', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('174', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('175', '1', '9', '16', 'Mana Potion', 'A mana potion', '4', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('176', '1', '9', '16', 'Mana Potion', 'A mana potion', '5', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('177', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('178', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('179', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('180', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '5', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('181', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('182', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('183', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('184', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('185', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('186', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('187', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('188', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('189', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('190', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('191', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('192', '1', '9', '16', 'Mana Potion', 'A mana potion', '4', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('193', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('194', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('195', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('196', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('197', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('198', '1', '9', '16', 'Mana Potion', 'A mana potion', '5', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('199', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('200', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('201', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('202', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('203', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('204', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('205', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('206', '1', '9', '16', 'Mana Potion', 'A mana potion', '4', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('207', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('208', '1', '9', '16', 'Mana Potion', 'A mana potion', '5', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('209', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('210', '1', '9', '16', 'Mana Potion', 'A mana potion', '4', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('211', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('212', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('213', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('214', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('215', '1', '9', '16', 'Mana Potion', 'A mana potion', '3', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('216', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('217', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('218', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('219', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('220', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('221', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('222', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('223', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('224', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '4', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('226', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('227', '1', '9', '16', 'Mana Potion', 'A mana potion', '5', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('228', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('230', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('231', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `npc_drops` VALUES ('1', '2', '2', '5', '25');
INSERT INTO `npc_drops` VALUES ('2', '1', '0', '2', '10');
INSERT INTO `npc_drops` VALUES ('3', '3', '1', '1', '50');
INSERT INTO `npc_drops` VALUES ('4', '4', '1', '1', '50');
INSERT INTO `npc_drops` VALUES ('5', '5', '1', '1', '50');
INSERT INTO `npc_templates` VALUES ('1', 'monster', 'A Test NPC', 'TestAI', '1', '2', '1,2,3,4,5', '1', '1', '5', '5', '1', '1', '1', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `user_equipped` VALUES ('54', '1', '0');
INSERT INTO `user_equipped` VALUES ('62', '1', '1');
INSERT INTO `user_inventory` VALUES ('138', '1');
INSERT INTO `user_inventory` VALUES ('146', '1');
INSERT INTO `user_inventory` VALUES ('150', '1');
INSERT INTO `user_inventory` VALUES ('180', '1');
INSERT INTO `user_inventory` VALUES ('224', '1');
INSERT INTO `users` VALUES ('1', 'Spodi', 'asdf', '2', '634.3', '466', '1', '2940', '12', '738', '527', '-3997', '50', '75', '50', '8', '7', '2', '1', '1', '4', '1', '2', '1', '1', '1', '1', '1', '1');
