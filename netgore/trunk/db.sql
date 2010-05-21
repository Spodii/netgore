-- MySQL dump 10.13  Distrib 5.1.38, for Win64 (unknown)
--
-- Host: localhost    Database: demogame
-- ------------------------------------------------------
-- Server version	5.1.38-community
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO,MYSQL40' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `account`
--

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
) TYPE=InnoDB;

--
-- Dumping data for table `account`
--

LOCK TABLES `account` WRITE;
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
INSERT INTO `account` VALUES (0,'Test','3fc0a7acf087f549ac2b266baf94b8b1','test@test.com','2010-02-11 17:52:28','2010-02-11 18:03:56',16777343,NULL),(1,'Spodi','3fc0a7acf087f549ac2b266baf94b8b1','spodi@netgore.com','2009-09-07 15:43:16','2010-05-20 18:40:10',16777343,16777343);
/*!40000 ALTER TABLE `account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_ips`
--

DROP TABLE IF EXISTS `account_ips`;
CREATE TABLE `account_ips` (
  `account_id` int(11) NOT NULL COMMENT 'The ID of the account.',
  `ip` int(10) unsigned NOT NULL COMMENT 'The IP that logged into the account.',
  `time` datetime NOT NULL COMMENT 'When this IP last logged into this account.',
  PRIMARY KEY (`account_id`,`time`),
  KEY `account_id` (`account_id`,`ip`),
  CONSTRAINT `account_ips_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `account_ips`
--

LOCK TABLES `account_ips` WRITE;
/*!40000 ALTER TABLE `account_ips` DISABLE KEYS */;
INSERT INTO `account_ips` VALUES (1,16777343,'2010-05-17 17:24:58'),(1,16777343,'2010-05-17 17:25:17'),(1,16777343,'2010-05-17 17:27:23'),(1,16777343,'2010-05-17 17:27:42'),(1,16777343,'2010-05-18 14:35:36'),(1,16777343,'2010-05-18 16:00:12'),(1,16777343,'2010-05-18 16:07:22'),(1,16777343,'2010-05-18 16:09:53'),(1,16777343,'2010-05-18 16:14:46'),(1,16777343,'2010-05-18 16:20:39'),(1,16777343,'2010-05-20 00:58:31'),(1,16777343,'2010-05-20 00:58:55'),(1,16777343,'2010-05-20 01:00:22'),(1,16777343,'2010-05-20 01:01:02'),(1,16777343,'2010-05-20 01:02:11'),(1,16777343,'2010-05-20 01:03:23'),(1,16777343,'2010-05-20 01:04:02'),(1,16777343,'2010-05-20 01:05:42'),(1,16777343,'2010-05-20 01:06:01'),(1,16777343,'2010-05-20 01:06:48'),(1,16777343,'2010-05-20 01:08:19'),(1,16777343,'2010-05-20 01:08:32'),(1,16777343,'2010-05-20 01:09:56'),(1,16777343,'2010-05-20 01:37:22'),(1,16777343,'2010-05-20 01:38:22'),(1,16777343,'2010-05-20 01:38:49'),(1,16777343,'2010-05-20 02:21:51'),(1,16777343,'2010-05-20 10:31:07'),(1,16777343,'2010-05-20 10:39:47'),(1,16777343,'2010-05-20 11:29:03'),(1,16777343,'2010-05-20 11:38:24'),(1,16777343,'2010-05-20 14:32:29'),(1,16777343,'2010-05-20 14:39:46'),(1,16777343,'2010-05-20 15:15:38'),(1,16777343,'2010-05-20 15:20:10'),(1,16777343,'2010-05-20 15:22:42'),(1,16777343,'2010-05-20 15:28:51'),(1,16777343,'2010-05-20 15:31:53'),(1,16777343,'2010-05-20 15:37:17'),(1,16777343,'2010-05-20 15:39:58'),(1,16777343,'2010-05-20 15:58:51'),(1,16777343,'2010-05-20 16:00:49'),(1,16777343,'2010-05-20 16:16:53'),(1,16777343,'2010-05-20 16:24:23'),(1,16777343,'2010-05-20 16:41:20'),(1,16777343,'2010-05-20 16:43:26'),(1,16777343,'2010-05-20 16:44:42'),(1,16777343,'2010-05-20 16:47:19'),(1,16777343,'2010-05-20 16:47:27'),(1,16777343,'2010-05-20 16:48:20'),(1,16777343,'2010-05-20 16:52:05'),(1,16777343,'2010-05-20 16:57:07'),(1,16777343,'2010-05-20 17:00:33'),(1,16777343,'2010-05-20 17:01:43'),(1,16777343,'2010-05-20 17:08:26'),(1,16777343,'2010-05-20 18:40:10');
/*!40000 ALTER TABLE `account_ips` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `alliance`
--

DROP TABLE IF EXISTS `alliance`;
CREATE TABLE `alliance` (
  `id` tinyint(3) unsigned NOT NULL,
  `name` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) TYPE=InnoDB;

--
-- Dumping data for table `alliance`
--

LOCK TABLES `alliance` WRITE;
/*!40000 ALTER TABLE `alliance` DISABLE KEYS */;
INSERT INTO `alliance` VALUES (0,'user'),(1,'monster'),(2,'townsperson'),(3,'aggressive monster');
/*!40000 ALTER TABLE `alliance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `alliance_attackable`
--

DROP TABLE IF EXISTS `alliance_attackable`;
CREATE TABLE `alliance_attackable` (
  `alliance_id` tinyint(3) unsigned NOT NULL,
  `attackable_id` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`alliance_id`,`attackable_id`),
  KEY `attackable_id` (`attackable_id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `alliance_attackable_ibfk_3` FOREIGN KEY (`attackable_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `alliance_attackable_ibfk_4` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `alliance_attackable`
--

LOCK TABLES `alliance_attackable` WRITE;
/*!40000 ALTER TABLE `alliance_attackable` DISABLE KEYS */;
INSERT INTO `alliance_attackable` VALUES (1,0),(0,1),(3,1),(3,3);
/*!40000 ALTER TABLE `alliance_attackable` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `alliance_hostile`
--

DROP TABLE IF EXISTS `alliance_hostile`;
CREATE TABLE `alliance_hostile` (
  `alliance_id` tinyint(3) unsigned NOT NULL,
  `hostile_id` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`alliance_id`,`hostile_id`),
  KEY `hostile_id` (`hostile_id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `alliance_hostile_ibfk_3` FOREIGN KEY (`hostile_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `alliance_hostile_ibfk_4` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `alliance_hostile`
--

LOCK TABLES `alliance_hostile` WRITE;
/*!40000 ALTER TABLE `alliance_hostile` DISABLE KEYS */;
INSERT INTO `alliance_hostile` VALUES (1,0),(0,1),(3,1),(3,3);
/*!40000 ALTER TABLE `alliance_hostile` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character`
--

DROP TABLE IF EXISTS `character`;
CREATE TABLE `character` (
  `id` int(11) NOT NULL,
  `account_id` int(11) DEFAULT NULL,
  `character_template_id` smallint(5) unsigned DEFAULT NULL,
  `name` varchar(30) NOT NULL,
  `permissions` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `shop_id` smallint(5) unsigned DEFAULT NULL,
  `chat_dialog` smallint(5) unsigned DEFAULT NULL,
  `ai_id` smallint(5) unsigned DEFAULT NULL,
  `load_map_id` smallint(5) unsigned NOT NULL DEFAULT '1',
  `load_x` smallint(5) unsigned NOT NULL DEFAULT '100',
  `load_y` smallint(5) unsigned NOT NULL DEFAULT '100',
  `respawn_map_id` smallint(5) unsigned DEFAULT NULL,
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
  KEY `character_ibfk_2` (`load_map_id`),
  KEY `idx_name` (`name`),
  KEY `account_id` (`account_id`),
  KEY `shop_id` (`shop_id`),
  KEY `character_ibfk_5` (`respawn_map_id`),
  CONSTRAINT `character_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_2` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_3` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_4` FOREIGN KEY (`load_map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_5` FOREIGN KEY (`respawn_map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `character`
--

LOCK TABLES `character` WRITE;
/*!40000 ALTER TABLE `character` DISABLE KEYS */;
INSERT INTO `character` VALUES (0,0,NULL,'Test',0,NULL,NULL,NULL,1,765,45,1,765,45,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1),(1,1,NULL,'Spodi',0,NULL,NULL,NULL,1,765,45,1,765,45,1,1800,201385,30,880,145,100,100,100,100,1,1,1,1,1,1),(2,NULL,1,'Test A',0,NULL,NULL,1,2,800,250,2,800,250,1,1800,3012,12,810,527,5,5,5,5,5,5,0,5,5,5),(3,NULL,1,'Test B',0,NULL,NULL,1,2,506,250,2,500,250,1,1800,3012,12,810,527,5,5,5,5,5,5,0,5,5,5),(4,1,NULL,'asdf',0,NULL,NULL,NULL,1,100,100,NULL,50,50,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1);
/*!40000 ALTER TABLE `character` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_equipped`
--

DROP TABLE IF EXISTS `character_equipped`;
CREATE TABLE `character_equipped` (
  `character_id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `slot` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`character_id`,`slot`),
  KEY `item_id` (`item_id`),
  CONSTRAINT `character_equipped_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_equipped_ibfk_4` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `character_equipped`
--

LOCK TABLES `character_equipped` WRITE;
/*!40000 ALTER TABLE `character_equipped` DISABLE KEYS */;
/*!40000 ALTER TABLE `character_equipped` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_inventory`
--

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
) TYPE=InnoDB;

--
-- Dumping data for table `character_inventory`
--

LOCK TABLES `character_inventory` WRITE;
/*!40000 ALTER TABLE `character_inventory` DISABLE KEYS */;
INSERT INTO `character_inventory` VALUES (1,24,0);
/*!40000 ALTER TABLE `character_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_quest_status`
--

DROP TABLE IF EXISTS `character_quest_status`;
CREATE TABLE `character_quest_status` (
  `character_id` int(11) NOT NULL,
  `quest_id` smallint(5) unsigned NOT NULL,
  `started_on` datetime NOT NULL,
  `completed_on` datetime DEFAULT NULL,
  PRIMARY KEY (`character_id`,`quest_id`)
) TYPE=InnoDB;

--
-- Dumping data for table `character_quest_status`
--

LOCK TABLES `character_quest_status` WRITE;
/*!40000 ALTER TABLE `character_quest_status` DISABLE KEYS */;
/*!40000 ALTER TABLE `character_quest_status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_quest_status_kills`
--

DROP TABLE IF EXISTS `character_quest_status_kills`;
CREATE TABLE `character_quest_status_kills` (
  `character_id` int(11) NOT NULL,
  `quest_id` smallint(5) unsigned NOT NULL,
  `character_template_id` smallint(5) unsigned NOT NULL,
  `count` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`character_id`,`quest_id`,`character_template_id`),
  KEY `quest_id` (`quest_id`),
  KEY `character_template_id` (`character_template_id`),
  CONSTRAINT `character_quest_status_kills_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_quest_status_kills_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_quest_status_kills_ibfk_3` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `character_quest_status_kills`
--

LOCK TABLES `character_quest_status_kills` WRITE;
/*!40000 ALTER TABLE `character_quest_status_kills` DISABLE KEYS */;
/*!40000 ALTER TABLE `character_quest_status_kills` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_status_effect`
--

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
) TYPE=InnoDB;

--
-- Dumping data for table `character_status_effect`
--

LOCK TABLES `character_status_effect` WRITE;
/*!40000 ALTER TABLE `character_status_effect` DISABLE KEYS */;
/*!40000 ALTER TABLE `character_status_effect` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template`
--

DROP TABLE IF EXISTS `character_template`;
CREATE TABLE `character_template` (
  `id` smallint(5) unsigned NOT NULL,
  `alliance_id` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `name` varchar(50) NOT NULL DEFAULT 'New NPC',
  `ai_id` smallint(5) unsigned DEFAULT NULL,
  `shop_id` smallint(5) unsigned DEFAULT NULL,
  `chat_dialog` smallint(5) unsigned DEFAULT NULL,
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
) TYPE=InnoDB;

--
-- Dumping data for table `character_template`
--

LOCK TABLES `character_template` WRITE;
/*!40000 ALTER TABLE `character_template` DISABLE KEYS */;
INSERT INTO `character_template` VALUES (0,0,'User Template',NULL,NULL,NULL,1,1800,5,1,0,0,0,0,50,50,1,2,1,1,1,1),(1,1,'A Test NPC',1,NULL,NULL,2,1800,10,1,0,0,5,5,5,5,1,2,1,1,1,1),(2,2,'Quest Giver',NULL,NULL,NULL,2,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(4,2,'Potion Vendor',NULL,1,NULL,2,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(5,2,'Talking Guy',NULL,NULL,0,1,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(6,3,'Brawler',1,NULL,NULL,2,1800,10,1,0,0,8,8,5,5,1,3,1,1,1,1);
/*!40000 ALTER TABLE `character_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_equipped`
--

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
) TYPE=InnoDB;

--
-- Dumping data for table `character_template_equipped`
--

LOCK TABLES `character_template_equipped` WRITE;
/*!40000 ALTER TABLE `character_template_equipped` DISABLE KEYS */;
INSERT INTO `character_template_equipped` VALUES (0,1,5,3000),(2,1,3,60000);
/*!40000 ALTER TABLE `character_template_equipped` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_inventory`
--

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
) TYPE=InnoDB;

--
-- Dumping data for table `character_template_inventory`
--

LOCK TABLES `character_template_inventory` WRITE;
/*!40000 ALTER TABLE `character_template_inventory` DISABLE KEYS */;
INSERT INTO `character_template_inventory` VALUES (0,1,5,0,2,10000),(2,1,3,1,1,5000);
/*!40000 ALTER TABLE `character_template_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_quest_provider`
--

DROP TABLE IF EXISTS `character_template_quest_provider`;
CREATE TABLE `character_template_quest_provider` (
  `character_template_id` smallint(5) unsigned NOT NULL,
  `quest_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`character_template_id`,`quest_id`),
  KEY `quest_id` (`quest_id`),
  CONSTRAINT `character_template_quest_provider_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_quest_provider_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `character_template_quest_provider`
--

LOCK TABLES `character_template_quest_provider` WRITE;
/*!40000 ALTER TABLE `character_template_quest_provider` DISABLE KEYS */;
INSERT INTO `character_template_quest_provider` VALUES (2,1);
/*!40000 ALTER TABLE `character_template_quest_provider` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `game_constant`
--

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
  `max_inventory_size` tinyint(3) unsigned NOT NULL,
  `max_status_effect_power` smallint(5) unsigned NOT NULL,
  `screen_width` smallint(5) unsigned NOT NULL,
  `screen_height` smallint(5) unsigned NOT NULL,
  `server_ping_port` smallint(5) unsigned NOT NULL,
  `server_tcp_port` smallint(5) unsigned NOT NULL,
  `server_ip` varchar(150) NOT NULL,
  `world_physics_update_rate` smallint(5) unsigned NOT NULL
) TYPE=InnoDB ROW_FORMAT=COMPACT;

--
-- Dumping data for table `game_constant`
--

LOCK TABLES `game_constant` WRITE;
/*!40000 ALTER TABLE `game_constant` DISABLE KEYS */;
INSERT INTO `game_constant` VALUES (10,3,30,3,30,3,15,36,36,500,1024,768,44446,44445,'127.0.0.1',20);
/*!40000 ALTER TABLE `game_constant` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guild`
--

DROP TABLE IF EXISTS `guild`;
CREATE TABLE `guild` (
  `id` smallint(5) unsigned NOT NULL,
  `name` varchar(50) NOT NULL,
  `tag` varchar(5) NOT NULL,
  `created` timestamp NOT NULL,
  PRIMARY KEY (`id`)
) TYPE=InnoDB;

--
-- Dumping data for table `guild`
--

LOCK TABLES `guild` WRITE;
/*!40000 ALTER TABLE `guild` DISABLE KEYS */;
INSERT INTO `guild` VALUES (0,'asdfasdf','tg','2010-05-16 19:58:00');
/*!40000 ALTER TABLE `guild` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guild_event`
--

DROP TABLE IF EXISTS `guild_event`;
CREATE TABLE `guild_event` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'The ID of the event.',
  `guild_id` smallint(5) unsigned NOT NULL COMMENT 'The guild the event took place on.',
  `character_id` int(11) NOT NULL COMMENT 'The character that invoked the event.',
  `target_character_id` int(11) DEFAULT NULL COMMENT 'The optional character that the event involves.',
  `event_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that took place.',
  `created` datetime NOT NULL COMMENT 'When the event was created.',
  `arg0` varchar(0) DEFAULT NULL COMMENT 'The first optional event argument.',
  `arg1` varchar(0) DEFAULT NULL COMMENT 'The second optional event argument.',
  `arg2` varchar(0) DEFAULT NULL COMMENT 'The third optional event argument.',
  PRIMARY KEY (`id`),
  KEY `guild_id` (`guild_id`),
  KEY `character_id` (`character_id`),
  KEY `target_character_id` (`target_character_id`),
  CONSTRAINT `guild_event_ibfk_1` FOREIGN KEY (`guild_id`) REFERENCES `guild` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `guild_event_ibfk_2` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `guild_event_ibfk_3` FOREIGN KEY (`target_character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `guild_event`
--

LOCK TABLES `guild_event` WRITE;
/*!40000 ALTER TABLE `guild_event` DISABLE KEYS */;
/*!40000 ALTER TABLE `guild_event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guild_member`
--

DROP TABLE IF EXISTS `guild_member`;
CREATE TABLE `guild_member` (
  `character_id` int(11) NOT NULL COMMENT 'The character that is a member of the guild.',
  `guild_id` smallint(5) unsigned NOT NULL COMMENT 'The guild the member is a part of.',
  `rank` tinyint(3) unsigned NOT NULL COMMENT 'The member''s ranking in the guild.',
  `joined` datetime NOT NULL COMMENT 'When the member joined the guild.',
  PRIMARY KEY (`character_id`),
  KEY `guild_id` (`guild_id`),
  CONSTRAINT `guild_member_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `guild_member_ibfk_2` FOREIGN KEY (`guild_id`) REFERENCES `guild` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `guild_member`
--

LOCK TABLES `guild_member` WRITE;
/*!40000 ALTER TABLE `guild_member` DISABLE KEYS */;
INSERT INTO `guild_member` VALUES (1,0,3,'2010-01-26 21:34:14');
/*!40000 ALTER TABLE `guild_member` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item`
--

DROP TABLE IF EXISTS `item`;
CREATE TABLE `item` (
  `id` int(11) NOT NULL,
  `item_template_id` smallint(5) unsigned DEFAULT NULL,
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `weapon_type` tinyint(3) unsigned NOT NULL,
  `range` smallint(5) unsigned NOT NULL,
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
  PRIMARY KEY (`id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `item_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES (0,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(1,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(2,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(3,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(4,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(5,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(6,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(7,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(8,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(9,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(10,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(11,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(12,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(13,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(14,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(15,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(16,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(17,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(18,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(19,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(20,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(21,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(22,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(23,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(24,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(25,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(26,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(27,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(28,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(29,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(30,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(31,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(32,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(33,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(34,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(35,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(36,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(37,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(38,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(39,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(40,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(41,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(42,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(43,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(44,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(45,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(46,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(47,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(48,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(49,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(50,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(51,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(52,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(53,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(54,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(55,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(56,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(57,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(58,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(59,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(60,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(61,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(62,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(63,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(64,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(65,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(66,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(67,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(68,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(69,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(70,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(71,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(72,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(73,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(74,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(75,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(76,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(77,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(78,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(79,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(80,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(81,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(82,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(83,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(84,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(85,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(86,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(87,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(88,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(89,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(90,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(91,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(92,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(93,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(94,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(95,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(96,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(97,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(98,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(99,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(100,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(101,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(102,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(103,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(104,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(105,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(106,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(107,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(108,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(109,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(110,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(111,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(112,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(113,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(114,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(115,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(116,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(117,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(118,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(119,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(120,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(121,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(122,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(123,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(124,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(125,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(126,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(127,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(128,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(129,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(130,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(131,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(132,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(133,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(134,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(135,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(136,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(137,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(138,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(139,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(140,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(141,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(142,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(143,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(144,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(145,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(146,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(147,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(148,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(149,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(150,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(151,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(152,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(153,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(154,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(155,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(156,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(157,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(158,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(159,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(160,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(161,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(162,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(163,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(164,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(165,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(166,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(167,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(168,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(169,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(170,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(171,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(172,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(173,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(174,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(175,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(176,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(177,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(178,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(179,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(180,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(181,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(182,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(183,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(184,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(185,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(186,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(187,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(188,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(189,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(190,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(191,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(192,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(193,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(194,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(195,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(196,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(197,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(198,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(199,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(200,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(201,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(202,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(203,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(204,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(205,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(206,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(207,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(208,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(209,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(210,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(211,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(212,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(213,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(214,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(215,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(216,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(217,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(218,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(219,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(220,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(221,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(222,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(223,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(224,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(225,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(226,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(227,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(228,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(229,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(230,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(231,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(232,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(233,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(234,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(235,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(236,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(237,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(238,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(239,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(240,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(241,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(242,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(243,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(244,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(245,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(246,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(247,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(248,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(249,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(250,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(251,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(252,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(253,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(254,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(255,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(256,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(257,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(258,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(259,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(260,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(261,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(262,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(263,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(264,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(265,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(266,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(267,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(268,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(269,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(270,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(271,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(272,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(273,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(274,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(275,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(276,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(277,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(278,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(279,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(280,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(281,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(282,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(283,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(284,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(285,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(286,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(287,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(288,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(289,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(290,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(291,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(292,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(293,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(294,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(295,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(296,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(297,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(298,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(299,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(300,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(301,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(302,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(303,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(304,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(305,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(306,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL);
/*!40000 ALTER TABLE `item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item_template`
--

DROP TABLE IF EXISTS `item_template`;
CREATE TABLE `item_template` (
  `id` smallint(5) unsigned NOT NULL,
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `weapon_type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `range` smallint(5) unsigned NOT NULL DEFAULT '10',
  `width` tinyint(3) unsigned NOT NULL DEFAULT '16',
  `height` tinyint(3) unsigned NOT NULL DEFAULT '16',
  `name` varchar(255) NOT NULL DEFAULT 'New item template',
  `description` varchar(255) NOT NULL DEFAULT ' ',
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
) TYPE=InnoDB;

--
-- Dumping data for table `item_template`
--

LOCK TABLES `item_template` WRITE;
/*!40000 ALTER TABLE `item_template` DISABLE KEYS */;
INSERT INTO `item_template` VALUES (0,2,1,10,16,16,'Unarmed','Unarmed',1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(1,1,0,0,9,16,'Healing Potion','A healing potion',94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(2,1,0,0,9,16,'Mana Potion','A mana potion',95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL);
/*!40000 ALTER TABLE `item_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `map`
--

DROP TABLE IF EXISTS `map`;
CREATE TABLE `map` (
  `id` smallint(5) unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) TYPE=InnoDB;

--
-- Dumping data for table `map`
--

LOCK TABLES `map` WRITE;
/*!40000 ALTER TABLE `map` DISABLE KEYS */;
INSERT INTO `map` VALUES (1,'Desert 1'),(2,'Desert 2');
/*!40000 ALTER TABLE `map` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `map_spawn`
--

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
) TYPE=InnoDB AUTO_INCREMENT=6;

--
-- Dumping data for table `map_spawn`
--

LOCK TABLES `map_spawn` WRITE;
/*!40000 ALTER TABLE `map_spawn` DISABLE KEYS */;
INSERT INTO `map_spawn` VALUES (0,1,1,3,NULL,NULL,NULL,NULL),(1,1,2,1,190,278,64,64),(3,1,4,1,545,151,64,64),(4,1,5,1,736,58,64,64),(5,2,6,25,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `map_spawn` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary table structure for view `npc_character`
--

DROP TABLE IF EXISTS `npc_character`;
/*!50001 DROP VIEW IF EXISTS `npc_character`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `npc_character` (
  `id` int(11),
  `account_id` int(11),
  `character_template_id` smallint(5) unsigned,
  `name` varchar(30),
  `permissions` tinyint(3) unsigned,
  `shop_id` smallint(5) unsigned,
  `chat_dialog` smallint(5) unsigned,
  `ai_id` smallint(5) unsigned,
  `load_map_id` smallint(5) unsigned,
  `load_x` smallint(5) unsigned,
  `load_y` smallint(5) unsigned,
  `respawn_map_id` smallint(5) unsigned,
  `respawn_x` float,
  `respawn_y` float,
  `body_id` smallint(5) unsigned,
  `move_speed` smallint(5) unsigned,
  `cash` int(11),
  `level` tinyint(3) unsigned,
  `exp` int(11),
  `statpoints` int(11),
  `hp` smallint(6),
  `mp` smallint(6),
  `stat_maxhp` smallint(6),
  `stat_maxmp` smallint(6),
  `stat_minhit` smallint(6),
  `stat_maxhit` smallint(6),
  `stat_defence` smallint(6),
  `stat_agi` smallint(6),
  `stat_int` smallint(6),
  `stat_str` smallint(6)
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `quest`
--

DROP TABLE IF EXISTS `quest`;
CREATE TABLE `quest` (
  `id` smallint(5) unsigned NOT NULL,
  `repeatable` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `reward_cash` int(11) NOT NULL DEFAULT '0',
  `reward_exp` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) TYPE=InnoDB;

--
-- Dumping data for table `quest`
--

LOCK TABLES `quest` WRITE;
/*!40000 ALTER TABLE `quest` DISABLE KEYS */;
INSERT INTO `quest` VALUES (0,0,500,1000),(1,1,10,10);
/*!40000 ALTER TABLE `quest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_require_finish_item`
--

DROP TABLE IF EXISTS `quest_require_finish_item`;
CREATE TABLE `quest_require_finish_item` (
  `quest_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  `amount` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_require_finish_item_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_finish_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `quest_require_finish_item`
--

LOCK TABLES `quest_require_finish_item` WRITE;
/*!40000 ALTER TABLE `quest_require_finish_item` DISABLE KEYS */;
/*!40000 ALTER TABLE `quest_require_finish_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_require_finish_quest`
--

DROP TABLE IF EXISTS `quest_require_finish_quest`;
CREATE TABLE `quest_require_finish_quest` (
  `quest_id` smallint(5) unsigned NOT NULL,
  `req_quest_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`req_quest_id`),
  KEY `req_quest_id` (`req_quest_id`),
  CONSTRAINT `quest_require_finish_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_finish_quest_ibfk_2` FOREIGN KEY (`req_quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `quest_require_finish_quest`
--

LOCK TABLES `quest_require_finish_quest` WRITE;
/*!40000 ALTER TABLE `quest_require_finish_quest` DISABLE KEYS */;
/*!40000 ALTER TABLE `quest_require_finish_quest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_require_kill`
--

DROP TABLE IF EXISTS `quest_require_kill`;
CREATE TABLE `quest_require_kill` (
  `quest_id` smallint(5) unsigned NOT NULL,
  `character_template_id` smallint(5) unsigned NOT NULL,
  `amount` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`character_template_id`),
  KEY `character_template_id` (`character_template_id`),
  CONSTRAINT `quest_require_kill_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_kill_ibfk_2` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `quest_require_kill`
--

LOCK TABLES `quest_require_kill` WRITE;
/*!40000 ALTER TABLE `quest_require_kill` DISABLE KEYS */;
INSERT INTO `quest_require_kill` VALUES (0,1,5);
/*!40000 ALTER TABLE `quest_require_kill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_require_start_item`
--

DROP TABLE IF EXISTS `quest_require_start_item`;
CREATE TABLE `quest_require_start_item` (
  `quest_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  `amount` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_require_start_item_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_start_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `quest_require_start_item`
--

LOCK TABLES `quest_require_start_item` WRITE;
/*!40000 ALTER TABLE `quest_require_start_item` DISABLE KEYS */;
/*!40000 ALTER TABLE `quest_require_start_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_require_start_quest`
--

DROP TABLE IF EXISTS `quest_require_start_quest`;
CREATE TABLE `quest_require_start_quest` (
  `quest_id` smallint(5) unsigned NOT NULL,
  `req_quest_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`req_quest_id`),
  KEY `req_quest_id` (`req_quest_id`),
  CONSTRAINT `quest_require_start_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_start_quest_ibfk_2` FOREIGN KEY (`req_quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `quest_require_start_quest`
--

LOCK TABLES `quest_require_start_quest` WRITE;
/*!40000 ALTER TABLE `quest_require_start_quest` DISABLE KEYS */;
/*!40000 ALTER TABLE `quest_require_start_quest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_reward_item`
--

DROP TABLE IF EXISTS `quest_reward_item`;
CREATE TABLE `quest_reward_item` (
  `quest_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  `amount` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_reward_item_ibfk_3` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_reward_item_ibfk_4` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `quest_reward_item`
--

LOCK TABLES `quest_reward_item` WRITE;
/*!40000 ALTER TABLE `quest_reward_item` DISABLE KEYS */;
INSERT INTO `quest_reward_item` VALUES (0,3,1);
/*!40000 ALTER TABLE `quest_reward_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `server_setting`
--

DROP TABLE IF EXISTS `server_setting`;
CREATE TABLE `server_setting` (
  `motd` varchar(250) NOT NULL DEFAULT '' COMMENT 'The message of the day.'
) TYPE=InnoDB;

--
-- Dumping data for table `server_setting`
--

LOCK TABLES `server_setting` WRITE;
/*!40000 ALTER TABLE `server_setting` DISABLE KEYS */;
INSERT INTO `server_setting` VALUES ('Welcome to NetGore! Use the arrow keys to move, control to attack, alt to talk to NPCs and use world entities, and space to pick up items.');
/*!40000 ALTER TABLE `server_setting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `server_time`
--

DROP TABLE IF EXISTS `server_time`;
CREATE TABLE `server_time` (
  `server_time` datetime NOT NULL,
  PRIMARY KEY (`server_time`)
) TYPE=InnoDB;

--
-- Dumping data for table `server_time`
--

LOCK TABLES `server_time` WRITE;
/*!40000 ALTER TABLE `server_time` DISABLE KEYS */;
INSERT INTO `server_time` VALUES ('2010-05-20 18:40:17');
/*!40000 ALTER TABLE `server_time` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shop`
--

DROP TABLE IF EXISTS `shop`;
CREATE TABLE `shop` (
  `id` smallint(5) unsigned NOT NULL,
  `name` varchar(60) NOT NULL,
  `can_buy` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) TYPE=InnoDB;

--
-- Dumping data for table `shop`
--

LOCK TABLES `shop` WRITE;
/*!40000 ALTER TABLE `shop` DISABLE KEYS */;
INSERT INTO `shop` VALUES (0,'Test Shop',1),(1,'Vending Machine',0);
/*!40000 ALTER TABLE `shop` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shop_item`
--

DROP TABLE IF EXISTS `shop_item`;
CREATE TABLE `shop_item` (
  `shop_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`shop_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `shop_item_ibfk_1` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `shop_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `shop_item`
--

LOCK TABLES `shop_item` WRITE;
/*!40000 ALTER TABLE `shop_item` DISABLE KEYS */;
INSERT INTO `shop_item` VALUES (0,1),(1,1),(0,2),(1,2),(0,3),(0,4),(0,5),(0,6),(0,7);
/*!40000 ALTER TABLE `shop_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary table structure for view `user_character`
--

DROP TABLE IF EXISTS `user_character`;
/*!50001 DROP VIEW IF EXISTS `user_character`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `user_character` (
  `id` int(11),
  `account_id` int(11),
  `character_template_id` smallint(5) unsigned,
  `name` varchar(30),
  `permissions` tinyint(3) unsigned,
  `shop_id` smallint(5) unsigned,
  `chat_dialog` smallint(5) unsigned,
  `ai_id` smallint(5) unsigned,
  `load_map_id` smallint(5) unsigned,
  `load_x` smallint(5) unsigned,
  `load_y` smallint(5) unsigned,
  `respawn_map_id` smallint(5) unsigned,
  `respawn_x` float,
  `respawn_y` float,
  `body_id` smallint(5) unsigned,
  `move_speed` smallint(5) unsigned,
  `cash` int(11),
  `level` tinyint(3) unsigned,
  `exp` int(11),
  `statpoints` int(11),
  `hp` smallint(6),
  `mp` smallint(6),
  `stat_maxhp` smallint(6),
  `stat_maxmp` smallint(6),
  `stat_minhit` smallint(6),
  `stat_maxhit` smallint(6),
  `stat_defence` smallint(6),
  `stat_agi` smallint(6),
  `stat_int` smallint(6),
  `stat_str` smallint(6)
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `world_stats_guild_user_change`
--

DROP TABLE IF EXISTS `world_stats_guild_user_change`;
CREATE TABLE `world_stats_guild_user_change` (
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user who changed the guild they are part of.',
  `guild_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the guild, or null if the user left a guild.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  KEY `user_id` (`user_id`),
  KEY `guild_id` (`guild_id`),
  CONSTRAINT `world_stats_guild_user_change_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_guild_user_change_ibfk_2` FOREIGN KEY (`guild_id`) REFERENCES `guild` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `world_stats_guild_user_change`
--

LOCK TABLES `world_stats_guild_user_change` WRITE;
/*!40000 ALTER TABLE `world_stats_guild_user_change` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_guild_user_change` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_network`
--

DROP TABLE IF EXISTS `world_stats_network`;
CREATE TABLE `world_stats_network` (
  `when` timestamp NOT NULL COMMENT 'When these network stats were logged. The values correspond to a time period defined in the WorldStatsTracker constructor. This timestamp marks the end of this period of time. So all stats correspond to the time frame range: [when - rate, when].',
  `tcp_recv` mediumint(8) unsigned NOT NULL COMMENT 'The number of bytes that have been received over the TCP channel.',
  `tcp_recvs` mediumint(8) unsigned NOT NULL COMMENT 'The number of messages that have been received over the TCP channel.',
  `tcp_sent` mediumint(8) unsigned NOT NULL COMMENT 'The number of bytes that have been sent over the TCP channel.',
  `tcp_sends` mediumint(8) unsigned NOT NULL COMMENT 'The number of messages that have been sent over the TCP channel.',
  `udp_recv` mediumint(8) unsigned NOT NULL COMMENT 'The number of bytes that have been received over the UDP channel.',
  `udp_recvs` mediumint(8) unsigned NOT NULL COMMENT 'The number of messages that have been received over the UDP channel.',
  `udp_sent` mediumint(8) unsigned NOT NULL COMMENT 'The number of bytes that have been sent over the UDP channel.',
  `udp_sends` mediumint(8) unsigned NOT NULL COMMENT 'The number of messages that have been sent over the UDP channel.',
  `connections` mediumint(8) unsigned NOT NULL,
  PRIMARY KEY (`when`)
) TYPE=MyISAM;

--
-- Dumping data for table `world_stats_network`
--

LOCK TABLES `world_stats_network` WRITE;
/*!40000 ALTER TABLE `world_stats_network` DISABLE KEYS */;
INSERT INTO `world_stats_network` VALUES ('2010-05-18 21:35:07',0,0,0,0,0,0,0,0,0),('2010-05-18 21:35:34',0,0,0,0,0,0,0,0,0),('2010-05-18 21:36:34',0,0,0,0,0,0,0,0,0),('2010-05-18 23:00:11',0,0,0,0,0,0,0,0,0),('2010-05-18 23:01:11',0,0,0,0,0,0,0,0,0),('2010-05-18 23:02:11',0,0,0,0,0,0,0,0,0),('2010-05-18 23:03:11',0,0,0,0,0,0,0,0,0),('2010-05-18 23:04:11',0,0,0,0,0,0,0,0,0),('2010-05-18 23:05:11',0,0,0,0,0,0,0,0,0),('2010-05-18 23:06:11',0,0,0,0,0,0,0,0,0),('2010-05-18 23:07:14',0,0,0,0,0,0,0,0,0),('2010-05-18 23:09:53',0,0,0,0,0,0,0,0,0),('2010-05-18 23:14:44',0,0,0,0,0,0,0,0,0),('2010-05-18 23:20:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:21:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:22:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:23:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:24:46',0,0,0,0,0,0,0,0,0),('2010-05-18 23:25:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:26:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:27:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:28:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:29:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:30:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:31:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:32:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:33:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:34:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:35:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:36:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:37:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:38:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:39:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:40:38',0,0,0,0,0,0,0,0,0),('2010-05-18 23:41:40',0,0,0,0,0,0,0,0,0),('2010-05-20 07:58:28',0,0,0,0,0,0,0,0,0),('2010-05-20 07:58:53',0,0,0,0,0,0,0,0,0),('2010-05-20 08:00:20',0,0,0,0,0,0,0,0,0),('2010-05-20 08:00:58',0,0,0,0,0,0,0,0,0),('2010-05-20 08:02:09',0,0,0,0,0,0,0,0,0),('2010-05-20 08:03:21',0,0,0,0,0,0,0,0,0),('2010-05-20 08:04:00',0,0,0,0,0,0,0,0,0),('2010-05-20 08:05:00',0,0,0,0,0,0,0,0,0),('2010-05-20 08:05:35',0,0,0,0,0,0,0,0,0),('2010-05-20 08:06:00',0,0,0,0,0,0,0,0,0),('2010-05-20 08:06:46',0,0,0,0,0,0,0,0,0),('2010-05-20 08:07:28',0,0,0,0,0,0,0,0,0),('2010-05-20 08:08:28',0,0,0,0,0,0,0,0,0),('2010-05-20 08:09:28',0,0,0,0,0,0,0,0,0),('2010-05-20 08:10:28',0,0,0,0,0,0,0,0,0),('2010-05-20 08:37:19',0,0,0,0,0,0,0,0,0),('2010-05-20 08:38:21',0,0,0,0,0,0,0,0,0),('2010-05-20 08:38:46',0,0,0,0,0,0,0,0,0),('2010-05-20 09:21:49',0,0,0,0,0,0,0,0,0),('2010-05-20 17:31:05',0,0,0,0,0,0,0,0,0),('2010-05-20 17:39:45',0,0,0,0,0,0,0,0,0),('2010-05-20 17:40:45',0,0,0,0,0,0,0,0,0),('2010-05-20 17:41:45',0,0,0,0,0,0,0,0,0),('2010-05-20 18:29:02',0,0,0,0,0,0,0,0,0),('2010-05-20 18:38:24',0,0,0,0,0,0,0,0,0),('2010-05-20 21:32:27',0,0,0,0,0,0,0,0,0),('2010-05-20 21:33:27',0,0,0,0,0,0,0,0,0),('2010-05-20 21:34:27',0,0,0,0,0,0,0,0,0),('2010-05-20 21:35:27',0,0,0,0,0,0,0,0,0),('2010-05-20 21:36:27',0,0,0,0,0,0,0,0,0),('2010-05-20 21:37:27',0,0,0,0,0,0,0,0,0),('2010-05-20 21:38:27',0,0,0,0,0,0,0,0,0),('2010-05-20 21:39:27',0,0,0,0,0,0,0,0,0),('2010-05-20 21:39:44',0,0,0,0,0,0,0,0,0),('2010-05-20 21:40:44',0,0,0,0,0,0,0,0,0),('2010-05-20 21:41:57',0,0,0,0,0,0,0,0,0),('2010-05-20 21:42:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:43:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:44:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:45:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:46:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:47:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:48:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:49:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:50:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:51:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:52:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:53:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:54:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:55:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:56:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:57:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:58:50',0,0,0,0,0,0,0,0,0),('2010-05-20 21:59:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:00:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:01:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:02:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:03:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:04:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:05:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:06:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:07:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:08:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:09:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:10:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:11:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:12:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:13:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:14:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:15:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:16:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:17:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:18:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:19:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:20:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:21:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:22:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:23:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:24:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:25:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:26:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:27:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:28:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:29:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:30:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:31:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:32:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:33:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:34:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:35:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:36:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:37:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:38:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:39:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:40:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:41:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:42:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:43:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:44:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:45:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:46:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:47:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:48:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:49:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:50:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:51:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:52:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:53:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:54:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:55:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:56:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:57:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:58:50',0,0,0,0,0,0,0,0,0),('2010-05-20 22:59:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:00:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:01:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:02:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:03:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:04:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:05:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:06:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:07:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:08:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:09:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:10:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:11:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:12:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:13:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:14:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:15:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:16:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:17:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:18:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:19:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:20:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:21:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:22:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:23:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:24:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:25:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:26:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:27:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:28:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:29:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:30:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:31:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:32:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:33:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:34:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:35:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:36:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:37:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:38:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:39:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:40:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:41:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:42:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:43:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:44:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:45:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:46:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:47:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:48:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:49:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:50:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:51:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:52:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:53:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:54:50',0,0,0,0,0,0,0,0,0),('2010-05-20 23:56:54',0,0,0,0,0,0,0,0,0),('2010-05-20 23:57:54',0,0,0,0,0,0,0,0,0),('2010-05-20 23:58:54',0,0,0,0,0,0,0,0,0),('2010-05-21 00:00:31',0,0,0,0,0,0,0,0,0),('2010-05-21 00:01:42',0,0,0,0,0,0,0,0,0),('2010-05-21 00:02:53',0,0,0,0,0,0,0,0,0),('2010-05-21 00:08:23',0,0,0,0,0,0,0,0,0),('2010-05-21 01:40:04',0,0,0,0,0,0,0,0,0);
/*!40000 ALTER TABLE `world_stats_network` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_npc_kill_user`
--

DROP TABLE IF EXISTS `world_stats_npc_kill_user`;
CREATE TABLE `world_stats_npc_kill_user` (
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user.',
  `npc_template_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The template ID of the NPC. Only valid when the NPC has a template ID set.',
  `user_level` tinyint(3) unsigned NOT NULL COMMENT 'The level of the user was when this event took place.',
  `user_x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place.',
  `user_y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place.',
  `npc_x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.',
  `npc_y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  KEY `user_id` (`user_id`),
  KEY `npc_template_id` (`npc_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_npc_kill_user_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_npc_kill_user_ibfk_2` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_npc_kill_user_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `world_stats_npc_kill_user`
--

LOCK TABLES `world_stats_npc_kill_user` WRITE;
/*!40000 ALTER TABLE `world_stats_npc_kill_user` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_npc_kill_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_quest_accept`
--

DROP TABLE IF EXISTS `world_stats_quest_accept`;
CREATE TABLE `world_stats_quest_accept` (
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user that accepted the quest.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that was accepted.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  KEY `user_id` (`user_id`),
  KEY `quest_id` (`quest_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_quest_accept_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_accept_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_accept_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB ROW_FORMAT=COMPACT;

--
-- Dumping data for table `world_stats_quest_accept`
--

LOCK TABLES `world_stats_quest_accept` WRITE;
/*!40000 ALTER TABLE `world_stats_quest_accept` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_quest_accept` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_quest_cancel`
--

DROP TABLE IF EXISTS `world_stats_quest_cancel`;
CREATE TABLE `world_stats_quest_cancel` (
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user that canceled the quest.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that was canceled.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  KEY `user_id` (`user_id`),
  KEY `quest_id` (`quest_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_quest_cancel_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_cancel_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_cancel_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB ROW_FORMAT=COMPACT;

--
-- Dumping data for table `world_stats_quest_cancel`
--

LOCK TABLES `world_stats_quest_cancel` WRITE;
/*!40000 ALTER TABLE `world_stats_quest_cancel` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_quest_cancel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_quest_complete`
--

DROP TABLE IF EXISTS `world_stats_quest_complete`;
CREATE TABLE `world_stats_quest_complete` (
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user that completed the quest.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that was completed.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  KEY `user_id` (`user_id`),
  KEY `quest_id` (`quest_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_quest_complete_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_complete_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_complete_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `world_stats_quest_complete`
--

LOCK TABLES `world_stats_quest_complete` WRITE;
/*!40000 ALTER TABLE `world_stats_quest_complete` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_quest_complete` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_user_consume_item`
--

DROP TABLE IF EXISTS `world_stats_user_consume_item`;
CREATE TABLE `world_stats_user_consume_item` (
  `user_id` int(11) NOT NULL COMMENT 'The user that this event is related to.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template ID of the item that was consumed. Only valid when the item has a set template ID.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The map the user was on when this event took place.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  KEY `user_id` (`user_id`),
  KEY `item_template_id` (`item_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_user_consume_item_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_consume_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_consume_item_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `world_stats_user_consume_item`
--

LOCK TABLES `world_stats_user_consume_item` WRITE;
/*!40000 ALTER TABLE `world_stats_user_consume_item` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_user_consume_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_user_kill_npc`
--

DROP TABLE IF EXISTS `world_stats_user_kill_npc`;
CREATE TABLE `world_stats_user_kill_npc` (
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user.',
  `npc_template_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The template ID of the NPC. Only valid when the NPC has a template ID set.',
  `user_level` tinyint(3) unsigned NOT NULL COMMENT 'The level of the user was when this event took place.',
  `user_x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `user_y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `npc_x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.',
  `npc_y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  KEY `user_id` (`user_id`),
  KEY `npc_template_id` (`npc_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_user_kill_npc_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_kill_npc_ibfk_2` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_kill_npc_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `world_stats_user_kill_npc`
--

LOCK TABLES `world_stats_user_kill_npc` WRITE;
/*!40000 ALTER TABLE `world_stats_user_kill_npc` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_user_kill_npc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_user_level`
--

DROP TABLE IF EXISTS `world_stats_user_level`;
CREATE TABLE `world_stats_user_level` (
  `character_id` int(11) NOT NULL COMMENT 'The ID of the character that leveled up.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `level` tinyint(3) unsigned NOT NULL COMMENT 'The level that the character leveled up to (their new level).',
  `when` timestamp NOT NULL COMMENT 'When this event took place.'
) TYPE=InnoDB;

--
-- Dumping data for table `world_stats_user_level`
--

LOCK TABLES `world_stats_user_level` WRITE;
/*!40000 ALTER TABLE `world_stats_user_level` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_user_level` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_user_shopping`
--

DROP TABLE IF EXISTS `world_stats_user_shopping`;
CREATE TABLE `world_stats_user_shopping` (
  `shop_id` smallint(5) unsigned NOT NULL COMMENT 'The ID of the shop the event took place at.',
  `character_id` int(11) NOT NULL COMMENT 'The ID of the character that performed this transaction with the shop.',
  `item_template_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the item template that the event relates to. Only valid when the item involved has a set item template ID.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map the event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map X coordinate of the shopper when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map Y coordinate of the shopper when this event took place. Only valid when the map_id is not null.',
  `cost` int(11) NOT NULL COMMENT 'The amount of money that was involved in this transaction (how much the shopper sold the items for, or how much they bought the items for). ',
  `amount` tinyint(3) unsigned NOT NULL COMMENT 'The number of items involved in the transaction. Should always be greater than 0, and should only be greater for 1 for items that can stack.',
  `sale_type` tinyint(4) NOT NULL COMMENT 'Whether the shop sold to the user, or vise versa. If 0, the shop sold an item to the shopper. If non-zero, the shopper sold an item to a shop.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  KEY `shop_id` (`shop_id`),
  KEY `character_id` (`character_id`),
  KEY `item_template_id` (`item_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_user_shopping_ibfk_1` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_shopping_ibfk_2` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_shopping_ibfk_3` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_shopping_ibfk_4` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `world_stats_user_shopping`
--

LOCK TABLES `world_stats_user_shopping` WRITE;
/*!40000 ALTER TABLE `world_stats_user_shopping` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_user_shopping` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'demogame'
--
/*!50003 DROP FUNCTION IF EXISTS `CreateUserOnAccount` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `CreateUserOnAccount`(accountName VARCHAR(50), characterName VARCHAR(30), characterID INT) RETURNS varchar(100) CHARSET latin1
BEGIN
		
		DECLARE character_count INT DEFAULT 0;
		DECLARE max_character_count INT DEFAULT 3;
		DECLARE is_id_free INT DEFAULT 0;
		DECLARE is_name_free INT DEFAULT 0;
		DECLARE errorMsg VARCHAR(100) DEFAULT "";
		DECLARE accountID INT DEFAULT NULL;

		SELECT `id` INTO accountID FROM `account` WHERE `name` = accountName;

		IF ISNULL(accountID) THEN
			SET errorMsg = "Account with the specified name does not exist.";
		ELSE
			SELECT COUNT(*) INTO character_count FROM `character` WHERE `account_id` = accountID;
			SELECT `max_characters_per_account` INTO max_character_count FROM `game_constant`;

			IF character_count > max_character_count THEN
				SET errorMsg = "No free character slots available in the account.";
			ELSE
				SELECT COUNT(*) INTO is_id_free FROM `character` WHERE `id` = characterID;
				
				IF is_id_free > 0 THEN
					SET errorMsg = "The specified CharacterID is not available for use.";
				ELSE
					SELECT COUNT(*) INTO is_name_free FROM `user_character` WHERE `name` = characterName;
						
					IF is_name_free > 0 THEN
						SET errorMsg = "The specified character name is not available for use.";
					ELSE
						INSERT INTO `character` SET `id` = characterID, `name`	= characterName, `account_id`= 	accountID;
					END IF;
				END IF;
			END IF;
		END IF;
				
		RETURN errorMsg;
  
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Rebuild_Views` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `Rebuild_Views`()
BEGIN
	
	CALL Rebuild_View_NPC_Character();
	CALL Rebuild_View_User_Character();
    
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Rebuild_View_NPC_Character` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = latin1 */ ;
/*!50003 SET character_set_results = latin1 */ ;
/*!50003 SET collation_connection  = latin1_swedish_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `Rebuild_View_NPC_Character`()
BEGIN
	
	DROP VIEW IF EXISTS `npc_character`;
	CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `npc_character` AS SELECT *FROM `character` WHERE `account_id` IS NULL;
    
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Rebuild_View_User_Character` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`%`*/ /*!50003 PROCEDURE `Rebuild_View_User_Character`()
BEGIN
	
	DROP VIEW IF EXISTS `user_character`;
	CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `user_character` AS SELECT * FROM `character` WHERE `account_id` IS NOT NULL;
    
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `npc_character`
--

/*!50001 DROP TABLE IF EXISTS `npc_character`*/;
/*!50001 DROP VIEW IF EXISTS `npc_character`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_swedish_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `npc_character` AS select `character`.`id` AS `id`,`character`.`account_id` AS `account_id`,`character`.`character_template_id` AS `character_template_id`,`character`.`name` AS `name`,`character`.`permissions` AS `permissions`,`character`.`shop_id` AS `shop_id`,`character`.`chat_dialog` AS `chat_dialog`,`character`.`ai_id` AS `ai_id`,`character`.`load_map_id` AS `load_map_id`,`character`.`load_x` AS `load_x`,`character`.`load_y` AS `load_y`,`character`.`respawn_map_id` AS `respawn_map_id`,`character`.`respawn_x` AS `respawn_x`,`character`.`respawn_y` AS `respawn_y`,`character`.`body_id` AS `body_id`,`character`.`move_speed` AS `move_speed`,`character`.`cash` AS `cash`,`character`.`level` AS `level`,`character`.`exp` AS `exp`,`character`.`statpoints` AS `statpoints`,`character`.`hp` AS `hp`,`character`.`mp` AS `mp`,`character`.`stat_maxhp` AS `stat_maxhp`,`character`.`stat_maxmp` AS `stat_maxmp`,`character`.`stat_minhit` AS `stat_minhit`,`character`.`stat_maxhit` AS `stat_maxhit`,`character`.`stat_defence` AS `stat_defence`,`character`.`stat_agi` AS `stat_agi`,`character`.`stat_int` AS `stat_int`,`character`.`stat_str` AS `stat_str` from `character` where isnull(`character`.`account_id`) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `user_character`
--

/*!50001 DROP TABLE IF EXISTS `user_character`*/;
/*!50001 DROP VIEW IF EXISTS `user_character`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `user_character` AS select `character`.`id` AS `id`,`character`.`account_id` AS `account_id`,`character`.`character_template_id` AS `character_template_id`,`character`.`name` AS `name`,`character`.`permissions` AS `permissions`,`character`.`shop_id` AS `shop_id`,`character`.`chat_dialog` AS `chat_dialog`,`character`.`ai_id` AS `ai_id`,`character`.`load_map_id` AS `load_map_id`,`character`.`load_x` AS `load_x`,`character`.`load_y` AS `load_y`,`character`.`respawn_map_id` AS `respawn_map_id`,`character`.`respawn_x` AS `respawn_x`,`character`.`respawn_y` AS `respawn_y`,`character`.`body_id` AS `body_id`,`character`.`move_speed` AS `move_speed`,`character`.`cash` AS `cash`,`character`.`level` AS `level`,`character`.`exp` AS `exp`,`character`.`statpoints` AS `statpoints`,`character`.`hp` AS `hp`,`character`.`mp` AS `mp`,`character`.`stat_maxhp` AS `stat_maxhp`,`character`.`stat_maxmp` AS `stat_maxmp`,`character`.`stat_minhit` AS `stat_minhit`,`character`.`stat_maxhit` AS `stat_maxhit`,`character`.`stat_defence` AS `stat_defence`,`character`.`stat_agi` AS `stat_agi`,`character`.`stat_int` AS `stat_int`,`character`.`stat_str` AS `stat_str` from `character` where (`character`.`account_id` is not null) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2010-05-20 18:40:51
