-- MySQL dump 10.13  Distrib 5.1.51, for Win64 (unknown)
--
-- Host: localhost    Database: demogame
-- ------------------------------------------------------
-- Server version	5.1.51-community

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
  `id` int(11) NOT NULL COMMENT 'The unique ID of the account.',
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The user accounts. Multiple chars can exist per account.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account`
--

LOCK TABLES `account` WRITE;
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
INSERT INTO `account` VALUES (0,'Test','3fc0a7acf087f549ac2b266baf94b8b1','test@test.com',255,'2010-02-11 17:52:28','2010-02-11 18:03:56',16777343,NULL),(1,'Spodi','3fc0a7acf087f549ac2b266baf94b8b1','spodi@netgore.com',255,'2009-09-07 15:43:16','2010-11-18 14:21:24',16777343,NULL),(2,'Skye','74B87337454200D4D33F80C4663DC5E5','skye@test.com',0,'2010-11-11 15:23:57','2010-11-11 15:25:53',16777343,NULL),(3,'Omnio','74B87337454200D4D33F80C4663DC5E5','omnio@test.com',0,'2010-11-11 15:24:07','2010-11-11 15:24:07',16777343,NULL),(4,'Cruzn','74B87337454200D4D33F80C4663DC5E5','cruzn@test.com',0,'2010-11-11 15:24:13','2010-11-11 15:24:13',16777343,NULL),(5,'Darkfrost','74B87337454200D4D33F80C4663DC5E5','df@test.com',0,'2010-11-11 15:24:36','2010-11-11 15:26:56',16777343,NULL);
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
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`%`*/ /*!50003 TRIGGER `bi_account_ban_fer` BEFORE INSERT ON `account_ban` FOR EACH ROW BEGIN
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
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`%`*/ /*!50003 TRIGGER `bu_account_ban_fer` BEFORE UPDATE ON `account_ban` FOR EACH ROW BEGIN
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
INSERT INTO `account_character` VALUES (0,0,NULL),(1,1,NULL),(2,1,NULL),(3,1,NULL),(4,2,NULL),(5,2,NULL),(6,2,NULL),(7,5,NULL),(8,5,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=latin1 COMMENT='The IPs used to access accounts.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_ips`
--

LOCK TABLES `account_ips` WRITE;
/*!40000 ALTER TABLE `account_ips` DISABLE KEYS */;
INSERT INTO `account_ips` VALUES (2,1,16777343,'2010-11-10 10:18:11'),(3,1,16777343,'2010-11-10 10:18:40'),(4,1,16777343,'2010-11-10 10:24:05'),(5,1,16777343,'2010-11-10 10:30:10'),(6,1,16777343,'2010-11-10 10:39:11'),(7,1,16777343,'2010-11-10 11:33:55'),(8,1,16777343,'2010-11-10 11:36:38'),(9,1,16777343,'2010-11-11 15:23:16'),(10,1,16777343,'2010-11-11 15:25:16'),(11,2,16777343,'2010-11-11 15:25:53'),(12,5,16777343,'2010-11-11 15:26:30'),(13,5,16777343,'2010-11-11 15:26:56'),(14,1,16777343,'2010-11-11 15:29:22'),(15,1,16777343,'2010-11-11 17:38:17'),(16,1,16777343,'2010-11-11 18:14:43'),(17,1,16777343,'2010-11-11 21:56:45'),(18,1,16777343,'2010-11-11 22:07:39'),(19,1,16777343,'2010-11-11 22:26:36'),(20,1,16777343,'2010-11-11 22:36:24'),(21,1,16777343,'2010-11-12 00:01:30'),(22,1,16777343,'2010-11-12 00:04:05'),(23,1,16777343,'2010-11-12 00:17:10'),(24,1,16777343,'2010-11-12 00:21:06'),(25,1,16777343,'2010-11-12 00:33:43'),(26,1,16777343,'2010-11-12 00:44:28'),(27,1,16777343,'2010-11-12 02:23:13'),(28,1,16777343,'2010-11-12 02:25:12'),(29,1,16777343,'2010-11-12 02:38:59'),(30,1,16777343,'2010-11-12 02:40:52'),(31,1,16777343,'2010-11-12 11:31:11'),(32,1,16777343,'2010-11-12 11:53:02'),(33,1,16777343,'2010-11-12 11:54:13'),(34,1,16777343,'2010-11-12 11:54:43'),(35,1,16777343,'2010-11-12 11:56:47'),(36,1,16777343,'2010-11-12 12:09:59'),(37,1,16777343,'2010-11-12 12:53:39'),(38,1,16777343,'2010-11-12 12:55:59'),(39,1,16777343,'2010-11-13 10:53:09'),(40,1,16777343,'2010-11-15 11:01:17'),(41,1,16777343,'2010-11-17 07:35:25'),(42,1,16777343,'2006-11-17 10:04:39'),(43,1,16777343,'2006-11-17 10:05:14'),(44,1,16777343,'2006-11-17 10:06:00'),(45,1,16777343,'2006-11-17 10:07:57'),(46,1,16777343,'2006-11-17 10:09:40'),(47,1,16777343,'2006-11-17 10:10:07'),(48,1,16777343,'2006-11-17 10:10:34'),(49,1,16777343,'2006-11-17 10:12:56'),(50,1,16777343,'2006-11-17 10:13:48'),(51,1,16777343,'2006-11-17 10:14:39'),(52,1,16777343,'2006-11-17 10:15:08'),(53,1,16777343,'2006-11-17 12:26:38'),(54,1,16777343,'2006-11-17 12:26:55'),(55,1,16777343,'2010-11-18 14:21:24');
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
INSERT INTO `alliance_attackable` VALUES (1,0),(0,1),(3,1),(3,3);
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
INSERT INTO `alliance_hostile` VALUES (1,0),(0,1),(3,1),(3,3);
/*!40000 ALTER TABLE `alliance_hostile` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character`
--

DROP TABLE IF EXISTS `character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character` (
  `id` int(11) NOT NULL COMMENT 'The unique ID of the character.',
  `character_template_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The template that this character was created from (not required - mostly for developer reference).',
  `name` varchar(60) NOT NULL DEFAULT '' COMMENT 'The character''s name. Prefixed with `~<ID>_` when its a deleted user. The ~ denotes deleted, and the <ID> ensures a unique value.',
  `shop_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The shop that this character runs. Null if not a shopkeeper.',
  `chat_dialog` smallint(5) unsigned DEFAULT NULL COMMENT 'The chat dialog that this character displays. Null for no chat. Intended for NPCs only.',
  `ai_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The AI used by this character. Null for no AI (does nothing, or is user-controller). Intended for NPCs only.',
  `load_map_id` smallint(5) unsigned NOT NULL DEFAULT '3' COMMENT 'The map to load on (when logging in / being created).',
  `load_x` smallint(5) unsigned NOT NULL DEFAULT '1024' COMMENT 'The x coordinate to load at.',
  `load_y` smallint(5) unsigned NOT NULL DEFAULT '600' COMMENT 'The y coordinate to load at.',
  `respawn_map_id` smallint(5) unsigned DEFAULT '3' COMMENT 'The map to respawn on (when null, cannot respawn). Used to reposition character after death.',
  `respawn_x` float NOT NULL DEFAULT '1024' COMMENT 'The x coordinate to respawn at.',
  `respawn_y` float NOT NULL DEFAULT '600' COMMENT 'The y coordinate to respawn at.',
  `body_id` smallint(5) unsigned NOT NULL DEFAULT '1' COMMENT 'The body to use to display this character.',
  `move_speed` smallint(5) unsigned NOT NULL DEFAULT '1800' COMMENT 'The movement speed of the character.',
  `cash` int(11) NOT NULL DEFAULT '0' COMMENT 'Amount of cash.',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '1' COMMENT 'Current level.',
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
  CONSTRAINT `character_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_3` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_4` FOREIGN KEY (`load_map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_5` FOREIGN KEY (`respawn_map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Persisted (users, persistent NPCs) chars.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character`
--

LOCK TABLES `character` WRITE;
/*!40000 ALTER TABLE `character` DISABLE KEYS */;
INSERT INTO `character` VALUES (0,NULL,'Test',NULL,NULL,NULL,3,1024,600,3,1024,600,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1),(1,NULL,'Spodi',NULL,NULL,NULL,3,1024,600,3,1024,600,1,1800,202755,76,2250,372,53,100,100,100,1,1,1,1,3,2),(2,NULL,'Spodii',NULL,NULL,NULL,3,1024,600,3,1024,600,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1),(3,NULL,'Spodiii',NULL,NULL,NULL,3,1024,600,3,1024,600,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1),(4,NULL,'Skye',NULL,NULL,NULL,3,1024,600,3,1024,600,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1),(5,NULL,'Blahh',NULL,NULL,NULL,3,1024,600,3,1024,600,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1),(6,NULL,'Grrrrr',NULL,NULL,NULL,3,1024,600,3,1024,600,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1),(7,NULL,'Kitty',NULL,NULL,NULL,1,1024,600,3,1024,600,1,1800,0,3,60,0,50,50,50,50,1,1,1,1,2,10),(8,NULL,'Pickles',NULL,NULL,NULL,3,1024,600,3,1024,600,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1);
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
INSERT INTO `character_equipped` VALUES (1,20,0),(1,124,2),(7,129,2),(7,132,0),(1,157,1);
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
INSERT INTO `character_inventory` VALUES (1,22,2),(1,33,0),(1,39,3),(1,40,4),(1,41,1),(1,112,5),(1,117,8),(7,128,1),(1,134,9),(1,137,7),(7,138,0),(1,145,12),(1,149,10),(1,150,6),(7,154,2),(1,165,13),(1,186,17),(1,191,11),(1,194,15),(1,195,18),(1,197,14),(1,200,16);
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
  PRIMARY KEY (`character_id`,`quest_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Quest status for characters. Intended for users chars.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_quest_status`
--

LOCK TABLES `character_quest_status` WRITE;
/*!40000 ALTER TABLE `character_quest_status` DISABLE KEYS */;
INSERT INTO `character_quest_status` VALUES (7,1,'2010-11-11 15:28:00','2010-11-11 15:28:05');
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
  `skill_id` smallint(5) unsigned NOT NULL COMMENT 'The skill the character knows.',
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
/*!40000 ALTER TABLE `character_skill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_status_effect`
--

DROP TABLE IF EXISTS `character_status_effect`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_status_effect` (
  `id` int(11) NOT NULL COMMENT 'Unique ID of the status effect instance.',
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
INSERT INTO `character_status_effect` VALUES (0,1,0,3,35071);
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
  `respawn` smallint(5) unsigned NOT NULL DEFAULT '5' COMMENT 'How long to wait after death to be respawned (intended for NPCs only).',
  `level` tinyint(3) unsigned NOT NULL DEFAULT '1' COMMENT 'The character''s level.',
  `exp` int(11) NOT NULL DEFAULT '0' COMMENT 'Current exp.',
  `statpoints` int(11) NOT NULL DEFAULT '0' COMMENT 'Number of stat points available to spend.',
  `give_exp` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Amount of exp to give when killed (intended for NPCs only).',
  `give_cash` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Amount of cash to give when killed (intended for NPCs only).',
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
  CONSTRAINT `character_template_ibfk_2` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_ibfk_3` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Character templates (used to instantiate characters).';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_template`
--

LOCK TABLES `character_template` WRITE;
/*!40000 ALTER TABLE `character_template` DISABLE KEYS */;
INSERT INTO `character_template` VALUES (0,0,'User Template',NULL,NULL,NULL,1,1800,5,1,0,0,0,0,50,50,1,2,1,1,1,1),(1,1,'A Test NPC',1,NULL,NULL,2,1800,10,1,0,0,5,5,5,5,1,2,1,1,1,1),(2,2,'Quest Giver',NULL,NULL,NULL,2,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(4,2,'Potion Vendor',NULL,1,NULL,2,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(5,2,'Talking Guy',NULL,NULL,0,1,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(6,3,'Brawler',1,NULL,NULL,2,1800,8,1,0,0,8,8,5,5,1,3,1,1,1,1);
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
INSERT INTO `character_template_equipped` VALUES (0,1,3,60000),(1,1,5,30000);
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
INSERT INTO `character_template_inventory` VALUES (0,1,5,0,2,10000),(1,1,7,1,10,65535),(2,1,3,1,1,5000),(3,1,5,0,2,10000),(4,1,7,1,10,65535),(5,1,3,1,1,5000),(6,1,7,1,10,65535),(7,1,3,1,1,5000),(8,1,3,1,1,5000),(9,1,7,1,10,65535),(10,1,3,1,1,5000),(11,1,7,1,10,65535),(12,1,3,1,1,5000),(13,1,5,0,2,10000);
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
INSERT INTO `character_template_quest_provider` VALUES (2,1);
/*!40000 ALTER TABLE `character_template_quest_provider` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guild`
--

DROP TABLE IF EXISTS `guild`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `guild` (
  `id` smallint(5) unsigned NOT NULL COMMENT 'The unique ID of the guild.',
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
  CONSTRAINT `guild_event_ibfk_3` FOREIGN KEY (`target_character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
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
  `id` int(11) NOT NULL COMMENT 'The unique ID of the item.',
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The live, persisted items.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES (0,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(1,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(2,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(3,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(4,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',25,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(5,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(6,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(7,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(8,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',31,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(9,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(10,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(11,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',34,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(12,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(13,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(14,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(15,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',22,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(16,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',25,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(17,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(18,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(19,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(20,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(21,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(22,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(23,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(24,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',23,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(25,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(26,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(27,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(28,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(29,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(30,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(31,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(32,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',28,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(33,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',27,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(34,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(35,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(36,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(37,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(38,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(39,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(40,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(41,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',24,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(42,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(43,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',20,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(44,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(45,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(46,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(47,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(48,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(49,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(50,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',33,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(51,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',3,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(52,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(53,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(54,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',27,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(55,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(56,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(57,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',21,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(58,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(59,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(60,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',26,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(61,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(62,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',28,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(63,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(64,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(65,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(66,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(67,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(68,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(69,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(70,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(71,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(72,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(73,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',36,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(74,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(75,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(76,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(77,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(78,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',19,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(79,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(80,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(81,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(82,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(83,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(84,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',31,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(85,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(86,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(87,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(88,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',26,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(89,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(90,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(91,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(92,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',37,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(93,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(96,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(97,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(99,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(102,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',22,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(104,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(105,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(106,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',40,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(107,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',35,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(108,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',26,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(109,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(110,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(111,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',21,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(112,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(113,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(114,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(115,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(116,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(117,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(121,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(122,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',23,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(123,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(124,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(125,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(126,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(127,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(128,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',38,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(129,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(130,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',31,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(131,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',18,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(132,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(133,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(134,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(135,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',31,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(136,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',26,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(137,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(138,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',3,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(139,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(140,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(141,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(142,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(143,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',23,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(144,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',21,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(145,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',3,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body',NULL),(146,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',15,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(147,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(148,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(149,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(150,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(151,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(152,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(154,1,1,0,0,9,16,'Healing Potion','A healing potion',4,94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(155,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',22,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(156,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(157,4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',1,99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body',NULL),(158,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(159,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(160,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(161,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(165,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(167,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(169,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',20,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(170,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(181,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(182,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(183,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(186,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(191,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(194,1,1,0,0,9,16,'Healing Potion','A healing potion',14,94,15,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(195,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',3,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(197,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',97,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(200,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',68,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1);
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
INSERT INTO `item_template` VALUES (0,2,1,10,16,16,'Unarmed','Unarmed',1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(1,1,0,0,9,16,'Healing Potion','A healing potion',94,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(2,1,0,0,9,16,'Mana Potion','A mana potion',95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(4,4,0,0,22,22,'Crystal Armor','Body armor made out of crystal',99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body',NULL),(5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL,NULL),(7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1);
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
INSERT INTO `map` VALUES (1,'Desert 1'),(2,'Desert 2'),(3,'Checkered'),(4,'Winter');
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1 COMMENT='NPC spawns for the maps.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `map_spawn`
--

LOCK TABLES `map_spawn` WRITE;
/*!40000 ALTER TABLE `map_spawn` DISABLE KEYS */;
INSERT INTO `map_spawn` VALUES (0,1,1,3,NULL,NULL,NULL,NULL),(1,1,2,1,190,278,64,64),(3,1,4,1,545,151,64,64),(4,1,5,1,736,58,64,64),(5,2,6,15,NULL,NULL,NULL,NULL),(6,3,1,3,NULL,NULL,NULL,NULL),(7,4,1,3,NULL,NULL,NULL,NULL);
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
INSERT INTO `quest_require_kill` VALUES (0,1,5);
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
INSERT INTO `quest_reward_item` VALUES (0,3,1);
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
INSERT INTO `server_time` VALUES ('2010-11-18 14:21:32');
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
INSERT INTO `shop` VALUES (0,'Test Shop',1),(1,'Vending Machine',0);
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
INSERT INTO `shop_item` VALUES (0,1),(1,1),(0,2),(1,2),(0,3),(0,4),(0,5),(0,6),(0,7);
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
  `id` int(11),
  `character_template_id` smallint(5) unsigned,
  `name` varchar(60),
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
-- Temporary table structure for view `view_user_character`
--

DROP TABLE IF EXISTS `view_user_character`;
/*!50001 DROP VIEW IF EXISTS `view_user_character`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE TABLE `view_user_character` (
  `id` int(11),
  `character_template_id` smallint(5) unsigned,
  `name` varchar(60),
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
INSERT INTO `world_stats_count_consume_item` VALUES (1,13,'2010-11-12 09:00:08');
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
INSERT INTO `world_stats_count_item_create` VALUES (3,922,'2010-11-18 22:21:29'),(5,710,'2010-11-18 22:21:20'),(7,616,'2010-11-18 22:21:20');
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
INSERT INTO `world_stats_count_npc_kill_user` VALUES (1,1,1,'0000-00-00 00:00:00');
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
INSERT INTO `world_stats_count_user_consume_item` VALUES (1,1,13,'2010-11-12 09:00:08');
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
INSERT INTO `world_stats_count_user_kill_npc` VALUES (1,1,137,'2010-11-18 22:21:29');
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
) ENGINE=MyISAM AUTO_INCREMENT=75 DEFAULT CHARSET=latin1 COMMENT='Snapshots of network deltas.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_network`
--

LOCK TABLES `world_stats_network` WRITE;
/*!40000 ALTER TABLE `world_stats_network` DISABLE KEYS */;
INSERT INTO `world_stats_network` VALUES (1,'2010-11-10 18:40:09',0,100,10,17,307,10,15),(2,'2010-11-10 19:34:50',1,56,5,10,163,6,8),(3,'2010-11-10 19:37:35',0,139,14,26,413,15,19),(4,'2010-11-11 23:24:07',0,5,0,0,3,0,0),(5,'2010-11-11 23:25:07',0,4,0,0,2,0,0),(6,'2010-11-11 23:26:07',1,7,1,1,7,1,1),(7,'2010-11-11 23:27:07',1,31,3,5,84,3,5),(8,'2010-11-11 23:28:07',1,125,13,22,365,13,19),(9,'2010-11-11 23:30:15',1,112,11,20,340,11,17),(10,'2010-11-12 01:37:57',0,0,0,0,0,0,0),(11,'2010-11-12 01:38:57',1,43,4,7,144,4,7),(12,'2010-11-12 01:39:57',1,105,11,20,328,11,14),(13,'2010-11-12 01:40:57',1,122,13,25,364,14,15),(14,'2010-11-12 01:41:57',1,117,13,25,327,14,14),(15,'2010-11-12 01:42:57',1,118,13,25,326,14,14),(16,'2010-11-12 01:43:57',1,116,13,25,325,14,14),(17,'2010-11-12 01:44:57',1,116,13,25,326,14,14),(18,'2010-11-12 01:45:57',1,117,13,25,328,14,14),(19,'2010-11-12 01:46:57',1,119,13,26,328,14,14),(20,'2010-11-12 01:47:57',1,117,13,25,326,14,14),(21,'2010-11-12 01:48:57',1,117,13,25,328,14,14),(22,'2010-11-12 01:49:57',1,118,13,25,331,14,14),(23,'2010-11-12 01:50:57',1,117,13,25,326,14,14),(24,'2010-11-12 01:51:57',1,117,13,25,326,14,14),(25,'2010-11-12 01:52:57',1,117,13,25,332,14,14),(26,'2010-11-12 01:53:57',1,117,13,25,326,14,14),(27,'2010-11-12 01:54:57',1,117,13,25,326,14,14),(28,'2010-11-12 01:55:57',1,118,13,25,328,14,14),(29,'2010-11-12 01:56:57',1,118,13,25,331,14,14),(30,'2010-11-12 01:57:57',1,117,13,25,333,14,14),(31,'2010-11-12 01:58:57',1,117,13,25,328,14,14),(32,'2010-11-12 01:59:57',1,118,13,25,332,14,14),(33,'2010-11-12 02:00:57',1,117,13,25,327,14,14),(34,'2010-11-12 02:01:57',1,116,12,25,329,14,14),(35,'2010-11-12 02:02:57',1,118,13,25,331,14,14),(36,'2010-11-12 02:03:57',1,118,13,25,329,14,14),(37,'2010-11-12 02:04:57',1,118,13,25,333,14,14),(38,'2010-11-12 02:05:57',1,119,13,25,330,14,14),(39,'2010-11-12 02:06:57',1,117,13,25,329,14,14),(40,'2010-11-12 02:07:57',1,117,12,24,340,14,14),(41,'2010-11-12 02:17:19',0,6,0,1,22,1,1),(42,'2010-11-12 02:17:19',0,0,0,0,0,0,0),(43,'2010-11-12 05:57:06',1,51,4,8,191,5,8),(44,'2010-11-12 06:27:33',1,53,5,10,160,6,7),(45,'2010-11-12 06:28:33',1,37,4,7,110,5,5),(46,'2010-11-12 08:02:26',1,59,6,10,189,6,9),(47,'2010-11-12 08:05:03',1,31,3,5,100,3,5),(48,'2010-11-12 08:06:03',1,3,0,0,3,0,0),(49,'2010-11-12 08:07:03',1,3,0,0,3,0,0),(50,'2010-11-12 08:08:04',1,5,1,1,7,1,1),(51,'2010-11-12 08:18:09',0,74,7,14,227,8,10),(52,'2010-11-12 08:34:42',1,89,9,17,281,9,13),(53,'2010-11-12 08:45:22',1,68,7,12,208,7,10),(54,'2010-11-12 08:46:22',1,59,6,11,172,6,8),(55,'2010-11-12 08:47:22',1,4,0,1,7,0,1),(56,'2010-11-12 08:48:22',1,3,0,0,4,0,1),(57,'2010-11-12 08:49:22',1,3,0,0,4,0,1),(58,'2010-11-12 08:50:22',1,3,0,0,3,0,0),(59,'2010-11-12 08:51:22',1,3,0,0,3,0,0),(60,'2010-11-12 08:52:22',1,3,0,0,3,0,0),(61,'2010-11-12 08:53:22',1,3,0,0,3,0,0),(62,'2010-11-12 08:54:22',1,3,0,0,3,0,0),(63,'2010-11-12 08:55:22',1,3,0,0,3,0,0),(64,'2010-11-12 08:56:22',1,3,0,0,3,0,0),(65,'2010-11-12 08:57:22',1,3,0,0,3,0,0),(66,'2010-11-12 08:58:22',1,3,0,0,3,0,0),(67,'2010-11-12 08:59:22',1,3,0,0,3,0,0),(68,'2010-11-12 09:00:22',1,4,0,1,6,0,0),(69,'2010-11-12 09:01:22',1,3,0,0,3,0,0),(70,'2010-11-12 09:02:22',1,3,0,0,3,0,0),(71,'2010-11-12 09:03:22',1,3,0,0,3,0,0),(72,'2010-11-12 10:39:55',1,60,6,11,200,6,9),(73,'2010-11-12 10:40:55',1,95,9,17,314,9,14),(74,'2010-11-13 19:36:16',0,0,0,0,0,0,0);
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
  `user_level` tinyint(3) unsigned NOT NULL COMMENT 'The level of the user was when this event took place.',
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1 COMMENT='Event log: NPC kill user.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_npc_kill_user`
--

LOCK TABLES `world_stats_npc_kill_user` WRITE;
/*!40000 ALTER TABLE `world_stats_npc_kill_user` DISABLE KEYS */;
INSERT INTO `world_stats_npc_kill_user` VALUES (1,1,1,53,1024,593,1131,754,3,'2010-11-10 18:39:25'),(2,1,1,54,1024,593,1017,594,3,'2010-11-10 19:34:37'),(3,1,1,54,1024,593,1033,518,3,'2010-11-10 19:35:28'),(4,7,1,1,1024,593,1152,754,3,'2010-11-11 23:27:07'),(5,7,1,1,1024,593,492,717,3,'2010-11-11 23:27:20'),(6,1,1,75,1024,593,1032,594,3,'2006-11-17 18:08:08');
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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Event log: User accepts a quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_quest_accept`
--

LOCK TABLES `world_stats_quest_accept` WRITE;
/*!40000 ALTER TABLE `world_stats_quest_accept` DISABLE KEYS */;
INSERT INTO `world_stats_quest_accept` VALUES (1,7,1,1,232,308,'2010-11-11 23:28:00');
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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COMMENT='Event log: User completes a quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_quest_complete`
--

LOCK TABLES `world_stats_quest_complete` WRITE;
/*!40000 ALTER TABLE `world_stats_quest_complete` DISABLE KEYS */;
INSERT INTO `world_stats_quest_complete` VALUES (1,7,1,1,218,308,'2010-11-11 23:28:05');
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
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1 COMMENT='Event log: User consumes use-once item.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_user_consume_item`
--

LOCK TABLES `world_stats_user_consume_item` WRITE;
/*!40000 ALTER TABLE `world_stats_user_consume_item` DISABLE KEYS */;
INSERT INTO `world_stats_user_consume_item` VALUES (1,1,1,3,883,498,'2010-11-12 01:38:29'),(2,1,2,3,1024,594,'2010-11-12 06:27:04'),(3,1,1,3,1024,594,'2010-11-12 06:27:15'),(4,1,1,3,1056,594,'2010-11-12 08:01:48'),(5,1,1,3,1056,594,'2010-11-12 08:01:49'),(6,1,1,3,1056,594,'2010-11-12 08:01:49'),(7,1,1,3,1056,594,'2010-11-12 08:01:49'),(8,1,1,3,1056,594,'2010-11-12 08:01:49'),(9,1,1,3,1056,594,'2010-11-12 08:01:49'),(10,1,1,3,1056,594,'2010-11-12 08:01:50'),(11,1,1,3,1056,594,'2010-11-12 08:01:50'),(12,1,1,3,651,434,'2010-11-12 09:00:05'),(13,1,1,3,651,434,'2010-11-12 09:00:06'),(14,1,1,3,651,434,'2010-11-12 09:00:08');
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
  `user_level` tinyint(3) unsigned NOT NULL COMMENT 'The level of the user was when this event took place.',
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
) ENGINE=InnoDB AUTO_INCREMENT=148 DEFAULT CHARSET=latin1 COMMENT='Event log: User kills NPC.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_user_kill_npc`
--

LOCK TABLES `world_stats_user_kill_npc` WRITE;
/*!40000 ALTER TABLE `world_stats_user_kill_npc` DISABLE KEYS */;
INSERT INTO `world_stats_user_kill_npc` VALUES (1,1,1,53,1142,754,0,0,3,'2010-11-10 18:39:31'),(2,1,1,53,1142,754,0,0,3,'2010-11-10 18:39:32'),(3,1,1,53,1142,754,0,0,3,'2010-11-10 18:39:32'),(4,1,1,53,1203,722,0,0,3,'2010-11-10 18:39:43'),(5,1,1,53,1106,589,0,0,3,'2010-11-10 18:39:45'),(6,1,1,54,1095,594,0,0,3,'2010-11-10 18:39:46'),(7,1,1,54,705,754,0,0,3,'2010-11-10 18:39:59'),(8,1,1,54,705,754,0,0,3,'2010-11-10 18:40:00'),(9,1,1,54,705,754,0,0,3,'2010-11-10 18:40:00'),(10,1,1,54,1024,594,0,0,3,'2010-11-10 19:34:45'),(11,1,1,54,929,730,0,0,3,'2010-11-10 19:36:41'),(12,1,1,55,882,754,0,0,3,'2010-11-10 19:36:45'),(13,1,1,55,1256,691,0,0,3,'2010-11-10 19:36:48'),(14,1,1,55,862,495,0,0,3,'2010-11-10 19:36:57'),(15,1,1,55,921,694,0,0,3,'2010-11-10 19:36:58'),(16,1,1,55,936,754,0,0,3,'2010-11-10 19:36:59'),(17,1,1,55,1177,754,0,0,3,'2010-11-10 19:37:09'),(18,1,1,56,1141,639,0,0,3,'2010-11-10 19:37:10'),(19,1,1,56,1162,706,0,0,3,'2010-11-10 19:37:10'),(20,1,1,56,1164,754,0,0,3,'2010-11-10 19:37:20'),(21,1,1,56,764,498,0,0,3,'2010-11-10 19:37:24'),(22,1,1,56,705,694,0,0,3,'2010-11-10 19:37:26'),(23,7,1,1,1160,754,0,0,3,'2010-11-11 23:27:05'),(24,7,1,1,677,754,0,0,3,'2010-11-11 23:27:11'),(25,7,1,1,397,754,0,0,3,'2010-11-11 23:27:17'),(26,7,1,1,860,754,0,0,3,'2010-11-11 23:27:23'),(27,7,1,1,932,754,0,0,3,'2010-11-11 23:27:28'),(28,7,1,1,936,754,0,0,3,'2010-11-11 23:27:33'),(29,7,1,2,911,754,0,0,3,'2010-11-11 23:27:33'),(30,7,1,2,317,754,0,0,3,'2010-11-11 23:27:45'),(31,7,1,2,841,658,0,0,1,'2010-11-11 23:27:47'),(32,7,1,2,603,658,0,0,1,'2010-11-11 23:27:49'),(33,1,1,56,1024,594,0,0,3,'2010-11-11 23:29:27'),(34,1,1,57,1265,722,0,0,3,'2010-11-11 23:29:29'),(35,1,1,57,1265,722,0,0,3,'2010-11-11 23:29:30'),(36,1,1,57,384,850,0,0,4,'2010-11-11 23:29:36'),(37,1,1,57,384,850,0,0,4,'2010-11-11 23:29:37'),(38,1,1,57,384,850,0,0,4,'2010-11-11 23:29:38'),(39,1,1,57,351,850,0,0,4,'2010-11-11 23:29:48'),(40,1,1,58,373,850,0,0,4,'2010-11-11 23:30:00'),(41,1,1,58,373,850,0,0,4,'2010-11-11 23:30:01'),(42,1,1,58,1257,722,0,0,3,'2010-11-11 23:30:12'),(43,1,1,58,1261,722,0,0,3,'2010-11-11 23:30:13'),(44,1,1,58,1261,722,0,0,3,'2010-11-11 23:30:14'),(45,1,1,58,903,754,0,0,3,'2010-11-12 01:39:22'),(46,1,1,59,903,754,0,0,3,'2010-11-12 01:39:23'),(47,1,1,59,903,754,0,0,3,'2010-11-12 01:39:25'),(48,1,1,59,893,498,0,0,3,'2010-11-12 01:39:37'),(49,1,1,59,993,594,0,0,3,'2010-11-12 01:39:52'),(50,1,1,59,1227,722,0,0,3,'2010-11-12 01:40:08'),(51,1,1,59,1098,594,0,0,3,'2010-11-12 02:08:09'),(52,1,1,60,993,754,0,0,3,'2010-11-12 02:17:19'),(53,1,1,60,1095,594,0,0,3,'2010-11-12 05:56:52'),(54,1,1,60,1193,722,0,0,3,'2010-11-12 05:56:56'),(55,1,1,60,1193,722,0,0,3,'2010-11-12 05:57:00'),(56,1,1,60,991,594,0,0,3,'2010-11-12 06:07:41'),(57,1,1,60,991,594,0,0,3,'2010-11-12 06:07:42'),(58,1,1,61,991,594,0,0,3,'2010-11-12 06:07:47'),(59,1,1,61,651,707,0,0,3,'2010-11-12 06:07:54'),(60,1,1,61,1190,623,0,0,3,'2010-11-12 06:07:58'),(61,1,1,61,1295,725,0,0,3,'2010-11-12 06:07:59'),(62,1,1,61,1215,722,0,0,3,'2010-11-12 06:08:05'),(63,1,1,61,776,406,0,0,3,'2010-11-12 06:08:10'),(64,1,1,62,804,498,0,0,3,'2010-11-12 06:36:34'),(65,1,1,62,804,498,0,0,3,'2010-11-12 06:36:34'),(66,1,1,62,1009,594,0,0,3,'2010-11-12 08:04:13'),(67,1,1,62,973,594,0,0,3,'2010-11-12 08:17:13'),(68,1,1,62,869,461,0,0,3,'2010-11-12 08:17:13'),(69,1,1,62,1024,594,0,0,3,'2010-11-12 08:33:47'),(70,1,1,63,1269,722,0,0,3,'2010-11-12 08:34:00'),(71,1,1,63,1168,720,0,0,3,'2010-11-12 08:34:04'),(72,1,1,63,1168,754,0,0,3,'2010-11-12 08:34:04'),(73,1,1,63,1021,594,0,0,3,'2010-11-12 08:34:13'),(74,1,1,63,1024,594,0,0,3,'2010-11-12 08:44:31'),(75,1,1,63,1024,594,0,0,3,'2010-11-12 08:44:33'),(76,1,1,64,1024,594,0,0,3,'2010-11-12 08:44:34'),(77,1,1,64,719,754,0,0,3,'2010-11-12 08:44:44'),(78,1,1,64,1192,712,0,0,3,'2010-11-12 08:44:47'),(79,1,1,64,1170,655,0,0,3,'2010-11-12 08:44:48'),(80,1,1,64,648,434,0,0,3,'2010-11-12 08:44:56'),(81,1,1,64,648,434,0,0,3,'2010-11-12 08:45:32'),(82,1,1,65,659,434,0,0,3,'2010-11-12 08:45:35'),(83,1,1,65,626,754,0,0,3,'2010-11-12 08:45:44'),(84,1,1,65,979,754,0,0,3,'2010-11-12 08:45:51'),(85,1,1,65,1195,633,0,0,3,'2010-11-12 08:45:52'),(86,1,1,65,835,498,0,0,3,'2010-11-12 08:45:55'),(87,1,1,65,1152,750,0,0,3,'2010-11-12 08:46:10'),(88,1,1,66,702,366,0,0,3,'2010-11-12 08:46:16'),(89,1,1,66,970,594,0,0,3,'2010-11-12 10:25:15'),(90,1,1,66,970,594,0,0,3,'2010-11-12 10:25:16'),(91,1,1,66,1207,722,0,0,3,'2010-11-12 10:25:19'),(92,1,1,66,877,469,0,0,3,'2010-11-12 10:25:29'),(93,1,1,66,693,366,0,0,3,'2010-11-12 10:25:30'),(94,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(95,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(96,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(97,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(98,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(99,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(100,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(101,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(102,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(103,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(104,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(105,1,1,68,715,754,0,0,3,'2010-11-12 10:40:48'),(106,1,1,69,1024,594,0,0,3,'2010-11-12 19:31:15'),(107,1,1,69,991,594,0,0,3,'2010-11-12 19:31:18'),(108,1,1,69,1175,693,0,0,3,'2010-11-12 19:31:22'),(109,1,1,69,1024,594,0,0,3,'2010-11-12 19:53:05'),(110,1,1,69,898,468,0,0,3,'2010-11-12 19:53:07'),(111,1,1,69,929,685,0,0,3,'2010-11-12 19:53:10'),(112,1,1,70,1040,594,0,0,3,'2010-11-12 19:53:24'),(113,1,1,70,1144,720,0,0,3,'2010-11-12 19:53:30'),(114,1,1,70,1144,754,0,0,3,'2010-11-12 19:53:31'),(115,1,1,70,1163,611,0,0,3,'2010-11-12 19:53:40'),(116,1,1,70,825,498,0,0,3,'2010-11-12 19:53:43'),(117,1,1,71,1144,647,0,0,3,'2010-11-12 19:54:35'),(118,1,1,71,1144,647,0,0,3,'2010-11-12 19:54:35'),(119,1,1,71,1144,647,0,0,3,'2010-11-12 19:54:35'),(120,1,1,71,1144,647,0,0,3,'2010-11-12 19:54:35'),(121,1,1,71,1144,647,0,0,3,'2010-11-12 19:54:35'),(122,1,1,71,1144,647,0,0,3,'2010-11-12 19:54:35'),(123,1,1,71,929,730,0,0,3,'2010-11-12 19:56:50'),(124,1,1,72,900,754,0,0,3,'2010-11-12 19:56:51'),(125,1,1,72,795,754,0,0,3,'2010-11-12 19:56:51'),(126,1,1,72,980,590,0,0,3,'2010-11-12 19:57:02'),(127,1,1,72,792,754,0,0,3,'2010-11-12 19:57:06'),(128,1,1,72,698,754,0,0,3,'2010-11-12 19:57:07'),(129,1,1,72,884,658,0,0,1,'2010-11-12 19:57:12'),(130,1,1,73,870,658,0,0,1,'2010-11-12 19:57:13'),(131,1,1,73,675,658,0,0,1,'2010-11-12 19:57:14'),(132,1,1,73,1147,754,0,0,3,'2010-11-12 19:57:21'),(133,1,1,73,1185,630,0,0,3,'2010-11-12 19:57:21'),(134,1,1,73,1127,590,0,0,3,'2010-11-12 19:57:23'),(135,1,1,73,1051,594,0,0,3,'2010-11-12 19:57:35'),(136,1,1,74,922,469,0,0,3,'2010-11-12 19:57:36'),(137,1,1,74,929,724,0,0,3,'2010-11-12 19:57:37'),(138,1,1,74,929,754,0,0,3,'2010-11-12 20:10:04'),(139,1,1,74,929,754,0,0,3,'2010-11-12 20:10:05'),(140,1,1,74,929,754,0,0,3,'2010-11-12 20:10:06'),(141,1,1,74,932,740,0,0,3,'2010-11-12 20:10:18'),(142,1,1,75,1026,754,0,0,3,'2010-11-12 20:10:18'),(143,1,1,75,1056,594,0,0,3,'2010-11-12 20:10:21'),(144,1,1,75,1024,594,0,0,3,'2010-11-12 20:53:42'),(145,1,1,75,1121,754,0,0,3,'2010-11-12 20:53:46'),(146,1,1,75,1024,594,0,0,3,'2010-11-18 22:21:28'),(147,1,1,75,894,461,0,0,3,'2010-11-18 22:21:29');
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
  `level` tinyint(3) unsigned NOT NULL COMMENT 'The level that the character leveled up to (their new level).',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=latin1 COMMENT='Event log: User levels up.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_user_level`
--

LOCK TABLES `world_stats_user_level` WRITE;
/*!40000 ALTER TABLE `world_stats_user_level` DISABLE KEYS */;
INSERT INTO `world_stats_user_level` VALUES (1,1,3,1106,589,54,'2010-11-10 18:39:45'),(2,1,3,929,730,55,'2010-11-10 19:36:41'),(3,1,3,1177,754,56,'2010-11-10 19:37:09'),(4,7,3,936,754,2,'2010-11-11 23:27:33'),(5,7,1,218,308,3,'2010-11-11 23:28:05'),(6,1,3,1024,594,57,'2010-11-11 23:29:27'),(7,1,4,351,850,58,'2010-11-11 23:29:48'),(8,1,3,903,754,59,'2010-11-12 01:39:22'),(9,1,3,1098,594,60,'2010-11-12 02:08:09'),(10,1,3,991,594,61,'2010-11-12 06:07:42'),(11,1,3,776,406,62,'2010-11-12 06:08:10'),(12,1,3,1024,594,63,'2010-11-12 08:33:47'),(13,1,3,1024,594,64,'2010-11-12 08:44:33'),(14,1,3,648,434,65,'2010-11-12 08:45:32'),(15,1,3,1152,750,66,'2010-11-12 08:46:10'),(16,1,3,693,366,67,'2010-11-12 10:25:30'),(17,1,3,715,754,69,'2010-11-12 10:40:48'),(18,1,3,715,754,69,'2010-11-12 10:40:48'),(19,1,3,929,685,70,'2010-11-12 19:53:10'),(20,1,3,1135,754,71,'2010-11-12 19:54:17'),(21,1,3,929,730,72,'2010-11-12 19:56:50'),(22,1,1,884,658,73,'2010-11-12 19:57:12'),(23,1,3,1051,594,74,'2010-11-12 19:57:35'),(24,1,3,932,740,75,'2010-11-12 20:10:18'),(25,1,3,894,461,76,'2010-11-18 22:21:29');
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
-- Dumping routines for database 'demogame'
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
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `create_user_on_account`(accountName VARCHAR(50), characterName VARCHAR(30), characterID INT) RETURNS varchar(100) CHARSET latin1
BEGIN
		
		DECLARE character_count INT DEFAULT 0;
		DECLARE max_character_count INT DEFAULT 9;
		DECLARE is_id_free INT DEFAULT 0;
		DECLARE is_name_free INT DEFAULT 0;
		DECLARE errorMsg VARCHAR(100) DEFAULT "";
		DECLARE accountID INT DEFAULT NULL;

		SELECT `id` INTO accountID FROM `account` WHERE `name` = accountName;

		IF ISNULL(accountID) THEN
			SET errorMsg = "Account with the specified name does not exist.";
		ELSE
			SELECT COUNT(*) INTO character_count FROM `account_character` WHERE `account_id` = accountID;

			IF character_count > max_character_count THEN
				SET errorMsg = "No free character slots available in the account.";
			ELSE
				SELECT COUNT(*) INTO is_id_free FROM `character` WHERE `id` = characterID;
				
				IF is_id_free > 0 THEN
					SET errorMsg = "The specified CharacterID is not available for use.";
				ELSE
					SELECT COUNT(*) INTO is_name_free FROM `character` WHERE `name` = characterName LIMIT 1;
						
					IF is_name_free > 0 THEN
						SET errorMsg = "The specified character name is not available for use.";
					ELSE
						INSERT INTO `character` SET `id` = characterID, `name`	= characterName;
						INSERT INTO `account_character` SET `character_id` = characterID, `account_id` = accountID;
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
	CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_npc_character` AS SELECT c.*  FROM `character` c LEFT JOIN `account_character` a ON c.id = a.character_id WHERE a.account_id IS NULL;
    
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
	CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_user_character` AS SELECT c.* FROM `character` c INNER JOIN `account_character` a ON c.id = a.character_id WHERE a.time_deleted IS NULL;
    
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

-- Dump completed on 2010-11-18 15:20:10
