/*
MySQL Data Transfer
Source Host: localhost
Source Database: demogame
Target Host: localhost
Target Database: demogame
Date: 7/4/2009 4:14:35 PM
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
-- Table structure for character_equipped
-- ----------------------------
CREATE TABLE `character_equipped` (
  `character_guid` int(10) unsigned NOT NULL,
  `item_guid` int(10) unsigned NOT NULL,
  `slot` tinyint(3) unsigned NOT NULL,
  KEY `character_guid` (`character_guid`),
  KEY `item_guid` (`item_guid`),
  CONSTRAINT `character_equipped_ibfk_2` FOREIGN KEY (`item_guid`) REFERENCES `items` (`guid`),
  CONSTRAINT `character_equipped_ibfk_1` FOREIGN KEY (`character_guid`) REFERENCES `characters` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for character_inventory
-- ----------------------------
CREATE TABLE `character_inventory` (
  `character_guid` int(10) unsigned NOT NULL,
  `item_guid` int(10) unsigned NOT NULL,
  KEY `character_guid` (`character_guid`),
  KEY `item_guid` (`item_guid`),
  CONSTRAINT `character_inventory_ibfk_2` FOREIGN KEY (`item_guid`) REFERENCES `items` (`guid`),
  CONSTRAINT `character_inventory_ibfk_1` FOREIGN KEY (`character_guid`) REFERENCES `characters` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for characters
-- ----------------------------
CREATE TABLE `characters` (
  `guid` int(10) unsigned NOT NULL,
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
-- Table structure for npc_templates
-- ----------------------------
CREATE TABLE `npc_templates` (
  `guid` smallint(5) unsigned NOT NULL,
  `alliance` varchar(255) NOT NULL,
  `name` varchar(50) NOT NULL DEFAULT 'New NPC',
  `ai` varchar(255) NOT NULL,
  `body` smallint(5) unsigned NOT NULL DEFAULT '1',
  `respawn` smallint(5) unsigned NOT NULL DEFAULT '5',
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
-- Table structure for npc_templates_drops
-- ----------------------------
CREATE TABLE `npc_templates_drops` (
  `npc_guid` smallint(5) unsigned NOT NULL,
  `item_guid` smallint(5) unsigned NOT NULL,
  `min` tinyint(3) unsigned NOT NULL,
  `max` tinyint(3) unsigned NOT NULL,
  `chance` smallint(5) unsigned NOT NULL,
  KEY `npc_guid` (`npc_guid`),
  KEY `item_guid` (`item_guid`),
  CONSTRAINT `npc_templates_drops_ibfk_1` FOREIGN KEY (`npc_guid`) REFERENCES `npc_templates` (`guid`),
  CONSTRAINT `npc_templates_drops_ibfk_2` FOREIGN KEY (`item_guid`) REFERENCES `item_templates` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `alliances` VALUES ('monster', 'user', 'user');
INSERT INTO `alliances` VALUES ('user', 'monster', 'monster');
INSERT INTO `character_inventory` VALUES ('1', '60');
INSERT INTO `characters` VALUES ('1', 'Spodi', 'asdf', '2', '394', '338', '1', '3012', '12', '810', '527', '5', '50', '75', '50', '8', '7', '2', '1', '1', '4', '1', '2', '1', '1', '1', '1', '1', '1');
INSERT INTO `characters` VALUES ('2', 'Test A', '', '2', '930', '530', '1', '3012', '12', '810', '527', '50', '50', '50', '50', '8', '7', '2', '1', '1', '4', '1', '2', '1', '1', '1', '1', '1', '1');
INSERT INTO `characters` VALUES ('3', 'Test B', '', '2', '908.4', '479.96', '1', '3012', '12', '810', '527', '50', '50', '50', '50', '8', '7', '2', '1', '1', '4', '1', '2', '1', '1', '1', '1', '1', '1');
INSERT INTO `item_templates` VALUES ('1', '1', '9', '16', 'Healing Potion', 'A healing potion', '127', '10', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_templates` VALUES ('2', '1', '9', '16', 'Mana Potion', 'A mana potion', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_templates` VALUES ('3', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_templates` VALUES ('4', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_templates` VALUES ('5', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('0', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('1', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('2', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('3', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('4', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('5', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('6', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('7', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('8', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('9', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('10', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('11', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('12', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('13', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('14', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('15', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('16', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('17', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('18', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('19', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('20', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('21', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('22', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('23', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('24', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('25', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('26', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('27', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('28', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('29', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('30', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('31', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('32', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('33', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('34', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('35', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('36', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('37', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('38', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('39', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('40', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('41', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('42', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('43', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('44', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('45', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('46', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('47', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('48', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('49', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('60', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('225', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('845', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('875', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '44', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('1646', '1', '9', '16', 'Mana Potion', 'A mana potion', '32', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('1771', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '49', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('1793', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `items` VALUES ('6655', '1', '9', '16', 'Healing Potion', 'A healing potion', '68', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `npc_templates` VALUES ('1', 'monster', 'A Test NPC', 'TestAI', '1', '2', '1', '1', '5', '5', '1', '1', '1', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `npc_templates_drops` VALUES ('1', '1', '0', '2', '10');
INSERT INTO `npc_templates_drops` VALUES ('1', '2', '2', '5', '25');
INSERT INTO `npc_templates_drops` VALUES ('1', '3', '1', '1', '50');
INSERT INTO `npc_templates_drops` VALUES ('1', '4', '1', '1', '50');
INSERT INTO `npc_templates_drops` VALUES ('1', '5', '1', '1', '50');
