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
-- Table structure for table `activitylogs`
--

DROP TABLE IF EXISTS `activitylogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activitylogs` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `MeetingRoomId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `UserAccountId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `ActivityType` int NOT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `MetadataJson` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreatedAtUtc` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ActivityLogs_CreatedAtUtc` (`CreatedAtUtc`),
  KEY `IX_ActivityLogs_MeetingRoomId` (`MeetingRoomId`),
  KEY `IX_ActivityLogs_UserAccountId` (`UserAccountId`),
  CONSTRAINT `FK_ActivityLogs_MeetingRooms_MeetingRoomId` FOREIGN KEY (`MeetingRoomId`) REFERENCES `meetingrooms` (`Id`),
  CONSTRAINT `FK_ActivityLogs_UserAccounts_UserAccountId` FOREIGN KEY (`UserAccountId`) REFERENCES `useraccounts` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activitylogs`
--

LOCK TABLES `activitylogs` WRITE;
/*!40000 ALTER TABLE `activitylogs` DISABLE KEYS */;
INSERT INTO `activitylogs` VALUES ('3bd8c459-b342-4db4-bf4c-fe3a3455561b','baf109dd-5b0d-4219-bfb9-5e034f8859d9','f3c2143a-8c38-4751-a99a-151fbf46d522',6,'Connection pJrW_GuQHNoiEGyBMVu6aw left room oukmp1j',NULL,'2026-03-07 19:27:29.490720'),('4d4b267a-2d53-4ce5-b0af-f4c29be66933','baf109dd-5b0d-4219-bfb9-5e034f8859d9','adb61522-5558-41cd-8f21-a18adf3e68c5',5,'Bharat Harkal joined room oukmp1j',NULL,'2026-03-07 19:22:30.881994'),('59489802-ec0a-4a9e-8e91-9780fdb4a634','baf109dd-5b0d-4219-bfb9-5e034f8859d9','adb61522-5558-41cd-8f21-a18adf3e68c5',2,'Join request by Bharat Harkal',NULL,'2026-03-07 19:22:30.854342'),('70b50fd6-3537-4370-a347-de5a7420cd75','baf109dd-5b0d-4219-bfb9-5e034f8859d9','f3c2143a-8c38-4751-a99a-151fbf46d522',5,'om joined room oukmp1j',NULL,'2026-03-07 19:23:06.401169'),('8da8f29e-9dd2-4e83-8220-5f51b3867809','83b40a16-4c70-4780-b070-c925984613d9','adb61522-5558-41cd-8f21-a18adf3e68c5',5,'Bharat Harkal joined room 8bhhpb5',NULL,'2026-03-08 06:11:07.616693'),('8edf1ffa-8b04-4e9b-a453-fcb12eac0255','83b40a16-4c70-4780-b070-c925984613d9','adb61522-5558-41cd-8f21-a18adf3e68c5',2,'Join request by Bharat Harkal',NULL,'2026-03-08 06:11:07.580888'),('a1516de0-1b36-4b92-9f5f-5d968d78529a','baf109dd-5b0d-4219-bfb9-5e034f8859d9','adb61522-5558-41cd-8f21-a18adf3e68c5',6,'Connection s0eID-eWwAaKy9-PK2bXkg left room oukmp1j',NULL,'2026-03-07 19:24:33.684007'),('a751535e-21cc-4631-8337-d09d07252aea','b8010ce1-366f-401c-8dab-592da6aa6008','adb61522-5558-41cd-8f21-a18adf3e68c5',1,'Room 5glgh1d created',NULL,'2026-03-08 06:47:48.797759'),('ac63e6d4-dc37-4fec-9265-029afa60dbd6','baf109dd-5b0d-4219-bfb9-5e034f8859d9','adb61522-5558-41cd-8f21-a18adf3e68c5',1,'Room oukmp1j created',NULL,'2026-03-07 19:22:30.825292'),('b0faff19-9161-44d1-a466-7450381025d3','b8010ce1-366f-401c-8dab-592da6aa6008','adb61522-5558-41cd-8f21-a18adf3e68c5',6,'Connection 0SCuht8GaCqSOvVNLL_ppQ left room 5glgh1d',NULL,'2026-03-08 06:47:53.087072'),('cc768792-5b90-48d7-8335-cd8a4fc07e57','baf109dd-5b0d-4219-bfb9-5e034f8859d9','f3c2143a-8c38-4751-a99a-151fbf46d522',2,'Join request by om',NULL,'2026-03-07 19:23:02.597616'),('d8ccda5a-b6a1-4818-9dbb-e5728276d8ec','83b40a16-4c70-4780-b070-c925984613d9','adb61522-5558-41cd-8f21-a18adf3e68c5',1,'Room 8bhhpb5 created',NULL,'2026-03-08 06:11:07.527347'),('e9bcd6a3-7765-4508-bcca-d98e3cadb617','83b40a16-4c70-4780-b070-c925984613d9','adb61522-5558-41cd-8f21-a18adf3e68c5',6,'Connection iPfS-cbZw4bd7aFgv53k_A left room 8bhhpb5',NULL,'2026-03-08 06:11:08.693214'),('f2c62f33-c7a5-4f2d-9b16-27cbdd8e2c30','b8010ce1-366f-401c-8dab-592da6aa6008','adb61522-5558-41cd-8f21-a18adf3e68c5',5,'Bharat Harkal joined room 5glgh1d',NULL,'2026-03-08 06:47:48.895601'),('fd148a7d-b1b2-48b3-a856-3d4a9649beeb','b8010ce1-366f-401c-8dab-592da6aa6008','adb61522-5558-41cd-8f21-a18adf3e68c5',2,'Join request by Bharat Harkal',NULL,'2026-03-08 06:47:48.858116');
/*!40000 ALTER TABLE `activitylogs` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-09 20:06:59
