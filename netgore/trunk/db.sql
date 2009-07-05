/*
MySQL Data Transfer
Source Host: localhost
Source Database: demogame
Target Host: localhost
Target Database: demogame
Date: 7/5/2009 2:09:54 PM
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for alliance
-- ----------------------------
CREATE TABLE `alliance` (
  `id` tinyint(3) unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for alliance_attackable
-- ----------------------------
CREATE TABLE `alliance_attackable` (
  `alliance_id` tinyint(3) unsigned NOT NULL,
  `attackable_id` tinyint(3) unsigned NOT NULL,
  KEY `alliance_id` (`alliance_id`),
  KEY `attackable_id` (`attackable_id`),
  CONSTRAINT `alliance_attackable_ibfk_2` FOREIGN KEY (`attackable_id`) REFERENCES `alliance` (`id`),
  CONSTRAINT `alliance_attackable_ibfk_1` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for alliance_hostile
-- ----------------------------
CREATE TABLE `alliance_hostile` (
  `alliance_id` tinyint(3) unsigned NOT NULL,
  `hostile_id` tinyint(3) unsigned NOT NULL,
  KEY `alliance_id` (`alliance_id`),
  KEY `hostile_id` (`hostile_id`),
  CONSTRAINT `alliance_hostile_ibfk_2` FOREIGN KEY (`hostile_id`) REFERENCES `alliance` (`id`),
  CONSTRAINT `alliance_hostile_ibfk_1` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for character
-- ----------------------------
CREATE TABLE `character` (
  `id` int(10) unsigned NOT NULL,
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
  PRIMARY KEY (`id`),
  UNIQUE KEY `idx_name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for character_equipped
-- ----------------------------
CREATE TABLE `character_equipped` (
  `character_id` int(10) unsigned NOT NULL,
  `item_id` int(10) unsigned NOT NULL,
  `slot` tinyint(3) unsigned NOT NULL,
  KEY `character_id` (`character_id`),
  KEY `item_id` (`item_id`),
  CONSTRAINT `character_equipped_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`),
  CONSTRAINT `character_equipped_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for character_inventory
-- ----------------------------
CREATE TABLE `character_inventory` (
  `character_id` int(10) unsigned NOT NULL,
  `item_id` int(10) unsigned NOT NULL,
  KEY `character_id` (`character_id`),
  KEY `item_id` (`item_id`),
  CONSTRAINT `character_inventory_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`),
  CONSTRAINT `character_inventory_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for character_template
-- ----------------------------
CREATE TABLE `character_template` (
  `id` smallint(5) unsigned NOT NULL,
  `alliance_id` tinyint(3) unsigned NOT NULL,
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
  PRIMARY KEY (`id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `character_template_ibfk_1` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for character_template_drops
-- ----------------------------
CREATE TABLE `character_template_drops` (
  `character_id` smallint(5) unsigned NOT NULL,
  `item_id` smallint(5) unsigned NOT NULL,
  `min` tinyint(3) unsigned NOT NULL,
  `max` tinyint(3) unsigned NOT NULL,
  `chance` smallint(5) unsigned NOT NULL,
  KEY `character_id` (`character_id`),
  KEY `item_id` (`item_id`),
  CONSTRAINT `character_template_drops_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `item_template` (`id`),
  CONSTRAINT `character_template_drops_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character_template` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for item
-- ----------------------------
CREATE TABLE `item` (
  `id` int(10) unsigned NOT NULL,
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for item_template
-- ----------------------------
CREATE TABLE `item_template` (
  `id` smallint(5) unsigned NOT NULL,
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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `alliance` VALUES ('0', 'user');
INSERT INTO `alliance` VALUES ('1', 'monster');
INSERT INTO `alliance_attackable` VALUES ('0', '1');
INSERT INTO `alliance_attackable` VALUES ('1', '0');
INSERT INTO `alliance_hostile` VALUES ('0', '1');
INSERT INTO `alliance_hostile` VALUES ('1', '0');
INSERT INTO `character` VALUES ('1', 'Spodi', 'asdf', '2', '372.4', '338', '1', '3012', '12', '810', '527', '5', '50', '75', '50', '8', '7', '2', '1', '1', '4', '1', '2', '1', '1', '1', '1', '1', '1');
INSERT INTO `character` VALUES ('2', 'Test A', '', '2', '574', '402', '1', '3012', '12', '810', '527', '50', '50', '50', '50', '8', '7', '2', '1', '1', '4', '1', '2', '1', '1', '1', '1', '1', '1');
INSERT INTO `character` VALUES ('3', 'Test B', '', '2', '736', '530', '1', '3012', '12', '810', '527', '50', '50', '50', '50', '8', '7', '2', '1', '1', '4', '1', '2', '1', '1', '1', '1', '1', '1');
INSERT INTO `character_inventory` VALUES ('1', '60');
INSERT INTO `character_template` VALUES ('1', '1', 'A Test NPC', 'TestAI', '1', '2', '1', '1', '5', '5', '1', '1', '1', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `character_template_drops` VALUES ('1', '1', '0', '2', '10');
INSERT INTO `character_template_drops` VALUES ('1', '2', '2', '5', '25');
INSERT INTO `character_template_drops` VALUES ('1', '3', '1', '1', '50');
INSERT INTO `character_template_drops` VALUES ('1', '4', '1', '1', '50');
INSERT INTO `character_template_drops` VALUES ('1', '5', '1', '1', '50');
INSERT INTO `item` VALUES ('0', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('1', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('2', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('3', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('4', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('5', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('6', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('7', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('8', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('9', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('10', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('11', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('12', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('13', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('14', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('15', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('16', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('17', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('18', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('19', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('20', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('21', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('22', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('23', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('24', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('25', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('26', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('27', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('28', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('29', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('30', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('31', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('32', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('33', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('34', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('35', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('36', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('37', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('38', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('39', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('40', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('41', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('42', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('43', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('44', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('45', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('46', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('47', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('48', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('49', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('50', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('51', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('52', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('53', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('54', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('55', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('56', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('57', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('58', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('59', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('60', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('61', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('62', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('63', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('64', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('65', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('66', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('67', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('68', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('69', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('70', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('71', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('72', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('73', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('74', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('75', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('76', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('77', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('78', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('79', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('80', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('89', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('225', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('845', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('875', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '44', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('1646', '1', '9', '16', 'Mana Potion', 'A mana potion', '32', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('1771', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '49', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('1793', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('6655', '1', '9', '16', 'Healing Potion', 'A healing potion', '68', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('1', '1', '9', '16', 'Healing Potion', 'A healing potion', '127', '10', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('2', '1', '9', '16', 'Mana Potion', 'A mana potion', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('3', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('4', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('5', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
