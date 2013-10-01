/*
SQLyog Community v10.5 Beta1
MySQL - 5.5.28 : Database - demogame
*********************************************************************
*/


/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`demogame` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `demogame`;

/*Table structure for table `account` */

DROP TABLE IF EXISTS `account`;

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

/*Data for the table `account` */

insert  into `account`(`id`,`name`,`password`,`email`,`permissions`,`time_created`,`time_last_login`,`creator_ip`,`current_ip`) values (1,'Spodi','3fc0a7acf087f549ac2b266baf94b8b1','spodi@netgore.com',255,'2009-09-07 15:43:16','2013-02-10 15:54:45',16777343,16777343);

/*Table structure for table `account_ban` */

DROP TABLE IF EXISTS `account_ban`;

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

/*Data for the table `account_ban` */

/*Table structure for table `account_character` */

DROP TABLE IF EXISTS `account_character`;

CREATE TABLE `account_character` (
  `character_id` int(11) NOT NULL COMMENT 'The character in the account.',
  `account_id` int(11) NOT NULL COMMENT 'The account the character is on.',
  `time_deleted` datetime DEFAULT NULL COMMENT 'When the character was removed from the account (NULL if not removed).',
  PRIMARY KEY (`character_id`),
  KEY `account_id` (`account_id`),
  CONSTRAINT `account_character_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `account_character_ibfk_2` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Links account to many characters. Retains deleted linkages.';

/*Data for the table `account_character` */

insert  into `account_character`(`character_id`,`account_id`,`time_deleted`) values (1,1,NULL),(6,3,NULL);

/*Table structure for table `account_ips` */

DROP TABLE IF EXISTS `account_ips`;

CREATE TABLE `account_ips` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'The unique row ID.',
  `account_id` int(11) NOT NULL COMMENT 'The ID of the account.',
  `ip` int(10) unsigned NOT NULL COMMENT 'The IP that logged into the account.',
  `time` datetime NOT NULL COMMENT 'When this IP last logged into this account.',
  PRIMARY KEY (`id`),
  KEY `account_id` (`account_id`),
  CONSTRAINT `account_ips_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=265 DEFAULT CHARSET=latin1 COMMENT='The IPs used to access accounts.';

/*Data for the table `account_ips` */

insert  into `account_ips`(`id`,`account_id`,`ip`,`time`) values (2,1,16777343,'2013-02-04 17:51:11'),(3,1,16777343,'2013-02-04 18:40:11'),(4,1,16777343,'2013-02-04 18:44:04'),(5,1,16777343,'2013-02-04 18:53:39'),(6,3,16777343,'2013-02-04 18:54:54'),(7,3,16777343,'2013-02-04 18:56:44'),(8,1,16777343,'2013-02-04 18:58:18'),(9,1,16777343,'2013-02-04 19:00:04'),(10,1,16777343,'2013-02-04 19:02:34'),(11,1,16777343,'2013-02-04 19:04:45'),(12,1,16777343,'2013-02-04 19:09:20'),(13,3,16777343,'2013-02-04 19:09:24'),(14,3,16777343,'2013-02-04 19:09:56'),(15,3,16777343,'2013-02-04 19:10:06'),(16,1,16777343,'2013-02-04 19:10:27'),(17,1,16777343,'2013-02-04 19:27:25'),(18,1,16777343,'2013-02-04 19:30:15'),(19,1,16777343,'2013-02-04 20:07:22'),(20,1,16777343,'2013-02-04 20:09:33'),(21,1,16777343,'2013-02-04 20:17:30'),(22,1,16777343,'2013-02-04 20:20:24'),(23,1,16777343,'2013-02-04 20:21:32'),(24,1,16777343,'2013-02-04 20:22:53'),(25,1,16777343,'2013-02-04 20:25:07'),(26,1,16777343,'2013-02-04 20:27:19'),(27,1,16777343,'2013-02-04 20:28:56'),(28,1,16777343,'2013-02-04 20:31:00'),(29,1,16777343,'2013-02-04 20:34:08'),(30,1,16777343,'2013-02-04 20:35:09'),(31,1,16777343,'2013-02-04 20:46:02'),(32,1,16777343,'2013-02-04 20:47:15'),(33,1,16777343,'2013-02-04 20:52:52'),(34,1,16777343,'2013-02-04 20:53:14'),(35,1,16777343,'2013-02-04 20:54:35'),(36,1,16777343,'2013-02-04 21:13:11'),(37,1,16777343,'2013-02-04 21:16:10'),(38,1,16777343,'2013-02-04 21:22:13'),(39,1,16777343,'2013-02-04 21:29:09'),(40,1,16777343,'2013-02-04 22:12:28'),(41,3,16777343,'2013-02-04 22:12:55'),(42,1,16777343,'2013-02-04 22:14:55'),(43,3,16777343,'2013-02-04 22:15:17'),(44,1,16777343,'2013-02-04 22:27:07'),(45,1,16777343,'2013-02-04 22:28:05'),(46,1,16777343,'2013-02-04 22:28:46'),(47,1,16777343,'2013-02-04 22:29:48'),(48,1,16777343,'2013-02-04 22:30:56'),(49,1,16777343,'2013-02-04 22:32:26'),(50,1,16777343,'2013-02-04 22:33:12'),(51,1,16777343,'2013-02-04 22:33:36'),(52,1,16777343,'2013-02-04 22:34:08'),(53,1,16777343,'2013-02-04 22:36:47'),(54,1,16777343,'2013-02-04 22:37:49'),(55,1,16777343,'2013-02-04 22:39:10'),(56,1,16777343,'2013-02-04 22:39:41'),(57,1,16777343,'2013-02-04 22:42:48'),(58,1,16777343,'2013-02-04 22:45:24'),(59,1,16777343,'2013-02-04 22:49:32'),(60,1,16777343,'2013-02-04 22:50:12'),(61,1,16777343,'2013-02-04 23:11:00'),(62,1,16777343,'2013-02-04 23:23:43'),(63,1,16777343,'2013-02-04 23:31:07'),(64,1,16777343,'2013-02-04 23:46:08'),(65,1,16777343,'2013-02-05 01:17:09'),(66,1,16777343,'2013-02-05 01:59:14'),(67,3,16777343,'2013-02-05 01:59:35'),(68,1,16777343,'2013-02-05 23:08:42'),(69,1,16777343,'2013-02-05 23:11:21'),(70,1,16777343,'2013-02-05 23:21:36'),(71,1,16777343,'2013-02-05 23:25:56'),(72,1,16777343,'2013-02-05 23:28:16'),(73,1,16777343,'2013-02-05 23:28:58'),(74,1,16777343,'2013-02-05 23:33:46'),(75,1,16777343,'2013-02-05 23:40:58'),(76,1,16777343,'2013-02-05 23:43:08'),(77,1,16777343,'2013-02-06 00:08:12'),(78,1,16777343,'2013-02-06 00:09:31'),(79,1,16777343,'2013-02-06 00:09:43'),(80,1,16777343,'2013-02-06 00:10:08'),(81,1,16777343,'2013-02-06 00:22:59'),(82,3,16777343,'2013-02-06 00:28:06'),(83,1,16777343,'2013-02-06 00:28:11'),(84,1,16777343,'2013-02-06 00:30:29'),(85,3,16777343,'2013-02-06 00:30:34'),(86,1,16777343,'2013-02-06 00:31:53'),(87,3,16777343,'2013-02-06 00:32:23'),(88,3,16777343,'2013-02-06 00:32:44'),(89,1,16777343,'2013-02-06 00:47:10'),(90,1,16777343,'2013-02-06 01:13:24'),(91,1,16777343,'2013-02-06 01:23:57'),(92,1,16777343,'2013-02-06 01:25:24'),(93,1,16777343,'2013-02-06 16:07:17'),(94,1,16777343,'2013-02-07 00:07:30'),(95,1,16777343,'2013-02-07 00:11:57'),(96,1,16777343,'2013-02-07 00:14:38'),(97,1,16777343,'2013-02-07 00:27:49'),(98,1,16777343,'2013-02-07 00:29:25'),(99,1,16777343,'2013-02-07 00:35:36'),(100,1,16777343,'2013-02-08 00:04:47'),(101,1,16777343,'2013-02-08 00:07:57'),(102,1,16777343,'2013-02-08 00:12:46'),(103,1,16777343,'2013-02-08 00:18:05'),(104,1,16777343,'2013-02-08 00:20:47'),(105,1,16777343,'2013-02-08 00:25:03'),(106,1,16777343,'2013-02-08 00:26:43'),(107,1,16777343,'2013-02-08 00:27:56'),(108,1,16777343,'2013-02-08 00:29:18'),(109,1,16777343,'2013-02-08 00:30:23'),(110,1,16777343,'2013-02-08 00:32:10'),(111,1,16777343,'2013-02-08 00:56:16'),(112,1,16777343,'2013-02-08 01:01:34'),(113,1,16777343,'2013-02-08 10:28:33'),(114,1,16777343,'2013-02-08 10:33:49'),(115,1,16777343,'2013-02-08 10:34:40'),(116,1,16777343,'2013-02-08 10:35:34'),(117,1,16777343,'2013-02-09 14:58:55'),(118,1,16777343,'2013-02-09 15:00:20'),(119,1,16777343,'2013-02-09 15:06:40'),(120,1,16777343,'2013-02-09 15:09:23'),(121,1,16777343,'2013-02-09 15:17:52'),(122,1,16777343,'2013-02-09 15:20:42'),(123,1,16777343,'2013-02-09 15:28:45'),(124,1,16777343,'2013-02-09 15:36:00'),(125,1,16777343,'2013-02-09 15:55:07'),(126,1,16777343,'2013-02-09 16:03:40'),(127,1,16777343,'2013-02-09 16:04:12'),(128,1,16777343,'2013-02-09 16:06:02'),(129,1,16777343,'2013-02-09 16:10:26'),(130,1,16777343,'2013-02-09 16:25:40'),(131,1,16777343,'2013-02-09 16:36:27'),(132,1,16777343,'2013-02-09 16:43:07'),(133,1,16777343,'2013-02-09 19:19:59'),(134,1,16777343,'2013-02-09 19:22:19'),(135,1,16777343,'2013-02-09 19:24:03'),(136,1,16777343,'2013-02-09 19:26:18'),(137,1,16777343,'2013-02-09 19:27:46'),(138,1,16777343,'2013-02-09 19:30:11'),(139,1,16777343,'2013-02-09 19:54:27'),(140,1,16777343,'2013-02-09 19:54:30'),(141,1,16777343,'2013-02-09 20:04:21'),(142,1,16777343,'2013-02-09 20:05:17'),(143,1,16777343,'2013-02-09 20:06:18'),(144,1,16777343,'2013-02-09 20:08:55'),(145,1,16777343,'2013-02-09 20:20:20'),(146,1,16777343,'2013-02-09 20:24:02'),(147,1,16777343,'2013-02-09 20:32:08'),(148,1,16777343,'2013-02-09 20:32:40'),(149,1,16777343,'2013-02-09 20:51:27'),(150,1,16777343,'2013-02-09 20:54:11'),(151,1,16777343,'2013-02-09 20:58:09'),(152,1,16777343,'2013-02-09 21:14:16'),(153,1,16777343,'2013-02-09 21:14:57'),(154,1,16777343,'2013-02-09 21:15:28'),(155,1,16777343,'2013-02-09 21:15:59'),(156,1,16777343,'2013-02-09 21:17:29'),(157,3,16777343,'2013-02-09 21:17:53'),(158,3,16777343,'2013-02-09 21:21:26'),(159,1,16777343,'2013-02-09 21:21:51'),(160,1,16777343,'2013-02-09 21:25:15'),(161,1,16777343,'2013-02-09 21:26:47'),(162,1,16777343,'2013-02-09 21:30:32'),(163,1,16777343,'2013-02-09 21:30:42'),(164,1,16777343,'2013-02-09 21:32:15'),(165,1,16777343,'2013-02-09 21:40:01'),(166,1,16777343,'2013-02-09 21:42:56'),(167,1,16777343,'2013-02-09 21:52:06'),(168,1,16777343,'2013-02-09 21:52:57'),(169,1,16777343,'2013-02-09 21:53:43'),(170,1,16777343,'2013-02-09 21:57:10'),(171,1,16777343,'2013-02-09 21:58:32'),(172,1,16777343,'2013-02-09 22:03:36'),(173,1,16777343,'2013-02-09 22:11:37'),(174,1,16777343,'2013-02-09 22:13:05'),(175,1,16777343,'2013-02-09 22:15:42'),(176,1,16777343,'2013-02-09 22:17:27'),(177,1,16777343,'2013-02-09 22:21:57'),(178,1,16777343,'2013-02-09 22:24:48'),(179,1,16777343,'2013-02-09 22:28:03'),(180,1,16777343,'2013-02-09 22:32:00'),(181,1,16777343,'2013-02-09 22:32:31'),(182,1,16777343,'2013-02-09 22:33:53'),(183,1,16777343,'2013-02-09 22:34:24'),(184,1,16777343,'2013-02-09 22:34:41'),(185,1,16777343,'2013-02-09 22:35:14'),(186,1,16777343,'2013-02-09 22:37:17'),(187,1,16777343,'2013-02-09 22:37:56'),(188,1,16777343,'2013-02-09 22:38:50'),(189,1,16777343,'2013-02-09 22:40:04'),(190,1,16777343,'2013-02-09 22:41:47'),(191,1,16777343,'2013-02-09 22:43:11'),(192,1,16777343,'2013-02-09 22:44:13'),(193,1,16777343,'2013-02-09 22:45:22'),(194,1,16777343,'2013-02-09 22:46:39'),(195,1,16777343,'2013-02-09 22:48:33'),(196,1,16777343,'2013-02-09 22:49:37'),(197,1,16777343,'2013-02-09 22:58:34'),(198,1,16777343,'2013-02-09 23:04:27'),(199,1,16777343,'2013-02-09 23:06:38'),(200,1,16777343,'2013-02-09 23:11:19'),(201,1,16777343,'2013-02-09 23:12:22'),(202,1,16777343,'2013-02-09 23:14:17'),(203,1,16777343,'2013-02-09 23:17:54'),(204,1,16777343,'2013-02-09 23:19:28'),(205,1,16777343,'2013-02-09 23:21:24'),(206,1,16777343,'2013-02-09 23:24:01'),(207,1,16777343,'2013-02-09 23:26:24'),(208,1,16777343,'2013-02-09 23:29:59'),(209,1,16777343,'2013-02-10 00:02:48'),(210,1,16777343,'2013-02-10 00:03:13'),(211,1,16777343,'2013-02-10 00:06:40'),(212,1,16777343,'2013-02-10 00:08:38'),(213,1,16777343,'2013-02-10 00:11:11'),(214,1,16777343,'2013-02-10 00:33:45'),(215,1,16777343,'2013-02-10 00:41:13'),(216,1,16777343,'2013-02-10 00:57:10'),(217,1,16777343,'2013-02-10 01:05:22'),(218,1,16777343,'2013-02-10 01:06:28'),(219,1,16777343,'2013-02-10 01:12:50'),(220,1,16777343,'2013-02-10 01:18:53'),(221,1,16777343,'2013-02-10 01:19:45'),(222,1,16777343,'2013-02-10 01:21:57'),(223,1,16777343,'2013-02-10 01:23:54'),(224,1,16777343,'2013-02-10 01:25:01'),(225,1,16777343,'2013-02-10 01:26:50'),(226,1,16777343,'2013-02-10 01:30:57'),(227,1,16777343,'2013-02-10 01:49:33'),(228,1,16777343,'2013-02-10 01:50:21'),(229,1,16777343,'2013-02-10 01:51:08'),(230,1,16777343,'2013-02-10 01:56:04'),(231,1,16777343,'2013-02-10 02:06:05'),(232,1,16777343,'2013-02-10 13:28:28'),(233,1,16777343,'2013-02-10 13:33:54'),(234,1,16777343,'2013-02-10 13:35:24'),(235,1,16777343,'2013-02-10 13:37:45'),(236,1,16777343,'2013-02-10 13:40:05'),(237,1,16777343,'2013-02-10 13:40:39'),(238,1,16777343,'2013-02-10 13:44:31'),(239,1,16777343,'2013-02-10 13:54:26'),(240,1,16777343,'2013-02-10 13:56:48'),(241,1,16777343,'2013-02-10 14:00:28'),(242,1,16777343,'2013-02-10 14:02:28'),(243,1,16777343,'2013-02-10 14:03:36'),(244,1,16777343,'2013-02-10 14:04:17'),(245,1,16777343,'2013-02-10 14:06:19'),(246,1,16777343,'2013-02-10 14:09:36'),(247,1,16777343,'2013-02-10 14:10:50'),(248,1,16777343,'2013-02-10 14:12:17'),(249,1,16777343,'2013-02-10 14:16:26'),(250,1,16777343,'2013-02-10 14:18:27'),(251,1,16777343,'2013-02-10 14:23:54'),(252,1,16777343,'2013-02-10 14:37:10'),(253,1,16777343,'2013-02-10 14:41:08'),(254,1,16777343,'2013-02-10 14:45:23'),(255,1,16777343,'2013-02-10 14:49:49'),(256,1,16777343,'2013-02-10 14:53:43'),(257,1,16777343,'2013-02-10 15:26:26'),(258,1,16777343,'2013-02-10 15:37:20'),(259,1,16777343,'2013-02-10 15:38:17'),(260,1,16777343,'2013-02-10 15:47:03'),(261,1,16777343,'2013-02-10 15:49:53'),(262,1,16777343,'2013-02-10 15:50:37'),(263,1,16777343,'2013-02-10 15:52:10'),(264,1,16777343,'2013-02-10 15:54:45');

/*Table structure for table `active_trade_cash` */

DROP TABLE IF EXISTS `active_trade_cash`;

CREATE TABLE `active_trade_cash` (
  `character_id` int(11) NOT NULL COMMENT 'The character that put the cash on the trade table.',
  `cash` int(11) NOT NULL COMMENT 'The amount of cash the character put down.',
  PRIMARY KEY (`character_id`),
  CONSTRAINT `active_trade_cash_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Cash that has been put down in an active trade.';

/*Data for the table `active_trade_cash` */

/*Table structure for table `active_trade_item` */

DROP TABLE IF EXISTS `active_trade_item`;

CREATE TABLE `active_trade_item` (
  `item_id` int(11) NOT NULL COMMENT 'The ID of the item the character put down.',
  `character_id` int(11) NOT NULL COMMENT 'The character that added the item.',
  PRIMARY KEY (`item_id`),
  KEY `character_id` (`character_id`),
  CONSTRAINT `active_trade_item_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `active_trade_item_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items that have been put down in an active trade.';

/*Data for the table `active_trade_item` */

/*Table structure for table `alliance` */

DROP TABLE IF EXISTS `alliance`;

CREATE TABLE `alliance` (
  `id` tinyint(3) unsigned NOT NULL COMMENT 'The unique ID of the alliance.',
  `name` varchar(255) NOT NULL DEFAULT '' COMMENT 'The name of the alliance.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The different character alliances.';

/*Data for the table `alliance` */

insert  into `alliance`(`id`,`name`) values (0,'user'),(1,'monster'),(2,'townsperson'),(3,'aggressive monster');

/*Table structure for table `alliance_attackable` */

DROP TABLE IF EXISTS `alliance_attackable`;

CREATE TABLE `alliance_attackable` (
  `alliance_id` tinyint(3) unsigned NOT NULL COMMENT 'The alliance.',
  `attackable_id` tinyint(3) unsigned NOT NULL COMMENT 'The alliance that this alliance (alliance_id) can attack.',
  PRIMARY KEY (`alliance_id`,`attackable_id`),
  KEY `attackable_id` (`attackable_id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `alliance_attackable_ibfk_3` FOREIGN KEY (`attackable_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `alliance_attackable_ibfk_4` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='List of alliances that an alliance can attack.';

/*Data for the table `alliance_attackable` */

insert  into `alliance_attackable`(`alliance_id`,`attackable_id`) values (1,0),(3,0),(0,1),(3,1),(0,3),(3,3);

/*Table structure for table `alliance_hostile` */

DROP TABLE IF EXISTS `alliance_hostile`;

CREATE TABLE `alliance_hostile` (
  `alliance_id` tinyint(3) unsigned NOT NULL COMMENT 'The alliance that is hotile.',
  `hostile_id` tinyint(3) unsigned NOT NULL COMMENT 'The alliance that this alliance (alliance_id) is hostile towards by default.',
  PRIMARY KEY (`alliance_id`,`hostile_id`),
  KEY `hostile_id` (`hostile_id`),
  KEY `alliance_id` (`alliance_id`),
  CONSTRAINT `alliance_hostile_ibfk_3` FOREIGN KEY (`hostile_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `alliance_hostile_ibfk_4` FOREIGN KEY (`alliance_id`) REFERENCES `alliance` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Alliances that an alliance is hostile towards by default.';

/*Data for the table `alliance_hostile` */

insert  into `alliance_hostile`(`alliance_id`,`hostile_id`) values (1,0),(3,0),(0,1),(3,1),(0,3),(3,3);

/*Table structure for table `applied_patches` */

DROP TABLE IF EXISTS `applied_patches`;

CREATE TABLE `applied_patches` (
  `file_name` varchar(255) NOT NULL,
  `date_applied` datetime NOT NULL,
  PRIMARY KEY (`file_name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `applied_patches` */

/*Table structure for table `character` */

DROP TABLE IF EXISTS `character`;

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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1 COMMENT='Persisted (users, persistent NPCs) chars.';

/*Data for the table `character` */

insert  into `character`(`id`,`character_template_id`,`name`,`shop_id`,`chat_dialog`,`ai_id`,`load_map_id`,`load_x`,`load_y`,`respawn_map_id`,`respawn_x`,`respawn_y`,`body_id`,`move_speed`,`cash`,`level`,`exp`,`statpoints`,`hp`,`mp`,`stat_maxhp`,`stat_maxmp`,`stat_minhit`,`stat_maxhit`,`stat_defence`,`stat_agi`,`stat_int`,`stat_str`) values (1,NULL,'Spodi',NULL,NULL,NULL,4,690,460,1,512,512,1,1800,515,34,1015,130,50,50,50,50,1,36,1,1,1,1),(6,NULL,'Helix',NULL,NULL,NULL,3,450,316,1,512,512,1,1800,20,1,20,0,50,50,50,50,1,1,1,1,1,1);

/*Table structure for table `character_equipped` */

DROP TABLE IF EXISTS `character_equipped`;

CREATE TABLE `character_equipped` (
  `character_id` int(11) NOT NULL COMMENT 'The character who the equipped item is on.',
  `item_id` int(11) NOT NULL COMMENT 'The item that is equipped by the character.',
  `slot` tinyint(3) unsigned NOT NULL COMMENT 'The slot the equipped item is in.',
  PRIMARY KEY (`character_id`,`slot`),
  KEY `item_id` (`item_id`),
  CONSTRAINT `character_equipped_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `item` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_equipped_ibfk_4` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items a character has equipped.';

/*Data for the table `character_equipped` */

/*Table structure for table `character_inventory` */

DROP TABLE IF EXISTS `character_inventory`;

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

/*Data for the table `character_inventory` */

/*Table structure for table `character_quest_status` */

DROP TABLE IF EXISTS `character_quest_status`;

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

/*Data for the table `character_quest_status` */

insert  into `character_quest_status`(`character_id`,`quest_id`,`started_on`,`completed_on`) values (1,0,'2013-02-07 00:44:31','2013-02-09 15:02:52'),(1,1,'2013-02-10 14:46:29','2013-02-10 15:56:39');

/*Table structure for table `character_quest_status_kills` */

DROP TABLE IF EXISTS `character_quest_status_kills`;

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

/*Data for the table `character_quest_status_kills` */

/*Table structure for table `character_skill` */

DROP TABLE IF EXISTS `character_skill`;

CREATE TABLE `character_skill` (
  `character_id` int(11) NOT NULL COMMENT 'The character that knows the skill.',
  `skill_id` tinyint(5) unsigned NOT NULL COMMENT 'The skill the character knows.',
  `time_added` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'When this row was added.',
  PRIMARY KEY (`character_id`,`skill_id`),
  CONSTRAINT `character_skill_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Skills known by a character.';

/*Data for the table `character_skill` */

insert  into `character_skill`(`character_id`,`skill_id`,`time_added`) values (1,0,'2012-12-15 06:18:10'),(1,1,'2012-12-15 06:18:11');

/*Table structure for table `character_status_effect` */

DROP TABLE IF EXISTS `character_status_effect`;

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

/*Data for the table `character_status_effect` */

/*Table structure for table `character_template` */

DROP TABLE IF EXISTS `character_template`;

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

/*Data for the table `character_template` */

insert  into `character_template`(`id`,`alliance_id`,`name`,`ai_id`,`shop_id`,`chat_dialog`,`body_id`,`move_speed`,`respawn`,`level`,`exp`,`statpoints`,`give_exp`,`give_cash`,`stat_maxhp`,`stat_maxmp`,`stat_minhit`,`stat_maxhit`,`stat_defence`,`stat_agi`,`stat_int`,`stat_str`) values (0,0,'User Template',NULL,NULL,NULL,1,1800,5,1,0,0,0,0,50,50,1,2,1,1,1,1),(1,1,'Bee',1,NULL,NULL,5,1500,10,1,0,0,5,5,5,5,1,2,1,1,1,1),(2,2,'Gallot',NULL,NULL,NULL,6,3000,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(4,2,'Shopkeeper',NULL,0,NULL,3,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(5,2,'Inn Keeper',NULL,NULL,0,3,1800,5,1,0,0,0,0,50,50,1,1,1,1,1,1),(6,3,'Brawler',1,NULL,NULL,2,500,5,1,0,0,8,8,20,20,1,2,1,1,1,1);

/*Table structure for table `character_template_equipped` */

DROP TABLE IF EXISTS `character_template_equipped`;

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

/*Data for the table `character_template_equipped` */

insert  into `character_template_equipped`(`id`,`character_template_id`,`item_template_id`,`chance`) values (0,1,7,30000),(1,1,5,2000),(2,1,3,2000),(3,6,8,65535);

/*Table structure for table `character_template_inventory` */

DROP TABLE IF EXISTS `character_template_inventory`;

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

/*Data for the table `character_template_inventory` */

insert  into `character_template_inventory`(`id`,`character_template_id`,`item_template_id`,`min`,`max`,`chance`) values (0,1,5,0,2,10000),(1,1,5,0,2,10000),(3,1,5,0,2,10000),(4,1,7,1,10,65535),(5,6,3,1,1,655),(6,6,8,1,1,655),(7,1,3,1,1,5000),(8,1,3,1,1,5000),(9,1,7,1,10,65535),(10,1,3,1,1,5000),(11,1,7,1,10,65535),(12,1,3,1,1,5000),(13,1,5,0,2,10000),(14,1,7,1,10,65535),(15,1,3,1,1,5000),(16,1,7,1,10,65535),(17,6,6,1,1,655),(18,6,7,1,1,655),(19,6,5,1,1,3277),(20,6,2,1,1,6554),(21,6,1,1,1,6554);

/*Table structure for table `character_template_quest_provider` */

DROP TABLE IF EXISTS `character_template_quest_provider`;

CREATE TABLE `character_template_quest_provider` (
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template.',
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest provided by this character template. Only applies for valid quest givers (that is, not users).',
  PRIMARY KEY (`character_template_id`,`quest_id`),
  KEY `quest_id` (`quest_id`),
  CONSTRAINT `character_template_quest_provider_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `character_template_quest_provider_ibfk_2` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Quests provided by character templates.';

/*Data for the table `character_template_quest_provider` */

insert  into `character_template_quest_provider`(`character_template_id`,`quest_id`) values (2,0),(2,1);

/*Table structure for table `character_template_skill` */

DROP TABLE IF EXISTS `character_template_skill`;

CREATE TABLE `character_template_skill` (
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template that knows the skill.',
  `skill_id` tinyint(5) unsigned NOT NULL COMMENT 'The skill the character template knows.',
  PRIMARY KEY (`character_template_id`,`skill_id`),
  CONSTRAINT `character_template_skill_ibfk_1` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Skills known by a character template.';

/*Data for the table `character_template_skill` */

insert  into `character_template_skill`(`character_template_id`,`skill_id`) values (0,0),(0,1),(2,0),(6,1);

/*Table structure for table `event_counters_guild` */

DROP TABLE IF EXISTS `event_counters_guild`;

CREATE TABLE `event_counters_guild` (
  `guild_id` smallint(5) unsigned NOT NULL COMMENT 'The guild the event occured on.',
  `guild_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`guild_id`,`guild_event_counter_id`),
  CONSTRAINT `event_counters_guild_ibfk_1` FOREIGN KEY (`guild_id`) REFERENCES `guild` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for guilds.';

/*Data for the table `event_counters_guild` */

/*Table structure for table `event_counters_item_template` */

DROP TABLE IF EXISTS `event_counters_item_template`;

CREATE TABLE `event_counters_item_template` (
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the item the event occured on.',
  `item_template_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`item_template_id`,`item_template_event_counter_id`),
  CONSTRAINT `event_counters_item_template_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for item templates.';

/*Data for the table `event_counters_item_template` */

insert  into `event_counters_item_template`(`item_template_id`,`item_template_event_counter_id`,`counter`) values (1,0,261),(1,1,11),(1,5,68),(1,6,53),(1,8,11),(2,0,270),(2,5,68),(2,6,53),(2,8,15),(3,0,233),(3,4,666),(3,5,96),(3,6,90),(3,8,23),(4,0,1),(5,0,330),(5,3,2),(5,5,109),(5,6,148),(5,8,43),(6,0,27),(6,5,6),(6,6,6),(7,0,9559),(7,4,10),(7,5,320),(7,6,5344),(7,8,1367),(8,0,2633),(8,5,19),(8,6,4);

/*Table structure for table `event_counters_map` */

DROP TABLE IF EXISTS `event_counters_map`;

CREATE TABLE `event_counters_map` (
  `map_id` smallint(5) unsigned NOT NULL COMMENT 'The map the event occured on.',
  `map_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`map_id`,`map_event_counter_id`),
  CONSTRAINT `event_counters_map_ibfk_1` FOREIGN KEY (`map_id`) REFERENCES `map` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for maps.';

/*Data for the table `event_counters_map` */

insert  into `event_counters_map`(`map_id`,`map_event_counter_id`,`counter`) values (1,0,46),(1,1,440),(1,2,5440),(1,3,145),(1,4,230),(1,5,15),(1,7,806),(1,8,806),(2,0,12),(2,1,2584),(2,2,141),(2,3,471),(2,4,471),(2,7,672),(2,8,672),(3,0,26),(3,1,94),(4,0,14),(4,1,5);

/*Table structure for table `event_counters_npc` */

DROP TABLE IF EXISTS `event_counters_npc`;

CREATE TABLE `event_counters_npc` (
  `npc_template_id` smallint(5) unsigned NOT NULL COMMENT 'The character template of the NPC the event occured on.',
  `npc_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`npc_template_id`,`npc_event_counter_id`),
  CONSTRAINT `event_counters_npc_ibfk_1` FOREIGN KEY (`npc_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for NPC templates.';

/*Data for the table `event_counters_npc` */

insert  into `event_counters_npc`(`npc_template_id`,`npc_event_counter_id`,`counter`) values (0,6,145),(0,8,32),(1,1,2),(1,2,15),(1,3,1425),(1,4,145),(1,5,1790),(1,7,990),(1,8,144),(6,1,382),(6,3,92),(6,4,20756),(6,5,2136),(6,6,20756),(6,7,20848),(6,8,20959);

/*Table structure for table `event_counters_quest` */

DROP TABLE IF EXISTS `event_counters_quest`;

CREATE TABLE `event_counters_quest` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest the event occured on.',
  `quest_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`quest_id`,`quest_event_counter_id`),
  CONSTRAINT `event_counters_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for quests.';

/*Data for the table `event_counters_quest` */

insert  into `event_counters_quest`(`quest_id`,`quest_event_counter_id`,`counter`) values (0,1,1),(0,2,1),(1,1,3),(1,2,3);

/*Table structure for table `event_counters_shop` */

DROP TABLE IF EXISTS `event_counters_shop`;

CREATE TABLE `event_counters_shop` (
  `shop_id` smallint(5) unsigned NOT NULL COMMENT 'The shop the event occured on.',
  `shop_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`shop_id`,`shop_event_counter_id`),
  CONSTRAINT `event_counters_shop_ibfk_1` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for shops.';

/*Data for the table `event_counters_shop` */

insert  into `event_counters_shop`(`shop_id`,`shop_event_counter_id`,`counter`) values (0,1,2),(0,2,2),(0,3,2000);

/*Table structure for table `event_counters_user` */

DROP TABLE IF EXISTS `event_counters_user`;

CREATE TABLE `event_counters_user` (
  `user_id` int(11) NOT NULL COMMENT 'The character ID for the user character the event occured on.',
  `user_event_counter_id` tinyint(3) unsigned NOT NULL COMMENT 'The ID of the event that the counter is for.',
  `counter` bigint(20) NOT NULL COMMENT 'The event counter.',
  PRIMARY KEY (`user_id`,`user_event_counter_id`),
  CONSTRAINT `event_counters_user_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `character` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event counters for users.';

/*Data for the table `event_counters_user` */

insert  into `event_counters_user`(`user_id`,`user_event_counter_id`,`counter`) values (1,2,232),(1,4,4555),(1,6,1524),(1,7,349),(1,8,1057),(1,9,11),(1,100,13),(1,101,76);

/*Table structure for table `guild` */

DROP TABLE IF EXISTS `guild`;

CREATE TABLE `guild` (
  `id` smallint(5) unsigned NOT NULL AUTO_INCREMENT COMMENT 'The unique ID of the guild.',
  `name` varchar(50) NOT NULL COMMENT 'The name of the guild.',
  `tag` varchar(5) NOT NULL COMMENT 'The guild''s tag.',
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'When this guild was created.',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The active guilds.';

/*Data for the table `guild` */

/*Table structure for table `guild_event` */

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
  CONSTRAINT `guild_event_ibfk_3` FOREIGN KEY (`target_character_id`) REFERENCES `character` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Event log for guilds.';

/*Data for the table `guild_event` */

/*Table structure for table `guild_member` */

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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The members of a guild.';

/*Data for the table `guild_member` */

/*Table structure for table `item` */

DROP TABLE IF EXISTS `item`;

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
) ENGINE=InnoDB AUTO_INCREMENT=97 DEFAULT CHARSET=latin1 COMMENT='The live, persisted items.';

/*Data for the table `item` */

/*Table structure for table `item_template` */

DROP TABLE IF EXISTS `item_template`;

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

/*Data for the table `item_template` */

insert  into `item_template`(`id`,`type`,`weapon_type`,`range`,`width`,`height`,`name`,`description`,`graphic`,`value`,`hp`,`mp`,`stat_agi`,`stat_int`,`stat_str`,`stat_minhit`,`stat_maxhit`,`stat_maxhp`,`stat_maxmp`,`stat_defence`,`stat_req_agi`,`stat_req_int`,`stat_req_str`,`equipped_body`,`action_display_id`,`skill_id`) values (0,2,1,10,16,16,'Unarmed','Unarmed',1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,NULL),(1,1,0,0,9,16,'Healing Potion','A healing potion',95,15,25,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,NULL),(2,1,0,0,9,16,'Mana Potion','A mana potion',94,10,0,25,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,NULL),(3,2,1,20,27,21,'Blue Sword','A sword... that is blue.',237,100,0,0,0,0,0,5,10,0,0,0,0,0,0,'Weapon.Sword',NULL,NULL),(4,4,0,0,30,30,'Black Armor','Body armor made out of... black.',234,2000,0,0,5,5,5,5,5,0,0,20,0,0,0,'Body.Black',NULL,NULL),(5,4,0,0,30,30,'Gold Armor','Body armor made out of gold. Designed by The Trump.',235,1000,0,0,-5,0,0,0,0,0,0,10,0,0,5,'Body.Gold',NULL,NULL),(6,2,2,500,16,16,'Pistol','Just point it at whatever you want to die.',177,500,0,0,0,0,0,25,50,0,0,0,3,3,1,NULL,NULL,NULL),(7,2,3,200,11,9,'Rock','Nothing says \"I fight dirty\" quite like a large rock',182,1,0,0,0,0,0,2,6,0,0,0,3,0,8,NULL,1,NULL),(8,4,0,0,30,30,'Iron Armor','Body armor made out of iron.',236,50,0,0,1,0,0,0,0,0,0,6,0,0,0,'Body.Iron',NULL,NULL);

/*Table structure for table `map` */

DROP TABLE IF EXISTS `map`;

CREATE TABLE `map` (
  `id` smallint(5) unsigned NOT NULL COMMENT 'The unique ID of the map.',
  `name` varchar(255) NOT NULL COMMENT 'Name of the map.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Map meta-information.';

/*Data for the table `map` */

insert  into `map`(`id`,`name`) values (1,'Town'),(2,'Jail'),(3,'Shop'),(4,'New map');

/*Table structure for table `map_spawn` */

DROP TABLE IF EXISTS `map_spawn`;

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

/*Data for the table `map_spawn` */

insert  into `map_spawn`(`id`,`map_id`,`character_template_id`,`amount`,`respawn`,`x`,`y`,`width`,`height`,`direction_id`) values (5,2,6,45,5,130,165,720,475,0),(9,1,1,3,20,NULL,NULL,NULL,NULL,0),(11,1,2,1,5,600,580,32,32,0),(16,3,4,1,5,250,250,1,1,0),(17,3,5,1,5,678,253,1,1,0),(18,4,2,1,5,600,400,128,128,5);

/*Table structure for table `quest` */

DROP TABLE IF EXISTS `quest`;

CREATE TABLE `quest` (
  `id` smallint(5) unsigned NOT NULL COMMENT 'The unique ID of the quest. Note: This table is like a template. Quest and character_quest_status are like character_template and character, respectively.',
  `repeatable` tinyint(1) unsigned NOT NULL DEFAULT '0' COMMENT 'If this quest can be repeated by a character after they have completed it.',
  `reward_cash` int(11) NOT NULL DEFAULT '0' COMMENT 'The base cash reward for completing this quest.',
  `reward_exp` int(11) NOT NULL DEFAULT '0' COMMENT 'The base experience reward for completing this quest.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The quests.';

/*Data for the table `quest` */

insert  into `quest`(`id`,`repeatable`,`reward_cash`,`reward_exp`) values (0,0,500,1000),(1,1,10,10);

/*Table structure for table `quest_require_finish_item` */

DROP TABLE IF EXISTS `quest_require_finish_item`;

CREATE TABLE `quest_require_finish_item` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that this requirement is for.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the item that is required for this quest to be finished.',
  `amount` tinyint(3) unsigned NOT NULL COMMENT 'The amount of the item required to finish this quest.',
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_require_finish_item_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_finish_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items required for finishing a quest.';

/*Data for the table `quest_require_finish_item` */

/*Table structure for table `quest_require_finish_quest` */

DROP TABLE IF EXISTS `quest_require_finish_quest`;

CREATE TABLE `quest_require_finish_quest` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that this requirement is for.',
  `req_quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest required to be finished before this quest can be finished.',
  PRIMARY KEY (`quest_id`,`req_quest_id`),
  KEY `req_quest_id` (`req_quest_id`),
  CONSTRAINT `quest_require_finish_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_finish_quest_ibfk_2` FOREIGN KEY (`req_quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Quests required to be finished before this quest is finished';

/*Data for the table `quest_require_finish_quest` */

/*Table structure for table `quest_require_kill` */

DROP TABLE IF EXISTS `quest_require_kill`;

CREATE TABLE `quest_require_kill` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that this requirement is for.',
  `character_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the characters that must be killed to complete this quest.',
  `amount` smallint(5) unsigned NOT NULL COMMENT 'The number of characters that must be killed to complete this quest.',
  PRIMARY KEY (`quest_id`,`character_template_id`),
  KEY `character_template_id` (`character_template_id`),
  CONSTRAINT `quest_require_kill_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_kill_ibfk_2` FOREIGN KEY (`character_template_id`) REFERENCES `character_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Kill requirements to finish a quest.';

/*Data for the table `quest_require_kill` */

insert  into `quest_require_kill`(`quest_id`,`character_template_id`,`amount`) values (0,1,5),(1,6,10);

/*Table structure for table `quest_require_start_item` */

DROP TABLE IF EXISTS `quest_require_start_item`;

CREATE TABLE `quest_require_start_item` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'Quest that this requirement is for.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the item that is required to start the quest.',
  `amount` tinyint(3) unsigned NOT NULL COMMENT 'The amount of the item that is required.',
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_require_start_item_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_start_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items required to start a quest.';

/*Data for the table `quest_require_start_item` */

/*Table structure for table `quest_require_start_quest` */

DROP TABLE IF EXISTS `quest_require_start_quest`;

CREATE TABLE `quest_require_start_quest` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that this requirement is for.',
  `req_quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that is required to be finished before this quest can be started.',
  PRIMARY KEY (`quest_id`,`req_quest_id`),
  KEY `req_quest_id` (`req_quest_id`),
  CONSTRAINT `quest_require_start_quest_ibfk_1` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_require_start_quest_ibfk_2` FOREIGN KEY (`req_quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Quests required to be finished to start this quest.';

/*Data for the table `quest_require_start_quest` */

/*Table structure for table `quest_reward_item` */

DROP TABLE IF EXISTS `quest_reward_item`;

CREATE TABLE `quest_reward_item` (
  `quest_id` smallint(5) unsigned NOT NULL COMMENT 'The quest that this completion reward is for.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the item to give as the reward.',
  `amount` tinyint(3) unsigned NOT NULL COMMENT 'The amount of the item to give (should be greater than 0).',
  PRIMARY KEY (`quest_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `quest_reward_item_ibfk_3` FOREIGN KEY (`quest_id`) REFERENCES `quest` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `quest_reward_item_ibfk_4` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Items given as reward for finishing quest.';

/*Data for the table `quest_reward_item` */

insert  into `quest_reward_item`(`quest_id`,`item_template_id`,`amount`) values (0,3,1),(1,3,1);

/*Table structure for table `server_time` */

DROP TABLE IF EXISTS `server_time`;

CREATE TABLE `server_time` (
  `server_time` datetime NOT NULL COMMENT 'The current time of the server, as seen by the server process. Only updated when server is running. Especially intended for when comparing the time to the server''s current time. Slightly low resolution (assume ~10 seconds).'
) ENGINE=MyISAM DEFAULT CHARSET=latin1 MAX_ROWS=1 ROW_FORMAT=FIXED COMMENT='Holds the current time of the server.';

/*Data for the table `server_time` */

insert  into `server_time`(`server_time`) values ('2013-02-10 16:02:14');

/*Table structure for table `shop` */

DROP TABLE IF EXISTS `shop`;

CREATE TABLE `shop` (
  `id` smallint(5) unsigned NOT NULL COMMENT 'The unique ID of the shop.',
  `name` varchar(60) NOT NULL COMMENT 'The name of this shop.',
  `can_buy` tinyint(1) NOT NULL COMMENT 'Whether or not this shop can buy items from shoppers. When false, the shop only sells items (users cannot sell to it).',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The shops.';

/*Data for the table `shop` */

insert  into `shop`(`id`,`name`,`can_buy`) values (0,'Test Shop',1);

/*Table structure for table `shop_item` */

DROP TABLE IF EXISTS `shop_item`;

CREATE TABLE `shop_item` (
  `shop_id` smallint(5) unsigned NOT NULL COMMENT 'The shop that the item is for.',
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item template that this shop sells. Item instantiated when sold to shopper.',
  PRIMARY KEY (`shop_id`,`item_template_id`),
  KEY `item_template_id` (`item_template_id`),
  CONSTRAINT `shop_item_ibfk_1` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `shop_item_ibfk_2` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='The items in a shop''s inventory.';

/*Data for the table `shop_item` */

insert  into `shop_item`(`shop_id`,`item_template_id`) values (0,1),(0,2),(0,3),(0,5),(0,6),(0,7);

/*Table structure for table `world_stats_count_consume_item` */

DROP TABLE IF EXISTS `world_stats_count_consume_item`;

CREATE TABLE `world_stats_count_consume_item` (
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item template the counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'Number of times items of this template have been consumed.',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_consume_item_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Counts number of time an use-once item has been consumed.';

/*Data for the table `world_stats_count_consume_item` */

insert  into `world_stats_count_consume_item`(`item_template_id`,`count`,`last_update`) values (1,11,'2013-02-09 23:31:56'),(2,14,'2013-02-09 22:25:18');

/*Table structure for table `world_stats_count_item_buy` */

DROP TABLE IF EXISTS `world_stats_count_item_buy`;

CREATE TABLE `world_stats_count_item_buy` (
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The template of the item that this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'The amount of this item that has been purchased from shops. When buying in bulk, this still updates by amount bought (so number of items purchased, not individual transactions).',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_item_buy_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Counts times an item has been purchased from a shop.';

/*Data for the table `world_stats_count_item_buy` */

insert  into `world_stats_count_item_buy`(`item_template_id`,`count`,`last_update`) values (5,2,'2013-02-06 00:48:10');

/*Table structure for table `world_stats_count_item_create` */

DROP TABLE IF EXISTS `world_stats_count_item_create`;

CREATE TABLE `world_stats_count_item_create` (
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item template this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'The total number of times this item has been instantiated. When instantiating multiple items at once, this is incremented by the amount of the item, not just one.',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_item_create_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Counts number of times an item has been instantiated.';

/*Data for the table `world_stats_count_item_create` */

insert  into `world_stats_count_item_create`(`item_template_id`,`count`,`last_update`) values (1,1288,'2013-02-10 15:56:03'),(2,1411,'2013-02-10 15:55:52'),(3,1039,'2013-02-10 15:56:39'),(4,2,'2013-02-04 22:59:25'),(5,1381,'2013-02-10 15:56:13'),(6,146,'2013-02-10 15:54:32'),(7,34112,'2013-02-10 15:56:26'),(8,13767,'2013-02-10 15:56:32');

/*Table structure for table `world_stats_count_item_sell` */

DROP TABLE IF EXISTS `world_stats_count_item_sell`;

CREATE TABLE `world_stats_count_item_sell` (
  `item_template_id` smallint(5) unsigned NOT NULL COMMENT 'The item template this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'Amount of this item template that has been sold to stores.',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`item_template_id`),
  CONSTRAINT `world_stats_count_item_sell_ibfk_1` FOREIGN KEY (`item_template_id`) REFERENCES `item_template` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Counts number of times shopper has sold item to store.';

/*Data for the table `world_stats_count_item_sell` */

/*Table structure for table `world_stats_count_npc_kill_user` */

DROP TABLE IF EXISTS `world_stats_count_npc_kill_user`;

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

/*Data for the table `world_stats_count_npc_kill_user` */

insert  into `world_stats_count_npc_kill_user`(`user_id`,`npc_template_id`,`count`,`last_update`) values (1,1,17,'2013-02-10 14:30:50'),(6,1,3,'2013-02-04 18:55:47');

/*Table structure for table `world_stats_count_shop_buy` */

DROP TABLE IF EXISTS `world_stats_count_shop_buy`;

CREATE TABLE `world_stats_count_shop_buy` (
  `shop_id` smallint(5) unsigned NOT NULL COMMENT 'The shop this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'The number of times this shop has sold (shopper has bought from this shop).',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`shop_id`),
  CONSTRAINT `world_stats_count_shop_buy_ibfk_2` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Counts number of items a shop has sold to shopper.';

/*Data for the table `world_stats_count_shop_buy` */

insert  into `world_stats_count_shop_buy`(`shop_id`,`count`,`last_update`) values (0,2,'2013-02-06 00:48:10');

/*Table structure for table `world_stats_count_shop_sell` */

DROP TABLE IF EXISTS `world_stats_count_shop_sell`;

CREATE TABLE `world_stats_count_shop_sell` (
  `shop_id` smallint(5) unsigned NOT NULL COMMENT 'The shop this counter is for.',
  `count` int(11) NOT NULL DEFAULT '0' COMMENT 'The number of times this shop has purchased items (shopper has sold to this shop).',
  `last_update` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this counter was last updated.',
  PRIMARY KEY (`shop_id`),
  CONSTRAINT `world_stats_count_shop_sell_ibfk_2` FOREIGN KEY (`shop_id`) REFERENCES `shop` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Counts number of items a shop has purchased from shopper.';

/*Data for the table `world_stats_count_shop_sell` */

/*Table structure for table `world_stats_count_user_consume_item` */

DROP TABLE IF EXISTS `world_stats_count_user_consume_item`;

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

/*Data for the table `world_stats_count_user_consume_item` */

insert  into `world_stats_count_user_consume_item`(`user_id`,`item_template_id`,`count`,`last_update`) values (1,1,11,'2013-02-09 23:31:56'),(1,2,14,'2013-02-09 22:25:18');

/*Table structure for table `world_stats_count_user_kill_npc` */

DROP TABLE IF EXISTS `world_stats_count_user_kill_npc`;

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

/*Data for the table `world_stats_count_user_kill_npc` */

insert  into `world_stats_count_user_kill_npc`(`user_id`,`npc_template_id`,`count`,`last_update`) values (1,1,350,'2013-02-10 15:56:13'),(1,6,250,'2013-02-10 15:55:26'),(6,1,4,'2013-02-09 21:18:09');

/*Table structure for table `world_stats_guild_user_change` */

DROP TABLE IF EXISTS `world_stats_guild_user_change`;

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

/*Data for the table `world_stats_guild_user_change` */

/*Table structure for table `world_stats_network` */

DROP TABLE IF EXISTS `world_stats_network`;

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
) ENGINE=MyISAM AUTO_INCREMENT=1820 DEFAULT CHARSET=latin1 COMMENT='Snapshots of network deltas.';

/*Data for the table `world_stats_network` */

insert  into `world_stats_network`(`id`,`when`,`connections`,`recv_bytes`,`recv_packets`,`recv_messages`,`sent_bytes`,`sent_packets`,`sent_messages`) values (1,'2013-02-04 17:51:41',1,0,0,0,0,0,0),(2,'2013-02-04 17:52:41',1,0,0,0,0,0,0),(3,'2013-02-04 17:53:41',0,0,0,0,0,0,0),(4,'2013-02-04 18:41:09',1,0,0,0,0,0,0),(5,'2013-02-04 18:42:09',1,0,0,0,0,0,0),(6,'2013-02-04 18:43:09',1,0,0,0,0,0,0),(7,'2013-02-04 18:54:38',1,0,0,0,0,0,0),(8,'2013-02-04 18:55:38',2,0,0,0,0,0,0),(9,'2013-02-04 18:56:38',1,0,0,0,0,0,0),(10,'2013-02-04 18:59:16',0,0,0,0,0,0,0),(11,'2013-02-04 19:10:17',2,0,0,0,0,0,0),(12,'2013-02-04 19:11:17',0,0,0,0,0,0,0),(13,'2013-02-04 19:31:09',1,0,0,0,0,0,0),(14,'2013-02-04 19:32:23',0,0,0,0,0,0,0),(15,'2013-02-04 20:10:26',1,0,0,0,0,0,0),(16,'2013-02-04 20:11:26',0,0,0,0,0,0,0),(17,'2013-02-04 20:21:22',0,0,0,0,0,0,0),(18,'2013-02-04 20:22:22',0,0,0,0,0,0,0),(19,'2013-02-04 20:23:22',0,0,0,0,0,0,0),(20,'2013-02-04 20:26:05',1,0,0,0,0,0,0),(21,'2013-02-04 20:27:05',0,0,0,0,0,0,0),(22,'2013-02-04 20:28:19',0,0,0,0,0,0,0),(23,'2013-02-04 20:34:59',1,0,0,0,0,0,0),(24,'2013-02-04 20:35:59',0,0,0,0,0,0,0),(25,'2013-02-04 21:14:08',1,0,0,0,0,0,0),(26,'2013-02-04 21:15:08',1,0,0,0,0,0,0),(27,'2013-02-04 21:22:54',1,0,0,0,0,0,0),(28,'2013-02-04 21:30:06',1,0,0,0,0,0,0),(29,'2013-02-04 22:13:20',2,0,0,0,0,0,0),(30,'2013-02-04 22:14:33',1,0,0,0,0,0,0),(31,'2013-02-04 22:15:20',2,0,0,0,0,0,0),(32,'2013-02-04 22:27:29',1,0,0,0,0,0,0),(33,'2013-02-04 22:35:05',1,0,0,0,0,0,0),(34,'2013-02-04 22:38:39',1,0,0,0,0,0,0),(35,'2013-02-04 22:40:40',1,0,0,0,0,0,0),(36,'2013-02-04 22:41:40',1,0,0,0,0,0,0),(37,'2013-02-04 22:43:46',1,0,0,0,0,0,0),(38,'2013-02-04 23:00:25',0,0,0,0,0,0,0),(39,'2013-02-04 23:01:25',0,0,0,0,0,0,0),(40,'2013-02-04 23:02:25',0,0,0,0,0,0,0),(41,'2013-02-04 23:03:25',0,0,0,0,0,0,0),(42,'2013-02-04 23:04:25',0,0,0,0,0,0,0),(43,'2013-02-04 23:05:25',0,0,0,0,0,0,0),(44,'2013-02-04 23:06:25',0,0,0,0,0,0,0),(45,'2013-02-04 23:07:25',0,0,0,0,0,0,0),(46,'2013-02-04 23:08:25',0,0,0,0,0,0,0),(47,'2013-02-04 23:09:25',0,0,0,0,0,0,0),(48,'2013-02-04 23:10:25',0,0,0,0,0,0,0),(49,'2013-02-04 23:11:59',1,0,0,0,0,0,0),(50,'2013-02-04 23:12:59',1,0,0,0,0,0,0),(51,'2013-02-04 23:13:59',1,0,0,0,0,0,0),(52,'2013-02-04 23:14:59',1,0,0,0,0,0,0),(53,'2013-02-04 23:15:59',1,0,0,0,0,0,0),(54,'2013-02-04 23:16:59',1,0,0,0,0,0,0),(55,'2013-02-04 23:17:59',1,0,0,0,0,0,0),(56,'2013-02-04 23:18:59',1,0,0,0,0,0,0),(57,'2013-02-04 23:19:59',1,0,0,0,0,0,0),(58,'2013-02-04 23:20:59',1,0,0,0,0,0,0),(59,'2013-02-04 23:21:59',1,0,0,0,0,0,0),(60,'2013-02-04 23:24:42',1,0,0,0,0,0,0),(61,'2013-02-04 23:25:42',1,0,0,0,0,0,0),(62,'2013-02-04 23:26:42',1,0,0,0,0,0,0),(63,'2013-02-04 23:27:42',1,0,0,0,0,0,0),(64,'2013-02-04 23:28:42',0,0,0,0,0,0,0),(65,'2013-02-04 23:29:42',0,0,0,0,0,0,0),(66,'2013-02-04 23:30:42',0,0,0,0,0,0,0),(67,'2013-02-04 23:32:01',1,0,0,0,0,0,0),(68,'2013-02-04 23:33:01',1,0,0,0,0,0,0),(69,'2013-02-04 23:34:01',1,0,0,0,0,0,0),(70,'2013-02-04 23:47:00',1,0,0,0,0,0,0),(71,'2013-02-04 23:48:00',1,0,0,0,0,0,0),(72,'2013-02-04 23:49:00',1,0,0,0,0,0,0),(73,'2013-02-04 23:50:00',1,0,0,0,0,0,0),(74,'2013-02-04 23:51:00',0,0,0,0,0,0,0),(75,'2013-02-04 23:52:00',0,0,0,0,0,0,0),(76,'2013-02-04 23:53:00',0,0,0,0,0,0,0),(77,'2013-02-04 23:54:00',0,0,0,0,0,0,0),(78,'2013-02-04 23:55:00',0,0,0,0,0,0,0),(79,'2013-02-04 23:56:00',0,0,0,0,0,0,0),(80,'2013-02-04 23:57:00',0,0,0,0,0,0,0),(81,'2013-02-04 23:58:00',0,0,0,0,0,0,0),(82,'2013-02-04 23:59:00',0,0,0,0,0,0,0),(83,'2013-02-05 00:00:00',0,0,0,0,0,0,0),(84,'2013-02-05 01:17:36',1,0,0,0,0,0,0),(85,'2013-02-05 01:18:36',0,0,0,0,0,0,0),(86,'2013-02-05 01:19:36',0,0,0,0,0,0,0),(87,'2013-02-05 01:20:36',0,0,0,0,0,0,0),(88,'2013-02-05 01:21:36',0,0,0,0,0,0,0),(89,'2013-02-05 01:22:36',0,0,0,0,0,0,0),(90,'2013-02-05 01:23:36',0,0,0,0,0,0,0),(91,'2013-02-05 01:24:36',0,0,0,0,0,0,0),(92,'2013-02-05 01:25:36',0,0,0,0,0,0,0),(93,'2013-02-05 01:26:36',0,0,0,0,0,0,0),(94,'2013-02-05 01:27:36',0,0,0,0,0,0,0),(95,'2013-02-05 01:28:36',0,0,0,0,0,0,0),(96,'2013-02-05 01:29:36',0,0,0,0,0,0,0),(97,'2013-02-05 01:30:36',0,0,0,0,0,0,0),(98,'2013-02-05 01:31:36',0,0,0,0,0,0,0),(99,'2013-02-05 01:32:36',0,0,0,0,0,0,0),(100,'2013-02-05 01:33:36',0,0,0,0,0,0,0),(101,'2013-02-05 01:34:36',0,0,0,0,0,0,0),(102,'2013-02-05 01:35:36',0,0,0,0,0,0,0),(103,'2013-02-05 01:36:36',0,0,0,0,0,0,0),(104,'2013-02-05 01:37:36',0,0,0,0,0,0,0),(105,'2013-02-05 01:38:36',0,0,0,0,0,0,0),(106,'2013-02-05 01:39:36',0,0,0,0,0,0,0),(107,'2013-02-05 01:40:36',0,0,0,0,0,0,0),(108,'2013-02-05 01:41:36',0,0,0,0,0,0,0),(109,'2013-02-05 01:42:36',0,0,0,0,0,0,0),(110,'2013-02-05 01:43:36',0,0,0,0,0,0,0),(111,'2013-02-05 01:44:36',0,0,0,0,0,0,0),(112,'2013-02-05 01:45:36',0,0,0,0,0,0,0),(113,'2013-02-05 01:46:36',0,0,0,0,0,0,0),(114,'2013-02-05 01:47:36',0,0,0,0,0,0,0),(115,'2013-02-05 01:48:36',0,0,0,0,0,0,0),(116,'2013-02-05 01:49:36',0,0,0,0,0,0,0),(117,'2013-02-05 01:50:36',0,0,0,0,0,0,0),(118,'2013-02-05 01:51:36',0,0,0,0,0,0,0),(119,'2013-02-05 01:52:36',0,0,0,0,0,0,0),(120,'2013-02-05 01:53:36',0,0,0,0,0,0,0),(121,'2013-02-05 01:54:36',0,0,0,0,0,0,0),(122,'2013-02-05 01:55:36',0,0,0,0,0,0,0),(123,'2013-02-05 01:56:36',0,0,0,0,0,0,0),(124,'2013-02-05 01:57:36',0,0,0,0,0,0,0),(125,'2013-02-05 01:58:36',0,0,0,0,0,0,0),(126,'2013-02-05 02:00:13',2,0,0,0,0,0,0),(127,'2013-02-05 02:01:13',2,0,0,0,0,0,0),(128,'2013-02-05 02:02:13',2,0,0,0,0,0,0),(129,'2013-02-05 02:03:13',2,0,0,0,0,0,0),(130,'2013-02-05 02:04:13',2,0,0,0,0,0,0),(131,'2013-02-05 23:09:38',1,0,0,0,0,0,0),(132,'2013-02-05 23:27:13',0,0,0,0,0,0,0),(133,'2013-02-05 23:29:53',0,0,0,0,0,0,0),(134,'2013-02-05 23:34:41',1,0,0,0,0,0,0),(135,'2013-02-05 23:35:41',1,0,0,0,0,0,0),(136,'2013-02-05 23:36:41',1,0,0,0,0,0,0),(137,'2013-02-05 23:37:41',1,0,0,0,0,0,0),(138,'2013-02-05 23:38:41',1,0,0,0,0,0,0),(139,'2013-02-05 23:39:41',1,0,0,0,0,0,0),(140,'2013-02-06 00:09:10',1,0,0,0,0,0,0),(141,'2013-02-06 00:10:10',1,0,0,0,0,0,0),(142,'2013-02-06 00:28:51',2,0,0,0,0,0,0),(143,'2013-02-06 00:32:47',2,0,0,0,0,0,0),(144,'2013-02-06 00:47:54',1,0,0,0,0,0,0),(145,'2013-02-06 00:48:54',1,0,0,0,0,0,0),(146,'2013-02-06 01:14:17',1,0,0,0,0,0,0),(147,'2013-02-06 01:15:17',1,0,0,0,0,0,0),(148,'2013-02-06 01:16:17',1,0,0,0,0,0,0),(149,'2013-02-06 01:17:17',1,0,0,0,0,0,0),(150,'2013-02-06 01:18:17',0,0,0,0,0,0,0),(151,'2013-02-06 01:19:17',0,0,0,0,0,0,0),(152,'2013-02-06 01:20:17',0,0,0,0,0,0,0),(153,'2013-02-06 01:21:17',0,0,0,0,0,0,0),(154,'2013-02-06 01:22:17',0,0,0,0,0,0,0),(155,'2013-02-06 01:23:17',0,0,0,0,0,0,0),(156,'2013-02-06 01:24:56',1,0,0,0,0,0,0),(157,'2013-02-06 01:25:56',0,0,0,0,0,0,0),(158,'2013-02-06 01:26:56',0,0,0,0,0,0,0),(159,'2013-02-06 01:27:56',0,0,0,0,0,0,0),(160,'2013-02-06 01:28:56',0,0,0,0,0,0,0),(161,'2013-02-06 01:29:56',0,0,0,0,0,0,0),(162,'2013-02-06 01:30:56',0,0,0,0,0,0,0),(163,'2013-02-06 01:31:56',0,0,0,0,0,0,0),(164,'2013-02-06 01:32:56',0,0,0,0,0,0,0),(165,'2013-02-06 01:33:56',0,0,0,0,0,0,0),(166,'2013-02-06 01:34:56',0,0,0,0,0,0,0),(167,'2013-02-06 01:35:56',0,0,0,0,0,0,0),(168,'2013-02-06 01:36:56',0,0,0,0,0,0,0),(169,'2013-02-06 01:37:56',0,0,0,0,0,0,0),(170,'2013-02-06 01:38:56',0,0,0,0,0,0,0),(171,'2013-02-06 01:39:56',0,0,0,0,0,0,0),(172,'2013-02-06 01:40:56',0,0,0,0,0,0,0),(173,'2013-02-06 01:41:56',0,0,0,0,0,0,0),(174,'2013-02-06 01:42:56',0,0,0,0,0,0,0),(175,'2013-02-06 01:43:56',0,0,0,0,0,0,0),(176,'2013-02-06 01:44:56',0,0,0,0,0,0,0),(177,'2013-02-06 01:45:56',0,0,0,0,0,0,0),(178,'2013-02-06 01:46:56',0,0,0,0,0,0,0),(179,'2013-02-06 01:47:56',0,0,0,0,0,0,0),(180,'2013-02-06 01:48:56',0,0,0,0,0,0,0),(181,'2013-02-06 01:49:56',0,0,0,0,0,0,0),(182,'2013-02-06 01:50:56',0,0,0,0,0,0,0),(183,'2013-02-06 01:51:56',0,0,0,0,0,0,0),(184,'2013-02-06 01:52:56',0,0,0,0,0,0,0),(185,'2013-02-06 01:53:56',0,0,0,0,0,0,0),(186,'2013-02-06 01:54:56',0,0,0,0,0,0,0),(187,'2013-02-06 01:55:56',0,0,0,0,0,0,0),(188,'2013-02-06 01:56:56',0,0,0,0,0,0,0),(189,'2013-02-06 01:57:56',0,0,0,0,0,0,0),(190,'2013-02-06 01:58:56',0,0,0,0,0,0,0),(191,'2013-02-06 01:59:56',0,0,0,0,0,0,0),(192,'2013-02-06 02:00:56',0,0,0,0,0,0,0),(193,'2013-02-06 02:01:56',0,0,0,0,0,0,0),(194,'2013-02-06 02:02:56',0,0,0,0,0,0,0),(195,'2013-02-06 02:03:56',0,0,0,0,0,0,0),(196,'2013-02-06 02:04:56',0,0,0,0,0,0,0),(197,'2013-02-06 02:05:56',0,0,0,0,0,0,0),(198,'2013-02-06 02:06:56',0,0,0,0,0,0,0),(199,'2013-02-06 02:07:56',0,0,0,0,0,0,0),(200,'2013-02-06 02:08:56',0,0,0,0,0,0,0),(201,'2013-02-06 02:09:56',0,0,0,0,0,0,0),(202,'2013-02-06 02:10:56',0,0,0,0,0,0,0),(203,'2013-02-06 02:11:56',0,0,0,0,0,0,0),(204,'2013-02-06 02:12:56',0,0,0,0,0,0,0),(205,'2013-02-06 02:13:56',0,0,0,0,0,0,0),(206,'2013-02-06 02:14:56',0,0,0,0,0,0,0),(207,'2013-02-06 02:15:56',0,0,0,0,0,0,0),(208,'2013-02-06 02:16:56',0,0,0,0,0,0,0),(209,'2013-02-06 02:17:56',0,0,0,0,0,0,0),(210,'2013-02-06 02:18:56',0,0,0,0,0,0,0),(211,'2013-02-06 02:19:56',0,0,0,0,0,0,0),(212,'2013-02-06 02:20:56',0,0,0,0,0,0,0),(213,'2013-02-06 02:21:56',0,0,0,0,0,0,0),(214,'2013-02-06 02:22:56',0,0,0,0,0,0,0),(215,'2013-02-06 02:23:56',0,0,0,0,0,0,0),(216,'2013-02-06 02:24:56',0,0,0,0,0,0,0),(217,'2013-02-06 02:25:56',0,0,0,0,0,0,0),(218,'2013-02-06 02:26:56',0,0,0,0,0,0,0),(219,'2013-02-06 02:27:56',0,0,0,0,0,0,0),(220,'2013-02-06 02:28:56',0,0,0,0,0,0,0),(221,'2013-02-06 02:29:56',0,0,0,0,0,0,0),(222,'2013-02-06 02:30:56',0,0,0,0,0,0,0),(223,'2013-02-06 02:31:56',0,0,0,0,0,0,0),(224,'2013-02-06 02:32:56',0,0,0,0,0,0,0),(225,'2013-02-06 02:33:56',0,0,0,0,0,0,0),(226,'2013-02-06 02:34:56',0,0,0,0,0,0,0),(227,'2013-02-06 02:35:56',0,0,0,0,0,0,0),(228,'2013-02-06 02:36:56',0,0,0,0,0,0,0),(229,'2013-02-06 02:37:56',0,0,0,0,0,0,0),(230,'2013-02-06 02:38:56',0,0,0,0,0,0,0),(231,'2013-02-06 02:39:56',0,0,0,0,0,0,0),(232,'2013-02-06 02:40:56',0,0,0,0,0,0,0),(233,'2013-02-06 02:41:56',0,0,0,0,0,0,0),(234,'2013-02-06 02:42:56',0,0,0,0,0,0,0),(235,'2013-02-06 02:43:56',0,0,0,0,0,0,0),(236,'2013-02-06 02:44:56',0,0,0,0,0,0,0),(237,'2013-02-06 02:45:56',0,0,0,0,0,0,0),(238,'2013-02-06 02:46:56',0,0,0,0,0,0,0),(239,'2013-02-06 02:47:56',0,0,0,0,0,0,0),(240,'2013-02-06 02:48:56',0,0,0,0,0,0,0),(241,'2013-02-06 02:49:56',0,0,0,0,0,0,0),(242,'2013-02-06 02:50:56',0,0,0,0,0,0,0),(243,'2013-02-06 02:51:56',0,0,0,0,0,0,0),(244,'2013-02-06 02:52:56',0,0,0,0,0,0,0),(245,'2013-02-06 02:53:56',0,0,0,0,0,0,0),(246,'2013-02-06 02:54:56',0,0,0,0,0,0,0),(247,'2013-02-06 02:55:56',0,0,0,0,0,0,0),(248,'2013-02-06 02:56:56',0,0,0,0,0,0,0),(249,'2013-02-06 02:57:56',0,0,0,0,0,0,0),(250,'2013-02-06 02:58:56',0,0,0,0,0,0,0),(251,'2013-02-06 02:59:56',0,0,0,0,0,0,0),(252,'2013-02-06 03:00:56',0,0,0,0,0,0,0),(253,'2013-02-06 03:01:56',0,0,0,0,0,0,0),(254,'2013-02-06 03:02:56',0,0,0,0,0,0,0),(255,'2013-02-06 03:03:56',0,0,0,0,0,0,0),(256,'2013-02-06 03:04:56',0,0,0,0,0,0,0),(257,'2013-02-06 03:05:56',0,0,0,0,0,0,0),(258,'2013-02-06 03:06:56',0,0,0,0,0,0,0),(259,'2013-02-06 03:07:56',0,0,0,0,0,0,0),(260,'2013-02-06 03:08:56',0,0,0,0,0,0,0),(261,'2013-02-06 03:09:56',0,0,0,0,0,0,0),(262,'2013-02-06 03:10:56',0,0,0,0,0,0,0),(263,'2013-02-06 03:11:56',0,0,0,0,0,0,0),(264,'2013-02-06 03:12:56',0,0,0,0,0,0,0),(265,'2013-02-06 03:13:56',0,0,0,0,0,0,0),(266,'2013-02-06 03:14:56',0,0,0,0,0,0,0),(267,'2013-02-06 03:15:56',0,0,0,0,0,0,0),(268,'2013-02-06 03:16:56',0,0,0,0,0,0,0),(269,'2013-02-06 03:17:56',0,0,0,0,0,0,0),(270,'2013-02-06 03:18:56',0,0,0,0,0,0,0),(271,'2013-02-06 03:19:56',0,0,0,0,0,0,0),(272,'2013-02-06 03:20:56',0,0,0,0,0,0,0),(273,'2013-02-06 03:21:56',0,0,0,0,0,0,0),(274,'2013-02-06 03:22:56',0,0,0,0,0,0,0),(275,'2013-02-06 03:23:56',0,0,0,0,0,0,0),(276,'2013-02-06 03:24:56',0,0,0,0,0,0,0),(277,'2013-02-06 03:25:56',0,0,0,0,0,0,0),(278,'2013-02-06 03:26:56',0,0,0,0,0,0,0),(279,'2013-02-06 03:27:56',0,0,0,0,0,0,0),(280,'2013-02-06 03:28:56',0,0,0,0,0,0,0),(281,'2013-02-06 03:29:56',0,0,0,0,0,0,0),(282,'2013-02-06 03:30:56',0,0,0,0,0,0,0),(283,'2013-02-06 03:31:56',0,0,0,0,0,0,0),(284,'2013-02-06 03:32:56',0,0,0,0,0,0,0),(285,'2013-02-06 03:33:56',0,0,0,0,0,0,0),(286,'2013-02-06 03:34:56',0,0,0,0,0,0,0),(287,'2013-02-06 03:35:56',0,0,0,0,0,0,0),(288,'2013-02-06 03:36:56',0,0,0,0,0,0,0),(289,'2013-02-06 03:37:56',0,0,0,0,0,0,0),(290,'2013-02-06 03:38:56',0,0,0,0,0,0,0),(291,'2013-02-06 03:39:56',0,0,0,0,0,0,0),(292,'2013-02-06 03:40:56',0,0,0,0,0,0,0),(293,'2013-02-06 03:41:56',0,0,0,0,0,0,0),(294,'2013-02-06 03:42:56',0,0,0,0,0,0,0),(295,'2013-02-06 03:43:56',0,0,0,0,0,0,0),(296,'2013-02-06 03:44:56',0,0,0,0,0,0,0),(297,'2013-02-06 03:45:56',0,0,0,0,0,0,0),(298,'2013-02-06 03:46:56',0,0,0,0,0,0,0),(299,'2013-02-06 03:47:56',0,0,0,0,0,0,0),(300,'2013-02-06 03:48:56',0,0,0,0,0,0,0),(301,'2013-02-06 03:49:56',0,0,0,0,0,0,0),(302,'2013-02-06 03:50:56',0,0,0,0,0,0,0),(303,'2013-02-06 03:51:56',0,0,0,0,0,0,0),(304,'2013-02-06 03:52:56',0,0,0,0,0,0,0),(305,'2013-02-06 03:53:56',0,0,0,0,0,0,0),(306,'2013-02-06 03:54:56',0,0,0,0,0,0,0),(307,'2013-02-06 03:55:56',0,0,0,0,0,0,0),(308,'2013-02-06 03:56:56',0,0,0,0,0,0,0),(309,'2013-02-06 03:57:56',0,0,0,0,0,0,0),(310,'2013-02-06 03:58:56',0,0,0,0,0,0,0),(311,'2013-02-06 03:59:56',0,0,0,0,0,0,0),(312,'2013-02-06 04:00:56',0,0,0,0,0,0,0),(313,'2013-02-06 04:01:56',0,0,0,0,0,0,0),(314,'2013-02-06 04:02:56',0,0,0,0,0,0,0),(315,'2013-02-06 04:03:56',0,0,0,0,0,0,0),(316,'2013-02-06 04:04:56',0,0,0,0,0,0,0),(317,'2013-02-06 04:05:56',0,0,0,0,0,0,0),(318,'2013-02-06 04:06:56',0,0,0,0,0,0,0),(319,'2013-02-06 04:07:56',0,0,0,0,0,0,0),(320,'2013-02-06 04:08:56',0,0,0,0,0,0,0),(321,'2013-02-06 04:09:56',0,0,0,0,0,0,0),(322,'2013-02-06 04:10:56',0,0,0,0,0,0,0),(323,'2013-02-06 04:11:56',0,0,0,0,0,0,0),(324,'2013-02-06 04:12:56',0,0,0,0,0,0,0),(325,'2013-02-06 04:13:56',0,0,0,0,0,0,0),(326,'2013-02-06 04:14:56',0,0,0,0,0,0,0),(327,'2013-02-06 04:15:56',0,0,0,0,0,0,0),(328,'2013-02-06 04:16:56',0,0,0,0,0,0,0),(329,'2013-02-06 04:17:56',0,0,0,0,0,0,0),(330,'2013-02-06 04:18:56',0,0,0,0,0,0,0),(331,'2013-02-06 04:19:56',0,0,0,0,0,0,0),(332,'2013-02-06 04:20:56',0,0,0,0,0,0,0),(333,'2013-02-06 04:21:56',0,0,0,0,0,0,0),(334,'2013-02-06 04:22:56',0,0,0,0,0,0,0),(335,'2013-02-06 04:23:56',0,0,0,0,0,0,0),(336,'2013-02-06 04:24:56',0,0,0,0,0,0,0),(337,'2013-02-06 04:25:56',0,0,0,0,0,0,0),(338,'2013-02-06 04:26:56',0,0,0,0,0,0,0),(339,'2013-02-06 04:27:56',0,0,0,0,0,0,0),(340,'2013-02-06 04:28:56',0,0,0,0,0,0,0),(341,'2013-02-06 04:29:56',0,0,0,0,0,0,0),(342,'2013-02-06 04:30:56',0,0,0,0,0,0,0),(343,'2013-02-06 04:31:56',0,0,0,0,0,0,0),(344,'2013-02-06 04:32:56',0,0,0,0,0,0,0),(345,'2013-02-06 04:33:56',0,0,0,0,0,0,0),(346,'2013-02-06 04:34:56',0,0,0,0,0,0,0),(347,'2013-02-06 04:35:56',0,0,0,0,0,0,0),(348,'2013-02-06 04:36:56',0,0,0,0,0,0,0),(349,'2013-02-06 04:37:56',0,0,0,0,0,0,0),(350,'2013-02-06 04:38:56',0,0,0,0,0,0,0),(351,'2013-02-06 04:39:56',0,0,0,0,0,0,0),(352,'2013-02-06 04:40:56',0,0,0,0,0,0,0),(353,'2013-02-06 04:41:56',0,0,0,0,0,0,0),(354,'2013-02-06 04:42:56',0,0,0,0,0,0,0),(355,'2013-02-06 04:43:56',0,0,0,0,0,0,0),(356,'2013-02-06 04:44:56',0,0,0,0,0,0,0),(357,'2013-02-06 04:45:56',0,0,0,0,0,0,0),(358,'2013-02-06 04:46:56',0,0,0,0,0,0,0),(359,'2013-02-06 04:47:56',0,0,0,0,0,0,0),(360,'2013-02-06 04:48:56',0,0,0,0,0,0,0),(361,'2013-02-06 04:49:56',0,0,0,0,0,0,0),(362,'2013-02-06 04:50:56',0,0,0,0,0,0,0),(363,'2013-02-06 04:51:56',0,0,0,0,0,0,0),(364,'2013-02-06 04:52:56',0,0,0,0,0,0,0),(365,'2013-02-06 04:53:56',0,0,0,0,0,0,0),(366,'2013-02-06 04:54:56',0,0,0,0,0,0,0),(367,'2013-02-06 04:55:56',0,0,0,0,0,0,0),(368,'2013-02-06 04:56:56',0,0,0,0,0,0,0),(369,'2013-02-06 04:57:56',0,0,0,0,0,0,0),(370,'2013-02-06 04:58:56',0,0,0,0,0,0,0),(371,'2013-02-06 04:59:56',0,0,0,0,0,0,0),(372,'2013-02-06 05:00:56',0,0,0,0,0,0,0),(373,'2013-02-06 05:01:56',0,0,0,0,0,0,0),(374,'2013-02-06 05:02:56',0,0,0,0,0,0,0),(375,'2013-02-06 05:03:56',0,0,0,0,0,0,0),(376,'2013-02-06 05:04:56',0,0,0,0,0,0,0),(377,'2013-02-06 05:05:56',0,0,0,0,0,0,0),(378,'2013-02-06 05:06:56',0,0,0,0,0,0,0),(379,'2013-02-06 05:07:56',0,0,0,0,0,0,0),(380,'2013-02-06 05:08:56',0,0,0,0,0,0,0),(381,'2013-02-06 05:09:56',0,0,0,0,0,0,0),(382,'2013-02-06 05:10:56',0,0,0,0,0,0,0),(383,'2013-02-06 05:11:56',0,0,0,0,0,0,0),(384,'2013-02-06 05:12:56',0,0,0,0,0,0,0),(385,'2013-02-06 05:13:56',0,0,0,0,0,0,0),(386,'2013-02-06 05:14:56',0,0,0,0,0,0,0),(387,'2013-02-06 05:15:56',0,0,0,0,0,0,0),(388,'2013-02-06 05:16:56',0,0,0,0,0,0,0),(389,'2013-02-06 05:17:56',0,0,0,0,0,0,0),(390,'2013-02-06 05:18:56',0,0,0,0,0,0,0),(391,'2013-02-06 05:19:56',0,0,0,0,0,0,0),(392,'2013-02-06 05:20:56',0,0,0,0,0,0,0),(393,'2013-02-06 05:21:56',0,0,0,0,0,0,0),(394,'2013-02-06 05:22:56',0,0,0,0,0,0,0),(395,'2013-02-06 05:23:56',0,0,0,0,0,0,0),(396,'2013-02-06 05:24:56',0,0,0,0,0,0,0),(397,'2013-02-06 05:25:56',0,0,0,0,0,0,0),(398,'2013-02-06 05:26:56',0,0,0,0,0,0,0),(399,'2013-02-06 05:27:56',0,0,0,0,0,0,0),(400,'2013-02-06 05:28:56',0,0,0,0,0,0,0),(401,'2013-02-06 05:29:56',0,0,0,0,0,0,0),(402,'2013-02-06 05:30:56',0,0,0,0,0,0,0),(403,'2013-02-06 05:31:56',0,0,0,0,0,0,0),(404,'2013-02-06 05:32:56',0,0,0,0,0,0,0),(405,'2013-02-06 05:33:56',0,0,0,0,0,0,0),(406,'2013-02-06 05:34:56',0,0,0,0,0,0,0),(407,'2013-02-06 05:35:56',0,0,0,0,0,0,0),(408,'2013-02-06 05:36:56',0,0,0,0,0,0,0),(409,'2013-02-06 05:37:56',0,0,0,0,0,0,0),(410,'2013-02-06 05:38:56',0,0,0,0,0,0,0),(411,'2013-02-06 05:39:56',0,0,0,0,0,0,0),(412,'2013-02-06 05:40:56',0,0,0,0,0,0,0),(413,'2013-02-06 05:41:56',0,0,0,0,0,0,0),(414,'2013-02-06 05:42:56',0,0,0,0,0,0,0),(415,'2013-02-06 05:43:56',0,0,0,0,0,0,0),(416,'2013-02-06 05:44:56',0,0,0,0,0,0,0),(417,'2013-02-06 05:45:56',0,0,0,0,0,0,0),(418,'2013-02-06 05:46:56',0,0,0,0,0,0,0),(419,'2013-02-06 05:47:56',0,0,0,0,0,0,0),(420,'2013-02-06 05:48:56',0,0,0,0,0,0,0),(421,'2013-02-06 05:49:56',0,0,0,0,0,0,0),(422,'2013-02-06 05:50:56',0,0,0,0,0,0,0),(423,'2013-02-06 05:51:56',0,0,0,0,0,0,0),(424,'2013-02-06 05:52:56',0,0,0,0,0,0,0),(425,'2013-02-06 05:53:56',0,0,0,0,0,0,0),(426,'2013-02-06 05:54:56',0,0,0,0,0,0,0),(427,'2013-02-06 05:55:56',0,0,0,0,0,0,0),(428,'2013-02-06 05:56:56',0,0,0,0,0,0,0),(429,'2013-02-06 05:57:56',0,0,0,0,0,0,0),(430,'2013-02-06 05:58:56',0,0,0,0,0,0,0),(431,'2013-02-06 05:59:56',0,0,0,0,0,0,0),(432,'2013-02-06 06:00:56',0,0,0,0,0,0,0),(433,'2013-02-06 06:01:56',0,0,0,0,0,0,0),(434,'2013-02-06 06:02:56',0,0,0,0,0,0,0),(435,'2013-02-06 06:03:56',0,0,0,0,0,0,0),(436,'2013-02-06 06:04:56',0,0,0,0,0,0,0),(437,'2013-02-06 06:05:56',0,0,0,0,0,0,0),(438,'2013-02-06 06:06:56',0,0,0,0,0,0,0),(439,'2013-02-06 06:07:56',0,0,0,0,0,0,0),(440,'2013-02-06 06:08:56',0,0,0,0,0,0,0),(441,'2013-02-06 06:09:56',0,0,0,0,0,0,0),(442,'2013-02-06 06:10:56',0,0,0,0,0,0,0),(443,'2013-02-06 06:11:56',0,0,0,0,0,0,0),(444,'2013-02-06 06:12:56',0,0,0,0,0,0,0),(445,'2013-02-06 06:13:56',0,0,0,0,0,0,0),(446,'2013-02-06 06:14:56',0,0,0,0,0,0,0),(447,'2013-02-06 06:15:56',0,0,0,0,0,0,0),(448,'2013-02-06 06:16:56',0,0,0,0,0,0,0),(449,'2013-02-06 06:17:56',0,0,0,0,0,0,0),(450,'2013-02-06 06:18:56',0,0,0,0,0,0,0),(451,'2013-02-06 06:19:56',0,0,0,0,0,0,0),(452,'2013-02-06 06:20:56',0,0,0,0,0,0,0),(453,'2013-02-06 06:21:56',0,0,0,0,0,0,0),(454,'2013-02-06 06:22:56',0,0,0,0,0,0,0),(455,'2013-02-06 06:23:56',0,0,0,0,0,0,0),(456,'2013-02-06 06:24:56',0,0,0,0,0,0,0),(457,'2013-02-06 06:25:56',0,0,0,0,0,0,0),(458,'2013-02-06 06:26:56',0,0,0,0,0,0,0),(459,'2013-02-06 06:27:56',0,0,0,0,0,0,0),(460,'2013-02-06 06:28:56',0,0,0,0,0,0,0),(461,'2013-02-06 06:29:56',0,0,0,0,0,0,0),(462,'2013-02-06 06:30:56',0,0,0,0,0,0,0),(463,'2013-02-06 06:31:56',0,0,0,0,0,0,0),(464,'2013-02-06 06:32:56',0,0,0,0,0,0,0),(465,'2013-02-06 06:33:56',0,0,0,0,0,0,0),(466,'2013-02-06 06:34:56',0,0,0,0,0,0,0),(467,'2013-02-06 06:35:56',0,0,0,0,0,0,0),(468,'2013-02-06 06:36:56',0,0,0,0,0,0,0),(469,'2013-02-06 06:37:56',0,0,0,0,0,0,0),(470,'2013-02-06 06:38:56',0,0,0,0,0,0,0),(471,'2013-02-06 06:39:56',0,0,0,0,0,0,0),(472,'2013-02-06 06:40:56',0,0,0,0,0,0,0),(473,'2013-02-06 06:41:56',0,0,0,0,0,0,0),(474,'2013-02-06 06:42:56',0,0,0,0,0,0,0),(475,'2013-02-06 06:43:56',0,0,0,0,0,0,0),(476,'2013-02-06 06:44:56',0,0,0,0,0,0,0),(477,'2013-02-06 06:45:56',0,0,0,0,0,0,0),(478,'2013-02-06 06:46:56',0,0,0,0,0,0,0),(479,'2013-02-06 06:47:56',0,0,0,0,0,0,0),(480,'2013-02-06 06:48:56',0,0,0,0,0,0,0),(481,'2013-02-06 06:49:56',0,0,0,0,0,0,0),(482,'2013-02-06 06:50:56',0,0,0,0,0,0,0),(483,'2013-02-06 06:51:56',0,0,0,0,0,0,0),(484,'2013-02-06 06:52:56',0,0,0,0,0,0,0),(485,'2013-02-06 06:53:56',0,0,0,0,0,0,0),(486,'2013-02-06 06:54:56',0,0,0,0,0,0,0),(487,'2013-02-06 06:55:56',0,0,0,0,0,0,0),(488,'2013-02-06 06:56:56',0,0,0,0,0,0,0),(489,'2013-02-06 06:57:56',0,0,0,0,0,0,0),(490,'2013-02-06 06:58:56',0,0,0,0,0,0,0),(491,'2013-02-06 06:59:56',0,0,0,0,0,0,0),(492,'2013-02-06 07:00:56',0,0,0,0,0,0,0),(493,'2013-02-06 07:01:56',0,0,0,0,0,0,0),(494,'2013-02-06 07:02:56',0,0,0,0,0,0,0),(495,'2013-02-06 07:03:56',0,0,0,0,0,0,0),(496,'2013-02-06 07:04:56',0,0,0,0,0,0,0),(497,'2013-02-06 07:05:56',0,0,0,0,0,0,0),(498,'2013-02-06 07:06:56',0,0,0,0,0,0,0),(499,'2013-02-06 07:07:56',0,0,0,0,0,0,0),(500,'2013-02-06 07:08:56',0,0,0,0,0,0,0),(501,'2013-02-06 07:09:56',0,0,0,0,0,0,0),(502,'2013-02-06 07:10:56',0,0,0,0,0,0,0),(503,'2013-02-06 07:11:56',0,0,0,0,0,0,0),(504,'2013-02-06 07:12:56',0,0,0,0,0,0,0),(505,'2013-02-06 07:13:56',0,0,0,0,0,0,0),(506,'2013-02-06 07:14:56',0,0,0,0,0,0,0),(507,'2013-02-06 07:15:56',0,0,0,0,0,0,0),(508,'2013-02-06 07:16:56',0,0,0,0,0,0,0),(509,'2013-02-06 07:17:56',0,0,0,0,0,0,0),(510,'2013-02-06 07:18:56',0,0,0,0,0,0,0),(511,'2013-02-06 07:19:56',0,0,0,0,0,0,0),(512,'2013-02-06 07:20:56',0,0,0,0,0,0,0),(513,'2013-02-06 07:21:56',0,0,0,0,0,0,0),(514,'2013-02-06 07:22:56',0,0,0,0,0,0,0),(515,'2013-02-06 07:23:56',0,0,0,0,0,0,0),(516,'2013-02-06 07:24:56',0,0,0,0,0,0,0),(517,'2013-02-06 07:25:56',0,0,0,0,0,0,0),(518,'2013-02-06 07:26:56',0,0,0,0,0,0,0),(519,'2013-02-06 07:27:56',0,0,0,0,0,0,0),(520,'2013-02-06 07:28:56',0,0,0,0,0,0,0),(521,'2013-02-06 07:29:56',0,0,0,0,0,0,0),(522,'2013-02-06 07:30:56',0,0,0,0,0,0,0),(523,'2013-02-06 07:31:56',0,0,0,0,0,0,0),(524,'2013-02-06 07:32:56',0,0,0,0,0,0,0),(525,'2013-02-06 07:33:56',0,0,0,0,0,0,0),(526,'2013-02-06 07:34:56',0,0,0,0,0,0,0),(527,'2013-02-06 07:35:56',0,0,0,0,0,0,0),(528,'2013-02-06 07:36:56',0,0,0,0,0,0,0),(529,'2013-02-06 07:37:56',0,0,0,0,0,0,0),(530,'2013-02-06 07:38:56',0,0,0,0,0,0,0),(531,'2013-02-06 07:39:56',0,0,0,0,0,0,0),(532,'2013-02-06 07:40:56',0,0,0,0,0,0,0),(533,'2013-02-06 07:41:56',0,0,0,0,0,0,0),(534,'2013-02-06 07:42:56',0,0,0,0,0,0,0),(535,'2013-02-06 07:43:56',0,0,0,0,0,0,0),(536,'2013-02-06 07:44:56',0,0,0,0,0,0,0),(537,'2013-02-06 07:45:56',0,0,0,0,0,0,0),(538,'2013-02-06 07:46:56',0,0,0,0,0,0,0),(539,'2013-02-06 07:47:56',0,0,0,0,0,0,0),(540,'2013-02-06 07:48:56',0,0,0,0,0,0,0),(541,'2013-02-06 07:49:56',0,0,0,0,0,0,0),(542,'2013-02-06 07:50:56',0,0,0,0,0,0,0),(543,'2013-02-06 07:51:56',0,0,0,0,0,0,0),(544,'2013-02-06 07:52:56',0,0,0,0,0,0,0),(545,'2013-02-06 07:53:56',0,0,0,0,0,0,0),(546,'2013-02-06 07:54:56',0,0,0,0,0,0,0),(547,'2013-02-06 07:55:56',0,0,0,0,0,0,0),(548,'2013-02-06 07:56:56',0,0,0,0,0,0,0),(549,'2013-02-06 07:57:56',0,0,0,0,0,0,0),(550,'2013-02-06 07:58:56',0,0,0,0,0,0,0),(551,'2013-02-06 07:59:56',0,0,0,0,0,0,0),(552,'2013-02-06 08:00:56',0,0,0,0,0,0,0),(553,'2013-02-06 08:01:56',0,0,0,0,0,0,0),(554,'2013-02-06 08:02:56',0,0,0,0,0,0,0),(555,'2013-02-06 08:03:56',0,0,0,0,0,0,0),(556,'2013-02-06 08:04:56',0,0,0,0,0,0,0),(557,'2013-02-06 08:05:56',0,0,0,0,0,0,0),(558,'2013-02-06 08:06:56',0,0,0,0,0,0,0),(559,'2013-02-06 08:07:56',0,0,0,0,0,0,0),(560,'2013-02-06 08:08:56',0,0,0,0,0,0,0),(561,'2013-02-06 08:09:56',0,0,0,0,0,0,0),(562,'2013-02-06 08:10:56',0,0,0,0,0,0,0),(563,'2013-02-06 08:11:56',0,0,0,0,0,0,0),(564,'2013-02-06 08:12:56',0,0,0,0,0,0,0),(565,'2013-02-06 08:13:56',0,0,0,0,0,0,0),(566,'2013-02-06 08:14:56',0,0,0,0,0,0,0),(567,'2013-02-06 08:15:56',0,0,0,0,0,0,0),(568,'2013-02-06 08:16:56',0,0,0,0,0,0,0),(569,'2013-02-06 08:17:56',0,0,0,0,0,0,0),(570,'2013-02-06 08:18:56',0,0,0,0,0,0,0),(571,'2013-02-06 08:19:56',0,0,0,0,0,0,0),(572,'2013-02-06 08:20:56',0,0,0,0,0,0,0),(573,'2013-02-06 08:21:56',0,0,0,0,0,0,0),(574,'2013-02-06 08:22:56',0,0,0,0,0,0,0),(575,'2013-02-06 08:23:56',0,0,0,0,0,0,0),(576,'2013-02-06 08:24:56',0,0,0,0,0,0,0),(577,'2013-02-06 08:25:56',0,0,0,0,0,0,0),(578,'2013-02-06 08:26:56',0,0,0,0,0,0,0),(579,'2013-02-06 08:27:56',0,0,0,0,0,0,0),(580,'2013-02-06 08:28:56',0,0,0,0,0,0,0),(581,'2013-02-06 08:29:56',0,0,0,0,0,0,0),(582,'2013-02-06 08:30:56',0,0,0,0,0,0,0),(583,'2013-02-06 08:31:56',0,0,0,0,0,0,0),(584,'2013-02-06 08:32:56',0,0,0,0,0,0,0),(585,'2013-02-06 08:33:56',0,0,0,0,0,0,0),(586,'2013-02-06 08:34:56',0,0,0,0,0,0,0),(587,'2013-02-06 08:35:56',0,0,0,0,0,0,0),(588,'2013-02-06 08:36:56',0,0,0,0,0,0,0),(589,'2013-02-06 08:37:56',0,0,0,0,0,0,0),(590,'2013-02-06 08:38:56',0,0,0,0,0,0,0),(591,'2013-02-06 08:39:56',0,0,0,0,0,0,0),(592,'2013-02-06 08:40:56',0,0,0,0,0,0,0),(593,'2013-02-06 08:41:56',0,0,0,0,0,0,0),(594,'2013-02-06 08:42:56',0,0,0,0,0,0,0),(595,'2013-02-06 08:43:56',0,0,0,0,0,0,0),(596,'2013-02-06 08:44:56',0,0,0,0,0,0,0),(597,'2013-02-06 08:45:56',0,0,0,0,0,0,0),(598,'2013-02-06 08:46:56',0,0,0,0,0,0,0),(599,'2013-02-06 08:47:56',0,0,0,0,0,0,0),(600,'2013-02-06 08:48:56',0,0,0,0,0,0,0),(601,'2013-02-06 08:49:56',0,0,0,0,0,0,0),(602,'2013-02-06 08:50:56',0,0,0,0,0,0,0),(603,'2013-02-06 08:51:56',0,0,0,0,0,0,0),(604,'2013-02-06 08:52:56',0,0,0,0,0,0,0),(605,'2013-02-06 08:53:56',0,0,0,0,0,0,0),(606,'2013-02-06 08:54:56',0,0,0,0,0,0,0),(607,'2013-02-06 08:55:56',0,0,0,0,0,0,0),(608,'2013-02-06 08:56:56',0,0,0,0,0,0,0),(609,'2013-02-06 08:57:56',0,0,0,0,0,0,0),(610,'2013-02-06 08:58:56',0,0,0,0,0,0,0),(611,'2013-02-06 08:59:56',0,0,0,0,0,0,0),(612,'2013-02-06 09:00:56',0,0,0,0,0,0,0),(613,'2013-02-06 09:01:56',0,0,0,0,0,0,0),(614,'2013-02-06 09:02:56',0,0,0,0,0,0,0),(615,'2013-02-06 09:03:56',0,0,0,0,0,0,0),(616,'2013-02-06 09:04:56',0,0,0,0,0,0,0),(617,'2013-02-06 09:05:56',0,0,0,0,0,0,0),(618,'2013-02-06 09:06:56',0,0,0,0,0,0,0),(619,'2013-02-06 09:07:56',0,0,0,0,0,0,0),(620,'2013-02-06 09:08:56',0,0,0,0,0,0,0),(621,'2013-02-06 09:09:56',0,0,0,0,0,0,0),(622,'2013-02-06 09:10:56',0,0,0,0,0,0,0),(623,'2013-02-06 09:11:56',0,0,0,0,0,0,0),(624,'2013-02-06 09:12:56',0,0,0,0,0,0,0),(625,'2013-02-06 09:13:56',0,0,0,0,0,0,0),(626,'2013-02-06 09:14:56',0,0,0,0,0,0,0),(627,'2013-02-06 09:15:56',0,0,0,0,0,0,0),(628,'2013-02-06 09:16:56',0,0,0,0,0,0,0),(629,'2013-02-06 09:17:56',0,0,0,0,0,0,0),(630,'2013-02-06 09:18:56',0,0,0,0,0,0,0),(631,'2013-02-06 09:19:56',0,0,0,0,0,0,0),(632,'2013-02-06 09:20:56',0,0,0,0,0,0,0),(633,'2013-02-06 09:21:56',0,0,0,0,0,0,0),(634,'2013-02-06 09:22:56',0,0,0,0,0,0,0),(635,'2013-02-06 09:23:56',0,0,0,0,0,0,0),(636,'2013-02-06 09:24:56',0,0,0,0,0,0,0),(637,'2013-02-06 09:25:56',0,0,0,0,0,0,0),(638,'2013-02-06 09:26:56',0,0,0,0,0,0,0),(639,'2013-02-06 09:27:56',0,0,0,0,0,0,0),(640,'2013-02-06 09:28:56',0,0,0,0,0,0,0),(641,'2013-02-06 09:29:56',0,0,0,0,0,0,0),(642,'2013-02-06 09:30:56',0,0,0,0,0,0,0),(643,'2013-02-06 09:31:56',0,0,0,0,0,0,0),(644,'2013-02-06 09:32:56',0,0,0,0,0,0,0),(645,'2013-02-06 09:33:56',0,0,0,0,0,0,0),(646,'2013-02-06 09:34:56',0,0,0,0,0,0,0),(647,'2013-02-06 09:35:56',0,0,0,0,0,0,0),(648,'2013-02-06 09:36:56',0,0,0,0,0,0,0),(649,'2013-02-06 09:37:56',0,0,0,0,0,0,0),(650,'2013-02-06 09:38:56',0,0,0,0,0,0,0),(651,'2013-02-06 09:39:56',0,0,0,0,0,0,0),(652,'2013-02-06 09:40:56',0,0,0,0,0,0,0),(653,'2013-02-06 09:41:56',0,0,0,0,0,0,0),(654,'2013-02-06 09:42:56',0,0,0,0,0,0,0),(655,'2013-02-06 09:43:56',0,0,0,0,0,0,0),(656,'2013-02-06 09:44:56',0,0,0,0,0,0,0),(657,'2013-02-06 09:45:56',0,0,0,0,0,0,0),(658,'2013-02-06 09:46:56',0,0,0,0,0,0,0),(659,'2013-02-06 09:47:56',0,0,0,0,0,0,0),(660,'2013-02-06 16:08:14',1,0,0,0,0,0,0),(661,'2013-02-06 16:09:14',1,0,0,0,0,0,0),(662,'2013-02-06 16:10:14',1,0,0,0,0,0,0),(663,'2013-02-06 16:11:14',1,0,0,0,0,0,0),(664,'2013-02-06 16:12:14',1,0,0,0,0,0,0),(665,'2013-02-07 00:08:23',0,0,0,0,0,0,0),(666,'2013-02-07 00:12:49',0,0,0,0,0,0,0),(667,'2013-02-07 00:15:34',1,0,0,0,0,0,0),(668,'2013-02-07 00:16:34',1,0,0,0,0,0,0),(669,'2013-02-07 00:17:34',1,0,0,0,0,0,0),(670,'2013-02-07 00:28:43',1,0,0,0,0,0,0),(671,'2013-02-07 00:30:22',1,0,0,0,0,0,0),(672,'2013-02-07 00:31:22',1,0,0,0,0,0,0),(673,'2013-02-07 00:36:20',1,0,0,0,0,0,0),(674,'2013-02-07 00:37:20',1,0,0,0,0,0,0),(675,'2013-02-07 00:38:20',1,0,0,0,0,0,0),(676,'2013-02-07 00:39:20',1,0,0,0,0,0,0),(677,'2013-02-07 00:40:20',1,0,0,0,0,0,0),(678,'2013-02-07 00:41:20',1,0,0,0,0,0,0),(679,'2013-02-07 00:42:20',1,0,0,0,0,0,0),(680,'2013-02-07 00:43:20',1,0,0,0,0,0,0),(681,'2013-02-07 00:44:20',1,0,0,0,0,0,0),(682,'2013-02-07 00:45:20',1,0,0,0,0,0,0),(683,'2013-02-07 00:46:20',1,0,0,0,0,0,0),(684,'2013-02-07 00:47:30',1,0,0,0,0,0,0),(685,'2013-02-07 00:48:20',1,0,0,0,0,0,0),(686,'2013-02-07 00:49:20',1,0,0,0,0,0,0),(687,'2013-02-08 00:05:45',1,0,0,0,0,0,0),(688,'2013-02-08 00:06:45',1,0,0,0,0,0,0),(689,'2013-02-08 00:13:42',1,0,0,0,0,0,0),(690,'2013-02-08 00:14:42',1,0,0,0,0,0,0),(691,'2013-02-08 00:15:42',1,0,0,0,0,0,0),(692,'2013-02-08 00:18:59',1,0,0,0,0,0,0),(693,'2013-02-08 00:31:18',1,0,0,0,0,0,0),(694,'2013-02-08 00:57:14',1,0,0,0,0,0,0),(695,'2013-02-08 01:02:31',1,0,0,0,0,0,0),(696,'2013-02-08 01:03:31',1,0,0,0,0,0,0),(697,'2013-02-08 01:04:31',1,0,0,0,0,0,0),(698,'2013-02-08 01:05:31',0,0,0,0,0,0,0),(699,'2013-02-08 10:29:32',1,0,0,0,0,0,0),(700,'2013-02-08 10:30:32',1,0,0,0,0,0,0),(701,'2013-02-08 10:31:32',1,0,0,0,0,0,0),(702,'2013-02-08 10:32:32',1,0,0,0,0,0,0),(703,'2013-02-08 10:36:29',1,0,0,0,0,0,0),(704,'2013-02-08 10:37:29',1,0,0,0,0,0,0),(705,'2013-02-08 10:38:29',1,0,0,0,0,0,0),(706,'2013-02-08 10:39:29',1,0,0,0,0,0,0),(707,'2013-02-08 10:40:29',1,0,0,0,0,0,0),(708,'2013-02-08 10:41:29',1,0,0,0,0,0,0),(709,'2013-02-08 10:42:29',1,0,0,0,0,0,0),(710,'2013-02-08 10:43:29',1,0,0,0,0,0,0),(711,'2013-02-08 10:44:29',1,0,0,0,0,0,0),(712,'2013-02-09 11:34:31',0,0,0,0,0,0,0),(713,'2013-02-09 11:35:31',0,0,0,0,0,0,0),(714,'2013-02-09 11:36:31',0,0,0,0,0,0,0),(715,'2013-02-09 11:37:31',0,0,0,0,0,0,0),(716,'2013-02-09 11:38:31',0,0,0,0,0,0,0),(717,'2013-02-09 11:39:31',0,0,0,0,0,0,0),(718,'2013-02-09 11:40:31',0,0,0,0,0,0,0),(719,'2013-02-09 11:41:31',0,0,0,0,0,0,0),(720,'2013-02-09 11:42:31',0,0,0,0,0,0,0),(721,'2013-02-09 11:43:31',0,0,0,0,0,0,0),(722,'2013-02-09 11:44:31',0,0,0,0,0,0,0),(723,'2013-02-09 11:45:31',0,0,0,0,0,0,0),(724,'2013-02-09 11:46:31',0,0,0,0,0,0,0),(725,'2013-02-09 11:47:31',0,0,0,0,0,0,0),(726,'2013-02-09 11:48:31',0,0,0,0,0,0,0),(727,'2013-02-09 11:49:31',0,0,0,0,0,0,0),(728,'2013-02-09 11:50:31',0,0,0,0,0,0,0),(729,'2013-02-09 11:51:31',0,0,0,0,0,0,0),(730,'2013-02-09 11:52:31',0,0,0,0,0,0,0),(731,'2013-02-09 11:53:31',0,0,0,0,0,0,0),(732,'2013-02-09 11:54:31',0,0,0,0,0,0,0),(733,'2013-02-09 11:55:31',0,0,0,0,0,0,0),(734,'2013-02-09 11:56:31',0,0,0,0,0,0,0),(735,'2013-02-09 11:57:31',0,0,0,0,0,0,0),(736,'2013-02-09 11:58:31',0,0,0,0,0,0,0),(737,'2013-02-09 11:59:31',0,0,0,0,0,0,0),(738,'2013-02-09 12:00:31',0,0,0,0,0,0,0),(739,'2013-02-09 12:01:31',0,0,0,0,0,0,0),(740,'2013-02-09 12:02:31',0,0,0,0,0,0,0),(741,'2013-02-09 12:03:31',0,0,0,0,0,0,0),(742,'2013-02-09 12:04:31',0,0,0,0,0,0,0),(743,'2013-02-09 12:05:31',0,0,0,0,0,0,0),(744,'2013-02-09 12:06:31',0,0,0,0,0,0,0),(745,'2013-02-09 12:07:31',0,0,0,0,0,0,0),(746,'2013-02-09 12:08:31',0,0,0,0,0,0,0),(747,'2013-02-09 12:09:31',0,0,0,0,0,0,0),(748,'2013-02-09 12:10:31',0,0,0,0,0,0,0),(749,'2013-02-09 12:11:31',0,0,0,0,0,0,0),(750,'2013-02-09 12:12:31',0,0,0,0,0,0,0),(751,'2013-02-09 12:13:31',0,0,0,0,0,0,0),(752,'2013-02-09 12:14:31',0,0,0,0,0,0,0),(753,'2013-02-09 12:15:31',0,0,0,0,0,0,0),(754,'2013-02-09 12:16:31',0,0,0,0,0,0,0),(755,'2013-02-09 12:17:31',0,0,0,0,0,0,0),(756,'2013-02-09 12:18:31',0,0,0,0,0,0,0),(757,'2013-02-09 12:19:31',0,0,0,0,0,0,0),(758,'2013-02-09 12:20:31',0,0,0,0,0,0,0),(759,'2013-02-09 12:21:31',0,0,0,0,0,0,0),(760,'2013-02-09 12:22:31',0,0,0,0,0,0,0),(761,'2013-02-09 12:23:31',0,0,0,0,0,0,0),(762,'2013-02-09 12:24:31',0,0,0,0,0,0,0),(763,'2013-02-09 12:25:31',0,0,0,0,0,0,0),(764,'2013-02-09 12:26:31',0,0,0,0,0,0,0),(765,'2013-02-09 12:27:31',0,0,0,0,0,0,0),(766,'2013-02-09 12:28:31',0,0,0,0,0,0,0),(767,'2013-02-09 12:29:31',0,0,0,0,0,0,0),(768,'2013-02-09 12:30:31',0,0,0,0,0,0,0),(769,'2013-02-09 12:31:31',0,0,0,0,0,0,0),(770,'2013-02-09 12:32:31',0,0,0,0,0,0,0),(771,'2013-02-09 12:33:31',0,0,0,0,0,0,0),(772,'2013-02-09 12:34:31',0,0,0,0,0,0,0),(773,'2013-02-09 12:35:31',0,0,0,0,0,0,0),(774,'2013-02-09 12:36:31',0,0,0,0,0,0,0),(775,'2013-02-09 12:37:31',0,0,0,0,0,0,0),(776,'2013-02-09 12:38:31',0,0,0,0,0,0,0),(777,'2013-02-09 12:39:31',0,0,0,0,0,0,0),(778,'2013-02-09 12:40:31',0,0,0,0,0,0,0),(779,'2013-02-09 12:41:31',0,0,0,0,0,0,0),(780,'2013-02-09 12:42:31',0,0,0,0,0,0,0),(781,'2013-02-09 12:43:31',0,0,0,0,0,0,0),(782,'2013-02-09 12:44:31',0,0,0,0,0,0,0),(783,'2013-02-09 12:45:31',0,0,0,0,0,0,0),(784,'2013-02-09 12:46:31',0,0,0,0,0,0,0),(785,'2013-02-09 12:47:31',0,0,0,0,0,0,0),(786,'2013-02-09 12:48:31',0,0,0,0,0,0,0),(787,'2013-02-09 12:49:31',0,0,0,0,0,0,0),(788,'2013-02-09 12:50:31',0,0,0,0,0,0,0),(789,'2013-02-09 12:51:31',0,0,0,0,0,0,0),(790,'2013-02-09 12:52:31',0,0,0,0,0,0,0),(791,'2013-02-09 12:53:31',0,0,0,0,0,0,0),(792,'2013-02-09 12:54:31',0,0,0,0,0,0,0),(793,'2013-02-09 12:55:31',0,0,0,0,0,0,0),(794,'2013-02-09 12:56:31',0,0,0,0,0,0,0),(795,'2013-02-09 12:57:31',0,0,0,0,0,0,0),(796,'2013-02-09 12:58:31',0,0,0,0,0,0,0),(797,'2013-02-09 12:59:31',0,0,0,0,0,0,0),(798,'2013-02-09 13:00:31',0,0,0,0,0,0,0),(799,'2013-02-09 13:01:31',0,0,0,0,0,0,0),(800,'2013-02-09 13:02:31',0,0,0,0,0,0,0),(801,'2013-02-09 13:03:31',0,0,0,0,0,0,0),(802,'2013-02-09 13:04:31',0,0,0,0,0,0,0),(803,'2013-02-09 13:05:31',0,0,0,0,0,0,0),(804,'2013-02-09 13:06:31',0,0,0,0,0,0,0),(805,'2013-02-09 13:07:31',0,0,0,0,0,0,0),(806,'2013-02-09 13:08:31',0,0,0,0,0,0,0),(807,'2013-02-09 13:09:31',0,0,0,0,0,0,0),(808,'2013-02-09 13:10:31',0,0,0,0,0,0,0),(809,'2013-02-09 13:11:31',0,0,0,0,0,0,0),(810,'2013-02-09 13:12:31',0,0,0,0,0,0,0),(811,'2013-02-09 13:13:31',0,0,0,0,0,0,0),(812,'2013-02-09 13:14:31',0,0,0,0,0,0,0),(813,'2013-02-09 13:15:31',0,0,0,0,0,0,0),(814,'2013-02-09 13:16:31',0,0,0,0,0,0,0),(815,'2013-02-09 13:17:31',0,0,0,0,0,0,0),(816,'2013-02-09 13:18:31',0,0,0,0,0,0,0),(817,'2013-02-09 13:19:31',0,0,0,0,0,0,0),(818,'2013-02-09 13:20:31',0,0,0,0,0,0,0),(819,'2013-02-09 13:21:31',0,0,0,0,0,0,0),(820,'2013-02-09 13:22:31',0,0,0,0,0,0,0),(821,'2013-02-09 13:23:31',0,0,0,0,0,0,0),(822,'2013-02-09 13:24:31',0,0,0,0,0,0,0),(823,'2013-02-09 13:25:31',0,0,0,0,0,0,0),(824,'2013-02-09 13:26:31',0,0,0,0,0,0,0),(825,'2013-02-09 13:27:31',0,0,0,0,0,0,0),(826,'2013-02-09 13:28:31',0,0,0,0,0,0,0),(827,'2013-02-09 13:29:31',0,0,0,0,0,0,0),(828,'2013-02-09 13:30:31',0,0,0,0,0,0,0),(829,'2013-02-09 13:31:31',0,0,0,0,0,0,0),(830,'2013-02-09 13:32:31',0,0,0,0,0,0,0),(831,'2013-02-09 13:33:31',0,0,0,0,0,0,0),(832,'2013-02-09 13:34:31',0,0,0,0,0,0,0),(833,'2013-02-09 13:35:31',0,0,0,0,0,0,0),(834,'2013-02-09 13:36:31',0,0,0,0,0,0,0),(835,'2013-02-09 13:37:31',0,0,0,0,0,0,0),(836,'2013-02-09 13:38:31',0,0,0,0,0,0,0),(837,'2013-02-09 13:39:31',0,0,0,0,0,0,0),(838,'2013-02-09 13:40:31',0,0,0,0,0,0,0),(839,'2013-02-09 13:41:31',0,0,0,0,0,0,0),(840,'2013-02-09 13:42:31',0,0,0,0,0,0,0),(841,'2013-02-09 13:43:31',0,0,0,0,0,0,0),(842,'2013-02-09 13:44:31',0,0,0,0,0,0,0),(843,'2013-02-09 13:45:31',0,0,0,0,0,0,0),(844,'2013-02-09 13:46:31',0,0,0,0,0,0,0),(845,'2013-02-09 13:47:31',0,0,0,0,0,0,0),(846,'2013-02-09 13:48:31',0,0,0,0,0,0,0),(847,'2013-02-09 13:49:31',0,0,0,0,0,0,0),(848,'2013-02-09 13:50:31',0,0,0,0,0,0,0),(849,'2013-02-09 13:51:31',0,0,0,0,0,0,0),(850,'2013-02-09 13:52:31',0,0,0,0,0,0,0),(851,'2013-02-09 13:53:31',0,0,0,0,0,0,0),(852,'2013-02-09 13:54:31',0,0,0,0,0,0,0),(853,'2013-02-09 13:55:31',0,0,0,0,0,0,0),(854,'2013-02-09 13:56:31',0,0,0,0,0,0,0),(855,'2013-02-09 13:57:31',0,0,0,0,0,0,0),(856,'2013-02-09 13:58:31',0,0,0,0,0,0,0),(857,'2013-02-09 13:59:31',0,0,0,0,0,0,0),(858,'2013-02-09 14:00:31',0,0,0,0,0,0,0),(859,'2013-02-09 14:01:31',0,0,0,0,0,0,0),(860,'2013-02-09 14:02:31',0,0,0,0,0,0,0),(861,'2013-02-09 14:03:31',0,0,0,0,0,0,0),(862,'2013-02-09 14:05:18',0,0,0,0,0,0,0),(863,'2013-02-09 14:21:28',0,0,0,0,0,0,0),(864,'2013-02-09 14:22:28',0,0,0,0,0,0,0),(865,'2013-02-09 14:23:28',0,0,0,0,0,0,0),(866,'2013-02-09 14:24:28',0,0,0,0,0,0,0),(867,'2013-02-09 14:25:28',0,0,0,0,0,0,0),(868,'2013-02-09 14:27:33',0,0,0,0,0,0,0),(869,'2013-02-09 14:28:33',0,0,0,0,0,0,0),(870,'2013-02-09 14:29:33',0,0,0,0,0,0,0),(871,'2013-02-09 14:30:33',0,0,0,0,0,0,0),(872,'2013-02-09 14:31:33',0,0,0,0,0,0,0),(873,'2013-02-09 14:32:33',0,0,0,0,0,0,0),(874,'2013-02-09 14:33:33',0,0,0,0,0,0,0),(875,'2013-02-09 15:01:14',1,0,0,0,0,0,0),(876,'2013-02-09 15:02:14',1,0,0,0,0,0,0),(877,'2013-02-09 15:05:13',1,0,0,0,0,0,0),(878,'2013-02-09 15:05:13',1,0,0,0,0,0,0),(879,'2013-02-09 15:06:56',1,0,0,0,0,0,0),(880,'2013-02-09 15:07:56',1,0,0,0,0,0,0),(881,'2013-02-09 15:10:18',1,0,0,0,0,0,0),(882,'2013-02-09 15:11:18',1,0,0,0,0,0,0),(883,'2013-02-09 15:12:18',1,0,0,0,0,0,0),(884,'2013-02-09 15:13:18',1,0,0,0,0,0,0),(885,'2013-02-09 15:14:18',1,0,0,0,0,0,0),(886,'2013-02-09 15:15:18',1,0,0,0,0,0,0),(887,'2013-02-09 15:18:39',1,0,0,0,0,0,0),(888,'2013-02-09 15:19:42',0,0,0,0,0,0,0),(889,'2013-02-09 15:21:30',1,0,0,0,0,0,0),(890,'2013-02-09 15:26:03',0,0,0,0,0,0,0),(891,'2013-02-09 15:27:03',0,0,0,0,0,0,0),(892,'2013-02-09 15:28:03',0,0,0,0,0,0,0),(893,'2013-02-09 15:29:03',1,0,0,0,0,0,0),(894,'2013-02-09 15:30:03',1,0,0,0,0,0,0),(895,'2013-02-09 15:31:03',1,0,0,0,0,0,0),(896,'2013-02-09 15:32:03',1,0,0,0,0,0,0),(897,'2013-02-09 15:33:03',1,0,0,0,0,0,0),(898,'2013-02-09 15:37:00',1,0,0,0,0,0,0),(899,'2013-02-09 16:06:51',1,0,0,0,0,0,0),(900,'2013-02-09 16:08:22',0,0,0,0,0,0,0),(901,'2013-02-09 16:08:51',0,0,0,0,0,0,0),(902,'2013-02-09 16:11:24',1,0,0,0,0,0,0),(903,'2013-02-09 16:28:05',0,0,0,0,0,0,0),(904,'2013-02-09 16:29:05',0,0,0,0,0,0,0),(905,'2013-02-09 16:30:05',0,0,0,0,0,0,0),(906,'2013-02-09 16:31:05',0,0,0,0,0,0,0),(907,'2013-02-09 16:32:05',0,0,0,0,0,0,0),(908,'2013-02-09 16:33:05',0,0,0,0,0,0,0),(909,'2013-02-09 16:34:05',0,0,0,0,0,0,0),(910,'2013-02-09 16:35:05',0,0,0,0,0,0,0),(911,'2013-02-09 16:36:05',0,0,0,0,0,0,0),(912,'2013-02-09 16:37:54',0,0,0,0,0,0,0),(913,'2013-02-09 16:38:54',0,0,0,0,0,0,0),(914,'2013-02-09 19:20:33',1,0,0,0,0,0,0),(915,'2013-02-09 19:21:33',1,0,0,0,0,0,0),(916,'2013-02-09 19:23:14',1,0,0,0,0,0,0),(917,'2013-02-09 19:25:45',0,0,0,0,0,0,0),(918,'2013-02-09 19:28:39',1,0,0,0,0,0,0),(919,'2013-02-09 19:29:39',1,0,0,0,0,0,0),(920,'2013-02-09 19:31:06',1,0,0,0,0,0,0),(921,'2013-02-09 19:32:18',0,0,0,0,0,0,0),(922,'2013-02-09 19:33:18',0,0,0,0,0,0,0),(923,'2013-02-09 19:34:18',0,0,0,0,0,0,0),(924,'2013-02-09 19:35:18',0,0,0,0,0,0,0),(925,'2013-02-09 19:36:18',0,0,0,0,0,0,0),(926,'2013-02-09 19:37:18',0,0,0,0,0,0,0),(927,'2013-02-09 19:38:18',0,0,0,0,0,0,0),(928,'2013-02-09 19:39:18',0,0,0,0,0,0,0),(929,'2013-02-09 19:40:18',0,0,0,0,0,0,0),(930,'2013-02-09 19:54:44',0,0,0,0,0,0,0),(931,'2013-02-09 20:06:12',0,0,0,0,0,0,0),(932,'2013-02-09 20:07:12',0,0,0,0,0,0,0),(933,'2013-02-09 20:08:12',0,0,0,0,0,0,0),(934,'2013-02-09 20:09:12',0,0,0,0,0,0,0),(935,'2013-02-09 20:10:12',0,0,0,0,0,0,0),(936,'2013-02-09 20:11:12',0,0,0,0,0,0,0),(937,'2013-02-09 20:12:12',0,0,0,0,0,0,0),(938,'2013-02-09 20:13:12',0,0,0,0,0,0,0),(939,'2013-02-09 20:14:12',0,0,0,0,0,0,0),(940,'2013-02-09 20:15:12',0,0,0,0,0,0,0),(941,'2013-02-09 20:16:12',0,0,0,0,0,0,0),(942,'2013-02-09 20:17:12',0,0,0,0,0,0,0),(943,'2013-02-09 20:18:12',0,0,0,0,0,0,0),(944,'2013-02-09 20:19:12',0,0,0,0,0,0,0),(945,'2013-02-09 20:20:42',0,0,0,0,0,0,0),(946,'2013-02-09 20:22:05',0,0,0,0,0,0,0),(947,'2013-02-09 20:32:51',1,0,0,0,0,0,0),(948,'2013-02-09 20:41:22',0,0,0,0,0,0,0),(949,'2013-02-09 20:42:22',0,0,0,0,0,0,0),(950,'2013-02-09 20:43:22',0,0,0,0,0,0,0),(951,'2013-02-09 20:44:22',0,0,0,0,0,0,0),(952,'2013-02-09 20:51:49',1,0,0,0,0,0,0),(953,'2013-02-09 20:52:49',1,0,0,0,0,0,0),(954,'2013-02-09 20:53:49',1,0,0,0,0,0,0),(955,'2013-02-09 20:55:05',1,0,0,0,0,0,0),(956,'2013-02-09 20:59:03',1,0,0,0,0,0,0),(957,'2013-02-09 21:00:03',1,0,0,0,0,0,0),(958,'2013-02-09 21:16:54',1,0,0,0,0,0,0),(959,'2013-02-09 21:18:20',1,0,0,0,0,0,0),(960,'2013-02-09 21:21:59',1,0,0,0,0,0,0),(961,'2013-02-09 21:25:39',1,0,0,0,0,0,0),(962,'2013-02-09 21:52:24',1,0,0,0,0,0,0),(963,'2013-02-09 21:54:39',1,0,0,0,0,0,0),(964,'2013-02-09 21:55:39',1,0,0,0,0,0,0),(965,'2013-02-09 21:59:20',1,0,0,0,0,0,0),(966,'2013-02-09 22:00:20',1,0,0,0,0,0,0),(967,'2013-02-09 22:01:20',1,0,0,0,0,0,0),(968,'2013-02-09 22:04:34',1,0,0,0,0,0,0),(969,'2013-02-09 22:14:01',1,0,0,0,0,0,0),(970,'2013-02-09 22:16:34',1,0,0,0,0,0,0),(971,'2013-02-09 22:18:24',1,0,0,0,0,0,0),(972,'2013-02-09 22:19:24',1,0,0,0,0,0,0),(973,'2013-02-09 22:20:24',1,0,0,0,0,0,0),(974,'2013-02-09 22:21:24',1,0,0,0,0,0,0),(975,'2013-02-09 22:22:51',1,0,0,0,0,0,0),(976,'2013-02-09 22:25:42',1,0,0,0,0,0,0),(977,'2013-02-09 22:28:48',1,0,0,0,0,0,0),(978,'2013-02-09 22:29:48',1,0,0,0,0,0,0),(979,'2013-02-09 22:30:48',1,0,0,0,0,0,0),(980,'2013-02-09 22:33:28',1,0,0,0,0,0,0),(981,'2013-02-09 22:45:34',1,0,0,0,0,0,0),(982,'2013-02-09 22:47:35',1,0,0,0,0,0,0),(983,'2013-02-09 22:59:29',1,0,0,0,0,0,0),(984,'2013-02-09 23:07:34',1,0,0,0,0,0,0),(985,'2013-02-09 23:08:34',1,0,0,0,0,0,0),(986,'2013-02-09 23:09:34',1,0,0,0,0,0,0),(987,'2013-02-09 23:12:50',1,0,0,0,0,0,0),(988,'2013-02-09 23:15:15',1,0,0,0,0,0,0),(989,'2013-02-09 23:16:15',1,0,0,0,0,0,0),(990,'2013-02-09 23:17:15',1,0,0,0,0,0,0),(991,'2013-02-09 23:27:20',1,0,0,0,0,0,0),(992,'2013-02-09 23:30:23',1,0,0,0,0,0,0),(993,'2013-02-09 23:31:23',1,0,0,0,0,0,0),(994,'2013-02-09 23:32:23',1,0,0,0,0,0,0),(995,'2013-02-09 23:33:23',1,0,0,0,0,0,0),(996,'2013-02-09 23:34:23',1,0,0,0,0,0,0),(997,'2013-02-09 23:35:23',1,0,0,0,0,0,0),(998,'2013-02-09 23:36:23',1,0,0,0,0,0,0),(999,'2013-02-09 23:37:23',1,0,0,0,0,0,0),(1000,'2013-02-09 23:38:23',1,0,0,0,0,0,0),(1001,'2013-02-09 23:39:23',1,0,0,0,0,0,0),(1002,'2013-02-09 23:40:23',1,0,0,0,0,0,0),(1003,'2013-02-09 23:41:23',1,0,0,0,0,0,0),(1004,'2013-02-09 23:42:23',1,0,0,0,0,0,0),(1005,'2013-02-09 23:43:23',1,0,0,0,0,0,0),(1006,'2013-02-09 23:44:23',1,0,0,0,0,0,0),(1007,'2013-02-09 23:45:23',1,0,0,0,0,0,0),(1008,'2013-02-09 23:46:23',1,0,0,0,0,0,0),(1009,'2013-02-09 23:47:23',1,0,0,0,0,0,0),(1010,'2013-02-09 23:48:23',1,0,0,0,0,0,0),(1011,'2013-02-09 23:49:23',1,0,0,0,0,0,0),(1012,'2013-02-09 23:50:23',1,0,0,0,0,0,0),(1013,'2013-02-09 23:51:23',1,0,0,0,0,0,0),(1014,'2013-02-09 23:52:23',1,0,0,0,0,0,0),(1015,'2013-02-09 23:53:23',1,0,0,0,0,0,0),(1016,'2013-02-09 23:54:23',1,0,0,0,0,0,0),(1017,'2013-02-09 23:55:23',1,0,0,0,0,0,0),(1018,'2013-02-09 23:56:23',1,0,0,0,0,0,0),(1019,'2013-02-09 23:57:23',1,0,0,0,0,0,0),(1020,'2013-02-09 23:58:23',1,0,0,0,0,0,0),(1021,'2013-02-09 23:59:23',1,0,0,0,0,0,0),(1022,'2013-02-10 00:00:23',1,0,0,0,0,0,0),(1023,'2013-02-10 00:01:23',1,0,0,0,0,0,0),(1024,'2013-02-10 00:02:23',1,0,0,0,0,0,0),(1025,'2013-02-10 00:04:09',1,0,0,0,0,0,0),(1026,'2013-02-10 00:05:09',1,0,0,0,0,0,0),(1027,'2013-02-10 00:06:09',1,0,0,0,0,0,0),(1028,'2013-02-10 00:07:24',1,0,0,0,0,0,0),(1029,'2013-02-10 00:09:29',1,0,0,0,0,0,0),(1030,'2013-02-10 00:12:08',1,0,0,0,0,0,0),(1031,'2013-02-10 00:13:08',1,0,0,0,0,0,0),(1032,'2013-02-10 00:14:08',1,0,0,0,0,0,0),(1033,'2013-02-10 00:15:08',1,0,0,0,0,0,0),(1034,'2013-02-10 00:16:08',1,0,0,0,0,0,0),(1035,'2013-02-10 00:17:08',1,0,0,0,0,0,0),(1036,'2013-02-10 00:18:08',1,0,0,0,0,0,0),(1037,'2013-02-10 00:19:08',1,0,0,0,0,0,0),(1038,'2013-02-10 00:20:08',1,0,0,0,0,0,0),(1039,'2013-02-10 00:21:08',1,0,0,0,0,0,0),(1040,'2013-02-10 00:22:08',1,0,0,0,0,0,0),(1041,'2013-02-10 00:23:08',1,0,0,0,0,0,0),(1042,'2013-02-10 00:24:08',1,0,0,0,0,0,0),(1043,'2013-02-10 00:25:08',1,0,0,0,0,0,0),(1044,'2013-02-10 00:26:08',1,0,0,0,0,0,0),(1045,'2013-02-10 00:27:08',1,0,0,0,0,0,0),(1046,'2013-02-10 00:28:08',1,0,0,0,0,0,0),(1047,'2013-02-10 00:29:08',1,0,0,0,0,0,0),(1048,'2013-02-10 00:30:08',1,0,0,0,0,0,0),(1049,'2013-02-10 00:31:08',1,0,0,0,0,0,0),(1050,'2013-02-10 00:32:52',0,0,0,0,0,0,0),(1051,'2013-02-10 00:34:35',1,0,0,0,0,0,0),(1052,'2013-02-10 00:35:35',1,0,0,0,0,0,0),(1053,'2013-02-10 00:36:35',1,0,0,0,0,0,0),(1054,'2013-02-10 00:37:35',1,0,0,0,0,0,0),(1055,'2013-02-10 00:38:35',1,0,0,0,0,0,0),(1056,'2013-02-10 00:39:35',1,0,0,0,0,0,0),(1057,'2013-02-10 00:42:08',1,0,0,0,0,0,0),(1058,'2013-02-10 00:43:08',1,0,0,0,0,0,0),(1059,'2013-02-10 00:44:08',1,0,0,0,0,0,0),(1060,'2013-02-10 00:45:08',1,0,0,0,0,0,0),(1061,'2013-02-10 00:46:08',1,0,0,0,0,0,0),(1062,'2013-02-10 00:47:08',1,0,0,0,0,0,0),(1063,'2013-02-10 00:48:08',1,0,0,0,0,0,0),(1064,'2013-02-10 00:49:08',1,0,0,0,0,0,0),(1065,'2013-02-10 00:50:08',1,0,0,0,0,0,0),(1066,'2013-02-10 00:51:08',1,0,0,0,0,0,0),(1067,'2013-02-10 00:52:08',1,0,0,0,0,0,0),(1068,'2013-02-10 00:53:08',1,0,0,0,0,0,0),(1069,'2013-02-10 00:54:08',1,0,0,0,0,0,0),(1070,'2013-02-10 00:55:08',1,0,0,0,0,0,0),(1071,'2013-02-10 00:56:08',1,0,0,0,0,0,0),(1072,'2013-02-10 01:07:25',1,0,0,0,0,0,0),(1073,'2013-02-10 01:08:25',1,0,0,0,0,0,0),(1074,'2013-02-10 01:09:25',1,0,0,0,0,0,0),(1075,'2013-02-10 01:13:36',1,0,0,0,0,0,0),(1076,'2013-02-10 01:14:36',1,0,0,0,0,0,0),(1077,'2013-02-10 01:15:36',1,0,0,0,0,0,0),(1078,'2013-02-10 01:16:36',1,0,0,0,0,0,0),(1079,'2013-02-10 01:17:36',1,0,0,0,0,0,0),(1080,'2013-02-10 01:20:33',1,0,0,0,0,0,0),(1081,'2013-02-10 01:21:33',1,0,0,0,0,0,0),(1082,'2013-02-10 01:27:38',1,0,0,0,0,0,0),(1083,'2013-02-10 01:28:38',1,0,0,0,0,0,0),(1084,'2013-02-10 01:29:38',1,0,0,0,0,0,0),(1085,'2013-02-10 01:31:50',1,0,0,0,0,0,0),(1086,'2013-02-10 01:32:50',1,0,0,0,0,0,0),(1087,'2013-02-10 01:33:50',1,0,0,0,0,0,0),(1088,'2013-02-10 01:34:50',1,0,0,0,0,0,0),(1089,'2013-02-10 01:35:50',1,0,0,0,0,0,0),(1090,'2013-02-10 01:36:50',1,0,0,0,0,0,0),(1091,'2013-02-10 01:37:50',1,0,0,0,0,0,0),(1092,'2013-02-10 01:38:50',1,0,0,0,0,0,0),(1093,'2013-02-10 01:39:50',1,0,0,0,0,0,0),(1094,'2013-02-10 01:40:50',1,0,0,0,0,0,0),(1095,'2013-02-10 01:41:50',1,0,0,0,0,0,0),(1096,'2013-02-10 01:42:50',1,0,0,0,0,0,0),(1097,'2013-02-10 01:43:50',1,0,0,0,0,0,0),(1098,'2013-02-10 01:45:48',0,0,0,0,0,0,0),(1099,'2013-02-10 01:46:48',0,0,0,0,0,0,0),(1100,'2013-02-10 01:47:48',0,0,0,0,0,0,0),(1101,'2013-02-10 01:48:48',0,0,0,0,0,0,0),(1102,'2013-02-10 01:54:47',0,0,0,0,0,0,0),(1103,'2013-02-10 01:59:35',0,0,0,0,0,0,0),(1104,'2013-02-10 02:00:35',0,0,0,0,0,0,0),(1105,'2013-02-10 02:01:35',0,0,0,0,0,0,0),(1106,'2013-02-10 02:02:35',0,0,0,0,0,0,0),(1107,'2013-02-10 02:03:35',0,0,0,0,0,0,0),(1108,'2013-02-10 02:04:35',0,0,0,0,0,0,0),(1109,'2013-02-10 02:05:35',0,0,0,0,0,0,0),(1110,'2013-02-10 02:06:35',1,0,0,0,0,0,0),(1111,'2013-02-10 02:07:35',1,0,0,0,0,0,0),(1112,'2013-02-10 02:08:35',1,0,0,0,0,0,0),(1113,'2013-02-10 02:09:35',1,0,0,0,0,0,0),(1114,'2013-02-10 02:10:35',1,0,0,0,0,0,0),(1115,'2013-02-10 02:11:35',1,0,0,0,0,0,0),(1116,'2013-02-10 02:12:35',1,0,0,0,0,0,0),(1117,'2013-02-10 02:13:35',1,0,0,0,0,0,0),(1118,'2013-02-10 02:14:35',1,0,0,0,0,0,0),(1119,'2013-02-10 02:15:35',1,0,0,0,0,0,0),(1120,'2013-02-10 02:16:35',1,0,0,0,0,0,0),(1121,'2013-02-10 02:17:35',1,0,0,0,0,0,0),(1122,'2013-02-10 02:18:35',1,0,0,0,0,0,0),(1123,'2013-02-10 02:19:35',1,0,0,0,0,0,0),(1124,'2013-02-10 02:20:35',1,0,0,0,0,0,0),(1125,'2013-02-10 02:21:35',1,0,0,0,0,0,0),(1126,'2013-02-10 02:22:35',1,0,0,0,0,0,0),(1127,'2013-02-10 02:23:35',1,0,0,0,0,0,0),(1128,'2013-02-10 02:24:35',1,0,0,0,0,0,0),(1129,'2013-02-10 02:25:35',1,0,0,0,0,0,0),(1130,'2013-02-10 02:26:35',1,0,0,0,0,0,0),(1131,'2013-02-10 02:27:35',1,0,0,0,0,0,0),(1132,'2013-02-10 02:28:35',1,0,0,0,0,0,0),(1133,'2013-02-10 02:29:35',1,0,0,0,0,0,0),(1134,'2013-02-10 02:30:35',1,0,0,0,0,0,0),(1135,'2013-02-10 02:31:35',1,0,0,0,0,0,0),(1136,'2013-02-10 02:32:35',1,0,0,0,0,0,0),(1137,'2013-02-10 02:33:35',1,0,0,0,0,0,0),(1138,'2013-02-10 02:34:35',1,0,0,0,0,0,0),(1139,'2013-02-10 02:35:35',1,0,0,0,0,0,0),(1140,'2013-02-10 02:36:35',1,0,0,0,0,0,0),(1141,'2013-02-10 02:37:35',1,0,0,0,0,0,0),(1142,'2013-02-10 02:38:35',1,0,0,0,0,0,0),(1143,'2013-02-10 02:39:35',1,0,0,0,0,0,0),(1144,'2013-02-10 02:40:35',1,0,0,0,0,0,0),(1145,'2013-02-10 02:41:35',1,0,0,0,0,0,0),(1146,'2013-02-10 02:42:35',1,0,0,0,0,0,0),(1147,'2013-02-10 02:43:35',1,0,0,0,0,0,0),(1148,'2013-02-10 02:44:35',1,0,0,0,0,0,0),(1149,'2013-02-10 02:45:35',1,0,0,0,0,0,0),(1150,'2013-02-10 02:46:35',1,0,0,0,0,0,0),(1151,'2013-02-10 02:47:35',1,0,0,0,0,0,0),(1152,'2013-02-10 02:48:35',1,0,0,0,0,0,0),(1153,'2013-02-10 02:49:35',1,0,0,0,0,0,0),(1154,'2013-02-10 02:50:35',1,0,0,0,0,0,0),(1155,'2013-02-10 02:51:35',1,0,0,0,0,0,0),(1156,'2013-02-10 02:52:35',1,0,0,0,0,0,0),(1157,'2013-02-10 02:53:35',1,0,0,0,0,0,0),(1158,'2013-02-10 02:54:35',1,0,0,0,0,0,0),(1159,'2013-02-10 02:55:35',1,0,0,0,0,0,0),(1160,'2013-02-10 02:56:35',1,0,0,0,0,0,0),(1161,'2013-02-10 02:57:35',1,0,0,0,0,0,0),(1162,'2013-02-10 02:58:35',1,0,0,0,0,0,0),(1163,'2013-02-10 02:59:35',1,0,0,0,0,0,0),(1164,'2013-02-10 03:00:35',1,0,0,0,0,0,0),(1165,'2013-02-10 03:01:35',1,0,0,0,0,0,0),(1166,'2013-02-10 03:02:35',1,0,0,0,0,0,0),(1167,'2013-02-10 03:03:35',1,0,0,0,0,0,0),(1168,'2013-02-10 03:04:35',1,0,0,0,0,0,0),(1169,'2013-02-10 03:05:35',1,0,0,0,0,0,0),(1170,'2013-02-10 03:06:35',1,0,0,0,0,0,0),(1171,'2013-02-10 03:07:35',1,0,0,0,0,0,0),(1172,'2013-02-10 03:08:35',1,0,0,0,0,0,0),(1173,'2013-02-10 03:09:35',1,0,0,0,0,0,0),(1174,'2013-02-10 03:10:35',1,0,0,0,0,0,0),(1175,'2013-02-10 03:11:35',1,0,0,0,0,0,0),(1176,'2013-02-10 03:12:35',1,0,0,0,0,0,0),(1177,'2013-02-10 03:13:35',1,0,0,0,0,0,0),(1178,'2013-02-10 03:14:35',1,0,0,0,0,0,0),(1179,'2013-02-10 03:15:35',1,0,0,0,0,0,0),(1180,'2013-02-10 03:16:35',1,0,0,0,0,0,0),(1181,'2013-02-10 03:17:35',1,0,0,0,0,0,0),(1182,'2013-02-10 03:18:35',1,0,0,0,0,0,0),(1183,'2013-02-10 03:19:35',1,0,0,0,0,0,0),(1184,'2013-02-10 03:20:35',1,0,0,0,0,0,0),(1185,'2013-02-10 03:21:35',1,0,0,0,0,0,0),(1186,'2013-02-10 03:22:35',1,0,0,0,0,0,0),(1187,'2013-02-10 03:23:35',1,0,0,0,0,0,0),(1188,'2013-02-10 03:24:35',1,0,0,0,0,0,0),(1189,'2013-02-10 03:25:35',1,0,0,0,0,0,0),(1190,'2013-02-10 03:26:35',1,0,0,0,0,0,0),(1191,'2013-02-10 03:27:35',1,0,0,0,0,0,0),(1192,'2013-02-10 03:28:35',1,0,0,0,0,0,0),(1193,'2013-02-10 03:29:35',1,0,0,0,0,0,0),(1194,'2013-02-10 03:30:35',1,0,0,0,0,0,0),(1195,'2013-02-10 03:31:35',1,0,0,0,0,0,0),(1196,'2013-02-10 03:32:35',1,0,0,0,0,0,0),(1197,'2013-02-10 03:33:35',1,0,0,0,0,0,0),(1198,'2013-02-10 03:34:35',1,0,0,0,0,0,0),(1199,'2013-02-10 03:35:35',1,0,0,0,0,0,0),(1200,'2013-02-10 03:36:35',1,0,0,0,0,0,0),(1201,'2013-02-10 03:37:35',1,0,0,0,0,0,0),(1202,'2013-02-10 03:38:35',1,0,0,0,0,0,0),(1203,'2013-02-10 03:39:35',1,0,0,0,0,0,0),(1204,'2013-02-10 03:40:35',1,0,0,0,0,0,0),(1205,'2013-02-10 03:41:35',1,0,0,0,0,0,0),(1206,'2013-02-10 03:42:35',1,0,0,0,0,0,0),(1207,'2013-02-10 03:43:35',1,0,0,0,0,0,0),(1208,'2013-02-10 03:44:35',1,0,0,0,0,0,0),(1209,'2013-02-10 03:45:35',1,0,0,0,0,0,0),(1210,'2013-02-10 03:46:35',1,0,0,0,0,0,0),(1211,'2013-02-10 03:47:35',1,0,0,0,0,0,0),(1212,'2013-02-10 03:48:35',1,0,0,0,0,0,0),(1213,'2013-02-10 03:49:35',1,0,0,0,0,0,0),(1214,'2013-02-10 03:50:35',1,0,0,0,0,0,0),(1215,'2013-02-10 03:51:35',1,0,0,0,0,0,0),(1216,'2013-02-10 03:52:35',1,0,0,0,0,0,0),(1217,'2013-02-10 03:53:35',1,0,0,0,0,0,0),(1218,'2013-02-10 03:54:35',1,0,0,0,0,0,0),(1219,'2013-02-10 03:55:35',1,0,0,0,0,0,0),(1220,'2013-02-10 03:56:35',1,0,0,0,0,0,0),(1221,'2013-02-10 03:57:35',1,0,0,0,0,0,0),(1222,'2013-02-10 03:58:35',1,0,0,0,0,0,0),(1223,'2013-02-10 03:59:35',1,0,0,0,0,0,0),(1224,'2013-02-10 04:00:35',1,0,0,0,0,0,0),(1225,'2013-02-10 04:01:35',1,0,0,0,0,0,0),(1226,'2013-02-10 04:02:35',1,0,0,0,0,0,0),(1227,'2013-02-10 04:03:35',1,0,0,0,0,0,0),(1228,'2013-02-10 04:04:35',1,0,0,0,0,0,0),(1229,'2013-02-10 04:05:35',1,0,0,0,0,0,0),(1230,'2013-02-10 04:06:35',1,0,0,0,0,0,0),(1231,'2013-02-10 04:07:35',1,0,0,0,0,0,0),(1232,'2013-02-10 04:08:35',1,0,0,0,0,0,0),(1233,'2013-02-10 04:09:35',1,0,0,0,0,0,0),(1234,'2013-02-10 04:10:35',1,0,0,0,0,0,0),(1235,'2013-02-10 04:11:35',1,0,0,0,0,0,0),(1236,'2013-02-10 04:12:35',1,0,0,0,0,0,0),(1237,'2013-02-10 04:13:35',1,0,0,0,0,0,0),(1238,'2013-02-10 04:14:35',1,0,0,0,0,0,0),(1239,'2013-02-10 04:15:35',1,0,0,0,0,0,0),(1240,'2013-02-10 04:16:35',1,0,0,0,0,0,0),(1241,'2013-02-10 04:17:35',1,0,0,0,0,0,0),(1242,'2013-02-10 04:18:35',1,0,0,0,0,0,0),(1243,'2013-02-10 04:19:35',1,0,0,0,0,0,0),(1244,'2013-02-10 04:20:35',1,0,0,0,0,0,0),(1245,'2013-02-10 04:21:35',1,0,0,0,0,0,0),(1246,'2013-02-10 04:22:35',1,0,0,0,0,0,0),(1247,'2013-02-10 04:23:35',1,0,0,0,0,0,0),(1248,'2013-02-10 04:24:35',1,0,0,0,0,0,0),(1249,'2013-02-10 04:25:35',1,0,0,0,0,0,0),(1250,'2013-02-10 04:26:35',1,0,0,0,0,0,0),(1251,'2013-02-10 04:27:35',1,0,0,0,0,0,0),(1252,'2013-02-10 04:28:35',1,0,0,0,0,0,0),(1253,'2013-02-10 04:29:35',1,0,0,0,0,0,0),(1254,'2013-02-10 04:30:35',1,0,0,0,0,0,0),(1255,'2013-02-10 04:31:35',1,0,0,0,0,0,0),(1256,'2013-02-10 04:32:35',1,0,0,0,0,0,0),(1257,'2013-02-10 04:33:35',1,0,0,0,0,0,0),(1258,'2013-02-10 04:34:35',1,0,0,0,0,0,0),(1259,'2013-02-10 04:35:35',1,0,0,0,0,0,0),(1260,'2013-02-10 04:36:35',1,0,0,0,0,0,0),(1261,'2013-02-10 04:37:35',1,0,0,0,0,0,0),(1262,'2013-02-10 04:38:35',1,0,0,0,0,0,0),(1263,'2013-02-10 04:39:35',1,0,0,0,0,0,0),(1264,'2013-02-10 04:40:35',1,0,0,0,0,0,0),(1265,'2013-02-10 04:41:35',1,0,0,0,0,0,0),(1266,'2013-02-10 04:42:35',1,0,0,0,0,0,0),(1267,'2013-02-10 04:43:35',1,0,0,0,0,0,0),(1268,'2013-02-10 04:44:35',1,0,0,0,0,0,0),(1269,'2013-02-10 04:45:35',1,0,0,0,0,0,0),(1270,'2013-02-10 04:46:35',1,0,0,0,0,0,0),(1271,'2013-02-10 04:47:35',1,0,0,0,0,0,0),(1272,'2013-02-10 04:48:35',1,0,0,0,0,0,0),(1273,'2013-02-10 04:49:35',1,0,0,0,0,0,0),(1274,'2013-02-10 04:50:35',1,0,0,0,0,0,0),(1275,'2013-02-10 04:51:35',1,0,0,0,0,0,0),(1276,'2013-02-10 04:52:35',1,0,0,0,0,0,0),(1277,'2013-02-10 04:53:35',1,0,0,0,0,0,0),(1278,'2013-02-10 04:54:35',1,0,0,0,0,0,0),(1279,'2013-02-10 04:55:35',1,0,0,0,0,0,0),(1280,'2013-02-10 04:56:35',1,0,0,0,0,0,0),(1281,'2013-02-10 04:57:35',1,0,0,0,0,0,0),(1282,'2013-02-10 04:58:35',1,0,0,0,0,0,0),(1283,'2013-02-10 04:59:35',1,0,0,0,0,0,0),(1284,'2013-02-10 05:00:35',1,0,0,0,0,0,0),(1285,'2013-02-10 05:01:35',1,0,0,0,0,0,0),(1286,'2013-02-10 05:02:35',1,0,0,0,0,0,0),(1287,'2013-02-10 05:03:35',1,0,0,0,0,0,0),(1288,'2013-02-10 05:04:35',1,0,0,0,0,0,0),(1289,'2013-02-10 05:05:35',1,0,0,0,0,0,0),(1290,'2013-02-10 05:06:35',1,0,0,0,0,0,0),(1291,'2013-02-10 05:07:35',1,0,0,0,0,0,0),(1292,'2013-02-10 05:08:35',1,0,0,0,0,0,0),(1293,'2013-02-10 05:09:35',1,0,0,0,0,0,0),(1294,'2013-02-10 05:10:35',1,0,0,0,0,0,0),(1295,'2013-02-10 05:11:35',1,0,0,0,0,0,0),(1296,'2013-02-10 05:12:35',1,0,0,0,0,0,0),(1297,'2013-02-10 05:13:35',1,0,0,0,0,0,0),(1298,'2013-02-10 05:14:35',1,0,0,0,0,0,0),(1299,'2013-02-10 05:15:35',1,0,0,0,0,0,0),(1300,'2013-02-10 05:16:35',1,0,0,0,0,0,0),(1301,'2013-02-10 05:17:35',1,0,0,0,0,0,0),(1302,'2013-02-10 05:18:35',1,0,0,0,0,0,0),(1303,'2013-02-10 05:19:35',1,0,0,0,0,0,0),(1304,'2013-02-10 05:20:35',1,0,0,0,0,0,0),(1305,'2013-02-10 05:21:35',1,0,0,0,0,0,0),(1306,'2013-02-10 05:22:35',1,0,0,0,0,0,0),(1307,'2013-02-10 05:23:35',1,0,0,0,0,0,0),(1308,'2013-02-10 05:24:35',1,0,0,0,0,0,0),(1309,'2013-02-10 05:25:35',1,0,0,0,0,0,0),(1310,'2013-02-10 05:26:35',1,0,0,0,0,0,0),(1311,'2013-02-10 05:27:35',1,0,0,0,0,0,0),(1312,'2013-02-10 05:28:35',1,0,0,0,0,0,0),(1313,'2013-02-10 05:29:35',1,0,0,0,0,0,0),(1314,'2013-02-10 05:30:35',1,0,0,0,0,0,0),(1315,'2013-02-10 05:31:35',1,0,0,0,0,0,0),(1316,'2013-02-10 05:32:35',1,0,0,0,0,0,0),(1317,'2013-02-10 05:33:35',1,0,0,0,0,0,0),(1318,'2013-02-10 05:34:35',1,0,0,0,0,0,0),(1319,'2013-02-10 05:35:35',1,0,0,0,0,0,0),(1320,'2013-02-10 05:36:35',1,0,0,0,0,0,0),(1321,'2013-02-10 05:37:35',1,0,0,0,0,0,0),(1322,'2013-02-10 05:38:35',1,0,0,0,0,0,0),(1323,'2013-02-10 05:39:35',1,0,0,0,0,0,0),(1324,'2013-02-10 05:40:35',1,0,0,0,0,0,0),(1325,'2013-02-10 05:41:35',1,0,0,0,0,0,0),(1326,'2013-02-10 05:42:35',1,0,0,0,0,0,0),(1327,'2013-02-10 05:43:35',1,0,0,0,0,0,0),(1328,'2013-02-10 05:44:35',1,0,0,0,0,0,0),(1329,'2013-02-10 05:45:35',1,0,0,0,0,0,0),(1330,'2013-02-10 05:46:35',1,0,0,0,0,0,0),(1331,'2013-02-10 05:47:35',1,0,0,0,0,0,0),(1332,'2013-02-10 05:48:35',1,0,0,0,0,0,0),(1333,'2013-02-10 05:49:35',1,0,0,0,0,0,0),(1334,'2013-02-10 05:50:35',1,0,0,0,0,0,0),(1335,'2013-02-10 05:51:35',1,0,0,0,0,0,0),(1336,'2013-02-10 05:52:35',1,0,0,0,0,0,0),(1337,'2013-02-10 05:53:35',1,0,0,0,0,0,0),(1338,'2013-02-10 05:54:35',1,0,0,0,0,0,0),(1339,'2013-02-10 05:55:35',1,0,0,0,0,0,0),(1340,'2013-02-10 05:56:35',1,0,0,0,0,0,0),(1341,'2013-02-10 05:57:35',1,0,0,0,0,0,0),(1342,'2013-02-10 05:58:35',1,0,0,0,0,0,0),(1343,'2013-02-10 05:59:35',1,0,0,0,0,0,0),(1344,'2013-02-10 06:00:35',1,0,0,0,0,0,0),(1345,'2013-02-10 06:01:35',1,0,0,0,0,0,0),(1346,'2013-02-10 06:02:35',1,0,0,0,0,0,0),(1347,'2013-02-10 06:03:35',1,0,0,0,0,0,0),(1348,'2013-02-10 06:04:35',1,0,0,0,0,0,0),(1349,'2013-02-10 06:05:35',1,0,0,0,0,0,0),(1350,'2013-02-10 06:06:35',1,0,0,0,0,0,0),(1351,'2013-02-10 06:07:35',1,0,0,0,0,0,0),(1352,'2013-02-10 06:08:35',1,0,0,0,0,0,0),(1353,'2013-02-10 06:09:35',1,0,0,0,0,0,0),(1354,'2013-02-10 06:10:35',1,0,0,0,0,0,0),(1355,'2013-02-10 06:11:35',1,0,0,0,0,0,0),(1356,'2013-02-10 06:12:35',1,0,0,0,0,0,0),(1357,'2013-02-10 06:13:35',1,0,0,0,0,0,0),(1358,'2013-02-10 06:14:35',1,0,0,0,0,0,0),(1359,'2013-02-10 06:15:35',1,0,0,0,0,0,0),(1360,'2013-02-10 06:16:35',1,0,0,0,0,0,0),(1361,'2013-02-10 06:17:35',1,0,0,0,0,0,0),(1362,'2013-02-10 06:18:35',1,0,0,0,0,0,0),(1363,'2013-02-10 06:19:35',1,0,0,0,0,0,0),(1364,'2013-02-10 06:20:35',1,0,0,0,0,0,0),(1365,'2013-02-10 06:21:35',1,0,0,0,0,0,0),(1366,'2013-02-10 06:22:35',1,0,0,0,0,0,0),(1367,'2013-02-10 06:23:35',1,0,0,0,0,0,0),(1368,'2013-02-10 06:24:35',1,0,0,0,0,0,0),(1369,'2013-02-10 06:25:35',1,0,0,0,0,0,0),(1370,'2013-02-10 06:26:35',1,0,0,0,0,0,0),(1371,'2013-02-10 06:27:35',1,0,0,0,0,0,0),(1372,'2013-02-10 06:28:35',1,0,0,0,0,0,0),(1373,'2013-02-10 06:29:35',1,0,0,0,0,0,0),(1374,'2013-02-10 06:30:35',1,0,0,0,0,0,0),(1375,'2013-02-10 06:31:35',1,0,0,0,0,0,0),(1376,'2013-02-10 06:32:35',1,0,0,0,0,0,0),(1377,'2013-02-10 06:33:35',1,0,0,0,0,0,0),(1378,'2013-02-10 06:34:35',1,0,0,0,0,0,0),(1379,'2013-02-10 06:35:35',1,0,0,0,0,0,0),(1380,'2013-02-10 06:36:35',1,0,0,0,0,0,0),(1381,'2013-02-10 06:37:35',1,0,0,0,0,0,0),(1382,'2013-02-10 06:38:35',1,0,0,0,0,0,0),(1383,'2013-02-10 06:39:35',1,0,0,0,0,0,0),(1384,'2013-02-10 06:40:35',1,0,0,0,0,0,0),(1385,'2013-02-10 06:41:35',1,0,0,0,0,0,0),(1386,'2013-02-10 06:42:35',1,0,0,0,0,0,0),(1387,'2013-02-10 06:43:35',1,0,0,0,0,0,0),(1388,'2013-02-10 06:44:35',1,0,0,0,0,0,0),(1389,'2013-02-10 06:45:35',1,0,0,0,0,0,0),(1390,'2013-02-10 06:46:35',1,0,0,0,0,0,0),(1391,'2013-02-10 06:47:35',1,0,0,0,0,0,0),(1392,'2013-02-10 06:48:35',1,0,0,0,0,0,0),(1393,'2013-02-10 06:49:35',1,0,0,0,0,0,0),(1394,'2013-02-10 06:50:35',1,0,0,0,0,0,0),(1395,'2013-02-10 06:51:35',1,0,0,0,0,0,0),(1396,'2013-02-10 06:52:35',1,0,0,0,0,0,0),(1397,'2013-02-10 06:53:35',1,0,0,0,0,0,0),(1398,'2013-02-10 06:54:35',1,0,0,0,0,0,0),(1399,'2013-02-10 06:55:35',1,0,0,0,0,0,0),(1400,'2013-02-10 06:56:35',1,0,0,0,0,0,0),(1401,'2013-02-10 06:57:35',1,0,0,0,0,0,0),(1402,'2013-02-10 06:58:35',1,0,0,0,0,0,0),(1403,'2013-02-10 06:59:35',1,0,0,0,0,0,0),(1404,'2013-02-10 07:00:35',1,0,0,0,0,0,0),(1405,'2013-02-10 07:01:35',1,0,0,0,0,0,0),(1406,'2013-02-10 07:02:35',1,0,0,0,0,0,0),(1407,'2013-02-10 07:03:35',1,0,0,0,0,0,0),(1408,'2013-02-10 07:04:35',1,0,0,0,0,0,0),(1409,'2013-02-10 07:05:35',1,0,0,0,0,0,0),(1410,'2013-02-10 07:06:35',1,0,0,0,0,0,0),(1411,'2013-02-10 07:07:35',1,0,0,0,0,0,0),(1412,'2013-02-10 07:08:35',1,0,0,0,0,0,0),(1413,'2013-02-10 07:09:35',1,0,0,0,0,0,0),(1414,'2013-02-10 07:10:35',1,0,0,0,0,0,0),(1415,'2013-02-10 07:11:35',1,0,0,0,0,0,0),(1416,'2013-02-10 07:12:35',1,0,0,0,0,0,0),(1417,'2013-02-10 07:13:35',1,0,0,0,0,0,0),(1418,'2013-02-10 07:14:35',1,0,0,0,0,0,0),(1419,'2013-02-10 07:15:35',1,0,0,0,0,0,0),(1420,'2013-02-10 07:16:35',1,0,0,0,0,0,0),(1421,'2013-02-10 07:17:35',1,0,0,0,0,0,0),(1422,'2013-02-10 07:18:35',1,0,0,0,0,0,0),(1423,'2013-02-10 07:19:35',1,0,0,0,0,0,0),(1424,'2013-02-10 07:20:35',1,0,0,0,0,0,0),(1425,'2013-02-10 07:21:35',1,0,0,0,0,0,0),(1426,'2013-02-10 07:22:35',1,0,0,0,0,0,0),(1427,'2013-02-10 07:23:35',1,0,0,0,0,0,0),(1428,'2013-02-10 07:24:35',1,0,0,0,0,0,0),(1429,'2013-02-10 07:25:35',1,0,0,0,0,0,0),(1430,'2013-02-10 07:26:35',1,0,0,0,0,0,0),(1431,'2013-02-10 07:27:35',1,0,0,0,0,0,0),(1432,'2013-02-10 07:28:35',1,0,0,0,0,0,0),(1433,'2013-02-10 07:29:35',1,0,0,0,0,0,0),(1434,'2013-02-10 07:30:35',1,0,0,0,0,0,0),(1435,'2013-02-10 07:31:35',1,0,0,0,0,0,0),(1436,'2013-02-10 07:32:35',1,0,0,0,0,0,0),(1437,'2013-02-10 07:33:35',1,0,0,0,0,0,0),(1438,'2013-02-10 07:34:35',1,0,0,0,0,0,0),(1439,'2013-02-10 07:35:35',1,0,0,0,0,0,0),(1440,'2013-02-10 07:36:35',1,0,0,0,0,0,0),(1441,'2013-02-10 07:37:35',1,0,0,0,0,0,0),(1442,'2013-02-10 07:38:35',1,0,0,0,0,0,0),(1443,'2013-02-10 07:39:35',1,0,0,0,0,0,0),(1444,'2013-02-10 07:40:35',1,0,0,0,0,0,0),(1445,'2013-02-10 07:41:35',1,0,0,0,0,0,0),(1446,'2013-02-10 07:42:35',1,0,0,0,0,0,0),(1447,'2013-02-10 07:43:35',1,0,0,0,0,0,0),(1448,'2013-02-10 07:44:35',1,0,0,0,0,0,0),(1449,'2013-02-10 07:45:35',1,0,0,0,0,0,0),(1450,'2013-02-10 07:46:35',1,0,0,0,0,0,0),(1451,'2013-02-10 07:47:35',1,0,0,0,0,0,0),(1452,'2013-02-10 07:48:35',1,0,0,0,0,0,0),(1453,'2013-02-10 07:49:35',1,0,0,0,0,0,0),(1454,'2013-02-10 07:50:35',1,0,0,0,0,0,0),(1455,'2013-02-10 07:51:35',1,0,0,0,0,0,0),(1456,'2013-02-10 07:52:35',1,0,0,0,0,0,0),(1457,'2013-02-10 07:53:35',1,0,0,0,0,0,0),(1458,'2013-02-10 07:54:35',1,0,0,0,0,0,0),(1459,'2013-02-10 07:55:35',1,0,0,0,0,0,0),(1460,'2013-02-10 07:56:35',1,0,0,0,0,0,0),(1461,'2013-02-10 07:57:35',1,0,0,0,0,0,0),(1462,'2013-02-10 07:58:35',1,0,0,0,0,0,0),(1463,'2013-02-10 07:59:35',1,0,0,0,0,0,0),(1464,'2013-02-10 08:00:35',1,0,0,0,0,0,0),(1465,'2013-02-10 08:01:35',1,0,0,0,0,0,0),(1466,'2013-02-10 08:02:35',1,0,0,0,0,0,0),(1467,'2013-02-10 08:03:35',1,0,0,0,0,0,0),(1468,'2013-02-10 08:04:35',1,0,0,0,0,0,0),(1469,'2013-02-10 08:05:35',1,0,0,0,0,0,0),(1470,'2013-02-10 08:06:35',1,0,0,0,0,0,0),(1471,'2013-02-10 08:07:35',1,0,0,0,0,0,0),(1472,'2013-02-10 08:08:35',1,0,0,0,0,0,0),(1473,'2013-02-10 08:09:35',1,0,0,0,0,0,0),(1474,'2013-02-10 08:10:35',1,0,0,0,0,0,0),(1475,'2013-02-10 08:11:35',1,0,0,0,0,0,0),(1476,'2013-02-10 08:12:35',1,0,0,0,0,0,0),(1477,'2013-02-10 08:13:35',1,0,0,0,0,0,0),(1478,'2013-02-10 08:14:35',1,0,0,0,0,0,0),(1479,'2013-02-10 08:15:35',1,0,0,0,0,0,0),(1480,'2013-02-10 08:16:35',1,0,0,0,0,0,0),(1481,'2013-02-10 08:17:35',1,0,0,0,0,0,0),(1482,'2013-02-10 08:18:35',1,0,0,0,0,0,0),(1483,'2013-02-10 08:19:35',1,0,0,0,0,0,0),(1484,'2013-02-10 08:20:35',1,0,0,0,0,0,0),(1485,'2013-02-10 08:21:35',1,0,0,0,0,0,0),(1486,'2013-02-10 08:22:35',1,0,0,0,0,0,0),(1487,'2013-02-10 08:23:35',1,0,0,0,0,0,0),(1488,'2013-02-10 08:24:35',1,0,0,0,0,0,0),(1489,'2013-02-10 08:25:35',1,0,0,0,0,0,0),(1490,'2013-02-10 08:26:35',1,0,0,0,0,0,0),(1491,'2013-02-10 08:27:35',1,0,0,0,0,0,0),(1492,'2013-02-10 08:28:35',1,0,0,0,0,0,0),(1493,'2013-02-10 08:29:35',1,0,0,0,0,0,0),(1494,'2013-02-10 08:30:35',1,0,0,0,0,0,0),(1495,'2013-02-10 08:31:35',1,0,0,0,0,0,0),(1496,'2013-02-10 08:32:35',1,0,0,0,0,0,0),(1497,'2013-02-10 08:33:35',1,0,0,0,0,0,0),(1498,'2013-02-10 08:34:35',1,0,0,0,0,0,0),(1499,'2013-02-10 08:35:35',1,0,0,0,0,0,0),(1500,'2013-02-10 08:36:35',1,0,0,0,0,0,0),(1501,'2013-02-10 08:37:35',1,0,0,0,0,0,0),(1502,'2013-02-10 08:38:35',1,0,0,0,0,0,0),(1503,'2013-02-10 08:39:35',1,0,0,0,0,0,0),(1504,'2013-02-10 08:40:35',1,0,0,0,0,0,0),(1505,'2013-02-10 08:41:35',1,0,0,0,0,0,0),(1506,'2013-02-10 08:42:35',1,0,0,0,0,0,0),(1507,'2013-02-10 08:43:35',1,0,0,0,0,0,0),(1508,'2013-02-10 08:44:35',1,0,0,0,0,0,0),(1509,'2013-02-10 08:45:35',1,0,0,0,0,0,0),(1510,'2013-02-10 08:46:35',1,0,0,0,0,0,0),(1511,'2013-02-10 08:47:35',1,0,0,0,0,0,0),(1512,'2013-02-10 08:48:35',1,0,0,0,0,0,0),(1513,'2013-02-10 08:49:35',1,0,0,0,0,0,0),(1514,'2013-02-10 08:50:35',1,0,0,0,0,0,0),(1515,'2013-02-10 08:51:35',1,0,0,0,0,0,0),(1516,'2013-02-10 08:52:35',1,0,0,0,0,0,0),(1517,'2013-02-10 08:53:35',1,0,0,0,0,0,0),(1518,'2013-02-10 08:54:35',1,0,0,0,0,0,0),(1519,'2013-02-10 08:55:35',1,0,0,0,0,0,0),(1520,'2013-02-10 08:56:35',1,0,0,0,0,0,0),(1521,'2013-02-10 08:57:35',1,0,0,0,0,0,0),(1522,'2013-02-10 08:58:35',1,0,0,0,0,0,0),(1523,'2013-02-10 08:59:35',1,0,0,0,0,0,0),(1524,'2013-02-10 09:00:35',1,0,0,0,0,0,0),(1525,'2013-02-10 09:01:35',1,0,0,0,0,0,0),(1526,'2013-02-10 09:02:35',1,0,0,0,0,0,0),(1527,'2013-02-10 09:03:35',1,0,0,0,0,0,0),(1528,'2013-02-10 09:04:35',1,0,0,0,0,0,0),(1529,'2013-02-10 09:05:35',1,0,0,0,0,0,0),(1530,'2013-02-10 09:06:35',1,0,0,0,0,0,0),(1531,'2013-02-10 09:07:35',1,0,0,0,0,0,0),(1532,'2013-02-10 09:08:35',1,0,0,0,0,0,0),(1533,'2013-02-10 09:09:35',1,0,0,0,0,0,0),(1534,'2013-02-10 09:10:35',1,0,0,0,0,0,0),(1535,'2013-02-10 09:11:35',1,0,0,0,0,0,0),(1536,'2013-02-10 09:12:35',1,0,0,0,0,0,0),(1537,'2013-02-10 09:13:35',1,0,0,0,0,0,0),(1538,'2013-02-10 09:14:35',1,0,0,0,0,0,0),(1539,'2013-02-10 09:15:35',1,0,0,0,0,0,0),(1540,'2013-02-10 09:16:35',1,0,0,0,0,0,0),(1541,'2013-02-10 09:17:35',1,0,0,0,0,0,0),(1542,'2013-02-10 09:18:35',1,0,0,0,0,0,0),(1543,'2013-02-10 09:19:35',1,0,0,0,0,0,0),(1544,'2013-02-10 09:20:35',1,0,0,0,0,0,0),(1545,'2013-02-10 09:21:35',1,0,0,0,0,0,0),(1546,'2013-02-10 09:22:35',1,0,0,0,0,0,0),(1547,'2013-02-10 09:23:35',1,0,0,0,0,0,0),(1548,'2013-02-10 09:24:35',1,0,0,0,0,0,0),(1549,'2013-02-10 09:25:35',1,0,0,0,0,0,0),(1550,'2013-02-10 09:26:35',1,0,0,0,0,0,0),(1551,'2013-02-10 09:27:35',1,0,0,0,0,0,0),(1552,'2013-02-10 09:28:35',1,0,0,0,0,0,0),(1553,'2013-02-10 09:29:35',1,0,0,0,0,0,0),(1554,'2013-02-10 09:30:35',1,0,0,0,0,0,0),(1555,'2013-02-10 09:31:35',1,0,0,0,0,0,0),(1556,'2013-02-10 09:32:35',1,0,0,0,0,0,0),(1557,'2013-02-10 09:33:35',1,0,0,0,0,0,0),(1558,'2013-02-10 09:34:35',1,0,0,0,0,0,0),(1559,'2013-02-10 09:35:35',1,0,0,0,0,0,0),(1560,'2013-02-10 09:36:35',1,0,0,0,0,0,0),(1561,'2013-02-10 09:37:35',1,0,0,0,0,0,0),(1562,'2013-02-10 09:38:35',1,0,0,0,0,0,0),(1563,'2013-02-10 09:39:35',1,0,0,0,0,0,0),(1564,'2013-02-10 09:40:35',1,0,0,0,0,0,0),(1565,'2013-02-10 09:41:35',1,0,0,0,0,0,0),(1566,'2013-02-10 09:42:35',1,0,0,0,0,0,0),(1567,'2013-02-10 09:43:35',1,0,0,0,0,0,0),(1568,'2013-02-10 09:44:35',1,0,0,0,0,0,0),(1569,'2013-02-10 09:45:35',1,0,0,0,0,0,0),(1570,'2013-02-10 09:46:35',1,0,0,0,0,0,0),(1571,'2013-02-10 09:47:35',1,0,0,0,0,0,0),(1572,'2013-02-10 09:48:35',1,0,0,0,0,0,0),(1573,'2013-02-10 09:49:35',1,0,0,0,0,0,0),(1574,'2013-02-10 09:50:35',1,0,0,0,0,0,0),(1575,'2013-02-10 09:51:35',1,0,0,0,0,0,0),(1576,'2013-02-10 09:52:35',1,0,0,0,0,0,0),(1577,'2013-02-10 09:53:35',1,0,0,0,0,0,0),(1578,'2013-02-10 09:54:35',1,0,0,0,0,0,0),(1579,'2013-02-10 09:55:35',1,0,0,0,0,0,0),(1580,'2013-02-10 09:56:35',1,0,0,0,0,0,0),(1581,'2013-02-10 09:57:35',1,0,0,0,0,0,0),(1582,'2013-02-10 09:58:35',1,0,0,0,0,0,0),(1583,'2013-02-10 09:59:35',1,0,0,0,0,0,0),(1584,'2013-02-10 10:00:35',1,0,0,0,0,0,0),(1585,'2013-02-10 10:01:35',1,0,0,0,0,0,0),(1586,'2013-02-10 10:02:35',1,0,0,0,0,0,0),(1587,'2013-02-10 10:03:35',1,0,0,0,0,0,0),(1588,'2013-02-10 10:04:35',1,0,0,0,0,0,0),(1589,'2013-02-10 10:05:35',1,0,0,0,0,0,0),(1590,'2013-02-10 10:06:35',1,0,0,0,0,0,0),(1591,'2013-02-10 10:07:35',1,0,0,0,0,0,0),(1592,'2013-02-10 10:08:35',1,0,0,0,0,0,0),(1593,'2013-02-10 10:09:35',1,0,0,0,0,0,0),(1594,'2013-02-10 10:10:35',1,0,0,0,0,0,0),(1595,'2013-02-10 10:11:35',1,0,0,0,0,0,0),(1596,'2013-02-10 10:12:35',1,0,0,0,0,0,0),(1597,'2013-02-10 10:13:35',1,0,0,0,0,0,0),(1598,'2013-02-10 10:14:35',1,0,0,0,0,0,0),(1599,'2013-02-10 10:15:35',1,0,0,0,0,0,0),(1600,'2013-02-10 10:16:35',1,0,0,0,0,0,0),(1601,'2013-02-10 10:17:35',1,0,0,0,0,0,0),(1602,'2013-02-10 10:18:35',1,0,0,0,0,0,0),(1603,'2013-02-10 10:19:35',1,0,0,0,0,0,0),(1604,'2013-02-10 10:20:35',1,0,0,0,0,0,0),(1605,'2013-02-10 10:21:35',1,0,0,0,0,0,0),(1606,'2013-02-10 10:22:35',1,0,0,0,0,0,0),(1607,'2013-02-10 10:23:35',1,0,0,0,0,0,0),(1608,'2013-02-10 10:24:35',1,0,0,0,0,0,0),(1609,'2013-02-10 10:25:35',1,0,0,0,0,0,0),(1610,'2013-02-10 10:26:35',1,0,0,0,0,0,0),(1611,'2013-02-10 10:27:35',1,0,0,0,0,0,0),(1612,'2013-02-10 10:28:35',1,0,0,0,0,0,0),(1613,'2013-02-10 10:29:35',1,0,0,0,0,0,0),(1614,'2013-02-10 10:30:35',1,0,0,0,0,0,0),(1615,'2013-02-10 10:31:35',1,0,0,0,0,0,0),(1616,'2013-02-10 10:32:35',1,0,0,0,0,0,0),(1617,'2013-02-10 10:33:35',1,0,0,0,0,0,0),(1618,'2013-02-10 10:34:35',1,0,0,0,0,0,0),(1619,'2013-02-10 10:35:35',1,0,0,0,0,0,0),(1620,'2013-02-10 10:36:35',1,0,0,0,0,0,0),(1621,'2013-02-10 10:37:35',1,0,0,0,0,0,0),(1622,'2013-02-10 10:38:35',1,0,0,0,0,0,0),(1623,'2013-02-10 10:39:35',1,0,0,0,0,0,0),(1624,'2013-02-10 10:40:35',1,0,0,0,0,0,0),(1625,'2013-02-10 10:41:35',1,0,0,0,0,0,0),(1626,'2013-02-10 10:42:35',1,0,0,0,0,0,0),(1627,'2013-02-10 10:43:35',1,0,0,0,0,0,0),(1628,'2013-02-10 10:44:35',1,0,0,0,0,0,0),(1629,'2013-02-10 10:45:35',1,0,0,0,0,0,0),(1630,'2013-02-10 10:46:35',1,0,0,0,0,0,0),(1631,'2013-02-10 10:47:35',1,0,0,0,0,0,0),(1632,'2013-02-10 10:48:35',1,0,0,0,0,0,0),(1633,'2013-02-10 10:49:35',1,0,0,0,0,0,0),(1634,'2013-02-10 10:50:35',1,0,0,0,0,0,0),(1635,'2013-02-10 10:51:35',1,0,0,0,0,0,0),(1636,'2013-02-10 10:52:35',1,0,0,0,0,0,0),(1637,'2013-02-10 10:53:35',1,0,0,0,0,0,0),(1638,'2013-02-10 10:54:35',1,0,0,0,0,0,0),(1639,'2013-02-10 10:55:35',1,0,0,0,0,0,0),(1640,'2013-02-10 10:56:35',1,0,0,0,0,0,0),(1641,'2013-02-10 10:57:35',1,0,0,0,0,0,0),(1642,'2013-02-10 10:58:35',1,0,0,0,0,0,0),(1643,'2013-02-10 10:59:35',1,0,0,0,0,0,0),(1644,'2013-02-10 11:00:35',1,0,0,0,0,0,0),(1645,'2013-02-10 11:01:35',1,0,0,0,0,0,0),(1646,'2013-02-10 11:02:35',1,0,0,0,0,0,0),(1647,'2013-02-10 11:03:35',1,0,0,0,0,0,0),(1648,'2013-02-10 11:04:35',1,0,0,0,0,0,0),(1649,'2013-02-10 11:05:35',1,0,0,0,0,0,0),(1650,'2013-02-10 11:06:35',1,0,0,0,0,0,0),(1651,'2013-02-10 11:07:35',1,0,0,0,0,0,0),(1652,'2013-02-10 11:08:35',1,0,0,0,0,0,0),(1653,'2013-02-10 11:09:35',1,0,0,0,0,0,0),(1654,'2013-02-10 11:10:35',1,0,0,0,0,0,0),(1655,'2013-02-10 11:11:35',1,0,0,0,0,0,0),(1656,'2013-02-10 11:12:35',1,0,0,0,0,0,0),(1657,'2013-02-10 11:13:35',1,0,0,0,0,0,0),(1658,'2013-02-10 11:14:35',1,0,0,0,0,0,0),(1659,'2013-02-10 11:15:35',1,0,0,0,0,0,0),(1660,'2013-02-10 11:16:35',1,0,0,0,0,0,0),(1661,'2013-02-10 11:17:35',1,0,0,0,0,0,0),(1662,'2013-02-10 11:18:35',1,0,0,0,0,0,0),(1663,'2013-02-10 11:19:35',1,0,0,0,0,0,0),(1664,'2013-02-10 11:20:35',1,0,0,0,0,0,0),(1665,'2013-02-10 11:21:35',1,0,0,0,0,0,0),(1666,'2013-02-10 11:22:35',1,0,0,0,0,0,0),(1667,'2013-02-10 11:23:35',1,0,0,0,0,0,0),(1668,'2013-02-10 11:24:35',1,0,0,0,0,0,0),(1669,'2013-02-10 11:25:35',1,0,0,0,0,0,0),(1670,'2013-02-10 11:26:35',1,0,0,0,0,0,0),(1671,'2013-02-10 11:27:35',1,0,0,0,0,0,0),(1672,'2013-02-10 11:28:35',1,0,0,0,0,0,0),(1673,'2013-02-10 11:29:35',1,0,0,0,0,0,0),(1674,'2013-02-10 11:30:35',1,0,0,0,0,0,0),(1675,'2013-02-10 11:31:35',1,0,0,0,0,0,0),(1676,'2013-02-10 11:32:35',1,0,0,0,0,0,0),(1677,'2013-02-10 11:33:35',1,0,0,0,0,0,0),(1678,'2013-02-10 11:34:35',1,0,0,0,0,0,0),(1679,'2013-02-10 11:35:35',1,0,0,0,0,0,0),(1680,'2013-02-10 11:36:35',1,0,0,0,0,0,0),(1681,'2013-02-10 11:37:35',1,0,0,0,0,0,0),(1682,'2013-02-10 11:38:35',1,0,0,0,0,0,0),(1683,'2013-02-10 11:39:35',1,0,0,0,0,0,0),(1684,'2013-02-10 11:40:35',1,0,0,0,0,0,0),(1685,'2013-02-10 11:41:35',1,0,0,0,0,0,0),(1686,'2013-02-10 11:42:35',1,0,0,0,0,0,0),(1687,'2013-02-10 11:43:35',1,0,0,0,0,0,0),(1688,'2013-02-10 11:44:35',1,0,0,0,0,0,0),(1689,'2013-02-10 11:45:35',1,0,0,0,0,0,0),(1690,'2013-02-10 11:46:35',1,0,0,0,0,0,0),(1691,'2013-02-10 11:47:35',1,0,0,0,0,0,0),(1692,'2013-02-10 11:48:35',1,0,0,0,0,0,0),(1693,'2013-02-10 11:49:35',1,0,0,0,0,0,0),(1694,'2013-02-10 11:50:35',1,0,0,0,0,0,0),(1695,'2013-02-10 11:51:35',1,0,0,0,0,0,0),(1696,'2013-02-10 11:52:35',1,0,0,0,0,0,0),(1697,'2013-02-10 11:53:35',1,0,0,0,0,0,0),(1698,'2013-02-10 11:54:35',1,0,0,0,0,0,0),(1699,'2013-02-10 11:55:35',1,0,0,0,0,0,0),(1700,'2013-02-10 11:56:35',1,0,0,0,0,0,0),(1701,'2013-02-10 11:57:35',1,0,0,0,0,0,0),(1702,'2013-02-10 11:58:35',1,0,0,0,0,0,0),(1703,'2013-02-10 11:59:35',1,0,0,0,0,0,0),(1704,'2013-02-10 12:00:35',1,0,0,0,0,0,0),(1705,'2013-02-10 12:01:35',1,0,0,0,0,0,0),(1706,'2013-02-10 12:02:35',1,0,0,0,0,0,0),(1707,'2013-02-10 12:03:35',1,0,0,0,0,0,0),(1708,'2013-02-10 12:04:35',1,0,0,0,0,0,0),(1709,'2013-02-10 12:05:35',1,0,0,0,0,0,0),(1710,'2013-02-10 12:06:35',1,0,0,0,0,0,0),(1711,'2013-02-10 12:07:35',1,0,0,0,0,0,0),(1712,'2013-02-10 12:08:35',1,0,0,0,0,0,0),(1713,'2013-02-10 12:09:35',1,0,0,0,0,0,0),(1714,'2013-02-10 12:10:35',1,0,0,0,0,0,0),(1715,'2013-02-10 12:11:35',1,0,0,0,0,0,0),(1716,'2013-02-10 12:12:35',1,0,0,0,0,0,0),(1717,'2013-02-10 12:13:35',1,0,0,0,0,0,0),(1718,'2013-02-10 12:14:35',1,0,0,0,0,0,0),(1719,'2013-02-10 12:15:35',1,0,0,0,0,0,0),(1720,'2013-02-10 12:16:35',1,0,0,0,0,0,0),(1721,'2013-02-10 12:17:35',1,0,0,0,0,0,0),(1722,'2013-02-10 12:18:35',1,0,0,0,0,0,0),(1723,'2013-02-10 12:19:35',1,0,0,0,0,0,0),(1724,'2013-02-10 12:20:35',1,0,0,0,0,0,0),(1725,'2013-02-10 12:21:35',1,0,0,0,0,0,0),(1726,'2013-02-10 12:22:35',1,0,0,0,0,0,0),(1727,'2013-02-10 12:23:35',1,0,0,0,0,0,0),(1728,'2013-02-10 12:24:35',1,0,0,0,0,0,0),(1729,'2013-02-10 12:25:35',1,0,0,0,0,0,0),(1730,'2013-02-10 12:26:35',1,0,0,0,0,0,0),(1731,'2013-02-10 12:27:35',1,0,0,0,0,0,0),(1732,'2013-02-10 12:28:35',1,0,0,0,0,0,0),(1733,'2013-02-10 12:29:35',1,0,0,0,0,0,0),(1734,'2013-02-10 12:30:35',1,0,0,0,0,0,0),(1735,'2013-02-10 12:31:35',1,0,0,0,0,0,0),(1736,'2013-02-10 12:32:35',1,0,0,0,0,0,0),(1737,'2013-02-10 12:33:35',1,0,0,0,0,0,0),(1738,'2013-02-10 12:34:35',1,0,0,0,0,0,0),(1739,'2013-02-10 12:35:35',1,0,0,0,0,0,0),(1740,'2013-02-10 12:36:35',1,0,0,0,0,0,0),(1741,'2013-02-10 12:37:35',1,0,0,0,0,0,0),(1742,'2013-02-10 12:38:35',1,0,0,0,0,0,0),(1743,'2013-02-10 12:39:35',1,0,0,0,0,0,0),(1744,'2013-02-10 12:40:35',1,0,0,0,0,0,0),(1745,'2013-02-10 12:41:35',1,0,0,0,0,0,0),(1746,'2013-02-10 12:42:35',1,0,0,0,0,0,0),(1747,'2013-02-10 12:43:35',1,0,0,0,0,0,0),(1748,'2013-02-10 12:44:35',1,0,0,0,0,0,0),(1749,'2013-02-10 12:45:35',1,0,0,0,0,0,0),(1750,'2013-02-10 12:46:35',1,0,0,0,0,0,0),(1751,'2013-02-10 12:47:35',1,0,0,0,0,0,0),(1752,'2013-02-10 12:48:35',1,0,0,0,0,0,0),(1753,'2013-02-10 12:49:35',1,0,0,0,0,0,0),(1754,'2013-02-10 12:50:35',1,0,0,0,0,0,0),(1755,'2013-02-10 12:51:35',1,0,0,0,0,0,0),(1756,'2013-02-10 12:52:35',1,0,0,0,0,0,0),(1757,'2013-02-10 12:53:35',1,0,0,0,0,0,0),(1758,'2013-02-10 12:54:35',1,0,0,0,0,0,0),(1759,'2013-02-10 12:55:35',1,0,0,0,0,0,0),(1760,'2013-02-10 12:56:35',1,0,0,0,0,0,0),(1761,'2013-02-10 12:57:35',1,0,0,0,0,0,0),(1762,'2013-02-10 12:58:35',1,0,0,0,0,0,0),(1763,'2013-02-10 13:23:13',0,0,0,0,0,0,0),(1764,'2013-02-10 13:24:13',0,0,0,0,0,0,0),(1765,'2013-02-10 13:25:13',0,0,0,0,0,0,0),(1766,'2013-02-10 13:26:13',0,0,0,0,0,0,0),(1767,'2013-02-10 13:27:13',0,0,0,0,0,0,0),(1768,'2013-02-10 13:28:13',0,0,0,0,0,0,0),(1769,'2013-02-10 13:29:13',1,0,0,0,0,0,0),(1770,'2013-02-10 13:30:13',1,0,0,0,0,0,0),(1771,'2013-02-10 13:31:13',1,0,0,0,0,0,0),(1772,'2013-02-10 13:32:24',0,0,0,0,0,0,0),(1773,'2013-02-10 13:39:34',0,0,0,0,0,0,0),(1774,'2013-02-10 13:43:40',0,0,0,0,0,0,0),(1775,'2013-02-10 13:44:40',1,0,0,0,0,0,0),(1776,'2013-02-10 13:45:40',1,0,0,0,0,0,0),(1777,'2013-02-10 13:46:40',1,0,0,0,0,0,0),(1778,'2013-02-10 13:47:40',1,0,0,0,0,0,0),(1779,'2013-02-10 13:48:40',1,0,0,0,0,0,0),(1780,'2013-02-10 13:49:40',1,0,0,0,0,0,0),(1781,'2013-02-10 13:50:40',1,0,0,0,0,0,0),(1782,'2013-02-10 13:51:40',1,0,0,0,0,0,0),(1783,'2013-02-10 13:52:40',1,0,0,0,0,0,0),(1784,'2013-02-10 13:53:40',1,0,0,0,0,0,0),(1785,'2013-02-10 13:57:44',1,0,0,0,0,0,0),(1786,'2013-02-10 13:58:44',1,0,0,0,0,0,0),(1787,'2013-02-10 13:59:44',1,0,0,0,0,0,0),(1788,'2013-02-10 14:01:20',1,0,0,0,0,0,0),(1789,'2013-02-10 14:05:12',1,0,0,0,0,0,0),(1790,'2013-02-10 14:14:37',0,0,0,0,0,0,0),(1791,'2013-02-10 14:15:37',0,0,0,0,0,0,0),(1792,'2013-02-10 14:19:26',1,0,0,0,0,0,0),(1793,'2013-02-10 14:20:26',1,0,0,0,0,0,0),(1794,'2013-02-10 14:24:48',1,0,0,0,0,0,0),(1795,'2013-02-10 14:25:48',1,0,0,0,0,0,0),(1796,'2013-02-10 14:26:48',1,0,0,0,0,0,0),(1797,'2013-02-10 14:27:48',1,0,0,0,0,0,0),(1798,'2013-02-10 14:28:48',1,0,0,0,0,0,0),(1799,'2013-02-10 14:29:48',1,0,0,0,0,0,0),(1800,'2013-02-10 14:30:48',1,0,0,0,0,0,0),(1801,'2013-02-10 14:31:48',1,0,0,0,0,0,0),(1802,'2013-02-10 14:32:48',1,0,0,0,0,0,0),(1803,'2013-02-10 14:33:48',1,0,0,0,0,0,0),(1804,'2013-02-10 14:34:48',1,0,0,0,0,0,0),(1805,'2013-02-10 14:35:48',1,0,0,0,0,0,0),(1806,'2013-02-10 14:38:04',1,0,0,0,0,0,0),(1807,'2013-02-10 14:45:59',1,0,0,0,0,0,0),(1808,'2013-02-10 14:46:59',1,0,0,0,0,0,0),(1809,'2013-02-10 14:50:40',1,0,0,0,0,0,0),(1810,'2013-02-10 14:51:40',1,0,0,0,0,0,0),(1811,'2013-02-10 14:52:40',1,0,0,0,0,0,0),(1812,'2013-02-10 14:54:37',1,0,0,0,0,0,0),(1813,'2013-02-10 15:55:32',1,0,0,0,0,0,0),(1814,'2013-02-10 15:56:32',1,0,0,0,0,0,0),(1815,'2013-02-10 15:57:32',1,0,0,0,0,0,0),(1816,'2013-02-10 15:58:32',1,0,0,0,0,0,0),(1817,'2013-02-10 15:59:32',1,0,0,0,0,0,0),(1818,'2013-02-10 16:00:32',1,0,0,0,0,0,0),(1819,'2013-02-10 16:01:32',1,0,0,0,0,0,0);

/*Table structure for table `world_stats_npc_kill_user` */

DROP TABLE IF EXISTS `world_stats_npc_kill_user`;

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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=latin1 COMMENT='Event log: NPC kill user.';

/*Data for the table `world_stats_npc_kill_user` */

insert  into `world_stats_npc_kill_user`(`id`,`user_id`,`npc_template_id`,`user_level`,`user_x`,`user_y`,`npc_x`,`npc_y`,`map_id`,`when`) values (1,6,1,1,512,512,279,594,1,'2013-02-04 18:55:22'),(2,6,1,1,512,512,762,1047,1,'2013-02-04 18:55:35'),(3,6,1,1,512,512,342,502,1,'2013-02-04 18:55:47'),(4,1,1,18,512,512,734,913,1,'2013-02-05 23:37:25'),(5,1,1,18,512,512,503,754,1,'2013-02-05 23:37:29'),(6,1,1,18,512,512,665,523,1,'2013-02-05 23:37:32'),(7,1,1,18,512,512,614,523,1,'2013-02-05 23:37:34'),(8,1,1,18,512,512,536,523,1,'2013-02-05 23:37:35'),(9,1,1,18,512,512,536,523,1,'2013-02-05 23:37:37'),(10,1,1,22,512,512,1531,713,1,'2013-02-06 00:08:38'),(11,1,1,22,512,512,1394,715,1,'2013-02-06 01:25:04'),(12,1,1,23,512,512,369,603,1,'2013-02-08 00:58:08'),(13,1,1,35,512,512,668,630,1,'2013-02-10 14:26:07'),(14,1,1,35,512,512,496,511,1,'2013-02-10 14:26:46'),(15,1,1,35,512,512,510,538,1,'2013-02-10 14:26:58'),(16,1,1,35,512,512,508,511,1,'2013-02-10 14:27:03'),(17,1,1,35,512,512,508,511,1,'2013-02-10 14:27:19'),(18,1,1,35,512,512,508,511,1,'2013-02-10 14:27:35'),(19,1,1,35,512,512,510,538,1,'2013-02-10 14:27:50'),(20,1,1,40,512,512,727,628,1,'2013-02-10 14:30:50');

/*Table structure for table `world_stats_quest_accept` */

DROP TABLE IF EXISTS `world_stats_quest_accept`;

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
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT COMMENT='Event log: User accepts a quest.';

/*Data for the table `world_stats_quest_accept` */

insert  into `world_stats_quest_accept`(`id`,`user_id`,`quest_id`,`map_id`,`x`,`y`,`when`) values (1,1,0,1,623,591,'2013-02-07 00:44:31'),(2,1,1,1,623,591,'2013-02-07 00:44:34'),(3,1,1,1,630,621,'2013-02-07 00:47:19'),(4,1,1,1,658,572,'2013-02-09 15:02:58'),(5,1,1,1,628,578,'2013-02-09 15:07:59'),(6,1,1,1,616,587,'2013-02-09 15:15:44'),(7,1,1,1,617,556,'2013-02-09 15:20:55'),(8,1,1,1,624,585,'2013-02-09 15:22:12'),(9,1,1,1,630,589,'2013-02-09 15:33:32'),(10,1,1,1,614,639,'2013-02-09 16:04:40'),(11,1,1,1,621,578,'2013-02-09 16:08:22'),(12,1,1,1,630,601,'2013-02-10 14:05:29'),(13,1,1,1,598,594,'2013-02-10 14:21:26'),(14,1,1,1,634,587,'2013-02-10 14:46:29');

/*Table structure for table `world_stats_quest_cancel` */

DROP TABLE IF EXISTS `world_stats_quest_cancel`;

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

/*Data for the table `world_stats_quest_cancel` */

/*Table structure for table `world_stats_quest_complete` */

DROP TABLE IF EXISTS `world_stats_quest_complete`;

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

/*Data for the table `world_stats_quest_complete` */

insert  into `world_stats_quest_complete`(`id`,`user_id`,`quest_id`,`map_id`,`x`,`y`,`when`) values (1,1,1,1,630,621,'2013-02-07 00:47:06'),(2,1,1,1,626,572,'2013-02-09 15:02:50'),(3,1,0,1,626,572,'2013-02-09 15:02:52'),(4,1,1,1,628,578,'2013-02-09 15:07:52'),(5,1,1,1,616,587,'2013-02-09 15:15:36'),(6,1,1,1,625,596,'2013-02-09 15:19:16'),(7,1,1,1,562,585,'2013-02-09 15:22:00'),(8,1,1,1,655,614,'2013-02-09 15:33:25'),(9,1,1,1,607,596,'2013-02-09 15:55:21'),(10,1,1,1,632,606,'2013-02-09 16:04:30'),(11,1,1,1,621,578,'2013-02-09 16:06:49'),(12,1,1,1,618,585,'2013-02-09 16:11:28'),(13,1,1,1,598,594,'2013-02-10 14:21:23'),(14,1,1,4,657,434,'2013-02-10 14:37:26'),(15,1,1,4,683,439,'2013-02-10 15:56:39');

/*Table structure for table `world_stats_user_consume_item` */

DROP TABLE IF EXISTS `world_stats_user_consume_item`;

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

/*Data for the table `world_stats_user_consume_item` */

insert  into `world_stats_user_consume_item`(`id`,`user_id`,`item_template_id`,`map_id`,`x`,`y`,`when`) values (1,1,2,1,425,714,'2013-02-04 18:41:25'),(2,1,2,1,425,714,'2013-02-04 18:41:26'),(3,1,1,1,1436,737,'2013-02-08 10:37:42'),(4,1,1,1,1436,737,'2013-02-08 10:37:43'),(5,1,1,1,1436,737,'2013-02-08 10:37:43'),(6,1,1,1,1436,737,'2013-02-08 10:37:44'),(7,1,1,1,1436,737,'2013-02-08 10:37:45'),(8,1,1,1,1436,737,'2013-02-08 10:37:45'),(9,1,1,1,1436,737,'2013-02-08 10:37:45'),(10,1,2,4,631,458,'2013-02-09 20:59:53'),(11,1,2,4,631,458,'2013-02-09 20:59:59'),(12,1,2,4,631,458,'2013-02-09 20:59:59'),(13,1,2,4,364,454,'2013-02-09 22:25:16'),(14,1,2,4,364,454,'2013-02-09 22:25:17'),(15,1,2,4,364,454,'2013-02-09 22:25:17'),(16,1,2,4,364,454,'2013-02-09 22:25:17'),(17,1,2,4,364,454,'2013-02-09 22:25:18'),(18,1,2,4,364,454,'2013-02-09 22:25:18'),(19,1,2,4,364,454,'2013-02-09 22:25:18'),(20,1,2,4,364,454,'2013-02-09 22:25:18'),(21,1,2,4,364,454,'2013-02-09 22:25:18'),(22,1,1,4,572,413,'2013-02-09 23:31:54'),(23,1,1,4,572,413,'2013-02-09 23:31:55'),(24,1,1,4,572,413,'2013-02-09 23:31:55'),(25,1,1,4,572,413,'2013-02-09 23:31:56');

/*Table structure for table `world_stats_user_kill_npc` */

DROP TABLE IF EXISTS `world_stats_user_kill_npc`;

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
) ENGINE=InnoDB AUTO_INCREMENT=605 DEFAULT CHARSET=latin1 COMMENT='Event log: User kills NPC.';

/*Data for the table `world_stats_user_kill_npc` */

insert  into `world_stats_user_kill_npc`(`id`,`user_id`,`npc_template_id`,`user_level`,`user_x`,`user_y`,`npc_x`,`npc_y`,`map_id`,`when`) values (1,1,1,1,496,1201,0,0,1,'2013-02-04 17:51:43'),(2,1,1,1,478,1201,0,0,1,'2013-02-04 17:51:43'),(3,1,1,1,489,1226,0,0,1,'2013-02-04 17:52:00'),(4,1,1,2,1193,289,0,0,1,'2013-02-04 17:52:24'),(5,1,1,2,1193,318,0,0,1,'2013-02-04 17:52:25'),(6,1,1,2,1035,732,0,0,1,'2013-02-04 17:52:35'),(7,1,1,2,873,732,0,0,1,'2013-02-04 17:52:37'),(8,1,1,2,966,721,0,0,1,'2013-02-04 17:52:43'),(9,1,1,2,347,545,0,0,1,'2013-02-04 17:53:00'),(10,1,1,3,1440,705,0,0,1,'2013-02-04 17:53:12'),(11,1,1,3,1537,705,0,0,1,'2013-02-04 17:53:13'),(12,1,1,3,1505,226,0,0,1,'2013-02-04 17:53:18'),(13,1,1,3,1130,733,0,0,1,'2013-02-04 18:40:26'),(14,1,1,3,1011,733,0,0,1,'2013-02-04 18:40:27'),(15,1,1,3,518,625,0,0,1,'2013-02-04 18:40:30'),(16,1,1,4,1163,1857,0,0,1,'2013-02-04 18:40:53'),(17,1,1,4,1163,1857,0,0,1,'2013-02-04 18:40:53'),(18,1,1,4,1163,1857,0,0,1,'2013-02-04 18:40:54'),(19,1,1,4,375,728,0,0,1,'2013-02-04 18:41:19'),(20,1,1,4,781,788,0,0,1,'2013-02-04 18:41:59'),(21,6,1,1,558,512,0,0,1,'2013-02-04 18:55:51'),(22,6,1,1,558,512,0,0,1,'2013-02-04 18:55:54'),(23,6,1,1,558,512,0,0,1,'2013-02-04 18:55:59'),(24,1,1,4,106,656,0,0,1,'2013-02-04 18:56:30'),(25,1,1,4,285,481,0,0,1,'2013-02-04 19:02:44'),(26,1,1,5,346,649,0,0,1,'2013-02-04 19:03:04'),(27,1,1,4,285,703,0,0,1,'2013-02-04 19:05:01'),(28,1,1,5,285,677,0,0,1,'2013-02-04 19:05:01'),(29,1,1,4,248,591,0,0,1,'2013-02-04 20:10:01'),(30,1,1,5,529,638,0,0,1,'2013-02-04 20:10:15'),(31,1,1,5,412,575,0,0,1,'2013-02-04 20:10:29'),(32,1,1,5,348,690,0,0,1,'2013-02-04 20:10:30'),(33,1,1,5,1235,698,0,0,1,'2013-02-04 20:10:37'),(34,1,1,5,1505,258,0,0,1,'2013-02-04 20:21:39'),(35,1,1,5,547,463,0,0,1,'2013-02-04 20:21:47'),(36,1,1,6,486,463,0,0,1,'2013-02-04 20:21:47'),(37,1,1,6,544,440,0,0,1,'2013-02-04 20:22:00'),(38,1,1,6,595,332,0,0,1,'2013-02-04 20:22:03'),(39,1,1,6,595,760,0,0,1,'2013-02-04 20:22:07'),(40,1,1,6,449,227,0,0,1,'2013-02-04 20:23:02'),(41,1,1,6,449,227,0,0,1,'2013-02-04 20:23:02'),(42,1,1,7,449,227,0,0,1,'2013-02-04 20:23:04'),(43,1,1,7,449,227,0,0,1,'2013-02-04 20:25:12'),(44,1,1,7,595,227,0,0,1,'2013-02-04 20:29:09'),(45,1,1,7,328,1012,0,0,1,'2013-02-04 20:34:27'),(46,1,1,7,751,613,0,0,1,'2013-02-04 20:35:16'),(47,1,1,7,486,472,0,0,1,'2013-02-04 20:35:18'),(48,1,1,7,587,490,0,0,1,'2013-02-04 21:13:23'),(49,1,1,8,514,238,0,0,1,'2013-02-04 21:13:35'),(50,1,1,8,698,933,0,0,1,'2013-02-04 21:13:39'),(51,1,1,8,802,908,0,0,1,'2013-02-04 21:13:44'),(52,1,1,8,658,623,0,0,1,'2013-02-04 21:13:47'),(53,1,1,8,522,577,0,0,1,'2013-02-04 21:13:56'),(54,1,1,8,738,605,0,0,1,'2013-02-04 21:13:57'),(55,1,1,9,1512,735,0,0,1,'2013-02-04 21:14:02'),(56,1,1,9,947,729,0,0,1,'2013-02-04 21:14:26'),(57,1,1,9,662,513,0,0,1,'2013-02-04 21:14:28'),(58,1,1,9,662,416,0,0,1,'2013-02-04 21:14:29'),(59,1,1,9,662,607,0,0,1,'2013-02-04 21:14:38'),(60,1,1,9,630,1073,0,0,1,'2013-02-04 21:14:42'),(61,1,1,10,709,1192,0,0,1,'2013-02-04 21:14:43'),(62,1,1,10,546,808,0,0,1,'2013-02-04 21:15:03'),(63,1,1,10,453,336,0,0,1,'2013-02-04 21:15:08'),(64,1,1,10,449,421,0,0,1,'2013-02-04 21:15:09'),(65,1,1,7,400,598,0,0,1,'2013-02-04 21:29:21'),(66,1,1,8,519,598,0,0,1,'2013-02-04 21:29:33'),(67,1,1,8,587,620,0,0,1,'2013-02-04 21:29:41'),(68,1,1,8,544,782,0,0,1,'2013-02-04 21:30:01'),(69,1,1,8,986,1523,0,0,1,'2013-02-04 21:30:05'),(70,1,1,7,436,893,0,0,1,'2013-02-04 23:11:07'),(71,1,1,8,529,800,0,0,1,'2013-02-04 23:11:08'),(72,1,1,8,533,627,0,0,1,'2013-02-04 23:11:20'),(73,1,1,8,378,627,0,0,1,'2013-02-04 23:11:23'),(74,1,1,8,1217,256,0,0,1,'2013-02-04 23:11:39'),(75,1,1,8,1184,325,0,0,1,'2013-02-04 23:11:39'),(76,1,1,8,192,1099,0,0,1,'2013-02-04 23:11:45'),(77,1,1,9,1699,719,0,0,1,'2013-02-04 23:11:56'),(78,1,1,9,449,421,0,0,1,'2013-02-04 23:20:32'),(79,1,1,9,449,421,0,0,1,'2013-02-04 23:20:33'),(80,1,1,9,449,421,0,0,1,'2013-02-04 23:20:34'),(81,1,1,9,1240,705,0,0,1,'2013-02-04 23:20:44'),(82,1,1,9,679,752,0,0,1,'2013-02-04 23:20:47'),(83,1,1,10,574,856,0,0,1,'2013-02-04 23:20:48'),(84,1,1,10,1623,778,0,0,1,'2013-02-04 23:20:57'),(85,1,1,10,662,1059,0,0,1,'2013-02-04 23:22:04'),(86,1,1,10,619,994,0,0,1,'2013-02-04 23:22:07'),(87,1,1,10,1530,762,0,0,1,'2013-02-04 23:22:24'),(88,1,1,10,1242,726,0,0,1,'2013-02-04 23:22:30'),(89,1,1,11,1137,726,0,0,1,'2013-02-04 23:22:45'),(90,1,1,7,414,818,0,0,1,'2013-02-04 23:23:50'),(91,1,1,8,1038,483,0,0,1,'2013-02-04 23:24:12'),(92,1,6,8,303,434,0,0,2,'2013-02-04 23:24:36'),(93,1,6,8,202,311,0,0,2,'2013-02-04 23:25:02'),(94,1,6,8,205,290,0,0,2,'2013-02-04 23:25:03'),(95,1,6,8,468,290,0,0,2,'2013-02-04 23:25:09'),(96,1,1,9,1026,943,0,0,1,'2013-02-04 23:25:55'),(97,1,1,9,929,1040,0,0,1,'2013-02-04 23:25:56'),(98,1,1,9,363,1605,0,0,1,'2013-02-04 23:25:59'),(99,1,1,9,105,641,0,0,1,'2013-02-04 23:26:08'),(100,1,1,9,514,359,0,0,1,'2013-02-04 23:26:32'),(101,1,6,10,220,484,0,0,2,'2013-02-04 23:26:47'),(102,1,1,10,960,430,0,0,1,'2013-02-04 23:27:12'),(103,1,1,10,1179,895,0,0,1,'2013-02-04 23:27:15'),(104,1,1,10,1235,891,0,0,1,'2013-02-04 23:27:16'),(105,1,1,10,1235,743,0,0,1,'2013-02-04 23:27:35'),(106,1,1,11,1235,743,0,0,1,'2013-02-04 23:31:51'),(107,1,1,11,1235,743,0,0,1,'2013-02-04 23:32:08'),(108,1,1,11,1098,768,0,0,1,'2013-02-04 23:32:20'),(109,1,1,11,605,584,0,0,1,'2013-02-04 23:32:24'),(110,1,1,11,576,411,0,0,1,'2013-02-04 23:32:25'),(111,1,1,11,373,485,0,0,1,'2013-02-04 23:32:31'),(112,1,1,12,285,577,0,0,1,'2013-02-04 23:33:16'),(113,1,1,12,485,404,0,0,1,'2013-02-04 23:33:19'),(114,1,1,12,776,1325,0,0,1,'2013-02-04 23:33:27'),(115,1,1,12,1342,713,0,0,1,'2013-02-04 23:33:34'),(116,1,1,12,1558,704,0,0,1,'2013-02-04 23:33:43'),(117,1,1,12,1472,705,0,0,1,'2013-02-04 23:33:46'),(118,1,1,13,567,355,0,0,1,'2013-02-04 23:33:54'),(119,1,1,11,1152,743,0,0,1,'2013-02-04 23:46:11'),(120,1,1,11,1044,743,0,0,1,'2013-02-04 23:46:12'),(121,1,1,11,108,524,0,0,1,'2013-02-04 23:47:04'),(122,1,6,11,417,438,0,0,2,'2013-02-04 23:48:35'),(123,1,6,11,413,438,0,0,2,'2013-02-04 23:48:37'),(124,1,6,12,427,438,0,0,2,'2013-02-04 23:48:42'),(125,1,6,12,427,463,0,0,2,'2013-02-04 23:48:42'),(126,1,6,12,456,589,0,0,2,'2013-02-04 23:48:50'),(127,1,6,12,604,611,0,0,2,'2013-02-04 23:48:54'),(128,1,6,13,575,611,0,0,2,'2013-02-04 23:48:58'),(129,1,6,13,586,611,0,0,2,'2013-02-04 23:49:02'),(130,1,6,13,614,611,0,0,2,'2013-02-04 23:49:03'),(131,1,6,13,596,611,0,0,2,'2013-02-04 23:49:04'),(132,1,6,14,611,611,0,0,2,'2013-02-04 23:49:06'),(133,1,1,14,1096,805,0,0,1,'2013-02-04 23:49:18'),(134,1,1,14,1211,700,0,0,1,'2013-02-04 23:49:19'),(135,1,1,14,1069,711,0,0,1,'2013-02-04 23:49:21'),(136,1,1,14,249,1074,0,0,1,'2013-02-04 23:49:43'),(137,1,1,15,249,1189,0,0,1,'2013-02-04 23:49:44'),(138,1,1,15,483,1189,0,0,1,'2013-02-04 23:49:45'),(139,1,1,15,353,1739,0,0,1,'2013-02-04 23:49:59'),(140,1,1,15,1584,370,0,0,1,'2013-02-04 23:50:09'),(141,1,1,15,1407,742,0,0,1,'2013-02-04 23:50:13'),(142,1,1,15,824,666,0,0,1,'2013-02-04 23:50:16'),(143,1,1,16,849,706,0,0,1,'2013-02-05 01:17:39'),(144,1,1,16,684,825,0,0,1,'2013-02-05 23:09:04'),(145,1,1,16,756,825,0,0,1,'2013-02-05 23:09:05'),(146,1,1,16,489,547,0,0,1,'2013-02-05 23:29:08'),(147,1,1,16,489,547,0,0,1,'2013-02-05 23:29:09'),(148,1,1,16,960,517,0,0,1,'2013-02-05 23:29:38'),(149,1,1,17,654,510,0,0,1,'2013-02-05 23:29:41'),(150,1,1,17,727,1198,0,0,1,'2013-02-05 23:33:50'),(151,1,1,17,727,1198,0,0,1,'2013-02-05 23:33:51'),(152,1,1,17,874,1144,0,0,1,'2013-02-05 23:34:05'),(153,1,1,17,457,672,470,683,1,'2013-02-05 23:34:27'),(154,1,1,17,586,672,575,683,1,'2013-02-05 23:34:32'),(155,1,1,18,586,672,575,683,1,'2013-02-05 23:34:33'),(156,1,1,18,327,672,367,672,1,'2013-02-05 23:34:43'),(157,1,1,18,205,672,193,672,1,'2013-02-05 23:34:45'),(158,1,1,18,205,672,193,672,1,'2013-02-05 23:34:45'),(159,1,1,18,562,749,0,0,1,'2013-02-05 23:37:42'),(160,1,1,18,417,560,0,0,1,'2013-02-05 23:38:08'),(161,1,1,19,1004,917,994,924,1,'2013-02-05 23:38:19'),(162,1,1,19,1004,917,994,924,1,'2013-02-05 23:38:19'),(163,1,1,19,1004,917,994,924,1,'2013-02-05 23:38:20'),(164,1,1,19,1004,917,994,924,1,'2013-02-05 23:38:22'),(165,1,1,19,1004,917,994,924,1,'2013-02-05 23:38:22'),(166,1,1,19,1004,917,994,924,1,'2013-02-05 23:38:23'),(167,1,1,20,1004,917,994,924,1,'2013-02-05 23:38:24'),(168,1,1,20,1004,917,994,924,1,'2013-02-05 23:38:24'),(169,1,1,20,1004,917,994,924,1,'2013-02-05 23:38:25'),(170,1,1,20,1004,917,994,924,1,'2013-02-05 23:38:25'),(171,1,1,20,1004,917,0,0,1,'2013-02-05 23:38:26'),(172,1,1,20,1069,946,0,0,1,'2013-02-05 23:38:39'),(173,1,1,21,1069,841,0,0,1,'2013-02-05 23:38:51'),(174,1,1,21,1447,716,0,0,1,'2013-02-05 23:38:54'),(175,1,1,21,1580,464,0,0,1,'2013-02-05 23:38:56'),(176,1,1,21,1638,432,0,0,1,'2013-02-05 23:39:53'),(177,1,1,21,1584,259,1608,227,1,'2013-02-06 00:08:26'),(178,1,1,21,1584,294,1608,269,1,'2013-02-06 00:08:26'),(179,1,1,22,1609,294,1614,269,1,'2013-02-06 00:08:28'),(180,1,1,22,1609,294,1614,269,1,'2013-02-06 00:08:28'),(181,1,1,22,327,775,0,0,1,'2013-02-06 00:31:59'),(182,1,1,22,1368,705,0,0,1,'2013-02-06 01:25:02'),(183,1,1,22,1368,705,0,0,1,'2013-02-06 01:25:03'),(184,1,1,22,1505,424,1505,424,1,'2013-02-06 16:07:46'),(185,1,1,22,1505,416,0,0,1,'2013-02-07 00:28:03'),(186,1,1,23,1160,355,0,0,1,'2013-02-07 00:28:17'),(187,1,1,23,1505,425,0,0,1,'2013-02-07 00:28:26'),(188,1,1,22,1630,419,0,0,1,'2013-02-07 00:29:29'),(189,1,1,23,496,254,0,0,1,'2013-02-07 00:29:49'),(190,1,1,23,575,805,0,0,1,'2013-02-07 00:29:53'),(191,1,1,23,632,733,0,0,1,'2013-02-07 00:30:01'),(192,1,1,23,794,614,0,0,1,'2013-02-07 00:30:19'),(193,1,1,22,1505,970,0,0,1,'2013-02-07 00:35:57'),(194,1,1,23,1558,1010,0,0,1,'2013-02-07 00:35:58'),(195,1,1,23,889,718,0,0,1,'2013-02-07 00:36:29'),(196,1,1,23,914,718,0,0,1,'2013-02-07 00:36:29'),(197,1,1,23,594,592,0,0,1,'2013-02-07 00:36:37'),(198,1,1,23,652,1070,0,0,1,'2013-02-07 00:36:59'),(199,1,1,23,799,811,0,0,1,'2013-02-07 00:37:32'),(200,1,1,24,867,811,0,0,1,'2013-02-07 00:37:33'),(201,1,1,24,972,811,0,0,1,'2013-02-07 00:37:47'),(202,1,1,24,939,735,0,0,1,'2013-02-07 00:38:06'),(203,1,1,24,939,735,0,0,1,'2013-02-07 00:38:07'),(204,1,1,24,939,735,0,0,1,'2013-02-07 00:38:21'),(205,1,1,24,965,764,0,0,1,'2013-02-07 00:38:37'),(206,1,1,25,965,764,0,0,1,'2013-02-07 00:38:42'),(207,1,1,25,1001,1109,0,0,1,'2013-02-07 00:38:53'),(208,1,1,25,1016,911,0,0,1,'2013-02-07 00:39:05'),(209,1,1,25,1156,706,0,0,1,'2013-02-07 00:39:12'),(210,1,1,25,651,918,0,0,1,'2013-02-07 00:39:23'),(211,1,1,25,149,1410,0,0,1,'2013-02-07 00:39:36'),(212,1,6,26,1085,908,1061,908,1,'2013-02-07 00:39:48'),(213,1,6,26,1085,908,1080,908,1,'2013-02-07 00:39:50'),(214,1,1,26,1085,908,0,0,1,'2013-02-07 00:39:52'),(215,1,1,26,1117,815,0,0,1,'2013-02-07 00:39:57'),(216,1,1,26,1533,739,0,0,1,'2013-02-07 00:40:00'),(217,1,6,27,309,211,0,0,2,'2013-02-07 00:40:38'),(218,1,6,27,291,211,0,0,2,'2013-02-07 00:40:42'),(219,1,6,27,291,211,0,0,2,'2013-02-07 00:40:43'),(220,1,6,27,323,211,0,0,2,'2013-02-07 00:40:45'),(221,1,6,28,370,291,0,0,2,'2013-02-07 00:40:50'),(222,1,6,28,370,316,0,0,2,'2013-02-07 00:40:54'),(223,1,6,28,392,471,0,0,2,'2013-02-07 00:41:00'),(224,1,6,29,467,492,0,0,2,'2013-02-07 00:41:05'),(225,1,6,29,428,564,0,0,2,'2013-02-07 00:41:08'),(226,1,1,29,1375,729,0,0,1,'2013-02-07 00:41:19'),(227,1,1,29,1505,369,0,0,1,'2013-02-07 00:41:33'),(228,1,1,29,1199,317,0,0,1,'2013-02-07 00:45:05'),(229,1,6,30,403,427,0,0,2,'2013-02-07 00:45:17'),(230,1,6,30,403,427,0,0,2,'2013-02-07 00:45:19'),(231,1,6,30,403,427,0,0,2,'2013-02-07 00:45:22'),(232,1,6,30,403,427,0,0,2,'2013-02-07 00:45:25'),(233,1,6,31,439,567,0,0,2,'2013-02-07 00:45:51'),(234,1,6,31,439,567,0,0,2,'2013-02-07 00:45:52'),(235,1,6,31,439,567,0,0,2,'2013-02-07 00:45:53'),(236,1,6,31,414,545,0,0,2,'2013-02-07 00:45:55'),(237,1,6,32,414,545,0,0,2,'2013-02-07 00:45:56'),(238,1,6,32,494,545,0,0,2,'2013-02-07 00:45:57'),(239,1,6,32,544,545,0,0,2,'2013-02-07 00:45:59'),(240,1,6,32,555,545,0,0,2,'2013-02-07 00:46:00'),(241,1,6,33,555,545,0,0,2,'2013-02-07 00:46:01'),(242,1,6,33,555,545,0,0,2,'2013-02-07 00:46:03'),(243,1,6,33,555,545,0,0,2,'2013-02-07 00:46:04'),(244,1,6,34,555,545,0,0,2,'2013-02-07 00:46:09'),(245,1,6,34,547,545,0,0,2,'2013-02-07 00:46:11'),(246,1,6,34,501,545,0,0,2,'2013-02-07 00:46:12'),(247,1,6,34,512,571,0,0,2,'2013-02-07 00:46:14'),(248,1,6,35,792,571,0,0,2,'2013-02-07 00:46:16'),(249,1,6,35,814,571,0,0,2,'2013-02-07 00:46:17'),(250,1,6,35,814,499,0,0,2,'2013-02-07 00:46:19'),(251,1,6,35,814,499,0,0,2,'2013-02-07 00:46:20'),(252,1,6,36,814,499,0,0,2,'2013-02-07 00:46:21'),(253,1,6,36,814,484,0,0,2,'2013-02-07 00:46:23'),(254,1,6,36,814,517,0,0,2,'2013-02-07 00:46:23'),(255,1,6,36,681,542,0,0,2,'2013-02-07 00:46:24'),(256,1,6,37,605,477,0,0,2,'2013-02-07 00:46:26'),(257,1,6,37,652,283,0,0,2,'2013-02-07 00:46:27'),(258,1,6,37,688,283,0,0,2,'2013-02-07 00:46:28'),(259,1,6,38,688,283,0,0,2,'2013-02-07 00:46:31'),(260,1,6,38,637,283,0,0,2,'2013-02-07 00:46:32'),(261,1,6,38,591,265,0,0,2,'2013-02-07 00:46:33'),(262,1,6,38,591,250,0,0,2,'2013-02-07 00:46:34'),(263,1,6,39,573,250,0,0,2,'2013-02-07 00:46:35'),(264,1,6,39,367,250,0,0,2,'2013-02-07 00:46:36'),(265,1,6,39,335,185,0,0,2,'2013-02-07 00:46:37'),(266,1,6,39,263,236,0,0,2,'2013-02-07 00:46:38'),(267,1,6,40,195,545,0,0,2,'2013-02-07 00:46:42'),(268,1,6,40,234,545,0,0,2,'2013-02-07 00:46:42'),(269,1,6,40,465,545,0,0,2,'2013-02-07 00:46:44'),(270,1,1,41,594,603,0,0,1,'2013-02-07 00:47:36'),(271,1,1,41,932,603,0,0,1,'2013-02-07 00:48:09'),(272,1,1,41,1555,691,0,0,1,'2013-02-07 00:48:59'),(273,1,1,22,1641,916,0,0,1,'2013-02-08 00:05:19'),(274,1,1,23,623,473,0,0,1,'2013-02-08 00:05:31'),(275,1,1,23,666,481,0,0,1,'2013-02-08 00:05:31'),(276,1,1,23,727,549,0,0,1,'2013-02-08 00:05:32'),(277,1,1,23,741,588,0,0,1,'2013-02-08 00:06:58'),(278,1,1,22,659,642,0,0,1,'2013-02-08 00:08:10'),(279,1,1,23,1425,742,0,0,1,'2013-02-08 00:08:25'),(280,1,1,22,950,826,0,0,1,'2013-02-08 00:13:18'),(281,1,1,23,770,930,0,0,1,'2013-02-08 00:13:20'),(282,1,1,23,569,941,0,0,1,'2013-02-08 00:13:21'),(283,1,1,23,1677,1147,0,0,1,'2013-02-08 00:13:40'),(284,1,6,23,429,387,0,0,2,'2013-02-08 00:14:18'),(285,1,6,23,439,387,0,0,2,'2013-02-08 00:14:18'),(286,1,6,24,468,419,0,0,2,'2013-02-08 00:14:20'),(287,1,6,24,468,329,0,0,2,'2013-02-08 00:14:20'),(288,1,6,24,468,376,0,0,2,'2013-02-08 00:14:21'),(289,1,6,24,468,358,0,0,2,'2013-02-08 00:14:21'),(290,1,6,25,468,376,0,0,2,'2013-02-08 00:14:22'),(291,1,6,25,468,358,0,0,2,'2013-02-08 00:14:23'),(292,1,6,25,457,369,0,0,2,'2013-02-08 00:14:23'),(293,1,6,26,342,293,0,0,2,'2013-02-08 00:14:26'),(294,1,6,26,497,542,0,0,2,'2013-02-08 00:14:31'),(295,1,6,26,493,614,0,0,2,'2013-02-08 00:14:32'),(296,1,1,26,589,700,0,0,1,'2013-02-08 00:14:39'),(297,1,1,26,423,700,0,0,1,'2013-02-08 00:14:41'),(298,1,1,27,398,700,0,0,1,'2013-02-08 00:14:42'),(299,1,1,27,873,1082,0,0,1,'2013-02-08 00:15:12'),(300,1,1,27,776,1082,0,0,1,'2013-02-08 00:15:13'),(301,1,1,22,666,458,0,0,1,'2013-02-08 00:18:29'),(302,1,1,23,612,296,0,0,1,'2013-02-08 00:18:31'),(303,1,1,23,759,653,0,0,1,'2013-02-08 00:18:39'),(304,1,1,22,1037,743,0,0,1,'2013-02-08 00:25:16'),(305,1,1,23,936,743,0,0,1,'2013-02-08 00:25:17'),(306,1,1,23,1396,742,0,0,1,'2013-02-08 00:25:28'),(307,1,1,23,1505,652,0,0,1,'2013-02-08 00:25:36'),(308,1,1,22,914,714,0,0,1,'2013-02-08 00:26:52'),(309,1,1,23,594,714,0,0,1,'2013-02-08 00:26:54'),(310,1,1,23,471,714,0,0,1,'2013-02-08 00:26:55'),(311,1,1,22,1505,606,0,0,1,'2013-02-08 00:28:02'),(312,1,1,22,1382,729,0,0,1,'2013-02-08 00:29:27'),(313,1,1,23,1173,729,0,0,1,'2013-02-08 00:29:28'),(314,1,1,22,583,473,0,0,1,'2013-02-08 00:30:38'),(315,1,1,23,486,473,0,0,1,'2013-02-08 00:30:39'),(316,1,1,22,1274,707,0,0,1,'2013-02-08 00:32:16'),(317,1,1,23,529,790,0,0,1,'2013-02-08 00:32:28'),(318,1,1,23,453,1056,0,0,1,'2013-02-08 00:32:30'),(319,1,1,23,464,1056,0,0,1,'2013-02-08 00:32:31'),(320,1,1,22,896,686,0,0,1,'2013-02-08 00:56:44'),(321,1,1,23,752,686,0,0,1,'2013-02-08 00:56:46'),(322,1,1,23,763,686,0,0,1,'2013-02-08 00:56:47'),(323,1,1,22,713,725,0,0,1,'2013-02-08 01:01:44'),(324,1,1,23,775,1543,0,0,1,'2013-02-08 10:29:14'),(325,1,1,23,656,1543,0,0,1,'2013-02-08 10:29:15'),(326,1,1,23,1461,742,0,0,1,'2013-02-08 10:34:47'),(327,1,1,23,763,742,0,0,1,'2013-02-08 10:35:11'),(328,1,1,23,1652,212,0,0,1,'2013-02-08 10:36:19'),(329,1,1,23,306,737,0,0,1,'2013-02-08 10:38:08'),(330,1,1,23,432,737,0,0,1,'2013-02-08 10:38:09'),(331,1,1,23,335,737,0,0,1,'2013-02-08 10:38:10'),(332,1,1,23,374,748,0,0,1,'2013-02-08 10:38:45'),(333,1,1,23,285,965,0,0,1,'2013-02-08 10:39:15'),(334,1,1,24,1321,729,0,0,1,'2013-02-08 10:39:26'),(335,1,1,24,1447,729,0,0,1,'2013-02-08 10:39:27'),(336,1,1,24,960,481,0,0,1,'2013-02-08 10:40:09'),(337,1,1,24,519,548,0,0,1,'2013-02-08 10:40:12'),(338,1,1,24,584,548,0,0,1,'2013-02-08 10:40:13'),(339,1,1,24,761,1756,0,0,1,'2013-02-08 10:42:08'),(340,1,1,25,819,1756,0,0,1,'2013-02-08 10:42:10'),(341,1,1,25,428,770,0,0,1,'2013-02-08 10:42:33'),(342,1,1,23,795,700,0,0,1,'2013-02-09 14:59:22'),(343,1,1,23,792,578,0,0,1,'2013-02-09 14:59:24'),(344,1,1,23,482,484,0,0,1,'2013-02-09 14:59:26'),(345,1,1,23,561,669,0,0,1,'2013-02-09 15:00:32'),(346,1,1,23,504,669,0,0,1,'2013-02-09 15:00:32'),(347,1,1,23,464,398,0,0,1,'2013-02-09 15:00:37'),(348,1,1,23,1504,770,0,0,1,'2013-02-09 15:06:48'),(349,1,1,23,647,583,0,0,1,'2013-02-09 15:06:58'),(350,1,1,23,669,583,0,0,1,'2013-02-09 15:07:02'),(351,1,6,23,504,553,0,0,2,'2013-02-09 15:07:17'),(352,1,6,23,547,553,0,0,2,'2013-02-09 15:07:20'),(353,1,6,24,512,553,0,0,2,'2013-02-09 15:07:23'),(354,1,6,24,512,553,0,0,2,'2013-02-09 15:07:25'),(355,1,6,24,512,553,0,0,2,'2013-02-09 15:07:27'),(356,1,6,24,512,553,0,0,2,'2013-02-09 15:07:28'),(357,1,6,25,605,553,0,0,2,'2013-02-09 15:07:37'),(358,1,6,25,645,607,0,0,2,'2013-02-09 15:07:37'),(359,1,6,25,645,599,0,0,2,'2013-02-09 15:07:38'),(360,1,6,26,627,556,0,0,2,'2013-02-09 15:07:40'),(361,1,6,26,670,517,0,0,2,'2013-02-09 15:07:41'),(362,1,6,26,634,517,0,0,2,'2013-02-09 15:07:41'),(363,1,1,23,720,589,0,0,1,'2013-02-09 15:09:39'),(364,1,1,23,759,697,0,0,1,'2013-02-09 15:09:41'),(365,1,6,23,497,437,0,0,2,'2013-02-09 15:09:53'),(366,1,6,23,497,437,0,0,2,'2013-02-09 15:09:54'),(367,1,6,23,497,437,0,0,2,'2013-02-09 15:09:57'),(368,1,6,24,497,437,0,0,2,'2013-02-09 15:09:58'),(369,1,6,24,497,437,0,0,2,'2013-02-09 15:09:59'),(370,1,6,24,497,437,0,0,2,'2013-02-09 15:10:01'),(371,1,6,25,497,437,0,0,2,'2013-02-09 15:10:07'),(372,1,6,25,468,625,0,0,2,'2013-02-09 15:10:16'),(373,1,1,25,804,445,0,0,1,'2013-02-09 15:10:21'),(374,1,1,25,724,574,0,0,1,'2013-02-09 15:10:23'),(375,1,1,25,753,643,0,0,1,'2013-02-09 15:10:36'),(376,1,6,26,443,495,0,0,2,'2013-02-09 15:10:41'),(377,1,6,26,414,520,0,0,2,'2013-02-09 15:10:42'),(378,1,6,26,429,520,0,0,2,'2013-02-09 15:10:42'),(379,1,6,26,339,592,0,0,2,'2013-02-09 15:10:43'),(380,1,6,27,313,592,0,0,2,'2013-02-09 15:10:44'),(381,1,6,27,238,466,0,0,2,'2013-02-09 15:10:45'),(382,1,6,27,238,473,0,0,2,'2013-02-09 15:10:46'),(383,1,6,27,238,506,0,0,2,'2013-02-09 15:10:47'),(384,1,6,28,346,538,0,0,2,'2013-02-09 15:10:47'),(385,1,1,28,616,587,0,0,1,'2013-02-09 15:15:33'),(386,1,1,23,994,180,0,0,1,'2013-02-09 15:18:04'),(387,1,6,23,468,310,0,0,2,'2013-02-09 15:18:28'),(388,1,6,23,349,230,0,0,2,'2013-02-09 15:18:29'),(389,1,6,23,353,263,0,0,2,'2013-02-09 15:18:31'),(390,1,6,24,360,263,0,0,2,'2013-02-09 15:18:31'),(391,1,6,24,414,263,0,0,2,'2013-02-09 15:18:32'),(392,1,6,24,439,263,0,0,2,'2013-02-09 15:18:33'),(393,1,6,24,475,263,0,0,2,'2013-02-09 15:18:33'),(394,1,6,25,443,273,0,0,2,'2013-02-09 15:18:34'),(395,1,1,25,621,582,0,0,1,'2013-02-09 15:18:42'),(396,1,6,25,605,535,0,0,2,'2013-02-09 15:18:50'),(397,1,6,25,565,535,0,0,2,'2013-02-09 15:18:51'),(398,1,6,26,472,535,0,0,2,'2013-02-09 15:18:51'),(399,1,6,26,555,498,0,0,2,'2013-02-09 15:18:58'),(400,1,6,26,616,516,0,0,2,'2013-02-09 15:18:59'),(401,1,6,26,616,531,0,0,2,'2013-02-09 15:18:59'),(402,1,6,27,616,531,0,0,2,'2013-02-09 15:19:00'),(403,1,6,27,580,531,0,0,2,'2013-02-09 15:19:01'),(404,1,6,27,497,545,0,0,2,'2013-02-09 15:19:01'),(405,1,1,28,625,596,0,0,1,'2013-02-09 15:20:46'),(406,1,1,28,585,596,0,0,1,'2013-02-09 15:20:48'),(407,1,1,28,693,596,0,0,1,'2013-02-09 15:20:49'),(408,1,1,28,671,617,0,0,1,'2013-02-09 15:21:01'),(409,1,6,28,634,610,0,0,2,'2013-02-09 15:21:21'),(410,1,6,29,605,581,0,0,2,'2013-02-09 15:21:22'),(411,1,6,29,436,581,0,0,2,'2013-02-09 15:21:23'),(412,1,6,29,544,581,0,0,2,'2013-02-09 15:21:24'),(413,1,6,30,526,581,0,0,2,'2013-02-09 15:21:25'),(414,1,6,30,512,581,0,0,2,'2013-02-09 15:21:26'),(415,1,6,30,533,581,0,0,2,'2013-02-09 15:21:27'),(416,1,6,30,547,556,0,0,2,'2013-02-09 15:21:28'),(417,1,6,31,479,578,0,0,2,'2013-02-09 15:21:31'),(418,1,1,31,747,585,0,0,1,'2013-02-09 15:21:41'),(419,1,6,31,458,499,0,0,2,'2013-02-09 15:21:46'),(420,1,6,31,458,470,0,0,2,'2013-02-09 15:21:47'),(421,1,6,32,407,470,0,0,2,'2013-02-09 15:21:47'),(422,1,1,32,873,463,0,0,1,'2013-02-09 15:21:52'),(423,1,1,32,681,585,0,0,1,'2013-02-09 15:21:56'),(424,1,1,33,562,585,0,0,1,'2013-02-09 15:22:07'),(425,1,1,33,512,585,0,0,1,'2013-02-09 15:22:07'),(426,1,1,33,624,585,0,0,1,'2013-02-09 15:22:09'),(427,1,1,28,850,577,0,0,1,'2013-02-09 15:29:11'),(428,1,1,28,803,577,0,0,1,'2013-02-09 15:30:06'),(429,1,6,28,497,506,0,0,2,'2013-02-09 15:33:05'),(430,1,6,28,429,520,0,0,2,'2013-02-09 15:33:06'),(431,1,6,29,476,495,0,0,2,'2013-02-09 15:33:06'),(432,1,6,29,371,495,0,0,2,'2013-02-09 15:33:07'),(433,1,6,29,364,495,0,0,2,'2013-02-09 15:33:08'),(434,1,6,29,540,614,0,0,2,'2013-02-09 15:33:09'),(435,1,6,30,497,614,0,0,2,'2013-02-09 15:33:09'),(436,1,6,30,429,578,0,0,2,'2013-02-09 15:33:10'),(437,1,6,30,468,560,0,0,2,'2013-02-09 15:33:10'),(438,1,6,31,407,614,0,0,2,'2013-02-09 15:33:13'),(439,1,6,31,417,585,0,0,2,'2013-02-09 15:33:14'),(440,1,6,31,265,578,0,0,2,'2013-02-09 15:33:15'),(441,1,1,32,760,614,0,0,1,'2013-02-09 15:33:28'),(442,1,6,28,483,542,0,0,2,'2013-02-09 15:36:37'),(443,1,6,28,306,445,0,0,2,'2013-02-09 15:36:38'),(444,1,6,28,558,391,0,0,2,'2013-02-09 15:36:40'),(445,1,6,29,623,589,0,0,2,'2013-02-09 15:36:41'),(446,1,6,29,504,589,0,0,2,'2013-02-09 15:36:42'),(447,1,6,29,436,589,0,0,2,'2013-02-09 15:36:43'),(448,1,6,29,558,571,0,0,2,'2013-02-09 15:36:43'),(449,1,6,30,542,574,0,0,2,'2013-02-09 15:36:46'),(450,1,6,30,502,499,0,0,2,'2013-02-09 15:36:47'),(451,1,6,30,495,531,0,0,2,'2013-02-09 15:36:48'),(452,1,6,30,517,531,0,0,2,'2013-02-09 15:36:49'),(453,1,6,31,517,531,0,0,2,'2013-02-09 15:36:49'),(454,1,6,31,517,567,0,0,2,'2013-02-09 15:36:51'),(455,1,6,31,646,477,0,0,2,'2013-02-09 15:36:52'),(456,1,6,32,686,520,0,0,2,'2013-02-09 15:36:53'),(457,1,6,32,686,520,0,0,2,'2013-02-09 15:36:54'),(458,1,6,32,747,520,0,0,2,'2013-02-09 15:36:55'),(459,1,6,32,747,488,0,0,2,'2013-02-09 15:36:55'),(460,1,6,33,747,477,0,0,2,'2013-02-09 15:36:56'),(461,1,6,33,790,401,0,0,2,'2013-02-09 15:36:57'),(462,1,6,33,664,517,0,0,2,'2013-02-09 15:36:59'),(463,1,6,33,574,517,0,0,2,'2013-02-09 15:37:00'),(464,1,1,34,906,470,0,0,1,'2013-02-09 15:37:10'),(465,1,1,34,650,531,0,0,1,'2013-02-09 15:37:12'),(466,1,1,28,502,596,0,0,1,'2013-02-09 16:03:53'),(467,1,1,28,642,596,0,0,1,'2013-02-09 16:04:17'),(468,1,1,28,617,606,0,0,1,'2013-02-09 16:04:32'),(469,1,1,28,563,667,0,0,1,'2013-02-09 16:04:34'),(470,1,1,28,625,596,0,0,1,'2013-02-09 16:06:05'),(471,1,1,28,599,596,0,0,1,'2013-02-09 16:06:06'),(472,1,1,28,632,596,0,0,1,'2013-02-09 16:06:07'),(473,1,6,28,458,434,0,0,2,'2013-02-09 16:06:21'),(474,1,6,29,458,319,0,0,2,'2013-02-09 16:06:21'),(475,1,6,29,501,268,0,0,2,'2013-02-09 16:06:22'),(476,1,6,29,497,268,0,0,2,'2013-02-09 16:06:23'),(477,1,6,29,522,347,0,0,2,'2013-02-09 16:06:25'),(478,1,6,30,515,311,0,0,2,'2013-02-09 16:06:26'),(479,1,6,30,515,329,0,0,2,'2013-02-09 16:06:30'),(480,1,6,30,512,329,0,0,2,'2013-02-09 16:06:31'),(481,1,6,30,512,329,0,0,2,'2013-02-09 16:06:31'),(482,1,6,31,547,329,0,0,2,'2013-02-09 16:06:32'),(483,1,6,31,594,556,0,0,2,'2013-02-09 16:06:34'),(484,1,6,31,652,603,0,0,2,'2013-02-09 16:06:35'),(485,1,6,32,652,578,0,0,2,'2013-02-09 16:06:35'),(486,1,6,32,598,578,0,0,2,'2013-02-09 16:06:37'),(487,1,6,32,598,549,0,0,2,'2013-02-09 16:06:37'),(488,1,6,32,580,549,0,0,2,'2013-02-09 16:06:38'),(489,1,6,33,605,549,0,0,2,'2013-02-09 16:06:38'),(490,1,1,33,960,409,0,0,1,'2013-02-09 16:06:44'),(491,1,1,33,621,578,0,0,1,'2013-02-09 16:10:35'),(492,1,6,34,429,495,0,0,2,'2013-02-09 16:10:57'),(493,1,6,34,281,495,0,0,2,'2013-02-09 16:10:59'),(494,1,6,34,375,495,0,0,2,'2013-02-09 16:10:59'),(495,1,6,34,519,581,0,0,2,'2013-02-09 16:11:00'),(496,1,6,35,519,614,0,0,2,'2013-02-09 16:11:01'),(497,1,6,35,497,495,0,0,2,'2013-02-09 16:11:02'),(498,1,6,35,432,538,0,0,2,'2013-02-09 16:11:02'),(499,1,6,35,472,599,0,0,2,'2013-02-09 16:11:04'),(500,1,6,36,411,567,0,0,2,'2013-02-09 16:11:05'),(501,1,6,36,396,556,0,0,2,'2013-02-09 16:11:06'),(502,1,6,36,454,556,0,0,2,'2013-02-09 16:11:07'),(503,1,6,36,443,556,0,0,2,'2013-02-09 16:11:08'),(504,1,6,37,357,556,0,0,2,'2013-02-09 16:11:09'),(505,1,6,37,245,556,0,0,2,'2013-02-09 16:11:09'),(506,1,6,37,335,556,0,0,2,'2013-02-09 16:11:10'),(507,1,1,38,726,556,0,0,1,'2013-02-09 16:11:15'),(508,1,1,38,610,585,0,0,1,'2013-02-09 16:11:16'),(509,1,1,33,343,578,0,0,1,'2013-02-09 16:36:33'),(510,1,1,34,1620,1325,0,0,1,'2013-02-09 16:43:44'),(511,1,1,34,1620,1325,0,0,1,'2013-02-09 16:43:45'),(512,1,1,34,1620,1209,0,0,1,'2013-02-09 16:43:45'),(513,1,1,34,512,512,0,0,1,'2013-02-09 20:05:31'),(514,1,1,34,310,526,0,0,1,'2013-02-09 20:06:37'),(515,6,1,1,576,519,0,0,1,'2013-02-09 21:18:09'),(516,1,1,34,526,583,0,0,1,'2013-02-10 13:35:32'),(517,1,1,34,512,512,0,0,1,'2013-02-10 14:05:22'),(518,1,1,35,562,512,0,0,1,'2013-02-10 14:05:22'),(519,1,1,35,943,145,0,0,1,'2013-02-10 14:05:34'),(520,1,1,34,512,512,0,0,1,'2013-02-10 14:06:38'),(521,1,1,35,713,512,0,0,1,'2013-02-10 14:06:39'),(522,1,6,34,529,466,0,0,2,'2013-02-10 14:18:50'),(523,1,6,35,748,527,0,0,2,'2013-02-10 14:18:56'),(524,1,6,35,726,462,0,0,2,'2013-02-10 14:19:08'),(525,1,6,35,431,528,0,0,2,'2013-02-10 14:20:37'),(526,1,6,35,431,528,0,0,2,'2013-02-10 14:20:37'),(527,1,6,36,431,514,0,0,2,'2013-02-10 14:20:43'),(528,1,6,36,431,435,0,0,2,'2013-02-10 14:20:44'),(529,1,6,36,522,545,0,0,2,'2013-02-10 14:21:07'),(530,1,6,36,522,545,0,0,2,'2013-02-10 14:21:08'),(531,1,6,37,562,545,0,0,2,'2013-02-10 14:21:08'),(532,1,6,37,562,517,0,0,2,'2013-02-10 14:21:09'),(533,1,6,37,583,484,0,0,2,'2013-02-10 14:21:11'),(534,1,6,38,583,459,0,0,2,'2013-02-10 14:21:11'),(535,1,6,38,583,459,0,0,2,'2013-02-10 14:21:12'),(536,1,6,38,583,491,0,0,2,'2013-02-10 14:21:12'),(537,1,6,38,562,535,0,0,2,'2013-02-10 14:21:14'),(538,1,1,34,512,512,0,0,1,'2013-02-10 14:24:04'),(539,1,1,35,789,512,0,0,1,'2013-02-10 14:26:09'),(540,1,1,35,677,512,0,0,1,'2013-02-10 14:26:10'),(541,1,1,35,512,512,0,0,1,'2013-02-10 14:28:21'),(542,1,1,35,512,512,0,0,1,'2013-02-10 14:28:21'),(543,1,1,35,526,457,0,0,1,'2013-02-10 14:28:23'),(544,1,1,35,626,573,0,0,1,'2013-02-10 14:28:34'),(545,1,6,36,417,592,0,0,2,'2013-02-10 14:29:14'),(546,1,6,36,489,477,0,0,2,'2013-02-10 14:29:15'),(547,1,6,36,435,589,0,0,2,'2013-02-10 14:29:16'),(548,1,6,36,417,535,0,0,2,'2013-02-10 14:29:19'),(549,1,6,37,392,481,0,0,2,'2013-02-10 14:29:20'),(550,1,6,37,320,481,0,0,2,'2013-02-10 14:29:21'),(551,1,6,37,417,481,0,0,2,'2013-02-10 14:29:22'),(552,1,6,37,432,481,0,0,2,'2013-02-10 14:29:24'),(553,1,6,38,432,481,0,0,2,'2013-02-10 14:29:24'),(554,1,6,38,374,481,0,0,2,'2013-02-10 14:29:25'),(555,1,6,38,396,420,0,0,2,'2013-02-10 14:29:26'),(556,1,6,38,396,237,0,0,2,'2013-02-10 14:29:27'),(557,1,1,39,515,681,0,0,1,'2013-02-10 14:29:35'),(558,1,1,39,637,879,0,0,1,'2013-02-10 14:29:38'),(559,1,1,39,521,390,0,0,1,'2013-02-10 14:30:09'),(560,1,1,39,715,548,0,0,1,'2013-02-10 14:30:10'),(561,1,1,39,730,602,0,0,1,'2013-02-10 14:30:24'),(562,1,1,40,730,602,0,0,1,'2013-02-10 14:30:50'),(563,1,1,40,648,512,0,0,1,'2013-02-10 14:30:51'),(564,1,1,40,256,612,0,0,1,'2013-02-10 14:31:59'),(565,1,6,40,519,434,0,0,2,'2013-02-10 14:35:52'),(566,1,6,40,483,551,0,0,2,'2013-02-10 14:35:55'),(567,1,6,41,605,511,0,0,2,'2013-02-10 14:35:55'),(568,1,1,34,281,519,0,0,1,'2013-02-10 14:41:31'),(569,1,1,35,324,519,0,0,1,'2013-02-10 14:41:32'),(570,1,1,35,454,562,0,0,1,'2013-02-10 14:41:45'),(571,1,1,35,576,562,0,0,1,'2013-02-10 14:41:46'),(572,1,1,34,450,659,0,0,1,'2013-02-10 14:45:37'),(573,1,6,35,360,351,0,0,2,'2013-02-10 14:45:57'),(574,1,6,35,360,351,0,0,2,'2013-02-10 14:45:57'),(575,1,6,35,587,488,0,0,2,'2013-02-10 14:45:59'),(576,1,1,35,634,540,0,0,1,'2013-02-10 14:46:24'),(577,1,1,35,529,540,0,0,1,'2013-02-10 14:46:25'),(578,1,1,36,742,595,0,0,1,'2013-02-10 14:46:39'),(579,1,1,36,673,465,0,0,1,'2013-02-10 14:47:13'),(580,1,1,34,565,555,0,0,1,'2013-02-10 15:50:43'),(581,1,1,35,753,805,0,0,1,'2013-02-10 15:51:00'),(582,1,1,35,397,1374,0,0,1,'2013-02-10 15:51:05'),(583,1,1,35,498,1374,0,0,1,'2013-02-10 15:51:06'),(584,1,1,34,1001,495,0,0,1,'2013-02-10 15:52:38'),(585,1,6,34,537,405,0,0,2,'2013-02-10 15:55:07'),(586,1,6,35,476,560,0,0,2,'2013-02-10 15:55:13'),(587,1,6,35,461,578,0,0,2,'2013-02-10 15:55:14'),(588,1,6,35,461,553,0,0,2,'2013-02-10 15:55:16'),(589,1,6,35,501,553,0,0,2,'2013-02-10 15:55:17'),(590,1,6,36,349,499,0,0,2,'2013-02-10 15:55:18'),(591,1,6,36,191,517,0,0,2,'2013-02-10 15:55:20'),(592,1,6,36,321,491,0,0,2,'2013-02-10 15:55:22'),(593,1,6,36,349,470,0,0,2,'2013-02-10 15:55:23'),(594,1,6,37,360,470,0,0,2,'2013-02-10 15:55:23'),(595,1,6,37,385,419,0,0,2,'2013-02-10 15:55:25'),(596,1,6,37,439,448,0,0,2,'2013-02-10 15:55:26'),(597,1,1,38,490,512,0,0,1,'2013-02-10 15:55:34'),(598,1,1,38,573,619,0,0,1,'2013-02-10 15:55:37'),(599,1,1,38,972,627,0,0,1,'2013-02-10 15:55:49'),(600,1,1,38,703,1408,0,0,1,'2013-02-10 15:55:54'),(601,1,1,38,491,1527,0,0,1,'2013-02-10 15:55:55'),(602,1,1,38,1387,717,0,0,1,'2013-02-10 15:56:06'),(603,1,1,39,1538,623,0,0,1,'2013-02-10 15:56:07'),(604,1,1,39,1159,785,0,0,1,'2013-02-10 15:56:13');

/*Table structure for table `world_stats_user_level` */

DROP TABLE IF EXISTS `world_stats_user_level`;

CREATE TABLE `world_stats_user_level` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `character_id` int(11) NOT NULL COMMENT 'The ID of the character that leveled up.',
  `map_id` smallint(5) unsigned DEFAULT NULL COMMENT 'The ID of the map this event took place on.',
  `x` smallint(5) unsigned NOT NULL COMMENT 'The map x coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `y` smallint(5) unsigned NOT NULL COMMENT 'The map y coordinate of the user when this event took place. Only valid when the map_id is not null.',
  `level` smallint(6) NOT NULL COMMENT 'The level that the character leveled up to (their new level).',
  `when` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'When this event took place.',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=177 DEFAULT CHARSET=latin1 COMMENT='Event log: User levels up.';

/*Data for the table `world_stats_user_level` */

insert  into `world_stats_user_level`(`id`,`character_id`,`map_id`,`x`,`y`,`level`,`when`) values (1,1,1,489,1226,2,'2013-02-04 17:52:00'),(2,1,1,347,545,3,'2013-02-04 17:53:00'),(3,1,1,518,625,4,'2013-02-04 18:40:30'),(4,1,1,106,656,5,'2013-02-04 18:56:30'),(5,1,1,285,481,5,'2013-02-04 19:02:44'),(6,1,1,285,703,5,'2013-02-04 19:05:01'),(7,1,1,248,591,5,'2013-02-04 20:10:01'),(8,1,1,547,463,6,'2013-02-04 20:21:47'),(9,1,1,449,227,7,'2013-02-04 20:23:02'),(10,1,1,587,490,8,'2013-02-04 21:13:23'),(11,1,1,738,605,9,'2013-02-04 21:13:57'),(12,1,1,630,1073,10,'2013-02-04 21:14:42'),(13,1,1,400,598,8,'2013-02-04 21:29:21'),(14,1,1,436,893,8,'2013-02-04 23:11:07'),(15,1,1,192,1099,9,'2013-02-04 23:11:45'),(16,1,1,679,752,10,'2013-02-04 23:20:47'),(17,1,1,1242,726,11,'2013-02-04 23:22:30'),(18,1,1,414,818,8,'2013-02-04 23:23:50'),(19,1,2,468,290,9,'2013-02-04 23:25:09'),(20,1,1,514,359,10,'2013-02-04 23:26:32'),(21,1,1,1235,743,11,'2013-02-04 23:27:35'),(22,1,1,373,485,12,'2013-02-04 23:32:31'),(23,1,1,1472,705,13,'2013-02-04 23:33:46'),(24,1,2,413,438,12,'2013-02-04 23:48:37'),(25,1,2,604,611,13,'2013-02-04 23:48:54'),(26,1,2,596,611,14,'2013-02-04 23:49:04'),(27,1,1,249,1074,15,'2013-02-04 23:49:43'),(28,1,1,824,666,16,'2013-02-04 23:50:16'),(29,1,1,960,517,17,'2013-02-05 23:29:38'),(30,1,1,586,672,18,'2013-02-05 23:34:32'),(31,1,1,417,560,19,'2013-02-05 23:38:08'),(32,1,1,1004,917,20,'2013-02-05 23:38:23'),(33,1,1,1069,946,21,'2013-02-05 23:38:39'),(34,1,1,1584,294,22,'2013-02-06 00:08:26'),(35,1,3,262,255,22,'2013-02-06 00:47:55'),(36,1,3,262,255,22,'2013-02-06 00:48:10'),(37,1,1,1505,416,23,'2013-02-07 00:28:03'),(38,1,1,1630,419,23,'2013-02-07 00:29:29'),(39,1,1,1505,970,23,'2013-02-07 00:35:57'),(40,1,1,799,811,24,'2013-02-07 00:37:32'),(41,1,1,965,764,25,'2013-02-07 00:38:37'),(42,1,1,149,1410,26,'2013-02-07 00:39:36'),(43,1,1,1533,739,27,'2013-02-07 00:40:00'),(44,1,2,323,211,28,'2013-02-07 00:40:45'),(45,1,2,392,471,29,'2013-02-07 00:41:00'),(46,1,1,1199,317,30,'2013-02-07 00:45:05'),(47,1,2,403,427,31,'2013-02-07 00:45:25'),(48,1,2,414,545,32,'2013-02-07 00:45:55'),(49,1,2,555,545,33,'2013-02-07 00:46:00'),(50,1,2,555,545,34,'2013-02-07 00:46:04'),(51,1,2,512,571,35,'2013-02-07 00:46:14'),(52,1,2,814,499,36,'2013-02-07 00:46:20'),(53,1,2,681,542,37,'2013-02-07 00:46:24'),(54,1,2,688,283,38,'2013-02-07 00:46:28'),(55,1,2,591,250,39,'2013-02-07 00:46:34'),(56,1,2,263,236,40,'2013-02-07 00:46:38'),(57,1,1,630,621,41,'2013-02-07 00:47:06'),(58,1,1,1641,916,23,'2013-02-08 00:05:19'),(59,1,1,659,642,23,'2013-02-08 00:08:10'),(60,1,1,950,826,23,'2013-02-08 00:13:18'),(61,1,2,439,387,24,'2013-02-08 00:14:18'),(62,1,2,468,358,25,'2013-02-08 00:14:21'),(63,1,2,457,369,26,'2013-02-08 00:14:23'),(64,1,1,423,700,27,'2013-02-08 00:14:41'),(65,1,1,666,458,23,'2013-02-08 00:18:29'),(66,1,1,1037,743,23,'2013-02-08 00:25:16'),(67,1,1,914,714,23,'2013-02-08 00:26:52'),(68,1,1,1505,606,23,'2013-02-08 00:28:02'),(69,1,1,1382,729,23,'2013-02-08 00:29:27'),(70,1,1,583,473,23,'2013-02-08 00:30:38'),(71,1,1,1274,707,23,'2013-02-08 00:32:16'),(72,1,1,896,686,23,'2013-02-08 00:56:44'),(73,1,1,713,725,23,'2013-02-08 01:01:44'),(74,1,1,285,965,24,'2013-02-08 10:39:15'),(75,1,1,761,1756,25,'2013-02-08 10:42:08'),(76,1,1,626,572,24,'2013-02-09 15:02:52'),(77,1,1,626,572,25,'2013-02-09 15:02:52'),(78,1,1,626,572,26,'2013-02-09 15:02:52'),(79,1,1,626,572,27,'2013-02-09 15:02:52'),(80,1,1,626,572,28,'2013-02-09 15:02:52'),(81,1,1,626,572,29,'2013-02-09 15:02:52'),(82,1,1,626,572,30,'2013-02-09 15:02:52'),(83,1,1,626,572,31,'2013-02-09 15:02:52'),(84,1,1,626,572,32,'2013-02-09 15:02:52'),(85,1,1,626,572,33,'2013-02-09 15:02:52'),(86,1,1,626,572,34,'2013-02-09 15:02:52'),(87,1,1,626,572,35,'2013-02-09 15:02:52'),(88,1,1,626,572,36,'2013-02-09 15:02:52'),(89,1,1,626,572,37,'2013-02-09 15:02:52'),(90,1,1,626,572,38,'2013-02-09 15:02:52'),(91,1,1,626,572,39,'2013-02-09 15:02:52'),(92,1,1,626,572,40,'2013-02-09 15:02:52'),(93,1,1,626,572,41,'2013-02-09 15:02:52'),(94,1,1,626,572,42,'2013-02-09 15:02:52'),(95,1,1,626,572,43,'2013-02-09 15:02:52'),(96,1,1,626,572,44,'2013-02-09 15:02:52'),(97,1,1,626,572,45,'2013-02-09 15:02:52'),(98,1,1,626,572,46,'2013-02-09 15:02:52'),(99,1,1,626,572,47,'2013-02-09 15:02:52'),(100,1,1,626,572,48,'2013-02-09 15:02:52'),(101,1,1,626,572,49,'2013-02-09 15:02:52'),(102,1,1,626,572,50,'2013-02-09 15:02:52'),(103,1,1,626,572,51,'2013-02-09 15:02:52'),(104,1,1,626,572,52,'2013-02-09 15:02:52'),(105,1,1,626,572,53,'2013-02-09 15:02:52'),(106,1,1,626,572,54,'2013-02-09 15:02:52'),(107,1,1,626,572,55,'2013-02-09 15:02:52'),(108,1,1,626,572,56,'2013-02-09 15:02:52'),(109,1,1,626,572,57,'2013-02-09 15:02:52'),(110,1,2,547,553,24,'2013-02-09 15:07:20'),(111,1,2,512,553,25,'2013-02-09 15:07:28'),(112,1,2,645,599,26,'2013-02-09 15:07:38'),(113,1,1,628,578,27,'2013-02-09 15:07:52'),(114,1,2,497,437,24,'2013-02-09 15:09:57'),(115,1,2,497,437,25,'2013-02-09 15:10:01'),(116,1,1,753,643,26,'2013-02-09 15:10:36'),(117,1,2,339,592,27,'2013-02-09 15:10:43'),(118,1,2,238,506,28,'2013-02-09 15:10:47'),(119,1,2,353,263,24,'2013-02-09 15:18:31'),(120,1,2,475,263,25,'2013-02-09 15:18:33'),(121,1,2,565,535,26,'2013-02-09 15:18:51'),(122,1,2,616,531,27,'2013-02-09 15:18:59'),(123,1,1,625,596,28,'2013-02-09 15:19:16'),(124,1,2,634,610,29,'2013-02-09 15:21:21'),(125,1,2,544,581,30,'2013-02-09 15:21:24'),(126,1,2,547,556,31,'2013-02-09 15:21:28'),(127,1,2,458,470,32,'2013-02-09 15:21:47'),(128,1,1,562,585,33,'2013-02-09 15:22:00'),(129,1,2,429,520,29,'2013-02-09 15:33:06'),(130,1,2,540,614,30,'2013-02-09 15:33:09'),(131,1,2,468,560,31,'2013-02-09 15:33:10'),(132,1,1,655,614,32,'2013-02-09 15:33:25'),(133,1,2,558,391,29,'2013-02-09 15:36:40'),(134,1,2,558,571,30,'2013-02-09 15:36:43'),(135,1,2,517,531,31,'2013-02-09 15:36:49'),(136,1,2,646,477,32,'2013-02-09 15:36:52'),(137,1,2,747,488,33,'2013-02-09 15:36:55'),(138,1,2,574,517,34,'2013-02-09 15:37:00'),(139,1,1,563,667,29,'2013-02-09 16:04:34'),(140,1,2,458,434,29,'2013-02-09 16:06:21'),(141,1,2,522,347,30,'2013-02-09 16:06:25'),(142,1,2,512,329,31,'2013-02-09 16:06:31'),(143,1,2,652,603,32,'2013-02-09 16:06:35'),(144,1,2,580,549,33,'2013-02-09 16:06:38'),(145,1,1,621,578,34,'2013-02-09 16:10:35'),(146,1,2,519,581,35,'2013-02-09 16:11:00'),(147,1,2,472,599,36,'2013-02-09 16:11:04'),(148,1,2,443,556,37,'2013-02-09 16:11:08'),(149,1,2,335,556,38,'2013-02-09 16:11:10'),(150,1,1,343,578,34,'2013-02-09 16:36:33'),(151,1,1,526,583,35,'2013-02-10 13:35:32'),(152,1,1,512,512,35,'2013-02-10 14:05:22'),(153,1,1,512,512,35,'2013-02-10 14:06:38'),(154,1,2,529,466,35,'2013-02-10 14:18:50'),(155,1,2,431,528,36,'2013-02-10 14:20:37'),(156,1,2,522,545,37,'2013-02-10 14:21:08'),(157,1,2,583,484,38,'2013-02-10 14:21:11'),(158,1,2,562,535,39,'2013-02-10 14:21:14'),(159,1,1,512,512,35,'2013-02-10 14:24:04'),(160,1,1,626,573,36,'2013-02-10 14:28:34'),(161,1,2,417,535,37,'2013-02-10 14:29:19'),(162,1,2,432,481,38,'2013-02-10 14:29:24'),(163,1,2,396,237,39,'2013-02-10 14:29:27'),(164,1,1,730,602,40,'2013-02-10 14:30:24'),(165,1,2,483,551,41,'2013-02-10 14:35:55'),(166,1,4,657,434,35,'2013-02-10 14:37:26'),(167,1,1,281,519,35,'2013-02-10 14:41:31'),(168,1,1,450,659,35,'2013-02-10 14:45:37'),(169,1,1,529,540,36,'2013-02-10 14:46:25'),(170,1,1,565,555,35,'2013-02-10 15:50:43'),(171,1,1,1001,495,35,'2013-02-10 15:52:38'),(172,1,2,537,405,35,'2013-02-10 15:55:07'),(173,1,2,501,553,36,'2013-02-10 15:55:17'),(174,1,2,349,470,37,'2013-02-10 15:55:23'),(175,1,2,439,448,38,'2013-02-10 15:55:26'),(176,1,1,1387,717,39,'2013-02-10 15:56:06');

/*Table structure for table `world_stats_user_shopping` */

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

/*Data for the table `world_stats_user_shopping` */

/* Trigger structure for table `account_ban` */

DELIMITER $$

/*!50003 DROP TRIGGER*//*!50032 IF EXISTS */ /*!50003 `bi_account_ban_fer` */$$

/*!50003 CREATE */ /*!50017 DEFINER = 'root'@'localhost' */ /*!50003 TRIGGER `bi_account_ban_fer` BEFORE INSERT ON `account_ban` FOR EACH ROW BEGIN




	IF new.end_time <= NOW() THEN




		SET new.expired = 1;




	ELSE




		SET new.expired = 0;




	END IF;




END */$$


DELIMITER ;

/* Trigger structure for table `account_ban` */

DELIMITER $$

/*!50003 DROP TRIGGER*//*!50032 IF EXISTS */ /*!50003 `bu_account_ban_fer` */$$

/*!50003 CREATE */ /*!50017 DEFINER = 'root'@'localhost' */ /*!50003 TRIGGER `bu_account_ban_fer` BEFORE UPDATE ON `account_ban` FOR EACH ROW BEGIN




	IF new.end_time <= NOW() THEN




		SET new.expired = 1;




	ELSE




		SET new.expired = 0;




	END IF;




END */$$


DELIMITER ;

/* Function  structure for function  `create_user_on_account` */

/*!50003 DROP FUNCTION IF EXISTS `create_user_on_account` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `create_user_on_account`(accountName VARCHAR(50), characterName VARCHAR(30)) RETURNS varchar(100) CHARSET latin1
BEGIN				DECLARE character_count INT DEFAULT 0;		DECLARE max_character_count INT DEFAULT 9;		DECLARE is_name_free INT DEFAULT 0;		DECLARE errorMsg VARCHAR(100) DEFAULT "";		DECLARE accountID INT DEFAULT NULL;		DECLARE charID INT DEFAULT 0;		SELECT `id` INTO accountID FROM `account` WHERE `name` = accountName;		IF ISNULL(accountID) THEN			SET errorMsg = "Account with the specified name does not exist.";		ELSE			SELECT COUNT(*) INTO character_count FROM `account_character` WHERE `account_id` = accountID AND ISNULL(time_deleted);			IF character_count > max_character_count THEN				SET errorMsg = "No free character slots available in the account.";			ELSE				SELECT COUNT(*) INTO is_name_free FROM `character` WHERE `name` = characterName LIMIT 1;				IF is_name_free > 0 THEN					SET errorMsg = "The specified character name is not available for use.";				ELSE					INSERT INTO `character` SET `name`	= characterName;					SET charID = LAST_INSERT_ID();					INSERT INTO `account_character` SET `character_id` = charID, `account_id` = accountID;				END IF;			END IF;		END IF;						RETURN errorMsg;  END */$$
DELIMITER ;

/* Function  structure for function  `ft_banning_isbanned` */

/*!50003 DROP FUNCTION IF EXISTS `ft_banning_isbanned` */;
DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` FUNCTION `ft_banning_isbanned`(accountID INT) RETURNS int(11)
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
END */$$
DELIMITER ;

/* Procedure structure for procedure `delete_user_on_account` */

/*!50003 DROP PROCEDURE IF EXISTS  `delete_user_on_account` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `delete_user_on_account`(characterID INT)
BEGIN
	UPDATE `account_character`
		SET `time_deleted` = NOW()
		WHERE `character_id` = characterID
		AND `time_deleted` IS NULL;
	UPDATE `character`
		SET `name` = CONCAT('~',`id`,'_',name)
		WHERE `id` = characterID
			AND SUBSTR(`name`, 1) != '~';
END */$$
DELIMITER ;

/* Procedure structure for procedure `find_foreign_keys` */

/*!50003 DROP PROCEDURE IF EXISTS  `find_foreign_keys` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `find_foreign_keys`(tableSchema VARCHAR(100), tableName VARCHAR(100), columnName VARCHAR(100))
BEGIN
		SELECT `TABLE_SCHEMA`, `TABLE_NAME`, `COLUMN_NAME`
			FROM information_schema.KEY_COLUMN_USAGE
			WHERE `REFERENCED_TABLE_SCHEMA` = tableSchema
				AND `REFERENCED_TABLE_NAME` = tableName
				AND `REFERENCED_COLUMN_NAME` = columnName;
END */$$
DELIMITER ;

/* Procedure structure for procedure `ft_banning_get_reasons` */

/*!50003 DROP PROCEDURE IF EXISTS  `ft_banning_get_reasons` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ft_banning_get_reasons`(accountID INT)
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
		
END */$$
DELIMITER ;

/* Procedure structure for procedure `ft_banning_update_expired` */

/*!50003 DROP PROCEDURE IF EXISTS  `ft_banning_update_expired` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `ft_banning_update_expired`()
BEGIN
		DECLARE tnow TIMESTAMP;
		
		SET tnow = NOW();
		
		UPDATE `account_ban`
			SET `expired` = 1
			WHERE `expired` = 0
				AND `end_time` <= tnow;
END */$$
DELIMITER ;

/* Procedure structure for procedure `rebuild_views` */

/*!50003 DROP PROCEDURE IF EXISTS  `rebuild_views` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `rebuild_views`()
BEGIN
	
	CALL rebuild_view_npc_character();
	CALL rebuild_view_user_character();
    
END */$$
DELIMITER ;

/* Procedure structure for procedure `rebuild_view_npc_character` */

/*!50003 DROP PROCEDURE IF EXISTS  `rebuild_view_npc_character` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `rebuild_view_npc_character`()
BEGIN
	
	DROP VIEW IF EXISTS `view_npc_character`;
	CREATE ALGORITHM=UNDEFINED DEFINER=CURRENT_USER SQL SECURITY DEFINER VIEW `view_npc_character` AS SELECT c.*  FROM `character` c LEFT JOIN `account_character` a ON c.id = a.character_id WHERE a.account_id IS NULL;
    
END */$$
DELIMITER ;

/* Procedure structure for procedure `rebuild_view_user_character` */

/*!50003 DROP PROCEDURE IF EXISTS  `rebuild_view_user_character` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `rebuild_view_user_character`()
BEGIN
	
	DROP VIEW IF EXISTS `view_user_character`;
	CREATE ALGORITHM=UNDEFINED DEFINER=CURRENT_USER SQL SECURITY DEFINER VIEW `view_user_character` AS SELECT c.* FROM `character` c INNER JOIN `account_character` a ON c.id = a.character_id WHERE a.time_deleted IS NULL;
    
END */$$
DELIMITER ;

/*Table structure for table `view_npc_character` */

DROP TABLE IF EXISTS `view_npc_character`;

/*!50001 DROP VIEW IF EXISTS `view_npc_character` */;
/*!50001 DROP TABLE IF EXISTS `view_npc_character` */;

/*!50001 CREATE TABLE  `view_npc_character`(
 `id` int(11) ,
 `character_template_id` smallint(5) unsigned ,
 `name` varchar(60) ,
 `shop_id` smallint(5) unsigned ,
 `chat_dialog` smallint(5) unsigned ,
 `ai_id` smallint(5) unsigned ,
 `load_map_id` smallint(5) unsigned ,
 `load_x` smallint(5) unsigned ,
 `load_y` smallint(5) unsigned ,
 `respawn_map_id` smallint(5) unsigned ,
 `respawn_x` float ,
 `respawn_y` float ,
 `body_id` smallint(5) unsigned ,
 `move_speed` smallint(5) unsigned ,
 `cash` int(11) ,
 `level` smallint(6) ,
 `exp` int(11) ,
 `statpoints` int(11) ,
 `hp` smallint(6) ,
 `mp` smallint(6) ,
 `stat_maxhp` smallint(6) ,
 `stat_maxmp` smallint(6) ,
 `stat_minhit` smallint(6) ,
 `stat_maxhit` smallint(6) ,
 `stat_defence` smallint(6) ,
 `stat_agi` smallint(6) ,
 `stat_int` smallint(6) ,
 `stat_str` smallint(6) 
)*/;

/*Table structure for table `view_user_character` */

DROP TABLE IF EXISTS `view_user_character`;

/*!50001 DROP VIEW IF EXISTS `view_user_character` */;
/*!50001 DROP TABLE IF EXISTS `view_user_character` */;

/*!50001 CREATE TABLE  `view_user_character`(
 `id` int(11) ,
 `character_template_id` smallint(5) unsigned ,
 `name` varchar(60) ,
 `shop_id` smallint(5) unsigned ,
 `chat_dialog` smallint(5) unsigned ,
 `ai_id` smallint(5) unsigned ,
 `load_map_id` smallint(5) unsigned ,
 `load_x` smallint(5) unsigned ,
 `load_y` smallint(5) unsigned ,
 `respawn_map_id` smallint(5) unsigned ,
 `respawn_x` float ,
 `respawn_y` float ,
 `body_id` smallint(5) unsigned ,
 `move_speed` smallint(5) unsigned ,
 `cash` int(11) ,
 `level` smallint(6) ,
 `exp` int(11) ,
 `statpoints` int(11) ,
 `hp` smallint(6) ,
 `mp` smallint(6) ,
 `stat_maxhp` smallint(6) ,
 `stat_maxmp` smallint(6) ,
 `stat_minhit` smallint(6) ,
 `stat_maxhit` smallint(6) ,
 `stat_defence` smallint(6) ,
 `stat_agi` smallint(6) ,
 `stat_int` smallint(6) ,
 `stat_str` smallint(6) 
)*/;

/*View structure for view view_npc_character */

/*!50001 DROP TABLE IF EXISTS `view_npc_character` */;
/*!50001 DROP VIEW IF EXISTS `view_npc_character` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_npc_character` AS select `c`.`id` AS `id`,`c`.`character_template_id` AS `character_template_id`,`c`.`name` AS `name`,`c`.`shop_id` AS `shop_id`,`c`.`chat_dialog` AS `chat_dialog`,`c`.`ai_id` AS `ai_id`,`c`.`load_map_id` AS `load_map_id`,`c`.`load_x` AS `load_x`,`c`.`load_y` AS `load_y`,`c`.`respawn_map_id` AS `respawn_map_id`,`c`.`respawn_x` AS `respawn_x`,`c`.`respawn_y` AS `respawn_y`,`c`.`body_id` AS `body_id`,`c`.`move_speed` AS `move_speed`,`c`.`cash` AS `cash`,`c`.`level` AS `level`,`c`.`exp` AS `exp`,`c`.`statpoints` AS `statpoints`,`c`.`hp` AS `hp`,`c`.`mp` AS `mp`,`c`.`stat_maxhp` AS `stat_maxhp`,`c`.`stat_maxmp` AS `stat_maxmp`,`c`.`stat_minhit` AS `stat_minhit`,`c`.`stat_maxhit` AS `stat_maxhit`,`c`.`stat_defence` AS `stat_defence`,`c`.`stat_agi` AS `stat_agi`,`c`.`stat_int` AS `stat_int`,`c`.`stat_str` AS `stat_str` from (`character` `c` left join `account_character` `a` on((`c`.`id` = `a`.`character_id`))) where isnull(`a`.`account_id`) */;

/*View structure for view view_user_character */

/*!50001 DROP TABLE IF EXISTS `view_user_character` */;
/*!50001 DROP VIEW IF EXISTS `view_user_character` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_user_character` AS select `c`.`id` AS `id`,`c`.`character_template_id` AS `character_template_id`,`c`.`name` AS `name`,`c`.`shop_id` AS `shop_id`,`c`.`chat_dialog` AS `chat_dialog`,`c`.`ai_id` AS `ai_id`,`c`.`load_map_id` AS `load_map_id`,`c`.`load_x` AS `load_x`,`c`.`load_y` AS `load_y`,`c`.`respawn_map_id` AS `respawn_map_id`,`c`.`respawn_x` AS `respawn_x`,`c`.`respawn_y` AS `respawn_y`,`c`.`body_id` AS `body_id`,`c`.`move_speed` AS `move_speed`,`c`.`cash` AS `cash`,`c`.`level` AS `level`,`c`.`exp` AS `exp`,`c`.`statpoints` AS `statpoints`,`c`.`hp` AS `hp`,`c`.`mp` AS `mp`,`c`.`stat_maxhp` AS `stat_maxhp`,`c`.`stat_maxmp` AS `stat_maxmp`,`c`.`stat_minhit` AS `stat_minhit`,`c`.`stat_maxhit` AS `stat_maxhit`,`c`.`stat_defence` AS `stat_defence`,`c`.`stat_agi` AS `stat_agi`,`c`.`stat_int` AS `stat_int`,`c`.`stat_str` AS `stat_str` from (`character` `c` join `account_character` `a` on((`c`.`id` = `a`.`character_id`))) where isnull(`a`.`time_deleted`) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
