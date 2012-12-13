-- MySQL dump 10.13  Distrib 5.5.27, for Win64 (x86)
--
-- Host: localhost    Database: demogame
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
INSERT INTO `account` VALUES (1,'Spodi','3fc0a7acf087f549ac2b266baf94b8b1','spodi@netgore.com',255,'2009-09-07 15:43:16','2012-12-12 19:16:26',16777343,NULL),(2,'Spodii','3FC0A7ACF087F549AC2B266BAF94B8B1','test@test.com',0,'2010-12-26 14:11:08','2010-12-26 14:14:23',16777343,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COMMENT='The bans (active and inactive) placed on accounts.';
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
INSERT INTO `account_character` VALUES (1,1,NULL),(2,2,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=180 DEFAULT CHARSET=latin1 COMMENT='The IPs used to access accounts.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_ips`
--

LOCK TABLES `account_ips` WRITE;
/*!40000 ALTER TABLE `account_ips` DISABLE KEYS */;
INSERT INTO `account_ips` VALUES (1,1,16777343,'2010-11-26 20:53:58'),(2,1,16777343,'2010-11-26 23:20:14'),(3,1,16777343,'2010-11-27 01:29:44'),(4,1,16777343,'2010-11-27 01:31:43'),(5,1,16777343,'2010-11-27 01:32:38'),(6,1,16777343,'2010-11-27 01:34:24'),(8,1,16777343,'2010-11-27 01:45:49'),(9,1,16777343,'2010-11-28 15:28:17'),(10,1,16777343,'2010-11-29 01:41:54'),(11,1,16777343,'2010-11-29 01:49:37'),(12,1,16777343,'2010-11-29 09:35:05'),(13,1,16777343,'2010-11-29 09:53:51'),(14,1,16777343,'2010-11-29 10:01:28'),(15,1,16777343,'2010-11-29 10:18:45'),(16,1,16777343,'2010-11-29 10:19:40'),(17,1,16777343,'2010-11-29 10:22:02'),(18,1,16777343,'2010-11-29 10:32:04'),(19,1,16777343,'2010-11-29 10:35:13'),(20,1,16777343,'2010-11-29 10:39:52'),(21,1,16777343,'2010-11-29 10:44:09'),(22,1,16777343,'2010-12-05 19:43:09'),(23,1,16777343,'2010-12-07 22:26:29'),(24,1,16777343,'2010-12-07 22:27:51'),(25,1,16777343,'2010-12-07 22:28:51'),(26,1,16777343,'2010-12-07 22:30:53'),(27,1,16777343,'2010-12-07 22:32:12'),(28,1,16777343,'2010-12-08 15:44:02'),(29,1,16777343,'2010-12-08 15:53:31'),(30,1,16777343,'2010-12-08 15:54:00'),(31,1,16777343,'2010-12-08 15:57:44'),(32,1,16777343,'2010-12-08 15:58:23'),(33,1,16777343,'2010-12-08 16:12:08'),(34,1,16777343,'2010-12-08 16:14:31'),(35,1,16777343,'2010-12-08 16:24:07'),(36,1,16777343,'2010-12-08 17:21:40'),(37,1,16777343,'2010-12-08 17:24:10'),(38,1,16777343,'2010-12-09 02:23:45'),(39,1,16777343,'2010-12-09 02:26:40'),(40,1,16777343,'2010-12-16 12:54:10'),(41,1,16777343,'2010-12-16 12:58:55'),(42,1,16777343,'2010-12-16 15:47:18'),(43,1,16777343,'2010-12-19 01:18:11'),(44,1,16777343,'2010-12-21 11:14:13'),(45,1,16777343,'2010-12-21 17:47:03'),(46,1,16777343,'2010-12-26 09:43:59'),(47,1,16777343,'2010-12-26 12:16:07'),(48,1,16777343,'2010-12-26 12:23:14'),(49,1,16777343,'2010-12-26 12:25:24'),(50,1,16777343,'2010-12-26 12:35:37'),(51,1,16777343,'2010-12-26 12:36:31'),(52,1,16777343,'2010-12-26 12:41:31'),(53,1,16777343,'2010-12-26 12:44:07'),(54,1,16777343,'2010-12-26 12:45:24'),(55,1,16777343,'2010-12-26 12:45:27'),(56,1,16777343,'2010-12-26 12:45:30'),(57,1,16777343,'2010-12-26 13:59:11'),(58,1,16777343,'2010-12-26 13:59:17'),(59,1,16777343,'2010-12-26 14:06:37'),(60,1,16777343,'2010-12-26 14:06:47'),(61,1,16777343,'2010-12-26 14:08:40'),(62,1,16777343,'2010-12-26 14:11:11'),(63,2,16777343,'2010-12-26 14:11:26'),(64,1,16777343,'2010-12-26 14:13:43'),(65,2,16777343,'2010-12-26 14:14:23'),(66,1,16777343,'2010-12-26 15:03:15'),(67,1,16777343,'2010-12-26 15:06:19'),(68,1,16777343,'2010-12-26 15:08:24'),(69,1,16777343,'2010-12-26 16:26:16'),(70,1,16777343,'2010-12-26 16:48:52'),(71,1,16777343,'2010-12-26 16:56:43'),(72,1,16777343,'2010-12-29 00:57:39'),(73,1,16777343,'2010-12-29 00:58:01'),(74,1,16777343,'2010-12-29 01:02:22'),(75,1,16777343,'2010-12-29 01:10:52'),(76,1,16777343,'2010-12-29 01:18:57'),(77,1,16777343,'2012-12-04 23:51:17'),(78,1,16777343,'2012-12-04 23:52:09'),(79,1,16777343,'2012-12-09 15:09:47'),(80,1,16777343,'2012-12-09 15:34:02'),(81,1,16777343,'2012-12-09 15:34:59'),(82,1,16777343,'2012-12-09 15:37:19'),(83,1,16777343,'2012-12-09 16:15:19'),(84,1,16777343,'2012-12-09 16:15:50'),(85,1,16777343,'2012-12-09 16:18:08'),(86,1,16777343,'2012-12-09 16:18:30'),(87,1,16777343,'2012-12-09 21:50:31'),(88,1,16777343,'2012-12-09 21:51:30'),(89,1,16777343,'2012-12-09 22:13:57'),(90,1,16777343,'2012-12-09 22:14:52'),(91,1,16777343,'2012-12-09 22:21:13'),(92,1,16777343,'2012-12-09 22:21:27'),(93,1,16777343,'2012-12-09 22:25:06'),(94,1,16777343,'2012-12-09 22:25:13'),(95,1,16777343,'2012-12-09 22:27:53'),(96,1,16777343,'2012-12-09 22:28:37'),(97,1,16777343,'2012-12-09 22:29:47'),(98,1,16777343,'2012-12-09 22:30:03'),(99,1,16777343,'2012-12-09 22:36:53'),(100,1,16777343,'2012-12-09 22:38:15'),(101,1,16777343,'2012-12-09 22:39:07'),(102,1,16777343,'2012-12-09 22:39:07'),(103,1,16777343,'2012-12-09 22:39:18'),(104,1,16777343,'2012-12-09 22:43:44'),(105,1,16777343,'2012-12-09 22:49:07'),(106,1,16777343,'2012-12-09 22:49:27'),(107,1,16777343,'2012-12-09 22:49:27'),(108,1,16777343,'2012-12-09 22:49:35'),(109,1,16777343,'2012-12-09 22:52:06'),(110,1,16777343,'2012-12-09 22:59:07'),(111,1,16777343,'2012-12-10 08:31:21'),(112,1,16777343,'2012-12-10 08:51:26'),(113,1,16777343,'2012-12-10 08:58:17'),(114,1,16777343,'2012-12-10 09:00:58'),(115,1,16777343,'2012-12-10 09:01:44'),(116,1,16777343,'2012-12-11 20:22:14'),(117,1,16777343,'2012-12-11 20:24:26'),(118,1,16777343,'2012-12-11 20:30:00'),(119,1,16777343,'2012-12-11 20:42:10'),(120,1,16777343,'2012-12-11 20:43:22'),(121,1,16777343,'2012-12-11 20:44:31'),(122,1,16777343,'2012-12-11 20:46:30'),(123,1,16777343,'2012-12-11 20:48:20'),(124,1,16777343,'2012-12-11 20:49:09'),(125,1,16777343,'2012-12-11 20:52:10'),(126,1,16777343,'2012-12-11 20:58:55'),(127,1,16777343,'2012-12-11 20:59:56'),(128,1,16777343,'2012-12-11 21:00:37'),(129,1,16777343,'2012-12-11 21:01:28'),(130,1,16777343,'2012-12-11 21:02:11'),(131,1,16777343,'2012-12-11 21:06:55'),(132,1,16777343,'2012-12-11 21:07:32'),(133,1,16777343,'2012-12-11 21:08:20'),(134,1,16777343,'2012-12-11 21:09:06'),(135,1,16777343,'2012-12-11 21:09:50'),(136,1,16777343,'2012-12-11 21:12:50'),(137,1,16777343,'2012-12-11 21:13:49'),(138,1,16777343,'2012-12-11 21:14:28'),(139,1,16777343,'2012-12-11 21:17:08'),(140,1,16777343,'2012-12-11 21:39:55'),(141,1,16777343,'2012-12-11 21:41:41'),(142,1,16777343,'2012-12-11 21:58:11'),(143,1,16777343,'2012-12-11 22:04:46'),(144,1,16777343,'2012-12-11 22:05:24'),(145,1,16777343,'2012-12-11 22:08:08'),(146,1,16777343,'2012-12-11 22:17:19'),(147,1,16777343,'2012-12-11 22:23:06'),(148,1,16777343,'2012-12-11 22:24:04'),(149,1,16777343,'2012-12-11 22:24:37'),(150,1,16777343,'2012-12-11 22:25:03'),(151,1,16777343,'2012-12-12 08:22:15'),(152,1,16777343,'2012-12-12 08:22:49'),(153,1,16777343,'2012-12-12 08:24:20'),(154,1,16777343,'2012-12-12 08:30:22'),(155,1,16777343,'2012-12-12 08:42:08'),(156,1,16777343,'2012-12-12 08:45:08'),(157,1,16777343,'2012-12-12 08:48:58'),(158,1,16777343,'2012-12-12 08:49:31'),(159,1,16777343,'2012-12-12 09:05:55'),(160,1,16777343,'2012-12-12 09:07:05'),(161,1,16777343,'2012-12-12 09:14:31'),(162,1,16777343,'2012-12-12 09:16:26'),(163,1,16777343,'2012-12-12 09:28:53'),(164,1,16777343,'2012-12-12 09:31:43'),(165,1,16777343,'2012-12-12 09:35:10'),(166,1,16777343,'2012-12-12 09:39:50'),(167,1,16777343,'2012-12-12 09:44:15'),(168,1,16777343,'2012-12-12 09:52:41'),(169,1,16777343,'2012-12-12 09:56:25'),(170,1,16777343,'2012-12-12 10:54:00'),(171,1,16777343,'2012-12-12 10:57:25'),(172,1,16777343,'2012-12-12 11:17:59'),(173,1,16777343,'2012-12-12 11:20:48'),(174,1,16777343,'2012-12-12 11:28:07'),(175,1,16777343,'2012-12-12 11:46:02'),(176,1,16777343,'2012-12-12 12:21:42'),(177,1,16777343,'2012-12-12 12:27:36'),(178,1,16777343,'2012-12-12 19:02:23'),(179,1,16777343,'2012-12-12 19:16:26');
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
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'The unique ID of the character.',
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
  CONSTRAINT `character_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_3` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_4` FOREIGN KEY (`load_map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_ibfk_5` FOREIGN KEY (`respawn_map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COMMENT='Persisted (users, persistent NPCs) chars.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character`
--

LOCK TABLES `character` WRITE;
/*!40000 ALTER TABLE `character` DISABLE KEYS */;
INSERT INTO `character` VALUES (1,NULL,'Spodi',NULL,NULL,NULL,1,933,1389,1,1024,600,1,1800,203255,92,2750,452,77,100,100,100,1,1,1,1,3,2),(2,NULL,'test',NULL,NULL,NULL,1,1024,600,3,1024,600,1,1800,0,1,0,0,50,50,50,50,1,1,1,1,1,1);
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
INSERT INTO `character_equipped` VALUES (1,3,2),(1,4,0);
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
INSERT INTO `character_inventory` VALUES (1,1,0),(1,2,1),(1,6,3),(1,7,4);
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
INSERT INTO `character_skill` VALUES (1,0,'2010-11-22 20:11:38'),(1,1,'2010-11-22 00:35:25');
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
  CONSTRAINT `character_template_ibfk_2` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_ibfk_3` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Character templates (used to instantiate characters).';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_template`
--

LOCK TABLES `character_template` WRITE;
/*!40000 ALTER TABLE `character_template` DISABLE KEYS */;
INSERT INTO `character_template` VALUES (0,0,'User Template',NULL,NULL,NULL,1,1800,5,1,0,0,0,0,50,50,1,2,1,1,1,1),(1,1,'Bee',1,NULL,NULL,5,2500,10,1,0,0,5,5,5,5,1,2,1,1,1,1),(2,2,'Quest Giver',NULL,NULL,NULL,3,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(4,2,'Potion Vendor',NULL,1,NULL,3,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(5,2,'Inn Keeper',NULL,NULL,0,3,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(6,3,'Brawler',1,NULL,NULL,2,500,25,1,0,0,8,8,20,20,1,2,1,1,1,1);
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
INSERT INTO `character_template_inventory` VALUES (0,1,5,0,2,10000),(1,1,5,0,2,10000),(2,1,3,1,1,5000),(3,1,5,0,2,10000),(4,1,7,1,10,65535),(7,1,3,1,1,5000),(8,1,3,1,1,5000),(9,1,7,1,10,65535),(10,1,3,1,1,5000),(11,1,7,1,10,65535),(12,1,3,1,1,5000),(13,1,5,0,2,10000),(14,1,7,1,10,65535),(15,1,3,1,1,5000),(16,1,7,1,10,65535);
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
INSERT INTO `event_counters_item_template` VALUES (2,1,5),(2,5,16),(3,0,4986),(3,4,394),(3,5,33),(3,6,49),(3,8,12),(5,0,3832),(5,5,44),(5,6,66),(5,8,13),(6,5,24),(7,0,101217),(7,4,1),(7,5,98),(7,6,2227),(7,8,564),(8,0,193);
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
INSERT INTO `event_counters_map` VALUES (1,0,83),(1,1,3471),(1,2,2202),(1,3,91),(1,4,91),(1,5,15),(1,6,90),(1,7,390),(1,8,485),(2,1,1143),(3,0,85),(3,1,231),(3,3,4),(3,4,4),(3,5,220),(3,8,305),(4,1,285);
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
INSERT INTO `event_counters_npc` VALUES (1,2,100),(1,3,2143),(1,5,435),(1,7,393),(1,8,76),(6,4,121),(6,6,121),(6,7,121),(6,8,121);
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
INSERT INTO `event_counters_user` VALUES (1,2,58),(1,4,425),(1,6,2107),(1,7,82),(1,8,1269);
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1 COMMENT='The live, persisted items.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES (1,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',3,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(2,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',55,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL),(3,3,2,1,20,24,24,'Titanium Sword','A sword made out of titanium',1,96,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(4,5,3,0,0,11,16,'Crystal Helmet','A helmet made out of crystal',1,97,50,0,0,0,0,0,0,0,0,0,2,0,0,0,'crystal helmet',NULL),(6,2,1,0,0,9,16,'Mana Potion','A mana potion',4,95,10,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(7,6,2,2,500,16,16,'Pistol','A pistol that goes BANG BANG SUCKA!',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL,NULL),(8,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',1,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,NULL);
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
INSERT INTO `item_template` VALUES (0,2,1,10,16,16,'Unarmed','Unarmed',1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(1,1,0,0,9,16,'Healing Potion','A healing potion',95,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(2,1,0,0,9,16,'Mana Potion','A mana potion',94,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL),(3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,NULL,NULL),(4,4,0,0,30,30,'Black Armor','Body armor made out of... black.',234,2000,0,0,5,5,5,5,5,0,0,20,0,0,0,'Body.Black',NULL),(5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'crystal helmet',NULL),(6,2,2,500,16,16,'Pistol','Just point it at whatever you want to die.',177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL,NULL),(7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1),(8,4,0,0,30,30,'Iron Armor','Body armor made out of iron.',236,50,0,0,1,0,0,0,0,0,0,6,0,0,0,'Body.Iron',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1 COMMENT='NPC spawns for the maps.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `map_spawn`
--

LOCK TABLES `map_spawn` WRITE;
/*!40000 ALTER TABLE `map_spawn` DISABLE KEYS */;
INSERT INTO `map_spawn` VALUES (5,2,6,15,NULL,NULL,NULL,NULL),(6,3,1,3,NULL,NULL,NULL,NULL),(7,4,1,3,NULL,NULL,NULL,NULL),(9,1,1,50,NULL,NULL,NULL,NULL),(10,1,6,30,2000,2000,200,200),(11,1,2,1,800,300,32,32),(12,1,4,1,1000,300,32,32),(13,1,5,1,900,400,32,32);
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
INSERT INTO `server_time` VALUES ('2012-12-12 19:18:46');
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
INSERT INTO `world_stats_count_consume_item` VALUES (2,2,'2010-12-09 10:26:49');
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
INSERT INTO `world_stats_count_item_create` VALUES (2,6,'2010-12-09 10:26:47'),(3,9077,'2012-12-13 03:18:43'),(5,7014,'2012-12-13 03:18:43'),(6,1,'2010-12-16 20:55:11'),(7,129668,'2012-12-13 03:18:43'),(8,193,'2012-12-13 03:18:43');
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
INSERT INTO `world_stats_count_npc_kill_user` VALUES (1,1,48,'2012-12-13 03:17:14');
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
INSERT INTO `world_stats_count_user_consume_item` VALUES (1,2,2,'2010-12-09 10:26:49');
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
INSERT INTO `world_stats_count_user_kill_npc` VALUES (1,1,97,'2012-12-13 03:17:19');
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
) ENGINE=MyISAM AUTO_INCREMENT=216 DEFAULT CHARSET=latin1 COMMENT='Snapshots of network deltas.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_network`
--

LOCK TABLES `world_stats_network` WRITE;
/*!40000 ALTER TABLE `world_stats_network` DISABLE KEYS */;
INSERT INTO `world_stats_network` VALUES (1,'2010-11-27 04:54:55',1,0,0,0,343,12,16),(2,'2010-11-27 09:33:36',1,0,0,0,4,0,0),(3,'2010-11-29 09:42:46',1,0,0,0,217,6,9),(4,'2010-11-29 09:43:46',1,0,0,0,131,5,6),(5,'2010-11-29 09:44:46',1,0,0,0,146,6,6),(6,'2010-11-29 09:45:46',1,0,0,0,164,7,7),(7,'2010-11-29 09:46:46',1,0,0,0,138,6,6),(8,'2010-11-29 09:47:46',1,0,0,0,204,8,9),(9,'2010-11-29 09:48:46',1,0,0,0,145,6,6),(10,'2010-11-29 17:36:09',1,0,0,0,185,6,8),(11,'2010-11-29 17:38:13',1,0,0,0,1581,65,129),(12,'2010-11-29 17:38:13',1,0,0,0,1581,65,129),(13,'2010-11-29 18:02:33',1,0,0,0,44,1,2),(14,'2010-11-29 18:03:28',1,0,0,0,179,6,8),(15,'2010-11-29 18:08:38',1,0,0,0,8,0,0),(16,'2010-11-29 18:08:41',1,0,0,0,11,1,2),(17,'2010-11-29 18:09:38',1,0,0,0,132,5,6),(18,'2010-11-29 18:10:38',1,0,0,0,111,5,5),(19,'2010-11-29 18:11:38',1,0,0,0,78,3,4),(20,'2010-11-29 18:12:38',1,0,0,0,88,4,4),(21,'2010-11-29 18:13:38',1,0,0,0,134,6,6),(22,'2010-11-29 18:14:38',1,0,0,0,141,6,6),(23,'2010-11-29 18:22:58',1,0,0,0,187,7,8),(24,'2010-11-29 18:23:58',1,0,0,0,94,4,4),(25,'2010-11-29 18:24:58',1,0,0,0,141,6,6),(26,'2010-11-29 18:25:58',1,0,0,0,125,5,5),(27,'2010-11-29 18:26:58',1,0,0,0,128,5,6),(28,'2010-11-29 18:27:58',1,0,0,0,169,7,7),(29,'2010-11-29 18:28:58',1,0,0,0,133,5,6),(30,'2010-11-29 18:30:05',1,0,0,0,86,4,4),(31,'2010-11-29 18:30:58',1,0,0,0,31352,75,1306),(32,'2010-11-29 18:33:03',1,0,0,0,171,6,7),(33,'2010-11-29 18:36:41',1,0,0,0,102,4,5),(34,'2010-11-30 17:05:34',0,0,0,0,0,0,0),(35,'2010-11-30 17:06:34',0,0,0,0,0,0,0),(36,'2010-12-04 20:26:01',0,0,0,0,0,0,0),(37,'2010-12-08 06:27:16',1,0,0,0,203,6,9),(38,'2010-12-08 06:28:49',0,0,0,0,83,3,4),(39,'2010-12-16 20:54:59',1,0,0,0,212,8,10),(40,'2010-12-16 20:55:59',1,0,0,0,138,6,7),(41,'2010-12-16 20:56:59',1,0,0,0,153,7,8),(42,'2010-12-16 20:59:52',1,0,0,0,359,12,17),(43,'2010-12-26 20:16:59',1,0,0,0,261,9,12),(44,'2010-12-26 20:17:59',1,0,0,0,167,7,7),(45,'2010-12-26 20:36:37',1,0,0,0,112,3,5),(46,'2010-12-26 20:37:37',1,0,0,0,234,9,10),(47,'2010-12-26 20:42:28',1,0,0,0,210,9,10),(48,'2010-12-26 20:45:07',1,0,0,0,2445,140,225),(49,'2010-12-26 22:11:59',2,0,0,0,382,12,17),(50,'2010-12-26 22:12:59',0,0,0,0,176,7,8),(51,'2010-12-26 22:14:40',2,0,0,0,501,16,23),(52,'2010-12-26 22:15:40',2,0,0,0,297,13,13),(53,'2010-12-27 00:27:02',1,0,0,0,187,6,10),(54,'2010-12-27 00:28:02',1,0,0,0,166,7,10),(55,'2010-12-27 00:29:02',1,0,0,0,188,8,11),(56,'2010-12-27 00:30:02',1,0,0,0,175,7,10),(57,'2010-12-27 00:31:02',0,0,0,0,209,8,11),(58,'2010-12-29 09:03:21',1,0,0,0,146,5,6),(59,'2010-12-29 09:04:21',1,0,0,0,105,4,5),(60,'2010-12-29 09:05:21',1,0,0,0,110,5,5),(61,'2010-12-29 09:06:21',1,0,0,0,114,5,5),(62,'2010-12-29 09:07:21',1,0,0,0,122,5,5),(63,'2010-12-29 09:08:21',1,0,0,0,139,6,6),(64,'2010-12-29 09:09:21',1,0,0,0,107,5,5),(65,'2010-12-29 09:10:47',0,0,0,0,39,2,2),(66,'2010-12-29 09:11:21',1,0,0,0,156,5,7),(67,'2010-12-29 09:12:21',0,0,0,0,6,0,0),(68,'2010-12-29 09:13:21',0,0,0,0,0,0,0),(69,'2010-12-29 09:14:21',0,0,0,0,0,0,0),(70,'2012-12-12 04:25:24',1,0,0,0,0,0,0),(71,'2012-12-12 04:26:24',1,0,0,0,0,0,0),(72,'2012-12-12 04:27:24',1,0,0,0,0,0,0),(73,'2012-12-12 04:28:24',1,0,0,0,0,0,0),(74,'2012-12-12 04:30:59',1,0,0,0,0,0,0),(75,'2012-12-12 04:45:31',1,0,0,0,0,0,0),(76,'2012-12-12 04:47:34',1,0,0,0,0,0,0),(77,'2012-12-12 04:50:08',1,0,0,0,0,0,0),(78,'2012-12-12 04:53:08',1,0,0,0,0,0,0),(79,'2012-12-12 04:54:08',1,0,0,0,0,0,0),(80,'2012-12-12 04:55:08',1,0,0,0,0,0,0),(81,'2012-12-12 04:56:08',1,0,0,0,0,0,0),(82,'2012-12-12 04:57:08',1,0,0,0,0,0,0),(83,'2012-12-12 04:58:08',1,0,0,0,0,0,0),(84,'2012-12-12 05:03:06',0,0,0,0,0,0,0),(85,'2012-12-12 05:15:26',1,0,0,0,0,0,0),(86,'2012-12-12 05:17:56',1,0,0,0,0,0,0),(87,'2012-12-12 05:18:59',1,0,0,0,0,0,0),(88,'2012-12-12 05:19:56',1,0,0,0,0,0,0),(89,'2012-12-12 05:20:56',1,0,0,0,0,0,0),(90,'2012-12-12 05:21:56',1,0,0,0,0,0,0),(91,'2012-12-12 05:22:56',1,0,0,0,0,0,0),(92,'2012-12-12 05:23:56',1,0,0,0,0,0,0),(93,'2012-12-12 05:24:56',1,0,0,0,0,0,0),(94,'2012-12-12 05:25:56',1,0,0,0,0,0,0),(95,'2012-12-12 05:26:56',1,0,0,0,0,0,0),(96,'2012-12-12 05:27:56',1,0,0,0,0,0,0),(97,'2012-12-12 05:28:56',1,0,0,0,0,0,0),(98,'2012-12-12 05:29:56',1,0,0,0,0,0,0),(99,'2012-12-12 05:30:56',1,0,0,0,0,0,0),(100,'2012-12-12 05:31:56',1,0,0,0,0,0,0),(101,'2012-12-12 05:32:56',1,0,0,0,0,0,0),(102,'2012-12-12 05:33:56',1,0,0,0,0,0,0),(103,'2012-12-12 05:34:56',1,0,0,0,0,0,0),(104,'2012-12-12 05:35:56',1,0,0,0,0,0,0),(105,'2012-12-12 05:36:59',1,0,0,0,0,0,0),(106,'2012-12-12 05:38:01',1,0,0,0,0,0,0),(107,'2012-12-12 05:39:00',1,0,0,0,0,0,0),(108,'2012-12-12 05:42:41',1,0,0,0,0,0,0),(109,'2012-12-12 05:43:47',1,0,0,0,0,0,0),(110,'2012-12-12 05:44:41',1,0,0,0,0,0,0),(111,'2012-12-12 05:45:41',1,0,0,0,0,0,0),(112,'2012-12-12 05:46:41',1,0,0,0,0,0,0),(113,'2012-12-12 05:47:41',1,0,0,0,0,0,0),(114,'2012-12-12 05:48:41',1,0,0,0,0,0,0),(115,'2012-12-12 05:49:41',1,0,0,0,0,0,0),(116,'2012-12-12 05:50:46',1,0,0,0,0,0,0),(117,'2012-12-12 05:51:41',1,0,0,0,0,0,0),(118,'2012-12-12 05:52:41',1,0,0,0,0,0,0),(119,'2012-12-12 05:53:41',1,0,0,0,0,0,0),(120,'2012-12-12 05:54:41',1,0,0,0,0,0,0),(121,'2012-12-12 05:55:53',1,0,0,0,0,0,0),(122,'2012-12-12 05:56:49',1,0,0,0,0,0,0),(123,'2012-12-12 05:57:41',1,0,0,0,0,0,0),(124,'2012-12-12 05:59:08',1,0,0,0,0,0,0),(125,'2012-12-12 06:10:24',0,0,0,0,0,0,0),(126,'2012-12-12 06:11:24',0,0,0,0,0,0,0),(127,'2012-12-12 06:12:24',0,0,0,0,0,0,0),(128,'2012-12-12 06:13:24',0,0,0,0,0,0,0),(129,'2012-12-12 06:14:24',0,0,0,0,0,0,0),(130,'2012-12-12 06:15:24',0,0,0,0,0,0,0),(131,'2012-12-12 06:16:24',0,0,0,0,0,0,0),(132,'2012-12-12 16:25:20',1,0,0,0,0,0,0),(133,'2012-12-12 16:26:19',1,0,0,0,0,0,0),(134,'2012-12-12 16:27:19',1,0,0,0,0,0,0),(135,'2012-12-12 16:28:19',1,0,0,0,0,0,0),(136,'2012-12-12 16:29:19',1,0,0,0,0,0,0),(137,'2012-12-12 16:46:04',1,0,0,0,0,0,0),(138,'2012-12-12 16:47:04',1,0,0,0,0,0,0),(139,'2012-12-12 16:50:28',1,0,0,0,0,0,0),(140,'2012-12-12 16:51:28',1,0,0,0,0,0,0),(141,'2012-12-12 16:52:36',1,0,0,0,0,0,0),(142,'2012-12-12 16:53:28',1,0,0,0,0,0,0),(143,'2012-12-12 16:54:28',1,0,0,0,0,0,0),(144,'2012-12-12 16:55:28',1,0,0,0,0,0,0),(145,'2012-12-12 16:56:28',1,0,0,0,0,0,0),(146,'2012-12-12 16:57:46',1,0,0,0,0,0,0),(147,'2012-12-12 16:58:28',1,0,0,0,0,0,0),(148,'2012-12-12 16:59:28',1,0,0,0,0,0,0),(149,'2012-12-12 17:00:28',1,0,0,0,0,0,0),(150,'2012-12-12 17:01:34',1,0,0,0,0,0,0),(151,'2012-12-12 17:02:31',1,0,0,0,0,0,0),(152,'2012-12-12 17:03:29',1,0,0,0,0,0,0),(153,'2012-12-12 17:04:28',1,0,0,0,0,0,0),(154,'2012-12-12 17:17:24',1,0,0,0,0,0,0),(155,'2012-12-12 17:18:47',1,0,0,0,0,0,0),(156,'2012-12-12 17:19:24',1,0,0,0,0,0,0),(157,'2012-12-12 17:29:52',1,0,0,0,0,0,0),(158,'2012-12-12 17:30:52',1,0,0,0,0,0,0),(159,'2012-12-12 17:32:52',1,0,0,0,0,0,0),(160,'2012-12-12 17:33:41',1,0,0,0,0,0,0),(161,'2012-12-12 17:36:08',1,0,0,0,0,0,0),(162,'2012-12-12 17:37:08',1,0,0,0,0,0,0),(163,'2012-12-12 17:38:08',1,0,0,0,0,0,0),(164,'2012-12-12 17:39:08',1,0,0,0,0,0,0),(165,'2012-12-12 17:40:48',1,0,0,0,0,0,0),(166,'2012-12-12 17:41:48',1,0,0,0,0,0,0),(167,'2012-12-12 17:42:48',1,0,0,0,0,0,0),(168,'2012-12-12 17:43:48',1,0,0,0,0,0,0),(169,'2012-12-12 17:45:14',1,0,0,0,0,0,0),(170,'2012-12-12 17:46:19',1,0,0,0,0,0,0),(171,'2012-12-12 17:47:14',1,0,0,0,0,0,0),(172,'2012-12-12 17:48:18',1,0,0,0,0,0,0),(173,'2012-12-12 17:49:14',1,0,0,0,0,0,0),(174,'2012-12-12 17:50:14',1,0,0,0,0,0,0),(175,'2012-12-12 17:51:14',1,0,0,0,0,0,0),(176,'2012-12-12 17:52:14',1,0,0,0,0,0,0),(177,'2012-12-12 17:53:41',1,0,0,0,0,0,0),(178,'2012-12-12 18:54:59',1,0,0,0,0,0,0),(179,'2012-12-12 18:56:10',1,0,0,0,0,0,0),(180,'2012-12-12 18:58:24',1,0,0,0,0,0,0),(181,'2012-12-12 18:59:24',1,0,0,0,0,0,0),(182,'2012-12-12 19:00:24',1,0,0,0,0,0,0),(183,'2012-12-12 19:01:30',1,0,0,0,0,0,0),(184,'2012-12-12 19:02:24',1,0,0,0,0,0,0),(185,'2012-12-12 19:03:24',1,0,0,0,0,0,0),(186,'2012-12-12 19:04:32',1,0,0,0,0,0,0),(187,'2012-12-12 19:05:24',1,0,0,0,0,0,0),(188,'2012-12-12 19:06:24',1,0,0,0,0,0,0),(189,'2012-12-12 19:07:24',1,0,0,0,0,0,0),(190,'2012-12-12 19:08:24',1,0,0,0,0,0,0),(191,'2012-12-12 19:09:24',1,0,0,0,0,0,0),(192,'2012-12-12 19:10:24',1,0,0,0,0,0,0),(193,'2012-12-12 19:18:56',1,0,0,0,0,0,0),(194,'2012-12-12 19:21:47',1,0,0,0,0,0,0),(195,'2012-12-12 19:22:47',1,0,0,0,0,0,0),(196,'2012-12-12 19:23:47',1,0,0,0,0,0,0),(197,'2012-12-12 19:24:47',1,0,0,0,0,0,0),(198,'2012-12-12 19:29:05',1,0,0,0,0,0,0),(199,'2012-12-12 19:30:05',1,0,0,0,0,0,0),(200,'2012-12-12 19:31:12',1,0,0,0,0,0,0),(201,'2012-12-12 19:32:05',1,0,0,0,0,0,0),(202,'2012-12-12 19:33:05',1,0,0,0,0,0,0),(203,'2012-12-12 19:34:05',1,0,0,0,0,0,0),(204,'2012-12-12 19:35:05',1,0,0,0,0,0,0),(205,'2012-12-12 19:36:05',1,0,0,0,0,0,0),(206,'2012-12-12 19:37:05',1,0,0,0,0,0,0),(207,'2012-12-12 19:38:05',1,0,0,0,0,0,0),(208,'2012-12-12 19:39:13',1,0,0,0,0,0,0),(209,'2012-12-12 19:40:05',1,0,0,0,0,0,0),(210,'2012-12-12 19:41:05',1,0,0,0,0,0,0),(211,'2012-12-12 19:42:05',1,0,0,0,0,0,0),(212,'2012-12-12 19:43:05',1,0,0,0,0,0,0),(213,'2012-12-12 19:44:51',1,0,0,0,0,0,0),(214,'2012-12-12 19:45:05',1,0,0,0,0,0,0),(215,'2012-12-12 19:47:00',0,0,0,0,0,0,0);
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
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=latin1 COMMENT='Event log: NPC kill user.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_npc_kill_user`
--

LOCK TABLES `world_stats_npc_kill_user` WRITE;
/*!40000 ALTER TABLE `world_stats_npc_kill_user` DISABLE KEYS */;
INSERT INTO `world_stats_npc_kill_user` VALUES (1,1,1,76,1024,593,39,754,3,'2010-11-27 04:54:42'),(2,1,1,76,1024,593,673,754,3,'2010-12-16 20:54:23'),(3,1,1,77,1024,593,790,479,3,'2010-12-26 20:23:29'),(4,1,1,77,1024,593,857,498,3,'2010-12-26 22:11:42'),(5,1,1,78,1024,593,867,658,1,'2010-12-27 00:26:29'),(6,1,1,78,1024,593,1016,594,3,'2010-12-27 00:26:42'),(7,1,1,78,1024,593,1016,594,3,'2010-12-27 00:26:52'),(8,1,1,78,1024,593,1017,594,3,'2010-12-27 00:27:01'),(9,1,1,78,1024,593,1015,594,3,'2010-12-27 00:27:09'),(10,1,1,78,1024,593,1016,594,3,'2010-12-27 00:27:18'),(11,1,1,78,1024,593,1015,594,3,'2010-12-27 00:27:25'),(12,1,1,78,1024,593,1015,546,3,'2010-12-27 00:27:36'),(13,1,1,78,1024,593,1017,594,3,'2010-12-27 00:27:45'),(14,1,1,78,1024,593,1016,594,3,'2010-12-27 00:27:53'),(15,1,1,78,1024,593,1016,594,3,'2010-12-27 00:28:04'),(16,1,1,78,1024,593,1016,594,3,'2010-12-27 00:28:14'),(17,1,1,78,1024,593,1016,594,3,'2010-12-27 00:28:25'),(18,1,1,78,1024,593,1017,594,3,'2010-12-27 00:28:36'),(19,1,1,78,1024,593,1015,594,3,'2010-12-27 00:28:44'),(20,1,1,78,1024,593,1015,594,3,'2010-12-27 00:28:55'),(21,1,1,78,1024,593,1015,594,3,'2010-12-27 00:29:10'),(22,1,1,78,1024,593,1017,594,3,'2010-12-27 00:29:19'),(23,1,1,78,1024,593,1017,594,3,'2010-12-27 00:29:32'),(24,1,1,78,1024,593,1017,594,3,'2010-12-27 00:29:42'),(25,1,1,78,1024,593,1016,594,3,'2010-12-27 00:29:52'),(26,1,1,78,1024,593,1016,594,3,'2010-12-27 00:29:59'),(27,1,1,78,1024,593,1015,594,3,'2010-12-27 00:30:07'),(28,1,1,78,1024,593,1017,594,3,'2010-12-27 00:30:16'),(29,1,1,78,1024,593,1015,546,3,'2010-12-27 00:30:28'),(30,1,1,78,1024,593,1017,594,3,'2010-12-27 00:30:37'),(31,1,1,78,1024,593,1017,594,3,'2010-12-27 00:30:53'),(32,1,1,80,32,32,162,970,1,'2012-12-10 06:14:01'),(33,1,1,80,32,32,500,778,1,'2012-12-10 06:14:56'),(34,1,1,81,32,32,205,698,1,'2012-12-10 06:15:02'),(35,1,1,81,32,32,470,778,1,'2012-12-10 06:15:07'),(36,1,1,83,32,32,15,234,1,'2012-12-10 06:39:27'),(37,1,1,83,32,32,493,768,1,'2012-12-10 06:59:14'),(38,1,1,83,32,32,605,765,1,'2012-12-10 16:31:28'),(39,1,1,84,32,32,378,1026,1,'2012-12-10 16:58:27'),(40,1,1,86,32,32,828,973,1,'2012-12-12 04:24:31'),(41,1,1,87,32,32,1676,594,1,'2012-12-12 04:24:57'),(42,1,1,88,32,32,756,1201,1,'2012-12-12 04:25:14'),(43,1,1,88,32,32,828,1226,1,'2012-12-12 04:25:46'),(44,1,1,89,32,32,1183,1014,1,'2012-12-12 19:46:09'),(45,1,1,89,32,32,1687,373,1,'2012-12-12 19:46:20'),(46,1,1,89,32,32,1616,1201,1,'2012-12-12 19:46:40'),(47,1,1,89,32,32,592,1014,1,'2012-12-12 20:27:42'),(48,1,1,92,32,32,1490,2026,1,'2012-12-13 03:17:14');
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COMMENT='Event log: User consumes use-once item.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_user_consume_item`
--

LOCK TABLES `world_stats_user_consume_item` WRITE;
/*!40000 ALTER TABLE `world_stats_user_consume_item` DISABLE KEYS */;
INSERT INTO `world_stats_user_consume_item` VALUES (1,1,2,3,1024,594,'2010-12-09 10:24:00'),(2,1,2,3,1024,594,'2010-12-09 10:26:49');
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
) ENGINE=InnoDB AUTO_INCREMENT=98 DEFAULT CHARSET=latin1 COMMENT='Event log: User kills NPC.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_user_kill_npc`
--

LOCK TABLES `world_stats_user_kill_npc` WRITE;
/*!40000 ALTER TABLE `world_stats_user_kill_npc` DISABLE KEYS */;
INSERT INTO `world_stats_user_kill_npc` VALUES (1,1,1,76,1024,594,0,0,3,'2010-11-27 07:20:23'),(2,1,1,76,797,498,0,0,3,'2010-12-16 20:55:20'),(3,1,1,76,829,498,0,0,3,'2010-12-16 20:59:02'),(4,1,1,77,525,754,0,0,3,'2010-12-16 20:59:41'),(5,1,1,77,60,550,0,0,1,'2010-12-26 22:13:59'),(6,1,1,77,1024,658,0,0,1,'2010-12-26 23:06:22'),(7,1,1,77,1024,658,0,0,1,'2010-12-26 23:06:23'),(8,1,1,77,1024,658,0,0,1,'2010-12-26 23:08:28'),(9,1,1,77,1024,658,0,0,1,'2010-12-26 23:08:28'),(10,1,1,78,1024,594,0,0,3,'2010-12-27 00:48:54'),(11,1,1,78,1024,594,0,0,3,'2010-12-27 00:48:54'),(12,1,1,78,1024,594,0,0,3,'2010-12-27 00:48:56'),(13,1,1,78,700,391,0,0,3,'2010-12-27 00:49:08'),(14,1,1,78,584,434,0,0,3,'2010-12-27 00:49:09'),(15,1,1,78,1024,594,0,0,3,'2010-12-27 00:56:45'),(16,1,1,79,800,344,0,0,3,'2012-12-05 07:51:23'),(17,1,1,79,800,344,0,0,3,'2012-12-05 07:51:23'),(18,1,1,79,800,344,0,0,3,'2012-12-05 07:51:24'),(19,1,1,79,365,222,0,0,3,'2012-12-05 07:51:39'),(20,1,1,79,365,222,0,0,1,'2012-12-05 07:52:13'),(21,1,1,79,365,222,0,0,1,'2012-12-05 07:52:14'),(22,1,1,80,269,670,0,0,1,'2012-12-10 06:14:55'),(23,1,1,80,384,778,0,0,1,'2012-12-10 06:14:55'),(24,1,1,80,492,778,0,0,1,'2012-12-10 06:14:56'),(25,1,1,80,190,748,0,0,1,'2012-12-10 06:14:58'),(26,1,1,80,193,778,0,0,1,'2012-12-10 06:14:59'),(27,1,1,80,250,667,0,0,1,'2012-12-10 06:14:59'),(28,1,1,81,362,715,0,0,1,'2012-12-10 06:15:00'),(29,1,1,81,430,667,0,0,1,'2012-12-10 06:15:00'),(30,1,1,81,319,715,0,0,1,'2012-12-10 06:15:01'),(31,1,1,81,203,649,0,0,1,'2012-12-10 06:15:02'),(32,1,1,81,294,748,0,0,1,'2012-12-10 06:15:03'),(33,1,1,81,32,234,0,0,1,'2012-12-10 06:21:30'),(34,1,1,82,32,234,0,0,1,'2012-12-10 06:25:15'),(35,1,1,82,32,234,0,0,1,'2012-12-10 06:28:42'),(36,1,1,82,10,234,0,0,1,'2012-12-10 06:30:06'),(37,1,1,82,10,234,0,0,1,'2012-12-10 06:30:06'),(38,1,1,82,10,234,0,0,1,'2012-12-10 06:36:57'),(39,1,1,82,10,234,0,0,1,'2012-12-10 06:36:58'),(40,1,1,83,262,640,0,0,1,'2012-12-10 06:59:12'),(41,1,1,83,378,778,0,0,1,'2012-12-10 06:59:12'),(42,1,1,83,493,778,0,0,1,'2012-12-10 06:59:13'),(43,1,1,83,540,778,0,0,1,'2012-12-10 06:59:14'),(44,1,1,83,593,1026,0,0,1,'2012-12-10 16:51:46'),(45,1,1,83,593,1026,0,0,1,'2012-12-10 16:51:47'),(46,1,1,84,607,1022,0,0,1,'2012-12-10 16:58:21'),(47,1,1,84,391,995,0,0,1,'2012-12-10 16:58:22'),(48,1,1,84,373,1026,0,0,1,'2012-12-10 16:58:22'),(49,1,1,84,438,896,0,0,1,'2012-12-10 16:58:23'),(50,1,1,84,369,1026,0,0,1,'2012-12-10 16:58:25'),(51,1,1,84,646,1026,0,0,1,'2012-12-10 17:01:51'),(52,1,1,85,669,919,0,0,1,'2012-12-12 04:22:17'),(53,1,1,85,669,919,0,0,1,'2012-12-12 04:22:18'),(54,1,1,85,669,850,0,0,1,'2012-12-12 04:22:18'),(55,1,1,85,798,983,0,0,1,'2012-12-12 04:24:29'),(56,1,1,85,841,983,0,0,1,'2012-12-12 04:24:30'),(57,1,1,85,841,983,0,0,1,'2012-12-12 04:24:30'),(58,1,1,86,841,983,0,0,1,'2012-12-12 04:24:31'),(59,1,1,86,1653,570,0,0,1,'2012-12-12 04:24:52'),(60,1,1,86,1653,570,0,0,1,'2012-12-12 04:24:53'),(61,1,1,86,1653,570,0,0,1,'2012-12-12 04:24:53'),(62,1,1,86,1653,570,0,0,1,'2012-12-12 04:24:54'),(63,1,1,86,1653,570,0,0,1,'2012-12-12 04:24:55'),(64,1,1,87,1653,570,0,0,1,'2012-12-12 04:24:55'),(65,1,1,87,1653,570,0,0,1,'2012-12-12 04:24:56'),(66,1,1,87,1653,570,0,0,1,'2012-12-12 04:24:56'),(67,1,1,87,710,1025,0,0,1,'2012-12-12 04:25:04'),(68,1,1,87,710,1025,0,0,1,'2012-12-12 04:25:05'),(69,1,1,87,736,1176,0,0,1,'2012-12-12 04:25:14'),(70,1,1,88,643,991,0,0,1,'2012-12-12 04:25:17'),(71,1,1,88,643,991,0,0,1,'2012-12-12 04:25:18'),(72,1,1,88,643,991,0,0,1,'2012-12-12 04:25:19'),(73,1,1,88,643,991,0,0,1,'2012-12-12 04:25:19'),(74,1,1,88,643,991,0,0,1,'2012-12-12 04:25:29'),(75,1,1,88,611,1175,0,0,1,'2012-12-12 16:22:31'),(76,1,1,89,401,918,0,0,1,'2012-12-13 03:02:27'),(77,1,1,89,495,838,0,0,1,'2012-12-13 03:16:29'),(78,1,1,89,566,838,0,0,1,'2012-12-13 03:16:30'),(79,1,1,89,710,726,0,0,1,'2012-12-13 03:16:31'),(80,1,1,89,757,650,0,0,1,'2012-12-13 03:16:32'),(81,1,1,89,858,535,0,0,1,'2012-12-13 03:16:34'),(82,1,1,90,908,427,0,0,1,'2012-12-13 03:16:46'),(83,1,1,90,908,427,0,0,1,'2012-12-13 03:16:47'),(84,1,1,90,2144,2036,0,0,1,'2012-12-13 03:17:08'),(85,1,1,90,2130,2036,0,0,1,'2012-12-13 03:17:09'),(86,1,1,90,2130,2036,0,0,1,'2012-12-13 03:17:09'),(87,1,1,90,2130,2036,0,0,1,'2012-12-13 03:17:10'),(88,1,1,91,2112,2036,0,0,1,'2012-12-13 03:17:11'),(89,1,1,91,2022,2036,0,0,1,'2012-12-13 03:17:11'),(90,1,1,91,1907,2036,0,0,1,'2012-12-13 03:17:12'),(91,1,1,91,1795,2036,0,0,1,'2012-12-13 03:17:12'),(92,1,1,91,1680,2036,0,0,1,'2012-12-13 03:17:13'),(93,1,1,91,1572,2036,0,0,1,'2012-12-13 03:17:14'),(94,1,1,92,933,1389,0,0,1,'2012-12-13 03:17:17'),(95,1,1,92,933,1389,0,0,1,'2012-12-13 03:17:18'),(96,1,1,92,933,1389,0,0,1,'2012-12-13 03:17:19'),(97,1,1,92,933,1389,0,0,1,'2012-12-13 03:17:19');
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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1 COMMENT='Event log: User levels up.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_user_level`
--

LOCK TABLES `world_stats_user_level` WRITE;
/*!40000 ALTER TABLE `world_stats_user_level` DISABLE KEYS */;
INSERT INTO `world_stats_user_level` VALUES (1,1,3,829,498,77,'2010-12-16 20:59:02'),(2,1,1,1024,658,78,'2010-12-26 23:08:28'),(3,1,3,1024,594,79,'2010-12-27 00:56:45'),(4,1,1,365,222,80,'2012-12-05 07:52:14'),(5,1,1,250,667,81,'2012-12-10 06:14:59'),(6,1,1,32,234,82,'2012-12-10 06:21:30'),(7,1,1,10,234,83,'2012-12-10 06:36:58'),(8,1,1,593,1026,84,'2012-12-10 16:51:47'),(9,1,1,646,1026,85,'2012-12-10 17:01:51'),(10,1,1,841,983,86,'2012-12-12 04:24:30'),(11,1,1,1653,570,87,'2012-12-12 04:24:55'),(12,1,1,736,1176,88,'2012-12-12 04:25:14'),(13,1,1,611,1175,89,'2012-12-12 16:22:31'),(14,1,1,858,535,90,'2012-12-13 03:16:34'),(15,1,1,2130,2036,91,'2012-12-13 03:17:10'),(16,1,1,1572,2036,92,'2012-12-13 03:17:14');
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

-- Dump completed on 2012-12-12 19:21:26
