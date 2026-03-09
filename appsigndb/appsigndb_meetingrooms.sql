CREATE DATABASE  IF NOT EXISTS `appsigndb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `appsigndb`;
-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: appsigndb
-- ------------------------------------------------------
-- Server version	8.0.36

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `meetingrooms`
--

DROP TABLE IF EXISTS `meetingrooms`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `meetingrooms` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RoomCode` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Visibility` int NOT NULL,
  `Status` int NOT NULL,
  `HostUserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `OrganizationId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreatedAtUtc` datetime(6) NOT NULL,
  `EndedAtUtc` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_MeetingRooms_RoomCode` (`RoomCode`),
  KEY `IX_MeetingRooms_HostUserId` (`HostUserId`),
  KEY `IX_MeetingRooms_OrganizationId` (`OrganizationId`),
  CONSTRAINT `FK_MeetingRooms_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `organizations` (`Id`),
  CONSTRAINT `FK_MeetingRooms_UserAccounts_HostUserId` FOREIGN KEY (`HostUserId`) REFERENCES `useraccounts` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meetingrooms`
--

LOCK TABLES `meetingrooms` WRITE;
/*!40000 ALTER TABLE `meetingrooms` DISABLE KEYS */;
INSERT INTO `meetingrooms` VALUES ('0cbf345d-9f75-4154-b872-c49a323ab945','uhq4seb',NULL,1,2,'adb61522-5558-41cd-8f21-a18adf3e68c5',NULL,'2026-03-07 18:50:23.633869','2026-03-07 18:59:44.386317'),('120cc157-b8ca-4658-a9a1-97640fea6f1a','mjtz0gf',NULL,1,2,'adb61522-5558-41cd-8f21-a18adf3e68c5',NULL,'2026-03-07 19:08:59.843325','2026-03-07 19:19:08.302874'),('20e13d7b-b303-45fd-919a-c71eca542589','iywxw5a',NULL,1,2,'adb61522-5558-41cd-8f21-a18adf3e68c5',NULL,'2026-03-07 15:42:02.870717','2026-03-07 15:46:45.384549'),('83b40a16-4c70-4780-b070-c925984613d9','8bhhpb5',NULL,1,2,'adb61522-5558-41cd-8f21-a18adf3e68c5',NULL,'2026-03-08 06:11:07.397807','2026-03-08 06:11:08.669405'),('906dd6cd-45e9-4bec-9573-0b17721ba936','2hpxs4z',NULL,1,2,'adb61522-5558-41cd-8f21-a18adf3e68c5',NULL,'2026-03-07 15:54:09.630991','2026-03-07 16:35:27.069953'),('b8010ce1-366f-401c-8dab-592da6aa6008','5glgh1d',NULL,1,2,'adb61522-5558-41cd-8f21-a18adf3e68c5',NULL,'2026-03-08 06:47:48.681898','2026-03-08 06:47:53.062524'),('baf109dd-5b0d-4219-bfb9-5e034f8859d9','oukmp1j',NULL,1,2,'adb61522-5558-41cd-8f21-a18adf3e68c5',NULL,'2026-03-07 19:22:30.762553','2026-03-07 19:27:29.479611'),('e79c755a-2134-400e-80c8-84ac4e4c5270','79c2nie',NULL,1,2,'adb61522-5558-41cd-8f21-a18adf3e68c5',NULL,'2026-03-07 18:07:27.019801','2026-03-07 18:09:37.469336');
/*!40000 ALTER TABLE `meetingrooms` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-09 20:07:00
