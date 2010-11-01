-- MySQL dump 10.13  Distrib 5.1.51, for Win64 (unknown)
--
-- Host: localhost    Database: demogame
-- ------------------------------------------------------
-- Server version	5.1.51-community
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
  `permissions` tinyint(3) unsigned NOT NULL DEFAULT '0',
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
INSERT INTO `account` VALUES (0,'Test','3fc0a7acf087f549ac2b266baf94b8b1','test@test.com',255,'2010-02-11 17:52:28','2010-02-11 18:03:56',16777343,NULL),(1,'Spodi','3fc0a7acf087f549ac2b266baf94b8b1','spodi@netgore.com',255,'2009-09-07 15:43:16','2010-11-01 14:05:28',16777343,NULL),(2,'Spodit','3fc0a7acf087f549ac2b266baf94b8b1','test@test.com',255,'2010-08-06 15:00:47','2010-08-18 00:41:59',16777343,NULL);
/*!40000 ALTER TABLE `account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_ban`
--

DROP TABLE IF EXISTS `account_ban`;
CREATE TABLE `account_ban` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'The unique ban ID.',
  `account_id` int(11) NOT NULL COMMENT 'The account that this ban is for.',
  `start_time` datetime NOT NULL COMMENT 'When this ban started.',
  `end_time` datetime NOT NULL COMMENT 'When this ban ends.',
  `reason` varchar(255) NOT NULL COMMENT 'The reason why this account was banned.',
  `issued_by` varchar(255) DEFAULT NULL COMMENT 'Name of the person or system that issued this ban.',
  `expired` tinyint(1) unsigned NOT NULL DEFAULT '0' COMMENT 'If the ban is expired. A non-zero value means true.',
  PRIMARY KEY (`id`),
  KEY `account_id` (`account_id`),
  KEY `expired` (`expired`),
  CONSTRAINT `account_ban_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

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
CREATE TABLE `account_character` (
  `character_id` int(11) NOT NULL,
  `account_id` int(11) NOT NULL,
  `time_deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`character_id`),
  KEY `account_id` (`account_id`),
  CONSTRAINT `account_character_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `account_character_ibfk_2` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `account_character`
--

LOCK TABLES `account_character` WRITE;
/*!40000 ALTER TABLE `account_character` DISABLE KEYS */;
INSERT INTO `account_character` VALUES (0,0,NULL),(1,1,NULL),(4,2,NULL);
/*!40000 ALTER TABLE `account_character` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_ips`
--

DROP TABLE IF EXISTS `account_ips`;
CREATE TABLE `account_ips` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `account_id` int(11) NOT NULL COMMENT 'The ID of the account.',
  `ip` int(10) unsigned NOT NULL COMMENT 'The IP that logged into the account.',
  `time` datetime NOT NULL COMMENT 'When this IP last logged into this account.',
  PRIMARY KEY (`id`),
  KEY `account_id` (`account_id`,`ip`),
  CONSTRAINT `account_ips_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB AUTO_INCREMENT=117;

--
-- Dumping data for table `account_ips`
--

LOCK TABLES `account_ips` WRITE;
/*!40000 ALTER TABLE `account_ips` DISABLE KEYS */;
INSERT INTO `account_ips` VALUES (68,1,16777343,'2010-10-28 23:24:58'),(69,1,16777343,'2010-10-28 23:29:08'),(70,1,16777343,'2010-10-29 00:11:07'),(71,1,16777343,'2010-10-29 00:28:18'),(72,1,16777343,'2010-10-29 00:35:14'),(73,1,16777343,'2010-10-29 02:05:31'),(74,1,16777343,'2010-10-29 02:06:13'),(75,1,16777343,'2010-10-29 02:20:21'),(76,1,16777343,'2010-10-29 02:23:26'),(77,1,16777343,'2010-10-29 12:39:20'),(78,1,16777343,'2010-10-29 12:49:50'),(79,1,16777343,'2010-10-29 12:56:22'),(80,1,16777343,'2010-10-29 14:22:24'),(81,1,16777343,'2010-10-29 14:23:54'),(82,1,16777343,'2010-10-29 16:18:57'),(83,1,16777343,'2010-10-29 16:21:04'),(84,1,16777343,'2010-10-29 16:27:51'),(85,1,16777343,'2010-10-29 16:35:26'),(86,1,16777343,'2010-10-29 20:39:25'),(87,1,16777343,'2010-10-30 15:22:12'),(88,1,16777343,'2010-10-30 15:24:23'),(89,1,16777343,'2010-10-30 16:01:36'),(90,1,16777343,'2010-10-30 16:06:03'),(91,1,16777343,'2010-10-30 16:08:18'),(92,1,16777343,'2010-10-30 16:08:47'),(93,1,16777343,'2010-10-30 16:32:51'),(94,1,16777343,'2010-10-30 16:35:55'),(95,1,16777343,'2010-10-30 16:48:21'),(96,1,16777343,'2010-10-30 17:28:34'),(97,1,16777343,'2010-10-30 17:29:17'),(98,1,16777343,'2010-10-30 17:30:08'),(99,1,16777343,'2010-10-30 17:30:57'),(100,1,16777343,'2010-10-30 17:31:56'),(101,1,16777343,'2010-10-30 17:39:29'),(102,1,16777343,'2010-10-31 14:18:55'),(103,1,16777343,'2010-10-31 14:29:28'),(104,1,16777343,'2010-10-31 17:14:31'),(105,1,16777343,'2010-10-31 17:22:27'),(106,1,16777343,'2010-10-31 19:14:01'),(107,1,16777343,'2010-10-31 19:17:05'),(108,1,16777343,'2010-10-31 19:18:46'),(109,1,16777343,'2010-10-31 19:20:42'),(110,1,16777343,'2010-10-31 19:28:22'),(111,1,16777343,'2010-10-31 19:43:07'),(112,1,16777343,'2010-11-01 12:15:59'),(113,1,16777343,'2010-11-01 12:30:35'),(114,1,16777343,'2010-11-01 12:30:42'),(115,1,16777343,'2010-11-01 13:54:57'),(116,1,16777343,'2010-11-01 14:05:28');
/*!40000 ALTER TABLE `account_ips` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `active_trade_cash`
--

DROP TABLE IF EXISTS `active_trade_cash`;
CREATE TABLE `active_trade_cash` (
  `character_id` int(11) NOT NULL,
  `cash` int(11) NOT NULL,
  PRIMARY KEY (`character_id`),
  CONSTRAINT `active_trade_cash_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

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
CREATE TABLE `active_trade_item` (
  `item_id` int(11) NOT NULL COMMENT 'The unique ID of the row.',
  `character_id` int(11) NOT NULL COMMENT 'The ID of the character that added the item.',
  PRIMARY KEY (`item_id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `active_trade_item_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `active_trade_item_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

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
  `character_template_id` smallint(5) unsigned DEFAULT NULL,
  `name` varchar(60) NOT NULL DEFAULT '' COMMENT 'The character''s name. Prefixed with `~<ID>_` when its a deleted user. The ~ denotes deleted, and the <ID> ensures a unique value.',
  `shop_id` smallint(5) unsigned DEFAULT NULL,
  `chat_dialog` smallint(5) unsigned DEFAULT NULL,
  `ai_id` smallint(5) unsigned DEFAULT NULL,
  `load_map_id` smallint(5) unsigned NOT NULL DEFAULT '3',
  `load_x` smallint(5) unsigned NOT NULL DEFAULT '1024',
  `load_y` smallint(5) unsigned NOT NULL DEFAULT '600',
  `respawn_map_id` smallint(5) unsigned DEFAULT '3',
  `respawn_x` float NOT NULL DEFAULT '1024',
  `respawn_y` float NOT NULL DEFAULT '600',
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
  UNIQUE KEY `idx_name` (`name`),
  KEY `template_id` (`character_template_id`),
  KEY `character_ibfk_2` (`load_map_id`),
  KEY `shop_id` (`shop_id`),
  KEY `character_ibfk_5` (`respawn_map_id`),
  CONSTRAINT `character_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_3` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_4` FOREIGN KEY (`load_map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_5` FOREIGN KEY (`respawn_map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `character`
--

LOCK TABLES `character` WRITE;
/*!40000 ALTER TABLE `character` DISABLE KEYS */;
INSERT INTO `character` VALUES (0,NULL,'Test',NULL,NULL,NULL,3,1024,600,3,1024,600,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1),(1,NULL,'Spodi',NULL,NULL,NULL,1,1024,600,3,1024,600,1,1800,201985,50,1480,243,100,100,100,100,1,1,1,1,3,1),(2,1,'Test A',NULL,NULL,1,2,535,1201,2,800,250,1,1800,3012,12,810,527,3,5,5,5,5,5,0,5,5,5),(3,1,'Test B',NULL,NULL,1,2,3,1330,2,500,250,1,1800,3012,12,810,527,5,5,5,5,5,5,0,5,5,5),(4,NULL,'testchar',NULL,NULL,NULL,3,1024,600,3,1024,600,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1);
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
INSERT INTO `character_template` VALUES (0,0,'User Template',NULL,NULL,NULL,1,1800,5,1,0,0,0,0,50,50,1,2,1,1,1,1),(1,1,'A Test NPC',1,NULL,NULL,2,1800,10,1,0,0,5,5,5,5,1,2,1,1,1,1),(2,2,'Quest Giver',NULL,NULL,NULL,2,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(4,2,'Potion Vendor',NULL,1,NULL,2,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(5,2,'Talking Guy',NULL,NULL,0,1,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(6,3,'Brawler',1,NULL,NULL,2,1800,8,1,0,0,8,8,5,5,1,3,1,1,1,1);
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
INSERT INTO `character_template_equipped` VALUES (0,1,3,60000),(1,1,5,30000);
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
INSERT INTO `character_template_inventory` VALUES (0,1,5,0,2,10000),(1,1,7,1,10,65535),(2,1,3,1,1,5000),(3,1,5,0,2,10000),(4,1,7,1,10,65535),(5,1,3,1,1,5000),(6,1,7,1,10,65535),(7,1,3,1,1,5000),(8,1,3,1,1,5000),(9,1,7,1,10,65535),(10,1,3,1,1,5000),(11,1,7,1,10,65535),(12,1,3,1,1,5000),(13,1,5,0,2,10000);
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
  `action_display_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ActionDisplayID to use when using this item.',
  PRIMARY KEY (`id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `item_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) TYPE=InnoDB;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES (0,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',10,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(1,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',11,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(2,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',12,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(3,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',13,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(4,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',14,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(5,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',15,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(6,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',16,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(7,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',17,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(8,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',18,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(9,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',19,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(10,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',10,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(11,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',11,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(12,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',12,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(13,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',13,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(14,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',14,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(15,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',15,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(16,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',16,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(17,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',17,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(18,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',18,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(19,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',19,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(20,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',20,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(21,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',21,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(22,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',22,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(23,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',23,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(24,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',24,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(25,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',25,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(26,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',26,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(27,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',27,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(28,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',28,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(29,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',29,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(30,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(31,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',31,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(32,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(33,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(34,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(35,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(36,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(37,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',21,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(38,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(39,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(40,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(41,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(42,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(43,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',26,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(44,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(45,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(46,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(47,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',34,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(48,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(49,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(50,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',38,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(51,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(52,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(53,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',18,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(54,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(55,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(56,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(57,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',22,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(58,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(59,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(60,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(61,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(62,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(63,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(64,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',28,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(65,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(66,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(67,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(68,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(69,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(70,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(71,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(72,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',19,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(73,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(74,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(75,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(76,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(77,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(78,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(79,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(80,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',25,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(81,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(82,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(83,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(84,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',20,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(85,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(86,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(87,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',27,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(88,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(89,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(90,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',28,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(91,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(92,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(93,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(94,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(95,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(96,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',23,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(97,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(98,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(99,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(100,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',20,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(101,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(102,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(103,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',21,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(104,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(105,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(106,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(107,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',33,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(108,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(109,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',18,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(110,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(111,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',36,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(112,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(113,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(114,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',43,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(115,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(116,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(117,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',28,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(118,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(119,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(120,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',14,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(121,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(122,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(123,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',38,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(124,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(125,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',27,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(126,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(127,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(128,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(129,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',20,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(130,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(131,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(132,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',22,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(133,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(134,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',19,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(135,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(136,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',20,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(137,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(138,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',34,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(139,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(140,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(141,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(142,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(143,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(144,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',36,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(145,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(146,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(147,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(148,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(149,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',36,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(150,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(151,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(152,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(153,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(154,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(155,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(156,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(157,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(158,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',27,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(159,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(160,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(161,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',38,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(162,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(163,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(164,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',20,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(165,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(166,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(167,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(168,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',28,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(169,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(170,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(171,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(172,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',23,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(173,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(174,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(175,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(176,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(177,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(178,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',38,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(179,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(180,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(181,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',21,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(182,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(183,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(184,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(185,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(186,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(187,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(188,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(189,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',27,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(190,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(191,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',31,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(192,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(193,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(194,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(195,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(196,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(197,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(198,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(199,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',25,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(200,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(201,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(202,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(203,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(204,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(205,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',31,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(206,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(207,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(208,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',26,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(209,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(210,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(211,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',19,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(212,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(213,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(214,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(215,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(216,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',28,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(217,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(218,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(219,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',33,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(220,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(221,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',26,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(222,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(223,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(224,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(225,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(226,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(227,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(228,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',22,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(229,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(230,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',20,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(231,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(232,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(233,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(234,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(235,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(236,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(237,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(238,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(239,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(240,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(241,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(242,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',35,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(243,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(244,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(245,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',25,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(246,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(247,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(248,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(249,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',21,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(250,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(251,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(252,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(253,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',19,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(254,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(255,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(256,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',20,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(257,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(258,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(259,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(260,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',33,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(261,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(262,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(263,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',34,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(264,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(265,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',22,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(266,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(267,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',30,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(268,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(269,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',33,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(270,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(271,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(272,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(273,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(274,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(275,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(276,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(277,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',7,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(278,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(279,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',35,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(280,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(281,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',22,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(282,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(283,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(284,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(285,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(286,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(287,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',31,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(288,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(289,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(290,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(291,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(292,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(293,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(294,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(295,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',23,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(296,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(297,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(298,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(299,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(300,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(301,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',27,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(302,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(303,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',3,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(304,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',25,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(305,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(306,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(307,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',23,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(308,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(309,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(310,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(311,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(312,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',31,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(313,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(314,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(315,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',22,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(316,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',23,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(317,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(318,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(319,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(320,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',21,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(321,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',3,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(322,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(323,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(324,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(325,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',35,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(326,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(327,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(328,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(329,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',26,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(330,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(331,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(332,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(333,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(334,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(335,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(336,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(337,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(338,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(339,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(340,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(341,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',29,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(342,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(343,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',15,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(344,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(345,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(346,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(347,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(348,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(349,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(350,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(351,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',17,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(352,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(353,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(354,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(355,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',26,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(356,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(357,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(358,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(359,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',34,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(360,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(361,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(362,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(363,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',24,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(364,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(365,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(366,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(367,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',22,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(368,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(369,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(370,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(371,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',25,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(372,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(373,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',31,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(374,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(375,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(376,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(377,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',33,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(378,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(379,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(380,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',27,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(381,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(382,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(383,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',2,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(384,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',28,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(385,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',2,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(386,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(387,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(388,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',25,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(389,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(390,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',32,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(391,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(392,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(393,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',34,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(394,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(395,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(396,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(398,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',21,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1);
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
  `action_display_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ActionDisplayID to use when using this item.',
  PRIMARY KEY (`id`)
) TYPE=InnoDB;

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
INSERT INTO `map` VALUES (1,'Desert 1'),(2,'Desert 2'),(3,'Checkered'),(4,'Winter');
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
) TYPE=InnoDB AUTO_INCREMENT=8;

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
INSERT INTO `server_time` VALUES ('2010-11-01 14:41:45');
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
CREATE TABLE `world_stats_count_consume_item` (
  `item_template_id` smallint(5) unsigned NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `last_update` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_consume_item_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

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
CREATE TABLE `world_stats_count_item_buy` (
  `item_template_id` smallint(5) unsigned NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `last_update` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_item_buy_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB ROW_FORMAT=COMPACT;

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
CREATE TABLE `world_stats_count_item_create` (
  `item_template_id` smallint(5) unsigned NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `last_update` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_item_create_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB ROW_FORMAT=COMPACT;

--
-- Dumping data for table `world_stats_count_item_create`
--

LOCK TABLES `world_stats_count_item_create` WRITE;
/*!40000 ALTER TABLE `world_stats_count_item_create` DISABLE KEYS */;
INSERT INTO `world_stats_count_item_create` VALUES (3,860,'0000-00-00 00:00:00'),(5,523,'0000-00-00 00:00:00'),(7,15640,'0000-00-00 00:00:00');
/*!40000 ALTER TABLE `world_stats_count_item_create` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_item_sell`
--

DROP TABLE IF EXISTS `world_stats_count_item_sell`;
CREATE TABLE `world_stats_count_item_sell` (
  `item_template_id` smallint(5) unsigned NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `last_update` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_item_sell_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

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
CREATE TABLE `world_stats_count_npc_kill_user` (
  `user_id` int(11) NOT NULL,
  `npc_template_id` smallint(5) unsigned NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `last_update` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`user_id`,`npc_template_id`),
  KEY `npc_template_id` (`npc_template_id`),
  CONSTRAINT `world_stats_count_npc_kill_user_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_count_npc_kill_user_ibfk_2` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB ROW_FORMAT=COMPACT;

--
-- Dumping data for table `world_stats_count_npc_kill_user`
--

LOCK TABLES `world_stats_count_npc_kill_user` WRITE;
/*!40000 ALTER TABLE `world_stats_count_npc_kill_user` DISABLE KEYS */;
INSERT INTO `world_stats_count_npc_kill_user` VALUES (1,1,16,'0000-00-00 00:00:00');
/*!40000 ALTER TABLE `world_stats_count_npc_kill_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_shop_buy`
--

DROP TABLE IF EXISTS `world_stats_count_shop_buy`;
CREATE TABLE `world_stats_count_shop_buy` (
  `shop_id` smallint(5) unsigned NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `last_update` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`shop_id`),
  CONSTRAINT `world_stats_count_shop_buy_ibfk_2` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB ROW_FORMAT=COMPACT;

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
CREATE TABLE `world_stats_count_shop_sell` (
  `shop_id` smallint(5) unsigned NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `last_update` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`shop_id`),
  CONSTRAINT `world_stats_count_shop_sell_ibfk_2` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB ROW_FORMAT=COMPACT;

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
CREATE TABLE `world_stats_count_user_consume_item` (
  `user_id` int(11) NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `last_update` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`user_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `world_stats_count_user_consume_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_count_user_consume_item_ibfk_3` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

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
CREATE TABLE `world_stats_count_user_kill_npc` (
  `user_id` int(11) NOT NULL,
  `npc_template_id` smallint(5) unsigned NOT NULL,
  `count` int(11) NOT NULL DEFAULT '0',
  `last_update` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`user_id`,`npc_template_id`),
  KEY `npc_template_id` (`npc_template_id`),
  CONSTRAINT `world_stats_count_user_kill_npc_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_count_user_kill_npc_ibfk_2` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB;

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
CREATE TABLE `world_stats_guild_user_change` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user who changed the guild they are part of.',
  `guild_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the guild, or null if the user left a guild.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
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
  `id` mediumint(8) unsigned NOT NULL AUTO_INCREMENT,
  `when` timestamp NOT NULL COMMENT 'The time the snapshot took place.',
  `connections` smallint(5) unsigned NOT NULL COMMENT 'Number of connections to the server at the time of the snapshot.',
  `recv_bytes` mediumint(8) unsigned NOT NULL COMMENT 'The average bytes received per second since the last snapshot.',
  `recv_packets` mediumint(8) unsigned NOT NULL COMMENT 'The average packets received per second since the last snapshot.',
  `recv_messages` mediumint(8) unsigned NOT NULL COMMENT 'The average messages received per second since the last snapshot.',
  `sent_bytes` mediumint(8) unsigned NOT NULL COMMENT 'The average bytes sent per second since the last snapshot.',
  `sent_packets` mediumint(8) unsigned NOT NULL COMMENT 'The average packets sent per second since the last snapshot.',
  `sent_messages` mediumint(8) unsigned NOT NULL COMMENT 'The average messages sent per second since the last snapshot.',
  PRIMARY KEY (`id`)
) TYPE=MyISAM AUTO_INCREMENT=731;

--
-- Dumping data for table `world_stats_network`
--

LOCK TABLES `world_stats_network` WRITE;
/*!40000 ALTER TABLE `world_stats_network` DISABLE KEYS */;
INSERT INTO `world_stats_network` VALUES (695,'2010-11-01 21:06:27',1,992,31,62,4863,56,263),(696,'2010-11-01 21:07:27',1,941,38,75,4216,58,233),(697,'2010-11-01 21:08:27',1,241,20,40,772,23,40),(698,'2010-11-01 21:09:27',1,247,21,41,808,23,41),(699,'2010-11-01 21:10:27',1,233,20,39,731,22,38),(700,'2010-11-01 21:11:27',1,258,21,43,856,25,43),(701,'2010-11-01 21:12:27',1,238,20,39,769,23,40),(704,'2010-11-01 21:15:27',1,236,20,40,761,23,39),(705,'2010-11-01 21:16:27',1,256,21,42,840,24,43),(706,'2010-11-01 21:17:27',1,242,20,40,780,23,40),(707,'2010-11-01 21:18:27',1,239,20,39,775,23,40),(708,'2010-11-01 21:19:27',1,221,18,36,726,21,37),(709,'2010-11-01 21:20:27',1,231,20,39,726,22,38),(710,'2010-11-01 21:21:27',1,246,20,41,816,23,41),(711,'2010-11-01 21:22:27',1,228,19,38,734,21,38),(712,'2010-11-01 21:23:27',1,239,21,41,754,23,39),(713,'2010-11-01 21:24:27',1,262,22,43,861,25,43),(714,'2010-11-01 21:25:27',1,232,20,40,724,22,37),(715,'2010-11-01 21:26:27',1,244,21,41,801,24,40),(716,'2010-11-01 21:27:27',1,234,19,39,758,23,39),(717,'2010-11-01 21:28:27',1,118,10,20,385,12,19),(718,'2010-11-01 21:29:27',1,3,0,0,3,0,0),(719,'2010-11-01 21:30:27',1,3,0,0,3,0,0),(720,'2010-11-01 21:31:27',1,3,0,0,3,0,0),(721,'2010-11-01 21:32:27',1,3,0,0,3,0,0),(722,'2010-11-01 21:33:27',1,3,0,0,3,0,0),(723,'2010-11-01 21:34:27',1,3,0,0,3,0,0),(724,'2010-11-01 21:35:27',1,3,0,0,3,0,0),(725,'2010-11-01 21:36:27',1,3,0,0,3,0,0),(726,'2010-11-01 21:37:27',1,3,0,0,3,0,0),(727,'2010-11-01 21:38:27',1,3,0,0,3,0,0),(728,'2010-11-01 21:39:27',1,3,0,0,3,0,0),(729,'2010-11-01 21:40:27',1,3,0,0,3,0,0),(730,'2010-11-01 21:41:27',1,3,0,0,3,0,0),(702,'2010-11-01 21:13:27',1,274,22,44,938,27,47),(703,'2010-11-01 21:14:27',1,225,19,37,708,22,38);
/*!40000 ALTER TABLE `world_stats_network` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_npc_kill_user`
--

DROP TABLE IF EXISTS `world_stats_npc_kill_user`;
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
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
  KEY `user_id` (`user_id`),
  KEY `npc_template_id` (`npc_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `world_stats_npc_kill_user_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_npc_kill_user_ibfk_2` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `world_stats_npc_kill_user_ibfk_3` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) TYPE=InnoDB AUTO_INCREMENT=17;

--
-- Dumping data for table `world_stats_npc_kill_user`
--

LOCK TABLES `world_stats_npc_kill_user` WRITE;
/*!40000 ALTER TABLE `world_stats_npc_kill_user` DISABLE KEYS */;
INSERT INTO `world_stats_npc_kill_user` VALUES (1,1,1,50,1024,593,1000,754,3,'2010-10-29 19:39:41'),(2,1,1,50,1024,593,957,754,3,'2010-10-29 19:39:55'),(3,1,1,50,1024,593,821,689,3,'2010-10-29 19:40:06'),(4,1,1,50,1024,593,790,754,3,'2010-10-29 19:40:20'),(5,1,1,50,1024,593,1017,754,3,'2010-10-29 19:40:33'),(6,1,1,50,1024,593,881,754,3,'2010-10-29 19:40:41'),(7,1,1,50,1024,593,731,754,3,'2010-10-29 19:56:31'),(8,1,1,50,1024,593,911,754,3,'2010-10-29 19:56:48'),(9,1,1,50,1024,600,858,658,1,'2010-10-30 22:22:50'),(10,1,1,50,1024,600,1009,658,1,'2010-10-30 22:23:17'),(11,1,1,50,1024,600,944,630,1,'2010-10-30 22:23:30'),(12,1,1,50,1024,593,134,754,3,'2010-10-30 22:23:40'),(13,1,1,50,1024,593,1014,658,1,'2010-10-30 23:06:20'),(14,1,1,50,1024,593,1061,658,1,'2010-10-30 23:08:55'),(15,1,1,50,1024,593,1014,594,3,'2010-10-31 00:29:28'),(16,1,1,50,1024,593,749,498,3,'2010-10-31 00:39:34');
/*!40000 ALTER TABLE `world_stats_npc_kill_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_quest_accept`
--

DROP TABLE IF EXISTS `world_stats_quest_accept`;
CREATE TABLE `world_stats_quest_accept` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user that accepted the quest.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that was accepted.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
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
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user that canceled the quest.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that was canceled.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
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
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user that completed the quest.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that was completed.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
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
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The user that this event is related to.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template ID of the item that was consumed. Only valid when the item has a set template ID.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The map the user was on when this event took place.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
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
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL COMMENT 'The ID of the user.',
  `npc_template_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The template ID of the NPC. Only valid when the NPC has a template ID set.',
  `user_level` tinyint(3) unsigned NOT NULL COMMENT 'The level of the user was when this event took place.',
  `user_x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `user_y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `npc_x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the NPC when this event took place. Only valid when the map_id is not null.',
  `npc_y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the NPC when this event took place. Only valid when the map_id is not null.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
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
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `character_id` int(11) NOT NULL COMMENT 'The ID of the character that leveled up.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `level` tinyint(3) unsigned NOT NULL COMMENT 'The level that the character leveled up to (their new level).',
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  PRIMARY KEY (`id`)
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
  `when` timestamp NOT NULL COMMENT 'When this event took place.',
  PRIMARY KEY (`id`),
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
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`%`*/ /*!50003 FUNCTION `create_user_on_account`(accountName VARCHAR(50), characterName VARCHAR(30), characterID INT) RETURNS varchar(100) CHARSET latin1
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
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`%`*/ /*!50003 PROCEDURE `delete_user_on_account`(characterID INT)
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
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`%`*/ /*!50003 PROCEDURE `rebuild_views`()
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
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`%`*/ /*!50003 PROCEDURE `rebuild_view_npc_character`()
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
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`%`*/ /*!50003 PROCEDURE `rebuild_view_user_character`()
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
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2010-11-01 14:42:09
