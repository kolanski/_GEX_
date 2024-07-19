CREATE DATABASE  IF NOT EXISTS `bets` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `bets`;
-- MySQL dump 10.13  Distrib 5.6.23, for Win32 (x86)
--
-- Host: localhost    Database: bets
-- ------------------------------------------------------
-- Server version	5.6.24-log

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
-- Table structure for table `arbitrage`
--

DROP TABLE IF EXISTS `arbitrage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `arbitrage` (
  `Player1` varchar(140) NOT NULL,
  `Player2` varchar(140) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bet365`
--

DROP TABLE IF EXISTS `bet365`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bet365` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `betcity`
--

DROP TABLE IF EXISTS `betcity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `betcity` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fonbet`
--

DROP TABLE IF EXISTS `fonbet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fonbet` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `internetbet`
--

DROP TABLE IF EXISTS `internetbet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `internetbet` (
  `Player1` varchar(70) DEFAULT NULL,
  `Player2` varchar(70) DEFAULT NULL,
  `Coefficent1` double DEFAULT NULL,
  `Coefficent2` double DEFAULT NULL,
  `Bookmaker1` varchar(45) DEFAULT NULL,
  `Bookmaker2` varchar(45) DEFAULT NULL,
  `NumGame` varchar(45) DEFAULT NULL,
  `Score` varchar(45) DEFAULT NULL,
  `ScoreGame` varchar(45) DEFAULT NULL,
  `NumSet` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `marathon`
--

DROP TABLE IF EXISTS `marathon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `marathon` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `olimp`
--

DROP TABLE IF EXISTS `olimp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `olimp` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `parimatch`
--

DROP TABLE IF EXISTS `parimatch`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `parimatch` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pinnacle`
--

DROP TABLE IF EXISTS `pinnacle`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pinnacle` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sportingbet`
--

DROP TABLE IF EXISTS `sportingbet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sportingbet` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `titanbet`
--

DROP TABLE IF EXISTS `titanbet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `titanbet` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `unibet`
--

DROP TABLE IF EXISTS `unibet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unibet` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `williams`
--

DROP TABLE IF EXISTS `williams`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `williams` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `winline`
--

DROP TABLE IF EXISTS `winline`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `winline` (
  `Player1` varchar(70) NOT NULL,
  `Player2` varchar(70) NOT NULL,
  `Coefficent1` double NOT NULL,
  `Coefficent2` double NOT NULL,
  `Bookmaker1` varchar(45) NOT NULL,
  `Bookmaker2` varchar(45) NOT NULL,
  `NumGame` varchar(45) NOT NULL,
  `Score` varchar(45) NOT NULL,
  `ScoreGame` varchar(45) NOT NULL,
  `NumSet` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping routines for database 'bets'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-09-01 17:02:34
