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
-- Table structure for table `signcoordinates`
--

DROP TABLE IF EXISTS `signcoordinates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `signcoordinates` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Label` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `TimeId` bigint NOT NULL,
  `CoordinatesJson` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedAtUtc` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SignCoordinates_CreatedAtUtc` (`CreatedAtUtc`),
  KEY `IX_SignCoordinates_Label` (`Label`),
  KEY `IX_SignCoordinates_TimeId` (`TimeId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `signcoordinates`
--

LOCK TABLES `signcoordinates` WRITE;
/*!40000 ALTER TABLE `signcoordinates` DISABLE KEYS */;
INSERT INTO `signcoordinates` VALUES ('0d8303a9-ddc2-4d28-88ca-ec2dcd6a47c2','hello',NULL,1772951907961,'[{\"hand\":0,\"points\":[{\"x\":204,\"y\":401,\"z\":0},{\"x\":261,\"y\":380,\"z\":-10},{\"x\":307,\"y\":343,\"z\":-13},{\"x\":336,\"y\":304,\"z\":-15},{\"x\":357,\"y\":270,\"z\":-17},{\"x\":297,\"y\":278,\"z\":4},{\"x\":315,\"y\":232,\"z\":6},{\"x\":326,\"y\":202,\"z\":5},{\"x\":334,\"y\":177,\"z\":4},{\"x\":267,\"y\":267,\"z\":6},{\"x\":289,\"y\":218,\"z\":10},{\"x\":300,\"y\":184,\"z\":9},{\"x\":308,\"y\":156,\"z\":8},{\"x\":238,\"y\":264,\"z\":6},{\"x\":255,\"y\":219,\"z\":9},{\"x\":265,\"y\":188,\"z\":9},{\"x\":272,\"y\":163,\"z\":9},{\"x\":206,\"y\":267,\"z\":4},{\"x\":213,\"y\":230,\"z\":6},{\"x\":218,\"y\":206,\"z\":7},{\"x\":223,\"y\":183,\"z\":8}]}]','2026-03-08 06:38:27.980461'),('4b29d3d0-a786-45a2-9214-0d8c83ee7eb7','hello',NULL,1772951888164,'[{\"hand\":0,\"points\":[{\"x\":539,\"y\":375,\"z\":0},{\"x\":459,\"y\":338,\"z\":-7},{\"x\":403,\"y\":297,\"z\":-8},{\"x\":364,\"y\":256,\"z\":-9},{\"x\":329,\"y\":224,\"z\":-10},{\"x\":406,\"y\":227,\"z\":9},{\"x\":376,\"y\":172,\"z\":11},{\"x\":358,\"y\":139,\"z\":11},{\"x\":343,\"y\":114,\"z\":10},{\"x\":442,\"y\":213,\"z\":10},{\"x\":414,\"y\":160,\"z\":14},{\"x\":398,\"y\":131,\"z\":13},{\"x\":384,\"y\":112,\"z\":11},{\"x\":479,\"y\":209,\"z\":10},{\"x\":460,\"y\":160,\"z\":12},{\"x\":448,\"y\":137,\"z\":11},{\"x\":436,\"y\":122,\"z\":9},{\"x\":516,\"y\":212,\"z\":9},{\"x\":506,\"y\":170,\"z\":9},{\"x\":499,\"y\":149,\"z\":9},{\"x\":491,\"y\":132,\"z\":8}]}]','2026-03-08 06:38:08.273211'),('4dec1fd6-c54d-43eb-a71e-879da4b17028','hello',NULL,1772951925994,'[{\"hand\":0,\"points\":[{\"x\":177,\"y\":367,\"z\":0},{\"x\":233,\"y\":359,\"z\":-9},{\"x\":280,\"y\":332,\"z\":-11},{\"x\":317,\"y\":305,\"z\":-13},{\"x\":350,\"y\":280,\"z\":-14},{\"x\":268,\"y\":265,\"z\":4},{\"x\":296,\"y\":218,\"z\":5},{\"x\":312,\"y\":188,\"z\":4},{\"x\":324,\"y\":162,\"z\":3},{\"x\":237,\"y\":251,\"z\":5},{\"x\":264,\"y\":199,\"z\":8},{\"x\":279,\"y\":165,\"z\":7},{\"x\":290,\"y\":139,\"z\":7},{\"x\":206,\"y\":246,\"z\":5},{\"x\":225,\"y\":199,\"z\":7},{\"x\":238,\"y\":169,\"z\":7},{\"x\":248,\"y\":146,\"z\":8},{\"x\":173,\"y\":248,\"z\":2},{\"x\":179,\"y\":208,\"z\":4},{\"x\":186,\"y\":185,\"z\":5},{\"x\":194,\"y\":165,\"z\":6}]}]','2026-03-08 06:38:46.008651');
/*!40000 ALTER TABLE `signcoordinates` ENABLE KEYS */;
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
