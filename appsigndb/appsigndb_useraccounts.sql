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
-- Table structure for table `useraccounts`
--

DROP TABLE IF EXISTS `useraccounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `useraccounts` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DisplayName` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` varchar(180) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `OrganizationId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `CreatedAtUtc` datetime(6) NOT NULL,
  `LastSeenAtUtc` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_UserAccounts_Email` (`Email`),
  KEY `IX_UserAccounts_OrganizationId` (`OrganizationId`),
  CONSTRAINT `FK_UserAccounts_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `organizations` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `useraccounts`
--

LOCK TABLES `useraccounts` WRITE;
/*!40000 ALTER TABLE `useraccounts` DISABLE KEYS */;
INSERT INTO `useraccounts` VALUES ('0d96773f-401e-4830-9ecc-9586c4ba2dfa','gaju','gaju.28764f685d55461597be592e5f713929@appsign.local',NULL,1,'2026-03-07 15:54:32.855357','2026-03-07 15:54:36.194100'),('2d703ec7-f235-4b64-b052-ccb8cfa6294e','omi','omi.dd02efe430ec4d5db1cd64bcbaa2c835@appsign.local',NULL,1,'2026-03-07 18:07:55.635158','2026-03-07 18:08:00.135498'),('63030662-2d9e-47a3-9b3c-e83608a282f7','omkar','omkar.e66caf54b5724c55a900857a11180b2c@appsign.local',NULL,1,'2026-03-07 15:42:22.357693','2026-03-07 18:50:47.817742'),('adb61522-5558-41cd-8f21-a18adf3e68c5','Bharat Harkal','bharat.harkal.dfd97a5b3735441ea556a8eaa52a1446@appsign.local',NULL,1,'2026-03-07 15:42:02.600974','2026-03-08 06:47:48.879190'),('f3c2143a-8c38-4751-a99a-151fbf46d522','om','om.f8e21370c7394a9090a12b64aa146136@appsign.local',NULL,1,'2026-03-07 19:09:28.689331','2026-03-07 19:23:06.376274');
/*!40000 ALTER TABLE `useraccounts` ENABLE KEYS */;
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
