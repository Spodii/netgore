-- MySQL dump 10.13  Distrib 8.0.13, for Win64 (x86_64)
--
-- Host: localhost    Database: demogame
-- ------------------------------------------------------
-- Server version	8.0.13

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET GLOBAL log_bin_trust_function_creators = 1;

--
-- Table structure for table `account`
--

DROP TABLE IF EXISTS `account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
  `friends` varchar(800) NOT NULL DEFAULT '' COMMENT 'A list of friends that the user has.',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 COMMENT='The user accounts. Multiple chars can exist per account.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account`
--

LOCK TABLES `account` WRITE;
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
INSERT INTO `account` VALUES (1,'Spodi','3fc0a7acf087f549ac2b266baf94b8b1','spodi@netgore.com',255,'2009-09-07 15:43:16','2018-12-18 15:30:41',16777343,NULL,'');
/*!40000 ALTER TABLE `account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_ban`
--

DROP TABLE IF EXISTS `account_ban`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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

--
-- Table structure for table `account_character`
--

DROP TABLE IF EXISTS `account_character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `account_character` VALUES (1,1,NULL),(6,3,NULL),(7,1,NULL),(8,1,NULL),(9,1,NULL),(10,1,NULL);
/*!40000 ALTER TABLE `account_character` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_ips`
--

DROP TABLE IF EXISTS `account_ips`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `account_ips` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'The unique row ID.',
  `account_id` int(11) NOT NULL COMMENT 'The ID of the account.',
  `ip` int(10) unsigned NOT NULL COMMENT 'The IP that logged into the account.',
  `time` datetime NOT NULL COMMENT 'When this IP last logged into this account.',
  PRIMARY KEY (`id`),
  KEY `account_id` (`account_id`),
  CONSTRAINT `account_ips_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=277 DEFAULT CHARSET=latin1 COMMENT='The IPs used to access accounts.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_ips`
--

LOCK TABLES `account_ips` WRITE;
/*!40000 ALTER TABLE `account_ips` DISABLE KEYS */;
INSERT INTO `account_ips` VALUES (2,1,16777343,'2013-02-04 17:51:11'),(3,1,16777343,'2013-02-04 18:40:11'),(4,1,16777343,'2013-02-04 18:44:04'),(5,1,16777343,'2013-02-04 18:53:39'),(6,3,16777343,'2013-02-04 18:54:54'),(7,3,16777343,'2013-02-04 18:56:44'),(8,1,16777343,'2013-02-04 18:58:18'),(9,1,16777343,'2013-02-04 19:00:04'),(10,1,16777343,'2013-02-04 19:02:34'),(11,1,16777343,'2013-02-04 19:04:45'),(12,1,16777343,'2013-02-04 19:09:20'),(13,3,16777343,'2013-02-04 19:09:24'),(14,3,16777343,'2013-02-04 19:09:56'),(15,3,16777343,'2013-02-04 19:10:06'),(16,1,16777343,'2013-02-04 19:10:27'),(17,1,16777343,'2013-02-04 19:27:25'),(18,1,16777343,'2013-02-04 19:30:15'),(19,1,16777343,'2013-02-04 20:07:22'),(20,1,16777343,'2013-02-04 20:09:33'),(21,1,16777343,'2013-02-04 20:17:30'),(22,1,16777343,'2013-02-04 20:20:24'),(23,1,16777343,'2013-02-04 20:21:32'),(24,1,16777343,'2013-02-04 20:22:53'),(25,1,16777343,'2013-02-04 20:25:07'),(26,1,16777343,'2013-02-04 20:27:19'),(27,1,16777343,'2013-02-04 20:28:56'),(28,1,16777343,'2013-02-04 20:31:00'),(29,1,16777343,'2013-02-04 20:34:08'),(30,1,16777343,'2013-02-04 20:35:09'),(31,1,16777343,'2013-02-04 20:46:02'),(32,1,16777343,'2013-02-04 20:47:15'),(33,1,16777343,'2013-02-04 20:52:52'),(34,1,16777343,'2013-02-04 20:53:14'),(35,1,16777343,'2013-02-04 20:54:35'),(36,1,16777343,'2013-02-04 21:13:11'),(37,1,16777343,'2013-02-04 21:16:10'),(38,1,16777343,'2013-02-04 21:22:13'),(39,1,16777343,'2013-02-04 21:29:09'),(40,1,16777343,'2013-02-04 22:12:28'),(41,3,16777343,'2013-02-04 22:12:55'),(42,1,16777343,'2013-02-04 22:14:55'),(43,3,16777343,'2013-02-04 22:15:17'),(44,1,16777343,'2013-02-04 22:27:07'),(45,1,16777343,'2013-02-04 22:28:05'),(46,1,16777343,'2013-02-04 22:28:46'),(47,1,16777343,'2013-02-04 22:29:48'),(48,1,16777343,'2013-02-04 22:30:56'),(49,1,16777343,'2013-02-04 22:32:26'),(50,1,16777343,'2013-02-04 22:33:12'),(51,1,16777343,'2013-02-04 22:33:36'),(52,1,16777343,'2013-02-04 22:34:08'),(53,1,16777343,'2013-02-04 22:36:47'),(54,1,16777343,'2013-02-04 22:37:49'),(55,1,16777343,'2013-02-04 22:39:10'),(56,1,16777343,'2013-02-04 22:39:41'),(57,1,16777343,'2013-02-04 22:42:48'),(58,1,16777343,'2013-02-04 22:45:24'),(59,1,16777343,'2013-02-04 22:49:32'),(60,1,16777343,'2013-02-04 22:50:12'),(61,1,16777343,'2013-02-04 23:11:00'),(62,1,16777343,'2013-02-04 23:23:43'),(63,1,16777343,'2013-02-04 23:31:07'),(64,1,16777343,'2013-02-04 23:46:08'),(65,1,16777343,'2013-02-05 01:17:09'),(66,1,16777343,'2013-02-05 01:59:14'),(67,3,16777343,'2013-02-05 01:59:35'),(68,1,16777343,'2013-02-05 23:08:42'),(69,1,16777343,'2013-02-05 23:11:21'),(70,1,16777343,'2013-02-05 23:21:36'),(71,1,16777343,'2013-02-05 23:25:56'),(72,1,16777343,'2013-02-05 23:28:16'),(73,1,16777343,'2013-02-05 23:28:58'),(74,1,16777343,'2013-02-05 23:33:46'),(75,1,16777343,'2013-02-05 23:40:58'),(76,1,16777343,'2013-02-05 23:43:08'),(77,1,16777343,'2013-02-06 00:08:12'),(78,1,16777343,'2013-02-06 00:09:31'),(79,1,16777343,'2013-02-06 00:09:43'),(80,1,16777343,'2013-02-06 00:10:08'),(81,1,16777343,'2013-02-06 00:22:59'),(82,3,16777343,'2013-02-06 00:28:06'),(83,1,16777343,'2013-02-06 00:28:11'),(84,1,16777343,'2013-02-06 00:30:29'),(85,3,16777343,'2013-02-06 00:30:34'),(86,1,16777343,'2013-02-06 00:31:53'),(87,3,16777343,'2013-02-06 00:32:23'),(88,3,16777343,'2013-02-06 00:32:44'),(89,1,16777343,'2013-02-06 00:47:10'),(90,1,16777343,'2013-02-06 01:13:24'),(91,1,16777343,'2013-02-06 01:23:57'),(92,1,16777343,'2013-02-06 01:25:24'),(93,1,16777343,'2013-02-06 16:07:17'),(94,1,16777343,'2013-02-07 00:07:30'),(95,1,16777343,'2013-02-07 00:11:57'),(96,1,16777343,'2013-02-07 00:14:38'),(97,1,16777343,'2013-02-07 00:27:49'),(98,1,16777343,'2013-02-07 00:29:25'),(99,1,16777343,'2013-02-07 00:35:36'),(100,1,16777343,'2013-02-08 00:04:47'),(101,1,16777343,'2013-02-08 00:07:57'),(102,1,16777343,'2013-02-08 00:12:46'),(103,1,16777343,'2013-02-08 00:18:05'),(104,1,16777343,'2013-02-08 00:20:47'),(105,1,16777343,'2013-02-08 00:25:03'),(106,1,16777343,'2013-02-08 00:26:43'),(107,1,16777343,'2013-02-08 00:27:56'),(108,1,16777343,'2013-02-08 00:29:18'),(109,1,16777343,'2013-02-08 00:30:23'),(110,1,16777343,'2013-02-08 00:32:10'),(111,1,16777343,'2013-02-08 00:56:16'),(112,1,16777343,'2013-02-08 01:01:34'),(113,1,16777343,'2013-02-08 10:28:33'),(114,1,16777343,'2013-02-08 10:33:49'),(115,1,16777343,'2013-02-08 10:34:40'),(116,1,16777343,'2013-02-08 10:35:34'),(117,1,16777343,'2013-02-09 14:58:55'),(118,1,16777343,'2013-02-09 15:00:20'),(119,1,16777343,'2013-02-09 15:06:40'),(120,1,16777343,'2013-02-09 15:09:23'),(121,1,16777343,'2013-02-09 15:17:52'),(122,1,16777343,'2013-02-09 15:20:42'),(123,1,16777343,'2013-02-09 15:28:45'),(124,1,16777343,'2013-02-09 15:36:00'),(125,1,16777343,'2013-02-09 15:55:07'),(126,1,16777343,'2013-02-09 16:03:40'),(127,1,16777343,'2013-02-09 16:04:12'),(128,1,16777343,'2013-02-09 16:06:02'),(129,1,16777343,'2013-02-09 16:10:26'),(130,1,16777343,'2013-02-09 16:25:40'),(131,1,16777343,'2013-02-09 16:36:27'),(132,1,16777343,'2013-02-09 16:43:07'),(133,1,16777343,'2013-02-09 19:19:59'),(134,1,16777343,'2013-02-09 19:22:19'),(135,1,16777343,'2013-02-09 19:24:03'),(136,1,16777343,'2013-02-09 19:26:18'),(137,1,16777343,'2013-02-09 19:27:46'),(138,1,16777343,'2013-02-09 19:30:11'),(139,1,16777343,'2013-02-09 19:54:27'),(140,1,16777343,'2013-02-09 19:54:30'),(141,1,16777343,'2013-02-09 20:04:21'),(142,1,16777343,'2013-02-09 20:05:17'),(143,1,16777343,'2013-02-09 20:06:18'),(144,1,16777343,'2013-02-09 20:08:55'),(145,1,16777343,'2013-02-09 20:20:20'),(146,1,16777343,'2013-02-09 20:24:02'),(147,1,16777343,'2013-02-09 20:32:08'),(148,1,16777343,'2013-02-09 20:32:40'),(149,1,16777343,'2013-02-09 20:51:27'),(150,1,16777343,'2013-02-09 20:54:11'),(151,1,16777343,'2013-02-09 20:58:09'),(152,1,16777343,'2013-02-09 21:14:16'),(153,1,16777343,'2013-02-09 21:14:57'),(154,1,16777343,'2013-02-09 21:15:28'),(155,1,16777343,'2013-02-09 21:15:59'),(156,1,16777343,'2013-02-09 21:17:29'),(157,3,16777343,'2013-02-09 21:17:53'),(158,3,16777343,'2013-02-09 21:21:26'),(159,1,16777343,'2013-02-09 21:21:51'),(160,1,16777343,'2013-02-09 21:25:15'),(161,1,16777343,'2013-02-09 21:26:47'),(162,1,16777343,'2013-02-09 21:30:32'),(163,1,16777343,'2013-02-09 21:30:42'),(164,1,16777343,'2013-02-09 21:32:15'),(165,1,16777343,'2013-02-09 21:40:01'),(166,1,16777343,'2013-02-09 21:42:56'),(167,1,16777343,'2013-02-09 21:52:06'),(168,1,16777343,'2013-02-09 21:52:57'),(169,1,16777343,'2013-02-09 21:53:43'),(170,1,16777343,'2013-02-09 21:57:10'),(171,1,16777343,'2013-02-09 21:58:32'),(172,1,16777343,'2013-02-09 22:03:36'),(173,1,16777343,'2013-02-09 22:11:37'),(174,1,16777343,'2013-02-09 22:13:05'),(175,1,16777343,'2013-02-09 22:15:42'),(176,1,16777343,'2013-02-09 22:17:27'),(177,1,16777343,'2013-02-09 22:21:57'),(178,1,16777343,'2013-02-09 22:24:48'),(179,1,16777343,'2013-02-09 22:28:03'),(180,1,16777343,'2013-02-09 22:32:00'),(181,1,16777343,'2013-02-09 22:32:31'),(182,1,16777343,'2013-02-09 22:33:53'),(183,1,16777343,'2013-02-09 22:34:24'),(184,1,16777343,'2013-02-09 22:34:41'),(185,1,16777343,'2013-02-09 22:35:14'),(186,1,16777343,'2013-02-09 22:37:17'),(187,1,16777343,'2013-02-09 22:37:56'),(188,1,16777343,'2013-02-09 22:38:50'),(189,1,16777343,'2013-02-09 22:40:04'),(190,1,16777343,'2013-02-09 22:41:47'),(191,1,16777343,'2013-02-09 22:43:11'),(192,1,16777343,'2013-02-09 22:44:13'),(193,1,16777343,'2013-02-09 22:45:22'),(194,1,16777343,'2013-02-09 22:46:39'),(195,1,16777343,'2013-02-09 22:48:33'),(196,1,16777343,'2013-02-09 22:49:37'),(197,1,16777343,'2013-02-09 22:58:34'),(198,1,16777343,'2013-02-09 23:04:27'),(199,1,16777343,'2013-02-09 23:06:38'),(200,1,16777343,'2013-02-09 23:11:19'),(201,1,16777343,'2013-02-09 23:12:22'),(202,1,16777343,'2013-02-09 23:14:17'),(203,1,16777343,'2013-02-09 23:17:54'),(204,1,16777343,'2013-02-09 23:19:28'),(205,1,16777343,'2013-02-09 23:21:24'),(206,1,16777343,'2013-02-09 23:24:01'),(207,1,16777343,'2013-02-09 23:26:24'),(208,1,16777343,'2013-02-09 23:29:59'),(209,1,16777343,'2013-02-10 00:02:48'),(210,1,16777343,'2013-02-10 00:03:13'),(211,1,16777343,'2013-02-10 00:06:40'),(212,1,16777343,'2013-02-10 00:08:38'),(213,1,16777343,'2013-02-10 00:11:11'),(214,1,16777343,'2013-02-10 00:33:45'),(215,1,16777343,'2013-02-10 00:41:13'),(216,1,16777343,'2013-02-10 00:57:10'),(217,1,16777343,'2013-02-10 01:05:22'),(218,1,16777343,'2013-02-10 01:06:28'),(219,1,16777343,'2013-02-10 01:12:50'),(220,1,16777343,'2013-02-10 01:18:53'),(221,1,16777343,'2013-02-10 01:19:45'),(222,1,16777343,'2013-02-10 01:21:57'),(223,1,16777343,'2013-02-10 01:23:54'),(224,1,16777343,'2013-02-10 01:25:01'),(225,1,16777343,'2013-02-10 01:26:50'),(226,1,16777343,'2013-02-10 01:30:57'),(227,1,16777343,'2013-02-10 01:49:33'),(228,1,16777343,'2013-02-10 01:50:21'),(229,1,16777343,'2013-02-10 01:51:08'),(230,1,16777343,'2013-02-10 01:56:04'),(231,1,16777343,'2013-02-10 02:06:05'),(232,1,16777343,'2013-02-10 13:28:28'),(233,1,16777343,'2013-02-10 13:33:54'),(234,1,16777343,'2013-02-10 13:35:24'),(235,1,16777343,'2013-02-10 13:37:45'),(236,1,16777343,'2013-02-10 13:40:05'),(237,1,16777343,'2013-02-10 13:40:39'),(238,1,16777343,'2013-02-10 13:44:31'),(239,1,16777343,'2013-02-10 13:54:26'),(240,1,16777343,'2013-02-10 13:56:48'),(241,1,16777343,'2013-02-10 14:00:28'),(242,1,16777343,'2013-02-10 14:02:28'),(243,1,16777343,'2013-02-10 14:03:36'),(244,1,16777343,'2013-02-10 14:04:17'),(245,1,16777343,'2013-02-10 14:06:19'),(246,1,16777343,'2013-02-10 14:09:36'),(247,1,16777343,'2013-02-10 14:10:50'),(248,1,16777343,'2013-02-10 14:12:17'),(249,1,16777343,'2013-02-10 14:16:26'),(250,1,16777343,'2013-02-10 14:18:27'),(251,1,16777343,'2013-02-10 14:23:54'),(252,1,16777343,'2013-02-10 14:37:10'),(253,1,16777343,'2013-02-10 14:41:08'),(254,1,16777343,'2013-02-10 14:45:23'),(255,1,16777343,'2013-02-10 14:49:49'),(256,1,16777343,'2013-02-10 14:53:43'),(257,1,16777343,'2013-02-10 15:26:26'),(258,1,16777343,'2013-02-10 15:37:20'),(259,1,16777343,'2013-02-10 15:38:17'),(260,1,16777343,'2013-02-10 15:47:03'),(261,1,16777343,'2013-02-10 15:49:53'),(262,1,16777343,'2013-02-10 15:50:37'),(263,1,16777343,'2013-02-10 15:52:10'),(264,1,16777343,'2013-02-10 15:54:45'),(265,1,16777343,'2018-12-18 14:44:44'),(266,1,16777343,'2018-12-18 14:45:23'),(267,1,16777343,'2018-12-18 14:53:53'),(268,1,16777343,'2018-12-18 14:55:00'),(269,1,16777343,'2018-12-18 15:04:40'),(270,1,16777343,'2018-12-18 15:08:24'),(271,1,16777343,'2018-12-18 15:16:08'),(272,1,16777343,'2018-12-18 15:17:44'),(273,1,16777343,'2018-12-18 15:22:07'),(274,1,16777343,'2018-12-18 15:25:20'),(275,1,16777343,'2018-12-18 15:25:47'),(276,1,16777343,'2018-12-18 15:30:41');
/*!40000 ALTER TABLE `account_ips` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `active_trade_cash`
--

DROP TABLE IF EXISTS `active_trade_cash`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
-- Table structure for table `applied_patches`
--

DROP TABLE IF EXISTS `applied_patches`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `applied_patches` (
  `file_name` varchar(255) NOT NULL,
  `date_applied` datetime NOT NULL,
  PRIMARY KEY (`file_name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applied_patches`
--

LOCK TABLES `applied_patches` WRITE;
/*!40000 ALTER TABLE `applied_patches` DISABLE KEYS */;
INSERT INTO `applied_patches` VALUES ('2013-02-01 Sample.sql','2018-12-18 14:42:49');
/*!40000 ALTER TABLE `applied_patches` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character`
--

DROP TABLE IF EXISTS `character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1 COMMENT='Persisted (users, persistent NPCs) chars.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character`
--

LOCK TABLES `character` WRITE;
/*!40000 ALTER TABLE `character` DISABLE KEYS */;
INSERT INTO `character` VALUES (1,NULL,'Spodi',NULL,NULL,NULL,4,654,481,1,512,512,1,1800,515,34,1015,130,50,50,50,50,1,36,1,1,1,1),(6,NULL,'Helix',NULL,NULL,NULL,3,450,316,1,512,512,1,1800,20,1,20,0,50,50,50,50,1,1,1,1,1,1),(7,NULL,'tester',NULL,NULL,NULL,2,458,625,1,512,512,1,1800,40,1,15,0,3,50,50,50,1,1,1,1,1,1),(8,NULL,'waat',NULL,NULL,NULL,2,458,625,1,512,512,1,1800,10,1,10,0,50,50,50,50,1,1,1,1,1,1),(9,NULL,'uid',NULL,NULL,NULL,2,458,625,1,512,512,1,1800,35,2,35,0,49,50,50,50,5,2,1,1,1,1),(10,NULL,'ptd',NULL,NULL,NULL,1,523,650,1,512,512,1,1800,185,2,35,0,50,50,50,50,6,1,1,1,1,1);
/*!40000 ALTER TABLE `character` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_equipped`
--

DROP TABLE IF EXISTS `character_equipped`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `character_equipped` VALUES (7,100,2),(7,101,1),(8,104,1),(8,105,2),(9,109,1),(9,110,2),(10,127,2);
/*!40000 ALTER TABLE `character_equipped` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_inventory`
--

DROP TABLE IF EXISTS `character_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `character_inventory` VALUES (7,98,0),(7,99,1),(8,102,0),(8,103,1),(9,106,0),(9,107,1),(9,108,2),(9,111,3),(10,112,0),(10,117,3),(10,128,1),(10,130,2);
/*!40000 ALTER TABLE `character_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_quest_status`
--

DROP TABLE IF EXISTS `character_quest_status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `character_quest_status` VALUES (1,0,'2013-02-07 00:44:31','2013-02-09 15:02:52'),(1,1,'2013-02-10 14:46:29','2013-02-10 15:56:39'),(8,0,'2018-12-18 15:18:28',NULL),(8,1,'2018-12-18 15:18:30',NULL);
/*!40000 ALTER TABLE `character_quest_status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_quest_status_kills`
--

DROP TABLE IF EXISTS `character_quest_status_kills`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `character_skill` VALUES (1,0,'2012-12-15 11:18:10'),(1,1,'2012-12-15 11:18:11');
/*!40000 ALTER TABLE `character_skill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_status_effect`
--

DROP TABLE IF EXISTS `character_status_effect`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `character_status_effect` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Unique ID of the status effect instance.',
  `character_id` int(11) NOT NULL COMMENT 'ID of the Character that the status effect is on.',
  `status_effect_id` tinyint(3) unsigned NOT NULL COMMENT 'ID of the status effect that this effect is for. This corresponds to the StatusEffectType enum''s value.',
  `power` smallint(5) unsigned NOT NULL COMMENT 'The power of this status effect instance.',
  `time_left_secs` smallint(5) unsigned NOT NULL COMMENT 'The amount of time remaining for this status effect in seconds.',
  PRIMARY KEY (`id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `character_status_effect_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1 COMMENT='Active status effects on a character.';
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
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `character_template` VALUES (0,0,'User Template',NULL,NULL,NULL,1,1800,5,1,0,0,0,0,50,50,1,2,1,1,1,1),(1,1,'Bee',1,NULL,NULL,5,1500,10,1,0,0,5,5,5,5,1,2,1,1,1,1),(2,2,'Gallot',NULL,NULL,NULL,6,3000,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(4,2,'Shopkeeper',NULL,0,NULL,3,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(5,2,'Inn Keeper',NULL,NULL,0,3,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(6,3,'Brawler',1,NULL,NULL,2,500,5,1,0,0,8,8,20,20,1,2,1,1,1,1);
/*!40000 ALTER TABLE `character_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_equipped`
--

DROP TABLE IF EXISTS `character_template_equipped`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `character_template_inventory` VALUES (0,1,5,0,2,10000),(1,1,5,0,2,10000),(3,1,5,0,2,10000),(4,1,7,1,10,65535),(5,6,3,1,1,655),(6,6,8,1,1,655),(7,1,3,1,1,5000),(8,1,3,1,1,5000),(9,1,7,1,10,65535),(10,1,3,1,1,5000),(11,1,7,1,10,65535),(12,1,3,1,1,5000),(13,1,5,0,2,10000),(14,1,7,1,10,65535),(15,1,3,1,1,5000),(16,1,7,1,10,65535),(17,6,6,1,1,655),(18,6,7,1,1,655),(19,6,5,1,1,3277),(20,6,2,1,1,6554),(21,6,1,1,1,6554);
/*!40000 ALTER TABLE `character_template_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_template_quest_provider`
--

DROP TABLE IF EXISTS `character_template_quest_provider`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `event_counters_item_template` VALUES (1,0,312),(1,1,11),(1,5,95),(1,6,73),(1,8,28),(2,0,331),(2,5,92),(2,6,77),(2,8,30),(3,0,264),(3,4,1000),(3,5,104),(3,6,100),(3,8,27),(4,0,1),(5,0,384),(5,3,2),(5,5,132),(5,6,174),(5,8,55),(6,0,33),(6,5,9),(6,6,9),(6,8,2),(7,0,10564),(7,2,25),(7,4,11),(7,5,333),(7,6,5621),(7,8,1367),(8,0,3237),(8,5,25),(8,6,10),(8,8,5);
/*!40000 ALTER TABLE `event_counters_item_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_map`
--

DROP TABLE IF EXISTS `event_counters_map`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `event_counters_map` VALUES (1,0,50),(1,1,459),(1,2,5522),(1,3,148),(1,4,233),(1,5,15),(1,7,821),(1,8,821),(2,0,16),(2,1,3038),(2,2,205),(2,3,705),(2,4,705),(2,7,672),(2,8,672),(3,0,27),(3,1,104),(4,0,15),(4,1,10);
/*!40000 ALTER TABLE `event_counters_map` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_npc`
--

DROP TABLE IF EXISTS `event_counters_npc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `event_counters_npc` VALUES (0,6,145),(0,8,32),(1,1,2),(1,2,15),(1,3,1474),(1,4,145),(1,5,1810),(1,7,1003),(1,8,164),(6,1,616),(6,3,169),(6,4,30903),(6,5,2136),(6,6,30903),(6,7,31072),(6,8,31106);
/*!40000 ALTER TABLE `event_counters_npc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_quest`
--

DROP TABLE IF EXISTS `event_counters_quest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `event_counters_quest` VALUES (0,1,2),(0,2,1),(1,1,4),(1,2,3);
/*!40000 ALTER TABLE `event_counters_quest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_shop`
--

DROP TABLE IF EXISTS `event_counters_shop`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `event_counters_shop` VALUES (0,1,2),(0,2,2),(0,3,2000),(0,4,1),(0,5,25),(0,6,25);
/*!40000 ALTER TABLE `event_counters_shop` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event_counters_user`
--

DROP TABLE IF EXISTS `event_counters_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `event_counters_user` VALUES (1,2,232),(1,4,4555),(1,6,1524),(1,7,349),(1,8,1057),(1,9,11),(1,100,13),(1,101,76),(7,2,1),(7,4,5),(7,6,82),(7,7,5),(7,8,82),(8,2,2),(8,4,12),(8,7,12),(9,4,3),(9,6,44),(9,7,3),(9,8,8);
/*!40000 ALTER TABLE `event_counters_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guild`
--

DROP TABLE IF EXISTS `guild`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
  `skill_id` tinyint(5) unsigned DEFAULT NULL COMMENT 'The skill the item can set for a user.',
  PRIMARY KEY (`id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `item_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=131 DEFAULT CHARSET=latin1 COMMENT='The live, persisted items.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES (98,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',36,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1,NULL),(99,3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',1,237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL,NULL),(100,3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',1,237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL,NULL),(101,5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',1,235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL,NULL),(102,5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',1,235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL,NULL),(103,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',54,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1,NULL),(104,5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',1,235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL,NULL),(105,3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',1,237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL,NULL),(106,5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',5,235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL,NULL),(107,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1,NULL),(108,3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',2,237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL,NULL),(109,5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',1,235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL,NULL),(110,3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',1,237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL,NULL),(111,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',62,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1,NULL),(112,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',99,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1,NULL),(113,3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',1,237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL,NULL),(116,5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',1,235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL,NULL),(117,7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',90,182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1,NULL),(118,5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',1,235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL,NULL),(127,6,2,2,500,16,16,'Pistol','Just point it at whatever you want to die.',1,177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL,NULL,NULL),(128,5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',3,235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL,NULL),(130,3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',1,237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL,NULL);
/*!40000 ALTER TABLE `item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item_template`
--

DROP TABLE IF EXISTS `item_template`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
  `skill_id` tinyint(5) unsigned DEFAULT NULL COMMENT 'The skill the item can set for a user.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPRESSED COMMENT='The templates used to instantiate items.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item_template`
--

LOCK TABLES `item_template` WRITE;
/*!40000 ALTER TABLE `item_template` DISABLE KEYS */;
INSERT INTO `item_template` VALUES (0,2,1,10,16,16,'Unarmed','Unarmed',1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,NULL),(1,1,0,0,9,16,'Healing Potion','A healing potion',95,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,NULL),(2,1,0,0,9,16,'Mana Potion','A mana potion',94,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,NULL),(3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL,NULL),(4,4,0,0,30,30,'Black Armor','Body armor made out of... black.',234,2000,0,0,5,5,5,5,5,0,0,20,0,0,0,'Body.Black',NULL,NULL),(5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL,NULL),(6,2,2,500,16,16,'Pistol','Just point it at whatever you want to die.',177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL,NULL,NULL),(7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1,NULL),(8,4,0,0,30,30,'Iron Armor','Body armor made out of iron.',236,50,0,0,1,0,0,0,0,0,0,6,0,0,0,'Body.Iron',NULL,NULL);
/*!40000 ALTER TABLE `item_template` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `map`
--

DROP TABLE IF EXISTS `map`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `map` VALUES (1,'Town'),(2,'Jail'),(3,'Shop'),(4,'New map');
/*!40000 ALTER TABLE `map` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `map_spawn`
--

DROP TABLE IF EXISTS `map_spawn`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `map_spawn` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'The unique ID of this NPC spawn.',
  `map_id` smallint(5) unsigned NOT NULL COMMENT 'The map that this spawn takes place on.',
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template used to instantiate the spawned NPCs.',
  `amount` tinyint(3) unsigned NOT NULL COMMENT 'The total number of NPCs this spawner will spawn.',
  `respawn` smallint(5) unsigned NOT NULL DEFAULT '5' COMMENT 'How long in seconds to wait after death to be respawned (intended for NPCs only).',
  `x` smallint(5) unsigned DEFAULT NULL COMMENT 'The x coordinate of the spawner (NULL indicates the left-most side of the map). Example: All x/y/width/height set to NULL spawns NPCs anywhere on the map.',
  `y` smallint(5) unsigned DEFAULT NULL COMMENT 'The y coordinate of the spawner (NULL indicates the top-most side of the map).',
  `width` smallint(5) unsigned DEFAULT NULL COMMENT 'The width of the spawner (NULL indicates the right-most side of the map).',
  `height` smallint(5) unsigned DEFAULT NULL COMMENT 'The height of the spawner (NULL indicates the bottom- side of the map).',
  `direction_id` smallint(5) NOT NULL DEFAULT '0' COMMENT 'The direction of this spawn; None if randonm',
  PRIMARY KEY (`id`),
  KEY `character_id` (`character_template_id`),
  KEY `map_id` (`map_id`),
  CONSTRAINT `map_spawn_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `map_spawn_ibfk_2` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=latin1 COMMENT='NPC spawns for the maps.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `map_spawn`
--

LOCK TABLES `map_spawn` WRITE;
/*!40000 ALTER TABLE `map_spawn` DISABLE KEYS */;
INSERT INTO `map_spawn` VALUES (5,2,6,45,5,130,165,720,475,0),(9,1,1,3,20,NULL,NULL,NULL,NULL,0),(11,1,2,1,5,600,580,32,32,0),(16,3,4,1,5,250,250,1,1,0),(17,3,5,1,5,678,253,1,1,0),(18,4,2,1,5,600,400,128,128,5);
/*!40000 ALTER TABLE `map_spawn` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest`
--

DROP TABLE IF EXISTS `quest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
CREATE TABLE `server_time` (
  `server_time` datetime NOT NULL COMMENT 'The current time of the server, as seen by the server process. Only updated when server is running. Especially intended for when comparing the time to the server''s current time. Slightly low resolution (assume ~10 seconds).'
) ENGINE=MyISAM DEFAULT CHARSET=latin1 MAX_ROWS=1 ROW_FORMAT=FIXED COMMENT='Holds the current time of the server.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `server_time`
--

LOCK TABLES `server_time` WRITE;
/*!40000 ALTER TABLE `server_time` DISABLE KEYS */;
INSERT INTO `server_time` VALUES ('2018-12-18 15:34:07');
/*!40000 ALTER TABLE `server_time` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shop`
--

DROP TABLE IF EXISTS `shop`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
-- Temporary view structure for view `view_npc_character`
--

DROP TABLE IF EXISTS `view_npc_character`;
/*!50001 DROP VIEW IF EXISTS `view_npc_character`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `view_npc_character` AS SELECT 
 1 AS `id`,
 1 AS `character_template_id`,
 1 AS `name`,
 1 AS `shop_id`,
 1 AS `chat_dialog`,
 1 AS `ai_id`,
 1 AS `load_map_id`,
 1 AS `load_x`,
 1 AS `load_y`,
 1 AS `respawn_map_id`,
 1 AS `respawn_x`,
 1 AS `respawn_y`,
 1 AS `body_id`,
 1 AS `move_speed`,
 1 AS `cash`,
 1 AS `level`,
 1 AS `exp`,
 1 AS `statpoints`,
 1 AS `hp`,
 1 AS `mp`,
 1 AS `stat_maxhp`,
 1 AS `stat_maxmp`,
 1 AS `stat_minhit`,
 1 AS `stat_maxhit`,
 1 AS `stat_defence`,
 1 AS `stat_agi`,
 1 AS `stat_int`,
 1 AS `stat_str`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_user_character`
--

DROP TABLE IF EXISTS `view_user_character`;
/*!50001 DROP VIEW IF EXISTS `view_user_character`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8mb4;
/*!50001 CREATE VIEW `view_user_character` AS SELECT 
 1 AS `id`,
 1 AS `character_template_id`,
 1 AS `name`,
 1 AS `shop_id`,
 1 AS `chat_dialog`,
 1 AS `ai_id`,
 1 AS `load_map_id`,
 1 AS `load_x`,
 1 AS `load_y`,
 1 AS `respawn_map_id`,
 1 AS `respawn_x`,
 1 AS `respawn_y`,
 1 AS `body_id`,
 1 AS `move_speed`,
 1 AS `cash`,
 1 AS `level`,
 1 AS `exp`,
 1 AS `statpoints`,
 1 AS `hp`,
 1 AS `mp`,
 1 AS `stat_maxhp`,
 1 AS `stat_maxmp`,
 1 AS `stat_minhit`,
 1 AS `stat_maxhit`,
 1 AS `stat_defence`,
 1 AS `stat_agi`,
 1 AS `stat_int`,
 1 AS `stat_str`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `world_stats_count_consume_item`
--

DROP TABLE IF EXISTS `world_stats_count_consume_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `world_stats_count_consume_item` VALUES (1,11,'2013-02-10 04:31:56'),(2,14,'2013-02-10 03:25:18');
/*!40000 ALTER TABLE `world_stats_count_consume_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_item_buy`
--

DROP TABLE IF EXISTS `world_stats_count_item_buy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `world_stats_count_item_buy` VALUES (5,2,'2013-02-06 05:48:10'),(6,1,'2018-12-18 20:32:31');
/*!40000 ALTER TABLE `world_stats_count_item_buy` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_item_create`
--

DROP TABLE IF EXISTS `world_stats_count_item_create`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `world_stats_count_item_create` VALUES (1,1342,'2018-12-18 20:30:37'),(2,1474,'2018-12-18 20:30:37'),(3,1073,'2018-12-18 20:33:47'),(4,2,'2013-02-05 03:59:25'),(5,1449,'2018-12-18 20:33:47'),(6,153,'2018-12-18 20:32:31'),(7,35212,'2018-12-18 20:33:47'),(8,14416,'2018-12-18 20:30:37');
/*!40000 ALTER TABLE `world_stats_count_item_create` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_item_sell`
--

DROP TABLE IF EXISTS `world_stats_count_item_sell`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `world_stats_count_item_sell` VALUES (3,3,'2018-12-18 20:32:25'),(5,1,'2018-12-18 20:32:28'),(7,25,'2018-12-18 20:09:31');
/*!40000 ALTER TABLE `world_stats_count_item_sell` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_npc_kill_user`
--

DROP TABLE IF EXISTS `world_stats_count_npc_kill_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `world_stats_count_npc_kill_user` VALUES (1,1,17,'2013-02-10 19:30:50'),(6,1,3,'2013-02-04 23:55:47'),(9,1,1,'2018-12-18 20:26:04');
/*!40000 ALTER TABLE `world_stats_count_npc_kill_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_shop_buy`
--

DROP TABLE IF EXISTS `world_stats_count_shop_buy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `world_stats_count_shop_buy` VALUES (0,3,'2018-12-18 20:32:31');
/*!40000 ALTER TABLE `world_stats_count_shop_buy` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_shop_sell`
--

DROP TABLE IF EXISTS `world_stats_count_shop_sell`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `world_stats_count_shop_sell` VALUES (0,29,'2018-12-18 20:32:28');
/*!40000 ALTER TABLE `world_stats_count_shop_sell` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_user_consume_item`
--

DROP TABLE IF EXISTS `world_stats_count_user_consume_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `world_stats_count_user_consume_item` VALUES (1,1,11,'2013-02-10 04:31:56'),(1,2,14,'2013-02-10 03:25:18');
/*!40000 ALTER TABLE `world_stats_count_user_consume_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_count_user_kill_npc`
--

DROP TABLE IF EXISTS `world_stats_count_user_kill_npc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
INSERT INTO `world_stats_count_user_kill_npc` VALUES (1,1,350,'2013-02-10 20:56:13'),(1,6,250,'2013-02-10 20:55:26'),(6,1,4,'2013-02-10 02:18:09'),(7,1,3,'2018-12-18 20:10:05'),(8,1,2,'2018-12-18 20:18:17'),(9,1,7,'2018-12-18 20:26:54'),(10,1,7,'2018-12-18 20:33:47');
/*!40000 ALTER TABLE `world_stats_count_user_kill_npc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_guild_user_change`
--

DROP TABLE IF EXISTS `world_stats_guild_user_change`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
) ENGINE=MyISAM AUTO_INCREMENT=1860 DEFAULT CHARSET=latin1 COMMENT='Snapshots of network deltas.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_network`
--

LOCK TABLES `world_stats_network` WRITE;
/*!40000 ALTER TABLE `world_stats_network` DISABLE KEYS */;
INSERT INTO `world_stats_network` VALUES (1,'2013-02-04 22:51:41',1,0,0,0,0,0,0),(2,'2013-02-04 22:52:41',1,0,0,0,0,0,0),(3,'2013-02-04 22:53:41',0,0,0,0,0,0,0),(4,'2013-02-04 23:41:09',1,0,0,0,0,0,0),(5,'2013-02-04 23:42:09',1,0,0,0,0,0,0),(6,'2013-02-04 23:43:09',1,0,0,0,0,0,0),(7,'2013-02-04 23:54:38',1,0,0,0,0,0,0),(8,'2013-02-04 23:55:38',2,0,0,0,0,0,0),(9,'2013-02-04 23:56:38',1,0,0,0,0,0,0),(10,'2013-02-04 23:59:16',0,0,0,0,0,0,0),(11,'2013-02-05 00:10:17',2,0,0,0,0,0,0),(12,'2013-02-05 00:11:17',0,0,0,0,0,0,0),(13,'2013-02-05 00:31:09',1,0,0,0,0,0,0),(14,'2013-02-05 00:32:23',0,0,0,0,0,0,0),(15,'2013-02-05 01:10:26',1,0,0,0,0,0,0),(16,'2013-02-05 01:11:26',0,0,0,0,0,0,0),(17,'2013-02-05 01:21:22',0,0,0,0,0,0,0),(18,'2013-02-05 01:22:22',0,0,0,0,0,0,0),(19,'2013-02-05 01:23:22',0,0,0,0,0,0,0),(20,'2013-02-05 01:26:05',1,0,0,0,0,0,0),(21,'2013-02-05 01:27:05',0,0,0,0,0,0,0),(22,'2013-02-05 01:28:19',0,0,0,0,0,0,0),(23,'2013-02-05 01:34:59',1,0,0,0,0,0,0),(24,'2013-02-05 01:35:59',0,0,0,0,0,0,0),(25,'2013-02-05 02:14:08',1,0,0,0,0,0,0),(26,'2013-02-05 02:15:08',1,0,0,0,0,0,0),(27,'2013-02-05 02:22:54',1,0,0,0,0,0,0),(28,'2013-02-05 02:30:06',1,0,0,0,0,0,0),(29,'2013-02-05 03:13:20',2,0,0,0,0,0,0),(30,'2013-02-05 03:14:33',1,0,0,0,0,0,0),(31,'2013-02-05 03:15:20',2,0,0,0,0,0,0),(32,'2013-02-05 03:27:29',1,0,0,0,0,0,0),(33,'2013-02-05 03:35:05',1,0,0,0,0,0,0),(34,'2013-02-05 03:38:39',1,0,0,0,0,0,0),(35,'2013-02-05 03:40:40',1,0,0,0,0,0,0),(36,'2013-02-05 03:41:40',1,0,0,0,0,0,0),(37,'2013-02-05 03:43:46',1,0,0,0,0,0,0),(38,'2013-02-05 04:00:25',0,0,0,0,0,0,0),(39,'2013-02-05 04:01:25',0,0,0,0,0,0,0),(40,'2013-02-05 04:02:25',0,0,0,0,0,0,0),(41,'2013-02-05 04:03:25',0,0,0,0,0,0,0),(42,'2013-02-05 04:04:25',0,0,0,0,0,0,0),(43,'2013-02-05 04:05:25',0,0,0,0,0,0,0),(44,'2013-02-05 04:06:25',0,0,0,0,0,0,0),(45,'2013-02-05 04:07:25',0,0,0,0,0,0,0),(46,'2013-02-05 04:08:25',0,0,0,0,0,0,0),(47,'2013-02-05 04:09:25',0,0,0,0,0,0,0),(48,'2013-02-05 04:10:25',0,0,0,0,0,0,0),(49,'2013-02-05 04:11:59',1,0,0,0,0,0,0),(50,'2013-02-05 04:12:59',1,0,0,0,0,0,0),(51,'2013-02-05 04:13:59',1,0,0,0,0,0,0),(52,'2013-02-05 04:14:59',1,0,0,0,0,0,0),(53,'2013-02-05 04:15:59',1,0,0,0,0,0,0),(54,'2013-02-05 04:16:59',1,0,0,0,0,0,0),(55,'2013-02-05 04:17:59',1,0,0,0,0,0,0),(56,'2013-02-05 04:18:59',1,0,0,0,0,0,0),(57,'2013-02-05 04:19:59',1,0,0,0,0,0,0),(58,'2013-02-05 04:20:59',1,0,0,0,0,0,0),(59,'2013-02-05 04:21:59',1,0,0,0,0,0,0),(60,'2013-02-05 04:24:42',1,0,0,0,0,0,0),(61,'2013-02-05 04:25:42',1,0,0,0,0,0,0),(62,'2013-02-05 04:26:42',1,0,0,0,0,0,0),(63,'2013-02-05 04:27:42',1,0,0,0,0,0,0),(64,'2013-02-05 04:28:42',0,0,0,0,0,0,0),(65,'2013-02-05 04:29:42',0,0,0,0,0,0,0),(66,'2013-02-05 04:30:42',0,0,0,0,0,0,0),(67,'2013-02-05 04:32:01',1,0,0,0,0,0,0),(68,'2013-02-05 04:33:01',1,0,0,0,0,0,0),(69,'2013-02-05 04:34:01',1,0,0,0,0,0,0),(70,'2013-02-05 04:47:00',1,0,0,0,0,0,0),(71,'2013-02-05 04:48:00',1,0,0,0,0,0,0),(72,'2013-02-05 04:49:00',1,0,0,0,0,0,0),(73,'2013-02-05 04:50:00',1,0,0,0,0,0,0),(74,'2013-02-05 04:51:00',0,0,0,0,0,0,0),(75,'2013-02-05 04:52:00',0,0,0,0,0,0,0),(76,'2013-02-05 04:53:00',0,0,0,0,0,0,0),(77,'2013-02-05 04:54:00',0,0,0,0,0,0,0),(78,'2013-02-05 04:55:00',0,0,0,0,0,0,0),(79,'2013-02-05 04:56:00',0,0,0,0,0,0,0),(80,'2013-02-05 04:57:00',0,0,0,0,0,0,0),(81,'2013-02-05 04:58:00',0,0,0,0,0,0,0),(82,'2013-02-05 04:59:00',0,0,0,0,0,0,0),(83,'2013-02-05 05:00:00',0,0,0,0,0,0,0),(84,'2013-02-05 06:17:36',1,0,0,0,0,0,0),(85,'2013-02-05 06:18:36',0,0,0,0,0,0,0),(86,'2013-02-05 06:19:36',0,0,0,0,0,0,0),(87,'2013-02-05 06:20:36',0,0,0,0,0,0,0),(88,'2013-02-05 06:21:36',0,0,0,0,0,0,0),(89,'2013-02-05 06:22:36',0,0,0,0,0,0,0),(90,'2013-02-05 06:23:36',0,0,0,0,0,0,0),(91,'2013-02-05 06:24:36',0,0,0,0,0,0,0),(92,'2013-02-05 06:25:36',0,0,0,0,0,0,0),(93,'2013-02-05 06:26:36',0,0,0,0,0,0,0),(94,'2013-02-05 06:27:36',0,0,0,0,0,0,0),(95,'2013-02-05 06:28:36',0,0,0,0,0,0,0),(96,'2013-02-05 06:29:36',0,0,0,0,0,0,0),(97,'2013-02-05 06:30:36',0,0,0,0,0,0,0),(98,'2013-02-05 06:31:36',0,0,0,0,0,0,0),(99,'2013-02-05 06:32:36',0,0,0,0,0,0,0),(100,'2013-02-05 06:33:36',0,0,0,0,0,0,0),(101,'2013-02-05 06:34:36',0,0,0,0,0,0,0),(102,'2013-02-05 06:35:36',0,0,0,0,0,0,0),(103,'2013-02-05 06:36:36',0,0,0,0,0,0,0),(104,'2013-02-05 06:37:36',0,0,0,0,0,0,0),(105,'2013-02-05 06:38:36',0,0,0,0,0,0,0),(106,'2013-02-05 06:39:36',0,0,0,0,0,0,0),(107,'2013-02-05 06:40:36',0,0,0,0,0,0,0),(108,'2013-02-05 06:41:36',0,0,0,0,0,0,0),(109,'2013-02-05 06:42:36',0,0,0,0,0,0,0),(110,'2013-02-05 06:43:36',0,0,0,0,0,0,0),(111,'2013-02-05 06:44:36',0,0,0,0,0,0,0),(112,'2013-02-05 06:45:36',0,0,0,0,0,0,0),(113,'2013-02-05 06:46:36',0,0,0,0,0,0,0),(114,'2013-02-05 06:47:36',0,0,0,0,0,0,0),(115,'2013-02-05 06:48:36',0,0,0,0,0,0,0),(116,'2013-02-05 06:49:36',0,0,0,0,0,0,0),(117,'2013-02-05 06:50:36',0,0,0,0,0,0,0),(118,'2013-02-05 06:51:36',0,0,0,0,0,0,0),(119,'2013-02-05 06:52:36',0,0,0,0,0,0,0),(120,'2013-02-05 06:53:36',0,0,0,0,0,0,0),(121,'2013-02-05 06:54:36',0,0,0,0,0,0,0),(122,'2013-02-05 06:55:36',0,0,0,0,0,0,0),(123,'2013-02-05 06:56:36',0,0,0,0,0,0,0),(124,'2013-02-05 06:57:36',0,0,0,0,0,0,0),(125,'2013-02-05 06:58:36',0,0,0,0,0,0,0),(126,'2013-02-05 07:00:13',2,0,0,0,0,0,0),(127,'2013-02-05 07:01:13',2,0,0,0,0,0,0),(128,'2013-02-05 07:02:13',2,0,0,0,0,0,0),(129,'2013-02-05 07:03:13',2,0,0,0,0,0,0),(130,'2013-02-05 07:04:13',2,0,0,0,0,0,0),(131,'2013-02-06 04:09:38',1,0,0,0,0,0,0),(132,'2013-02-06 04:27:13',0,0,0,0,0,0,0),(133,'2013-02-06 04:29:53',0,0,0,0,0,0,0),(134,'2013-02-06 04:34:41',1,0,0,0,0,0,0),(135,'2013-02-06 04:35:41',1,0,0,0,0,0,0),(136,'2013-02-06 04:36:41',1,0,0,0,0,0,0),(137,'2013-02-06 04:37:41',1,0,0,0,0,0,0),(138,'2013-02-06 04:38:41',1,0,0,0,0,0,0),(139,'2013-02-06 04:39:41',1,0,0,0,0,0,0),(140,'2013-02-06 05:09:10',1,0,0,0,0,0,0),(141,'2013-02-06 05:10:10',1,0,0,0,0,0,0),(142,'2013-02-06 05:28:51',2,0,0,0,0,0,0),(143,'2013-02-06 05:32:47',2,0,0,0,0,0,0),(144,'2013-02-06 05:47:54',1,0,0,0,0,0,0),(145,'2013-02-06 05:48:54',1,0,0,0,0,0,0),(146,'2013-02-06 06:14:17',1,0,0,0,0,0,0),(147,'2013-02-06 06:15:17',1,0,0,0,0,0,0),(148,'2013-02-06 06:16:17',1,0,0,0,0,0,0),(149,'2013-02-06 06:17:17',1,0,0,0,0,0,0),(150,'2013-02-06 06:18:17',0,0,0,0,0,0,0),(151,'2013-02-06 06:19:17',0,0,0,0,0,0,0),(152,'2013-02-06 06:20:17',0,0,0,0,0,0,0),(153,'2013-02-06 06:21:17',0,0,0,0,0,0,0),(154,'2013-02-06 06:22:17',0,0,0,0,0,0,0),(155,'2013-02-06 06:23:17',0,0,0,0,0,0,0),(156,'2013-02-06 06:24:56',1,0,0,0,0,0,0),(157,'2013-02-06 06:25:56',0,0,0,0,0,0,0),(158,'2013-02-06 06:26:56',0,0,0,0,0,0,0),(159,'2013-02-06 06:27:56',0,0,0,0,0,0,0),(160,'2013-02-06 06:28:56',0,0,0,0,0,0,0),(161,'2013-02-06 06:29:56',0,0,0,0,0,0,0),(162,'2013-02-06 06:30:56',0,0,0,0,0,0,0),(163,'2013-02-06 06:31:56',0,0,0,0,0,0,0),(164,'2013-02-06 06:32:56',0,0,0,0,0,0,0),(165,'2013-02-06 06:33:56',0,0,0,0,0,0,0),(166,'2013-02-06 06:34:56',0,0,0,0,0,0,0),(167,'2013-02-06 06:35:56',0,0,0,0,0,0,0),(168,'2013-02-06 06:36:56',0,0,0,0,0,0,0),(169,'2013-02-06 06:37:56',0,0,0,0,0,0,0),(170,'2013-02-06 06:38:56',0,0,0,0,0,0,0),(171,'2013-02-06 06:39:56',0,0,0,0,0,0,0),(172,'2013-02-06 06:40:56',0,0,0,0,0,0,0),(173,'2013-02-06 06:41:56',0,0,0,0,0,0,0),(174,'2013-02-06 06:42:56',0,0,0,0,0,0,0),(175,'2013-02-06 06:43:56',0,0,0,0,0,0,0),(176,'2013-02-06 06:44:56',0,0,0,0,0,0,0),(177,'2013-02-06 06:45:56',0,0,0,0,0,0,0),(178,'2013-02-06 06:46:56',0,0,0,0,0,0,0),(179,'2013-02-06 06:47:56',0,0,0,0,0,0,0),(180,'2013-02-06 06:48:56',0,0,0,0,0,0,0),(181,'2013-02-06 06:49:56',0,0,0,0,0,0,0),(182,'2013-02-06 06:50:56',0,0,0,0,0,0,0),(183,'2013-02-06 06:51:56',0,0,0,0,0,0,0),(184,'2013-02-06 06:52:56',0,0,0,0,0,0,0),(185,'2013-02-06 06:53:56',0,0,0,0,0,0,0),(186,'2013-02-06 06:54:56',0,0,0,0,0,0,0),(187,'2013-02-06 06:55:56',0,0,0,0,0,0,0),(188,'2013-02-06 06:56:56',0,0,0,0,0,0,0),(189,'2013-02-06 06:57:56',0,0,0,0,0,0,0),(190,'2013-02-06 06:58:56',0,0,0,0,0,0,0),(191,'2013-02-06 06:59:56',0,0,0,0,0,0,0),(192,'2013-02-06 07:00:56',0,0,0,0,0,0,0),(193,'2013-02-06 07:01:56',0,0,0,0,0,0,0),(194,'2013-02-06 07:02:56',0,0,0,0,0,0,0),(195,'2013-02-06 07:03:56',0,0,0,0,0,0,0),(196,'2013-02-06 07:04:56',0,0,0,0,0,0,0),(197,'2013-02-06 07:05:56',0,0,0,0,0,0,0),(198,'2013-02-06 07:06:56',0,0,0,0,0,0,0),(199,'2013-02-06 07:07:56',0,0,0,0,0,0,0),(200,'2013-02-06 07:08:56',0,0,0,0,0,0,0),(201,'2013-02-06 07:09:56',0,0,0,0,0,0,0),(202,'2013-02-06 07:10:56',0,0,0,0,0,0,0),(203,'2013-02-06 07:11:56',0,0,0,0,0,0,0),(204,'2013-02-06 07:12:56',0,0,0,0,0,0,0),(205,'2013-02-06 07:13:56',0,0,0,0,0,0,0),(206,'2013-02-06 07:14:56',0,0,0,0,0,0,0),(207,'2013-02-06 07:15:56',0,0,0,0,0,0,0),(208,'2013-02-06 07:16:56',0,0,0,0,0,0,0),(209,'2013-02-06 07:17:56',0,0,0,0,0,0,0),(210,'2013-02-06 07:18:56',0,0,0,0,0,0,0),(211,'2013-02-06 07:19:56',0,0,0,0,0,0,0),(212,'2013-02-06 07:20:56',0,0,0,0,0,0,0),(213,'2013-02-06 07:21:56',0,0,0,0,0,0,0),(214,'2013-02-06 07:22:56',0,0,0,0,0,0,0),(215,'2013-02-06 07:23:56',0,0,0,0,0,0,0),(216,'2013-02-06 07:24:56',0,0,0,0,0,0,0),(217,'2013-02-06 07:25:56',0,0,0,0,0,0,0),(218,'2013-02-06 07:26:56',0,0,0,0,0,0,0),(219,'2013-02-06 07:27:56',0,0,0,0,0,0,0),(220,'2013-02-06 07:28:56',0,0,0,0,0,0,0),(221,'2013-02-06 07:29:56',0,0,0,0,0,0,0),(222,'2013-02-06 07:30:56',0,0,0,0,0,0,0),(223,'2013-02-06 07:31:56',0,0,0,0,0,0,0),(224,'2013-02-06 07:32:56',0,0,0,0,0,0,0),(225,'2013-02-06 07:33:56',0,0,0,0,0,0,0),(226,'2013-02-06 07:34:56',0,0,0,0,0,0,0),(227,'2013-02-06 07:35:56',0,0,0,0,0,0,0),(228,'2013-02-06 07:36:56',0,0,0,0,0,0,0),(229,'2013-02-06 07:37:56',0,0,0,0,0,0,0),(230,'2013-02-06 07:38:56',0,0,0,0,0,0,0),(231,'2013-02-06 07:39:56',0,0,0,0,0,0,0),(232,'2013-02-06 07:40:56',0,0,0,0,0,0,0),(233,'2013-02-06 07:41:56',0,0,0,0,0,0,0),(234,'2013-02-06 07:42:56',0,0,0,0,0,0,0),(235,'2013-02-06 07:43:56',0,0,0,0,0,0,0),(236,'2013-02-06 07:44:56',0,0,0,0,0,0,0),(237,'2013-02-06 07:45:56',0,0,0,0,0,0,0),(238,'2013-02-06 07:46:56',0,0,0,0,0,0,0),(239,'2013-02-06 07:47:56',0,0,0,0,0,0,0),(240,'2013-02-06 07:48:56',0,0,0,0,0,0,0),(241,'2013-02-06 07:49:56',0,0,0,0,0,0,0),(242,'2013-02-06 07:50:56',0,0,0,0,0,0,0),(243,'2013-02-06 07:51:56',0,0,0,0,0,0,0),(244,'2013-02-06 07:52:56',0,0,0,0,0,0,0),(245,'2013-02-06 07:53:56',0,0,0,0,0,0,0),(246,'2013-02-06 07:54:56',0,0,0,0,0,0,0),(247,'2013-02-06 07:55:56',0,0,0,0,0,0,0),(248,'2013-02-06 07:56:56',0,0,0,0,0,0,0),(249,'2013-02-06 07:57:56',0,0,0,0,0,0,0),(250,'2013-02-06 07:58:56',0,0,0,0,0,0,0),(251,'2013-02-06 07:59:56',0,0,0,0,0,0,0),(252,'2013-02-06 08:00:56',0,0,0,0,0,0,0),(253,'2013-02-06 08:01:56',0,0,0,0,0,0,0),(254,'2013-02-06 08:02:56',0,0,0,0,0,0,0),(255,'2013-02-06 08:03:56',0,0,0,0,0,0,0),(256,'2013-02-06 08:04:56',0,0,0,0,0,0,0),(257,'2013-02-06 08:05:56',0,0,0,0,0,0,0),(258,'2013-02-06 08:06:56',0,0,0,0,0,0,0),(259,'2013-02-06 08:07:56',0,0,0,0,0,0,0),(260,'2013-02-06 08:08:56',0,0,0,0,0,0,0),(261,'2013-02-06 08:09:56',0,0,0,0,0,0,0),(262,'2013-02-06 08:10:56',0,0,0,0,0,0,0),(263,'2013-02-06 08:11:56',0,0,0,0,0,0,0),(264,'2013-02-06 08:12:56',0,0,0,0,0,0,0),(265,'2013-02-06 08:13:56',0,0,0,0,0,0,0),(266,'2013-02-06 08:14:56',0,0,0,0,0,0,0),(267,'2013-02-06 08:15:56',0,0,0,0,0,0,0),(268,'2013-02-06 08:16:56',0,0,0,0,0,0,0),(269,'2013-02-06 08:17:56',0,0,0,0,0,0,0),(270,'2013-02-06 08:18:56',0,0,0,0,0,0,0),(271,'2013-02-06 08:19:56',0,0,0,0,0,0,0),(272,'2013-02-06 08:20:56',0,0,0,0,0,0,0),(273,'2013-02-06 08:21:56',0,0,0,0,0,0,0),(274,'2013-02-06 08:22:56',0,0,0,0,0,0,0),(275,'2013-02-06 08:23:56',0,0,0,0,0,0,0),(276,'2013-02-06 08:24:56',0,0,0,0,0,0,0),(277,'2013-02-06 08:25:56',0,0,0,0,0,0,0),(278,'2013-02-06 08:26:56',0,0,0,0,0,0,0),(279,'2013-02-06 08:27:56',0,0,0,0,0,0,0),(280,'2013-02-06 08:28:56',0,0,0,0,0,0,0),(281,'2013-02-06 08:29:56',0,0,0,0,0,0,0),(282,'2013-02-06 08:30:56',0,0,0,0,0,0,0),(283,'2013-02-06 08:31:56',0,0,0,0,0,0,0),(284,'2013-02-06 08:32:56',0,0,0,0,0,0,0),(285,'2013-02-06 08:33:56',0,0,0,0,0,0,0),(286,'2013-02-06 08:34:56',0,0,0,0,0,0,0),(287,'2013-02-06 08:35:56',0,0,0,0,0,0,0),(288,'2013-02-06 08:36:56',0,0,0,0,0,0,0),(289,'2013-02-06 08:37:56',0,0,0,0,0,0,0),(290,'2013-02-06 08:38:56',0,0,0,0,0,0,0),(291,'2013-02-06 08:39:56',0,0,0,0,0,0,0),(292,'2013-02-06 08:40:56',0,0,0,0,0,0,0),(293,'2013-02-06 08:41:56',0,0,0,0,0,0,0),(294,'2013-02-06 08:42:56',0,0,0,0,0,0,0),(295,'2013-02-06 08:43:56',0,0,0,0,0,0,0),(296,'2013-02-06 08:44:56',0,0,0,0,0,0,0),(297,'2013-02-06 08:45:56',0,0,0,0,0,0,0),(298,'2013-02-06 08:46:56',0,0,0,0,0,0,0),(299,'2013-02-06 08:47:56',0,0,0,0,0,0,0),(300,'2013-02-06 08:48:56',0,0,0,0,0,0,0),(301,'2013-02-06 08:49:56',0,0,0,0,0,0,0),(302,'2013-02-06 08:50:56',0,0,0,0,0,0,0),(303,'2013-02-06 08:51:56',0,0,0,0,0,0,0),(304,'2013-02-06 08:52:56',0,0,0,0,0,0,0),(305,'2013-02-06 08:53:56',0,0,0,0,0,0,0),(306,'2013-02-06 08:54:56',0,0,0,0,0,0,0),(307,'2013-02-06 08:55:56',0,0,0,0,0,0,0),(308,'2013-02-06 08:56:56',0,0,0,0,0,0,0),(309,'2013-02-06 08:57:56',0,0,0,0,0,0,0),(310,'2013-02-06 08:58:56',0,0,0,0,0,0,0),(311,'2013-02-06 08:59:56',0,0,0,0,0,0,0),(312,'2013-02-06 09:00:56',0,0,0,0,0,0,0),(313,'2013-02-06 09:01:56',0,0,0,0,0,0,0),(314,'2013-02-06 09:02:56',0,0,0,0,0,0,0),(315,'2013-02-06 09:03:56',0,0,0,0,0,0,0),(316,'2013-02-06 09:04:56',0,0,0,0,0,0,0),(317,'2013-02-06 09:05:56',0,0,0,0,0,0,0),(318,'2013-02-06 09:06:56',0,0,0,0,0,0,0),(319,'2013-02-06 09:07:56',0,0,0,0,0,0,0),(320,'2013-02-06 09:08:56',0,0,0,0,0,0,0),(321,'2013-02-06 09:09:56',0,0,0,0,0,0,0),(322,'2013-02-06 09:10:56',0,0,0,0,0,0,0),(323,'2013-02-06 09:11:56',0,0,0,0,0,0,0),(324,'2013-02-06 09:12:56',0,0,0,0,0,0,0),(325,'2013-02-06 09:13:56',0,0,0,0,0,0,0),(326,'2013-02-06 09:14:56',0,0,0,0,0,0,0),(327,'2013-02-06 09:15:56',0,0,0,0,0,0,0),(328,'2013-02-06 09:16:56',0,0,0,0,0,0,0),(329,'2013-02-06 09:17:56',0,0,0,0,0,0,0),(330,'2013-02-06 09:18:56',0,0,0,0,0,0,0),(331,'2013-02-06 09:19:56',0,0,0,0,0,0,0),(332,'2013-02-06 09:20:56',0,0,0,0,0,0,0),(333,'2013-02-06 09:21:56',0,0,0,0,0,0,0),(334,'2013-02-06 09:22:56',0,0,0,0,0,0,0),(335,'2013-02-06 09:23:56',0,0,0,0,0,0,0),(336,'2013-02-06 09:24:56',0,0,0,0,0,0,0),(337,'2013-02-06 09:25:56',0,0,0,0,0,0,0),(338,'2013-02-06 09:26:56',0,0,0,0,0,0,0),(339,'2013-02-06 09:27:56',0,0,0,0,0,0,0),(340,'2013-02-06 09:28:56',0,0,0,0,0,0,0),(341,'2013-02-06 09:29:56',0,0,0,0,0,0,0),(342,'2013-02-06 09:30:56',0,0,0,0,0,0,0),(343,'2013-02-06 09:31:56',0,0,0,0,0,0,0),(344,'2013-02-06 09:32:56',0,0,0,0,0,0,0),(345,'2013-02-06 09:33:56',0,0,0,0,0,0,0),(346,'2013-02-06 09:34:56',0,0,0,0,0,0,0),(347,'2013-02-06 09:35:56',0,0,0,0,0,0,0),(348,'2013-02-06 09:36:56',0,0,0,0,0,0,0),(349,'2013-02-06 09:37:56',0,0,0,0,0,0,0),(350,'2013-02-06 09:38:56',0,0,0,0,0,0,0),(351,'2013-02-06 09:39:56',0,0,0,0,0,0,0),(352,'2013-02-06 09:40:56',0,0,0,0,0,0,0),(353,'2013-02-06 09:41:56',0,0,0,0,0,0,0),(354,'2013-02-06 09:42:56',0,0,0,0,0,0,0),(355,'2013-02-06 09:43:56',0,0,0,0,0,0,0),(356,'2013-02-06 09:44:56',0,0,0,0,0,0,0),(357,'2013-02-06 09:45:56',0,0,0,0,0,0,0),(358,'2013-02-06 09:46:56',0,0,0,0,0,0,0),(359,'2013-02-06 09:47:56',0,0,0,0,0,0,0),(360,'2013-02-06 09:48:56',0,0,0,0,0,0,0),(361,'2013-02-06 09:49:56',0,0,0,0,0,0,0),(362,'2013-02-06 09:50:56',0,0,0,0,0,0,0),(363,'2013-02-06 09:51:56',0,0,0,0,0,0,0),(364,'2013-02-06 09:52:56',0,0,0,0,0,0,0),(365,'2013-02-06 09:53:56',0,0,0,0,0,0,0),(366,'2013-02-06 09:54:56',0,0,0,0,0,0,0),(367,'2013-02-06 09:55:56',0,0,0,0,0,0,0),(368,'2013-02-06 09:56:56',0,0,0,0,0,0,0),(369,'2013-02-06 09:57:56',0,0,0,0,0,0,0),(370,'2013-02-06 09:58:56',0,0,0,0,0,0,0),(371,'2013-02-06 09:59:56',0,0,0,0,0,0,0),(372,'2013-02-06 10:00:56',0,0,0,0,0,0,0),(373,'2013-02-06 10:01:56',0,0,0,0,0,0,0),(374,'2013-02-06 10:02:56',0,0,0,0,0,0,0),(375,'2013-02-06 10:03:56',0,0,0,0,0,0,0),(376,'2013-02-06 10:04:56',0,0,0,0,0,0,0),(377,'2013-02-06 10:05:56',0,0,0,0,0,0,0),(378,'2013-02-06 10:06:56',0,0,0,0,0,0,0),(379,'2013-02-06 10:07:56',0,0,0,0,0,0,0),(380,'2013-02-06 10:08:56',0,0,0,0,0,0,0),(381,'2013-02-06 10:09:56',0,0,0,0,0,0,0),(382,'2013-02-06 10:10:56',0,0,0,0,0,0,0),(383,'2013-02-06 10:11:56',0,0,0,0,0,0,0),(384,'2013-02-06 10:12:56',0,0,0,0,0,0,0),(385,'2013-02-06 10:13:56',0,0,0,0,0,0,0),(386,'2013-02-06 10:14:56',0,0,0,0,0,0,0),(387,'2013-02-06 10:15:56',0,0,0,0,0,0,0),(388,'2013-02-06 10:16:56',0,0,0,0,0,0,0),(389,'2013-02-06 10:17:56',0,0,0,0,0,0,0),(390,'2013-02-06 10:18:56',0,0,0,0,0,0,0),(391,'2013-02-06 10:19:56',0,0,0,0,0,0,0),(392,'2013-02-06 10:20:56',0,0,0,0,0,0,0),(393,'2013-02-06 10:21:56',0,0,0,0,0,0,0),(394,'2013-02-06 10:22:56',0,0,0,0,0,0,0),(395,'2013-02-06 10:23:56',0,0,0,0,0,0,0),(396,'2013-02-06 10:24:56',0,0,0,0,0,0,0),(397,'2013-02-06 10:25:56',0,0,0,0,0,0,0),(398,'2013-02-06 10:26:56',0,0,0,0,0,0,0),(399,'2013-02-06 10:27:56',0,0,0,0,0,0,0),(400,'2013-02-06 10:28:56',0,0,0,0,0,0,0),(401,'2013-02-06 10:29:56',0,0,0,0,0,0,0),(402,'2013-02-06 10:30:56',0,0,0,0,0,0,0),(403,'2013-02-06 10:31:56',0,0,0,0,0,0,0),(404,'2013-02-06 10:32:56',0,0,0,0,0,0,0),(405,'2013-02-06 10:33:56',0,0,0,0,0,0,0),(406,'2013-02-06 10:34:56',0,0,0,0,0,0,0),(407,'2013-02-06 10:35:56',0,0,0,0,0,0,0),(408,'2013-02-06 10:36:56',0,0,0,0,0,0,0),(409,'2013-02-06 10:37:56',0,0,0,0,0,0,0),(410,'2013-02-06 10:38:56',0,0,0,0,0,0,0),(411,'2013-02-06 10:39:56',0,0,0,0,0,0,0),(412,'2013-02-06 10:40:56',0,0,0,0,0,0,0),(413,'2013-02-06 10:41:56',0,0,0,0,0,0,0),(414,'2013-02-06 10:42:56',0,0,0,0,0,0,0),(415,'2013-02-06 10:43:56',0,0,0,0,0,0,0),(416,'2013-02-06 10:44:56',0,0,0,0,0,0,0),(417,'2013-02-06 10:45:56',0,0,0,0,0,0,0),(418,'2013-02-06 10:46:56',0,0,0,0,0,0,0),(419,'2013-02-06 10:47:56',0,0,0,0,0,0,0),(420,'2013-02-06 10:48:56',0,0,0,0,0,0,0),(421,'2013-02-06 10:49:56',0,0,0,0,0,0,0),(422,'2013-02-06 10:50:56',0,0,0,0,0,0,0),(423,'2013-02-06 10:51:56',0,0,0,0,0,0,0),(424,'2013-02-06 10:52:56',0,0,0,0,0,0,0),(425,'2013-02-06 10:53:56',0,0,0,0,0,0,0),(426,'2013-02-06 10:54:56',0,0,0,0,0,0,0),(427,'2013-02-06 10:55:56',0,0,0,0,0,0,0),(428,'2013-02-06 10:56:56',0,0,0,0,0,0,0),(429,'2013-02-06 10:57:56',0,0,0,0,0,0,0),(430,'2013-02-06 10:58:56',0,0,0,0,0,0,0),(431,'2013-02-06 10:59:56',0,0,0,0,0,0,0),(432,'2013-02-06 11:00:56',0,0,0,0,0,0,0),(433,'2013-02-06 11:01:56',0,0,0,0,0,0,0),(434,'2013-02-06 11:02:56',0,0,0,0,0,0,0),(435,'2013-02-06 11:03:56',0,0,0,0,0,0,0),(436,'2013-02-06 11:04:56',0,0,0,0,0,0,0),(437,'2013-02-06 11:05:56',0,0,0,0,0,0,0),(438,'2013-02-06 11:06:56',0,0,0,0,0,0,0),(439,'2013-02-06 11:07:56',0,0,0,0,0,0,0),(440,'2013-02-06 11:08:56',0,0,0,0,0,0,0),(441,'2013-02-06 11:09:56',0,0,0,0,0,0,0),(442,'2013-02-06 11:10:56',0,0,0,0,0,0,0),(443,'2013-02-06 11:11:56',0,0,0,0,0,0,0),(444,'2013-02-06 11:12:56',0,0,0,0,0,0,0),(445,'2013-02-06 11:13:56',0,0,0,0,0,0,0),(446,'2013-02-06 11:14:56',0,0,0,0,0,0,0),(447,'2013-02-06 11:15:56',0,0,0,0,0,0,0),(448,'2013-02-06 11:16:56',0,0,0,0,0,0,0),(449,'2013-02-06 11:17:56',0,0,0,0,0,0,0),(450,'2013-02-06 11:18:56',0,0,0,0,0,0,0),(451,'2013-02-06 11:19:56',0,0,0,0,0,0,0),(452,'2013-02-06 11:20:56',0,0,0,0,0,0,0),(453,'2013-02-06 11:21:56',0,0,0,0,0,0,0),(454,'2013-02-06 11:22:56',0,0,0,0,0,0,0),(455,'2013-02-06 11:23:56',0,0,0,0,0,0,0),(456,'2013-02-06 11:24:56',0,0,0,0,0,0,0),(457,'2013-02-06 11:25:56',0,0,0,0,0,0,0),(458,'2013-02-06 11:26:56',0,0,0,0,0,0,0),(459,'2013-02-06 11:27:56',0,0,0,0,0,0,0),(460,'2013-02-06 11:28:56',0,0,0,0,0,0,0),(461,'2013-02-06 11:29:56',0,0,0,0,0,0,0),(462,'2013-02-06 11:30:56',0,0,0,0,0,0,0),(463,'2013-02-06 11:31:56',0,0,0,0,0,0,0),(464,'2013-02-06 11:32:56',0,0,0,0,0,0,0),(465,'2013-02-06 11:33:56',0,0,0,0,0,0,0),(466,'2013-02-06 11:34:56',0,0,0,0,0,0,0),(467,'2013-02-06 11:35:56',0,0,0,0,0,0,0),(468,'2013-02-06 11:36:56',0,0,0,0,0,0,0),(469,'2013-02-06 11:37:56',0,0,0,0,0,0,0),(470,'2013-02-06 11:38:56',0,0,0,0,0,0,0),(471,'2013-02-06 11:39:56',0,0,0,0,0,0,0),(472,'2013-02-06 11:40:56',0,0,0,0,0,0,0),(473,'2013-02-06 11:41:56',0,0,0,0,0,0,0),(474,'2013-02-06 11:42:56',0,0,0,0,0,0,0),(475,'2013-02-06 11:43:56',0,0,0,0,0,0,0),(476,'2013-02-06 11:44:56',0,0,0,0,0,0,0),(477,'2013-02-06 11:45:56',0,0,0,0,0,0,0),(478,'2013-02-06 11:46:56',0,0,0,0,0,0,0),(479,'2013-02-06 11:47:56',0,0,0,0,0,0,0),(480,'2013-02-06 11:48:56',0,0,0,0,0,0,0),(481,'2013-02-06 11:49:56',0,0,0,0,0,0,0),(482,'2013-02-06 11:50:56',0,0,0,0,0,0,0),(483,'2013-02-06 11:51:56',0,0,0,0,0,0,0),(484,'2013-02-06 11:52:56',0,0,0,0,0,0,0),(485,'2013-02-06 11:53:56',0,0,0,0,0,0,0),(486,'2013-02-06 11:54:56',0,0,0,0,0,0,0),(487,'2013-02-06 11:55:56',0,0,0,0,0,0,0),(488,'2013-02-06 11:56:56',0,0,0,0,0,0,0),(489,'2013-02-06 11:57:56',0,0,0,0,0,0,0),(490,'2013-02-06 11:58:56',0,0,0,0,0,0,0),(491,'2013-02-06 11:59:56',0,0,0,0,0,0,0),(492,'2013-02-06 12:00:56',0,0,0,0,0,0,0),(493,'2013-02-06 12:01:56',0,0,0,0,0,0,0),(494,'2013-02-06 12:02:56',0,0,0,0,0,0,0),(495,'2013-02-06 12:03:56',0,0,0,0,0,0,0),(496,'2013-02-06 12:04:56',0,0,0,0,0,0,0),(497,'2013-02-06 12:05:56',0,0,0,0,0,0,0),(498,'2013-02-06 12:06:56',0,0,0,0,0,0,0),(499,'2013-02-06 12:07:56',0,0,0,0,0,0,0),(500,'2013-02-06 12:08:56',0,0,0,0,0,0,0),(501,'2013-02-06 12:09:56',0,0,0,0,0,0,0),(502,'2013-02-06 12:10:56',0,0,0,0,0,0,0),(503,'2013-02-06 12:11:56',0,0,0,0,0,0,0),(504,'2013-02-06 12:12:56',0,0,0,0,0,0,0),(505,'2013-02-06 12:13:56',0,0,0,0,0,0,0),(506,'2013-02-06 12:14:56',0,0,0,0,0,0,0),(507,'2013-02-06 12:15:56',0,0,0,0,0,0,0),(508,'2013-02-06 12:16:56',0,0,0,0,0,0,0),(509,'2013-02-06 12:17:56',0,0,0,0,0,0,0),(510,'2013-02-06 12:18:56',0,0,0,0,0,0,0),(511,'2013-02-06 12:19:56',0,0,0,0,0,0,0),(512,'2013-02-06 12:20:56',0,0,0,0,0,0,0),(513,'2013-02-06 12:21:56',0,0,0,0,0,0,0),(514,'2013-02-06 12:22:56',0,0,0,0,0,0,0),(515,'2013-02-06 12:23:56',0,0,0,0,0,0,0),(516,'2013-02-06 12:24:56',0,0,0,0,0,0,0),(517,'2013-02-06 12:25:56',0,0,0,0,0,0,0),(518,'2013-02-06 12:26:56',0,0,0,0,0,0,0),(519,'2013-02-06 12:27:56',0,0,0,0,0,0,0),(520,'2013-02-06 12:28:56',0,0,0,0,0,0,0),(521,'2013-02-06 12:29:56',0,0,0,0,0,0,0),(522,'2013-02-06 12:30:56',0,0,0,0,0,0,0),(523,'2013-02-06 12:31:56',0,0,0,0,0,0,0),(524,'2013-02-06 12:32:56',0,0,0,0,0,0,0),(525,'2013-02-06 12:33:56',0,0,0,0,0,0,0),(526,'2013-02-06 12:34:56',0,0,0,0,0,0,0),(527,'2013-02-06 12:35:56',0,0,0,0,0,0,0),(528,'2013-02-06 12:36:56',0,0,0,0,0,0,0),(529,'2013-02-06 12:37:56',0,0,0,0,0,0,0),(530,'2013-02-06 12:38:56',0,0,0,0,0,0,0),(531,'2013-02-06 12:39:56',0,0,0,0,0,0,0),(532,'2013-02-06 12:40:56',0,0,0,0,0,0,0),(533,'2013-02-06 12:41:56',0,0,0,0,0,0,0),(534,'2013-02-06 12:42:56',0,0,0,0,0,0,0),(535,'2013-02-06 12:43:56',0,0,0,0,0,0,0),(536,'2013-02-06 12:44:56',0,0,0,0,0,0,0),(537,'2013-02-06 12:45:56',0,0,0,0,0,0,0),(538,'2013-02-06 12:46:56',0,0,0,0,0,0,0),(539,'2013-02-06 12:47:56',0,0,0,0,0,0,0),(540,'2013-02-06 12:48:56',0,0,0,0,0,0,0),(541,'2013-02-06 12:49:56',0,0,0,0,0,0,0),(542,'2013-02-06 12:50:56',0,0,0,0,0,0,0),(543,'2013-02-06 12:51:56',0,0,0,0,0,0,0),(544,'2013-02-06 12:52:56',0,0,0,0,0,0,0),(545,'2013-02-06 12:53:56',0,0,0,0,0,0,0),(546,'2013-02-06 12:54:56',0,0,0,0,0,0,0),(547,'2013-02-06 12:55:56',0,0,0,0,0,0,0),(548,'2013-02-06 12:56:56',0,0,0,0,0,0,0),(549,'2013-02-06 12:57:56',0,0,0,0,0,0,0),(550,'2013-02-06 12:58:56',0,0,0,0,0,0,0),(551,'2013-02-06 12:59:56',0,0,0,0,0,0,0),(552,'2013-02-06 13:00:56',0,0,0,0,0,0,0),(553,'2013-02-06 13:01:56',0,0,0,0,0,0,0),(554,'2013-02-06 13:02:56',0,0,0,0,0,0,0),(555,'2013-02-06 13:03:56',0,0,0,0,0,0,0),(556,'2013-02-06 13:04:56',0,0,0,0,0,0,0),(557,'2013-02-06 13:05:56',0,0,0,0,0,0,0),(558,'2013-02-06 13:06:56',0,0,0,0,0,0,0),(559,'2013-02-06 13:07:56',0,0,0,0,0,0,0),(560,'2013-02-06 13:08:56',0,0,0,0,0,0,0),(561,'2013-02-06 13:09:56',0,0,0,0,0,0,0),(562,'2013-02-06 13:10:56',0,0,0,0,0,0,0),(563,'2013-02-06 13:11:56',0,0,0,0,0,0,0),(564,'2013-02-06 13:12:56',0,0,0,0,0,0,0),(565,'2013-02-06 13:13:56',0,0,0,0,0,0,0),(566,'2013-02-06 13:14:56',0,0,0,0,0,0,0),(567,'2013-02-06 13:15:56',0,0,0,0,0,0,0),(568,'2013-02-06 13:16:56',0,0,0,0,0,0,0),(569,'2013-02-06 13:17:56',0,0,0,0,0,0,0),(570,'2013-02-06 13:18:56',0,0,0,0,0,0,0),(571,'2013-02-06 13:19:56',0,0,0,0,0,0,0),(572,'2013-02-06 13:20:56',0,0,0,0,0,0,0),(573,'2013-02-06 13:21:56',0,0,0,0,0,0,0),(574,'2013-02-06 13:22:56',0,0,0,0,0,0,0),(575,'2013-02-06 13:23:56',0,0,0,0,0,0,0),(576,'2013-02-06 13:24:56',0,0,0,0,0,0,0),(577,'2013-02-06 13:25:56',0,0,0,0,0,0,0),(578,'2013-02-06 13:26:56',0,0,0,0,0,0,0),(579,'2013-02-06 13:27:56',0,0,0,0,0,0,0),(580,'2013-02-06 13:28:56',0,0,0,0,0,0,0),(581,'2013-02-06 13:29:56',0,0,0,0,0,0,0),(582,'2013-02-06 13:30:56',0,0,0,0,0,0,0),(583,'2013-02-06 13:31:56',0,0,0,0,0,0,0),(584,'2013-02-06 13:32:56',0,0,0,0,0,0,0),(585,'2013-02-06 13:33:56',0,0,0,0,0,0,0),(586,'2013-02-06 13:34:56',0,0,0,0,0,0,0),(587,'2013-02-06 13:35:56',0,0,0,0,0,0,0),(588,'2013-02-06 13:36:56',0,0,0,0,0,0,0),(589,'2013-02-06 13:37:56',0,0,0,0,0,0,0),(590,'2013-02-06 13:38:56',0,0,0,0,0,0,0),(591,'2013-02-06 13:39:56',0,0,0,0,0,0,0),(592,'2013-02-06 13:40:56',0,0,0,0,0,0,0),(593,'2013-02-06 13:41:56',0,0,0,0,0,0,0),(594,'2013-02-06 13:42:56',0,0,0,0,0,0,0),(595,'2013-02-06 13:43:56',0,0,0,0,0,0,0),(596,'2013-02-06 13:44:56',0,0,0,0,0,0,0),(597,'2013-02-06 13:45:56',0,0,0,0,0,0,0),(598,'2013-02-06 13:46:56',0,0,0,0,0,0,0),(599,'2013-02-06 13:47:56',0,0,0,0,0,0,0),(600,'2013-02-06 13:48:56',0,0,0,0,0,0,0),(601,'2013-02-06 13:49:56',0,0,0,0,0,0,0),(602,'2013-02-06 13:50:56',0,0,0,0,0,0,0),(603,'2013-02-06 13:51:56',0,0,0,0,0,0,0),(604,'2013-02-06 13:52:56',0,0,0,0,0,0,0),(605,'2013-02-06 13:53:56',0,0,0,0,0,0,0),(606,'2013-02-06 13:54:56',0,0,0,0,0,0,0),(607,'2013-02-06 13:55:56',0,0,0,0,0,0,0),(608,'2013-02-06 13:56:56',0,0,0,0,0,0,0),(609,'2013-02-06 13:57:56',0,0,0,0,0,0,0),(610,'2013-02-06 13:58:56',0,0,0,0,0,0,0),(611,'2013-02-06 13:59:56',0,0,0,0,0,0,0),(612,'2013-02-06 14:00:56',0,0,0,0,0,0,0),(613,'2013-02-06 14:01:56',0,0,0,0,0,0,0),(614,'2013-02-06 14:02:56',0,0,0,0,0,0,0),(615,'2013-02-06 14:03:56',0,0,0,0,0,0,0),(616,'2013-02-06 14:04:56',0,0,0,0,0,0,0),(617,'2013-02-06 14:05:56',0,0,0,0,0,0,0),(618,'2013-02-06 14:06:56',0,0,0,0,0,0,0),(619,'2013-02-06 14:07:56',0,0,0,0,0,0,0),(620,'2013-02-06 14:08:56',0,0,0,0,0,0,0),(621,'2013-02-06 14:09:56',0,0,0,0,0,0,0),(622,'2013-02-06 14:10:56',0,0,0,0,0,0,0),(623,'2013-02-06 14:11:56',0,0,0,0,0,0,0),(624,'2013-02-06 14:12:56',0,0,0,0,0,0,0),(625,'2013-02-06 14:13:56',0,0,0,0,0,0,0),(626,'2013-02-06 14:14:56',0,0,0,0,0,0,0),(627,'2013-02-06 14:15:56',0,0,0,0,0,0,0),(628,'2013-02-06 14:16:56',0,0,0,0,0,0,0),(629,'2013-02-06 14:17:56',0,0,0,0,0,0,0),(630,'2013-02-06 14:18:56',0,0,0,0,0,0,0),(631,'2013-02-06 14:19:56',0,0,0,0,0,0,0),(632,'2013-02-06 14:20:56',0,0,0,0,0,0,0),(633,'2013-02-06 14:21:56',0,0,0,0,0,0,0),(634,'2013-02-06 14:22:56',0,0,0,0,0,0,0),(635,'2013-02-06 14:23:56',0,0,0,0,0,0,0),(636,'2013-02-06 14:24:56',0,0,0,0,0,0,0),(637,'2013-02-06 14:25:56',0,0,0,0,0,0,0),(638,'2013-02-06 14:26:56',0,0,0,0,0,0,0),(639,'2013-02-06 14:27:56',0,0,0,0,0,0,0),(640,'2013-02-06 14:28:56',0,0,0,0,0,0,0),(641,'2013-02-06 14:29:56',0,0,0,0,0,0,0),(642,'2013-02-06 14:30:56',0,0,0,0,0,0,0),(643,'2013-02-06 14:31:56',0,0,0,0,0,0,0),(644,'2013-02-06 14:32:56',0,0,0,0,0,0,0),(645,'2013-02-06 14:33:56',0,0,0,0,0,0,0),(646,'2013-02-06 14:34:56',0,0,0,0,0,0,0),(647,'2013-02-06 14:35:56',0,0,0,0,0,0,0),(648,'2013-02-06 14:36:56',0,0,0,0,0,0,0),(649,'2013-02-06 14:37:56',0,0,0,0,0,0,0),(650,'2013-02-06 14:38:56',0,0,0,0,0,0,0),(651,'2013-02-06 14:39:56',0,0,0,0,0,0,0),(652,'2013-02-06 14:40:56',0,0,0,0,0,0,0),(653,'2013-02-06 14:41:56',0,0,0,0,0,0,0),(654,'2013-02-06 14:42:56',0,0,0,0,0,0,0),(655,'2013-02-06 14:43:56',0,0,0,0,0,0,0),(656,'2013-02-06 14:44:56',0,0,0,0,0,0,0),(657,'2013-02-06 14:45:56',0,0,0,0,0,0,0),(658,'2013-02-06 14:46:56',0,0,0,0,0,0,0),(659,'2013-02-06 14:47:56',0,0,0,0,0,0,0),(660,'2013-02-06 21:08:14',1,0,0,0,0,0,0),(661,'2013-02-06 21:09:14',1,0,0,0,0,0,0),(662,'2013-02-06 21:10:14',1,0,0,0,0,0,0),(663,'2013-02-06 21:11:14',1,0,0,0,0,0,0),(664,'2013-02-06 21:12:14',1,0,0,0,0,0,0),(665,'2013-02-07 05:08:23',0,0,0,0,0,0,0),(666,'2013-02-07 05:12:49',0,0,0,0,0,0,0),(667,'2013-02-07 05:15:34',1,0,0,0,0,0,0),(668,'2013-02-07 05:16:34',1,0,0,0,0,0,0),(669,'2013-02-07 05:17:34',1,0,0,0,0,0,0),(670,'2013-02-07 05:28:43',1,0,0,0,0,0,0),(671,'2013-02-07 05:30:22',1,0,0,0,0,0,0),(672,'2013-02-07 05:31:22',1,0,0,0,0,0,0),(673,'2013-02-07 05:36:20',1,0,0,0,0,0,0),(674,'2013-02-07 05:37:20',1,0,0,0,0,0,0),(675,'2013-02-07 05:38:20',1,0,0,0,0,0,0),(676,'2013-02-07 05:39:20',1,0,0,0,0,0,0),(677,'2013-02-07 05:40:20',1,0,0,0,0,0,0),(678,'2013-02-07 05:41:20',1,0,0,0,0,0,0),(679,'2013-02-07 05:42:20',1,0,0,0,0,0,0),(680,'2013-02-07 05:43:20',1,0,0,0,0,0,0),(681,'2013-02-07 05:44:20',1,0,0,0,0,0,0),(682,'2013-02-07 05:45:20',1,0,0,0,0,0,0),(683,'2013-02-07 05:46:20',1,0,0,0,0,0,0),(684,'2013-02-07 05:47:30',1,0,0,0,0,0,0),(685,'2013-02-07 05:48:20',1,0,0,0,0,0,0),(686,'2013-02-07 05:49:20',1,0,0,0,0,0,0),(687,'2013-02-08 05:05:45',1,0,0,0,0,0,0),(688,'2013-02-08 05:06:45',1,0,0,0,0,0,0),(689,'2013-02-08 05:13:42',1,0,0,0,0,0,0),(690,'2013-02-08 05:14:42',1,0,0,0,0,0,0),(691,'2013-02-08 05:15:42',1,0,0,0,0,0,0),(692,'2013-02-08 05:18:59',1,0,0,0,0,0,0),(693,'2013-02-08 05:31:18',1,0,0,0,0,0,0),(694,'2013-02-08 05:57:14',1,0,0,0,0,0,0),(695,'2013-02-08 06:02:31',1,0,0,0,0,0,0),(696,'2013-02-08 06:03:31',1,0,0,0,0,0,0),(697,'2013-02-08 06:04:31',1,0,0,0,0,0,0),(698,'2013-02-08 06:05:31',0,0,0,0,0,0,0),(699,'2013-02-08 15:29:32',1,0,0,0,0,0,0),(700,'2013-02-08 15:30:32',1,0,0,0,0,0,0),(701,'2013-02-08 15:31:32',1,0,0,0,0,0,0),(702,'2013-02-08 15:32:32',1,0,0,0,0,0,0),(703,'2013-02-08 15:36:29',1,0,0,0,0,0,0),(704,'2013-02-08 15:37:29',1,0,0,0,0,0,0),(705,'2013-02-08 15:38:29',1,0,0,0,0,0,0),(706,'2013-02-08 15:39:29',1,0,0,0,0,0,0),(707,'2013-02-08 15:40:29',1,0,0,0,0,0,0),(708,'2013-02-08 15:41:29',1,0,0,0,0,0,0),(709,'2013-02-08 15:42:29',1,0,0,0,0,0,0),(710,'2013-02-08 15:43:29',1,0,0,0,0,0,0),(711,'2013-02-08 15:44:29',1,0,0,0,0,0,0),(712,'2013-02-09 16:34:31',0,0,0,0,0,0,0),(713,'2013-02-09 16:35:31',0,0,0,0,0,0,0),(714,'2013-02-09 16:36:31',0,0,0,0,0,0,0),(715,'2013-02-09 16:37:31',0,0,0,0,0,0,0),(716,'2013-02-09 16:38:31',0,0,0,0,0,0,0),(717,'2013-02-09 16:39:31',0,0,0,0,0,0,0),(718,'2013-02-09 16:40:31',0,0,0,0,0,0,0),(719,'2013-02-09 16:41:31',0,0,0,0,0,0,0),(720,'2013-02-09 16:42:31',0,0,0,0,0,0,0),(721,'2013-02-09 16:43:31',0,0,0,0,0,0,0),(722,'2013-02-09 16:44:31',0,0,0,0,0,0,0),(723,'2013-02-09 16:45:31',0,0,0,0,0,0,0),(724,'2013-02-09 16:46:31',0,0,0,0,0,0,0),(725,'2013-02-09 16:47:31',0,0,0,0,0,0,0),(726,'2013-02-09 16:48:31',0,0,0,0,0,0,0),(727,'2013-02-09 16:49:31',0,0,0,0,0,0,0),(728,'2013-02-09 16:50:31',0,0,0,0,0,0,0),(729,'2013-02-09 16:51:31',0,0,0,0,0,0,0),(730,'2013-02-09 16:52:31',0,0,0,0,0,0,0),(731,'2013-02-09 16:53:31',0,0,0,0,0,0,0),(732,'2013-02-09 16:54:31',0,0,0,0,0,0,0),(733,'2013-02-09 16:55:31',0,0,0,0,0,0,0),(734,'2013-02-09 16:56:31',0,0,0,0,0,0,0),(735,'2013-02-09 16:57:31',0,0,0,0,0,0,0),(736,'2013-02-09 16:58:31',0,0,0,0,0,0,0),(737,'2013-02-09 16:59:31',0,0,0,0,0,0,0),(738,'2013-02-09 17:00:31',0,0,0,0,0,0,0),(739,'2013-02-09 17:01:31',0,0,0,0,0,0,0),(740,'2013-02-09 17:02:31',0,0,0,0,0,0,0),(741,'2013-02-09 17:03:31',0,0,0,0,0,0,0),(742,'2013-02-09 17:04:31',0,0,0,0,0,0,0),(743,'2013-02-09 17:05:31',0,0,0,0,0,0,0),(744,'2013-02-09 17:06:31',0,0,0,0,0,0,0),(745,'2013-02-09 17:07:31',0,0,0,0,0,0,0),(746,'2013-02-09 17:08:31',0,0,0,0,0,0,0),(747,'2013-02-09 17:09:31',0,0,0,0,0,0,0),(748,'2013-02-09 17:10:31',0,0,0,0,0,0,0),(749,'2013-02-09 17:11:31',0,0,0,0,0,0,0),(750,'2013-02-09 17:12:31',0,0,0,0,0,0,0),(751,'2013-02-09 17:13:31',0,0,0,0,0,0,0),(752,'2013-02-09 17:14:31',0,0,0,0,0,0,0),(753,'2013-02-09 17:15:31',0,0,0,0,0,0,0),(754,'2013-02-09 17:16:31',0,0,0,0,0,0,0),(755,'2013-02-09 17:17:31',0,0,0,0,0,0,0),(756,'2013-02-09 17:18:31',0,0,0,0,0,0,0),(757,'2013-02-09 17:19:31',0,0,0,0,0,0,0),(758,'2013-02-09 17:20:31',0,0,0,0,0,0,0),(759,'2013-02-09 17:21:31',0,0,0,0,0,0,0),(760,'2013-02-09 17:22:31',0,0,0,0,0,0,0),(761,'2013-02-09 17:23:31',0,0,0,0,0,0,0),(762,'2013-02-09 17:24:31',0,0,0,0,0,0,0),(763,'2013-02-09 17:25:31',0,0,0,0,0,0,0),(764,'2013-02-09 17:26:31',0,0,0,0,0,0,0),(765,'2013-02-09 17:27:31',0,0,0,0,0,0,0),(766,'2013-02-09 17:28:31',0,0,0,0,0,0,0),(767,'2013-02-09 17:29:31',0,0,0,0,0,0,0),(768,'2013-02-09 17:30:31',0,0,0,0,0,0,0),(769,'2013-02-09 17:31:31',0,0,0,0,0,0,0),(770,'2013-02-09 17:32:31',0,0,0,0,0,0,0),(771,'2013-02-09 17:33:31',0,0,0,0,0,0,0),(772,'2013-02-09 17:34:31',0,0,0,0,0,0,0),(773,'2013-02-09 17:35:31',0,0,0,0,0,0,0),(774,'2013-02-09 17:36:31',0,0,0,0,0,0,0),(775,'2013-02-09 17:37:31',0,0,0,0,0,0,0),(776,'2013-02-09 17:38:31',0,0,0,0,0,0,0),(777,'2013-02-09 17:39:31',0,0,0,0,0,0,0),(778,'2013-02-09 17:40:31',0,0,0,0,0,0,0),(779,'2013-02-09 17:41:31',0,0,0,0,0,0,0),(780,'2013-02-09 17:42:31',0,0,0,0,0,0,0),(781,'2013-02-09 17:43:31',0,0,0,0,0,0,0),(782,'2013-02-09 17:44:31',0,0,0,0,0,0,0),(783,'2013-02-09 17:45:31',0,0,0,0,0,0,0),(784,'2013-02-09 17:46:31',0,0,0,0,0,0,0),(785,'2013-02-09 17:47:31',0,0,0,0,0,0,0),(786,'2013-02-09 17:48:31',0,0,0,0,0,0,0),(787,'2013-02-09 17:49:31',0,0,0,0,0,0,0),(788,'2013-02-09 17:50:31',0,0,0,0,0,0,0),(789,'2013-02-09 17:51:31',0,0,0,0,0,0,0),(790,'2013-02-09 17:52:31',0,0,0,0,0,0,0),(791,'2013-02-09 17:53:31',0,0,0,0,0,0,0),(792,'2013-02-09 17:54:31',0,0,0,0,0,0,0),(793,'2013-02-09 17:55:31',0,0,0,0,0,0,0),(794,'2013-02-09 17:56:31',0,0,0,0,0,0,0),(795,'2013-02-09 17:57:31',0,0,0,0,0,0,0),(796,'2013-02-09 17:58:31',0,0,0,0,0,0,0),(797,'2013-02-09 17:59:31',0,0,0,0,0,0,0),(798,'2013-02-09 18:00:31',0,0,0,0,0,0,0),(799,'2013-02-09 18:01:31',0,0,0,0,0,0,0),(800,'2013-02-09 18:02:31',0,0,0,0,0,0,0),(801,'2013-02-09 18:03:31',0,0,0,0,0,0,0),(802,'2013-02-09 18:04:31',0,0,0,0,0,0,0),(803,'2013-02-09 18:05:31',0,0,0,0,0,0,0),(804,'2013-02-09 18:06:31',0,0,0,0,0,0,0),(805,'2013-02-09 18:07:31',0,0,0,0,0,0,0),(806,'2013-02-09 18:08:31',0,0,0,0,0,0,0),(807,'2013-02-09 18:09:31',0,0,0,0,0,0,0),(808,'2013-02-09 18:10:31',0,0,0,0,0,0,0),(809,'2013-02-09 18:11:31',0,0,0,0,0,0,0),(810,'2013-02-09 18:12:31',0,0,0,0,0,0,0),(811,'2013-02-09 18:13:31',0,0,0,0,0,0,0),(812,'2013-02-09 18:14:31',0,0,0,0,0,0,0),(813,'2013-02-09 18:15:31',0,0,0,0,0,0,0),(814,'2013-02-09 18:16:31',0,0,0,0,0,0,0),(815,'2013-02-09 18:17:31',0,0,0,0,0,0,0),(816,'2013-02-09 18:18:31',0,0,0,0,0,0,0),(817,'2013-02-09 18:19:31',0,0,0,0,0,0,0),(818,'2013-02-09 18:20:31',0,0,0,0,0,0,0),(819,'2013-02-09 18:21:31',0,0,0,0,0,0,0),(820,'2013-02-09 18:22:31',0,0,0,0,0,0,0),(821,'2013-02-09 18:23:31',0,0,0,0,0,0,0),(822,'2013-02-09 18:24:31',0,0,0,0,0,0,0),(823,'2013-02-09 18:25:31',0,0,0,0,0,0,0),(824,'2013-02-09 18:26:31',0,0,0,0,0,0,0),(825,'2013-02-09 18:27:31',0,0,0,0,0,0,0),(826,'2013-02-09 18:28:31',0,0,0,0,0,0,0),(827,'2013-02-09 18:29:31',0,0,0,0,0,0,0),(828,'2013-02-09 18:30:31',0,0,0,0,0,0,0),(829,'2013-02-09 18:31:31',0,0,0,0,0,0,0),(830,'2013-02-09 18:32:31',0,0,0,0,0,0,0),(831,'2013-02-09 18:33:31',0,0,0,0,0,0,0),(832,'2013-02-09 18:34:31',0,0,0,0,0,0,0),(833,'2013-02-09 18:35:31',0,0,0,0,0,0,0),(834,'2013-02-09 18:36:31',0,0,0,0,0,0,0),(835,'2013-02-09 18:37:31',0,0,0,0,0,0,0),(836,'2013-02-09 18:38:31',0,0,0,0,0,0,0),(837,'2013-02-09 18:39:31',0,0,0,0,0,0,0),(838,'2013-02-09 18:40:31',0,0,0,0,0,0,0),(839,'2013-02-09 18:41:31',0,0,0,0,0,0,0),(840,'2013-02-09 18:42:31',0,0,0,0,0,0,0),(841,'2013-02-09 18:43:31',0,0,0,0,0,0,0),(842,'2013-02-09 18:44:31',0,0,0,0,0,0,0),(843,'2013-02-09 18:45:31',0,0,0,0,0,0,0),(844,'2013-02-09 18:46:31',0,0,0,0,0,0,0),(845,'2013-02-09 18:47:31',0,0,0,0,0,0,0),(846,'2013-02-09 18:48:31',0,0,0,0,0,0,0),(847,'2013-02-09 18:49:31',0,0,0,0,0,0,0),(848,'2013-02-09 18:50:31',0,0,0,0,0,0,0),(849,'2013-02-09 18:51:31',0,0,0,0,0,0,0),(850,'2013-02-09 18:52:31',0,0,0,0,0,0,0),(851,'2013-02-09 18:53:31',0,0,0,0,0,0,0),(852,'2013-02-09 18:54:31',0,0,0,0,0,0,0),(853,'2013-02-09 18:55:31',0,0,0,0,0,0,0),(854,'2013-02-09 18:56:31',0,0,0,0,0,0,0),(855,'2013-02-09 18:57:31',0,0,0,0,0,0,0),(856,'2013-02-09 18:58:31',0,0,0,0,0,0,0),(857,'2013-02-09 18:59:31',0,0,0,0,0,0,0),(858,'2013-02-09 19:00:31',0,0,0,0,0,0,0),(859,'2013-02-09 19:01:31',0,0,0,0,0,0,0),(860,'2013-02-09 19:02:31',0,0,0,0,0,0,0),(861,'2013-02-09 19:03:31',0,0,0,0,0,0,0),(862,'2013-02-09 19:05:18',0,0,0,0,0,0,0),(863,'2013-02-09 19:21:28',0,0,0,0,0,0,0),(864,'2013-02-09 19:22:28',0,0,0,0,0,0,0),(865,'2013-02-09 19:23:28',0,0,0,0,0,0,0),(866,'2013-02-09 19:24:28',0,0,0,0,0,0,0),(867,'2013-02-09 19:25:28',0,0,0,0,0,0,0),(868,'2013-02-09 19:27:33',0,0,0,0,0,0,0),(869,'2013-02-09 19:28:33',0,0,0,0,0,0,0),(870,'2013-02-09 19:29:33',0,0,0,0,0,0,0),(871,'2013-02-09 19:30:33',0,0,0,0,0,0,0),(872,'2013-02-09 19:31:33',0,0,0,0,0,0,0),(873,'2013-02-09 19:32:33',0,0,0,0,0,0,0),(874,'2013-02-09 19:33:33',0,0,0,0,0,0,0),(875,'2013-02-09 20:01:14',1,0,0,0,0,0,0),(876,'2013-02-09 20:02:14',1,0,0,0,0,0,0),(877,'2013-02-09 20:05:13',1,0,0,0,0,0,0),(878,'2013-02-09 20:05:13',1,0,0,0,0,0,0),(879,'2013-02-09 20:06:56',1,0,0,0,0,0,0),(880,'2013-02-09 20:07:56',1,0,0,0,0,0,0),(881,'2013-02-09 20:10:18',1,0,0,0,0,0,0),(882,'2013-02-09 20:11:18',1,0,0,0,0,0,0),(883,'2013-02-09 20:12:18',1,0,0,0,0,0,0),(884,'2013-02-09 20:13:18',1,0,0,0,0,0,0),(885,'2013-02-09 20:14:18',1,0,0,0,0,0,0),(886,'2013-02-09 20:15:18',1,0,0,0,0,0,0),(887,'2013-02-09 20:18:39',1,0,0,0,0,0,0),(888,'2013-02-09 20:19:42',0,0,0,0,0,0,0),(889,'2013-02-09 20:21:30',1,0,0,0,0,0,0),(890,'2013-02-09 20:26:03',0,0,0,0,0,0,0),(891,'2013-02-09 20:27:03',0,0,0,0,0,0,0),(892,'2013-02-09 20:28:03',0,0,0,0,0,0,0),(893,'2013-02-09 20:29:03',1,0,0,0,0,0,0),(894,'2013-02-09 20:30:03',1,0,0,0,0,0,0),(895,'2013-02-09 20:31:03',1,0,0,0,0,0,0),(896,'2013-02-09 20:32:03',1,0,0,0,0,0,0),(897,'2013-02-09 20:33:03',1,0,0,0,0,0,0),(898,'2013-02-09 20:37:00',1,0,0,0,0,0,0),(899,'2013-02-09 21:06:51',1,0,0,0,0,0,0),(900,'2013-02-09 21:08:22',0,0,0,0,0,0,0),(901,'2013-02-09 21:08:51',0,0,0,0,0,0,0),(902,'2013-02-09 21:11:24',1,0,0,0,0,0,0),(903,'2013-02-09 21:28:05',0,0,0,0,0,0,0),(904,'2013-02-09 21:29:05',0,0,0,0,0,0,0),(905,'2013-02-09 21:30:05',0,0,0,0,0,0,0),(906,'2013-02-09 21:31:05',0,0,0,0,0,0,0),(907,'2013-02-09 21:32:05',0,0,0,0,0,0,0),(908,'2013-02-09 21:33:05',0,0,0,0,0,0,0),(909,'2013-02-09 21:34:05',0,0,0,0,0,0,0),(910,'2013-02-09 21:35:05',0,0,0,0,0,0,0),(911,'2013-02-09 21:36:05',0,0,0,0,0,0,0),(912,'2013-02-09 21:37:54',0,0,0,0,0,0,0),(913,'2013-02-09 21:38:54',0,0,0,0,0,0,0),(914,'2013-02-10 00:20:33',1,0,0,0,0,0,0),(915,'2013-02-10 00:21:33',1,0,0,0,0,0,0),(916,'2013-02-10 00:23:14',1,0,0,0,0,0,0),(917,'2013-02-10 00:25:45',0,0,0,0,0,0,0),(918,'2013-02-10 00:28:39',1,0,0,0,0,0,0),(919,'2013-02-10 00:29:39',1,0,0,0,0,0,0),(920,'2013-02-10 00:31:06',1,0,0,0,0,0,0),(921,'2013-02-10 00:32:18',0,0,0,0,0,0,0),(922,'2013-02-10 00:33:18',0,0,0,0,0,0,0),(923,'2013-02-10 00:34:18',0,0,0,0,0,0,0),(924,'2013-02-10 00:35:18',0,0,0,0,0,0,0),(925,'2013-02-10 00:36:18',0,0,0,0,0,0,0),(926,'2013-02-10 00:37:18',0,0,0,0,0,0,0),(927,'2013-02-10 00:38:18',0,0,0,0,0,0,0),(928,'2013-02-10 00:39:18',0,0,0,0,0,0,0),(929,'2013-02-10 00:40:18',0,0,0,0,0,0,0),(930,'2013-02-10 00:54:44',0,0,0,0,0,0,0),(931,'2013-02-10 01:06:12',0,0,0,0,0,0,0),(932,'2013-02-10 01:07:12',0,0,0,0,0,0,0),(933,'2013-02-10 01:08:12',0,0,0,0,0,0,0),(934,'2013-02-10 01:09:12',0,0,0,0,0,0,0),(935,'2013-02-10 01:10:12',0,0,0,0,0,0,0),(936,'2013-02-10 01:11:12',0,0,0,0,0,0,0),(937,'2013-02-10 01:12:12',0,0,0,0,0,0,0),(938,'2013-02-10 01:13:12',0,0,0,0,0,0,0),(939,'2013-02-10 01:14:12',0,0,0,0,0,0,0),(940,'2013-02-10 01:15:12',0,0,0,0,0,0,0),(941,'2013-02-10 01:16:12',0,0,0,0,0,0,0),(942,'2013-02-10 01:17:12',0,0,0,0,0,0,0),(943,'2013-02-10 01:18:12',0,0,0,0,0,0,0),(944,'2013-02-10 01:19:12',0,0,0,0,0,0,0),(945,'2013-02-10 01:20:42',0,0,0,0,0,0,0),(946,'2013-02-10 01:22:05',0,0,0,0,0,0,0),(947,'2013-02-10 01:32:51',1,0,0,0,0,0,0),(948,'2013-02-10 01:41:22',0,0,0,0,0,0,0),(949,'2013-02-10 01:42:22',0,0,0,0,0,0,0),(950,'2013-02-10 01:43:22',0,0,0,0,0,0,0),(951,'2013-02-10 01:44:22',0,0,0,0,0,0,0),(952,'2013-02-10 01:51:49',1,0,0,0,0,0,0),(953,'2013-02-10 01:52:49',1,0,0,0,0,0,0),(954,'2013-02-10 01:53:49',1,0,0,0,0,0,0),(955,'2013-02-10 01:55:05',1,0,0,0,0,0,0),(956,'2013-02-10 01:59:03',1,0,0,0,0,0,0),(957,'2013-02-10 02:00:03',1,0,0,0,0,0,0),(958,'2013-02-10 02:16:54',1,0,0,0,0,0,0),(959,'2013-02-10 02:18:20',1,0,0,0,0,0,0),(960,'2013-02-10 02:21:59',1,0,0,0,0,0,0),(961,'2013-02-10 02:25:39',1,0,0,0,0,0,0),(962,'2013-02-10 02:52:24',1,0,0,0,0,0,0),(963,'2013-02-10 02:54:39',1,0,0,0,0,0,0),(964,'2013-02-10 02:55:39',1,0,0,0,0,0,0),(965,'2013-02-10 02:59:20',1,0,0,0,0,0,0),(966,'2013-02-10 03:00:20',1,0,0,0,0,0,0),(967,'2013-02-10 03:01:20',1,0,0,0,0,0,0),(968,'2013-02-10 03:04:34',1,0,0,0,0,0,0),(969,'2013-02-10 03:14:01',1,0,0,0,0,0,0),(970,'2013-02-10 03:16:34',1,0,0,0,0,0,0),(971,'2013-02-10 03:18:24',1,0,0,0,0,0,0),(972,'2013-02-10 03:19:24',1,0,0,0,0,0,0),(973,'2013-02-10 03:20:24',1,0,0,0,0,0,0),(974,'2013-02-10 03:21:24',1,0,0,0,0,0,0),(975,'2013-02-10 03:22:51',1,0,0,0,0,0,0),(976,'2013-02-10 03:25:42',1,0,0,0,0,0,0),(977,'2013-02-10 03:28:48',1,0,0,0,0,0,0),(978,'2013-02-10 03:29:48',1,0,0,0,0,0,0),(979,'2013-02-10 03:30:48',1,0,0,0,0,0,0),(980,'2013-02-10 03:33:28',1,0,0,0,0,0,0),(981,'2013-02-10 03:45:34',1,0,0,0,0,0,0),(982,'2013-02-10 03:47:35',1,0,0,0,0,0,0),(983,'2013-02-10 03:59:29',1,0,0,0,0,0,0),(984,'2013-02-10 04:07:34',1,0,0,0,0,0,0),(985,'2013-02-10 04:08:34',1,0,0,0,0,0,0),(986,'2013-02-10 04:09:34',1,0,0,0,0,0,0),(987,'2013-02-10 04:12:50',1,0,0,0,0,0,0),(988,'2013-02-10 04:15:15',1,0,0,0,0,0,0),(989,'2013-02-10 04:16:15',1,0,0,0,0,0,0),(990,'2013-02-10 04:17:15',1,0,0,0,0,0,0),(991,'2013-02-10 04:27:20',1,0,0,0,0,0,0),(992,'2013-02-10 04:30:23',1,0,0,0,0,0,0),(993,'2013-02-10 04:31:23',1,0,0,0,0,0,0),(994,'2013-02-10 04:32:23',1,0,0,0,0,0,0),(995,'2013-02-10 04:33:23',1,0,0,0,0,0,0),(996,'2013-02-10 04:34:23',1,0,0,0,0,0,0),(997,'2013-02-10 04:35:23',1,0,0,0,0,0,0),(998,'2013-02-10 04:36:23',1,0,0,0,0,0,0),(999,'2013-02-10 04:37:23',1,0,0,0,0,0,0),(1000,'2013-02-10 04:38:23',1,0,0,0,0,0,0),(1001,'2013-02-10 04:39:23',1,0,0,0,0,0,0),(1002,'2013-02-10 04:40:23',1,0,0,0,0,0,0),(1003,'2013-02-10 04:41:23',1,0,0,0,0,0,0),(1004,'2013-02-10 04:42:23',1,0,0,0,0,0,0),(1005,'2013-02-10 04:43:23',1,0,0,0,0,0,0),(1006,'2013-02-10 04:44:23',1,0,0,0,0,0,0),(1007,'2013-02-10 04:45:23',1,0,0,0,0,0,0),(1008,'2013-02-10 04:46:23',1,0,0,0,0,0,0),(1009,'2013-02-10 04:47:23',1,0,0,0,0,0,0),(1010,'2013-02-10 04:48:23',1,0,0,0,0,0,0),(1011,'2013-02-10 04:49:23',1,0,0,0,0,0,0),(1012,'2013-02-10 04:50:23',1,0,0,0,0,0,0),(1013,'2013-02-10 04:51:23',1,0,0,0,0,0,0),(1014,'2013-02-10 04:52:23',1,0,0,0,0,0,0),(1015,'2013-02-10 04:53:23',1,0,0,0,0,0,0),(1016,'2013-02-10 04:54:23',1,0,0,0,0,0,0),(1017,'2013-02-10 04:55:23',1,0,0,0,0,0,0),(1018,'2013-02-10 04:56:23',1,0,0,0,0,0,0),(1019,'2013-02-10 04:57:23',1,0,0,0,0,0,0),(1020,'2013-02-10 04:58:23',1,0,0,0,0,0,0),(1021,'2013-02-10 04:59:23',1,0,0,0,0,0,0),(1022,'2013-02-10 05:00:23',1,0,0,0,0,0,0),(1023,'2013-02-10 05:01:23',1,0,0,0,0,0,0),(1024,'2013-02-10 05:02:23',1,0,0,0,0,0,0),(1025,'2013-02-10 05:04:09',1,0,0,0,0,0,0),(1026,'2013-02-10 05:05:09',1,0,0,0,0,0,0),(1027,'2013-02-10 05:06:09',1,0,0,0,0,0,0),(1028,'2013-02-10 05:07:24',1,0,0,0,0,0,0),(1029,'2013-02-10 05:09:29',1,0,0,0,0,0,0),(1030,'2013-02-10 05:12:08',1,0,0,0,0,0,0),(1031,'2013-02-10 05:13:08',1,0,0,0,0,0,0),(1032,'2013-02-10 05:14:08',1,0,0,0,0,0,0),(1033,'2013-02-10 05:15:08',1,0,0,0,0,0,0),(1034,'2013-02-10 05:16:08',1,0,0,0,0,0,0),(1035,'2013-02-10 05:17:08',1,0,0,0,0,0,0),(1036,'2013-02-10 05:18:08',1,0,0,0,0,0,0),(1037,'2013-02-10 05:19:08',1,0,0,0,0,0,0),(1038,'2013-02-10 05:20:08',1,0,0,0,0,0,0),(1039,'2013-02-10 05:21:08',1,0,0,0,0,0,0),(1040,'2013-02-10 05:22:08',1,0,0,0,0,0,0),(1041,'2013-02-10 05:23:08',1,0,0,0,0,0,0),(1042,'2013-02-10 05:24:08',1,0,0,0,0,0,0),(1043,'2013-02-10 05:25:08',1,0,0,0,0,0,0),(1044,'2013-02-10 05:26:08',1,0,0,0,0,0,0),(1045,'2013-02-10 05:27:08',1,0,0,0,0,0,0),(1046,'2013-02-10 05:28:08',1,0,0,0,0,0,0),(1047,'2013-02-10 05:29:08',1,0,0,0,0,0,0),(1048,'2013-02-10 05:30:08',1,0,0,0,0,0,0),(1049,'2013-02-10 05:31:08',1,0,0,0,0,0,0),(1050,'2013-02-10 05:32:52',0,0,0,0,0,0,0),(1051,'2013-02-10 05:34:35',1,0,0,0,0,0,0),(1052,'2013-02-10 05:35:35',1,0,0,0,0,0,0),(1053,'2013-02-10 05:36:35',1,0,0,0,0,0,0),(1054,'2013-02-10 05:37:35',1,0,0,0,0,0,0),(1055,'2013-02-10 05:38:35',1,0,0,0,0,0,0),(1056,'2013-02-10 05:39:35',1,0,0,0,0,0,0),(1057,'2013-02-10 05:42:08',1,0,0,0,0,0,0),(1058,'2013-02-10 05:43:08',1,0,0,0,0,0,0),(1059,'2013-02-10 05:44:08',1,0,0,0,0,0,0),(1060,'2013-02-10 05:45:08',1,0,0,0,0,0,0),(1061,'2013-02-10 05:46:08',1,0,0,0,0,0,0),(1062,'2013-02-10 05:47:08',1,0,0,0,0,0,0),(1063,'2013-02-10 05:48:08',1,0,0,0,0,0,0),(1064,'2013-02-10 05:49:08',1,0,0,0,0,0,0),(1065,'2013-02-10 05:50:08',1,0,0,0,0,0,0),(1066,'2013-02-10 05:51:08',1,0,0,0,0,0,0),(1067,'2013-02-10 05:52:08',1,0,0,0,0,0,0),(1068,'2013-02-10 05:53:08',1,0,0,0,0,0,0),(1069,'2013-02-10 05:54:08',1,0,0,0,0,0,0),(1070,'2013-02-10 05:55:08',1,0,0,0,0,0,0),(1071,'2013-02-10 05:56:08',1,0,0,0,0,0,0),(1072,'2013-02-10 06:07:25',1,0,0,0,0,0,0),(1073,'2013-02-10 06:08:25',1,0,0,0,0,0,0),(1074,'2013-02-10 06:09:25',1,0,0,0,0,0,0),(1075,'2013-02-10 06:13:36',1,0,0,0,0,0,0),(1076,'2013-02-10 06:14:36',1,0,0,0,0,0,0),(1077,'2013-02-10 06:15:36',1,0,0,0,0,0,0),(1078,'2013-02-10 06:16:36',1,0,0,0,0,0,0),(1079,'2013-02-10 06:17:36',1,0,0,0,0,0,0),(1080,'2013-02-10 06:20:33',1,0,0,0,0,0,0),(1081,'2013-02-10 06:21:33',1,0,0,0,0,0,0),(1082,'2013-02-10 06:27:38',1,0,0,0,0,0,0),(1083,'2013-02-10 06:28:38',1,0,0,0,0,0,0),(1084,'2013-02-10 06:29:38',1,0,0,0,0,0,0),(1085,'2013-02-10 06:31:50',1,0,0,0,0,0,0),(1086,'2013-02-10 06:32:50',1,0,0,0,0,0,0),(1087,'2013-02-10 06:33:50',1,0,0,0,0,0,0),(1088,'2013-02-10 06:34:50',1,0,0,0,0,0,0),(1089,'2013-02-10 06:35:50',1,0,0,0,0,0,0),(1090,'2013-02-10 06:36:50',1,0,0,0,0,0,0),(1091,'2013-02-10 06:37:50',1,0,0,0,0,0,0),(1092,'2013-02-10 06:38:50',1,0,0,0,0,0,0),(1093,'2013-02-10 06:39:50',1,0,0,0,0,0,0),(1094,'2013-02-10 06:40:50',1,0,0,0,0,0,0),(1095,'2013-02-10 06:41:50',1,0,0,0,0,0,0),(1096,'2013-02-10 06:42:50',1,0,0,0,0,0,0),(1097,'2013-02-10 06:43:50',1,0,0,0,0,0,0),(1098,'2013-02-10 06:45:48',0,0,0,0,0,0,0),(1099,'2013-02-10 06:46:48',0,0,0,0,0,0,0),(1100,'2013-02-10 06:47:48',0,0,0,0,0,0,0),(1101,'2013-02-10 06:48:48',0,0,0,0,0,0,0),(1102,'2013-02-10 06:54:47',0,0,0,0,0,0,0),(1103,'2013-02-10 06:59:35',0,0,0,0,0,0,0),(1104,'2013-02-10 07:00:35',0,0,0,0,0,0,0),(1105,'2013-02-10 07:01:35',0,0,0,0,0,0,0),(1106,'2013-02-10 07:02:35',0,0,0,0,0,0,0),(1107,'2013-02-10 07:03:35',0,0,0,0,0,0,0),(1108,'2013-02-10 07:04:35',0,0,0,0,0,0,0),(1109,'2013-02-10 07:05:35',0,0,0,0,0,0,0),(1110,'2013-02-10 07:06:35',1,0,0,0,0,0,0),(1111,'2013-02-10 07:07:35',1,0,0,0,0,0,0),(1112,'2013-02-10 07:08:35',1,0,0,0,0,0,0),(1113,'2013-02-10 07:09:35',1,0,0,0,0,0,0),(1114,'2013-02-10 07:10:35',1,0,0,0,0,0,0),(1115,'2013-02-10 07:11:35',1,0,0,0,0,0,0),(1116,'2013-02-10 07:12:35',1,0,0,0,0,0,0),(1117,'2013-02-10 07:13:35',1,0,0,0,0,0,0),(1118,'2013-02-10 07:14:35',1,0,0,0,0,0,0),(1119,'2013-02-10 07:15:35',1,0,0,0,0,0,0),(1120,'2013-02-10 07:16:35',1,0,0,0,0,0,0),(1121,'2013-02-10 07:17:35',1,0,0,0,0,0,0),(1122,'2013-02-10 07:18:35',1,0,0,0,0,0,0),(1123,'2013-02-10 07:19:35',1,0,0,0,0,0,0),(1124,'2013-02-10 07:20:35',1,0,0,0,0,0,0),(1125,'2013-02-10 07:21:35',1,0,0,0,0,0,0),(1126,'2013-02-10 07:22:35',1,0,0,0,0,0,0),(1127,'2013-02-10 07:23:35',1,0,0,0,0,0,0),(1128,'2013-02-10 07:24:35',1,0,0,0,0,0,0),(1129,'2013-02-10 07:25:35',1,0,0,0,0,0,0),(1130,'2013-02-10 07:26:35',1,0,0,0,0,0,0),(1131,'2013-02-10 07:27:35',1,0,0,0,0,0,0),(1132,'2013-02-10 07:28:35',1,0,0,0,0,0,0),(1133,'2013-02-10 07:29:35',1,0,0,0,0,0,0),(1134,'2013-02-10 07:30:35',1,0,0,0,0,0,0),(1135,'2013-02-10 07:31:35',1,0,0,0,0,0,0),(1136,'2013-02-10 07:32:35',1,0,0,0,0,0,0),(1137,'2013-02-10 07:33:35',1,0,0,0,0,0,0),(1138,'2013-02-10 07:34:35',1,0,0,0,0,0,0),(1139,'2013-02-10 07:35:35',1,0,0,0,0,0,0),(1140,'2013-02-10 07:36:35',1,0,0,0,0,0,0),(1141,'2013-02-10 07:37:35',1,0,0,0,0,0,0),(1142,'2013-02-10 07:38:35',1,0,0,0,0,0,0),(1143,'2013-02-10 07:39:35',1,0,0,0,0,0,0),(1144,'2013-02-10 07:40:35',1,0,0,0,0,0,0),(1145,'2013-02-10 07:41:35',1,0,0,0,0,0,0),(1146,'2013-02-10 07:42:35',1,0,0,0,0,0,0),(1147,'2013-02-10 07:43:35',1,0,0,0,0,0,0),(1148,'2013-02-10 07:44:35',1,0,0,0,0,0,0),(1149,'2013-02-10 07:45:35',1,0,0,0,0,0,0),(1150,'2013-02-10 07:46:35',1,0,0,0,0,0,0),(1151,'2013-02-10 07:47:35',1,0,0,0,0,0,0),(1152,'2013-02-10 07:48:35',1,0,0,0,0,0,0),(1153,'2013-02-10 07:49:35',1,0,0,0,0,0,0),(1154,'2013-02-10 07:50:35',1,0,0,0,0,0,0),(1155,'2013-02-10 07:51:35',1,0,0,0,0,0,0),(1156,'2013-02-10 07:52:35',1,0,0,0,0,0,0),(1157,'2013-02-10 07:53:35',1,0,0,0,0,0,0),(1158,'2013-02-10 07:54:35',1,0,0,0,0,0,0),(1159,'2013-02-10 07:55:35',1,0,0,0,0,0,0),(1160,'2013-02-10 07:56:35',1,0,0,0,0,0,0),(1161,'2013-02-10 07:57:35',1,0,0,0,0,0,0),(1162,'2013-02-10 07:58:35',1,0,0,0,0,0,0),(1163,'2013-02-10 07:59:35',1,0,0,0,0,0,0),(1164,'2013-02-10 08:00:35',1,0,0,0,0,0,0),(1165,'2013-02-10 08:01:35',1,0,0,0,0,0,0),(1166,'2013-02-10 08:02:35',1,0,0,0,0,0,0),(1167,'2013-02-10 08:03:35',1,0,0,0,0,0,0),(1168,'2013-02-10 08:04:35',1,0,0,0,0,0,0),(1169,'2013-02-10 08:05:35',1,0,0,0,0,0,0),(1170,'2013-02-10 08:06:35',1,0,0,0,0,0,0),(1171,'2013-02-10 08:07:35',1,0,0,0,0,0,0),(1172,'2013-02-10 08:08:35',1,0,0,0,0,0,0),(1173,'2013-02-10 08:09:35',1,0,0,0,0,0,0),(1174,'2013-02-10 08:10:35',1,0,0,0,0,0,0),(1175,'2013-02-10 08:11:35',1,0,0,0,0,0,0),(1176,'2013-02-10 08:12:35',1,0,0,0,0,0,0),(1177,'2013-02-10 08:13:35',1,0,0,0,0,0,0),(1178,'2013-02-10 08:14:35',1,0,0,0,0,0,0),(1179,'2013-02-10 08:15:35',1,0,0,0,0,0,0),(1180,'2013-02-10 08:16:35',1,0,0,0,0,0,0),(1181,'2013-02-10 08:17:35',1,0,0,0,0,0,0),(1182,'2013-02-10 08:18:35',1,0,0,0,0,0,0),(1183,'2013-02-10 08:19:35',1,0,0,0,0,0,0),(1184,'2013-02-10 08:20:35',1,0,0,0,0,0,0),(1185,'2013-02-10 08:21:35',1,0,0,0,0,0,0),(1186,'2013-02-10 08:22:35',1,0,0,0,0,0,0),(1187,'2013-02-10 08:23:35',1,0,0,0,0,0,0),(1188,'2013-02-10 08:24:35',1,0,0,0,0,0,0),(1189,'2013-02-10 08:25:35',1,0,0,0,0,0,0),(1190,'2013-02-10 08:26:35',1,0,0,0,0,0,0),(1191,'2013-02-10 08:27:35',1,0,0,0,0,0,0),(1192,'2013-02-10 08:28:35',1,0,0,0,0,0,0),(1193,'2013-02-10 08:29:35',1,0,0,0,0,0,0),(1194,'2013-02-10 08:30:35',1,0,0,0,0,0,0),(1195,'2013-02-10 08:31:35',1,0,0,0,0,0,0),(1196,'2013-02-10 08:32:35',1,0,0,0,0,0,0),(1197,'2013-02-10 08:33:35',1,0,0,0,0,0,0),(1198,'2013-02-10 08:34:35',1,0,0,0,0,0,0),(1199,'2013-02-10 08:35:35',1,0,0,0,0,0,0),(1200,'2013-02-10 08:36:35',1,0,0,0,0,0,0),(1201,'2013-02-10 08:37:35',1,0,0,0,0,0,0),(1202,'2013-02-10 08:38:35',1,0,0,0,0,0,0),(1203,'2013-02-10 08:39:35',1,0,0,0,0,0,0),(1204,'2013-02-10 08:40:35',1,0,0,0,0,0,0),(1205,'2013-02-10 08:41:35',1,0,0,0,0,0,0),(1206,'2013-02-10 08:42:35',1,0,0,0,0,0,0),(1207,'2013-02-10 08:43:35',1,0,0,0,0,0,0),(1208,'2013-02-10 08:44:35',1,0,0,0,0,0,0),(1209,'2013-02-10 08:45:35',1,0,0,0,0,0,0),(1210,'2013-02-10 08:46:35',1,0,0,0,0,0,0),(1211,'2013-02-10 08:47:35',1,0,0,0,0,0,0),(1212,'2013-02-10 08:48:35',1,0,0,0,0,0,0),(1213,'2013-02-10 08:49:35',1,0,0,0,0,0,0),(1214,'2013-02-10 08:50:35',1,0,0,0,0,0,0),(1215,'2013-02-10 08:51:35',1,0,0,0,0,0,0),(1216,'2013-02-10 08:52:35',1,0,0,0,0,0,0),(1217,'2013-02-10 08:53:35',1,0,0,0,0,0,0),(1218,'2013-02-10 08:54:35',1,0,0,0,0,0,0),(1219,'2013-02-10 08:55:35',1,0,0,0,0,0,0),(1220,'2013-02-10 08:56:35',1,0,0,0,0,0,0),(1221,'2013-02-10 08:57:35',1,0,0,0,0,0,0),(1222,'2013-02-10 08:58:35',1,0,0,0,0,0,0),(1223,'2013-02-10 08:59:35',1,0,0,0,0,0,0),(1224,'2013-02-10 09:00:35',1,0,0,0,0,0,0),(1225,'2013-02-10 09:01:35',1,0,0,0,0,0,0),(1226,'2013-02-10 09:02:35',1,0,0,0,0,0,0),(1227,'2013-02-10 09:03:35',1,0,0,0,0,0,0),(1228,'2013-02-10 09:04:35',1,0,0,0,0,0,0),(1229,'2013-02-10 09:05:35',1,0,0,0,0,0,0),(1230,'2013-02-10 09:06:35',1,0,0,0,0,0,0),(1231,'2013-02-10 09:07:35',1,0,0,0,0,0,0),(1232,'2013-02-10 09:08:35',1,0,0,0,0,0,0),(1233,'2013-02-10 09:09:35',1,0,0,0,0,0,0),(1234,'2013-02-10 09:10:35',1,0,0,0,0,0,0),(1235,'2013-02-10 09:11:35',1,0,0,0,0,0,0),(1236,'2013-02-10 09:12:35',1,0,0,0,0,0,0),(1237,'2013-02-10 09:13:35',1,0,0,0,0,0,0),(1238,'2013-02-10 09:14:35',1,0,0,0,0,0,0),(1239,'2013-02-10 09:15:35',1,0,0,0,0,0,0),(1240,'2013-02-10 09:16:35',1,0,0,0,0,0,0),(1241,'2013-02-10 09:17:35',1,0,0,0,0,0,0),(1242,'2013-02-10 09:18:35',1,0,0,0,0,0,0),(1243,'2013-02-10 09:19:35',1,0,0,0,0,0,0),(1244,'2013-02-10 09:20:35',1,0,0,0,0,0,0),(1245,'2013-02-10 09:21:35',1,0,0,0,0,0,0),(1246,'2013-02-10 09:22:35',1,0,0,0,0,0,0),(1247,'2013-02-10 09:23:35',1,0,0,0,0,0,0),(1248,'2013-02-10 09:24:35',1,0,0,0,0,0,0),(1249,'2013-02-10 09:25:35',1,0,0,0,0,0,0),(1250,'2013-02-10 09:26:35',1,0,0,0,0,0,0),(1251,'2013-02-10 09:27:35',1,0,0,0,0,0,0),(1252,'2013-02-10 09:28:35',1,0,0,0,0,0,0),(1253,'2013-02-10 09:29:35',1,0,0,0,0,0,0),(1254,'2013-02-10 09:30:35',1,0,0,0,0,0,0),(1255,'2013-02-10 09:31:35',1,0,0,0,0,0,0),(1256,'2013-02-10 09:32:35',1,0,0,0,0,0,0),(1257,'2013-02-10 09:33:35',1,0,0,0,0,0,0),(1258,'2013-02-10 09:34:35',1,0,0,0,0,0,0),(1259,'2013-02-10 09:35:35',1,0,0,0,0,0,0),(1260,'2013-02-10 09:36:35',1,0,0,0,0,0,0),(1261,'2013-02-10 09:37:35',1,0,0,0,0,0,0),(1262,'2013-02-10 09:38:35',1,0,0,0,0,0,0),(1263,'2013-02-10 09:39:35',1,0,0,0,0,0,0),(1264,'2013-02-10 09:40:35',1,0,0,0,0,0,0),(1265,'2013-02-10 09:41:35',1,0,0,0,0,0,0),(1266,'2013-02-10 09:42:35',1,0,0,0,0,0,0),(1267,'2013-02-10 09:43:35',1,0,0,0,0,0,0),(1268,'2013-02-10 09:44:35',1,0,0,0,0,0,0),(1269,'2013-02-10 09:45:35',1,0,0,0,0,0,0),(1270,'2013-02-10 09:46:35',1,0,0,0,0,0,0),(1271,'2013-02-10 09:47:35',1,0,0,0,0,0,0),(1272,'2013-02-10 09:48:35',1,0,0,0,0,0,0),(1273,'2013-02-10 09:49:35',1,0,0,0,0,0,0),(1274,'2013-02-10 09:50:35',1,0,0,0,0,0,0),(1275,'2013-02-10 09:51:35',1,0,0,0,0,0,0),(1276,'2013-02-10 09:52:35',1,0,0,0,0,0,0),(1277,'2013-02-10 09:53:35',1,0,0,0,0,0,0),(1278,'2013-02-10 09:54:35',1,0,0,0,0,0,0),(1279,'2013-02-10 09:55:35',1,0,0,0,0,0,0),(1280,'2013-02-10 09:56:35',1,0,0,0,0,0,0),(1281,'2013-02-10 09:57:35',1,0,0,0,0,0,0),(1282,'2013-02-10 09:58:35',1,0,0,0,0,0,0),(1283,'2013-02-10 09:59:35',1,0,0,0,0,0,0),(1284,'2013-02-10 10:00:35',1,0,0,0,0,0,0),(1285,'2013-02-10 10:01:35',1,0,0,0,0,0,0),(1286,'2013-02-10 10:02:35',1,0,0,0,0,0,0),(1287,'2013-02-10 10:03:35',1,0,0,0,0,0,0),(1288,'2013-02-10 10:04:35',1,0,0,0,0,0,0),(1289,'2013-02-10 10:05:35',1,0,0,0,0,0,0),(1290,'2013-02-10 10:06:35',1,0,0,0,0,0,0),(1291,'2013-02-10 10:07:35',1,0,0,0,0,0,0),(1292,'2013-02-10 10:08:35',1,0,0,0,0,0,0),(1293,'2013-02-10 10:09:35',1,0,0,0,0,0,0),(1294,'2013-02-10 10:10:35',1,0,0,0,0,0,0),(1295,'2013-02-10 10:11:35',1,0,0,0,0,0,0),(1296,'2013-02-10 10:12:35',1,0,0,0,0,0,0),(1297,'2013-02-10 10:13:35',1,0,0,0,0,0,0),(1298,'2013-02-10 10:14:35',1,0,0,0,0,0,0),(1299,'2013-02-10 10:15:35',1,0,0,0,0,0,0),(1300,'2013-02-10 10:16:35',1,0,0,0,0,0,0),(1301,'2013-02-10 10:17:35',1,0,0,0,0,0,0),(1302,'2013-02-10 10:18:35',1,0,0,0,0,0,0),(1303,'2013-02-10 10:19:35',1,0,0,0,0,0,0),(1304,'2013-02-10 10:20:35',1,0,0,0,0,0,0),(1305,'2013-02-10 10:21:35',1,0,0,0,0,0,0),(1306,'2013-02-10 10:22:35',1,0,0,0,0,0,0),(1307,'2013-02-10 10:23:35',1,0,0,0,0,0,0),(1308,'2013-02-10 10:24:35',1,0,0,0,0,0,0),(1309,'2013-02-10 10:25:35',1,0,0,0,0,0,0),(1310,'2013-02-10 10:26:35',1,0,0,0,0,0,0),(1311,'2013-02-10 10:27:35',1,0,0,0,0,0,0),(1312,'2013-02-10 10:28:35',1,0,0,0,0,0,0),(1313,'2013-02-10 10:29:35',1,0,0,0,0,0,0),(1314,'2013-02-10 10:30:35',1,0,0,0,0,0,0),(1315,'2013-02-10 10:31:35',1,0,0,0,0,0,0),(1316,'2013-02-10 10:32:35',1,0,0,0,0,0,0),(1317,'2013-02-10 10:33:35',1,0,0,0,0,0,0),(1318,'2013-02-10 10:34:35',1,0,0,0,0,0,0),(1319,'2013-02-10 10:35:35',1,0,0,0,0,0,0),(1320,'2013-02-10 10:36:35',1,0,0,0,0,0,0),(1321,'2013-02-10 10:37:35',1,0,0,0,0,0,0),(1322,'2013-02-10 10:38:35',1,0,0,0,0,0,0),(1323,'2013-02-10 10:39:35',1,0,0,0,0,0,0),(1324,'2013-02-10 10:40:35',1,0,0,0,0,0,0),(1325,'2013-02-10 10:41:35',1,0,0,0,0,0,0),(1326,'2013-02-10 10:42:35',1,0,0,0,0,0,0),(1327,'2013-02-10 10:43:35',1,0,0,0,0,0,0),(1328,'2013-02-10 10:44:35',1,0,0,0,0,0,0),(1329,'2013-02-10 10:45:35',1,0,0,0,0,0,0),(1330,'2013-02-10 10:46:35',1,0,0,0,0,0,0),(1331,'2013-02-10 10:47:35',1,0,0,0,0,0,0),(1332,'2013-02-10 10:48:35',1,0,0,0,0,0,0),(1333,'2013-02-10 10:49:35',1,0,0,0,0,0,0),(1334,'2013-02-10 10:50:35',1,0,0,0,0,0,0),(1335,'2013-02-10 10:51:35',1,0,0,0,0,0,0),(1336,'2013-02-10 10:52:35',1,0,0,0,0,0,0),(1337,'2013-02-10 10:53:35',1,0,0,0,0,0,0),(1338,'2013-02-10 10:54:35',1,0,0,0,0,0,0),(1339,'2013-02-10 10:55:35',1,0,0,0,0,0,0),(1340,'2013-02-10 10:56:35',1,0,0,0,0,0,0),(1341,'2013-02-10 10:57:35',1,0,0,0,0,0,0),(1342,'2013-02-10 10:58:35',1,0,0,0,0,0,0),(1343,'2013-02-10 10:59:35',1,0,0,0,0,0,0),(1344,'2013-02-10 11:00:35',1,0,0,0,0,0,0),(1345,'2013-02-10 11:01:35',1,0,0,0,0,0,0),(1346,'2013-02-10 11:02:35',1,0,0,0,0,0,0),(1347,'2013-02-10 11:03:35',1,0,0,0,0,0,0),(1348,'2013-02-10 11:04:35',1,0,0,0,0,0,0),(1349,'2013-02-10 11:05:35',1,0,0,0,0,0,0),(1350,'2013-02-10 11:06:35',1,0,0,0,0,0,0),(1351,'2013-02-10 11:07:35',1,0,0,0,0,0,0),(1352,'2013-02-10 11:08:35',1,0,0,0,0,0,0),(1353,'2013-02-10 11:09:35',1,0,0,0,0,0,0),(1354,'2013-02-10 11:10:35',1,0,0,0,0,0,0),(1355,'2013-02-10 11:11:35',1,0,0,0,0,0,0),(1356,'2013-02-10 11:12:35',1,0,0,0,0,0,0),(1357,'2013-02-10 11:13:35',1,0,0,0,0,0,0),(1358,'2013-02-10 11:14:35',1,0,0,0,0,0,0),(1359,'2013-02-10 11:15:35',1,0,0,0,0,0,0),(1360,'2013-02-10 11:16:35',1,0,0,0,0,0,0),(1361,'2013-02-10 11:17:35',1,0,0,0,0,0,0),(1362,'2013-02-10 11:18:35',1,0,0,0,0,0,0),(1363,'2013-02-10 11:19:35',1,0,0,0,0,0,0),(1364,'2013-02-10 11:20:35',1,0,0,0,0,0,0),(1365,'2013-02-10 11:21:35',1,0,0,0,0,0,0),(1366,'2013-02-10 11:22:35',1,0,0,0,0,0,0),(1367,'2013-02-10 11:23:35',1,0,0,0,0,0,0),(1368,'2013-02-10 11:24:35',1,0,0,0,0,0,0),(1369,'2013-02-10 11:25:35',1,0,0,0,0,0,0),(1370,'2013-02-10 11:26:35',1,0,0,0,0,0,0),(1371,'2013-02-10 11:27:35',1,0,0,0,0,0,0),(1372,'2013-02-10 11:28:35',1,0,0,0,0,0,0),(1373,'2013-02-10 11:29:35',1,0,0,0,0,0,0),(1374,'2013-02-10 11:30:35',1,0,0,0,0,0,0),(1375,'2013-02-10 11:31:35',1,0,0,0,0,0,0),(1376,'2013-02-10 11:32:35',1,0,0,0,0,0,0),(1377,'2013-02-10 11:33:35',1,0,0,0,0,0,0),(1378,'2013-02-10 11:34:35',1,0,0,0,0,0,0),(1379,'2013-02-10 11:35:35',1,0,0,0,0,0,0),(1380,'2013-02-10 11:36:35',1,0,0,0,0,0,0),(1381,'2013-02-10 11:37:35',1,0,0,0,0,0,0),(1382,'2013-02-10 11:38:35',1,0,0,0,0,0,0),(1383,'2013-02-10 11:39:35',1,0,0,0,0,0,0),(1384,'2013-02-10 11:40:35',1,0,0,0,0,0,0),(1385,'2013-02-10 11:41:35',1,0,0,0,0,0,0),(1386,'2013-02-10 11:42:35',1,0,0,0,0,0,0),(1387,'2013-02-10 11:43:35',1,0,0,0,0,0,0),(1388,'2013-02-10 11:44:35',1,0,0,0,0,0,0),(1389,'2013-02-10 11:45:35',1,0,0,0,0,0,0),(1390,'2013-02-10 11:46:35',1,0,0,0,0,0,0),(1391,'2013-02-10 11:47:35',1,0,0,0,0,0,0),(1392,'2013-02-10 11:48:35',1,0,0,0,0,0,0),(1393,'2013-02-10 11:49:35',1,0,0,0,0,0,0),(1394,'2013-02-10 11:50:35',1,0,0,0,0,0,0),(1395,'2013-02-10 11:51:35',1,0,0,0,0,0,0),(1396,'2013-02-10 11:52:35',1,0,0,0,0,0,0),(1397,'2013-02-10 11:53:35',1,0,0,0,0,0,0),(1398,'2013-02-10 11:54:35',1,0,0,0,0,0,0),(1399,'2013-02-10 11:55:35',1,0,0,0,0,0,0),(1400,'2013-02-10 11:56:35',1,0,0,0,0,0,0),(1401,'2013-02-10 11:57:35',1,0,0,0,0,0,0),(1402,'2013-02-10 11:58:35',1,0,0,0,0,0,0),(1403,'2013-02-10 11:59:35',1,0,0,0,0,0,0),(1404,'2013-02-10 12:00:35',1,0,0,0,0,0,0),(1405,'2013-02-10 12:01:35',1,0,0,0,0,0,0),(1406,'2013-02-10 12:02:35',1,0,0,0,0,0,0),(1407,'2013-02-10 12:03:35',1,0,0,0,0,0,0),(1408,'2013-02-10 12:04:35',1,0,0,0,0,0,0),(1409,'2013-02-10 12:05:35',1,0,0,0,0,0,0),(1410,'2013-02-10 12:06:35',1,0,0,0,0,0,0),(1411,'2013-02-10 12:07:35',1,0,0,0,0,0,0),(1412,'2013-02-10 12:08:35',1,0,0,0,0,0,0),(1413,'2013-02-10 12:09:35',1,0,0,0,0,0,0),(1414,'2013-02-10 12:10:35',1,0,0,0,0,0,0),(1415,'2013-02-10 12:11:35',1,0,0,0,0,0,0),(1416,'2013-02-10 12:12:35',1,0,0,0,0,0,0),(1417,'2013-02-10 12:13:35',1,0,0,0,0,0,0),(1418,'2013-02-10 12:14:35',1,0,0,0,0,0,0),(1419,'2013-02-10 12:15:35',1,0,0,0,0,0,0),(1420,'2013-02-10 12:16:35',1,0,0,0,0,0,0),(1421,'2013-02-10 12:17:35',1,0,0,0,0,0,0),(1422,'2013-02-10 12:18:35',1,0,0,0,0,0,0),(1423,'2013-02-10 12:19:35',1,0,0,0,0,0,0),(1424,'2013-02-10 12:20:35',1,0,0,0,0,0,0),(1425,'2013-02-10 12:21:35',1,0,0,0,0,0,0),(1426,'2013-02-10 12:22:35',1,0,0,0,0,0,0),(1427,'2013-02-10 12:23:35',1,0,0,0,0,0,0),(1428,'2013-02-10 12:24:35',1,0,0,0,0,0,0),(1429,'2013-02-10 12:25:35',1,0,0,0,0,0,0),(1430,'2013-02-10 12:26:35',1,0,0,0,0,0,0),(1431,'2013-02-10 12:27:35',1,0,0,0,0,0,0),(1432,'2013-02-10 12:28:35',1,0,0,0,0,0,0),(1433,'2013-02-10 12:29:35',1,0,0,0,0,0,0),(1434,'2013-02-10 12:30:35',1,0,0,0,0,0,0),(1435,'2013-02-10 12:31:35',1,0,0,0,0,0,0),(1436,'2013-02-10 12:32:35',1,0,0,0,0,0,0),(1437,'2013-02-10 12:33:35',1,0,0,0,0,0,0),(1438,'2013-02-10 12:34:35',1,0,0,0,0,0,0),(1439,'2013-02-10 12:35:35',1,0,0,0,0,0,0),(1440,'2013-02-10 12:36:35',1,0,0,0,0,0,0),(1441,'2013-02-10 12:37:35',1,0,0,0,0,0,0),(1442,'2013-02-10 12:38:35',1,0,0,0,0,0,0),(1443,'2013-02-10 12:39:35',1,0,0,0,0,0,0),(1444,'2013-02-10 12:40:35',1,0,0,0,0,0,0),(1445,'2013-02-10 12:41:35',1,0,0,0,0,0,0),(1446,'2013-02-10 12:42:35',1,0,0,0,0,0,0),(1447,'2013-02-10 12:43:35',1,0,0,0,0,0,0),(1448,'2013-02-10 12:44:35',1,0,0,0,0,0,0),(1449,'2013-02-10 12:45:35',1,0,0,0,0,0,0),(1450,'2013-02-10 12:46:35',1,0,0,0,0,0,0),(1451,'2013-02-10 12:47:35',1,0,0,0,0,0,0),(1452,'2013-02-10 12:48:35',1,0,0,0,0,0,0),(1453,'2013-02-10 12:49:35',1,0,0,0,0,0,0),(1454,'2013-02-10 12:50:35',1,0,0,0,0,0,0),(1455,'2013-02-10 12:51:35',1,0,0,0,0,0,0),(1456,'2013-02-10 12:52:35',1,0,0,0,0,0,0),(1457,'2013-02-10 12:53:35',1,0,0,0,0,0,0),(1458,'2013-02-10 12:54:35',1,0,0,0,0,0,0),(1459,'2013-02-10 12:55:35',1,0,0,0,0,0,0),(1460,'2013-02-10 12:56:35',1,0,0,0,0,0,0),(1461,'2013-02-10 12:57:35',1,0,0,0,0,0,0),(1462,'2013-02-10 12:58:35',1,0,0,0,0,0,0),(1463,'2013-02-10 12:59:35',1,0,0,0,0,0,0),(1464,'2013-02-10 13:00:35',1,0,0,0,0,0,0),(1465,'2013-02-10 13:01:35',1,0,0,0,0,0,0),(1466,'2013-02-10 13:02:35',1,0,0,0,0,0,0),(1467,'2013-02-10 13:03:35',1,0,0,0,0,0,0),(1468,'2013-02-10 13:04:35',1,0,0,0,0,0,0),(1469,'2013-02-10 13:05:35',1,0,0,0,0,0,0),(1470,'2013-02-10 13:06:35',1,0,0,0,0,0,0),(1471,'2013-02-10 13:07:35',1,0,0,0,0,0,0),(1472,'2013-02-10 13:08:35',1,0,0,0,0,0,0),(1473,'2013-02-10 13:09:35',1,0,0,0,0,0,0),(1474,'2013-02-10 13:10:35',1,0,0,0,0,0,0),(1475,'2013-02-10 13:11:35',1,0,0,0,0,0,0),(1476,'2013-02-10 13:12:35',1,0,0,0,0,0,0),(1477,'2013-02-10 13:13:35',1,0,0,0,0,0,0),(1478,'2013-02-10 13:14:35',1,0,0,0,0,0,0),(1479,'2013-02-10 13:15:35',1,0,0,0,0,0,0),(1480,'2013-02-10 13:16:35',1,0,0,0,0,0,0),(1481,'2013-02-10 13:17:35',1,0,0,0,0,0,0),(1482,'2013-02-10 13:18:35',1,0,0,0,0,0,0),(1483,'2013-02-10 13:19:35',1,0,0,0,0,0,0),(1484,'2013-02-10 13:20:35',1,0,0,0,0,0,0),(1485,'2013-02-10 13:21:35',1,0,0,0,0,0,0),(1486,'2013-02-10 13:22:35',1,0,0,0,0,0,0),(1487,'2013-02-10 13:23:35',1,0,0,0,0,0,0),(1488,'2013-02-10 13:24:35',1,0,0,0,0,0,0),(1489,'2013-02-10 13:25:35',1,0,0,0,0,0,0),(1490,'2013-02-10 13:26:35',1,0,0,0,0,0,0),(1491,'2013-02-10 13:27:35',1,0,0,0,0,0,0),(1492,'2013-02-10 13:28:35',1,0,0,0,0,0,0),(1493,'2013-02-10 13:29:35',1,0,0,0,0,0,0),(1494,'2013-02-10 13:30:35',1,0,0,0,0,0,0),(1495,'2013-02-10 13:31:35',1,0,0,0,0,0,0),(1496,'2013-02-10 13:32:35',1,0,0,0,0,0,0),(1497,'2013-02-10 13:33:35',1,0,0,0,0,0,0),(1498,'2013-02-10 13:34:35',1,0,0,0,0,0,0),(1499,'2013-02-10 13:35:35',1,0,0,0,0,0,0),(1500,'2013-02-10 13:36:35',1,0,0,0,0,0,0),(1501,'2013-02-10 13:37:35',1,0,0,0,0,0,0),(1502,'2013-02-10 13:38:35',1,0,0,0,0,0,0),(1503,'2013-02-10 13:39:35',1,0,0,0,0,0,0),(1504,'2013-02-10 13:40:35',1,0,0,0,0,0,0),(1505,'2013-02-10 13:41:35',1,0,0,0,0,0,0),(1506,'2013-02-10 13:42:35',1,0,0,0,0,0,0),(1507,'2013-02-10 13:43:35',1,0,0,0,0,0,0),(1508,'2013-02-10 13:44:35',1,0,0,0,0,0,0),(1509,'2013-02-10 13:45:35',1,0,0,0,0,0,0),(1510,'2013-02-10 13:46:35',1,0,0,0,0,0,0),(1511,'2013-02-10 13:47:35',1,0,0,0,0,0,0),(1512,'2013-02-10 13:48:35',1,0,0,0,0,0,0),(1513,'2013-02-10 13:49:35',1,0,0,0,0,0,0),(1514,'2013-02-10 13:50:35',1,0,0,0,0,0,0),(1515,'2013-02-10 13:51:35',1,0,0,0,0,0,0),(1516,'2013-02-10 13:52:35',1,0,0,0,0,0,0),(1517,'2013-02-10 13:53:35',1,0,0,0,0,0,0),(1518,'2013-02-10 13:54:35',1,0,0,0,0,0,0),(1519,'2013-02-10 13:55:35',1,0,0,0,0,0,0),(1520,'2013-02-10 13:56:35',1,0,0,0,0,0,0),(1521,'2013-02-10 13:57:35',1,0,0,0,0,0,0),(1522,'2013-02-10 13:58:35',1,0,0,0,0,0,0),(1523,'2013-02-10 13:59:35',1,0,0,0,0,0,0),(1524,'2013-02-10 14:00:35',1,0,0,0,0,0,0),(1525,'2013-02-10 14:01:35',1,0,0,0,0,0,0),(1526,'2013-02-10 14:02:35',1,0,0,0,0,0,0),(1527,'2013-02-10 14:03:35',1,0,0,0,0,0,0),(1528,'2013-02-10 14:04:35',1,0,0,0,0,0,0),(1529,'2013-02-10 14:05:35',1,0,0,0,0,0,0),(1530,'2013-02-10 14:06:35',1,0,0,0,0,0,0),(1531,'2013-02-10 14:07:35',1,0,0,0,0,0,0),(1532,'2013-02-10 14:08:35',1,0,0,0,0,0,0),(1533,'2013-02-10 14:09:35',1,0,0,0,0,0,0),(1534,'2013-02-10 14:10:35',1,0,0,0,0,0,0),(1535,'2013-02-10 14:11:35',1,0,0,0,0,0,0),(1536,'2013-02-10 14:12:35',1,0,0,0,0,0,0),(1537,'2013-02-10 14:13:35',1,0,0,0,0,0,0),(1538,'2013-02-10 14:14:35',1,0,0,0,0,0,0),(1539,'2013-02-10 14:15:35',1,0,0,0,0,0,0),(1540,'2013-02-10 14:16:35',1,0,0,0,0,0,0),(1541,'2013-02-10 14:17:35',1,0,0,0,0,0,0),(1542,'2013-02-10 14:18:35',1,0,0,0,0,0,0),(1543,'2013-02-10 14:19:35',1,0,0,0,0,0,0),(1544,'2013-02-10 14:20:35',1,0,0,0,0,0,0),(1545,'2013-02-10 14:21:35',1,0,0,0,0,0,0),(1546,'2013-02-10 14:22:35',1,0,0,0,0,0,0),(1547,'2013-02-10 14:23:35',1,0,0,0,0,0,0),(1548,'2013-02-10 14:24:35',1,0,0,0,0,0,0),(1549,'2013-02-10 14:25:35',1,0,0,0,0,0,0),(1550,'2013-02-10 14:26:35',1,0,0,0,0,0,0),(1551,'2013-02-10 14:27:35',1,0,0,0,0,0,0),(1552,'2013-02-10 14:28:35',1,0,0,0,0,0,0),(1553,'2013-02-10 14:29:35',1,0,0,0,0,0,0),(1554,'2013-02-10 14:30:35',1,0,0,0,0,0,0),(1555,'2013-02-10 14:31:35',1,0,0,0,0,0,0),(1556,'2013-02-10 14:32:35',1,0,0,0,0,0,0),(1557,'2013-02-10 14:33:35',1,0,0,0,0,0,0),(1558,'2013-02-10 14:34:35',1,0,0,0,0,0,0),(1559,'2013-02-10 14:35:35',1,0,0,0,0,0,0),(1560,'2013-02-10 14:36:35',1,0,0,0,0,0,0),(1561,'2013-02-10 14:37:35',1,0,0,0,0,0,0),(1562,'2013-02-10 14:38:35',1,0,0,0,0,0,0),(1563,'2013-02-10 14:39:35',1,0,0,0,0,0,0),(1564,'2013-02-10 14:40:35',1,0,0,0,0,0,0),(1565,'2013-02-10 14:41:35',1,0,0,0,0,0,0),(1566,'2013-02-10 14:42:35',1,0,0,0,0,0,0),(1567,'2013-02-10 14:43:35',1,0,0,0,0,0,0),(1568,'2013-02-10 14:44:35',1,0,0,0,0,0,0),(1569,'2013-02-10 14:45:35',1,0,0,0,0,0,0),(1570,'2013-02-10 14:46:35',1,0,0,0,0,0,0),(1571,'2013-02-10 14:47:35',1,0,0,0,0,0,0),(1572,'2013-02-10 14:48:35',1,0,0,0,0,0,0),(1573,'2013-02-10 14:49:35',1,0,0,0,0,0,0),(1574,'2013-02-10 14:50:35',1,0,0,0,0,0,0),(1575,'2013-02-10 14:51:35',1,0,0,0,0,0,0),(1576,'2013-02-10 14:52:35',1,0,0,0,0,0,0),(1577,'2013-02-10 14:53:35',1,0,0,0,0,0,0),(1578,'2013-02-10 14:54:35',1,0,0,0,0,0,0),(1579,'2013-02-10 14:55:35',1,0,0,0,0,0,0),(1580,'2013-02-10 14:56:35',1,0,0,0,0,0,0),(1581,'2013-02-10 14:57:35',1,0,0,0,0,0,0),(1582,'2013-02-10 14:58:35',1,0,0,0,0,0,0),(1583,'2013-02-10 14:59:35',1,0,0,0,0,0,0),(1584,'2013-02-10 15:00:35',1,0,0,0,0,0,0),(1585,'2013-02-10 15:01:35',1,0,0,0,0,0,0),(1586,'2013-02-10 15:02:35',1,0,0,0,0,0,0),(1587,'2013-02-10 15:03:35',1,0,0,0,0,0,0),(1588,'2013-02-10 15:04:35',1,0,0,0,0,0,0),(1589,'2013-02-10 15:05:35',1,0,0,0,0,0,0),(1590,'2013-02-10 15:06:35',1,0,0,0,0,0,0),(1591,'2013-02-10 15:07:35',1,0,0,0,0,0,0),(1592,'2013-02-10 15:08:35',1,0,0,0,0,0,0),(1593,'2013-02-10 15:09:35',1,0,0,0,0,0,0),(1594,'2013-02-10 15:10:35',1,0,0,0,0,0,0),(1595,'2013-02-10 15:11:35',1,0,0,0,0,0,0),(1596,'2013-02-10 15:12:35',1,0,0,0,0,0,0),(1597,'2013-02-10 15:13:35',1,0,0,0,0,0,0),(1598,'2013-02-10 15:14:35',1,0,0,0,0,0,0),(1599,'2013-02-10 15:15:35',1,0,0,0,0,0,0),(1600,'2013-02-10 15:16:35',1,0,0,0,0,0,0),(1601,'2013-02-10 15:17:35',1,0,0,0,0,0,0),(1602,'2013-02-10 15:18:35',1,0,0,0,0,0,0),(1603,'2013-02-10 15:19:35',1,0,0,0,0,0,0),(1604,'2013-02-10 15:20:35',1,0,0,0,0,0,0),(1605,'2013-02-10 15:21:35',1,0,0,0,0,0,0),(1606,'2013-02-10 15:22:35',1,0,0,0,0,0,0),(1607,'2013-02-10 15:23:35',1,0,0,0,0,0,0),(1608,'2013-02-10 15:24:35',1,0,0,0,0,0,0),(1609,'2013-02-10 15:25:35',1,0,0,0,0,0,0),(1610,'2013-02-10 15:26:35',1,0,0,0,0,0,0),(1611,'2013-02-10 15:27:35',1,0,0,0,0,0,0),(1612,'2013-02-10 15:28:35',1,0,0,0,0,0,0),(1613,'2013-02-10 15:29:35',1,0,0,0,0,0,0),(1614,'2013-02-10 15:30:35',1,0,0,0,0,0,0),(1615,'2013-02-10 15:31:35',1,0,0,0,0,0,0),(1616,'2013-02-10 15:32:35',1,0,0,0,0,0,0),(1617,'2013-02-10 15:33:35',1,0,0,0,0,0,0),(1618,'2013-02-10 15:34:35',1,0,0,0,0,0,0),(1619,'2013-02-10 15:35:35',1,0,0,0,0,0,0),(1620,'2013-02-10 15:36:35',1,0,0,0,0,0,0),(1621,'2013-02-10 15:37:35',1,0,0,0,0,0,0),(1622,'2013-02-10 15:38:35',1,0,0,0,0,0,0),(1623,'2013-02-10 15:39:35',1,0,0,0,0,0,0),(1624,'2013-02-10 15:40:35',1,0,0,0,0,0,0),(1625,'2013-02-10 15:41:35',1,0,0,0,0,0,0),(1626,'2013-02-10 15:42:35',1,0,0,0,0,0,0),(1627,'2013-02-10 15:43:35',1,0,0,0,0,0,0),(1628,'2013-02-10 15:44:35',1,0,0,0,0,0,0),(1629,'2013-02-10 15:45:35',1,0,0,0,0,0,0),(1630,'2013-02-10 15:46:35',1,0,0,0,0,0,0),(1631,'2013-02-10 15:47:35',1,0,0,0,0,0,0),(1632,'2013-02-10 15:48:35',1,0,0,0,0,0,0),(1633,'2013-02-10 15:49:35',1,0,0,0,0,0,0),(1634,'2013-02-10 15:50:35',1,0,0,0,0,0,0),(1635,'2013-02-10 15:51:35',1,0,0,0,0,0,0),(1636,'2013-02-10 15:52:35',1,0,0,0,0,0,0),(1637,'2013-02-10 15:53:35',1,0,0,0,0,0,0),(1638,'2013-02-10 15:54:35',1,0,0,0,0,0,0),(1639,'2013-02-10 15:55:35',1,0,0,0,0,0,0),(1640,'2013-02-10 15:56:35',1,0,0,0,0,0,0),(1641,'2013-02-10 15:57:35',1,0,0,0,0,0,0),(1642,'2013-02-10 15:58:35',1,0,0,0,0,0,0),(1643,'2013-02-10 15:59:35',1,0,0,0,0,0,0),(1644,'2013-02-10 16:00:35',1,0,0,0,0,0,0),(1645,'2013-02-10 16:01:35',1,0,0,0,0,0,0),(1646,'2013-02-10 16:02:35',1,0,0,0,0,0,0),(1647,'2013-02-10 16:03:35',1,0,0,0,0,0,0),(1648,'2013-02-10 16:04:35',1,0,0,0,0,0,0),(1649,'2013-02-10 16:05:35',1,0,0,0,0,0,0),(1650,'2013-02-10 16:06:35',1,0,0,0,0,0,0),(1651,'2013-02-10 16:07:35',1,0,0,0,0,0,0),(1652,'2013-02-10 16:08:35',1,0,0,0,0,0,0),(1653,'2013-02-10 16:09:35',1,0,0,0,0,0,0),(1654,'2013-02-10 16:10:35',1,0,0,0,0,0,0),(1655,'2013-02-10 16:11:35',1,0,0,0,0,0,0),(1656,'2013-02-10 16:12:35',1,0,0,0,0,0,0),(1657,'2013-02-10 16:13:35',1,0,0,0,0,0,0),(1658,'2013-02-10 16:14:35',1,0,0,0,0,0,0),(1659,'2013-02-10 16:15:35',1,0,0,0,0,0,0),(1660,'2013-02-10 16:16:35',1,0,0,0,0,0,0),(1661,'2013-02-10 16:17:35',1,0,0,0,0,0,0),(1662,'2013-02-10 16:18:35',1,0,0,0,0,0,0),(1663,'2013-02-10 16:19:35',1,0,0,0,0,0,0),(1664,'2013-02-10 16:20:35',1,0,0,0,0,0,0),(1665,'2013-02-10 16:21:35',1,0,0,0,0,0,0),(1666,'2013-02-10 16:22:35',1,0,0,0,0,0,0),(1667,'2013-02-10 16:23:35',1,0,0,0,0,0,0),(1668,'2013-02-10 16:24:35',1,0,0,0,0,0,0),(1669,'2013-02-10 16:25:35',1,0,0,0,0,0,0),(1670,'2013-02-10 16:26:35',1,0,0,0,0,0,0),(1671,'2013-02-10 16:27:35',1,0,0,0,0,0,0),(1672,'2013-02-10 16:28:35',1,0,0,0,0,0,0),(1673,'2013-02-10 16:29:35',1,0,0,0,0,0,0),(1674,'2013-02-10 16:30:35',1,0,0,0,0,0,0),(1675,'2013-02-10 16:31:35',1,0,0,0,0,0,0),(1676,'2013-02-10 16:32:35',1,0,0,0,0,0,0),(1677,'2013-02-10 16:33:35',1,0,0,0,0,0,0),(1678,'2013-02-10 16:34:35',1,0,0,0,0,0,0),(1679,'2013-02-10 16:35:35',1,0,0,0,0,0,0),(1680,'2013-02-10 16:36:35',1,0,0,0,0,0,0),(1681,'2013-02-10 16:37:35',1,0,0,0,0,0,0),(1682,'2013-02-10 16:38:35',1,0,0,0,0,0,0),(1683,'2013-02-10 16:39:35',1,0,0,0,0,0,0),(1684,'2013-02-10 16:40:35',1,0,0,0,0,0,0),(1685,'2013-02-10 16:41:35',1,0,0,0,0,0,0),(1686,'2013-02-10 16:42:35',1,0,0,0,0,0,0),(1687,'2013-02-10 16:43:35',1,0,0,0,0,0,0),(1688,'2013-02-10 16:44:35',1,0,0,0,0,0,0),(1689,'2013-02-10 16:45:35',1,0,0,0,0,0,0),(1690,'2013-02-10 16:46:35',1,0,0,0,0,0,0),(1691,'2013-02-10 16:47:35',1,0,0,0,0,0,0),(1692,'2013-02-10 16:48:35',1,0,0,0,0,0,0),(1693,'2013-02-10 16:49:35',1,0,0,0,0,0,0),(1694,'2013-02-10 16:50:35',1,0,0,0,0,0,0),(1695,'2013-02-10 16:51:35',1,0,0,0,0,0,0),(1696,'2013-02-10 16:52:35',1,0,0,0,0,0,0),(1697,'2013-02-10 16:53:35',1,0,0,0,0,0,0),(1698,'2013-02-10 16:54:35',1,0,0,0,0,0,0),(1699,'2013-02-10 16:55:35',1,0,0,0,0,0,0),(1700,'2013-02-10 16:56:35',1,0,0,0,0,0,0),(1701,'2013-02-10 16:57:35',1,0,0,0,0,0,0),(1702,'2013-02-10 16:58:35',1,0,0,0,0,0,0),(1703,'2013-02-10 16:59:35',1,0,0,0,0,0,0),(1704,'2013-02-10 17:00:35',1,0,0,0,0,0,0),(1705,'2013-02-10 17:01:35',1,0,0,0,0,0,0),(1706,'2013-02-10 17:02:35',1,0,0,0,0,0,0),(1707,'2013-02-10 17:03:35',1,0,0,0,0,0,0),(1708,'2013-02-10 17:04:35',1,0,0,0,0,0,0),(1709,'2013-02-10 17:05:35',1,0,0,0,0,0,0),(1710,'2013-02-10 17:06:35',1,0,0,0,0,0,0),(1711,'2013-02-10 17:07:35',1,0,0,0,0,0,0),(1712,'2013-02-10 17:08:35',1,0,0,0,0,0,0),(1713,'2013-02-10 17:09:35',1,0,0,0,0,0,0),(1714,'2013-02-10 17:10:35',1,0,0,0,0,0,0),(1715,'2013-02-10 17:11:35',1,0,0,0,0,0,0),(1716,'2013-02-10 17:12:35',1,0,0,0,0,0,0),(1717,'2013-02-10 17:13:35',1,0,0,0,0,0,0),(1718,'2013-02-10 17:14:35',1,0,0,0,0,0,0),(1719,'2013-02-10 17:15:35',1,0,0,0,0,0,0),(1720,'2013-02-10 17:16:35',1,0,0,0,0,0,0),(1721,'2013-02-10 17:17:35',1,0,0,0,0,0,0),(1722,'2013-02-10 17:18:35',1,0,0,0,0,0,0),(1723,'2013-02-10 17:19:35',1,0,0,0,0,0,0),(1724,'2013-02-10 17:20:35',1,0,0,0,0,0,0),(1725,'2013-02-10 17:21:35',1,0,0,0,0,0,0),(1726,'2013-02-10 17:22:35',1,0,0,0,0,0,0),(1727,'2013-02-10 17:23:35',1,0,0,0,0,0,0),(1728,'2013-02-10 17:24:35',1,0,0,0,0,0,0),(1729,'2013-02-10 17:25:35',1,0,0,0,0,0,0),(1730,'2013-02-10 17:26:35',1,0,0,0,0,0,0),(1731,'2013-02-10 17:27:35',1,0,0,0,0,0,0),(1732,'2013-02-10 17:28:35',1,0,0,0,0,0,0),(1733,'2013-02-10 17:29:35',1,0,0,0,0,0,0),(1734,'2013-02-10 17:30:35',1,0,0,0,0,0,0),(1735,'2013-02-10 17:31:35',1,0,0,0,0,0,0),(1736,'2013-02-10 17:32:35',1,0,0,0,0,0,0),(1737,'2013-02-10 17:33:35',1,0,0,0,0,0,0),(1738,'2013-02-10 17:34:35',1,0,0,0,0,0,0),(1739,'2013-02-10 17:35:35',1,0,0,0,0,0,0),(1740,'2013-02-10 17:36:35',1,0,0,0,0,0,0),(1741,'2013-02-10 17:37:35',1,0,0,0,0,0,0),(1742,'2013-02-10 17:38:35',1,0,0,0,0,0,0),(1743,'2013-02-10 17:39:35',1,0,0,0,0,0,0),(1744,'2013-02-10 17:40:35',1,0,0,0,0,0,0),(1745,'2013-02-10 17:41:35',1,0,0,0,0,0,0),(1746,'2013-02-10 17:42:35',1,0,0,0,0,0,0),(1747,'2013-02-10 17:43:35',1,0,0,0,0,0,0),(1748,'2013-02-10 17:44:35',1,0,0,0,0,0,0),(1749,'2013-02-10 17:45:35',1,0,0,0,0,0,0),(1750,'2013-02-10 17:46:35',1,0,0,0,0,0,0),(1751,'2013-02-10 17:47:35',1,0,0,0,0,0,0),(1752,'2013-02-10 17:48:35',1,0,0,0,0,0,0),(1753,'2013-02-10 17:49:35',1,0,0,0,0,0,0),(1754,'2013-02-10 17:50:35',1,0,0,0,0,0,0),(1755,'2013-02-10 17:51:35',1,0,0,0,0,0,0),(1756,'2013-02-10 17:52:35',1,0,0,0,0,0,0),(1757,'2013-02-10 17:53:35',1,0,0,0,0,0,0),(1758,'2013-02-10 17:54:35',1,0,0,0,0,0,0),(1759,'2013-02-10 17:55:35',1,0,0,0,0,0,0),(1760,'2013-02-10 17:56:35',1,0,0,0,0,0,0),(1761,'2013-02-10 17:57:35',1,0,0,0,0,0,0),(1762,'2013-02-10 17:58:35',1,0,0,0,0,0,0),(1763,'2013-02-10 18:23:13',0,0,0,0,0,0,0),(1764,'2013-02-10 18:24:13',0,0,0,0,0,0,0),(1765,'2013-02-10 18:25:13',0,0,0,0,0,0,0),(1766,'2013-02-10 18:26:13',0,0,0,0,0,0,0),(1767,'2013-02-10 18:27:13',0,0,0,0,0,0,0),(1768,'2013-02-10 18:28:13',0,0,0,0,0,0,0),(1769,'2013-02-10 18:29:13',1,0,0,0,0,0,0),(1770,'2013-02-10 18:30:13',1,0,0,0,0,0,0),(1771,'2013-02-10 18:31:13',1,0,0,0,0,0,0),(1772,'2013-02-10 18:32:24',0,0,0,0,0,0,0),(1773,'2013-02-10 18:39:34',0,0,0,0,0,0,0),(1774,'2013-02-10 18:43:40',0,0,0,0,0,0,0),(1775,'2013-02-10 18:44:40',1,0,0,0,0,0,0),(1776,'2013-02-10 18:45:40',1,0,0,0,0,0,0),(1777,'2013-02-10 18:46:40',1,0,0,0,0,0,0),(1778,'2013-02-10 18:47:40',1,0,0,0,0,0,0),(1779,'2013-02-10 18:48:40',1,0,0,0,0,0,0),(1780,'2013-02-10 18:49:40',1,0,0,0,0,0,0),(1781,'2013-02-10 18:50:40',1,0,0,0,0,0,0),(1782,'2013-02-10 18:51:40',1,0,0,0,0,0,0),(1783,'2013-02-10 18:52:40',1,0,0,0,0,0,0),(1784,'2013-02-10 18:53:40',1,0,0,0,0,0,0),(1785,'2013-02-10 18:57:44',1,0,0,0,0,0,0),(1786,'2013-02-10 18:58:44',1,0,0,0,0,0,0),(1787,'2013-02-10 18:59:44',1,0,0,0,0,0,0),(1788,'2013-02-10 19:01:20',1,0,0,0,0,0,0),(1789,'2013-02-10 19:05:12',1,0,0,0,0,0,0),(1790,'2013-02-10 19:14:37',0,0,0,0,0,0,0),(1791,'2013-02-10 19:15:37',0,0,0,0,0,0,0),(1792,'2013-02-10 19:19:26',1,0,0,0,0,0,0),(1793,'2013-02-10 19:20:26',1,0,0,0,0,0,0),(1794,'2013-02-10 19:24:48',1,0,0,0,0,0,0),(1795,'2013-02-10 19:25:48',1,0,0,0,0,0,0),(1796,'2013-02-10 19:26:48',1,0,0,0,0,0,0),(1797,'2013-02-10 19:27:48',1,0,0,0,0,0,0),(1798,'2013-02-10 19:28:48',1,0,0,0,0,0,0),(1799,'2013-02-10 19:29:48',1,0,0,0,0,0,0),(1800,'2013-02-10 19:30:48',1,0,0,0,0,0,0),(1801,'2013-02-10 19:31:48',1,0,0,0,0,0,0),(1802,'2013-02-10 19:32:48',1,0,0,0,0,0,0),(1803,'2013-02-10 19:33:48',1,0,0,0,0,0,0),(1804,'2013-02-10 19:34:48',1,0,0,0,0,0,0),(1805,'2013-02-10 19:35:48',1,0,0,0,0,0,0),(1806,'2013-02-10 19:38:04',1,0,0,0,0,0,0),(1807,'2013-02-10 19:45:59',1,0,0,0,0,0,0),(1808,'2013-02-10 19:46:59',1,0,0,0,0,0,0),(1809,'2013-02-10 19:50:40',1,0,0,0,0,0,0),(1810,'2013-02-10 19:51:40',1,0,0,0,0,0,0),(1811,'2013-02-10 19:52:40',1,0,0,0,0,0,0),(1812,'2013-02-10 19:54:37',1,0,0,0,0,0,0),(1813,'2013-02-10 20:55:32',1,0,0,0,0,0,0),(1814,'2013-02-10 20:56:32',1,0,0,0,0,0,0),(1815,'2013-02-10 20:57:32',1,0,0,0,0,0,0),(1816,'2013-02-10 20:58:32',1,0,0,0,0,0,0),(1817,'2013-02-10 20:59:32',1,0,0,0,0,0,0),(1818,'2013-02-10 21:00:32',1,0,0,0,0,0,0),(1819,'2013-02-10 21:01:32',1,0,0,0,0,0,0),(1820,'2018-12-18 19:44:00',0,0,0,0,0,0,0),(1821,'2018-12-18 19:46:21',0,2,0,0,2,0,0),(1822,'2018-12-18 19:47:21',0,0,0,0,0,0,0),(1823,'2018-12-18 19:48:21',0,0,0,0,0,0,0),(1824,'2018-12-18 19:49:21',0,0,0,0,0,0,0),(1825,'2018-12-18 19:50:21',0,0,0,0,0,0,0),(1826,'2018-12-18 19:51:21',0,0,0,0,0,0,0),(1827,'2018-12-18 19:52:21',0,0,0,0,0,0,0),(1828,'2018-12-18 19:54:52',0,2,0,0,2,0,0),(1829,'2018-12-18 19:55:52',0,2,0,0,2,0,0),(1830,'2018-12-18 19:56:52',0,0,0,0,0,0,0),(1831,'2018-12-18 19:57:52',0,0,0,0,0,0,0),(1832,'2018-12-18 19:58:52',0,0,0,0,0,0,0),(1833,'2018-12-18 19:59:52',0,0,0,0,0,0,0),(1834,'2018-12-18 20:00:52',0,0,0,0,0,0,0),(1835,'2018-12-18 20:01:52',0,0,0,0,0,0,0),(1836,'2018-12-18 20:02:52',0,0,0,0,0,0,0),(1837,'2018-12-18 20:03:52',0,0,0,0,0,0,0),(1838,'2018-12-18 20:05:33',1,281,34,34,333,33,36),(1839,'2018-12-18 20:06:33',0,0,0,0,1,0,0),(1840,'2018-12-18 20:07:33',0,0,0,0,0,0,0),(1841,'2018-12-18 20:08:33',1,8,1,1,28,1,1),(1842,'2018-12-18 20:09:33',1,157,18,19,258,18,22),(1843,'2018-12-18 20:10:33',1,260,30,31,463,29,37),(1844,'2018-12-18 20:11:33',1,367,26,26,1009,26,79),(1845,'2018-12-18 20:17:04',1,902,26,26,3737,38,283),(1846,'2018-12-18 20:18:04',1,80,8,8,1085,14,97),(1847,'2018-12-18 20:19:04',1,421,36,38,961,35,75),(1848,'2018-12-18 20:20:04',0,52,4,4,5246,26,328),(1849,'2018-12-18 20:21:04',0,0,0,0,0,0,0),(1850,'2018-12-18 20:22:04',0,0,0,0,0,0,0),(1851,'2018-12-18 20:23:04',1,413,28,28,1160,29,90),(1852,'2018-12-18 20:24:04',1,428,31,31,1102,31,91),(1853,'2018-12-18 20:25:04',1,434,31,31,1121,32,92),(1854,'2018-12-18 20:26:04',1,207,17,18,4657,45,352),(1855,'2018-12-18 20:27:04',1,312,36,37,500,35,44),(1856,'2018-12-18 20:28:04',0,235,18,18,4121,33,281),(1857,'2018-12-18 20:31:38',1,350,40,42,508,39,47),(1858,'2018-12-18 20:32:38',1,39,5,5,115,4,6),(1859,'2018-12-18 20:33:38',1,226,27,27,402,26,31);
/*!40000 ALTER TABLE `world_stats_network` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_npc_kill_user`
--

DROP TABLE IF EXISTS `world_stats_npc_kill_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=latin1 COMMENT='Event log: NPC kill user.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_npc_kill_user`
--

LOCK TABLES `world_stats_npc_kill_user` WRITE;
/*!40000 ALTER TABLE `world_stats_npc_kill_user` DISABLE KEYS */;
INSERT INTO `world_stats_npc_kill_user` VALUES (1,6,1,1,512,512,279,594,1,'2013-02-04 23:55:22'),(2,6,1,1,512,512,762,1047,1,'2013-02-04 23:55:35'),(3,6,1,1,512,512,342,502,1,'2013-02-04 23:55:47'),(4,1,1,18,512,512,734,913,1,'2013-02-06 04:37:25'),(5,1,1,18,512,512,503,754,1,'2013-02-06 04:37:29'),(6,1,1,18,512,512,665,523,1,'2013-02-06 04:37:32'),(7,1,1,18,512,512,614,523,1,'2013-02-06 04:37:34'),(8,1,1,18,512,512,536,523,1,'2013-02-06 04:37:35'),(9,1,1,18,512,512,536,523,1,'2013-02-06 04:37:37'),(10,1,1,22,512,512,1531,713,1,'2013-02-06 05:08:38'),(11,1,1,22,512,512,1394,715,1,'2013-02-06 06:25:04'),(12,1,1,23,512,512,369,603,1,'2013-02-08 05:58:08'),(13,1,1,35,512,512,668,630,1,'2013-02-10 19:26:07'),(14,1,1,35,512,512,496,511,1,'2013-02-10 19:26:46'),(15,1,1,35,512,512,510,538,1,'2013-02-10 19:26:58'),(16,1,1,35,512,512,508,511,1,'2013-02-10 19:27:03'),(17,1,1,35,512,512,508,511,1,'2013-02-10 19:27:19'),(18,1,1,35,512,512,508,511,1,'2013-02-10 19:27:35'),(19,1,1,35,512,512,510,538,1,'2013-02-10 19:27:50'),(20,1,1,40,512,512,727,628,1,'2013-02-10 19:30:50'),(21,9,1,1,512,512,509,485,1,'2018-12-18 20:26:05');
/*!40000 ALTER TABLE `world_stats_npc_kill_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_quest_accept`
--

DROP TABLE IF EXISTS `world_stats_quest_accept`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Event log: User accepts a quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_quest_accept`
--

LOCK TABLES `world_stats_quest_accept` WRITE;
/*!40000 ALTER TABLE `world_stats_quest_accept` DISABLE KEYS */;
INSERT INTO `world_stats_quest_accept` VALUES (1,1,0,1,623,591,'2013-02-07 05:44:31'),(2,1,1,1,623,591,'2013-02-07 05:44:34'),(3,1,1,1,630,621,'2013-02-07 05:47:19'),(4,1,1,1,658,572,'2013-02-09 20:02:58'),(5,1,1,1,628,578,'2013-02-09 20:07:59'),(6,1,1,1,616,587,'2013-02-09 20:15:44'),(7,1,1,1,617,556,'2013-02-09 20:20:55'),(8,1,1,1,624,585,'2013-02-09 20:22:12'),(9,1,1,1,630,589,'2013-02-09 20:33:32'),(10,1,1,1,614,639,'2013-02-09 21:04:40'),(11,1,1,1,621,578,'2013-02-09 21:08:22'),(12,1,1,1,630,601,'2013-02-10 19:05:29'),(13,1,1,1,598,594,'2013-02-10 19:21:26'),(14,1,1,1,634,587,'2013-02-10 19:46:29'),(15,8,0,1,581,616,'2018-12-18 20:18:29'),(16,8,1,1,581,616,'2018-12-18 20:18:30');
/*!40000 ALTER TABLE `world_stats_quest_accept` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_quest_cancel`
--

DROP TABLE IF EXISTS `world_stats_quest_cancel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
 SET character_set_client = utf8mb4 ;
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=latin1 COMMENT='Event log: User completes a quest.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_quest_complete`
--

LOCK TABLES `world_stats_quest_complete` WRITE;
/*!40000 ALTER TABLE `world_stats_quest_complete` DISABLE KEYS */;
INSERT INTO `world_stats_quest_complete` VALUES (1,1,1,1,630,621,'2013-02-07 05:47:06'),(2,1,1,1,626,572,'2013-02-09 20:02:50'),(3,1,0,1,626,572,'2013-02-09 20:02:52'),(4,1,1,1,628,578,'2013-02-09 20:07:52'),(5,1,1,1,616,587,'2013-02-09 20:15:36'),(6,1,1,1,625,596,'2013-02-09 20:19:16'),(7,1,1,1,562,585,'2013-02-09 20:22:00'),(8,1,1,1,655,614,'2013-02-09 20:33:25'),(9,1,1,1,607,596,'2013-02-09 20:55:21'),(10,1,1,1,632,606,'2013-02-09 21:04:30'),(11,1,1,1,621,578,'2013-02-09 21:06:49'),(12,1,1,1,618,585,'2013-02-09 21:11:28'),(13,1,1,1,598,594,'2013-02-10 19:21:23'),(14,1,1,4,657,434,'2013-02-10 19:37:26'),(15,1,1,4,683,439,'2013-02-10 20:56:39');
/*!40000 ALTER TABLE `world_stats_quest_complete` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_user_consume_item`
--

DROP TABLE IF EXISTS `world_stats_user_consume_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=latin1 COMMENT='Event log: User consumes use-once item.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_user_consume_item`
--

LOCK TABLES `world_stats_user_consume_item` WRITE;
/*!40000 ALTER TABLE `world_stats_user_consume_item` DISABLE KEYS */;
INSERT INTO `world_stats_user_consume_item` VALUES (1,1,2,1,425,714,'2013-02-04 23:41:25'),(2,1,2,1,425,714,'2013-02-04 23:41:26'),(3,1,1,1,1436,737,'2013-02-08 15:37:42'),(4,1,1,1,1436,737,'2013-02-08 15:37:43'),(5,1,1,1,1436,737,'2013-02-08 15:37:43'),(6,1,1,1,1436,737,'2013-02-08 15:37:44'),(7,1,1,1,1436,737,'2013-02-08 15:37:45'),(8,1,1,1,1436,737,'2013-02-08 15:37:45'),(9,1,1,1,1436,737,'2013-02-08 15:37:45'),(10,1,2,4,631,458,'2013-02-10 01:59:53'),(11,1,2,4,631,458,'2013-02-10 01:59:59'),(12,1,2,4,631,458,'2013-02-10 01:59:59'),(13,1,2,4,364,454,'2013-02-10 03:25:16'),(14,1,2,4,364,454,'2013-02-10 03:25:17'),(15,1,2,4,364,454,'2013-02-10 03:25:17'),(16,1,2,4,364,454,'2013-02-10 03:25:17'),(17,1,2,4,364,454,'2013-02-10 03:25:18'),(18,1,2,4,364,454,'2013-02-10 03:25:18'),(19,1,2,4,364,454,'2013-02-10 03:25:18'),(20,1,2,4,364,454,'2013-02-10 03:25:18'),(21,1,2,4,364,454,'2013-02-10 03:25:18'),(22,1,1,4,572,413,'2013-02-10 04:31:54'),(23,1,1,4,572,413,'2013-02-10 04:31:55'),(24,1,1,4,572,413,'2013-02-10 04:31:55'),(25,1,1,4,572,413,'2013-02-10 04:31:56');
/*!40000 ALTER TABLE `world_stats_user_consume_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_user_kill_npc`
--

DROP TABLE IF EXISTS `world_stats_user_kill_npc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
) ENGINE=InnoDB AUTO_INCREMENT=624 DEFAULT CHARSET=latin1 COMMENT='Event log: User kills NPC.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_user_kill_npc`
--

LOCK TABLES `world_stats_user_kill_npc` WRITE;
/*!40000 ALTER TABLE `world_stats_user_kill_npc` DISABLE KEYS */;
INSERT INTO `world_stats_user_kill_npc` VALUES (1,1,1,1,496,1201,0,0,1,'2013-02-04 22:51:43'),(2,1,1,1,478,1201,0,0,1,'2013-02-04 22:51:43'),(3,1,1,1,489,1226,0,0,1,'2013-02-04 22:52:00'),(4,1,1,2,1193,289,0,0,1,'2013-02-04 22:52:24'),(5,1,1,2,1193,318,0,0,1,'2013-02-04 22:52:25'),(6,1,1,2,1035,732,0,0,1,'2013-02-04 22:52:35'),(7,1,1,2,873,732,0,0,1,'2013-02-04 22:52:37'),(8,1,1,2,966,721,0,0,1,'2013-02-04 22:52:43'),(9,1,1,2,347,545,0,0,1,'2013-02-04 22:53:00'),(10,1,1,3,1440,705,0,0,1,'2013-02-04 22:53:12'),(11,1,1,3,1537,705,0,0,1,'2013-02-04 22:53:13'),(12,1,1,3,1505,226,0,0,1,'2013-02-04 22:53:18'),(13,1,1,3,1130,733,0,0,1,'2013-02-04 23:40:26'),(14,1,1,3,1011,733,0,0,1,'2013-02-04 23:40:27'),(15,1,1,3,518,625,0,0,1,'2013-02-04 23:40:30'),(16,1,1,4,1163,1857,0,0,1,'2013-02-04 23:40:53'),(17,1,1,4,1163,1857,0,0,1,'2013-02-04 23:40:53'),(18,1,1,4,1163,1857,0,0,1,'2013-02-04 23:40:54'),(19,1,1,4,375,728,0,0,1,'2013-02-04 23:41:19'),(20,1,1,4,781,788,0,0,1,'2013-02-04 23:41:59'),(21,6,1,1,558,512,0,0,1,'2013-02-04 23:55:51'),(22,6,1,1,558,512,0,0,1,'2013-02-04 23:55:54'),(23,6,1,1,558,512,0,0,1,'2013-02-04 23:55:59'),(24,1,1,4,106,656,0,0,1,'2013-02-04 23:56:30'),(25,1,1,4,285,481,0,0,1,'2013-02-05 00:02:44'),(26,1,1,5,346,649,0,0,1,'2013-02-05 00:03:04'),(27,1,1,4,285,703,0,0,1,'2013-02-05 00:05:01'),(28,1,1,5,285,677,0,0,1,'2013-02-05 00:05:01'),(29,1,1,4,248,591,0,0,1,'2013-02-05 01:10:01'),(30,1,1,5,529,638,0,0,1,'2013-02-05 01:10:15'),(31,1,1,5,412,575,0,0,1,'2013-02-05 01:10:29'),(32,1,1,5,348,690,0,0,1,'2013-02-05 01:10:30'),(33,1,1,5,1235,698,0,0,1,'2013-02-05 01:10:37'),(34,1,1,5,1505,258,0,0,1,'2013-02-05 01:21:39'),(35,1,1,5,547,463,0,0,1,'2013-02-05 01:21:47'),(36,1,1,6,486,463,0,0,1,'2013-02-05 01:21:47'),(37,1,1,6,544,440,0,0,1,'2013-02-05 01:22:00'),(38,1,1,6,595,332,0,0,1,'2013-02-05 01:22:03'),(39,1,1,6,595,760,0,0,1,'2013-02-05 01:22:07'),(40,1,1,6,449,227,0,0,1,'2013-02-05 01:23:02'),(41,1,1,6,449,227,0,0,1,'2013-02-05 01:23:02'),(42,1,1,7,449,227,0,0,1,'2013-02-05 01:23:04'),(43,1,1,7,449,227,0,0,1,'2013-02-05 01:25:12'),(44,1,1,7,595,227,0,0,1,'2013-02-05 01:29:09'),(45,1,1,7,328,1012,0,0,1,'2013-02-05 01:34:27'),(46,1,1,7,751,613,0,0,1,'2013-02-05 01:35:16'),(47,1,1,7,486,472,0,0,1,'2013-02-05 01:35:18'),(48,1,1,7,587,490,0,0,1,'2013-02-05 02:13:23'),(49,1,1,8,514,238,0,0,1,'2013-02-05 02:13:35'),(50,1,1,8,698,933,0,0,1,'2013-02-05 02:13:39'),(51,1,1,8,802,908,0,0,1,'2013-02-05 02:13:44'),(52,1,1,8,658,623,0,0,1,'2013-02-05 02:13:47'),(53,1,1,8,522,577,0,0,1,'2013-02-05 02:13:56'),(54,1,1,8,738,605,0,0,1,'2013-02-05 02:13:57'),(55,1,1,9,1512,735,0,0,1,'2013-02-05 02:14:02'),(56,1,1,9,947,729,0,0,1,'2013-02-05 02:14:26'),(57,1,1,9,662,513,0,0,1,'2013-02-05 02:14:28'),(58,1,1,9,662,416,0,0,1,'2013-02-05 02:14:29'),(59,1,1,9,662,607,0,0,1,'2013-02-05 02:14:38'),(60,1,1,9,630,1073,0,0,1,'2013-02-05 02:14:42'),(61,1,1,10,709,1192,0,0,1,'2013-02-05 02:14:43'),(62,1,1,10,546,808,0,0,1,'2013-02-05 02:15:03'),(63,1,1,10,453,336,0,0,1,'2013-02-05 02:15:08'),(64,1,1,10,449,421,0,0,1,'2013-02-05 02:15:09'),(65,1,1,7,400,598,0,0,1,'2013-02-05 02:29:21'),(66,1,1,8,519,598,0,0,1,'2013-02-05 02:29:33'),(67,1,1,8,587,620,0,0,1,'2013-02-05 02:29:41'),(68,1,1,8,544,782,0,0,1,'2013-02-05 02:30:01'),(69,1,1,8,986,1523,0,0,1,'2013-02-05 02:30:05'),(70,1,1,7,436,893,0,0,1,'2013-02-05 04:11:07'),(71,1,1,8,529,800,0,0,1,'2013-02-05 04:11:08'),(72,1,1,8,533,627,0,0,1,'2013-02-05 04:11:20'),(73,1,1,8,378,627,0,0,1,'2013-02-05 04:11:23'),(74,1,1,8,1217,256,0,0,1,'2013-02-05 04:11:39'),(75,1,1,8,1184,325,0,0,1,'2013-02-05 04:11:39'),(76,1,1,8,192,1099,0,0,1,'2013-02-05 04:11:45'),(77,1,1,9,1699,719,0,0,1,'2013-02-05 04:11:56'),(78,1,1,9,449,421,0,0,1,'2013-02-05 04:20:32'),(79,1,1,9,449,421,0,0,1,'2013-02-05 04:20:33'),(80,1,1,9,449,421,0,0,1,'2013-02-05 04:20:34'),(81,1,1,9,1240,705,0,0,1,'2013-02-05 04:20:44'),(82,1,1,9,679,752,0,0,1,'2013-02-05 04:20:47'),(83,1,1,10,574,856,0,0,1,'2013-02-05 04:20:48'),(84,1,1,10,1623,778,0,0,1,'2013-02-05 04:20:57'),(85,1,1,10,662,1059,0,0,1,'2013-02-05 04:22:04'),(86,1,1,10,619,994,0,0,1,'2013-02-05 04:22:07'),(87,1,1,10,1530,762,0,0,1,'2013-02-05 04:22:24'),(88,1,1,10,1242,726,0,0,1,'2013-02-05 04:22:30'),(89,1,1,11,1137,726,0,0,1,'2013-02-05 04:22:45'),(90,1,1,7,414,818,0,0,1,'2013-02-05 04:23:50'),(91,1,1,8,1038,483,0,0,1,'2013-02-05 04:24:12'),(92,1,6,8,303,434,0,0,2,'2013-02-05 04:24:36'),(93,1,6,8,202,311,0,0,2,'2013-02-05 04:25:02'),(94,1,6,8,205,290,0,0,2,'2013-02-05 04:25:03'),(95,1,6,8,468,290,0,0,2,'2013-02-05 04:25:09'),(96,1,1,9,1026,943,0,0,1,'2013-02-05 04:25:55'),(97,1,1,9,929,1040,0,0,1,'2013-02-05 04:25:56'),(98,1,1,9,363,1605,0,0,1,'2013-02-05 04:25:59'),(99,1,1,9,105,641,0,0,1,'2013-02-05 04:26:08'),(100,1,1,9,514,359,0,0,1,'2013-02-05 04:26:32'),(101,1,6,10,220,484,0,0,2,'2013-02-05 04:26:47'),(102,1,1,10,960,430,0,0,1,'2013-02-05 04:27:12'),(103,1,1,10,1179,895,0,0,1,'2013-02-05 04:27:15'),(104,1,1,10,1235,891,0,0,1,'2013-02-05 04:27:16'),(105,1,1,10,1235,743,0,0,1,'2013-02-05 04:27:35'),(106,1,1,11,1235,743,0,0,1,'2013-02-05 04:31:51'),(107,1,1,11,1235,743,0,0,1,'2013-02-05 04:32:08'),(108,1,1,11,1098,768,0,0,1,'2013-02-05 04:32:20'),(109,1,1,11,605,584,0,0,1,'2013-02-05 04:32:24'),(110,1,1,11,576,411,0,0,1,'2013-02-05 04:32:25'),(111,1,1,11,373,485,0,0,1,'2013-02-05 04:32:31'),(112,1,1,12,285,577,0,0,1,'2013-02-05 04:33:16'),(113,1,1,12,485,404,0,0,1,'2013-02-05 04:33:19'),(114,1,1,12,776,1325,0,0,1,'2013-02-05 04:33:27'),(115,1,1,12,1342,713,0,0,1,'2013-02-05 04:33:34'),(116,1,1,12,1558,704,0,0,1,'2013-02-05 04:33:43'),(117,1,1,12,1472,705,0,0,1,'2013-02-05 04:33:46'),(118,1,1,13,567,355,0,0,1,'2013-02-05 04:33:54'),(119,1,1,11,1152,743,0,0,1,'2013-02-05 04:46:11'),(120,1,1,11,1044,743,0,0,1,'2013-02-05 04:46:12'),(121,1,1,11,108,524,0,0,1,'2013-02-05 04:47:04'),(122,1,6,11,417,438,0,0,2,'2013-02-05 04:48:35'),(123,1,6,11,413,438,0,0,2,'2013-02-05 04:48:37'),(124,1,6,12,427,438,0,0,2,'2013-02-05 04:48:42'),(125,1,6,12,427,463,0,0,2,'2013-02-05 04:48:42'),(126,1,6,12,456,589,0,0,2,'2013-02-05 04:48:50'),(127,1,6,12,604,611,0,0,2,'2013-02-05 04:48:54'),(128,1,6,13,575,611,0,0,2,'2013-02-05 04:48:58'),(129,1,6,13,586,611,0,0,2,'2013-02-05 04:49:02'),(130,1,6,13,614,611,0,0,2,'2013-02-05 04:49:03'),(131,1,6,13,596,611,0,0,2,'2013-02-05 04:49:04'),(132,1,6,14,611,611,0,0,2,'2013-02-05 04:49:06'),(133,1,1,14,1096,805,0,0,1,'2013-02-05 04:49:18'),(134,1,1,14,1211,700,0,0,1,'2013-02-05 04:49:19'),(135,1,1,14,1069,711,0,0,1,'2013-02-05 04:49:21'),(136,1,1,14,249,1074,0,0,1,'2013-02-05 04:49:43'),(137,1,1,15,249,1189,0,0,1,'2013-02-05 04:49:44'),(138,1,1,15,483,1189,0,0,1,'2013-02-05 04:49:45'),(139,1,1,15,353,1739,0,0,1,'2013-02-05 04:49:59'),(140,1,1,15,1584,370,0,0,1,'2013-02-05 04:50:09'),(141,1,1,15,1407,742,0,0,1,'2013-02-05 04:50:13'),(142,1,1,15,824,666,0,0,1,'2013-02-05 04:50:16'),(143,1,1,16,849,706,0,0,1,'2013-02-05 06:17:39'),(144,1,1,16,684,825,0,0,1,'2013-02-06 04:09:04'),(145,1,1,16,756,825,0,0,1,'2013-02-06 04:09:05'),(146,1,1,16,489,547,0,0,1,'2013-02-06 04:29:08'),(147,1,1,16,489,547,0,0,1,'2013-02-06 04:29:09'),(148,1,1,16,960,517,0,0,1,'2013-02-06 04:29:38'),(149,1,1,17,654,510,0,0,1,'2013-02-06 04:29:41'),(150,1,1,17,727,1198,0,0,1,'2013-02-06 04:33:50'),(151,1,1,17,727,1198,0,0,1,'2013-02-06 04:33:51'),(152,1,1,17,874,1144,0,0,1,'2013-02-06 04:34:05'),(153,1,1,17,457,672,470,683,1,'2013-02-06 04:34:27'),(154,1,1,17,586,672,575,683,1,'2013-02-06 04:34:32'),(155,1,1,18,586,672,575,683,1,'2013-02-06 04:34:33'),(156,1,1,18,327,672,367,672,1,'2013-02-06 04:34:43'),(157,1,1,18,205,672,193,672,1,'2013-02-06 04:34:45'),(158,1,1,18,205,672,193,672,1,'2013-02-06 04:34:45'),(159,1,1,18,562,749,0,0,1,'2013-02-06 04:37:42'),(160,1,1,18,417,560,0,0,1,'2013-02-06 04:38:08'),(161,1,1,19,1004,917,994,924,1,'2013-02-06 04:38:19'),(162,1,1,19,1004,917,994,924,1,'2013-02-06 04:38:19'),(163,1,1,19,1004,917,994,924,1,'2013-02-06 04:38:20'),(164,1,1,19,1004,917,994,924,1,'2013-02-06 04:38:22'),(165,1,1,19,1004,917,994,924,1,'2013-02-06 04:38:22'),(166,1,1,19,1004,917,994,924,1,'2013-02-06 04:38:23'),(167,1,1,20,1004,917,994,924,1,'2013-02-06 04:38:24'),(168,1,1,20,1004,917,994,924,1,'2013-02-06 04:38:24'),(169,1,1,20,1004,917,994,924,1,'2013-02-06 04:38:25'),(170,1,1,20,1004,917,994,924,1,'2013-02-06 04:38:25'),(171,1,1,20,1004,917,0,0,1,'2013-02-06 04:38:26'),(172,1,1,20,1069,946,0,0,1,'2013-02-06 04:38:39'),(173,1,1,21,1069,841,0,0,1,'2013-02-06 04:38:51'),(174,1,1,21,1447,716,0,0,1,'2013-02-06 04:38:54'),(175,1,1,21,1580,464,0,0,1,'2013-02-06 04:38:56'),(176,1,1,21,1638,432,0,0,1,'2013-02-06 04:39:53'),(177,1,1,21,1584,259,1608,227,1,'2013-02-06 05:08:26'),(178,1,1,21,1584,294,1608,269,1,'2013-02-06 05:08:26'),(179,1,1,22,1609,294,1614,269,1,'2013-02-06 05:08:28'),(180,1,1,22,1609,294,1614,269,1,'2013-02-06 05:08:28'),(181,1,1,22,327,775,0,0,1,'2013-02-06 05:31:59'),(182,1,1,22,1368,705,0,0,1,'2013-02-06 06:25:02'),(183,1,1,22,1368,705,0,0,1,'2013-02-06 06:25:03'),(184,1,1,22,1505,424,1505,424,1,'2013-02-06 21:07:46'),(185,1,1,22,1505,416,0,0,1,'2013-02-07 05:28:03'),(186,1,1,23,1160,355,0,0,1,'2013-02-07 05:28:17'),(187,1,1,23,1505,425,0,0,1,'2013-02-07 05:28:26'),(188,1,1,22,1630,419,0,0,1,'2013-02-07 05:29:29'),(189,1,1,23,496,254,0,0,1,'2013-02-07 05:29:49'),(190,1,1,23,575,805,0,0,1,'2013-02-07 05:29:53'),(191,1,1,23,632,733,0,0,1,'2013-02-07 05:30:01'),(192,1,1,23,794,614,0,0,1,'2013-02-07 05:30:19'),(193,1,1,22,1505,970,0,0,1,'2013-02-07 05:35:57'),(194,1,1,23,1558,1010,0,0,1,'2013-02-07 05:35:58'),(195,1,1,23,889,718,0,0,1,'2013-02-07 05:36:29'),(196,1,1,23,914,718,0,0,1,'2013-02-07 05:36:29'),(197,1,1,23,594,592,0,0,1,'2013-02-07 05:36:37'),(198,1,1,23,652,1070,0,0,1,'2013-02-07 05:36:59'),(199,1,1,23,799,811,0,0,1,'2013-02-07 05:37:32'),(200,1,1,24,867,811,0,0,1,'2013-02-07 05:37:33'),(201,1,1,24,972,811,0,0,1,'2013-02-07 05:37:47'),(202,1,1,24,939,735,0,0,1,'2013-02-07 05:38:06'),(203,1,1,24,939,735,0,0,1,'2013-02-07 05:38:07'),(204,1,1,24,939,735,0,0,1,'2013-02-07 05:38:21'),(205,1,1,24,965,764,0,0,1,'2013-02-07 05:38:37'),(206,1,1,25,965,764,0,0,1,'2013-02-07 05:38:42'),(207,1,1,25,1001,1109,0,0,1,'2013-02-07 05:38:53'),(208,1,1,25,1016,911,0,0,1,'2013-02-07 05:39:05'),(209,1,1,25,1156,706,0,0,1,'2013-02-07 05:39:12'),(210,1,1,25,651,918,0,0,1,'2013-02-07 05:39:23'),(211,1,1,25,149,1410,0,0,1,'2013-02-07 05:39:36'),(212,1,6,26,1085,908,1061,908,1,'2013-02-07 05:39:48'),(213,1,6,26,1085,908,1080,908,1,'2013-02-07 05:39:50'),(214,1,1,26,1085,908,0,0,1,'2013-02-07 05:39:52'),(215,1,1,26,1117,815,0,0,1,'2013-02-07 05:39:57'),(216,1,1,26,1533,739,0,0,1,'2013-02-07 05:40:00'),(217,1,6,27,309,211,0,0,2,'2013-02-07 05:40:38'),(218,1,6,27,291,211,0,0,2,'2013-02-07 05:40:42'),(219,1,6,27,291,211,0,0,2,'2013-02-07 05:40:43'),(220,1,6,27,323,211,0,0,2,'2013-02-07 05:40:45'),(221,1,6,28,370,291,0,0,2,'2013-02-07 05:40:50'),(222,1,6,28,370,316,0,0,2,'2013-02-07 05:40:54'),(223,1,6,28,392,471,0,0,2,'2013-02-07 05:41:00'),(224,1,6,29,467,492,0,0,2,'2013-02-07 05:41:05'),(225,1,6,29,428,564,0,0,2,'2013-02-07 05:41:08'),(226,1,1,29,1375,729,0,0,1,'2013-02-07 05:41:19'),(227,1,1,29,1505,369,0,0,1,'2013-02-07 05:41:33'),(228,1,1,29,1199,317,0,0,1,'2013-02-07 05:45:05'),(229,1,6,30,403,427,0,0,2,'2013-02-07 05:45:17'),(230,1,6,30,403,427,0,0,2,'2013-02-07 05:45:19'),(231,1,6,30,403,427,0,0,2,'2013-02-07 05:45:22'),(232,1,6,30,403,427,0,0,2,'2013-02-07 05:45:25'),(233,1,6,31,439,567,0,0,2,'2013-02-07 05:45:51'),(234,1,6,31,439,567,0,0,2,'2013-02-07 05:45:52'),(235,1,6,31,439,567,0,0,2,'2013-02-07 05:45:53'),(236,1,6,31,414,545,0,0,2,'2013-02-07 05:45:55'),(237,1,6,32,414,545,0,0,2,'2013-02-07 05:45:56'),(238,1,6,32,494,545,0,0,2,'2013-02-07 05:45:57'),(239,1,6,32,544,545,0,0,2,'2013-02-07 05:45:59'),(240,1,6,32,555,545,0,0,2,'2013-02-07 05:46:00'),(241,1,6,33,555,545,0,0,2,'2013-02-07 05:46:01'),(242,1,6,33,555,545,0,0,2,'2013-02-07 05:46:03'),(243,1,6,33,555,545,0,0,2,'2013-02-07 05:46:04'),(244,1,6,34,555,545,0,0,2,'2013-02-07 05:46:09'),(245,1,6,34,547,545,0,0,2,'2013-02-07 05:46:11'),(246,1,6,34,501,545,0,0,2,'2013-02-07 05:46:12'),(247,1,6,34,512,571,0,0,2,'2013-02-07 05:46:14'),(248,1,6,35,792,571,0,0,2,'2013-02-07 05:46:16'),(249,1,6,35,814,571,0,0,2,'2013-02-07 05:46:17'),(250,1,6,35,814,499,0,0,2,'2013-02-07 05:46:19'),(251,1,6,35,814,499,0,0,2,'2013-02-07 05:46:20'),(252,1,6,36,814,499,0,0,2,'2013-02-07 05:46:21'),(253,1,6,36,814,484,0,0,2,'2013-02-07 05:46:23'),(254,1,6,36,814,517,0,0,2,'2013-02-07 05:46:23'),(255,1,6,36,681,542,0,0,2,'2013-02-07 05:46:24'),(256,1,6,37,605,477,0,0,2,'2013-02-07 05:46:26'),(257,1,6,37,652,283,0,0,2,'2013-02-07 05:46:27'),(258,1,6,37,688,283,0,0,2,'2013-02-07 05:46:28'),(259,1,6,38,688,283,0,0,2,'2013-02-07 05:46:31'),(260,1,6,38,637,283,0,0,2,'2013-02-07 05:46:32'),(261,1,6,38,591,265,0,0,2,'2013-02-07 05:46:33'),(262,1,6,38,591,250,0,0,2,'2013-02-07 05:46:34'),(263,1,6,39,573,250,0,0,2,'2013-02-07 05:46:35'),(264,1,6,39,367,250,0,0,2,'2013-02-07 05:46:36'),(265,1,6,39,335,185,0,0,2,'2013-02-07 05:46:37'),(266,1,6,39,263,236,0,0,2,'2013-02-07 05:46:38'),(267,1,6,40,195,545,0,0,2,'2013-02-07 05:46:42'),(268,1,6,40,234,545,0,0,2,'2013-02-07 05:46:42'),(269,1,6,40,465,545,0,0,2,'2013-02-07 05:46:44'),(270,1,1,41,594,603,0,0,1,'2013-02-07 05:47:36'),(271,1,1,41,932,603,0,0,1,'2013-02-07 05:48:09'),(272,1,1,41,1555,691,0,0,1,'2013-02-07 05:48:59'),(273,1,1,22,1641,916,0,0,1,'2013-02-08 05:05:19'),(274,1,1,23,623,473,0,0,1,'2013-02-08 05:05:31'),(275,1,1,23,666,481,0,0,1,'2013-02-08 05:05:31'),(276,1,1,23,727,549,0,0,1,'2013-02-08 05:05:32'),(277,1,1,23,741,588,0,0,1,'2013-02-08 05:06:58'),(278,1,1,22,659,642,0,0,1,'2013-02-08 05:08:10'),(279,1,1,23,1425,742,0,0,1,'2013-02-08 05:08:25'),(280,1,1,22,950,826,0,0,1,'2013-02-08 05:13:18'),(281,1,1,23,770,930,0,0,1,'2013-02-08 05:13:20'),(282,1,1,23,569,941,0,0,1,'2013-02-08 05:13:21'),(283,1,1,23,1677,1147,0,0,1,'2013-02-08 05:13:40'),(284,1,6,23,429,387,0,0,2,'2013-02-08 05:14:18'),(285,1,6,23,439,387,0,0,2,'2013-02-08 05:14:18'),(286,1,6,24,468,419,0,0,2,'2013-02-08 05:14:20'),(287,1,6,24,468,329,0,0,2,'2013-02-08 05:14:20'),(288,1,6,24,468,376,0,0,2,'2013-02-08 05:14:21'),(289,1,6,24,468,358,0,0,2,'2013-02-08 05:14:21'),(290,1,6,25,468,376,0,0,2,'2013-02-08 05:14:22'),(291,1,6,25,468,358,0,0,2,'2013-02-08 05:14:23'),(292,1,6,25,457,369,0,0,2,'2013-02-08 05:14:23'),(293,1,6,26,342,293,0,0,2,'2013-02-08 05:14:26'),(294,1,6,26,497,542,0,0,2,'2013-02-08 05:14:31'),(295,1,6,26,493,614,0,0,2,'2013-02-08 05:14:32'),(296,1,1,26,589,700,0,0,1,'2013-02-08 05:14:39'),(297,1,1,26,423,700,0,0,1,'2013-02-08 05:14:41'),(298,1,1,27,398,700,0,0,1,'2013-02-08 05:14:42'),(299,1,1,27,873,1082,0,0,1,'2013-02-08 05:15:12'),(300,1,1,27,776,1082,0,0,1,'2013-02-08 05:15:13'),(301,1,1,22,666,458,0,0,1,'2013-02-08 05:18:29'),(302,1,1,23,612,296,0,0,1,'2013-02-08 05:18:31'),(303,1,1,23,759,653,0,0,1,'2013-02-08 05:18:39'),(304,1,1,22,1037,743,0,0,1,'2013-02-08 05:25:16'),(305,1,1,23,936,743,0,0,1,'2013-02-08 05:25:17'),(306,1,1,23,1396,742,0,0,1,'2013-02-08 05:25:28'),(307,1,1,23,1505,652,0,0,1,'2013-02-08 05:25:36'),(308,1,1,22,914,714,0,0,1,'2013-02-08 05:26:52'),(309,1,1,23,594,714,0,0,1,'2013-02-08 05:26:54'),(310,1,1,23,471,714,0,0,1,'2013-02-08 05:26:55'),(311,1,1,22,1505,606,0,0,1,'2013-02-08 05:28:02'),(312,1,1,22,1382,729,0,0,1,'2013-02-08 05:29:27'),(313,1,1,23,1173,729,0,0,1,'2013-02-08 05:29:28'),(314,1,1,22,583,473,0,0,1,'2013-02-08 05:30:38'),(315,1,1,23,486,473,0,0,1,'2013-02-08 05:30:39'),(316,1,1,22,1274,707,0,0,1,'2013-02-08 05:32:16'),(317,1,1,23,529,790,0,0,1,'2013-02-08 05:32:28'),(318,1,1,23,453,1056,0,0,1,'2013-02-08 05:32:30'),(319,1,1,23,464,1056,0,0,1,'2013-02-08 05:32:31'),(320,1,1,22,896,686,0,0,1,'2013-02-08 05:56:44'),(321,1,1,23,752,686,0,0,1,'2013-02-08 05:56:46'),(322,1,1,23,763,686,0,0,1,'2013-02-08 05:56:47'),(323,1,1,22,713,725,0,0,1,'2013-02-08 06:01:44'),(324,1,1,23,775,1543,0,0,1,'2013-02-08 15:29:14'),(325,1,1,23,656,1543,0,0,1,'2013-02-08 15:29:15'),(326,1,1,23,1461,742,0,0,1,'2013-02-08 15:34:47'),(327,1,1,23,763,742,0,0,1,'2013-02-08 15:35:11'),(328,1,1,23,1652,212,0,0,1,'2013-02-08 15:36:19'),(329,1,1,23,306,737,0,0,1,'2013-02-08 15:38:08'),(330,1,1,23,432,737,0,0,1,'2013-02-08 15:38:09'),(331,1,1,23,335,737,0,0,1,'2013-02-08 15:38:10'),(332,1,1,23,374,748,0,0,1,'2013-02-08 15:38:45'),(333,1,1,23,285,965,0,0,1,'2013-02-08 15:39:15'),(334,1,1,24,1321,729,0,0,1,'2013-02-08 15:39:26'),(335,1,1,24,1447,729,0,0,1,'2013-02-08 15:39:27'),(336,1,1,24,960,481,0,0,1,'2013-02-08 15:40:09'),(337,1,1,24,519,548,0,0,1,'2013-02-08 15:40:12'),(338,1,1,24,584,548,0,0,1,'2013-02-08 15:40:13'),(339,1,1,24,761,1756,0,0,1,'2013-02-08 15:42:08'),(340,1,1,25,819,1756,0,0,1,'2013-02-08 15:42:10'),(341,1,1,25,428,770,0,0,1,'2013-02-08 15:42:33'),(342,1,1,23,795,700,0,0,1,'2013-02-09 19:59:22'),(343,1,1,23,792,578,0,0,1,'2013-02-09 19:59:24'),(344,1,1,23,482,484,0,0,1,'2013-02-09 19:59:26'),(345,1,1,23,561,669,0,0,1,'2013-02-09 20:00:32'),(346,1,1,23,504,669,0,0,1,'2013-02-09 20:00:32'),(347,1,1,23,464,398,0,0,1,'2013-02-09 20:00:37'),(348,1,1,23,1504,770,0,0,1,'2013-02-09 20:06:48'),(349,1,1,23,647,583,0,0,1,'2013-02-09 20:06:58'),(350,1,1,23,669,583,0,0,1,'2013-02-09 20:07:02'),(351,1,6,23,504,553,0,0,2,'2013-02-09 20:07:17'),(352,1,6,23,547,553,0,0,2,'2013-02-09 20:07:20'),(353,1,6,24,512,553,0,0,2,'2013-02-09 20:07:23'),(354,1,6,24,512,553,0,0,2,'2013-02-09 20:07:25'),(355,1,6,24,512,553,0,0,2,'2013-02-09 20:07:27'),(356,1,6,24,512,553,0,0,2,'2013-02-09 20:07:28'),(357,1,6,25,605,553,0,0,2,'2013-02-09 20:07:37'),(358,1,6,25,645,607,0,0,2,'2013-02-09 20:07:37'),(359,1,6,25,645,599,0,0,2,'2013-02-09 20:07:38'),(360,1,6,26,627,556,0,0,2,'2013-02-09 20:07:40'),(361,1,6,26,670,517,0,0,2,'2013-02-09 20:07:41'),(362,1,6,26,634,517,0,0,2,'2013-02-09 20:07:41'),(363,1,1,23,720,589,0,0,1,'2013-02-09 20:09:39'),(364,1,1,23,759,697,0,0,1,'2013-02-09 20:09:41'),(365,1,6,23,497,437,0,0,2,'2013-02-09 20:09:53'),(366,1,6,23,497,437,0,0,2,'2013-02-09 20:09:54'),(367,1,6,23,497,437,0,0,2,'2013-02-09 20:09:57'),(368,1,6,24,497,437,0,0,2,'2013-02-09 20:09:58'),(369,1,6,24,497,437,0,0,2,'2013-02-09 20:09:59'),(370,1,6,24,497,437,0,0,2,'2013-02-09 20:10:01'),(371,1,6,25,497,437,0,0,2,'2013-02-09 20:10:07'),(372,1,6,25,468,625,0,0,2,'2013-02-09 20:10:16'),(373,1,1,25,804,445,0,0,1,'2013-02-09 20:10:21'),(374,1,1,25,724,574,0,0,1,'2013-02-09 20:10:23'),(375,1,1,25,753,643,0,0,1,'2013-02-09 20:10:36'),(376,1,6,26,443,495,0,0,2,'2013-02-09 20:10:41'),(377,1,6,26,414,520,0,0,2,'2013-02-09 20:10:42'),(378,1,6,26,429,520,0,0,2,'2013-02-09 20:10:42'),(379,1,6,26,339,592,0,0,2,'2013-02-09 20:10:43'),(380,1,6,27,313,592,0,0,2,'2013-02-09 20:10:44'),(381,1,6,27,238,466,0,0,2,'2013-02-09 20:10:45'),(382,1,6,27,238,473,0,0,2,'2013-02-09 20:10:46'),(383,1,6,27,238,506,0,0,2,'2013-02-09 20:10:47'),(384,1,6,28,346,538,0,0,2,'2013-02-09 20:10:47'),(385,1,1,28,616,587,0,0,1,'2013-02-09 20:15:33'),(386,1,1,23,994,180,0,0,1,'2013-02-09 20:18:04'),(387,1,6,23,468,310,0,0,2,'2013-02-09 20:18:28'),(388,1,6,23,349,230,0,0,2,'2013-02-09 20:18:29'),(389,1,6,23,353,263,0,0,2,'2013-02-09 20:18:31'),(390,1,6,24,360,263,0,0,2,'2013-02-09 20:18:31'),(391,1,6,24,414,263,0,0,2,'2013-02-09 20:18:32'),(392,1,6,24,439,263,0,0,2,'2013-02-09 20:18:33'),(393,1,6,24,475,263,0,0,2,'2013-02-09 20:18:33'),(394,1,6,25,443,273,0,0,2,'2013-02-09 20:18:34'),(395,1,1,25,621,582,0,0,1,'2013-02-09 20:18:42'),(396,1,6,25,605,535,0,0,2,'2013-02-09 20:18:50'),(397,1,6,25,565,535,0,0,2,'2013-02-09 20:18:51'),(398,1,6,26,472,535,0,0,2,'2013-02-09 20:18:51'),(399,1,6,26,555,498,0,0,2,'2013-02-09 20:18:58'),(400,1,6,26,616,516,0,0,2,'2013-02-09 20:18:59'),(401,1,6,26,616,531,0,0,2,'2013-02-09 20:18:59'),(402,1,6,27,616,531,0,0,2,'2013-02-09 20:19:00'),(403,1,6,27,580,531,0,0,2,'2013-02-09 20:19:01'),(404,1,6,27,497,545,0,0,2,'2013-02-09 20:19:01'),(405,1,1,28,625,596,0,0,1,'2013-02-09 20:20:46'),(406,1,1,28,585,596,0,0,1,'2013-02-09 20:20:48'),(407,1,1,28,693,596,0,0,1,'2013-02-09 20:20:49'),(408,1,1,28,671,617,0,0,1,'2013-02-09 20:21:01'),(409,1,6,28,634,610,0,0,2,'2013-02-09 20:21:21'),(410,1,6,29,605,581,0,0,2,'2013-02-09 20:21:22'),(411,1,6,29,436,581,0,0,2,'2013-02-09 20:21:23'),(412,1,6,29,544,581,0,0,2,'2013-02-09 20:21:24'),(413,1,6,30,526,581,0,0,2,'2013-02-09 20:21:25'),(414,1,6,30,512,581,0,0,2,'2013-02-09 20:21:26'),(415,1,6,30,533,581,0,0,2,'2013-02-09 20:21:27'),(416,1,6,30,547,556,0,0,2,'2013-02-09 20:21:28'),(417,1,6,31,479,578,0,0,2,'2013-02-09 20:21:31'),(418,1,1,31,747,585,0,0,1,'2013-02-09 20:21:41'),(419,1,6,31,458,499,0,0,2,'2013-02-09 20:21:46'),(420,1,6,31,458,470,0,0,2,'2013-02-09 20:21:47'),(421,1,6,32,407,470,0,0,2,'2013-02-09 20:21:47'),(422,1,1,32,873,463,0,0,1,'2013-02-09 20:21:52'),(423,1,1,32,681,585,0,0,1,'2013-02-09 20:21:56'),(424,1,1,33,562,585,0,0,1,'2013-02-09 20:22:07'),(425,1,1,33,512,585,0,0,1,'2013-02-09 20:22:07'),(426,1,1,33,624,585,0,0,1,'2013-02-09 20:22:09'),(427,1,1,28,850,577,0,0,1,'2013-02-09 20:29:11'),(428,1,1,28,803,577,0,0,1,'2013-02-09 20:30:06'),(429,1,6,28,497,506,0,0,2,'2013-02-09 20:33:05'),(430,1,6,28,429,520,0,0,2,'2013-02-09 20:33:06'),(431,1,6,29,476,495,0,0,2,'2013-02-09 20:33:06'),(432,1,6,29,371,495,0,0,2,'2013-02-09 20:33:07'),(433,1,6,29,364,495,0,0,2,'2013-02-09 20:33:08'),(434,1,6,29,540,614,0,0,2,'2013-02-09 20:33:09'),(435,1,6,30,497,614,0,0,2,'2013-02-09 20:33:09'),(436,1,6,30,429,578,0,0,2,'2013-02-09 20:33:10'),(437,1,6,30,468,560,0,0,2,'2013-02-09 20:33:10'),(438,1,6,31,407,614,0,0,2,'2013-02-09 20:33:13'),(439,1,6,31,417,585,0,0,2,'2013-02-09 20:33:14'),(440,1,6,31,265,578,0,0,2,'2013-02-09 20:33:15'),(441,1,1,32,760,614,0,0,1,'2013-02-09 20:33:28'),(442,1,6,28,483,542,0,0,2,'2013-02-09 20:36:37'),(443,1,6,28,306,445,0,0,2,'2013-02-09 20:36:38'),(444,1,6,28,558,391,0,0,2,'2013-02-09 20:36:40'),(445,1,6,29,623,589,0,0,2,'2013-02-09 20:36:41'),(446,1,6,29,504,589,0,0,2,'2013-02-09 20:36:42'),(447,1,6,29,436,589,0,0,2,'2013-02-09 20:36:43'),(448,1,6,29,558,571,0,0,2,'2013-02-09 20:36:43'),(449,1,6,30,542,574,0,0,2,'2013-02-09 20:36:46'),(450,1,6,30,502,499,0,0,2,'2013-02-09 20:36:47'),(451,1,6,30,495,531,0,0,2,'2013-02-09 20:36:48'),(452,1,6,30,517,531,0,0,2,'2013-02-09 20:36:49'),(453,1,6,31,517,531,0,0,2,'2013-02-09 20:36:49'),(454,1,6,31,517,567,0,0,2,'2013-02-09 20:36:51'),(455,1,6,31,646,477,0,0,2,'2013-02-09 20:36:52'),(456,1,6,32,686,520,0,0,2,'2013-02-09 20:36:53'),(457,1,6,32,686,520,0,0,2,'2013-02-09 20:36:54'),(458,1,6,32,747,520,0,0,2,'2013-02-09 20:36:55'),(459,1,6,32,747,488,0,0,2,'2013-02-09 20:36:55'),(460,1,6,33,747,477,0,0,2,'2013-02-09 20:36:56'),(461,1,6,33,790,401,0,0,2,'2013-02-09 20:36:57'),(462,1,6,33,664,517,0,0,2,'2013-02-09 20:36:59'),(463,1,6,33,574,517,0,0,2,'2013-02-09 20:37:00'),(464,1,1,34,906,470,0,0,1,'2013-02-09 20:37:10'),(465,1,1,34,650,531,0,0,1,'2013-02-09 20:37:12'),(466,1,1,28,502,596,0,0,1,'2013-02-09 21:03:53'),(467,1,1,28,642,596,0,0,1,'2013-02-09 21:04:17'),(468,1,1,28,617,606,0,0,1,'2013-02-09 21:04:32'),(469,1,1,28,563,667,0,0,1,'2013-02-09 21:04:34'),(470,1,1,28,625,596,0,0,1,'2013-02-09 21:06:05'),(471,1,1,28,599,596,0,0,1,'2013-02-09 21:06:06'),(472,1,1,28,632,596,0,0,1,'2013-02-09 21:06:07'),(473,1,6,28,458,434,0,0,2,'2013-02-09 21:06:21'),(474,1,6,29,458,319,0,0,2,'2013-02-09 21:06:21'),(475,1,6,29,501,268,0,0,2,'2013-02-09 21:06:22'),(476,1,6,29,497,268,0,0,2,'2013-02-09 21:06:23'),(477,1,6,29,522,347,0,0,2,'2013-02-09 21:06:25'),(478,1,6,30,515,311,0,0,2,'2013-02-09 21:06:26'),(479,1,6,30,515,329,0,0,2,'2013-02-09 21:06:30'),(480,1,6,30,512,329,0,0,2,'2013-02-09 21:06:31'),(481,1,6,30,512,329,0,0,2,'2013-02-09 21:06:31'),(482,1,6,31,547,329,0,0,2,'2013-02-09 21:06:32'),(483,1,6,31,594,556,0,0,2,'2013-02-09 21:06:34'),(484,1,6,31,652,603,0,0,2,'2013-02-09 21:06:35'),(485,1,6,32,652,578,0,0,2,'2013-02-09 21:06:35'),(486,1,6,32,598,578,0,0,2,'2013-02-09 21:06:37'),(487,1,6,32,598,549,0,0,2,'2013-02-09 21:06:37'),(488,1,6,32,580,549,0,0,2,'2013-02-09 21:06:38'),(489,1,6,33,605,549,0,0,2,'2013-02-09 21:06:38'),(490,1,1,33,960,409,0,0,1,'2013-02-09 21:06:44'),(491,1,1,33,621,578,0,0,1,'2013-02-09 21:10:35'),(492,1,6,34,429,495,0,0,2,'2013-02-09 21:10:57'),(493,1,6,34,281,495,0,0,2,'2013-02-09 21:10:59'),(494,1,6,34,375,495,0,0,2,'2013-02-09 21:10:59'),(495,1,6,34,519,581,0,0,2,'2013-02-09 21:11:00'),(496,1,6,35,519,614,0,0,2,'2013-02-09 21:11:01'),(497,1,6,35,497,495,0,0,2,'2013-02-09 21:11:02'),(498,1,6,35,432,538,0,0,2,'2013-02-09 21:11:02'),(499,1,6,35,472,599,0,0,2,'2013-02-09 21:11:04'),(500,1,6,36,411,567,0,0,2,'2013-02-09 21:11:05'),(501,1,6,36,396,556,0,0,2,'2013-02-09 21:11:06'),(502,1,6,36,454,556,0,0,2,'2013-02-09 21:11:07'),(503,1,6,36,443,556,0,0,2,'2013-02-09 21:11:08'),(504,1,6,37,357,556,0,0,2,'2013-02-09 21:11:09'),(505,1,6,37,245,556,0,0,2,'2013-02-09 21:11:09'),(506,1,6,37,335,556,0,0,2,'2013-02-09 21:11:10'),(507,1,1,38,726,556,0,0,1,'2013-02-09 21:11:15'),(508,1,1,38,610,585,0,0,1,'2013-02-09 21:11:16'),(509,1,1,33,343,578,0,0,1,'2013-02-09 21:36:33'),(510,1,1,34,1620,1325,0,0,1,'2013-02-09 21:43:44'),(511,1,1,34,1620,1325,0,0,1,'2013-02-09 21:43:45'),(512,1,1,34,1620,1209,0,0,1,'2013-02-09 21:43:45'),(513,1,1,34,512,512,0,0,1,'2013-02-10 01:05:31'),(514,1,1,34,310,526,0,0,1,'2013-02-10 01:06:37'),(515,6,1,1,576,519,0,0,1,'2013-02-10 02:18:09'),(516,1,1,34,526,583,0,0,1,'2013-02-10 18:35:32'),(517,1,1,34,512,512,0,0,1,'2013-02-10 19:05:22'),(518,1,1,35,562,512,0,0,1,'2013-02-10 19:05:22'),(519,1,1,35,943,145,0,0,1,'2013-02-10 19:05:34'),(520,1,1,34,512,512,0,0,1,'2013-02-10 19:06:38'),(521,1,1,35,713,512,0,0,1,'2013-02-10 19:06:39'),(522,1,6,34,529,466,0,0,2,'2013-02-10 19:18:50'),(523,1,6,35,748,527,0,0,2,'2013-02-10 19:18:56'),(524,1,6,35,726,462,0,0,2,'2013-02-10 19:19:08'),(525,1,6,35,431,528,0,0,2,'2013-02-10 19:20:37'),(526,1,6,35,431,528,0,0,2,'2013-02-10 19:20:37'),(527,1,6,36,431,514,0,0,2,'2013-02-10 19:20:43'),(528,1,6,36,431,435,0,0,2,'2013-02-10 19:20:44'),(529,1,6,36,522,545,0,0,2,'2013-02-10 19:21:07'),(530,1,6,36,522,545,0,0,2,'2013-02-10 19:21:08'),(531,1,6,37,562,545,0,0,2,'2013-02-10 19:21:08'),(532,1,6,37,562,517,0,0,2,'2013-02-10 19:21:09'),(533,1,6,37,583,484,0,0,2,'2013-02-10 19:21:11'),(534,1,6,38,583,459,0,0,2,'2013-02-10 19:21:11'),(535,1,6,38,583,459,0,0,2,'2013-02-10 19:21:12'),(536,1,6,38,583,491,0,0,2,'2013-02-10 19:21:12'),(537,1,6,38,562,535,0,0,2,'2013-02-10 19:21:14'),(538,1,1,34,512,512,0,0,1,'2013-02-10 19:24:04'),(539,1,1,35,789,512,0,0,1,'2013-02-10 19:26:09'),(540,1,1,35,677,512,0,0,1,'2013-02-10 19:26:10'),(541,1,1,35,512,512,0,0,1,'2013-02-10 19:28:21'),(542,1,1,35,512,512,0,0,1,'2013-02-10 19:28:21'),(543,1,1,35,526,457,0,0,1,'2013-02-10 19:28:23'),(544,1,1,35,626,573,0,0,1,'2013-02-10 19:28:34'),(545,1,6,36,417,592,0,0,2,'2013-02-10 19:29:14'),(546,1,6,36,489,477,0,0,2,'2013-02-10 19:29:15'),(547,1,6,36,435,589,0,0,2,'2013-02-10 19:29:16'),(548,1,6,36,417,535,0,0,2,'2013-02-10 19:29:19'),(549,1,6,37,392,481,0,0,2,'2013-02-10 19:29:20'),(550,1,6,37,320,481,0,0,2,'2013-02-10 19:29:21'),(551,1,6,37,417,481,0,0,2,'2013-02-10 19:29:22'),(552,1,6,37,432,481,0,0,2,'2013-02-10 19:29:24'),(553,1,6,38,432,481,0,0,2,'2013-02-10 19:29:24'),(554,1,6,38,374,481,0,0,2,'2013-02-10 19:29:25'),(555,1,6,38,396,420,0,0,2,'2013-02-10 19:29:26'),(556,1,6,38,396,237,0,0,2,'2013-02-10 19:29:27'),(557,1,1,39,515,681,0,0,1,'2013-02-10 19:29:35'),(558,1,1,39,637,879,0,0,1,'2013-02-10 19:29:38'),(559,1,1,39,521,390,0,0,1,'2013-02-10 19:30:09'),(560,1,1,39,715,548,0,0,1,'2013-02-10 19:30:10'),(561,1,1,39,730,602,0,0,1,'2013-02-10 19:30:24'),(562,1,1,40,730,602,0,0,1,'2013-02-10 19:30:50'),(563,1,1,40,648,512,0,0,1,'2013-02-10 19:30:51'),(564,1,1,40,256,612,0,0,1,'2013-02-10 19:31:59'),(565,1,6,40,519,434,0,0,2,'2013-02-10 19:35:52'),(566,1,6,40,483,551,0,0,2,'2013-02-10 19:35:55'),(567,1,6,41,605,511,0,0,2,'2013-02-10 19:35:55'),(568,1,1,34,281,519,0,0,1,'2013-02-10 19:41:31'),(569,1,1,35,324,519,0,0,1,'2013-02-10 19:41:32'),(570,1,1,35,454,562,0,0,1,'2013-02-10 19:41:45'),(571,1,1,35,576,562,0,0,1,'2013-02-10 19:41:46'),(572,1,1,34,450,659,0,0,1,'2013-02-10 19:45:37'),(573,1,6,35,360,351,0,0,2,'2013-02-10 19:45:57'),(574,1,6,35,360,351,0,0,2,'2013-02-10 19:45:57'),(575,1,6,35,587,488,0,0,2,'2013-02-10 19:45:59'),(576,1,1,35,634,540,0,0,1,'2013-02-10 19:46:24'),(577,1,1,35,529,540,0,0,1,'2013-02-10 19:46:25'),(578,1,1,36,742,595,0,0,1,'2013-02-10 19:46:39'),(579,1,1,36,673,465,0,0,1,'2013-02-10 19:47:13'),(580,1,1,34,565,555,0,0,1,'2013-02-10 20:50:43'),(581,1,1,35,753,805,0,0,1,'2013-02-10 20:51:00'),(582,1,1,35,397,1374,0,0,1,'2013-02-10 20:51:05'),(583,1,1,35,498,1374,0,0,1,'2013-02-10 20:51:06'),(584,1,1,34,1001,495,0,0,1,'2013-02-10 20:52:38'),(585,1,6,34,537,405,0,0,2,'2013-02-10 20:55:07'),(586,1,6,35,476,560,0,0,2,'2013-02-10 20:55:13'),(587,1,6,35,461,578,0,0,2,'2013-02-10 20:55:14'),(588,1,6,35,461,553,0,0,2,'2013-02-10 20:55:16'),(589,1,6,35,501,553,0,0,2,'2013-02-10 20:55:17'),(590,1,6,36,349,499,0,0,2,'2013-02-10 20:55:18'),(591,1,6,36,191,517,0,0,2,'2013-02-10 20:55:20'),(592,1,6,36,321,491,0,0,2,'2013-02-10 20:55:22'),(593,1,6,36,349,470,0,0,2,'2013-02-10 20:55:23'),(594,1,6,37,360,470,0,0,2,'2013-02-10 20:55:23'),(595,1,6,37,385,419,0,0,2,'2013-02-10 20:55:25'),(596,1,6,37,439,448,0,0,2,'2013-02-10 20:55:26'),(597,1,1,38,490,512,0,0,1,'2013-02-10 20:55:34'),(598,1,1,38,573,619,0,0,1,'2013-02-10 20:55:37'),(599,1,1,38,972,627,0,0,1,'2013-02-10 20:55:49'),(600,1,1,38,703,1408,0,0,1,'2013-02-10 20:55:54'),(601,1,1,38,491,1527,0,0,1,'2013-02-10 20:55:55'),(602,1,1,38,1387,717,0,0,1,'2013-02-10 20:56:06'),(603,1,1,39,1538,623,0,0,1,'2013-02-10 20:56:07'),(604,1,1,39,1159,785,0,0,1,'2013-02-10 20:56:13'),(605,7,1,1,357,583,0,0,1,'2018-12-18 20:08:40'),(606,7,1,1,508,681,0,0,1,'2018-12-18 20:10:03'),(607,7,1,1,508,681,0,0,1,'2018-12-18 20:10:06'),(608,8,1,1,1114,814,0,0,1,'2018-12-18 20:18:14'),(609,8,1,1,1114,814,0,0,1,'2018-12-18 20:18:17'),(610,9,1,1,512,512,0,0,1,'2018-12-18 20:26:06'),(611,9,1,1,512,512,0,0,1,'2018-12-18 20:26:09'),(612,9,1,1,961,1491,0,0,1,'2018-12-18 20:26:21'),(613,9,1,1,1215,529,0,0,1,'2018-12-18 20:26:32'),(614,9,1,1,891,805,0,0,1,'2018-12-18 20:26:53'),(615,9,1,1,891,855,0,0,1,'2018-12-18 20:26:54'),(616,9,1,2,851,855,0,0,1,'2018-12-18 20:26:55'),(617,10,1,1,490,529,0,0,1,'2018-12-18 20:30:56'),(618,10,1,1,479,529,0,0,1,'2018-12-18 20:30:59'),(619,10,1,1,403,907,0,0,1,'2018-12-18 20:31:15'),(620,10,1,1,637,828,0,0,1,'2018-12-18 20:31:24'),(621,10,1,1,998,1010,0,0,1,'2018-12-18 20:33:09'),(622,10,1,1,332,1305,0,0,1,'2018-12-18 20:33:27'),(623,10,1,2,656,960,0,0,1,'2018-12-18 20:33:48');
/*!40000 ALTER TABLE `world_stats_user_kill_npc` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_user_level`
--

DROP TABLE IF EXISTS `world_stats_user_level`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `world_stats_user_level` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `character_id` int(11) NOT NULL COMMENT 'The ID of the character that leveled up.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `level` smallint(6) NOT NULL COMMENT 'The level that the character leveled up to (their new level).',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=183 DEFAULT CHARSET=latin1 COMMENT='Event log: User levels up.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `world_stats_user_level`
--

LOCK TABLES `world_stats_user_level` WRITE;
/*!40000 ALTER TABLE `world_stats_user_level` DISABLE KEYS */;
INSERT INTO `world_stats_user_level` VALUES (1,1,1,489,1226,2,'2013-02-04 22:52:00'),(2,1,1,347,545,3,'2013-02-04 22:53:00'),(3,1,1,518,625,4,'2013-02-04 23:40:30'),(4,1,1,106,656,5,'2013-02-04 23:56:30'),(5,1,1,285,481,5,'2013-02-05 00:02:44'),(6,1,1,285,703,5,'2013-02-05 00:05:01'),(7,1,1,248,591,5,'2013-02-05 01:10:01'),(8,1,1,547,463,6,'2013-02-05 01:21:47'),(9,1,1,449,227,7,'2013-02-05 01:23:02'),(10,1,1,587,490,8,'2013-02-05 02:13:23'),(11,1,1,738,605,9,'2013-02-05 02:13:57'),(12,1,1,630,1073,10,'2013-02-05 02:14:42'),(13,1,1,400,598,8,'2013-02-05 02:29:21'),(14,1,1,436,893,8,'2013-02-05 04:11:07'),(15,1,1,192,1099,9,'2013-02-05 04:11:45'),(16,1,1,679,752,10,'2013-02-05 04:20:47'),(17,1,1,1242,726,11,'2013-02-05 04:22:30'),(18,1,1,414,818,8,'2013-02-05 04:23:50'),(19,1,2,468,290,9,'2013-02-05 04:25:09'),(20,1,1,514,359,10,'2013-02-05 04:26:32'),(21,1,1,1235,743,11,'2013-02-05 04:27:35'),(22,1,1,373,485,12,'2013-02-05 04:32:31'),(23,1,1,1472,705,13,'2013-02-05 04:33:46'),(24,1,2,413,438,12,'2013-02-05 04:48:37'),(25,1,2,604,611,13,'2013-02-05 04:48:54'),(26,1,2,596,611,14,'2013-02-05 04:49:04'),(27,1,1,249,1074,15,'2013-02-05 04:49:43'),(28,1,1,824,666,16,'2013-02-05 04:50:16'),(29,1,1,960,517,17,'2013-02-06 04:29:38'),(30,1,1,586,672,18,'2013-02-06 04:34:32'),(31,1,1,417,560,19,'2013-02-06 04:38:08'),(32,1,1,1004,917,20,'2013-02-06 04:38:23'),(33,1,1,1069,946,21,'2013-02-06 04:38:39'),(34,1,1,1584,294,22,'2013-02-06 05:08:26'),(35,1,3,262,255,22,'2013-02-06 05:47:55'),(36,1,3,262,255,22,'2013-02-06 05:48:10'),(37,1,1,1505,416,23,'2013-02-07 05:28:03'),(38,1,1,1630,419,23,'2013-02-07 05:29:29'),(39,1,1,1505,970,23,'2013-02-07 05:35:57'),(40,1,1,799,811,24,'2013-02-07 05:37:32'),(41,1,1,965,764,25,'2013-02-07 05:38:37'),(42,1,1,149,1410,26,'2013-02-07 05:39:36'),(43,1,1,1533,739,27,'2013-02-07 05:40:00'),(44,1,2,323,211,28,'2013-02-07 05:40:45'),(45,1,2,392,471,29,'2013-02-07 05:41:00'),(46,1,1,1199,317,30,'2013-02-07 05:45:05'),(47,1,2,403,427,31,'2013-02-07 05:45:25'),(48,1,2,414,545,32,'2013-02-07 05:45:55'),(49,1,2,555,545,33,'2013-02-07 05:46:00'),(50,1,2,555,545,34,'2013-02-07 05:46:04'),(51,1,2,512,571,35,'2013-02-07 05:46:14'),(52,1,2,814,499,36,'2013-02-07 05:46:20'),(53,1,2,681,542,37,'2013-02-07 05:46:24'),(54,1,2,688,283,38,'2013-02-07 05:46:28'),(55,1,2,591,250,39,'2013-02-07 05:46:34'),(56,1,2,263,236,40,'2013-02-07 05:46:38'),(57,1,1,630,621,41,'2013-02-07 05:47:06'),(58,1,1,1641,916,23,'2013-02-08 05:05:19'),(59,1,1,659,642,23,'2013-02-08 05:08:10'),(60,1,1,950,826,23,'2013-02-08 05:13:18'),(61,1,2,439,387,24,'2013-02-08 05:14:18'),(62,1,2,468,358,25,'2013-02-08 05:14:21'),(63,1,2,457,369,26,'2013-02-08 05:14:23'),(64,1,1,423,700,27,'2013-02-08 05:14:41'),(65,1,1,666,458,23,'2013-02-08 05:18:29'),(66,1,1,1037,743,23,'2013-02-08 05:25:16'),(67,1,1,914,714,23,'2013-02-08 05:26:52'),(68,1,1,1505,606,23,'2013-02-08 05:28:02'),(69,1,1,1382,729,23,'2013-02-08 05:29:27'),(70,1,1,583,473,23,'2013-02-08 05:30:38'),(71,1,1,1274,707,23,'2013-02-08 05:32:16'),(72,1,1,896,686,23,'2013-02-08 05:56:44'),(73,1,1,713,725,23,'2013-02-08 06:01:44'),(74,1,1,285,965,24,'2013-02-08 15:39:15'),(75,1,1,761,1756,25,'2013-02-08 15:42:08'),(76,1,1,626,572,24,'2013-02-09 20:02:52'),(77,1,1,626,572,25,'2013-02-09 20:02:52'),(78,1,1,626,572,26,'2013-02-09 20:02:52'),(79,1,1,626,572,27,'2013-02-09 20:02:52'),(80,1,1,626,572,28,'2013-02-09 20:02:52'),(81,1,1,626,572,29,'2013-02-09 20:02:52'),(82,1,1,626,572,30,'2013-02-09 20:02:52'),(83,1,1,626,572,31,'2013-02-09 20:02:52'),(84,1,1,626,572,32,'2013-02-09 20:02:52'),(85,1,1,626,572,33,'2013-02-09 20:02:52'),(86,1,1,626,572,34,'2013-02-09 20:02:52'),(87,1,1,626,572,35,'2013-02-09 20:02:52'),(88,1,1,626,572,36,'2013-02-09 20:02:52'),(89,1,1,626,572,37,'2013-02-09 20:02:52'),(90,1,1,626,572,38,'2013-02-09 20:02:52'),(91,1,1,626,572,39,'2013-02-09 20:02:52'),(92,1,1,626,572,40,'2013-02-09 20:02:52'),(93,1,1,626,572,41,'2013-02-09 20:02:52'),(94,1,1,626,572,42,'2013-02-09 20:02:52'),(95,1,1,626,572,43,'2013-02-09 20:02:52'),(96,1,1,626,572,44,'2013-02-09 20:02:52'),(97,1,1,626,572,45,'2013-02-09 20:02:52'),(98,1,1,626,572,46,'2013-02-09 20:02:52'),(99,1,1,626,572,47,'2013-02-09 20:02:52'),(100,1,1,626,572,48,'2013-02-09 20:02:52'),(101,1,1,626,572,49,'2013-02-09 20:02:52'),(102,1,1,626,572,50,'2013-02-09 20:02:52'),(103,1,1,626,572,51,'2013-02-09 20:02:52'),(104,1,1,626,572,52,'2013-02-09 20:02:52'),(105,1,1,626,572,53,'2013-02-09 20:02:52'),(106,1,1,626,572,54,'2013-02-09 20:02:52'),(107,1,1,626,572,55,'2013-02-09 20:02:52'),(108,1,1,626,572,56,'2013-02-09 20:02:52'),(109,1,1,626,572,57,'2013-02-09 20:02:52'),(110,1,2,547,553,24,'2013-02-09 20:07:20'),(111,1,2,512,553,25,'2013-02-09 20:07:28'),(112,1,2,645,599,26,'2013-02-09 20:07:38'),(113,1,1,628,578,27,'2013-02-09 20:07:52'),(114,1,2,497,437,24,'2013-02-09 20:09:57'),(115,1,2,497,437,25,'2013-02-09 20:10:01'),(116,1,1,753,643,26,'2013-02-09 20:10:36'),(117,1,2,339,592,27,'2013-02-09 20:10:43'),(118,1,2,238,506,28,'2013-02-09 20:10:47'),(119,1,2,353,263,24,'2013-02-09 20:18:31'),(120,1,2,475,263,25,'2013-02-09 20:18:33'),(121,1,2,565,535,26,'2013-02-09 20:18:51'),(122,1,2,616,531,27,'2013-02-09 20:18:59'),(123,1,1,625,596,28,'2013-02-09 20:19:16'),(124,1,2,634,610,29,'2013-02-09 20:21:21'),(125,1,2,544,581,30,'2013-02-09 20:21:24'),(126,1,2,547,556,31,'2013-02-09 20:21:28'),(127,1,2,458,470,32,'2013-02-09 20:21:47'),(128,1,1,562,585,33,'2013-02-09 20:22:00'),(129,1,2,429,520,29,'2013-02-09 20:33:06'),(130,1,2,540,614,30,'2013-02-09 20:33:09'),(131,1,2,468,560,31,'2013-02-09 20:33:10'),(132,1,1,655,614,32,'2013-02-09 20:33:25'),(133,1,2,558,391,29,'2013-02-09 20:36:40'),(134,1,2,558,571,30,'2013-02-09 20:36:43'),(135,1,2,517,531,31,'2013-02-09 20:36:49'),(136,1,2,646,477,32,'2013-02-09 20:36:52'),(137,1,2,747,488,33,'2013-02-09 20:36:55'),(138,1,2,574,517,34,'2013-02-09 20:37:00'),(139,1,1,563,667,29,'2013-02-09 21:04:34'),(140,1,2,458,434,29,'2013-02-09 21:06:21'),(141,1,2,522,347,30,'2013-02-09 21:06:25'),(142,1,2,512,329,31,'2013-02-09 21:06:31'),(143,1,2,652,603,32,'2013-02-09 21:06:35'),(144,1,2,580,549,33,'2013-02-09 21:06:38'),(145,1,1,621,578,34,'2013-02-09 21:10:35'),(146,1,2,519,581,35,'2013-02-09 21:11:00'),(147,1,2,472,599,36,'2013-02-09 21:11:04'),(148,1,2,443,556,37,'2013-02-09 21:11:08'),(149,1,2,335,556,38,'2013-02-09 21:11:10'),(150,1,1,343,578,34,'2013-02-09 21:36:33'),(151,1,1,526,583,35,'2013-02-10 18:35:32'),(152,1,1,512,512,35,'2013-02-10 19:05:22'),(153,1,1,512,512,35,'2013-02-10 19:06:38'),(154,1,2,529,466,35,'2013-02-10 19:18:50'),(155,1,2,431,528,36,'2013-02-10 19:20:37'),(156,1,2,522,545,37,'2013-02-10 19:21:08'),(157,1,2,583,484,38,'2013-02-10 19:21:11'),(158,1,2,562,535,39,'2013-02-10 19:21:14'),(159,1,1,512,512,35,'2013-02-10 19:24:04'),(160,1,1,626,573,36,'2013-02-10 19:28:34'),(161,1,2,417,535,37,'2013-02-10 19:29:19'),(162,1,2,432,481,38,'2013-02-10 19:29:24'),(163,1,2,396,237,39,'2013-02-10 19:29:27'),(164,1,1,730,602,40,'2013-02-10 19:30:24'),(165,1,2,483,551,41,'2013-02-10 19:35:55'),(166,1,4,657,434,35,'2013-02-10 19:37:26'),(167,1,1,281,519,35,'2013-02-10 19:41:31'),(168,1,1,450,659,35,'2013-02-10 19:45:37'),(169,1,1,529,540,36,'2013-02-10 19:46:25'),(170,1,1,565,555,35,'2013-02-10 20:50:43'),(171,1,1,1001,495,35,'2013-02-10 20:52:38'),(172,1,2,537,405,35,'2013-02-10 20:55:07'),(173,1,2,501,553,36,'2013-02-10 20:55:17'),(174,1,2,349,470,37,'2013-02-10 20:55:23'),(175,1,2,439,448,38,'2013-02-10 20:55:26'),(176,1,1,1387,717,39,'2013-02-10 20:56:06'),(177,7,3,255,255,1,'2018-12-18 20:09:32'),(178,9,1,891,855,2,'2018-12-18 20:26:54'),(179,10,3,259,255,1,'2018-12-18 20:32:25'),(180,10,3,259,255,1,'2018-12-18 20:32:28'),(181,10,3,259,255,1,'2018-12-18 20:32:31'),(182,10,1,332,1305,2,'2018-12-18 20:33:27');
/*!40000 ALTER TABLE `world_stats_user_level` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `world_stats_user_shopping`
--

DROP TABLE IF EXISTS `world_stats_user_shopping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `create_user_on_account`(accountName VARCHAR(50), characterName VARCHAR(30)) RETURNS varchar(100) CHARSET latin1
BEGIN				DECLARE character_count INT DEFAULT 0;		DECLARE max_character_count INT DEFAULT 9;		DECLARE is_name_free INT DEFAULT 0;		DECLARE errorMsg VARCHAR(100) DEFAULT "";		DECLARE accountID INT DEFAULT NULL;		DECLARE charID INT DEFAULT 0;		SELECT `id` INTO accountID FROM `account` WHERE `name` = accountName;		IF ISNULL(accountID) THEN			SET errorMsg = "Account with the specified name does not exist.";		ELSE			SELECT COUNT(*) INTO character_count FROM `account_character` WHERE `account_id` = accountID AND ISNULL(time_deleted);			IF character_count > max_character_count THEN				SET errorMsg = "No free character slots available in the account.";			ELSE				SELECT COUNT(*) INTO is_name_free FROM `character` WHERE `name` = characterName LIMIT 1;				IF is_name_free > 0 THEN					SET errorMsg = "The specified character name is not available for use.";				ELSE					INSERT INTO `character` SET `name`	= characterName;					SET charID = LAST_INSERT_ID();					INSERT INTO `account_character` SET `character_id` = charID, `account_id` = accountID;				END IF;			END IF;		END IF;						RETURN errorMsg;  END ;;
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
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `ft_banning_isbanned`(accountID INT) RETURNS int(11)
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
END ;;
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
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `delete_user_on_account`(characterID INT)
BEGIN
	UPDATE `account_character`
		SET `time_deleted` = NOW()
		WHERE `character_id` = characterID
		AND `time_deleted` IS NULL;
	UPDATE `character`
		SET `name` = CONCAT('~',`id`,'_',name)
		WHERE `id` = characterID
			AND SUBSTR(`name`, 1) != '~';
END ;;
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
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `find_foreign_keys`(tableSchema VARCHAR(100), tableName VARCHAR(100), columnName VARCHAR(100))
BEGIN
		SELECT `TABLE_SCHEMA`, `TABLE_NAME`, `COLUMN_NAME`
			FROM information_schema.KEY_COLUMN_USAGE
			WHERE `REFERENCED_TABLE_SCHEMA` = tableSchema
				AND `REFERENCED_TABLE_NAME` = tableName
				AND `REFERENCED_COLUMN_NAME` = columnName;
END ;;
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
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `ft_banning_get_reasons`(accountID INT)
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
		
END ;;
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
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `ft_banning_update_expired`()
BEGIN
		DECLARE tnow TIMESTAMP;
		
		SET tnow = NOW();
		
		UPDATE `account_ban`
			SET `expired` = 1
			WHERE `expired` = 0
				AND `end_time` <= tnow;
END ;;
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
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rebuild_views`()
BEGIN
	
	CALL rebuild_view_npc_character();
	CALL rebuild_view_user_character();
    
END ;;
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
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rebuild_view_npc_character`()
BEGIN
	
	DROP VIEW IF EXISTS `view_npc_character`;
	CREATE ALGORITHM=UNDEFINED DEFINER=CURRENT_USER SQL SECURITY DEFINER VIEW `view_npc_character` AS SELECT c.*  FROM `character` c LEFT JOIN `account_character` a ON c.id = a.character_id WHERE a.account_id IS NULL;
    
END ;;
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
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rebuild_view_user_character`()
BEGIN
	
	DROP VIEW IF EXISTS `view_user_character`;
	CREATE ALGORITHM=UNDEFINED DEFINER=CURRENT_USER SQL SECURITY DEFINER VIEW `view_user_character` AS SELECT c.* FROM `character` c INNER JOIN `account_character` a ON c.id = a.character_id WHERE a.time_deleted IS NULL;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `view_npc_character`
--

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

-- Dump completed on 2018-12-18 16:47:43
