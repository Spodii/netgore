/*
MySQL Data Transfer
Source Host: localhost
Source Database: demogame
Target Host: localhost
Target Database: demogame
Date: 7/18/2009 9:20:37 PM
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
  KEY `attackable_id` (`attackable_id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `alliance_attackable_ibfk_3` FOREIGN KEY (`attackable_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `alliance_attackable_ibfk_4` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for alliance_hostile
-- ----------------------------
CREATE TABLE `alliance_hostile` (
  `alliance_id` tinyint(3) unsigned NOT NULL,
  `hostile_id` tinyint(3) unsigned NOT NULL,
  KEY `hostile_id` (`hostile_id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `alliance_hostile_ibfk_3` FOREIGN KEY (`hostile_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `alliance_hostile_ibfk_4` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for character
-- ----------------------------
CREATE TABLE `character` (
  `id` int(10) unsigned NOT NULL,
  `template_id` smallint(5) unsigned DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `map` smallint(5) unsigned NOT NULL DEFAULT '1',
  `x` float NOT NULL DEFAULT '100',
  `y` float NOT NULL DEFAULT '100',
  `body` smallint(5) unsigned NOT NULL DEFAULT '1',
  `cash` int(10) unsigned NOT NULL DEFAULT '0',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `exp` int(10) unsigned NOT NULL DEFAULT '0',
  `statpoints` int(10) unsigned NOT NULL DEFAULT '0',
  `hp` smallint(5) unsigned NOT NULL DEFAULT '50',
  `mp` smallint(5) unsigned NOT NULL DEFAULT '50',
  `maxhp` smallint(5) unsigned NOT NULL DEFAULT '50',
  `maxmp` smallint(5) unsigned NOT NULL DEFAULT '50',
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
  `ws` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `idx_name` (`name`),
  KEY `template_id` (`template_id`),
  CONSTRAINT `character_ibfk_1` FOREIGN KEY (`template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for character_equipped
-- ----------------------------
CREATE TABLE `character_equipped` (
  `character_id` int(10) unsigned NOT NULL,
  `item_id` int(10) unsigned NOT NULL,
  `slot` tinyint(3) unsigned NOT NULL,
  KEY `item_id` (`item_id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `character_equipped_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_equipped_ibfk_4` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for character_inventory
-- ----------------------------
CREATE TABLE `character_inventory` (
  `character_id` int(10) unsigned NOT NULL,
  `item_id` int(10) unsigned NOT NULL,
  KEY `item_id` (`item_id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `character_inventory_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_inventory_ibfk_4` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
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
-- Table structure for character_template_equipped
-- ----------------------------
CREATE TABLE `character_template_equipped` (
  `character_id` smallint(5) unsigned NOT NULL,
  `item_id` smallint(5) unsigned NOT NULL,
  `chance` smallint(5) unsigned NOT NULL,
  KEY `item_id` (`item_id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `character_template_equipped_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_equipped_ibfk_4` FOREIGN KEY (`character_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for character_template_inventory
-- ----------------------------
CREATE TABLE `character_template_inventory` (
  `character_id` smallint(5) unsigned NOT NULL,
  `item_id` smallint(5) unsigned NOT NULL,
  `min` tinyint(3) unsigned NOT NULL,
  `max` tinyint(3) unsigned NOT NULL,
  `chance` smallint(5) unsigned NOT NULL,
  KEY `item_id` (`item_id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `character_template_inventory_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_inventory_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
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
  `hp` smallint(5) unsigned NOT NULL DEFAULT '0',
  `imm` smallint(5) unsigned NOT NULL DEFAULT '0',
  `int` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxhit` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxhp` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxmp` smallint(5) unsigned NOT NULL DEFAULT '0',
  `minhit` smallint(5) unsigned NOT NULL DEFAULT '0',
  `mp` smallint(5) unsigned NOT NULL DEFAULT '0',
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
  `hp` smallint(5) unsigned NOT NULL DEFAULT '0',
  `mp` smallint(5) unsigned NOT NULL DEFAULT '0',
  `minhit` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxhit` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxhp` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxmp` smallint(5) unsigned NOT NULL DEFAULT '0',
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
INSERT INTO `character` VALUES ('1', null, 'Spodi', 'asdf', '2', '656.8', '466', '1', '1', '1', '4', '0', '50', '50', '50', '50', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1', '1');
INSERT INTO `character` VALUES ('2', '1', 'Test A', '', '2', '908.4', '530', '1', '3012', '12', '810', '527', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5');
INSERT INTO `character` VALUES ('3', '1', 'Test B', '', '2', '930', '530', '1', '3012', '12', '810', '527', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5', '5');
INSERT INTO `character_equipped` VALUES ('2', '221', '2');
INSERT INTO `character_equipped` VALUES ('2', '224', '1');
INSERT INTO `character_equipped` VALUES ('3', '211', '2');
INSERT INTO `character_equipped` VALUES ('3', '226', '0');
INSERT INTO `character_inventory` VALUES ('1', '40');
INSERT INTO `character_inventory` VALUES ('1', '19');
INSERT INTO `character_inventory` VALUES ('2', '222');
INSERT INTO `character_inventory` VALUES ('2', '207');
INSERT INTO `character_inventory` VALUES ('2', '223');
INSERT INTO `character_inventory` VALUES ('3', '225');
INSERT INTO `character_inventory` VALUES ('3', '48');
INSERT INTO `character_template` VALUES ('1', '1', 'A Test NPC', 'TestAI', '1', '2', '0', '0', '0', '5', '5', '5', '5', '0', '0', '0', '0', '1', '0', '1', '1', '0', '0', '1', '0', '0', '0', '1', '0', '0');
INSERT INTO `character_template_equipped` VALUES ('1', '3', '10000');
INSERT INTO `character_template_equipped` VALUES ('1', '4', '5000');
INSERT INTO `character_template_equipped` VALUES ('1', '5', '5000');
INSERT INTO `character_template_inventory` VALUES ('1', '1', '0', '5', '10000');
INSERT INTO `character_template_inventory` VALUES ('1', '2', '0', '1', '10000');
INSERT INTO `character_template_inventory` VALUES ('1', '3', '1', '1', '5000');
INSERT INTO `character_template_inventory` VALUES ('1', '4', '1', '2', '5000');
INSERT INTO `character_template_inventory` VALUES ('1', '5', '0', '2', '10000');
INSERT INTO `item` VALUES ('0', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('1', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('2', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('3', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('4', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('5', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('6', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('7', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('8', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('9', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('10', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('11', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('12', '1', '9', '16', 'Healing Potion', 'A healing potion', '4', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('13', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('14', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('15', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('16', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('17', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('18', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('19', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('20', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('21', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('22', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('23', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('24', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('25', '1', '9', '16', 'Healing Potion', 'A healing potion', '3', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('26', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('27', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('28', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('29', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('30', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('31', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('32', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('33', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('34', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('35', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('36', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('37', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('39', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('40', '1', '9', '16', 'Healing Potion', 'A healing potion', '5', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('41', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('42', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('43', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('48', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('50', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('51', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('52', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('53', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('54', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('55', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('56', '1', '9', '16', 'Healing Potion', 'A healing potion', '4', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('57', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('58', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('59', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('60', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('61', '1', '9', '16', 'Healing Potion', 'A healing potion', '5', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('62', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('63', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('64', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('65', '1', '9', '16', 'Healing Potion', 'A healing potion', '2', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('66', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('67', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('68', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('69', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('70', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('71', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('72', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('73', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('74', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('75', '1', '9', '16', 'Healing Potion', 'A healing potion', '3', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('76', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('77', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('78', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('79', '1', '9', '16', 'Healing Potion', 'A healing potion', '4', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('80', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('81', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('82', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('83', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('84', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('85', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '2', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('86', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('87', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('88', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('89', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('90', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('91', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('92', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('93', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('94', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('95', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('96', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('97', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('98', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('99', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('100', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('101', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('102', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('103', '1', '9', '16', 'Healing Potion', 'A healing potion', '4', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('104', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('105', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('106', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('107', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('108', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('109', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('110', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('111', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('112', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('113', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('114', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('115', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('116', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('117', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('118', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('119', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('120', '1', '9', '16', 'Healing Potion', 'A healing potion', '3', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('121', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('122', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('123', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('124', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('125', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('126', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('127', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('128', '1', '9', '16', 'Healing Potion', 'A healing potion', '5', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('129', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('130', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('131', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('132', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('133', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('134', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('135', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('136', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('137', '1', '9', '16', 'Healing Potion', 'A healing potion', '4', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('138', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('139', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('140', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('141', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('142', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('143', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('144', '1', '9', '16', 'Healing Potion', 'A healing potion', '3', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('145', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('146', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('147', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('148', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('149', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('150', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('151', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('152', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('153', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('154', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('155', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('156', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('157', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('158', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('177', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('178', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('179', '1', '9', '16', 'Healing Potion', 'A healing potion', '1', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('180', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('181', '1', '9', '16', 'Mana Potion', 'A mana potion', '1', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('207', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('211', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('221', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '1', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('222', '1', '9', '16', 'Healing Potion', 'A healing potion', '5', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('223', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '2', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('224', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '1', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('225', '1', '9', '16', 'Healing Potion', 'A healing potion', '4', '127', '10', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item` VALUES ('226', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '1', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('1', '1', '9', '16', 'Healing Potion', 'A healing potion', '127', '10', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('2', '1', '9', '16', 'Mana Potion', 'A mana potion', '128', '10', '0', '0', '0', '0', '0', '0', '0', '0', '0', '25', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('3', '2', '24', '24', 'Titanium Sword', 'A sword made out of titanium', '126', '100', '1', '0', '1', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('4', '4', '22', '22', 'Crystal Armor', 'Body armor made out of crystal', '130', '50', '0', '0', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `item_template` VALUES ('5', '3', '11', '16', 'Crystal Helmet', 'A helmet made out of crystal', '132', '50', '0', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');
