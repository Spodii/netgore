-- MySQL dump 10.13  Distrib 5.5.27, for Win64 (x86)
--
-- Host: localhost    Database: demogame_tmp
-- ------------------------------------------------------
-- Server version	5.5.27

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `account`
--

DROP TABLE IF EXISTS `account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'The unique ID of the account.',
  `name` varchar(30) NOT NULL COMMENT 'The account name.',
  `password` char(32) NOT NULL COMMENT 'The account password (MD5 hashed).',
  `email` varchar(60) NOT NULL COMMENT 'The email address.',
  `permissions` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT 'The permission level bit mask (see UserPermissions enum).',
  `time_created` datetime NOT NULL COMMENT 'When the account was created.',
  `time_last_login` datetime NOT NULL COMMENT 'When the account was last logged in to.',
  `creator_ip` int(10) unsigned NOT NULL COMMENT 'The IP address that created the account.',
  `current_ip` int(10) unsigned DEFAULT NULL COMMENT 'IP address currently logged in to the account, or null if nobody is logged in.',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COMMENT='The user accounts. Multiple chars can exist per account.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account`
--

LOCK TABLES `account` WRITE;
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
INSERT INTO `account` VALUES (1,'Spodi','3fc0a7acf087f549ac2b266baf94b8b1','spodi@netgore.com',255,'2009-09-07 15:43:16','2012-12-14 19:24:56',16777343,NULL);
/*!40000 ALTER TABLE `account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_ban`
--

DROP TABLE IF EXISTS `account_ban`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account_ban` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'The unique ID.',
  `account_id` int(11) NOT NULL COMMENT 'The account that this ban is for.',
  `start_time` datetime NOT NULL COMMENT 'When this ban started.',
  `end_time` datetime NOT NULL COMMENT 'When this ban ends.',
  `reason` varchar(255) NOT NULL COMMENT 'The reason why this account was banned.',
  `issued_by` varchar(255) DEFAULT NULL COMMENT 'Name of the person or system that issued this ban (not strongly typed at all).',
  `expired` tinyint(1) unsigned NOT NULL DEFAULT '0' COMMENT 'If the ban is expired. A non-zero value means true.',
  PRIMARY KEY (`id`),
  KEY `account_id` (`account_id`),
  KEY `expired` (`expired`),
  CONSTRAINT `account_ban_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The bans (active and inactive) placed on accounts.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_ban`
--

LOCK TABLES `account_ban` WRITE;
/*!40000 ALTER TABLE `account_ban` DISABLE KEYS */;
/*!40000 ALTER TABLE `account_ban` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `bi_account_ban_fer` BEFORE INSERT ON `account_ban` FOR EACH ROW BEGIN
	IF new.end_time <= NOW() THEN
		SET new.expired = 1;
	ELSE
		SET new.expired = 0;
	END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `bu_account_ban_fer` BEFORE UPDATE ON `account_ban` FOR EACH ROW BEGIN
	IF new.end_time <= NOW() THEN
		SET new.expired = 1;
	ELSE
		SET new.expired = 0;
	END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `account_character`
--

DROP TABLE IF EXISTS `account_character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account_character` (
  `character_id` int(11) NOT NULL COMMENT 'The character in the account.',
  `account_id` int(11) NOT NULL COMMENT 'The account the character is on.',
  `time_deleted` datetime DEFAULT NULL COMMENT 'When the character was removed from the account (NULL if not removed).',
  PRIMARY KEY (`character_id`),
  KEY `account_id` (`account_id`),
  CONSTRAINT `account_character_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `account_character_ibfk_2` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Links account to many characters. Retains deleted linkages.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_character`
--

LOCK TABLES `account_character` WRITE;
/*!40000 ALTER TABLE `account_character` DISABLE KEYS */;
INSERT INTO `account_character` VALUES (1,1,NULL);
/*!40000 ALTER TABLE `account_character` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_ips`
--

DROP TABLE IF EXISTS `account_ips`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account_ips` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'The unique row ID.',
  `account_id` int(11) NOT NULL COMMENT 'The ID of the account.',
  `ip` int(10) unsigned NOT NULL COMMENT 'The IP that logged into the account.',
  `time` datetime NOT NULL COMMENT 'When this IP last logged into this account.',
  PRIMARY KEY (`id`),
  KEY `account_id` (`account_id`),
  CONSTRAINT `account_ips_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The IPs used to access accounts.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_ips`
--

LOCK TABLES `account_ips` WRITE;
/*!40000 ALTER TABLE `account_ips` DISABLE KEYS */;
/*!40000 ALTER TABLE `account_ips` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `active_trade_cash`
--

DROP TABLE IF EXISTS `active_trade_cash`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `active_trade_cash` (
  `character_id` int(11) NOT NULL COMMENT 'The character that put the cash on the trade table.',
  `cash` int(11) NOT NULL COMMENT 'The amount of cash the character put down.',
  PRIMARY KEY (`character_id`),
  CONSTRAINT `active_trade_cash_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Cash that has been put down in an active trade.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `active_trade_cash`
--

LOCK TABLES `active_trade_cash` WRITE;
/*!40000 ALTER TABLE `active_trade_cash` DISABLE KEYS */;
/*!40000 ALTER TABLE `active_trade_cash` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `active_trade_item`
--

DROP TABLE IF EXISTS `active_trade_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `active_trade_item` (
  `item_id` int(11) NOT NULL COMMENT 'The ID of the item the character put down.',
  `character_id` int(11) NOT NULL COMMENT 'The character that added the item.',
  PRIMARY KEY (`item_id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `active_trade_item_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `active_trade_item_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items that have been put down in an active trade.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `active_trade_item`
--

LOCK TABLES `active_trade_item` WRITE;
/*!40000 ALTER TABLE `active_trade_item` DISABLE KEYS */;
/*!40000 ALTER TABLE `active_trade_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `alliance`
--

DROP TABLE IF EXISTS `alliance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `alliance` (
  `id` tinyint(3) unsigned NOT NULL COMMENT 'The unique ID of the alliance.',
  `name` varchar(255) NOT NULL DEFAULT '' COMMENT 'The name of the alliance.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The different character alliances.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `alliance_attackable` (
  `alliance_id` tinyint(3) unsigned NOT NULL COMMENT 'The alliance.',
  `attackable_id` tinyint(3) unsigned NOT NULL COMMENT 'The alliance that this alliance (alliance_id) can attack.',
  PRIMARY KEY (`alliance_id`,`attackable_id`),
  KEY `attackable_id` (`attackable_id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `alliance_attackable_ibfk_3` FOREIGN KEY (`attackable_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `alliance_attackable_ibfk_4` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='List of alliances that an alliance can attack.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `alliance_attackable`
--

LOCK TABLES `alliance_attackable` WRITE;
/*!40000 ALTER TABLE `alliance_attackable` DISABLE KEYS */;
INSERT INTO `alliance_attackable` VALUES (1,0),(3,0),(0,1),(3,1),(0,3),(3,3);
/*!40000 ALTER TABLE `alliance_attackable` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `alliance_hostile`
--

DROP TABLE IF EXISTS `alliance_hostile`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `alliance_hostile` (
  `alliance_id` tinyint(3) unsigned NOT NULL COMMENT 'The alliance that is hotile.',
  `hostile_id` tinyint(3) unsigned NOT NULL COMMENT 'The alliance that this alliance (alliance_id) is hostile towards by default.',
  PRIMARY KEY (`alliance_id`,`hostile_id`),
  KEY `hostile_id` (`hostile_id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `alliance_hostile_ibfk_3` FOREIGN KEY (`hostile_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `alliance_hostile_ibfk_4` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Alliances that an alliance is hostile towards by default.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `alliance_hostile`
--

LOCK TABLES `alliance_hostile` WRITE;
/*!40000 ALTER TABLE `alliance_hostile` DISABLE KEYS */;
INSERT INTO `alliance_hostile` VALUES (1,0),(3,0),(0,1),(3,1),(0,3),(3,3);
/*!40000 ALTER TABLE `alliance_hostile` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character`
--

DROP TABLE IF EXISTS `character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'The unique ID of the character.',
  `character_template_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The template that this character was created from (not required - mostly for developer reference).',
  `name` varchar(60) NOT NULL DEFAULT '' COMMENT 'The character''s name. Prefixed with `~<ID>_` when its a deleted user. The ~ denotes deleted, and the <ID> ensures a unique value.',
  `shop_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The shop that this character runs. Null if not a shopkeeper.',
  `chat_dialog` smallint(5) unsigned DEFAULT NULL COMMENT 'The chat dialog that this character displays. Null for no chat. Intended for NPCs only.',
  `ai_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The AI used by this character. Null for no AI (does nothing, or is user-controller). Intended for NPCs only.',
  `load_map_id` smallint(5) unsigned NOT NULL DEFAULT '1' COMMENT 'The map to load on (when logging in / being created).',
  `load_x` smallint(5) unsigned NOT NULL DEFAULT '512' COMMENT 'The x coordinate to load at.',
  `load_y` smallint(5) unsigned NOT NULL DEFAULT '512' COMMENT 'The y coordinate to load at.',
  `respawn_map_id` smallint(5) unsigned DEFAULT '1' COMMENT 'The map to respawn on (when null, cannot respawn). Used to reposition character after death.',
  `respawn_x` float NOT NULL DEFAULT '512' COMMENT 'The x coordinate to respawn at.',
  `respawn_y` float NOT NULL DEFAULT '512' COMMENT 'The y coordinate to respawn at.',
  `body_id` smallint(5) unsigned NOT NULL DEFAULT '1' COMMENT 'The body to use to display this character.',
  `move_speed` smallint(5) unsigned NOT NULL DEFAULT '1800' COMMENT 'The movement speed of the character.',
  `cash` int(11) NOT NULL DEFAULT '0' COMMENT 'Amount of cash.',
  `level` smallint(6) NOT NULL DEFAULT '1' COMMENT 'Current level.',
  `exp` int(11) NOT NULL DEFAULT '0' COMMENT 'Experience points.',
  `statpoints` int(11) NOT NULL DEFAULT '0' COMMENT 'Stat points available to be spent.',
  `hp` smallint(6) NOT NULL DEFAULT '50' COMMENT 'Current health points.',
  `mp` smallint(6) NOT NULL DEFAULT '50' COMMENT 'Current mana points.',
  `stat_maxhp` smallint(6) NOT NULL DEFAULT '50' COMMENT 'MaxHP stat.',
  `stat_maxmp` smallint(6) NOT NULL DEFAULT '50' COMMENT 'MaxMP stat.',
  `stat_minhit` smallint(6) NOT NULL DEFAULT '1' COMMENT 'MinHit stat.',
  `stat_maxhit` smallint(6) NOT NULL DEFAULT '1' COMMENT 'MaxHit stat.',
  `stat_defence` smallint(6) NOT NULL DEFAULT '1' COMMENT 'Defence stat.',
  `stat_agi` smallint(6) NOT NULL DEFAULT '1' COMMENT 'Agi stat.',
  `stat_int` smallint(6) NOT NULL DEFAULT '1' COMMENT 'Int stat.',
  `stat_str` smallint(6) NOT NULL DEFAULT '1' COMMENT 'Str stat.',
  PRIMARY KEY (`id`),
  UNIQUE KEY `idx_character_name` (`name`),
  KEY `character_ibfk_2` (`load_map_id`),
  KEY `shop_id` (`shop_id`),
  KEY `character_ibfk_5` (`respawn_map_id`),
  KEY `template_id` (`character_template_id`),
  CONSTRAINT `character_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_3` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_4` FOREIGN KEY (`load_map_id`) REFERENCES `map` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_5` FOREIGN KEY (`respawn_map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1 COMMENT='Persisted (users, persistent NPCs) chars.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character`
--

LOCK TABLES `character` WRITE;
/*!40000 ALTER TABLE `character` DISABLE KEYS */;
INSERT INTO `character` VALUES (1,NULL,'Spodi',NULL,NULL,NULL,3,424,366,1,512,512,1,1800,15,1,15,0,50,50,50,50,1,1,1,1,1,1);
/*!40000 ALTER TABLE `character` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_equipped`
--

DROP TABLE IF EXISTS `character_equipped`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_equipped` (
  `character_id` int(11) NOT NULL COMMENT 'The character who the equipped item is on.',
  `item_id` int(11) NOT NULL COMMENT 'The item that is equipped by the character.',
  `slot` tinyint(3) unsigned NOT NULL COMMENT 'The slot the equipped item is in.',
  PRIMARY KEY (`character_id`,`slot`),
  KEY `item_id` (`item_id`),
  CONSTRAINT `character_equipped_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_equipped_ibfk_4` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items a character has equipped.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_equipped`
--

LOCK TABLES `character_equipped` WRITE;
/*!40000 ALTER TABLE `character_equipped` DISABLE KEYS */;
INSERT INTO `character_equipped` VALUES (1,33,1),(1,35,2);
/*!40000 ALTER TABLE `character_equipped` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_inventory`
--

DROP TABLE IF EXISTS `character_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_inventory` (
  `character_id` int(11) NOT NULL COMMENT 'The character who has this item in their inventory.',
  `item_id` int(11) NOT NULL COMMENT 'The item that is in the character''s inventory.',
  `slot` tinyint(3) unsigned NOT NULL COMMENT 'The slot the item is in in the character''s inventory.',
  PRIMARY KEY (`character_id`,`slot`),
  KEY `item_id` (`item_id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `character_inventory_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_inventory_ibfk_4` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items in a character''s inventory.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_inventory`
--

LOCK TABLES `character_inventory` WRITE;
/*!40000 ALTER TABLE `character_inventory` DISABLE KEYS */;
INSERT INTO `character_inventory` VALUES (1,26,0),(1,27,1),(1,32,2);
/*!40000 ALTER TABLE `character_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_quest_status`
--

DROP TABLE IF EXISTS `character_quest_status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_quest_status` (
  `character_id` int(11) NOT NULL COMMENT 'Character this quest status info is for.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest this information is for.',
  `started_on` datetime NOT NULL COMMENT 'When the quest was started.',
  `completed_on` datetime DEFAULT NULL COMMENT 'When the quest was completed. Null if incomplete. Repeatable quests hold time is was most recently completed.',
  PRIMARY KEY (`character_id`,`quest_id`),
  KEY `quest_id` (`quest_id`),
  CONSTRAINT `character_quest_status_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_quest_status_ifk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Quest status for characters. Intended for users chars.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_quest_status_kills` (
  `character_id` int(11) NOT NULL COMMENT 'The character who is doing this quest.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that the kill count is for.',
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template that is to be killed for the quest.',
  `count` smallint(5) unsigned NOT NULL COMMENT 'The current kill count of characters with this template.',
  PRIMARY KEY (`character_id`,`quest_id`,`character_template_id`),
  KEY `quest_id` (`quest_id`),
  KEY `character_template_id` (`character_template_id`),
  CONSTRAINT `character_quest_status_kills_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_quest_status_kills_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_quest_status_kills_ibfk_3` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Kill counters for quests.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_quest_status_kills`
--

LOCK TABLES `character_quest_status_kills` WRITE;
/*!40000 ALTER TABLE `character_quest_status_kills` DISABLE KEYS */;
/*!40000 ALTER TABLE `character_quest_status_kills` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_skill`
--

DROP TABLE IF EXISTS `character_skill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_skill` (
  `character_id` int(11) NOT NULL COMMENT 'The character that knows the skill.',
  `skill_id` tinyint(5) unsigned NOT NULL COMMENT 'The skill the character knows.',
  `time_added` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'When this row was added.',
  PRIMARY KEY (`character_id`,`skill_id`),
  CONSTRAINT `character_skill_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Skills known by a character.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_skill`
--

LOCK TABLES `character_skill` WRITE;
/*!40000 ALTER TABLE `character_skill` DISABLE KEYS */;
INSERT INTO `character_skill` VALUES (1,0,'2012-12-15 06:18:10'),(1,1,'2012-12-15 06:18:11');
/*!40000 ALTER TABLE `character_skill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_status_effect`
--

DROP TABLE IF EXISTS `character_status_effect`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_status_effect` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Unique ID of the status effect instance.',
  `character_id` int(11) NOT NULL COMMENT 'ID of the Character that the status effect is on.',
  `status_effect_id` tinyint(3) unsigned NOT NULL COMMENT 'ID of the status effect that this effect is for. This corresponds to the StatusEffectType enum''s value.',
  `power` smallint(5) unsigned NOT NULL COMMENT 'The power of this status effect instance.',
  `time_left_secs` smallint(5) unsigned NOT NULL COMMENT 'The amount of time remaining for this status effect in seconds.',
  PRIMARY KEY (`id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `character_status_effect_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Active status effects on a character.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_template` (
  `id` smallint(5) unsigned NOT NULL COMMENT 'The unique ID of the character template.',
  `alliance_id` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT 'The alliance.',
  `name` varchar(50) NOT NULL DEFAULT 'New NPC' COMMENT 'Name of the template. NPCs usually use this name, while users usually have their own name and this value is just for dev reference.',
  `ai_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The AI (intended for NPCs only).',
  `shop_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The shop (intended for NPCs only).',
  `chat_dialog` smallint(5) unsigned DEFAULT NULL COMMENT 'The chat dialog (intended for NPCs only).',
  `body_id` smallint(5) unsigned NOT NULL DEFAULT '1' COMMENT 'The body to use.',
  `move_speed` smallint(5) unsigned NOT NULL DEFAULT '1800' COMMENT 'The movement speed.',
  `respawn` smallint(5) unsigned NOT NULL DEFAULT '5' COMMENT 'How long in seconds to wait after death to be respawned (intended for NPCs only).',
  `level` smallint(6) NOT NULL DEFAULT '1' COMMENT 'The character''s level.',
  `exp` int(11) NOT NULL DEFAULT '0' COMMENT 'Current exp.',
  `statpoints` int(11) NOT NULL DEFAULT '0' COMMENT 'Number of stat points available to spend.',
  `give_exp` int(11) NOT NULL DEFAULT '0' COMMENT 'Amount of exp to give when killed (intended for NPCs only).',
  `give_cash` int(11) NOT NULL DEFAULT '0' COMMENT 'Amount of cash to give when killed (intended for NPCs only).',
  `stat_maxhp` smallint(6) NOT NULL DEFAULT '50' COMMENT 'MaxHP stat.',
  `stat_maxmp` smallint(6) NOT NULL DEFAULT '50' COMMENT 'MaxMP stat.',
  `stat_minhit` smallint(6) NOT NULL DEFAULT '1' COMMENT 'MinHit stat.',
  `stat_maxhit` smallint(6) NOT NULL DEFAULT '1' COMMENT 'MaxHit stat.',
  `stat_defence` smallint(6) NOT NULL DEFAULT '1' COMMENT 'Defence stat.',
  `stat_agi` smallint(6) NOT NULL DEFAULT '1' COMMENT 'Agi stat.',
  `stat_int` smallint(6) NOT NULL DEFAULT '1' COMMENT 'Int stat.',
  `stat_str` smallint(6) NOT NULL DEFAULT '1' COMMENT 'Str stat.',
  PRIMARY KEY (`id`),
  KEY `alliance_id` (`alliance_id`),
  KEY `shop_id` (`shop_id`),
  CONSTRAINT `character_template_ibfk_2` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `character_template_ibfk_3` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Character templates (used to instantiate characters).';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_template`
--

LOCK TABLES `character_template` WRITE;
/*!40000 ALTER TABLE `character_template` DISABLE KEYS */;
INSERT INTO `character_template` VALUES (0,0,'User Template',NULL,NULL,NULL,1,1800,5,1,0,0,0,0,50,50,1,2,1,1,1,1),(1,1,'Bee',1,NULL,NULL,5,2500,10,1,0,0,5,5,5,5,1,2,1,1,1,1),(2,2,'Quest Giver',NULL,NULL,NULL,3,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(4,2,'Shopkeeper',NULL,0,NULL,3,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(5,2,'Inn Keeper',NULL,NULL,0,3,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(6,3,'Brawler',1,NULL,NULL,2,500,5,1,0,0,8,8,20,20,1,2,1,1,1,1);
/*!40000 ALTER TABLE `character_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_equipped`
--

DROP TABLE IF EXISTS `character_template_equipped`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_template_equipped` (
  `id` int(11) NOT NULL COMMENT 'The unique row ID.',
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item the character template has equipped.',
  `chance` smallint(5) unsigned NOT NULL COMMENT 'The chance of the item being equipped when a character is instantiated from this template.',
  PRIMARY KEY (`id`),
  KEY `item_id` (`item_template_id`),
  KEY `character_id` (`character_template_id`),
  CONSTRAINT `character_template_equipped_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_equipped_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Equipped items on a character template.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_template_equipped`
--

LOCK TABLES `character_template_equipped` WRITE;
/*!40000 ALTER TABLE `character_template_equipped` DISABLE KEYS */;
INSERT INTO `character_template_equipped` VALUES (0,1,7,30000),(1,1,5,2000),(2,1,3,2000),(3,6,8,65535);
/*!40000 ALTER TABLE `character_template_equipped` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_inventory`
--

DROP TABLE IF EXISTS `character_template_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_template_inventory` (
  `id` int(11) NOT NULL COMMENT 'The unique row ID.',
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item the character template has in their inventory.',
  `min` tinyint(3) unsigned NOT NULL COMMENT 'The minimum number of items to be created. Doesn''t affect item creation chance. Each value in range has equal distribution.',
  `max` tinyint(3) unsigned NOT NULL COMMENT 'The maximum number of items to be created.',
  `chance` smallint(5) unsigned NOT NULL COMMENT 'Chance that this item will be created when the character template is instantiated. Item quantity will be between min and max (equal chance distribution).',
  PRIMARY KEY (`id`),
  KEY `item_id` (`item_template_id`),
  KEY `character_id` (`character_template_id`),
  CONSTRAINT `character_template_inventory_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_inventory_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items in a character template''s inventory.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_template_inventory`
--

LOCK TABLES `character_template_inventory` WRITE;
/*!40000 ALTER TABLE `character_template_inventory` DISABLE KEYS */;
INSERT INTO `character_template_inventory` VALUES (0,1,5,0,2,10000),(1,6,4,1,1,66),(3,1,5,0,2,10000),(5,6,3,1,1,655),(6,6,8,1,1,655),(7,1,3,1,1,5000),(8,1,3,1,1,5000),(9,1,7,1,10,65535),(10,1,3,1,1,5000),(11,1,7,1,10,65535),(12,1,3,1,1,5000),(13,1,5,0,2,10000),(14,1,7,1,10,65535),(15,1,3,1,1,5000),(16,1,7,1,10,65535),(17,6,6,1,1,655),(18,6,7,1,1,655),(19,6,5,1,1,3277),(20,6,2,1,1,6554),(21,6,1,1,1,6554);
/*!40000 ALTER TABLE `character_template_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_quest_provider`
--

DROP TABLE IF EXISTS `character_template_quest_provider`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_template_quest_provider` (
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest provided by this character template. Only applies for valid quest givers (that is, not users).',
  PRIMARY KEY (`character_template_id`,`quest_id`),
  KEY `quest_id` (`quest_id`),
  CONSTRAINT `character_template_quest_provider_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_quest_provider_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Quests provided by character templates.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_template_quest_provider`
--

LOCK TABLES `character_template_quest_provider` WRITE;
/*!40000 ALTER TABLE `character_template_quest_provider` DISABLE KEYS */;
INSERT INTO `character_template_quest_provider` VALUES (2,0),(2,1);
/*!40000 ALTER TABLE `character_template_quest_provider` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_skill`
--

DROP TABLE IF EXISTS `character_template_skill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_template_skill` (
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template that knows the skill.',
  `skill_id` tinyint(5) unsigned NOT NULL COMMENT 'The skill the character template knows.',
  PRIMARY KEY (`character_template_id`,`skill_id`),
  CONSTRAINT `character_template_skill_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Skills known by a character template.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_template_skill`
--

LOCK TABLES `character_template_skill` WRITE;
/*!40000 ALTER TABLE `character_template_skill` DISABLE KEYS */;
INSERT INTO `character_template_skill` VALUES (0,0),(0,1),(2,0),(6,1);
/*!40000 ALTER TABLE `character_template_skill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_guild`
--

DROP TABLE IF EXISTS `event_counters_guild`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_counters_guild` (
  `guild_id` smallint(5) unsigned NOT NULL COMMENT 'The guild the event occured on.',
  `guild_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`guild_id`,`guild_event_counter_id`),
  CONSTRAINT `event_counters_guild_ibfk_1` FOREIGN KEY (`guild_id`) REFERENCES `guild` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for guilds.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event_counters_guild`
--

LOCK TABLES `event_counters_guild` WRITE;
/*!40000 ALTER TABLE `event_counters_guild` DISABLE KEYS */;
/*!40000 ALTER TABLE `event_counters_guild` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_item_template`
--

DROP TABLE IF EXISTS `event_counters_item_template`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_counters_item_template` (
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the item the event occured on.',
  `item_template_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`item_template_id`,`item_template_event_counter_id`),
  CONSTRAINT `event_counters_item_template_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for item templates.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event_counters_item_template`
--

LOCK TABLES `event_counters_item_template` WRITE;
/*!40000 ALTER TABLE `event_counters_item_template` DISABLE KEYS */;
/*!40000 ALTER TABLE `event_counters_item_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_map`
--

DROP TABLE IF EXISTS `event_counters_map`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_counters_map` (
  `map_id` smallint(5) unsigned NOT NULL COMMENT 'The map the event occured on.',
  `map_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`map_id`,`map_event_counter_id`),
  CONSTRAINT `event_counters_map_ibfk_1` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for maps.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event_counters_map`
--

LOCK TABLES `event_counters_map` WRITE;
/*!40000 ALTER TABLE `event_counters_map` DISABLE KEYS */;
/*!40000 ALTER TABLE `event_counters_map` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_npc`
--

DROP TABLE IF EXISTS `event_counters_npc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_counters_npc` (
  `npc_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template of the NPC the event occured on.',
  `npc_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`npc_template_id`,`npc_event_counter_id`),
  CONSTRAINT `event_counters_npc_ibfk_1` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for NPC templates.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event_counters_npc`
--

LOCK TABLES `event_counters_npc` WRITE;
/*!40000 ALTER TABLE `event_counters_npc` DISABLE KEYS */;
/*!40000 ALTER TABLE `event_counters_npc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_quest`
--

DROP TABLE IF EXISTS `event_counters_quest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_counters_quest` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest the event occured on.',
  `quest_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`quest_id`,`quest_event_counter_id`),
  CONSTRAINT `event_counters_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for quests.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event_counters_quest`
--

LOCK TABLES `event_counters_quest` WRITE;
/*!40000 ALTER TABLE `event_counters_quest` DISABLE KEYS */;
/*!40000 ALTER TABLE `event_counters_quest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_shop`
--

DROP TABLE IF EXISTS `event_counters_shop`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_counters_shop` (
  `shop_id` smallint(5) unsigned NOT NULL COMMENT 'The shop the event occured on.',
  `shop_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`shop_id`,`shop_event_counter_id`),
  CONSTRAINT `event_counters_shop_ibfk_1` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for shops.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event_counters_shop`
--

LOCK TABLES `event_counters_shop` WRITE;
/*!40000 ALTER TABLE `event_counters_shop` DISABLE KEYS */;
/*!40000 ALTER TABLE `event_counters_shop` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_user`
--

DROP TABLE IF EXISTS `event_counters_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_counters_user` (
  `user_id` int(11) NOT NULL COMMENT 'The character ID for the user character the event occured on.',
  `user_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`user_id`,`user_event_counter_id`),
  CONSTRAINT `event_counters_user_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for users.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event_counters_user`
--

LOCK TABLES `event_counters_user` WRITE;
/*!40000 ALTER TABLE `event_counters_user` DISABLE KEYS */;
/*!40000 ALTER TABLE `event_counters_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guild`
--

DROP TABLE IF EXISTS `guild`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `guild` (
  `id` smallint(5) unsigned NOT NULL AUTO_INCREMENT COMMENT 'The unique ID of the guild.',
  `name` varchar(50) NOT NULL COMMENT 'The name of the guild.',
  `tag` varchar(5) NOT NULL COMMENT 'The guild''s tag.',
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'When this guild was created.',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The active guilds.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `guild`
--

LOCK TABLES `guild` WRITE;
/*!40000 ALTER TABLE `guild` DISABLE KEYS */;
/*!40000 ALTER TABLE `guild` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guild_event`
--

DROP TABLE IF EXISTS `guild_event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
  CONSTRAINT `guild_event_ibfk_3` FOREIGN KEY (`target_character_id`) REFERENCES `character` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event log for guilds.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `guild_member` (
  `character_id` int(11) NOT NULL COMMENT 'The character that is a member of the guild.',
  `guild_id` smallint(5) unsigned NOT NULL COMMENT 'The guild the member is a part of.',
  `rank` tinyint(3) unsigned NOT NULL COMMENT 'The member''s ranking in the guild.',
  `joined` datetime NOT NULL COMMENT 'When the member joined the guild.',
  PRIMARY KEY (`character_id`),
  KEY `guild_id` (`guild_id`),
  CONSTRAINT `guild_member_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `guild_member_ibfk_2` FOREIGN KEY (`guild_id`) REFERENCES `guild` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The members of a guild.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `guild_member`
--

LOCK TABLES `guild_member` WRITE;
/*!40000 ALTER TABLE `guild_member` DISABLE KEYS */;
/*!40000 ALTER TABLE `guild_member` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item`
--

DROP TABLE IF EXISTS `item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `item` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'The unique ID of the item.',
  `item_template_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The template the item was created from. Not required. Mostly for development reference.',
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT 'The type of item (see ItemType enum).',
  `weapon_type` tinyint(3) unsigned NOT NULL COMMENT 'When used as a weapon, the type of weapon (see WeaponType enum).',
  `range` smallint(5) unsigned NOT NULL COMMENT 'The range of the item. Usually for attack range, but can depend on ItemType and/or WeaponType.',
  `width` tinyint(3) unsigned NOT NULL DEFAULT '16' COMMENT 'Width of the item in pixels. Mostly intended for when on a map. Usually set to the same size as the item''s sprite.',
  `height` tinyint(3) unsigned NOT NULL DEFAULT '16' COMMENT 'Height of the item in pixels. Mostly intended for when on a map. Usually set to the same size as the item''s sprite.',
  `name` varchar(255) NOT NULL COMMENT 'The name of the item.',
  `description` varchar(255) NOT NULL COMMENT 'The item''s textual description (don''t include stuff like stats).',
  `amount` tinyint(3) unsigned NOT NULL DEFAULT '1' COMMENT 'The quantity of the item (for stacked items). Stacks of items count as one single item instance with an amount greater than zero.',
  `graphic` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'The GrhData to use to display this item, both in GUI (inventory, equipped) and on the map.',
  `value` int(11) NOT NULL DEFAULT '0' COMMENT 'The base monetary value of the item.',
  `hp` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Amount of health gained from using this item (mostly for use-once items).',
  `mp` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Amount of mana gained from using this item (mostly for use-once items).',
  `stat_agi` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_int` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_str` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_minhit` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_maxhit` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_maxhp` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_maxmp` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_defence` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_req_agi` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Required amount of the corresponding stat to use this item.',
  `stat_req_int` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Required amount of the corresponding stat to use this item.',
  `stat_req_str` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Required amount of the corresponding stat to use this item.',
  `equipped_body` varchar(255) DEFAULT NULL COMMENT 'When equipped and not null, sets the character''s paper doll to include this layer.',
  `action_display_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ActionDisplayID to use when using this item (e.g. drink potion, attack with sword, etc).',
  PRIMARY KEY (`id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `item_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=latin1 COMMENT='The live, persisted items.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES (26,1,1,0,0,9,16,'Healing Potion','A healing potion',12,95,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(27,2,1,0,0,9,16,'Mana Potion','A mana potion',12,94,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(32,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',19,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(33,8,4,0,0,30,30,'Iron Armor','Body armor made out of iron.',1,236,50,0,0,1,0,0,0,0,0,0,6,0,0,0,'Body.Iron',NULL),(35,3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',1,237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL);
/*!40000 ALTER TABLE `item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item_template`
--

DROP TABLE IF EXISTS `item_template`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `item_template` (
  `id` smallint(5) unsigned NOT NULL COMMENT 'The unique ID of the item template.',
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT 'The type of item (see ItemType enum).',
  `weapon_type` tinyint(3) unsigned NOT NULL DEFAULT '0' COMMENT 'When used as a weapon, the type of weapon (see WeaponType enum).',
  `range` smallint(5) unsigned NOT NULL DEFAULT '10' COMMENT 'The range of the item. Usually for attack range, but can depend on ItemType and/or WeaponType.',
  `width` tinyint(3) unsigned NOT NULL DEFAULT '16' COMMENT 'Width of the item in pixels. Mostly intended for when on a map. Usually set to the same size as the item''s sprite.',
  `height` tinyint(3) unsigned NOT NULL DEFAULT '16' COMMENT 'Height of the item in pixels. Mostly intended for when on a map. Usually set to the same size as the item''s sprite.',
  `name` varchar(255) NOT NULL DEFAULT 'New item template' COMMENT 'The name of the item.',
  `description` varchar(255) NOT NULL DEFAULT ' ' COMMENT 'The item''s textual description (don''t include stuff like stats).',
  `graphic` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'The GrhData to use to display this item, both in GUI (inventory, equipped) and on the map.',
  `value` int(11) NOT NULL DEFAULT '0' COMMENT 'The base monetary value of the item.',
  `hp` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Amount of health gained from using this item (mostly for use-once items).',
  `mp` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Amount of mana gained from using this item (mostly for use-once items).',
  `stat_agi` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_int` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_str` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_minhit` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_maxhit` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_maxhp` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_maxmp` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_defence` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Stat modifier bonus. Use-once items often perminately increase this value, while equipped items provide a stat mod bonus.',
  `stat_req_agi` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Required amount of the corresponding stat to use this item.',
  `stat_req_int` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Required amount of the corresponding stat to use this item.',
  `stat_req_str` smallint(6) NOT NULL DEFAULT '0' COMMENT 'Required amount of the corresponding stat to use this item.',
  `equipped_body` varchar(255) DEFAULT NULL COMMENT 'When equipped and not null, sets the character''s paper doll to include this layer.',
  `action_display_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ActionDisplayID to use when using this item (e.g. drink potion, attack with sword, etc).',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPRESSED COMMENT='The templates used to instantiate items.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item_template`
--

LOCK TABLES `item_template` WRITE;
/*!40000 ALTER TABLE `item_template` DISABLE KEYS */;
INSERT INTO `item_template` VALUES (0,2,1,10,16,16,'Unarmed','Unarmed',1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(1,1,0,0,9,16,'Healing Potion','A healing potion',95,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(2,1,0,0,9,16,'Mana Potion','A mana potion',94,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL),(4,4,0,0,30,30,'Black Armor','Body armor made out of... black.',234,2000,0,0,5,5,5,5,5,0,0,20,0,0,0,'Body.Black',NULL),(5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL),(6,2,2,500,16,16,'Pistol','Just point it at whatever you want to die.',177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL,NULL),(7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(8,4,0,0,30,30,'Iron Armor','Body armor made out of iron.',236,50,0,0,1,0,0,0,0,0,0,6,0,0,0,'Body.Iron',NULL);
/*!40000 ALTER TABLE `item_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `map`
--

DROP TABLE IF EXISTS `map`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `map` (
  `id` smallint(5) unsigned NOT NULL COMMENT 'The unique ID of the map.',
  `name` varchar(255) NOT NULL COMMENT 'Name of the map.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Map meta-information.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `map`
--

LOCK TABLES `map` WRITE;
/*!40000 ALTER TABLE `map` DISABLE KEYS */;
INSERT INTO `map` VALUES (1,'Town'),(2,'Jail'),(3,'Shop');
/*!40000 ALTER TABLE `map` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `map_spawn`
--

DROP TABLE IF EXISTS `map_spawn`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `map_spawn` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'The unique ID of this NPC spawn.',
  `map_id` smallint(5) unsigned NOT NULL COMMENT 'The map that this spawn takes place on.',
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template used to instantiate the spawned NPCs.',
  `amount` tinyint(3) unsigned NOT NULL COMMENT 'The total number of NPCs this spawner will spawn.',
  `x` smallint(5) unsigned DEFAULT NULL COMMENT 'The x coordinate of the spawner (NULL indicates the left-most side of the map). Example: All x/y/width/height set to NULL spawns NPCs anywhere on the map.',
  `y` smallint(5) unsigned DEFAULT NULL COMMENT 'The y coordinate of the spawner (NULL indicates the top-most side of the map).',
  `width` smallint(5) unsigned DEFAULT NULL COMMENT 'The width of the spawner (NULL indicates the right-most side of the map).',
  `height` smallint(5) unsigned DEFAULT NULL COMMENT 'The height of the spawner (NULL indicates the bottom- side of the map).',
  PRIMARY KEY (`id`),
  KEY `character_id` (`character_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `map_spawn_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `map_spawn_ibfk_2` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1 COMMENT='NPC spawns for the maps.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `map_spawn`
--

LOCK TABLES `map_spawn` WRITE;
/*!40000 ALTER TABLE `map_spawn` DISABLE KEYS */;
INSERT INTO `map_spawn` VALUES (5,2,6,45,130,165,720,475),(9,1,1,3,NULL,NULL,NULL,NULL),(11,1,2,1,600,580,32,32),(16,3,4,1,250,250,1,1),(17,3,5,1,678,253,1,1);
/*!40000 ALTER TABLE `map_spawn` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest`
--

DROP TABLE IF EXISTS `quest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest` (
  `id` smallint(5) unsigned NOT NULL COMMENT 'The unique ID of the quest. Note: This table is like a template. Quest and character_quest_status are like character_template and character, respectively.',
  `repeatable` tinyint(1) unsigned NOT NULL DEFAULT '0' COMMENT 'If this quest can be repeated by a character after they have completed it.',
  `reward_cash` int(11) NOT NULL DEFAULT '0' COMMENT 'The base cash reward for completing this quest.',
  `reward_exp` int(11) NOT NULL DEFAULT '0' COMMENT 'The base experience reward for completing this quest.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The quests.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_require_finish_item` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that this requirement is for.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the item that is required for this quest to be finished.',
  `amount` tinyint(3) unsigned NOT NULL COMMENT 'The amount of the item required to finish this quest.',
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_require_finish_item_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_finish_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items required for finishing a quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_require_finish_quest` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that this requirement is for.',
  `req_quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest required to be finished before this quest can be finished.',
  PRIMARY KEY (`quest_id`,`req_quest_id`),
  KEY `req_quest_id` (`req_quest_id`),
  CONSTRAINT `quest_require_finish_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_finish_quest_ibfk_2` FOREIGN KEY (`req_quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Quests required to be finished before this quest is finished';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_require_kill` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that this requirement is for.',
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the characters that must be killed to complete this quest.',
  `amount` smallint(5) unsigned NOT NULL COMMENT 'The number of characters that must be killed to complete this quest.',
  PRIMARY KEY (`quest_id`,`character_template_id`),
  KEY `character_template_id` (`character_template_id`),
  CONSTRAINT `quest_require_kill_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_kill_ibfk_2` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Kill requirements to finish a quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_require_kill`
--

LOCK TABLES `quest_require_kill` WRITE;
/*!40000 ALTER TABLE `quest_require_kill` DISABLE KEYS */;
INSERT INTO `quest_require_kill` VALUES (0,1,5),(1,6,10);
/*!40000 ALTER TABLE `quest_require_kill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_require_start_item`
--

DROP TABLE IF EXISTS `quest_require_start_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_require_start_item` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'Quest that this requirement is for.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the item that is required to start the quest.',
  `amount` tinyint(3) unsigned NOT NULL COMMENT 'The amount of the item that is required.',
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_require_start_item_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_start_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items required to start a quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_require_start_quest` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that this requirement is for.',
  `req_quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that is required to be finished before this quest can be started.',
  PRIMARY KEY (`quest_id`,`req_quest_id`),
  KEY `req_quest_id` (`req_quest_id`),
  CONSTRAINT `quest_require_start_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_start_quest_ibfk_2` FOREIGN KEY (`req_quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Quests required to be finished to start this quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_reward_item` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that this completion reward is for.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the item to give as the reward.',
  `amount` tinyint(3) unsigned NOT NULL COMMENT 'The amount of the item to give (should be greater than 0).',
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_reward_item_ibfk_3` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_reward_item_ibfk_4` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items given as reward for finishing quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_reward_item`
--

LOCK TABLES `quest_reward_item` WRITE;
/*!40000 ALTER TABLE `quest_reward_item` DISABLE KEYS */;
INSERT INTO `quest_reward_item` VALUES (0,3,1),(1,3,1);
/*!40000 ALTER TABLE `quest_reward_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `server_time`
--

DROP TABLE IF EXISTS `server_time`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `server_time` (
  `server_time` datetime NOT NULL COMMENT 'The current time of the server, as seen by the server process. Only updated when server is running. Especially intended for when comparing the time to the server''s current time. Slightly low resolution (assume ~10 seconds).'
) ENGINE=MyISAM DEFAULT CHARSET=latin1 MAX_ROWS=1 ROW_FORMAT=FIXED COMMENT='Holds the current time of the server.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `server_time`
--

LOCK TABLES `server_time` WRITE;
/*!40000 ALTER TABLE `server_time` DISABLE KEYS */;
INSERT INTO `server_time` VALUES ('2012-12-14 19:25:10');
/*!40000 ALTER TABLE `server_time` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shop`
--

DROP TABLE IF EXISTS `shop`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `shop` (
  `id` smallint(5) unsigned NOT NULL COMMENT 'The unique ID of the shop.',
  `name` varchar(60) NOT NULL COMMENT 'The name of this shop.',
  `can_buy` tinyint(1) NOT NULL COMMENT 'Whether or not this shop can buy items from shoppers. When false, the shop only sells items (users cannot sell to it).',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The shops.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shop`
--

LOCK TABLES `shop` WRITE;
/*!40000 ALTER TABLE `shop` DISABLE KEYS */;
INSERT INTO `shop` VALUES (0,'Test Shop',1);
/*!40000 ALTER TABLE `shop` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shop_item`
--

DROP TABLE IF EXISTS `shop_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `shop_item` (
  `shop_id` smallint(5) unsigned NOT NULL COMMENT 'The shop that the item is for.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item template that this shop sells. Item instantiated when sold to shopper.',
  PRIMARY KEY (`shop_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `shop_item_ibfk_1` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `shop_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The items in a shop''s inventory.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shop_item`
--

LOCK TABLES `shop_item` WRITE;
/*!40000 ALTER TABLE `shop_item` DISABLE KEYS */;
INSERT INTO `shop_item` VALUES (0,1),(0,2),(0,3),(0,5),(0,6),(0,7);
/*!40000 ALTER TABLE `shop_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary table structure for view `view_npc_character`
--

DROP TABLE IF EXISTS `view_npc_character`;
/*!50001 DROP VIEW IF EXISTS `view_npc_character`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `view_npc_character` (
  `id` tinyint NOT NULL,
  `character_template_id` tinyint NOT NULL,
  `name` tinyint NOT NULL,
  `shop_id` tinyint NOT NULL,
  `chat_dialog` tinyint NOT NULL,
  `ai_id` tinyint NOT NULL,
  `load_map_id` tinyint NOT NULL,
  `load_x` tinyint NOT NULL,
  `load_y` tinyint NOT NULL,
  `respawn_map_id` tinyint NOT NULL,
  `respawn_x` tinyint NOT NULL,
  `respawn_y` tinyint NOT NULL,
  `body_id` tinyint NOT NULL,
  `move_speed` tinyint NOT NULL,
  `cash` tinyint NOT NULL,
  `level` tinyint NOT NULL,
  `exp` tinyint NOT NULL,
  `statpoints` tinyint NOT NULL,
  `hp` tinyint NOT NULL,
  `mp` tinyint NOT NULL,
  `stat_maxhp` tinyint NOT NULL,
  `stat_maxmp` tinyint NOT NULL,
  `stat_minhit` tinyint NOT NULL,
  `stat_maxhit` tinyint NOT NULL,
  `stat_defence` tinyint NOT NULL,
  `stat_agi` tinyint NOT NULL,
  `stat_int` tinyint NOT NULL,
  `stat_str` tinyint NOT NULL
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Temporary table structure for view `view_user_character`
--

DROP TABLE IF EXISTS `view_user_character`;
/*!50001 DROP VIEW IF EXISTS `view_user_character`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `view_user_character` (
  `id` tinyint NOT NULL,
  `character_template_id` tinyint NOT NULL,
  `name` tinyint NOT NULL,
  `shop_id` tinyint NOT NULL,
  `chat_dialog` tinyint NOT NULL,
  `ai_id` tinyint NOT NULL,
  `load_map_id` tinyint NOT NULL,
  `load_x` tinyint NOT NULL,
  `load_y` tinyint NOT NULL,
  `respawn_map_id` tinyint NOT NULL,
  `respawn_x` tinyint NOT NULL,
  `respawn_y` tinyint NOT NULL,
  `body_id` tinyint NOT NULL,
  `move_speed` tinyint NOT NULL,
  `cash` tinyint NOT NULL,
  `level` tinyint NOT NULL,
  `exp` tinyint NOT NULL,
  `statpoints` tinyint NOT NULL,
  `hp` tinyint NOT NULL,
  `mp` tinyint NOT NULL,
  `stat_maxhp` tinyint NOT NULL,
  `stat_maxmp` tinyint NOT NULL,
  `stat_minhit` tinyint NOT NULL,
  `stat_maxhit` tinyint NOT NULL,
  `stat_defence` tinyint NOT NULL,
  `stat_agi` tinyint NOT NULL,
  `stat_int` tinyint NOT NULL,
  `stat_str` tinyint NOT NULL
) ENGINE=MyISAM */;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `world_stats_count_consume_item`
--

DROP TABLE IF EXISTS `world_stats_count_consume_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_count_consume_item` (
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item template the counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'Number of times items of this template have been consumed.',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_consume_item_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Counts number of time an use-once item has been consumed.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_count_consume_item`
--

LOCK TABLES `world_stats_count_consume_item` WRITE;
/*!40000 ALTER TABLE `world_stats_count_consume_item` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_count_consume_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_item_buy`
--

DROP TABLE IF EXISTS `world_stats_count_item_buy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_count_item_buy` (
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the item that this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'The amount of this item that has been purchased from shops. When buying in bulk, this still updates by amount bought (so number of items purchased, not individual transactions).',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_item_buy_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Counts times an item has been purchased from a shop.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_count_item_buy`
--

LOCK TABLES `world_stats_count_item_buy` WRITE;
/*!40000 ALTER TABLE `world_stats_count_item_buy` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_count_item_buy` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_item_create`
--

DROP TABLE IF EXISTS `world_stats_count_item_create`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_count_item_create` (
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item template this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'The total number of times this item has been instantiated. When instantiating multiple items at once, this is incremented by the amount of the item, not just one.',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_item_create_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Counts number of times an item has been instantiated.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_count_item_create`
--

LOCK TABLES `world_stats_count_item_create` WRITE;
/*!40000 ALTER TABLE `world_stats_count_item_create` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_count_item_create` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_item_sell`
--

DROP TABLE IF EXISTS `world_stats_count_item_sell`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_count_item_sell` (
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item template this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'Amount of this item template that has been sold to stores.',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_item_sell_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Counts number of times shopper has sold item to store.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_count_item_sell`
--

LOCK TABLES `world_stats_count_item_sell` WRITE;
/*!40000 ALTER TABLE `world_stats_count_item_sell` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_count_item_sell` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_npc_kill_user`
--

DROP TABLE IF EXISTS `world_stats_count_npc_kill_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_count_npc_kill_user` (
  `user_id` int(11) NOT NULL COMMENT 'The character this counter is for (logically, should be a user).',
  `npc_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'The number of times NPCs of this character template have killed the user.',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`user_id`,`npc_template_id`),
  KEY `npc_template_id` (`npc_template_id`),
  CONSTRAINT `world_stats_count_npc_kill_user_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_count_npc_kill_user_ibfk_2` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Counts times a NPC has killed a user.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_count_npc_kill_user`
--

LOCK TABLES `world_stats_count_npc_kill_user` WRITE;
/*!40000 ALTER TABLE `world_stats_count_npc_kill_user` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_count_npc_kill_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_shop_buy`
--

DROP TABLE IF EXISTS `world_stats_count_shop_buy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_count_shop_buy` (
  `shop_id` smallint(5) unsigned NOT NULL COMMENT 'The shop this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'The number of times this shop has sold (shopper has bought from this shop).',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`shop_id`),
  CONSTRAINT `world_stats_count_shop_buy_ibfk_2` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Counts number of items a shop has sold to shopper.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_count_shop_buy`
--

LOCK TABLES `world_stats_count_shop_buy` WRITE;
/*!40000 ALTER TABLE `world_stats_count_shop_buy` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_count_shop_buy` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_shop_sell`
--

DROP TABLE IF EXISTS `world_stats_count_shop_sell`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_count_shop_sell` (
  `shop_id` smallint(5) unsigned NOT NULL COMMENT 'The shop this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'The number of times this shop has purchased items (shopper has sold to this shop).',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`shop_id`),
  CONSTRAINT `world_stats_count_shop_sell_ibfk_2` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Counts number of items a shop has purchased from shopper.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_count_shop_sell`
--

LOCK TABLES `world_stats_count_shop_sell` WRITE;
/*!40000 ALTER TABLE `world_stats_count_shop_sell` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_count_shop_sell` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_user_consume_item`
--

DROP TABLE IF EXISTS `world_stats_count_user_consume_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_count_user_consume_item` (
  `user_id` int(11) NOT NULL COMMENT 'Character this counter is for. Logically, it should be a user (not persistent NPC).',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item template that this consumption counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'The amount of the item that the user has consumed.',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`user_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `world_stats_count_user_consume_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_count_user_consume_item_ibfk_3` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Counts number of times user has consumed item (use-once).';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_count_user_consume_item`
--

LOCK TABLES `world_stats_count_user_consume_item` WRITE;
/*!40000 ALTER TABLE `world_stats_count_user_consume_item` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_count_user_consume_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_user_kill_npc`
--

DROP TABLE IF EXISTS `world_stats_count_user_kill_npc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_count_user_kill_npc` (
  `user_id` int(11) NOT NULL COMMENT 'The user that this kill counter is for.',
  `npc_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template that this NPC kill counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'Total number of NPCs killed by this user.',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`user_id`,`npc_template_id`),
  KEY `npc_template_id` (`npc_template_id`),
  CONSTRAINT `world_stats_count_user_kill_npc_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_count_user_kill_npc_ibfk_2` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Counts times a user has killed a NPC.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_count_user_kill_npc`
--

LOCK TABLES `world_stats_count_user_kill_npc` WRITE;
/*!40000 ALTER TABLE `world_stats_count_user_kill_npc` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_count_user_kill_npc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_guild_user_change`
--

DROP TABLE IF EXISTS `world_stats_guild_user_change`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_guild_user_change` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user who changed the guild they are part of.',
  `guild_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the guild, or null if the user left a guild.',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  KEY `guild_id` (`guild_id`),
  CONSTRAINT `world_stats_guild_user_change_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_guild_user_change_ibfk_2` FOREIGN KEY (`guild_id`) REFERENCES `guild` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Log of guild member join/leave events.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_network` (
  `id` mediumint(8) unsigned NOT NULL AUTO_INCREMENT,
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The time the snapshot took place.',
  `connections` smallint(5) unsigned NOT NULL COMMENT 'Number of connections to the server at the time of the snapshot.',
  `recv_bytes` mediumint(8) unsigned NOT NULL COMMENT 'The average bytes received per second since the last snapshot.',
  `recv_packets` mediumint(8) unsigned NOT NULL COMMENT 'The average packets received per second since the last snapshot.',
  `recv_messages` mediumint(8) unsigned NOT NULL COMMENT 'The average messages received per second since the last snapshot.',
  `sent_bytes` mediumint(8) unsigned NOT NULL COMMENT 'The average bytes sent per second since the last snapshot.',
  `sent_packets` mediumint(8) unsigned NOT NULL COMMENT 'The average packets sent per second since the last snapshot.',
  `sent_messages` mediumint(8) unsigned NOT NULL COMMENT 'The average messages sent per second since the last snapshot.',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COMMENT='Snapshots of network deltas.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_network`
--

LOCK TABLES `world_stats_network` WRITE;
/*!40000 ALTER TABLE `world_stats_network` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_network` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_npc_kill_user`
--

DROP TABLE IF EXISTS `world_stats_npc_kill_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_npc_kill_user` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user.',
  `npc_template_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The template ID of the NPC. Only valid when the NPC has a template ID set.',
  `user_level` smallint(6) NOT NULL COMMENT 'The level of the user was when this event took place.',
  `user_x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place.',
  `user_y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place.',
  `npc_x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.',
  `npc_y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  KEY `npc_template_id` (`npc_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_npc_kill_user_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_npc_kill_user_ibfk_2` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_npc_kill_user_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event log: NPC kill user.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_quest_accept` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user that accepted the quest.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that was accepted.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  KEY `quest_id` (`quest_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_quest_accept_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_accept_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_accept_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Event log: User accepts a quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_quest_cancel` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user that canceled the quest.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that was canceled.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  KEY `quest_id` (`quest_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_quest_cancel_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_cancel_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_cancel_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Event log: User cancels an accepted quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_quest_complete` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user that completed the quest.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that was completed.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  KEY `quest_id` (`quest_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_quest_complete_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_complete_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_quest_complete_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event log: User completes a quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_user_consume_item` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The user that this event is related to.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template ID of the item that was consumed. Only valid when the item has a set template ID.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The map the user was on when this event took place.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place.',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  KEY `item_template_id` (`item_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_user_consume_item_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_consume_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_consume_item_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event log: User consumes use-once item.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_user_kill_npc` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user.',
  `npc_template_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The template ID of the NPC. Only valid when the NPC has a template ID set.',
  `user_level` smallint(6) NOT NULL COMMENT 'The level of the user was when this event took place.',
  `user_x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `user_y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `npc_x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.',
  `npc_y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  KEY `npc_template_id` (`npc_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_user_kill_npc_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_kill_npc_ibfk_2` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_kill_npc_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event log: User kills NPC.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_user_level` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `character_id` int(11) NOT NULL COMMENT 'The ID of the character that leveled up.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `level` smallint(6) NOT NULL COMMENT 'The level that the character leveled up to (their new level).',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event log: User levels up.';
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `world_stats_user_shopping` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `shop_id` smallint(5) unsigned NOT NULL COMMENT 'The ID of the shop the event took place at.',
  `character_id` int(11) NOT NULL COMMENT 'The ID of the character that performed this transaction with the shop.',
  `item_template_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the item template that the event relates to. Only valid when the item involved has a set item template ID.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map the event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map X coordinate of the shopper when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map Y coordinate of the shopper when this event took place. Only valid when the map_id is not null.',
  `cost` int(11) NOT NULL COMMENT 'The amount of money that was involved in this transaction (how much the shopper sold the items for, or how much they bought the items for). ',
  `amount` tinyint(3) unsigned NOT NULL COMMENT 'The number of items involved in the transaction. Should always be greater than 0, and should only be greater for 1 for items that can stack.',
  `sale_type` tinyint(4) NOT NULL COMMENT 'Whether the shop sold to the user, or vise versa. If 0, the shop sold an item to the shopper. If non-zero, the shopper sold an item to a shop.',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
  KEY `shop_id` (`shop_id`),
  KEY `character_id` (`character_id`),
  KEY `item_template_id` (`item_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_user_shopping_ibfk_1` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_shopping_ibfk_2` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_shopping_ibfk_3` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_user_shopping_ibfk_4` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event log: User buys from or sells to shop.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_user_shopping`
--

LOCK TABLES `world_stats_user_shopping` WRITE;
/*!40000 ALTER TABLE `world_stats_user_shopping` DISABLE KEYS */;
/*!40000 ALTER TABLE `world_stats_user_shopping` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'demogame_tmp'
--
/*!50003 DROP FUNCTION IF EXISTS `create_user_on_account` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `create_user_on_account`(accountName VARCHAR(50), characterName VARCHAR(30)) RETURNS varchar(100) CHARSET latin1
BEGIN				DECLARE character_count INT DEFAULT 0;		DECLARE max_character_count INT DEFAULT 9;		DECLARE is_name_free INT DEFAULT 0;		DECLARE errorMsg VARCHAR(100) DEFAULT "";		DECLARE accountID INT DEFAULT NULL;		DECLARE charID INT DEFAULT 0;		SELECT `id` INTO accountID FROM `account` WHERE `name` = accountName;		IF ISNULL(accountID) THEN			SET errorMsg = "Account with the specified name does not exist.";		ELSE			SELECT COUNT(*) INTO character_count FROM `account_character` WHERE `account_id` = accountID;			IF character_count > max_character_count THEN				SET errorMsg = "No free character slots available in the account.";			ELSE				SELECT COUNT(*) INTO is_name_free FROM `character` WHERE `name` = characterName LIMIT 1;				IF is_name_free > 0 THEN					SET errorMsg = "The specified character name is not available for use.";				ELSE					INSERT INTO `character` SET `name`	= characterName;					SET charID = LAST_INSERT_ID();					INSERT INTO `account_character` SET `character_id` = charID, `account_id` = accountID;				END IF;			END IF;		END IF;						RETURN errorMsg;  END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `ft_banning_isbanned` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `ft_banning_isbanned`(accountID INT) RETURNS int(11)
BEGIN
		DECLARE cnt INT DEFAULT 0;
		DECLARE tnow TIMESTAMP;

		SET tnow = NOW();

		UPDATE `account_ban`
			SET `expired` = 1
			WHERE `expired` = 0
				AND `account_id` = accountID
				AND `end_time` <= tnow;
		
		SELECT COUNT(*)
			INTO cnt
			FROM `account_ban`
			WHERE `expired` = 0
				AND `account_id` = accountID;
				
		RETURN cnt;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `delete_user_on_account` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `delete_user_on_account`(characterID INT)
BEGIN

	UPDATE `account_character`
		SET `time_deleted` = NOW()
		WHERE `character_id` = characterID
		AND `time_deleted` IS NULL;

	UPDATE `character`
		SET `name` = CONCAT('~',`id`,'_',name)
		WHERE `id` = characterID
			AND SUBSTR(`name`, 1) != '~';

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `find_foreign_keys` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `find_foreign_keys`(tableSchema VARCHAR(100), tableName VARCHAR(100), columnName VARCHAR(100))
BEGIN

		SELECT `TABLE_SCHEMA`, `TABLE_NAME`, `COLUMN_NAME`
			FROM information_schema.KEY_COLUMN_USAGE
			WHERE `REFERENCED_TABLE_SCHEMA` = tableSchema
				AND `REFERENCED_TABLE_NAME` = tableName
				AND `REFERENCED_COLUMN_NAME` = columnName;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `ft_banning_get_reasons` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `ft_banning_get_reasons`(accountID INT)
BEGIN
		DECLARE tnow TIMESTAMP;

		SET tnow = NOW();

		UPDATE `account_ban`
			SET `expired` = 1
			WHERE `expired` = 0
				AND `account_id` = accountID
				AND `end_time` <= tnow;
		
		SELECT GROUP_CONCAT(DISTINCT `reason` SEPARATOR '\n\r') AS 'reasons',
				ROUND(TIME_TO_SEC(TIMEDIFF(MAX(`end_time`), NOW())) / 60) AS 'mins_left'
			FROM `account_ban`
			WHERE `account_id` = accountID
				AND	`expired` = 0;
		
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `ft_banning_update_expired` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `ft_banning_update_expired`()
BEGIN
		DECLARE tnow TIMESTAMP;
		
		SET tnow = NOW();
		
		UPDATE `account_ban`
			SET `expired` = 1
			WHERE `expired` = 0
				AND `end_time` <= tnow;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rebuild_views` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `rebuild_views`()
BEGIN
	
	CALL rebuild_view_npc_character();
	CALL rebuild_view_user_character();
    
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rebuild_view_npc_character` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `rebuild_view_npc_character`()
BEGIN
	
	DROP VIEW IF EXISTS `view_npc_character`;
	CREATE ALGORITHM=UNDEFINED DEFINER=CURRENT_USER SQL SECURITY DEFINER VIEW `view_npc_character` AS SELECT c.*  FROM `character` c LEFT JOIN `account_character` a ON c.id = a.character_id WHERE a.account_id IS NULL;
    
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rebuild_view_user_character` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `rebuild_view_user_character`()
BEGIN
	
	DROP VIEW IF EXISTS `view_user_character`;
	CREATE ALGORITHM=UNDEFINED DEFINER=CURRENT_USER SQL SECURITY DEFINER VIEW `view_user_character` AS SELECT c.* FROM `character` c INNER JOIN `account_character` a ON c.id = a.character_id WHERE a.time_deleted IS NULL;
    
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `view_npc_character`
--

/*!50001 DROP TABLE IF EXISTS `view_npc_character`*/;
/*!50001 DROP VIEW IF EXISTS `view_npc_character`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_npc_character` AS select `c`.`id` AS `id`,`c`.`character_template_id` AS `character_template_id`,`c`.`name` AS `name`,`c`.`shop_id` AS `shop_id`,`c`.`chat_dialog` AS `chat_dialog`,`c`.`ai_id` AS `ai_id`,`c`.`load_map_id` AS `load_map_id`,`c`.`load_x` AS `load_x`,`c`.`load_y` AS `load_y`,`c`.`respawn_map_id` AS `respawn_map_id`,`c`.`respawn_x` AS `respawn_x`,`c`.`respawn_y` AS `respawn_y`,`c`.`body_id` AS `body_id`,`c`.`move_speed` AS `move_speed`,`c`.`cash` AS `cash`,`c`.`level` AS `level`,`c`.`exp` AS `exp`,`c`.`statpoints` AS `statpoints`,`c`.`hp` AS `hp`,`c`.`mp` AS `mp`,`c`.`stat_maxhp` AS `stat_maxhp`,`c`.`stat_maxmp` AS `stat_maxmp`,`c`.`stat_minhit` AS `stat_minhit`,`c`.`stat_maxhit` AS `stat_maxhit`,`c`.`stat_defence` AS `stat_defence`,`c`.`stat_agi` AS `stat_agi`,`c`.`stat_int` AS `stat_int`,`c`.`stat_str` AS `stat_str` from (`character` `c` left join `account_character` `a` on((`c`.`id` = `a`.`character_id`))) where isnull(`a`.`account_id`) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_user_character`
--

/*!50001 DROP TABLE IF EXISTS `view_user_character`*/;
/*!50001 DROP VIEW IF EXISTS `view_user_character`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_user_character` AS select `c`.`id` AS `id`,`c`.`character_template_id` AS `character_template_id`,`c`.`name` AS `name`,`c`.`shop_id` AS `shop_id`,`c`.`chat_dialog` AS `chat_dialog`,`c`.`ai_id` AS `ai_id`,`c`.`load_map_id` AS `load_map_id`,`c`.`load_x` AS `load_x`,`c`.`load_y` AS `load_y`,`c`.`respawn_map_id` AS `respawn_map_id`,`c`.`respawn_x` AS `respawn_x`,`c`.`respawn_y` AS `respawn_y`,`c`.`body_id` AS `body_id`,`c`.`move_speed` AS `move_speed`,`c`.`cash` AS `cash`,`c`.`level` AS `level`,`c`.`exp` AS `exp`,`c`.`statpoints` AS `statpoints`,`c`.`hp` AS `hp`,`c`.`mp` AS `mp`,`c`.`stat_maxhp` AS `stat_maxhp`,`c`.`stat_maxmp` AS `stat_maxmp`,`c`.`stat_minhit` AS `stat_minhit`,`c`.`stat_maxhit` AS `stat_maxhit`,`c`.`stat_defence` AS `stat_defence`,`c`.`stat_agi` AS `stat_agi`,`c`.`stat_int` AS `stat_int`,`c`.`stat_str` AS `stat_str` from (`character` `c` join `account_character` `a` on((`c`.`id` = `a`.`character_id`))) where isnull(`a`.`time_deleted`) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-12-14 22:18:32
