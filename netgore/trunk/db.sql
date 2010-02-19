-- MySQL dump 10.13  Distrib 5.1.38, for Win64 (unknown)
--
-- Host: localhost    Database: demogame
-- ------------------------------------------------------
-- Server version	5.1.38-community

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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account`
--

LOCK TABLES `account` WRITE;
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
INSERT INTO `account` VALUES (0,'Test','3fc0a7acf087f549ac2b266baf94b8b1','test@test.com','2010-02-11 17:52:28','2010-02-11 18:03:56',16777343,NULL),(1,'Spodi','3fc0a7acf087f549ac2b266baf94b8b1','spodi@netgore.com','2009-09-07 15:43:16','2010-02-19 12:40:09',16777343,NULL);
/*!40000 ALTER TABLE `account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_ips`
--

DROP TABLE IF EXISTS `account_ips`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account_ips` (
  `account_id` int(11) NOT NULL COMMENT 'The ID of the account.',
  `ip` int(10) unsigned NOT NULL COMMENT 'The IP that logged into the account.',
  `time` datetime NOT NULL COMMENT 'When this IP last logged into this account.',
  PRIMARY KEY (`account_id`,`time`),
  KEY `account_id` (`account_id`,`ip`),
  CONSTRAINT `account_ips_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_ips`
--

LOCK TABLES `account_ips` WRITE;
/*!40000 ALTER TABLE `account_ips` DISABLE KEYS */;
INSERT INTO `account_ips` VALUES (0,16777343,'2010-02-11 17:52:39'),(0,16777343,'2010-02-11 17:57:51'),(0,16777343,'2010-02-11 18:03:56'),(1,16777343,'2010-01-24 15:08:41'),(1,16777343,'2010-01-24 15:36:10'),(1,16777343,'2010-01-24 15:36:18'),(1,16777343,'2010-01-24 15:36:35'),(1,16777343,'2010-01-24 15:36:56'),(1,16777343,'2010-01-24 15:38:05'),(1,16777343,'2010-01-24 15:41:14'),(1,16777343,'2010-01-24 15:42:08'),(1,16777343,'2010-01-24 15:42:51'),(1,16777343,'2010-01-24 15:49:14'),(1,16777343,'2010-01-24 15:50:59'),(1,16777343,'2010-01-24 15:51:57'),(1,16777343,'2010-01-24 15:54:50'),(1,16777343,'2010-01-24 15:56:23'),(1,16777343,'2010-01-24 16:00:20'),(1,16777343,'2010-01-26 20:44:25'),(1,16777343,'2010-01-26 21:22:42'),(1,16777343,'2010-01-26 21:23:37'),(1,16777343,'2010-01-26 21:25:00'),(1,16777343,'2010-01-26 21:33:59'),(1,16777343,'2010-01-26 21:34:27'),(1,16777343,'2010-01-27 10:53:46'),(1,16777343,'2010-01-27 10:55:21'),(1,16777343,'2010-01-27 10:56:12'),(1,16777343,'2010-01-27 10:58:09'),(1,16777343,'2010-01-27 11:00:19'),(1,16777343,'2010-01-27 11:38:01'),(1,16777343,'2010-01-27 11:49:55'),(1,16777343,'2010-01-27 11:51:50'),(1,16777343,'2010-01-27 18:02:34'),(1,16777343,'2010-01-28 21:20:14'),(1,16777343,'2010-01-28 21:24:00'),(1,16777343,'2010-01-28 21:26:39'),(1,16777343,'2010-01-29 00:03:38'),(1,16777343,'2010-01-29 00:11:26'),(1,16777343,'2010-01-29 00:12:08'),(1,16777343,'2010-01-29 11:46:34'),(1,16777343,'2010-01-29 11:47:25'),(1,16777343,'2010-01-29 11:49:06'),(1,16777343,'2010-01-29 11:52:22'),(1,16777343,'2010-01-29 11:53:04'),(1,16777343,'2010-01-29 11:54:32'),(1,16777343,'2010-01-29 12:12:17'),(1,16777343,'2010-01-31 12:05:42'),(1,16777343,'2010-01-31 12:07:35'),(1,16777343,'2010-01-31 12:08:27'),(1,16777343,'2010-01-31 12:08:56'),(1,16777343,'2010-01-31 12:12:53'),(1,16777343,'2010-01-31 12:13:49'),(1,16777343,'2010-01-31 12:15:50'),(1,16777343,'2010-01-31 12:17:59'),(1,16777343,'2010-01-31 12:19:02'),(1,16777343,'2010-01-31 12:19:27'),(1,16777343,'2010-01-31 12:20:15'),(1,16777343,'2010-01-31 12:21:39'),(1,16777343,'2010-01-31 12:22:35'),(1,16777343,'2010-01-31 12:25:48'),(1,16777343,'2010-01-31 12:27:27'),(1,16777343,'2010-01-31 12:28:51'),(1,16777343,'2010-01-31 13:23:45'),(1,16777343,'2010-01-31 13:49:28'),(1,16777343,'2010-01-31 13:54:15'),(1,16777343,'2010-01-31 13:58:34'),(1,16777343,'2010-01-31 14:36:36'),(1,16777343,'2010-01-31 14:53:56'),(1,16777343,'2010-01-31 15:07:16'),(1,16777343,'2010-01-31 15:50:47'),(1,16777343,'2010-01-31 17:39:41'),(1,16777343,'2010-01-31 17:43:39'),(1,16777343,'2010-01-31 17:47:20'),(1,16777343,'2010-01-31 18:06:58'),(1,16777343,'2010-01-31 18:08:35'),(1,16777343,'2010-01-31 18:10:00'),(1,16777343,'2010-01-31 18:12:35'),(1,16777343,'2010-01-31 18:17:18'),(1,16777343,'2010-01-31 18:25:10'),(1,16777343,'2010-01-31 18:26:38'),(1,16777343,'2010-02-01 16:53:41'),(1,16777343,'2010-02-02 21:24:09'),(1,16777343,'2010-02-02 22:15:56'),(1,16777343,'2010-02-02 22:17:44'),(1,16777343,'2010-02-02 22:47:54'),(1,16777343,'2010-02-02 22:53:47'),(1,16777343,'2010-02-02 23:08:01'),(1,16777343,'2010-02-02 23:23:17'),(1,16777343,'2010-02-02 23:24:57'),(1,16777343,'2010-02-11 17:47:35'),(1,16777343,'2010-02-11 17:50:41'),(1,16777343,'2010-02-11 17:51:53'),(1,16777343,'2010-02-11 17:58:08'),(1,16777343,'2010-02-11 18:03:47'),(1,16777343,'2010-02-11 19:06:57'),(1,16777343,'2010-02-11 19:08:24'),(1,16777343,'2010-02-11 19:09:15'),(1,16777343,'2010-02-12 11:57:52'),(1,16777343,'2010-02-12 12:03:56'),(1,16777343,'2010-02-12 12:08:22'),(1,16777343,'2010-02-13 18:36:52'),(1,16777343,'2010-02-13 18:37:34'),(1,16777343,'2010-02-13 18:38:13'),(1,16777343,'2010-02-13 18:51:22'),(1,16777343,'2010-02-13 18:52:47'),(1,16777343,'2010-02-13 18:56:34'),(1,16777343,'2010-02-13 19:02:58'),(1,16777343,'2010-02-13 19:06:52'),(1,16777343,'2010-02-13 19:09:12'),(1,16777343,'2010-02-13 19:09:46'),(1,16777343,'2010-02-13 19:15:23'),(1,16777343,'2010-02-13 19:16:01'),(1,16777343,'2010-02-13 19:16:34'),(1,16777343,'2010-02-13 19:19:42'),(1,16777343,'2010-02-15 12:46:32'),(1,16777343,'2010-02-15 12:55:24'),(1,16777343,'2010-02-15 13:27:25'),(1,16777343,'2010-02-15 13:28:07'),(1,16777343,'2010-02-15 13:29:30'),(1,16777343,'2010-02-15 13:51:22'),(1,16777343,'2010-02-15 13:53:07'),(1,16777343,'2010-02-15 14:18:52'),(1,16777343,'2010-02-15 14:19:41'),(1,16777343,'2010-02-15 14:30:56'),(1,16777343,'2010-02-15 15:17:33'),(1,16777343,'2010-02-15 17:31:27'),(1,16777343,'2010-02-15 17:36:02'),(1,16777343,'2010-02-15 18:10:08'),(1,16777343,'2010-02-15 18:13:35'),(1,16777343,'2010-02-15 18:15:45'),(1,16777343,'2010-02-15 18:17:47'),(1,16777343,'2010-02-15 18:20:30'),(1,16777343,'2010-02-15 18:22:50'),(1,16777343,'2010-02-15 18:29:27'),(1,16777343,'2010-02-15 18:59:57'),(1,16777343,'2010-02-15 19:06:08'),(1,16777343,'2010-02-15 19:28:02'),(1,16777343,'2010-02-17 22:34:55'),(1,16777343,'2010-02-19 12:40:09');
/*!40000 ALTER TABLE `account_ips` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `alliance`
--

DROP TABLE IF EXISTS `alliance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `alliance` (
  `id` tinyint(3) unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `alliance`
--

LOCK TABLES `alliance` WRITE;
/*!40000 ALTER TABLE `alliance` DISABLE KEYS */;
INSERT INTO `alliance` VALUES (0,'user'),(1,'monster');
/*!40000 ALTER TABLE `alliance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `alliance_attackable`
--

DROP TABLE IF EXISTS `alliance_attackable`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `alliance_attackable`
--

LOCK TABLES `alliance_attackable` WRITE;
/*!40000 ALTER TABLE `alliance_attackable` DISABLE KEYS */;
INSERT INTO `alliance_attackable` VALUES (0,1,NULL),(1,0,NULL);
/*!40000 ALTER TABLE `alliance_attackable` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `alliance_hostile`
--

DROP TABLE IF EXISTS `alliance_hostile`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `alliance_hostile`
--

LOCK TABLES `alliance_hostile` WRITE;
/*!40000 ALTER TABLE `alliance_hostile` DISABLE KEYS */;
INSERT INTO `alliance_hostile` VALUES (0,1,0),(1,0,0);
/*!40000 ALTER TABLE `alliance_hostile` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character`
--

DROP TABLE IF EXISTS `character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character`
--

LOCK TABLES `character` WRITE;
/*!40000 ALTER TABLE `character` DISABLE KEYS */;
INSERT INTO `character` VALUES (0,0,NULL,'Test',1,NULL,NULL,NULL,391.6,244,NULL,50,50,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1),(1,1,NULL,'Spodi',1,NULL,NULL,NULL,734.999,658,1,500,200,1,1800,3546,141,4224,678,50,50,50,50,7,12,0,1,1,2),(2,NULL,1,'Test A',2,NULL,NULL,1,800,250,2,800,250,1,1800,3012,12,810,527,5,5,5,5,5,5,0,5,5,5),(3,NULL,1,'Test B',2,NULL,NULL,1,506,250,2,500,250,1,1800,3012,12,810,527,5,5,5,5,5,5,0,5,5,5),(4,NULL,NULL,'Talking Guy',2,NULL,0,NULL,800,530,2,800,530,1,1800,0,1,0,0,50,50,50,50,1,1,0,1,1,1),(5,NULL,NULL,'Shopkeeper',2,0,NULL,NULL,600,530,2,600,530,1,1800,0,1,0,0,50,50,50,50,1,1,0,1,1,1),(6,NULL,NULL,'Vending Machine',2,1,NULL,NULL,500,530,2,500,530,1,1800,0,1,0,0,50,50,50,50,1,1,0,1,1,1);
/*!40000 ALTER TABLE `character` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_equipped`
--

DROP TABLE IF EXISTS `character_equipped`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_equipped` (
  `character_id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `slot` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`character_id`,`slot`),
  KEY `item_id` (`item_id`),
  CONSTRAINT `character_equipped_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_equipped_ibfk_4` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_inventory`
--

LOCK TABLES `character_inventory` WRITE;
/*!40000 ALTER TABLE `character_inventory` DISABLE KEYS */;
INSERT INTO `character_inventory` VALUES (1,18,4),(1,20,1),(1,21,2),(1,23,3),(1,35,0);
/*!40000 ALTER TABLE `character_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_quest_status`
--

DROP TABLE IF EXISTS `character_quest_status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_quest_status` (
  `character_id` int(11) NOT NULL,
  `quest_id` smallint(5) unsigned NOT NULL,
  `started_on` datetime NOT NULL,
  `completed_on` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_template`
--

LOCK TABLES `character_template` WRITE;
/*!40000 ALTER TABLE `character_template` DISABLE KEYS */;
INSERT INTO `character_template` VALUES (0,0,'User Template',NULL,NULL,1,1800,5,1,0,0,0,0,50,50,1,2,0,1,1,1),(1,1,'A Test NPC',1,NULL,2,1800,2,0,0,0,5,5,5,5,0,0,0,1,1,1);
/*!40000 ALTER TABLE `character_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_equipped`
--

DROP TABLE IF EXISTS `character_template_equipped`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_template_equipped`
--

LOCK TABLES `character_template_equipped` WRITE;
/*!40000 ALTER TABLE `character_template_equipped` DISABLE KEYS */;
INSERT INTO `character_template_equipped` VALUES (0,1,5,3000),(1,1,4,3000),(2,1,3,60000);
/*!40000 ALTER TABLE `character_template_equipped` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_inventory`
--

DROP TABLE IF EXISTS `character_template_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_template_inventory`
--

LOCK TABLES `character_template_inventory` WRITE;
/*!40000 ALTER TABLE `character_template_inventory` DISABLE KEYS */;
INSERT INTO `character_template_inventory` VALUES (0,1,5,0,2,10000),(1,1,4,1,2,5000),(2,1,3,1,1,5000),(3,1,2,0,1,10000),(4,1,1,0,5,10000),(5,1,6,1,1,10000),(6,1,7,0,10,30000);
/*!40000 ALTER TABLE `character_template_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `game_constant`
--

DROP TABLE IF EXISTS `game_constant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
  `server_ip` varchar(15) NOT NULL,
  `world_physics_update_rate` smallint(5) unsigned NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `guild` (
  `id` smallint(5) unsigned NOT NULL,
  `name` varchar(50) NOT NULL,
  `tag` varchar(5) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `guild`
--

LOCK TABLES `guild` WRITE;
/*!40000 ALTER TABLE `guild` DISABLE KEYS */;
INSERT INTO `guild` VALUES (0,'asdfasdf','tg');
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES (0,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(1,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(2,0,2,1,10,16,16,'Unarmed','Unarmed',1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(18,2,1,0,0,9,16,'Mana Potion','A mana potion',1,95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(20,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',2,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL),(21,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(23,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(35,1,1,0,0,9,16,'Healing Potion','A healing potion',1,94,10,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL);
/*!40000 ALTER TABLE `item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item_template`
--

DROP TABLE IF EXISTS `item_template`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `item_template` (
  `id` smallint(5) unsigned NOT NULL,
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `weapon_type` tinyint(3) unsigned NOT NULL,
  `ammo_type` tinyint(3) unsigned NOT NULL,
  `range` smallint(5) unsigned NOT NULL,
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item_template`
--

LOCK TABLES `item_template` WRITE;
/*!40000 ALTER TABLE `item_template` DISABLE KEYS */;
INSERT INTO `item_template` VALUES (0,2,1,0,10,16,16,'Unarmed','Unarmed',1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(1,1,0,0,0,9,16,'Healing Potion','A healing potion',94,10,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL),(2,1,0,0,0,9,16,'Mana Potion','A mana potion',95,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL),(3,2,0,0,20,24,24,'Titanium Sword','A sword made out of titanium',96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL),(4,4,0,0,0,22,22,'Crystal Armor','Body armor made out of crystal',99,50,0,0,0,0,0,0,0,0,0,5,0,0,0,'crystal body'),(5,3,0,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet'),(6,2,2,0,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL),(7,2,3,0,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL);
/*!40000 ALTER TABLE `item_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `map`
--

DROP TABLE IF EXISTS `map`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `map` (
  `id` smallint(5) unsigned NOT NULL,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `map`
--

LOCK TABLES `map` WRITE;
/*!40000 ALTER TABLE `map` DISABLE KEYS */;
INSERT INTO `map` VALUES (1,'INSERT VALUE'),(2,'INSERT VALUE');
/*!40000 ALTER TABLE `map` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `map_spawn`
--

DROP TABLE IF EXISTS `map_spawn`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `map_spawn`
--

LOCK TABLES `map_spawn` WRITE;
/*!40000 ALTER TABLE `map_spawn` DISABLE KEYS */;
INSERT INTO `map_spawn` VALUES (12,1,1,5,NULL,NULL,NULL,NULL),(13,1,1,1,NULL,NULL,NULL,NULL),(14,1,1,1,NULL,NULL,NULL,NULL);
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
  `map_id` smallint(5) unsigned,
  `shop_id` smallint(5) unsigned,
  `chat_dialog` smallint(5) unsigned,
  `ai_id` smallint(5) unsigned,
  `x` float,
  `y` float,
  `respawn_map` smallint(5) unsigned,
  `respawn_x` float,
  `respawn_y` float,
  `body_id` smallint(5) unsigned,
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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest` (
  `id` smallint(5) unsigned NOT NULL,
  `name` varchar(250) NOT NULL,
  `description` text NOT NULL,
  `repeatable` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `reward_cash` int(11) NOT NULL DEFAULT '0',
  `reward_exp` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest`
--

LOCK TABLES `quest` WRITE;
/*!40000 ALTER TABLE `quest` DISABLE KEYS */;
INSERT INTO `quest` VALUES (0,'Test Quest','Super duper awesome test quest of doom.',0,500,1000);
/*!40000 ALTER TABLE `quest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_require_finish_item`
--

DROP TABLE IF EXISTS `quest_require_finish_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_require_finish_item` (
  `quest_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  `amount` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_require_finish_item_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_finish_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_require_finish_item`
--

LOCK TABLES `quest_require_finish_item` WRITE;
/*!40000 ALTER TABLE `quest_require_finish_item` DISABLE KEYS */;
INSERT INTO `quest_require_finish_item` VALUES (0,4,1),(0,7,10);
/*!40000 ALTER TABLE `quest_require_finish_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_require_kill`
--

DROP TABLE IF EXISTS `quest_require_kill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_require_kill` (
  `quest_id` smallint(5) unsigned NOT NULL,
  `character_template_id` smallint(5) unsigned NOT NULL,
  `amount` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`character_template_id`),
  KEY `character_template_id` (`character_template_id`),
  CONSTRAINT `quest_require_kill_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_kill_ibfk_2` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
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
  `quest_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  `amount` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_require_start_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_start_item_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_require_start_item`
--

LOCK TABLES `quest_require_start_item` WRITE;
/*!40000 ALTER TABLE `quest_require_start_item` DISABLE KEYS */;
INSERT INTO `quest_require_start_item` VALUES (0,7,3);
/*!40000 ALTER TABLE `quest_require_start_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_require_start_quest`
--

DROP TABLE IF EXISTS `quest_require_start_quest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_require_start_quest` (
  `quest_id` smallint(5) unsigned NOT NULL,
  `req_quest_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`req_quest_id`),
  KEY `req_quest_id` (`req_quest_id`),
  CONSTRAINT `quest_require_start_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_start_quest_ibfk_2` FOREIGN KEY (`req_quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
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
  `quest_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  `amount` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_reward_item_ibfk_3` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_reward_item_ibfk_4` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_reward_item`
--

LOCK TABLES `quest_reward_item` WRITE;
/*!40000 ALTER TABLE `quest_reward_item` DISABLE KEYS */;
INSERT INTO `quest_reward_item` VALUES (0,1,1),(0,2,1),(0,3,1),(0,4,1),(0,7,10);
/*!40000 ALTER TABLE `quest_reward_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `server_setting`
--

DROP TABLE IF EXISTS `server_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `server_setting` (
  `motd` varchar(250) NOT NULL DEFAULT '' COMMENT 'The message of the day.'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `server_time` (
  `server_time` datetime NOT NULL,
  PRIMARY KEY (`server_time`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `server_time`
--

LOCK TABLES `server_time` WRITE;
/*!40000 ALTER TABLE `server_time` DISABLE KEYS */;
INSERT INTO `server_time` VALUES ('2010-02-19 12:40:15');
/*!40000 ALTER TABLE `server_time` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shop`
--

DROP TABLE IF EXISTS `shop`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `shop` (
  `id` smallint(5) unsigned NOT NULL,
  `name` varchar(60) NOT NULL,
  `can_buy` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shop`
--

LOCK TABLES `shop` WRITE;
/*!40000 ALTER TABLE `shop` DISABLE KEYS */;
INSERT INTO `shop` VALUES (0,'Test Shop',1),(1,'Soda Vending Machine',0);
/*!40000 ALTER TABLE `shop` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shop_item`
--

DROP TABLE IF EXISTS `shop_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `shop_item` (
  `shop_id` smallint(5) unsigned NOT NULL,
  `item_template_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`shop_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `shop_item_ibfk_1` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `shop_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shop_item`
--

LOCK TABLES `shop_item` WRITE;
/*!40000 ALTER TABLE `shop_item` DISABLE KEYS */;
INSERT INTO `shop_item` VALUES (0,1),(1,1),(0,2),(1,2),(0,3),(0,4),(0,5);
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
  `map_id` smallint(5) unsigned,
  `shop_id` smallint(5) unsigned,
  `chat_dialog` smallint(5) unsigned,
  `ai_id` smallint(5) unsigned,
  `x` float,
  `y` float,
  `respawn_map` smallint(5) unsigned,
  `respawn_x` float,
  `respawn_y` float,
  `body_id` smallint(5) unsigned,
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
/*!50003 DROP PROCEDURE IF EXISTS `TestProc` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `TestProc`(a INT)
BEGIN
		UPDATE `test` SET `fvalue`="asdfasdfasdfasdf" WHERE `id`=a;
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
/*!50001 VIEW `npc_character` AS select `character`.`id` AS `id`,`character`.`account_id` AS `account_id`,`character`.`character_template_id` AS `character_template_id`,`character`.`name` AS `name`,`character`.`map_id` AS `map_id`,`character`.`shop_id` AS `shop_id`,`character`.`chat_dialog` AS `chat_dialog`,`character`.`ai_id` AS `ai_id`,`character`.`x` AS `x`,`character`.`y` AS `y`,`character`.`respawn_map` AS `respawn_map`,`character`.`respawn_x` AS `respawn_x`,`character`.`respawn_y` AS `respawn_y`,`character`.`body_id` AS `body_id`,`character`.`cash` AS `cash`,`character`.`level` AS `level`,`character`.`exp` AS `exp`,`character`.`statpoints` AS `statpoints`,`character`.`hp` AS `hp`,`character`.`mp` AS `mp`,`character`.`stat_maxhp` AS `stat_maxhp`,`character`.`stat_maxmp` AS `stat_maxmp`,`character`.`stat_minhit` AS `stat_minhit`,`character`.`stat_maxhit` AS `stat_maxhit`,`character`.`stat_defence` AS `stat_defence`,`character`.`stat_agi` AS `stat_agi`,`character`.`stat_int` AS `stat_int`,`character`.`stat_str` AS `stat_str` from `character` where isnull(`character`.`account_id`) */;
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
/*!50001 SET character_set_client      = latin1 */;
/*!50001 SET character_set_results     = latin1 */;
/*!50001 SET collation_connection      = latin1_swedish_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `user_character` AS select `character`.`id` AS `id`,`character`.`account_id` AS `account_id`,`character`.`character_template_id` AS `character_template_id`,`character`.`name` AS `name`,`character`.`map_id` AS `map_id`,`character`.`shop_id` AS `shop_id`,`character`.`chat_dialog` AS `chat_dialog`,`character`.`ai_id` AS `ai_id`,`character`.`x` AS `x`,`character`.`y` AS `y`,`character`.`respawn_map` AS `respawn_map`,`character`.`respawn_x` AS `respawn_x`,`character`.`respawn_y` AS `respawn_y`,`character`.`body_id` AS `body_id`,`character`.`cash` AS `cash`,`character`.`level` AS `level`,`character`.`exp` AS `exp`,`character`.`statpoints` AS `statpoints`,`character`.`hp` AS `hp`,`character`.`mp` AS `mp`,`character`.`stat_maxhp` AS `stat_maxhp`,`character`.`stat_maxmp` AS `stat_maxmp`,`character`.`stat_minhit` AS `stat_minhit`,`character`.`stat_maxhit` AS `stat_maxhit`,`character`.`stat_defence` AS `stat_defence`,`character`.`stat_agi` AS `stat_agi`,`character`.`stat_int` AS `stat_int`,`character`.`stat_str` AS `stat_str` from `character` where (`character`.`account_id` is not null) */;
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

-- Dump completed on 2010-02-19 12:43:04
