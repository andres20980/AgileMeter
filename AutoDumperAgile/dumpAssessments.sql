CREATE DATABASE  IF NOT EXISTS `agilemeter` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `agilemeter`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: agilemeter
-- ------------------------------------------------------
-- Server version	5.7.24

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
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20181023105227_AddUserProyecto','2.0.2-rtm-10011'),('20181025094247_RemoveUserRoles','2.0.2-rtm-10011'),('20181026062550_AddAssessments','2.0.2-rtm-10011'),('20181029085809_LinkAssessmentWithSections','2.0.2-rtm-10011'),('20181029103724_LinkEvaluationsWithAssessments','2.0.2-rtm-10011');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `asignaciones`
--

DROP TABLE IF EXISTS `asignaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `asignaciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) NOT NULL,
  `Peso` int(11) DEFAULT '0',
  `SectionId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Asignaciones_SectionId` (`SectionId`),
  CONSTRAINT `FK_Asignaciones_Sections_SectionId` FOREIGN KEY (`SectionId`) REFERENCES `sections` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `asignaciones`
--

LOCK TABLES `asignaciones` WRITE;
/*!40000 ALTER TABLE `asignaciones` DISABLE KEYS */;
INSERT INTO `asignaciones` VALUES (1,'Daily',5,1),(2,'Retrospective',30,1),(3,'Sprint Review',20,1),(4,'Sprint Planning',15,1),(5,'Refinement',10,1),(6,'Product Owner',20,2),(7,'Scrum Master',60,2),(8,'Equipo Desarrollo',20,2),(9,'Product Backlog',30,3),(10,'Sprint Backlog',20,3),(11,'Incremento',15,3),(12,'Iteracion',5,3),(13,'Metricas',10,3),(14,'Daily-KB',5,4),(15,'Retrospective-KB',30,4),(16,'Sprint Review-KB',20,4),(17,'Sprint Planning-KB',15,4),(18,'Refinement-KB',10,4),(19,'Product Owner-KB',20,5),(20,'Scrum Master',60,5),(21,'Equipo Desarrollo',20,5);
/*!40000 ALTER TABLE `asignaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `assessment`
--

DROP TABLE IF EXISTS `assessment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `assessment` (
  `AssessmentId` int(11) NOT NULL AUTO_INCREMENT,
  `AssessmentName` varchar(50) NOT NULL,
  PRIMARY KEY (`AssessmentId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `assessment`
--

LOCK TABLES `assessment` WRITE;
/*!40000 ALTER TABLE `assessment` DISABLE KEYS */;
INSERT INTO `assessment` VALUES (1,'SCRUM'),(2,'KANBAN');
/*!40000 ALTER TABLE `assessment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `evaluaciones`
--

DROP TABLE IF EXISTS `evaluaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `evaluaciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Estado` bit(1) NOT NULL,
  `Fecha` datetime NOT NULL,
  `NotasEvaluacion` varchar(4000) DEFAULT NULL,
  `NotasObjetivos` varchar(4000) DEFAULT NULL,
  `ProyectoId` int(11) NOT NULL,
  `Puntuacion` double DEFAULT '0',
  `AssessmentId` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`),
  KEY `IX_Evaluaciones_ProyectoId` (`ProyectoId`),
  KEY `IX_Evaluaciones_AssessmentId` (`AssessmentId`),
  CONSTRAINT `FK_Evaluaciones_Assessment_AssessmentId` FOREIGN KEY (`AssessmentId`) REFERENCES `assessment` (`AssessmentId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Evaluaciones_Proyectos_ProyectoId` FOREIGN KEY (`ProyectoId`) REFERENCES `proyectos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `evaluaciones`
--

LOCK TABLES `evaluaciones` WRITE;
/*!40000 ALTER TABLE `evaluaciones` DISABLE KEYS */;
INSERT INTO `evaluaciones` VALUES (1,'\0','2018-10-23 12:06:07',NULL,NULL,2,0,1),(2,'','2018-10-24 14:49:36',NULL,NULL,1,59.5,1),(3,'','2018-10-24 14:53:29',NULL,NULL,1,2.8,1),(4,'','2018-10-26 10:19:41',NULL,NULL,5,0,1),(5,'','2018-10-29 11:28:50',NULL,NULL,5,0,1),(10,'','2018-10-29 12:15:53',NULL,NULL,5,0,1),(11,'\0','2018-10-29 12:16:24',NULL,NULL,5,0,1),(12,'\0','2018-10-29 12:52:34',NULL,NULL,5,0,1),(13,'\0','2018-10-29 12:52:53',NULL,NULL,5,0,1),(14,'\0','2018-10-29 13:12:43',NULL,NULL,5,0,1),(15,'\0','2018-10-29 14:05:38',NULL,NULL,5,0,2),(16,'','2018-10-29 14:06:53',NULL,NULL,5,0,1),(17,'\0','2018-10-29 16:06:31',NULL,NULL,5,0,2),(18,'','2018-10-29 16:06:46',NULL,NULL,5,0,2),(19,'','2018-10-29 16:53:19',NULL,NULL,5,0,2),(20,'','2018-10-29 16:54:19',NULL,NULL,5,0,1),(21,'','2018-10-29 16:58:55',NULL,NULL,5,0,2),(22,'\0','2018-10-30 11:01:11',NULL,NULL,5,0,2),(23,'\0','2018-10-30 11:01:43',NULL,NULL,5,0,1);
/*!40000 ALTER TABLE `evaluaciones` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = '' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `agilemeter`.`evaluaciones_AFTER_INSERT` AFTER INSERT ON `evaluaciones` FOR EACH ROW
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE PreId INT DEFAULT NEW.Id;
    DECLARE ids INT;
    DECLARE cur CURSOR FOR SELECT id FROM preguntas;
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN cur;
        ins_loop: LOOP
            FETCH cur INTO ids;
            IF done THEN
                LEAVE ins_loop;
            END IF;
            INSERT INTO respuestas (Estado, PreguntaId, EvaluacionId) VALUES (0 , ids , PreId);
        END LOOP;
    CLOSE cur;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `notasasignaciones`
--

DROP TABLE IF EXISTS `notasasignaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `notasasignaciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AsignacionId` int(11) NOT NULL,
  `EvaluacionId` int(11) NOT NULL,
  `Notas` varchar(4000) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_NotasAsignaciones_AsignacionId` (`AsignacionId`),
  KEY `IX_NotasAsignaciones_EvaluacionId` (`EvaluacionId`),
  CONSTRAINT `FK_NotasAsignaciones_Asignaciones_AsignacionId` FOREIGN KEY (`AsignacionId`) REFERENCES `asignaciones` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_NotasAsignaciones_Evaluaciones_EvaluacionId` FOREIGN KEY (`EvaluacionId`) REFERENCES `evaluaciones` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notasasignaciones`
--

LOCK TABLES `notasasignaciones` WRITE;
/*!40000 ALTER TABLE `notasasignaciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `notasasignaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notassections`
--

DROP TABLE IF EXISTS `notassections`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `notassections` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EvaluacionId` int(11) NOT NULL,
  `Notas` varchar(4000) DEFAULT NULL,
  `SectionId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_NotasSections_EvaluacionId` (`EvaluacionId`),
  KEY `IX_NotasSections_SectionId` (`SectionId`),
  CONSTRAINT `FK_NotasSections_Evaluaciones_EvaluacionId` FOREIGN KEY (`EvaluacionId`) REFERENCES `evaluaciones` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_NotasSections_Sections_SectionId` FOREIGN KEY (`SectionId`) REFERENCES `sections` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notassections`
--

LOCK TABLES `notassections` WRITE;
/*!40000 ALTER TABLE `notassections` DISABLE KEYS */;
/*!40000 ALTER TABLE `notassections` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `preguntas`
--

DROP TABLE IF EXISTS `preguntas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `preguntas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AsignacionId` int(11) NOT NULL,
  `Correcta` longtext,
  `Pregunta` varchar(120) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Preguntas_AsignacionId` (`AsignacionId`),
  CONSTRAINT `FK_Preguntas_Asignaciones_AsignacionId` FOREIGN KEY (`AsignacionId`) REFERENCES `asignaciones` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=170 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `preguntas`
--

LOCK TABLES `preguntas` WRITE;
/*!40000 ALTER TABLE `preguntas` DISABLE KEYS */;
INSERT INTO `preguntas` VALUES (1,1,NULL,'¿Se realiza la daily?'),(2,1,'Si','¿El equipo completo participa?'),(3,1,'Si','¿Se emplean como máximo 15 min?'),(4,1,'Si','¿Se mencionan los problemas e impedimentos?'),(5,1,'Si','¿Se revisan en cada daily los objetivos del Sprint?'),(6,1,'Si','¿Se realiza siempre a la misma hora y lugar?'),(7,1,'No','¿Participa gente que no pertenece al equipo?'),(8,2,NULL,'¿Se realiza la Retrospective al final de cada sprint?'),(9,2,'Si','¿Se plantean propuestas SMART?'),(10,2,'Si','¿Se implementan las propuestas?'),(11,2,'Si','¿Equipo al completo más PO participan?'),(12,2,'Si','¿Se analizan los problemas en profundidad?'),(13,2,'No','¿Participa gente que no pertenece al equipo?'),(14,2,'Si','¿Todo el equipo expresa su punto de vista?'),(15,2,'Si','¿Se analizan las métricas y su impacto durante la retro?'),(16,3,NULL,'¿Se realiza la Sprint Review al final de cada sprint?'),(17,3,'Si','¿Se muestra software funcionando y probado?'),(18,3,'Si','¿Se recibe feedback de interesados y PO?'),(19,3,'No','¿Se mencionan los items inacabados?'),(20,3,'Si','¿Se revisa si se ha alcanzado el objetivo del Sprint?'),(21,3,'No','¿Se muestran los items acabados al 99%?'),(22,4,NULL,'¿Se realiza Sprint Planning por cada Sprint?'),(23,4,'Si','¿El PO está disponible para dudas?'),(24,4,'Si','¿Está el PB preparado para el Sprint Planning?'),(25,4,'Si','¿El equipo completo participa?'),(26,4,'Si','¿El resultado de la sesión es el plan del Sprint?'),(27,4,'Si','¿El equipo completo cree que el plan es alcanzable?'),(28,4,'Si','¿El PO queda satisfecho con las prioridades?'),(29,4,'Si','¿Los PBI se dividen en tareas?'),(30,4,'Si','¿Las tareas son estimadas?'),(31,4,'Si','¿Se adquiere un compromiso por parte del equipo?'),(32,5,NULL,'¿Se realiza Refinement?'),(33,5,'Si','¿Es el PO quien decide cuando se hace un refinement?'),(34,5,'Si','¿El PO lleva las US definidas para discutir?'),(35,5,'Si','¿Se estima en tamaño relativo?'),(36,5,'Si','¿Existe DoR?'),(37,5,'Si','¿Se aplica DoR?'),(38,5,'Si','¿Se realizan preguntas y propuestas?'),(39,5,'Si','¿Participa todo el equipo?'),(40,5,'No','¿Participa en la estimación personas ajenas al equipo?'),(41,6,NULL,'¿Existe el rol de PO en el equipo?'),(42,6,'Si','¿El PO tiene poder para priorizar los elementos del PB?'),(43,6,'Si','¿El PO tiene el conocimiento suficiente para priorizar?'),(44,6,'Si','¿El PO tiene contacto directo con el equipo?'),(45,6,'Si','¿El PO tiene contacto directo con los interesados?'),(46,6,'Si','¿El PO tiene voz única (Si es equipo, solo hay una opinión)?'),(47,6,'Si','¿El PO tiene la visión del producto?'),(48,6,'No','¿El PO hace otras labores (codificar por ejemplo)?'),(49,6,'No','¿El PO toma decisiones técnicas?'),(50,7,NULL,'¿Existe el rol de SM en el equipo?'),(51,7,'Si','¿El SM se sienta con el equipo?'),(52,7,'Si','¿El SM se enfoca en la resolución de impedimentos?'),(53,7,'Si','¿El SM escala los impedimentos?'),(54,7,'No','¿El SM hace otras labores (codificar/analizar por ejemplo)?'),(55,7,'No','¿El SM toma decisiones técnicas o de negocio?'),(56,7,'Si','¿El SM ayuda/guía al PO para realizar correctamente su trabajo?'),(57,7,'Si','¿El SM empodera al equipo?'),(58,7,'No','¿El SM asume la responsabilidad si el equipo falla?'),(59,7,'Si','¿El SM permite que el equipo experimente y se equivoque?'),(60,7,'Si','¿Los líderes o managers de la organización conocen y/o comparten los principios ágiles?'),(61,8,'Si','¿El equipo tiene todas las habilidades necesarias?'),(62,8,'Si','¿Existen miembros del equipo encasillados, no conociendo absolutamente nada de otras áreas?'),(63,8,'Si','¿Los miembros del equipo se sientan juntos?'),(64,8,'Si','¿Hay com máximo 9 personas por equipo?'),(65,8,'No','¿Hay algún miembro del equipo que odie Scrum?'),(66,8,'No','¿Hay algún miembro del equipo profundamente desmotivado?'),(67,8,'Si','¿Tiene el equipo un drag factor interiorizado, planificado y consensuado con los stakeholders?'),(68,8,'No','¿Se realizan reuniones adicionales que estén fuera del framework de Scrum?'),(69,8,'Si','¿El equipo usa o dispone de herramientas para organizar sus tareas?'),(70,9,NULL,'¿Existe PB?'),(71,9,'Si','¿EL PB es visible y refleja la visión del producto?'),(72,9,'Si','¿Los PBI se priorizan por su valor de negocio?'),(73,9,'Si','¿Los PBI se estiman?'),(74,9,'Si','¿El equipo completo es quien realiza las estimaciones?'),(75,9,'Si','¿Los PBI son tan pequeños como para abordarse en un Sprint?'),(76,9,'Si','¿El PO entiende el propósito de todos los PBI?'),(77,10,NULL,'¿Existe SB?'),(78,10,'Si','¿El SB es visible y refleja el compromiso para el Sprint?'),(79,10,'Si','¿El SB se actualiza diariamente?'),(80,10,'Si','¿El SB es propiedad exclusiva del equipo?'),(81,11,NULL,'¿Existe DoD?'),(82,11,'Si','¿El DoD es alcanzable dentro de cada iteración?'),(83,11,'Si','¿El equipo respeta el DoD?'),(84,11,'Si','¿El Software entregado tiene calidad para subirse a producción si el negocio así lo pidiera?'),(85,11,'Si','¿Se actualiza el DoD?'),(86,11,'Si','¿Tanto PO como equipo están de acuerdo con el DoD?'),(87,12,NULL,'¿Existe iteraciones de tiempo fijo?'),(88,12,'Si','¿La longitud de las iteraciones está entre 2-4 semanas?'),(89,12,'Si','¿Siempre terminan a tiempo?'),(90,12,'No','¿El equipo es interrumpido durante una iteración?'),(91,12,'Si','¿El equipo normalmente entrega lo que comprometió?'),(92,12,'Si','¿Se ha cancelado alguna iteración que ha sido un fracaso?'),(93,13,NULL,'¿Se mide la velodidad del equipo?'),(94,13,'Si','¿Todos los PBI se estiman y se computan en la velocidad?'),(95,13,'Si','¿El PO usa la velocidad para planificar a futuro?'),(96,13,'Si','¿La velocidad sólo incluye PBI terminados?'),(97,13,'Si','¿El equipo tiene un Burndown por Sprint?'),(98,13,'Si','¿El Burndown es visible por todos los miembros del equipo?'),(99,13,'Si','¿El Burndown se actualiza diariamente?'),(100,13,'Si','¿El equipo conoce y entiende sus métricas?'),(101,14,NULL,'¿Se realiza la daily en Kanban?'),(102,14,'Si','¿El equipo completo participa?'),(103,14,'Si','¿Se emplean como máximo 15 min?'),(104,14,'Si','¿Se mencionan los problemas e impedimentos?'),(105,14,'Si','¿Se revisan en cada daily los objetivos del Sprint?'),(106,14,'Si','¿Se realiza siempre a la misma hora y lugar?'),(107,14,'No','¿Participa gente que no pertenece al equipo?'),(108,15,NULL,'¿Se realiza la Retrospective al final de cada sprint?'),(109,15,'Si','¿Se plantean propuestas SMART?'),(110,15,'Si','¿Se implementan las propuestas?'),(111,15,'Si','¿Equipo al completo más PO participan?'),(112,15,'Si','¿Se analizan los problemas en profundidad?'),(113,15,'No','¿Participa gente que no pertenece al equipo?'),(114,15,'Si','¿Todo el equipo expresa su punto de vista?'),(115,15,'Si','¿Se analizan las métricas y su impacto durante la retro?'),(116,16,NULL,'¿Se realiza la Sprint Review al final de cada sprint?'),(117,16,'Si','¿Se muestra software funcionando y probado?'),(118,16,'Si','¿Se recibe feedback de interesados y PO?'),(119,16,'No','¿Se mencionan los items inacabados?'),(120,16,'Si','¿Se revisa si se ha alcanzado el objetivo del Sprint?'),(121,16,'No','¿Se muestran los items acabados al 99%?'),(122,17,NULL,'¿Se realiza Sprint Planning por cada Sprint?'),(123,17,'Si','¿El PO está disponible para dudas?'),(124,17,'Si','¿Está el PB preparado para el Sprint Planning?'),(125,17,'Si','¿El equipo completo participa?'),(126,17,'Si','¿El resultado de la sesión es el plan del Sprint?'),(127,17,'Si','¿El equipo completo cree que el plan es alcanzable?'),(128,17,'Si','¿El PO queda satisfecho con las prioridades?'),(129,17,'Si','¿Los PBI se dividen en tareas?'),(130,17,'Si','¿Las tareas son estimadas?'),(131,17,'Si','¿Se adquiere un compromiso por parte del equipo?'),(132,18,NULL,'¿Se realiza Refinement?'),(133,18,'Si','¿Es el PO quien decide cuando se hace un refinement?'),(134,18,'Si','¿El PO lleva las US definidas para discutir?'),(135,18,'Si','¿Se estima en tamaño relativo?'),(136,18,'Si','¿Existe DoR?'),(137,18,'Si','¿Se aplica DoR?'),(138,18,'Si','¿Se realizan preguntas y propuestas?'),(139,18,'Si','¿Participa todo el equipo?'),(140,18,'No','¿Participa en la estimación personas ajenas al equipo?'),(141,19,NULL,'¿Existe el rol de PO en el equipo?'),(142,19,'Si','¿El PO tiene poder para priorizar los elementos del PB?'),(143,19,'Si','¿El PO tiene el conocimiento suficiente para priorizar?'),(144,19,'Si','¿El PO tiene contacto directo con el equipo?'),(145,19,'Si','¿El PO tiene contacto directo con los interesados?'),(146,19,'Si','¿El PO tiene voz única (Si es equipo, solo hay una opinión)?'),(147,19,'Si','¿El PO tiene la visión del producto?'),(148,19,'No','¿El PO hace otras labores (codificar por ejemplo)?'),(149,19,'No','¿El PO toma decisiones técnicas?'),(150,20,NULL,'¿Existe el rol de SM en el equipo?'),(151,20,'Si','¿El SM se sienta con el equipo?'),(152,20,'Si','¿El SM se enfoca en la resolución de impedimentos?'),(153,20,'Si','¿El SM escala los impedimentos?'),(154,20,'No','¿El SM hace otras labores (codificar/analizar por ejemplo)?'),(155,20,'No','¿El SM toma decisiones técnicas o de negocio?'),(156,20,'Si','¿El SM ayuda/guía al PO para realizar correctamente su trabajo?'),(157,20,'Si','¿El SM empodera al equipo?'),(158,20,'No','¿El SM asume la responsabilidad si el equipo falla?'),(159,20,'Si','¿El SM permite que el equipo experimente y se equivoque?'),(160,20,'Si','¿Los líderes o managers de la organización conocen y/o comparten los principios ágiles?'),(161,21,'Si','¿El equipo tiene todas las habilidades necesarias?'),(162,21,'Si','¿Existen miembros del equipo encasillados, no conociendo absolutamente nada de otras áreas?'),(163,21,'Si','¿Los miembros del equipo se sientan juntos?'),(164,21,'Si','¿Hay com máximo 9 personas por equipo?'),(165,21,'No','¿Hay algún miembro del equipo que odie Scrum?'),(166,21,'No','¿Hay algún miembro del equipo profundamente desmotivado?'),(167,21,'Si','¿Tiene el equipo un drag factor interiorizado, planificado y consensuado con los stakeholders?'),(168,21,'No','¿Se realizan reuniones adicionales que estén fuera del framework de Scrum?'),(169,21,'Si','¿El equipo usa o dispone de herramientas para organizar sus tareas?');
/*!40000 ALTER TABLE `preguntas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `proyectos`
--

DROP TABLE IF EXISTS `proyectos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `proyectos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Fecha` datetime NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `UserNombre` varchar(127) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Proyectos_UserNombre` (`UserNombre`),
  CONSTRAINT `FK_Proyectos_Users_UserNombre` FOREIGN KEY (`UserNombre`) REFERENCES `users` (`Nombre`) ON DELETE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `proyectos`
--

LOCK TABLES `proyectos` WRITE;
/*!40000 ALTER TABLE `proyectos` DISABLE KEYS */;
INSERT INTO `proyectos` VALUES (1,'2018-07-10 00:00:00','BCA','Admin'),(2,'2018-07-10 00:00:00','TESCO','Admin'),(3,'2018-07-10 00:00:00','BestDay','User'),(4,'2018-07-10 00:00:00','TVE','User'),(5,'2018-07-10 00:00:00','Proyecto Test','Test');
/*!40000 ALTER TABLE `proyectos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `respuestas`
--

DROP TABLE IF EXISTS `respuestas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `respuestas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Estado` int(11) DEFAULT '0',
  `EvaluacionId` int(11) NOT NULL,
  `Notas` varchar(4000) DEFAULT NULL,
  `NotasAdmin` varchar(4000) DEFAULT NULL,
  `PreguntaId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Respuestas_EvaluacionId` (`EvaluacionId`),
  KEY `IX_Respuestas_PreguntaId` (`PreguntaId`),
  CONSTRAINT `FK_Respuestas_Evaluaciones_EvaluacionId` FOREIGN KEY (`EvaluacionId`) REFERENCES `evaluaciones` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Respuestas_Preguntas_PreguntaId` FOREIGN KEY (`PreguntaId`) REFERENCES `preguntas` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2039 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `respuestas`
--

LOCK TABLES `respuestas` WRITE;
/*!40000 ALTER TABLE `respuestas` DISABLE KEYS */;
INSERT INTO `respuestas` VALUES (1,2,1,NULL,NULL,1),(2,0,1,NULL,NULL,2),(3,0,1,NULL,NULL,3),(4,0,1,NULL,NULL,4),(5,0,1,NULL,NULL,5),(6,0,1,NULL,NULL,6),(7,0,1,NULL,NULL,7),(8,0,1,NULL,NULL,8),(9,0,1,NULL,NULL,9),(10,0,1,NULL,NULL,10),(11,0,1,NULL,NULL,11),(12,0,1,NULL,NULL,12),(13,0,1,NULL,NULL,13),(14,0,1,NULL,NULL,14),(15,0,1,NULL,NULL,15),(16,0,1,NULL,NULL,16),(17,0,1,NULL,NULL,17),(18,0,1,NULL,NULL,18),(19,0,1,NULL,NULL,19),(20,0,1,NULL,NULL,20),(21,0,1,NULL,NULL,21),(22,0,1,NULL,NULL,22),(23,0,1,NULL,NULL,23),(24,0,1,NULL,NULL,24),(25,0,1,NULL,NULL,25),(26,0,1,NULL,NULL,26),(27,0,1,NULL,NULL,27),(28,0,1,NULL,NULL,28),(29,0,1,NULL,NULL,29),(30,0,1,NULL,NULL,30),(31,0,1,NULL,NULL,31),(32,0,1,NULL,NULL,32),(33,0,1,NULL,NULL,33),(34,0,1,NULL,NULL,34),(35,0,1,NULL,NULL,35),(36,0,1,NULL,NULL,36),(37,0,1,NULL,NULL,37),(38,0,1,NULL,NULL,38),(39,0,1,NULL,NULL,39),(40,0,1,NULL,NULL,40),(41,0,1,NULL,NULL,41),(42,0,1,NULL,NULL,42),(43,0,1,NULL,NULL,43),(44,0,1,NULL,NULL,44),(45,0,1,NULL,NULL,45),(46,0,1,NULL,NULL,46),(47,0,1,NULL,NULL,47),(48,0,1,NULL,NULL,48),(49,0,1,NULL,NULL,49),(50,0,1,NULL,NULL,50),(51,0,1,NULL,NULL,51),(52,0,1,NULL,NULL,52),(53,0,1,NULL,NULL,53),(54,0,1,NULL,NULL,54),(55,0,1,NULL,NULL,55),(56,0,1,NULL,NULL,56),(57,0,1,NULL,NULL,57),(58,0,1,NULL,NULL,58),(59,0,1,NULL,NULL,59),(60,0,1,NULL,NULL,60),(61,0,1,NULL,NULL,61),(62,0,1,NULL,NULL,62),(63,0,1,NULL,NULL,63),(64,0,1,NULL,NULL,64),(65,0,1,NULL,NULL,65),(66,0,1,NULL,NULL,66),(67,0,1,NULL,NULL,67),(68,0,1,NULL,NULL,68),(69,0,1,NULL,NULL,69),(70,0,1,NULL,NULL,70),(71,0,1,NULL,NULL,71),(72,0,1,NULL,NULL,72),(73,0,1,NULL,NULL,73),(74,0,1,NULL,NULL,74),(75,0,1,NULL,NULL,75),(76,0,1,NULL,NULL,76),(77,0,1,NULL,NULL,77),(78,0,1,NULL,NULL,78),(79,0,1,NULL,NULL,79),(80,0,1,NULL,NULL,80),(81,0,1,NULL,NULL,81),(82,0,1,NULL,NULL,82),(83,0,1,NULL,NULL,83),(84,0,1,NULL,NULL,84),(85,0,1,NULL,NULL,85),(86,0,1,NULL,NULL,86),(87,0,1,NULL,NULL,87),(88,0,1,NULL,NULL,88),(89,0,1,NULL,NULL,89),(90,0,1,NULL,NULL,90),(91,0,1,NULL,NULL,91),(92,0,1,NULL,NULL,92),(93,0,1,NULL,NULL,93),(94,0,1,NULL,NULL,94),(95,0,1,NULL,NULL,95),(96,0,1,NULL,NULL,96),(97,0,1,NULL,NULL,97),(98,0,1,NULL,NULL,98),(99,0,1,NULL,NULL,99),(100,0,1,NULL,NULL,100),(101,1,2,NULL,NULL,1),(102,1,2,NULL,NULL,2),(103,1,2,NULL,NULL,3),(104,1,2,NULL,NULL,4),(105,1,2,NULL,NULL,5),(106,2,2,NULL,NULL,6),(107,2,2,NULL,NULL,7),(108,1,2,NULL,NULL,8),(109,1,2,NULL,NULL,9),(110,2,2,NULL,NULL,10),(111,2,2,NULL,NULL,11),(112,1,2,NULL,NULL,12),(113,1,2,NULL,NULL,13),(114,1,2,NULL,NULL,14),(115,2,2,NULL,NULL,15),(116,1,2,NULL,NULL,16),(117,1,2,NULL,NULL,17),(118,1,2,NULL,NULL,18),(119,2,2,NULL,NULL,19),(120,1,2,NULL,NULL,20),(121,1,2,NULL,NULL,21),(122,1,2,NULL,NULL,22),(123,1,2,NULL,NULL,23),(124,2,2,NULL,NULL,24),(125,1,2,NULL,NULL,25),(126,1,2,NULL,NULL,26),(127,1,2,NULL,NULL,27),(128,2,2,NULL,NULL,28),(129,1,2,NULL,NULL,29),(130,1,2,NULL,NULL,30),(131,2,2,NULL,NULL,31),(132,1,2,NULL,NULL,32),(133,1,2,NULL,NULL,33),(134,1,2,NULL,NULL,34),(135,2,2,NULL,NULL,35),(136,1,2,NULL,NULL,36),(137,2,2,NULL,NULL,37),(138,1,2,NULL,NULL,38),(139,1,2,NULL,NULL,39),(140,1,2,NULL,NULL,40),(141,1,2,NULL,NULL,41),(142,1,2,NULL,NULL,42),(143,1,2,NULL,NULL,43),(144,1,2,NULL,NULL,44),(145,1,2,NULL,NULL,45),(146,2,2,NULL,NULL,46),(147,1,2,NULL,NULL,47),(148,2,2,NULL,NULL,48),(149,1,2,NULL,NULL,49),(150,1,2,NULL,NULL,50),(151,1,2,NULL,NULL,51),(152,1,2,NULL,NULL,52),(153,2,2,NULL,NULL,53),(154,1,2,NULL,NULL,54),(155,1,2,NULL,NULL,55),(156,1,2,NULL,NULL,56),(157,1,2,NULL,NULL,57),(158,1,2,NULL,NULL,58),(159,2,2,NULL,NULL,59),(160,1,2,NULL,NULL,60),(161,1,2,NULL,NULL,61),(162,1,2,NULL,NULL,62),(163,2,2,NULL,NULL,63),(164,2,2,NULL,NULL,64),(165,1,2,NULL,NULL,65),(166,1,2,NULL,NULL,66),(167,2,2,NULL,NULL,67),(168,1,2,NULL,NULL,68),(169,1,2,NULL,NULL,69),(170,1,2,NULL,NULL,70),(171,1,2,NULL,NULL,71),(172,2,2,NULL,NULL,72),(173,1,2,NULL,NULL,73),(174,1,2,NULL,NULL,74),(175,1,2,NULL,NULL,75),(176,1,2,NULL,NULL,76),(177,1,2,NULL,NULL,77),(178,1,2,NULL,NULL,78),(179,1,2,NULL,NULL,79),(180,2,2,NULL,NULL,80),(181,1,2,NULL,NULL,81),(182,1,2,NULL,NULL,82),(183,1,2,NULL,NULL,83),(184,1,2,NULL,NULL,84),(185,2,2,NULL,NULL,85),(186,1,2,NULL,NULL,86),(187,1,2,NULL,NULL,87),(188,1,2,NULL,NULL,88),(189,2,2,NULL,NULL,89),(190,2,2,NULL,NULL,90),(191,1,2,NULL,NULL,91),(192,1,2,NULL,NULL,92),(193,1,2,NULL,NULL,93),(194,1,2,NULL,NULL,94),(195,1,2,NULL,NULL,95),(196,2,2,NULL,NULL,96),(197,2,2,NULL,NULL,97),(198,1,2,NULL,NULL,98),(199,1,2,NULL,NULL,99),(200,1,2,NULL,NULL,100),(201,1,3,NULL,NULL,1),(202,0,3,NULL,NULL,2),(203,1,3,NULL,NULL,3),(204,1,3,NULL,NULL,4),(205,0,3,NULL,NULL,5),(206,0,3,NULL,NULL,6),(207,1,3,NULL,NULL,7),(208,1,3,NULL,NULL,8),(209,0,3,NULL,NULL,9),(210,1,3,NULL,NULL,10),(211,0,3,NULL,NULL,11),(212,0,3,NULL,NULL,12),(213,1,3,NULL,NULL,13),(214,0,3,NULL,NULL,14),(215,1,3,NULL,NULL,15),(216,1,3,NULL,NULL,16),(217,0,3,NULL,NULL,17),(218,0,3,NULL,NULL,18),(219,0,3,NULL,NULL,19),(220,0,3,NULL,NULL,20),(221,1,3,NULL,NULL,21),(222,0,3,NULL,NULL,22),(223,0,3,NULL,NULL,23),(224,0,3,NULL,NULL,24),(225,0,3,NULL,NULL,25),(226,0,3,NULL,NULL,26),(227,0,3,NULL,NULL,27),(228,0,3,NULL,NULL,28),(229,0,3,NULL,NULL,29),(230,0,3,NULL,NULL,30),(231,0,3,NULL,NULL,31),(232,0,3,NULL,NULL,32),(233,0,3,NULL,NULL,33),(234,0,3,NULL,NULL,34),(235,0,3,NULL,NULL,35),(236,0,3,NULL,NULL,36),(237,0,3,NULL,NULL,37),(238,0,3,NULL,NULL,38),(239,0,3,NULL,NULL,39),(240,0,3,NULL,NULL,40),(241,0,3,NULL,NULL,41),(242,0,3,NULL,NULL,42),(243,0,3,NULL,NULL,43),(244,0,3,NULL,NULL,44),(245,0,3,NULL,NULL,45),(246,0,3,NULL,NULL,46),(247,0,3,NULL,NULL,47),(248,0,3,NULL,NULL,48),(249,0,3,NULL,NULL,49),(250,0,3,NULL,NULL,50),(251,0,3,NULL,NULL,51),(252,0,3,NULL,NULL,52),(253,0,3,NULL,NULL,53),(254,0,3,NULL,NULL,54),(255,0,3,NULL,NULL,55),(256,0,3,NULL,NULL,56),(257,0,3,NULL,NULL,57),(258,0,3,NULL,NULL,58),(259,0,3,NULL,NULL,59),(260,0,3,NULL,NULL,60),(261,0,3,NULL,NULL,61),(262,0,3,NULL,NULL,62),(263,0,3,NULL,NULL,63),(264,0,3,NULL,NULL,64),(265,0,3,NULL,NULL,65),(266,0,3,NULL,NULL,66),(267,0,3,NULL,NULL,67),(268,0,3,NULL,NULL,68),(269,0,3,NULL,NULL,69),(270,0,3,NULL,NULL,70),(271,0,3,NULL,NULL,71),(272,0,3,NULL,NULL,72),(273,0,3,NULL,NULL,73),(274,0,3,NULL,NULL,74),(275,0,3,NULL,NULL,75),(276,0,3,NULL,NULL,76),(277,0,3,NULL,NULL,77),(278,0,3,NULL,NULL,78),(279,0,3,NULL,NULL,79),(280,0,3,NULL,NULL,80),(281,0,3,NULL,NULL,81),(282,0,3,NULL,NULL,82),(283,0,3,NULL,NULL,83),(284,0,3,NULL,NULL,84),(285,0,3,NULL,NULL,85),(286,0,3,NULL,NULL,86),(287,0,3,NULL,NULL,87),(288,0,3,NULL,NULL,88),(289,0,3,NULL,NULL,89),(290,0,3,NULL,NULL,90),(291,0,3,NULL,NULL,91),(292,0,3,NULL,NULL,92),(293,0,3,NULL,NULL,93),(294,0,3,NULL,NULL,94),(295,0,3,NULL,NULL,95),(296,0,3,NULL,NULL,96),(297,0,3,NULL,NULL,97),(298,0,3,NULL,NULL,98),(299,0,3,NULL,NULL,99),(300,0,3,NULL,NULL,100),(301,0,4,NULL,NULL,1),(302,0,4,NULL,NULL,2),(303,0,4,NULL,NULL,3),(304,0,4,NULL,NULL,4),(305,0,4,NULL,NULL,5),(306,0,4,NULL,NULL,6),(307,0,4,NULL,NULL,7),(308,0,4,NULL,NULL,8),(309,0,4,NULL,NULL,9),(310,0,4,NULL,NULL,10),(311,0,4,NULL,NULL,11),(312,0,4,NULL,NULL,12),(313,0,4,NULL,NULL,13),(314,0,4,NULL,NULL,14),(315,0,4,NULL,NULL,15),(316,0,4,NULL,NULL,16),(317,0,4,NULL,NULL,17),(318,0,4,NULL,NULL,18),(319,0,4,NULL,NULL,19),(320,0,4,NULL,NULL,20),(321,0,4,NULL,NULL,21),(322,0,4,NULL,NULL,22),(323,0,4,NULL,NULL,23),(324,0,4,NULL,NULL,24),(325,0,4,NULL,NULL,25),(326,0,4,NULL,NULL,26),(327,0,4,NULL,NULL,27),(328,0,4,NULL,NULL,28),(329,0,4,NULL,NULL,29),(330,0,4,NULL,NULL,30),(331,0,4,NULL,NULL,31),(332,0,4,NULL,NULL,32),(333,0,4,NULL,NULL,33),(334,0,4,NULL,NULL,34),(335,0,4,NULL,NULL,35),(336,0,4,NULL,NULL,36),(337,0,4,NULL,NULL,37),(338,0,4,NULL,NULL,38),(339,0,4,NULL,NULL,39),(340,0,4,NULL,NULL,40),(341,0,4,NULL,NULL,41),(342,0,4,NULL,NULL,42),(343,0,4,NULL,NULL,43),(344,0,4,NULL,NULL,44),(345,0,4,NULL,NULL,45),(346,0,4,NULL,NULL,46),(347,0,4,NULL,NULL,47),(348,0,4,NULL,NULL,48),(349,0,4,NULL,NULL,49),(350,0,4,NULL,NULL,50),(351,0,4,NULL,NULL,51),(352,0,4,NULL,NULL,52),(353,0,4,NULL,NULL,53),(354,0,4,NULL,NULL,54),(355,0,4,NULL,NULL,55),(356,0,4,NULL,NULL,56),(357,0,4,NULL,NULL,57),(358,0,4,NULL,NULL,58),(359,0,4,NULL,NULL,59),(360,0,4,NULL,NULL,60),(361,0,4,NULL,NULL,61),(362,0,4,NULL,NULL,62),(363,0,4,NULL,NULL,63),(364,0,4,NULL,NULL,64),(365,0,4,NULL,NULL,65),(366,0,4,NULL,NULL,66),(367,0,4,NULL,NULL,67),(368,0,4,NULL,NULL,68),(369,0,4,NULL,NULL,69),(370,0,4,NULL,NULL,70),(371,0,4,NULL,NULL,71),(372,0,4,NULL,NULL,72),(373,0,4,NULL,NULL,73),(374,0,4,NULL,NULL,74),(375,0,4,NULL,NULL,75),(376,0,4,NULL,NULL,76),(377,0,4,NULL,NULL,77),(378,0,4,NULL,NULL,78),(379,0,4,NULL,NULL,79),(380,0,4,NULL,NULL,80),(381,0,4,NULL,NULL,81),(382,0,4,NULL,NULL,82),(383,0,4,NULL,NULL,83),(384,0,4,NULL,NULL,84),(385,0,4,NULL,NULL,85),(386,0,4,NULL,NULL,86),(387,0,4,NULL,NULL,87),(388,0,4,NULL,NULL,88),(389,0,4,NULL,NULL,89),(390,0,4,NULL,NULL,90),(391,0,4,NULL,NULL,91),(392,0,4,NULL,NULL,92),(393,0,4,NULL,NULL,93),(394,0,4,NULL,NULL,94),(395,0,4,NULL,NULL,95),(396,0,4,NULL,NULL,96),(397,0,4,NULL,NULL,97),(398,0,4,NULL,NULL,98),(399,0,4,NULL,NULL,99),(400,0,4,NULL,NULL,100),(401,0,5,NULL,NULL,1),(402,0,5,NULL,NULL,2),(403,0,5,NULL,NULL,3),(404,0,5,NULL,NULL,4),(405,0,5,NULL,NULL,5),(406,0,5,NULL,NULL,6),(407,0,5,NULL,NULL,7),(408,0,5,NULL,NULL,8),(409,0,5,NULL,NULL,9),(410,0,5,NULL,NULL,10),(411,0,5,NULL,NULL,11),(412,0,5,NULL,NULL,12),(413,0,5,NULL,NULL,13),(414,0,5,NULL,NULL,14),(415,0,5,NULL,NULL,15),(416,0,5,NULL,NULL,16),(417,0,5,NULL,NULL,17),(418,0,5,NULL,NULL,18),(419,0,5,NULL,NULL,19),(420,0,5,NULL,NULL,20),(421,0,5,NULL,NULL,21),(422,0,5,NULL,NULL,22),(423,0,5,NULL,NULL,23),(424,0,5,NULL,NULL,24),(425,0,5,NULL,NULL,25),(426,0,5,NULL,NULL,26),(427,0,5,NULL,NULL,27),(428,0,5,NULL,NULL,28),(429,0,5,NULL,NULL,29),(430,0,5,NULL,NULL,30),(431,0,5,NULL,NULL,31),(432,0,5,NULL,NULL,32),(433,0,5,NULL,NULL,33),(434,0,5,NULL,NULL,34),(435,0,5,NULL,NULL,35),(436,0,5,NULL,NULL,36),(437,0,5,NULL,NULL,37),(438,0,5,NULL,NULL,38),(439,0,5,NULL,NULL,39),(440,0,5,NULL,NULL,40),(441,0,5,NULL,NULL,41),(442,0,5,NULL,NULL,42),(443,0,5,NULL,NULL,43),(444,0,5,NULL,NULL,44),(445,0,5,NULL,NULL,45),(446,0,5,NULL,NULL,46),(447,0,5,NULL,NULL,47),(448,0,5,NULL,NULL,48),(449,0,5,NULL,NULL,49),(450,0,5,NULL,NULL,50),(451,0,5,NULL,NULL,51),(452,0,5,NULL,NULL,52),(453,0,5,NULL,NULL,53),(454,0,5,NULL,NULL,54),(455,0,5,NULL,NULL,55),(456,0,5,NULL,NULL,56),(457,0,5,NULL,NULL,57),(458,0,5,NULL,NULL,58),(459,0,5,NULL,NULL,59),(460,0,5,NULL,NULL,60),(461,0,5,NULL,NULL,61),(462,0,5,NULL,NULL,62),(463,0,5,NULL,NULL,63),(464,0,5,NULL,NULL,64),(465,0,5,NULL,NULL,65),(466,0,5,NULL,NULL,66),(467,0,5,NULL,NULL,67),(468,0,5,NULL,NULL,68),(469,0,5,NULL,NULL,69),(470,0,5,NULL,NULL,70),(471,0,5,NULL,NULL,71),(472,0,5,NULL,NULL,72),(473,0,5,NULL,NULL,73),(474,0,5,NULL,NULL,74),(475,0,5,NULL,NULL,75),(476,0,5,NULL,NULL,76),(477,0,5,NULL,NULL,77),(478,0,5,NULL,NULL,78),(479,0,5,NULL,NULL,79),(480,0,5,NULL,NULL,80),(481,0,5,NULL,NULL,81),(482,0,5,NULL,NULL,82),(483,0,5,NULL,NULL,83),(484,0,5,NULL,NULL,84),(485,0,5,NULL,NULL,85),(486,0,5,NULL,NULL,86),(487,0,5,NULL,NULL,87),(488,0,5,NULL,NULL,88),(489,0,5,NULL,NULL,89),(490,0,5,NULL,NULL,90),(491,0,5,NULL,NULL,91),(492,0,5,NULL,NULL,92),(493,0,5,NULL,NULL,93),(494,0,5,NULL,NULL,94),(495,0,5,NULL,NULL,95),(496,0,5,NULL,NULL,96),(497,0,5,NULL,NULL,97),(498,0,5,NULL,NULL,98),(499,0,5,NULL,NULL,99),(500,0,5,NULL,NULL,100),(501,0,10,NULL,NULL,1),(502,0,10,NULL,NULL,2),(503,0,10,NULL,NULL,3),(504,0,10,NULL,NULL,4),(505,0,10,NULL,NULL,5),(506,0,10,NULL,NULL,6),(507,0,10,NULL,NULL,7),(508,0,10,NULL,NULL,8),(509,0,10,NULL,NULL,9),(510,0,10,NULL,NULL,10),(511,0,10,NULL,NULL,11),(512,0,10,NULL,NULL,12),(513,0,10,NULL,NULL,13),(514,0,10,NULL,NULL,14),(515,0,10,NULL,NULL,15),(516,0,10,NULL,NULL,16),(517,0,10,NULL,NULL,17),(518,0,10,NULL,NULL,18),(519,0,10,NULL,NULL,19),(520,0,10,NULL,NULL,20),(521,0,10,NULL,NULL,21),(522,0,10,NULL,NULL,22),(523,0,10,NULL,NULL,23),(524,0,10,NULL,NULL,24),(525,0,10,NULL,NULL,25),(526,0,10,NULL,NULL,26),(527,0,10,NULL,NULL,27),(528,0,10,NULL,NULL,28),(529,0,10,NULL,NULL,29),(530,0,10,NULL,NULL,30),(531,0,10,NULL,NULL,31),(532,0,10,NULL,NULL,32),(533,0,10,NULL,NULL,33),(534,0,10,NULL,NULL,34),(535,0,10,NULL,NULL,35),(536,0,10,NULL,NULL,36),(537,0,10,NULL,NULL,37),(538,0,10,NULL,NULL,38),(539,0,10,NULL,NULL,39),(540,0,10,NULL,NULL,40),(541,0,10,NULL,NULL,41),(542,0,10,NULL,NULL,42),(543,0,10,NULL,NULL,43),(544,0,10,NULL,NULL,44),(545,0,10,NULL,NULL,45),(546,0,10,NULL,NULL,46),(547,0,10,NULL,NULL,47),(548,0,10,NULL,NULL,48),(549,0,10,NULL,NULL,49),(550,0,10,NULL,NULL,50),(551,0,10,NULL,NULL,51),(552,0,10,NULL,NULL,52),(553,0,10,NULL,NULL,53),(554,0,10,NULL,NULL,54),(555,0,10,NULL,NULL,55),(556,0,10,NULL,NULL,56),(557,0,10,NULL,NULL,57),(558,0,10,NULL,NULL,58),(559,0,10,NULL,NULL,59),(560,0,10,NULL,NULL,60),(561,0,10,NULL,NULL,61),(562,0,10,NULL,NULL,62),(563,0,10,NULL,NULL,63),(564,0,10,NULL,NULL,64),(565,0,10,NULL,NULL,65),(566,0,10,NULL,NULL,66),(567,0,10,NULL,NULL,67),(568,0,10,NULL,NULL,68),(569,0,10,NULL,NULL,69),(570,0,10,NULL,NULL,70),(571,0,10,NULL,NULL,71),(572,0,10,NULL,NULL,72),(573,0,10,NULL,NULL,73),(574,0,10,NULL,NULL,74),(575,0,10,NULL,NULL,75),(576,0,10,NULL,NULL,76),(577,0,10,NULL,NULL,77),(578,0,10,NULL,NULL,78),(579,0,10,NULL,NULL,79),(580,0,10,NULL,NULL,80),(581,0,10,NULL,NULL,81),(582,0,10,NULL,NULL,82),(583,0,10,NULL,NULL,83),(584,0,10,NULL,NULL,84),(585,0,10,NULL,NULL,85),(586,0,10,NULL,NULL,86),(587,0,10,NULL,NULL,87),(588,0,10,NULL,NULL,88),(589,0,10,NULL,NULL,89),(590,0,10,NULL,NULL,90),(591,0,10,NULL,NULL,91),(592,0,10,NULL,NULL,92),(593,0,10,NULL,NULL,93),(594,0,10,NULL,NULL,94),(595,0,10,NULL,NULL,95),(596,0,10,NULL,NULL,96),(597,0,10,NULL,NULL,97),(598,0,10,NULL,NULL,98),(599,0,10,NULL,NULL,99),(600,0,10,NULL,NULL,100),(601,0,11,NULL,NULL,1),(602,0,11,NULL,NULL,2),(603,0,11,NULL,NULL,3),(604,0,11,NULL,NULL,4),(605,0,11,NULL,NULL,5),(606,0,11,NULL,NULL,6),(607,0,11,NULL,NULL,7),(608,0,11,NULL,NULL,8),(609,0,11,NULL,NULL,9),(610,0,11,NULL,NULL,10),(611,0,11,NULL,NULL,11),(612,0,11,NULL,NULL,12),(613,0,11,NULL,NULL,13),(614,0,11,NULL,NULL,14),(615,0,11,NULL,NULL,15),(616,0,11,NULL,NULL,16),(617,0,11,NULL,NULL,17),(618,0,11,NULL,NULL,18),(619,0,11,NULL,NULL,19),(620,0,11,NULL,NULL,20),(621,0,11,NULL,NULL,21),(622,0,11,NULL,NULL,22),(623,0,11,NULL,NULL,23),(624,0,11,NULL,NULL,24),(625,0,11,NULL,NULL,25),(626,0,11,NULL,NULL,26),(627,0,11,NULL,NULL,27),(628,0,11,NULL,NULL,28),(629,0,11,NULL,NULL,29),(630,0,11,NULL,NULL,30),(631,0,11,NULL,NULL,31),(632,0,11,NULL,NULL,32),(633,0,11,NULL,NULL,33),(634,0,11,NULL,NULL,34),(635,0,11,NULL,NULL,35),(636,0,11,NULL,NULL,36),(637,0,11,NULL,NULL,37),(638,0,11,NULL,NULL,38),(639,0,11,NULL,NULL,39),(640,0,11,NULL,NULL,40),(641,0,11,NULL,NULL,41),(642,0,11,NULL,NULL,42),(643,0,11,NULL,NULL,43),(644,0,11,NULL,NULL,44),(645,0,11,NULL,NULL,45),(646,0,11,NULL,NULL,46),(647,0,11,NULL,NULL,47),(648,0,11,NULL,NULL,48),(649,0,11,NULL,NULL,49),(650,0,11,NULL,NULL,50),(651,0,11,NULL,NULL,51),(652,0,11,NULL,NULL,52),(653,0,11,NULL,NULL,53),(654,0,11,NULL,NULL,54),(655,0,11,NULL,NULL,55),(656,0,11,NULL,NULL,56),(657,0,11,NULL,NULL,57),(658,0,11,NULL,NULL,58),(659,0,11,NULL,NULL,59),(660,0,11,NULL,NULL,60),(661,0,11,NULL,NULL,61),(662,0,11,NULL,NULL,62),(663,0,11,NULL,NULL,63),(664,0,11,NULL,NULL,64),(665,0,11,NULL,NULL,65),(666,0,11,NULL,NULL,66),(667,0,11,NULL,NULL,67),(668,0,11,NULL,NULL,68),(669,0,11,NULL,NULL,69),(670,0,11,NULL,NULL,70),(671,0,11,NULL,NULL,71),(672,0,11,NULL,NULL,72),(673,0,11,NULL,NULL,73),(674,0,11,NULL,NULL,74),(675,0,11,NULL,NULL,75),(676,0,11,NULL,NULL,76),(677,0,11,NULL,NULL,77),(678,0,11,NULL,NULL,78),(679,0,11,NULL,NULL,79),(680,0,11,NULL,NULL,80),(681,0,11,NULL,NULL,81),(682,0,11,NULL,NULL,82),(683,0,11,NULL,NULL,83),(684,0,11,NULL,NULL,84),(685,0,11,NULL,NULL,85),(686,0,11,NULL,NULL,86),(687,0,11,NULL,NULL,87),(688,0,11,NULL,NULL,88),(689,0,11,NULL,NULL,89),(690,0,11,NULL,NULL,90),(691,0,11,NULL,NULL,91),(692,0,11,NULL,NULL,92),(693,0,11,NULL,NULL,93),(694,0,11,NULL,NULL,94),(695,0,11,NULL,NULL,95),(696,0,11,NULL,NULL,96),(697,0,11,NULL,NULL,97),(698,0,11,NULL,NULL,98),(699,0,11,NULL,NULL,99),(700,0,11,NULL,NULL,100),(701,0,12,NULL,NULL,1),(702,0,12,NULL,NULL,2),(703,0,12,NULL,NULL,3),(704,0,12,NULL,NULL,4),(705,0,12,NULL,NULL,5),(706,0,12,NULL,NULL,6),(707,0,12,NULL,NULL,7),(708,0,12,NULL,NULL,8),(709,0,12,NULL,NULL,9),(710,0,12,NULL,NULL,10),(711,0,12,NULL,NULL,11),(712,0,12,NULL,NULL,12),(713,0,12,NULL,NULL,13),(714,0,12,NULL,NULL,14),(715,0,12,NULL,NULL,15),(716,0,12,NULL,NULL,16),(717,0,12,NULL,NULL,17),(718,0,12,NULL,NULL,18),(719,0,12,NULL,NULL,19),(720,0,12,NULL,NULL,20),(721,0,12,NULL,NULL,21),(722,0,12,NULL,NULL,22),(723,0,12,NULL,NULL,23),(724,0,12,NULL,NULL,24),(725,0,12,NULL,NULL,25),(726,0,12,NULL,NULL,26),(727,0,12,NULL,NULL,27),(728,0,12,NULL,NULL,28),(729,0,12,NULL,NULL,29),(730,0,12,NULL,NULL,30),(731,0,12,NULL,NULL,31),(732,0,12,NULL,NULL,32),(733,0,12,NULL,NULL,33),(734,0,12,NULL,NULL,34),(735,0,12,NULL,NULL,35),(736,0,12,NULL,NULL,36),(737,0,12,NULL,NULL,37),(738,0,12,NULL,NULL,38),(739,0,12,NULL,NULL,39),(740,0,12,NULL,NULL,40),(741,0,12,NULL,NULL,41),(742,0,12,NULL,NULL,42),(743,0,12,NULL,NULL,43),(744,0,12,NULL,NULL,44),(745,0,12,NULL,NULL,45),(746,0,12,NULL,NULL,46),(747,0,12,NULL,NULL,47),(748,0,12,NULL,NULL,48),(749,0,12,NULL,NULL,49),(750,0,12,NULL,NULL,50),(751,0,12,NULL,NULL,51),(752,0,12,NULL,NULL,52),(753,0,12,NULL,NULL,53),(754,0,12,NULL,NULL,54),(755,0,12,NULL,NULL,55),(756,0,12,NULL,NULL,56),(757,0,12,NULL,NULL,57),(758,0,12,NULL,NULL,58),(759,0,12,NULL,NULL,59),(760,0,12,NULL,NULL,60),(761,0,12,NULL,NULL,61),(762,0,12,NULL,NULL,62),(763,0,12,NULL,NULL,63),(764,0,12,NULL,NULL,64),(765,0,12,NULL,NULL,65),(766,0,12,NULL,NULL,66),(767,0,12,NULL,NULL,67),(768,0,12,NULL,NULL,68),(769,0,12,NULL,NULL,69),(770,0,12,NULL,NULL,70),(771,0,12,NULL,NULL,71),(772,0,12,NULL,NULL,72),(773,0,12,NULL,NULL,73),(774,0,12,NULL,NULL,74),(775,0,12,NULL,NULL,75),(776,0,12,NULL,NULL,76),(777,0,12,NULL,NULL,77),(778,0,12,NULL,NULL,78),(779,0,12,NULL,NULL,79),(780,0,12,NULL,NULL,80),(781,0,12,NULL,NULL,81),(782,0,12,NULL,NULL,82),(783,0,12,NULL,NULL,83),(784,0,12,NULL,NULL,84),(785,0,12,NULL,NULL,85),(786,0,12,NULL,NULL,86),(787,0,12,NULL,NULL,87),(788,0,12,NULL,NULL,88),(789,0,12,NULL,NULL,89),(790,0,12,NULL,NULL,90),(791,0,12,NULL,NULL,91),(792,0,12,NULL,NULL,92),(793,0,12,NULL,NULL,93),(794,0,12,NULL,NULL,94),(795,0,12,NULL,NULL,95),(796,0,12,NULL,NULL,96),(797,0,12,NULL,NULL,97),(798,0,12,NULL,NULL,98),(799,0,12,NULL,NULL,99),(800,0,12,NULL,NULL,100),(801,0,13,NULL,NULL,1),(802,0,13,NULL,NULL,2),(803,0,13,NULL,NULL,3),(804,0,13,NULL,NULL,4),(805,0,13,NULL,NULL,5),(806,0,13,NULL,NULL,6),(807,0,13,NULL,NULL,7),(808,0,13,NULL,NULL,8),(809,0,13,NULL,NULL,9),(810,0,13,NULL,NULL,10),(811,0,13,NULL,NULL,11),(812,0,13,NULL,NULL,12),(813,0,13,NULL,NULL,13),(814,0,13,NULL,NULL,14),(815,0,13,NULL,NULL,15),(816,0,13,NULL,NULL,16),(817,0,13,NULL,NULL,17),(818,0,13,NULL,NULL,18),(819,0,13,NULL,NULL,19),(820,0,13,NULL,NULL,20),(821,0,13,NULL,NULL,21),(822,0,13,NULL,NULL,22),(823,0,13,NULL,NULL,23),(824,0,13,NULL,NULL,24),(825,0,13,NULL,NULL,25),(826,0,13,NULL,NULL,26),(827,0,13,NULL,NULL,27),(828,0,13,NULL,NULL,28),(829,0,13,NULL,NULL,29),(830,0,13,NULL,NULL,30),(831,0,13,NULL,NULL,31),(832,0,13,NULL,NULL,32),(833,0,13,NULL,NULL,33),(834,0,13,NULL,NULL,34),(835,0,13,NULL,NULL,35),(836,0,13,NULL,NULL,36),(837,0,13,NULL,NULL,37),(838,0,13,NULL,NULL,38),(839,0,13,NULL,NULL,39),(840,0,13,NULL,NULL,40),(841,0,13,NULL,NULL,41),(842,0,13,NULL,NULL,42),(843,0,13,NULL,NULL,43),(844,0,13,NULL,NULL,44),(845,0,13,NULL,NULL,45),(846,0,13,NULL,NULL,46),(847,0,13,NULL,NULL,47),(848,0,13,NULL,NULL,48),(849,0,13,NULL,NULL,49),(850,0,13,NULL,NULL,50),(851,0,13,NULL,NULL,51),(852,0,13,NULL,NULL,52),(853,0,13,NULL,NULL,53),(854,0,13,NULL,NULL,54),(855,0,13,NULL,NULL,55),(856,0,13,NULL,NULL,56),(857,0,13,NULL,NULL,57),(858,0,13,NULL,NULL,58),(859,0,13,NULL,NULL,59),(860,0,13,NULL,NULL,60),(861,0,13,NULL,NULL,61),(862,0,13,NULL,NULL,62),(863,0,13,NULL,NULL,63),(864,0,13,NULL,NULL,64),(865,0,13,NULL,NULL,65),(866,0,13,NULL,NULL,66),(867,0,13,NULL,NULL,67),(868,0,13,NULL,NULL,68),(869,0,13,NULL,NULL,69),(870,0,13,NULL,NULL,70),(871,0,13,NULL,NULL,71),(872,0,13,NULL,NULL,72),(873,0,13,NULL,NULL,73),(874,0,13,NULL,NULL,74),(875,0,13,NULL,NULL,75),(876,0,13,NULL,NULL,76),(877,0,13,NULL,NULL,77),(878,0,13,NULL,NULL,78),(879,0,13,NULL,NULL,79),(880,0,13,NULL,NULL,80),(881,0,13,NULL,NULL,81),(882,0,13,NULL,NULL,82),(883,0,13,NULL,NULL,83),(884,0,13,NULL,NULL,84),(885,0,13,NULL,NULL,85),(886,0,13,NULL,NULL,86),(887,0,13,NULL,NULL,87),(888,0,13,NULL,NULL,88),(889,0,13,NULL,NULL,89),(890,0,13,NULL,NULL,90),(891,0,13,NULL,NULL,91),(892,0,13,NULL,NULL,92),(893,0,13,NULL,NULL,93),(894,0,13,NULL,NULL,94),(895,0,13,NULL,NULL,95),(896,0,13,NULL,NULL,96),(897,0,13,NULL,NULL,97),(898,0,13,NULL,NULL,98),(899,0,13,NULL,NULL,99),(900,0,13,NULL,NULL,100),(901,1,14,NULL,NULL,1),(902,1,14,NULL,NULL,2),(903,1,14,NULL,NULL,3),(904,2,14,NULL,NULL,4),(905,1,14,NULL,NULL,5),(906,2,14,NULL,NULL,6),(907,1,14,NULL,NULL,7),(908,0,14,NULL,NULL,8),(909,0,14,NULL,NULL,9),(910,0,14,NULL,NULL,10),(911,0,14,NULL,NULL,11),(912,0,14,NULL,NULL,12),(913,0,14,NULL,NULL,13),(914,0,14,NULL,NULL,14),(915,0,14,NULL,NULL,15),(916,0,14,NULL,NULL,16),(917,0,14,NULL,NULL,17),(918,0,14,NULL,NULL,18),(919,0,14,NULL,NULL,19),(920,0,14,NULL,NULL,20),(921,0,14,NULL,NULL,21),(922,0,14,NULL,NULL,22),(923,0,14,NULL,NULL,23),(924,0,14,NULL,NULL,24),(925,0,14,NULL,NULL,25),(926,0,14,NULL,NULL,26),(927,0,14,NULL,NULL,27),(928,0,14,NULL,NULL,28),(929,0,14,NULL,NULL,29),(930,0,14,NULL,NULL,30),(931,0,14,NULL,NULL,31),(932,0,14,NULL,NULL,32),(933,0,14,NULL,NULL,33),(934,0,14,NULL,NULL,34),(935,0,14,NULL,NULL,35),(936,0,14,NULL,NULL,36),(937,0,14,NULL,NULL,37),(938,0,14,NULL,NULL,38),(939,0,14,NULL,NULL,39),(940,0,14,NULL,NULL,40),(941,0,14,NULL,NULL,41),(942,0,14,NULL,NULL,42),(943,0,14,NULL,NULL,43),(944,0,14,NULL,NULL,44),(945,0,14,NULL,NULL,45),(946,0,14,NULL,NULL,46),(947,0,14,NULL,NULL,47),(948,0,14,NULL,NULL,48),(949,0,14,NULL,NULL,49),(950,0,14,NULL,NULL,50),(951,0,14,NULL,NULL,51),(952,0,14,NULL,NULL,52),(953,0,14,NULL,NULL,53),(954,0,14,NULL,NULL,54),(955,0,14,NULL,NULL,55),(956,0,14,NULL,NULL,56),(957,0,14,NULL,NULL,57),(958,0,14,NULL,NULL,58),(959,0,14,NULL,NULL,59),(960,0,14,NULL,NULL,60),(961,0,14,NULL,NULL,61),(962,0,14,NULL,NULL,62),(963,0,14,NULL,NULL,63),(964,0,14,NULL,NULL,64),(965,0,14,NULL,NULL,65),(966,0,14,NULL,NULL,66),(967,0,14,NULL,NULL,67),(968,0,14,NULL,NULL,68),(969,0,14,NULL,NULL,69),(970,0,14,NULL,NULL,70),(971,0,14,NULL,NULL,71),(972,0,14,NULL,NULL,72),(973,0,14,NULL,NULL,73),(974,0,14,NULL,NULL,74),(975,0,14,NULL,NULL,75),(976,0,14,NULL,NULL,76),(977,0,14,NULL,NULL,77),(978,0,14,NULL,NULL,78),(979,0,14,NULL,NULL,79),(980,0,14,NULL,NULL,80),(981,0,14,NULL,NULL,81),(982,0,14,NULL,NULL,82),(983,0,14,NULL,NULL,83),(984,0,14,NULL,NULL,84),(985,0,14,NULL,NULL,85),(986,0,14,NULL,NULL,86),(987,0,14,NULL,NULL,87),(988,0,14,NULL,NULL,88),(989,0,14,NULL,NULL,89),(990,0,14,NULL,NULL,90),(991,0,14,NULL,NULL,91),(992,0,14,NULL,NULL,92),(993,0,14,NULL,NULL,93),(994,0,14,NULL,NULL,94),(995,0,14,NULL,NULL,95),(996,0,14,NULL,NULL,96),(997,0,14,NULL,NULL,97),(998,0,14,NULL,NULL,98),(999,0,14,NULL,NULL,99),(1000,0,14,NULL,NULL,100),(1001,0,15,NULL,NULL,1),(1002,0,15,NULL,NULL,2),(1003,0,15,NULL,NULL,3),(1004,0,15,NULL,NULL,4),(1005,0,15,NULL,NULL,5),(1006,0,15,NULL,NULL,6),(1007,0,15,NULL,NULL,7),(1008,0,15,NULL,NULL,8),(1009,0,15,NULL,NULL,9),(1010,0,15,NULL,NULL,10),(1011,0,15,NULL,NULL,11),(1012,0,15,NULL,NULL,12),(1013,0,15,NULL,NULL,13),(1014,0,15,NULL,NULL,14),(1015,0,15,NULL,NULL,15),(1016,0,15,NULL,NULL,16),(1017,0,15,NULL,NULL,17),(1018,0,15,NULL,NULL,18),(1019,0,15,NULL,NULL,19),(1020,0,15,NULL,NULL,20),(1021,0,15,NULL,NULL,21),(1022,0,15,NULL,NULL,22),(1023,0,15,NULL,NULL,23),(1024,0,15,NULL,NULL,24),(1025,0,15,NULL,NULL,25),(1026,0,15,NULL,NULL,26),(1027,0,15,NULL,NULL,27),(1028,0,15,NULL,NULL,28),(1029,0,15,NULL,NULL,29),(1030,0,15,NULL,NULL,30),(1031,0,15,NULL,NULL,31),(1032,0,15,NULL,NULL,32),(1033,0,15,NULL,NULL,33),(1034,0,15,NULL,NULL,34),(1035,0,15,NULL,NULL,35),(1036,0,15,NULL,NULL,36),(1037,0,15,NULL,NULL,37),(1038,0,15,NULL,NULL,38),(1039,0,15,NULL,NULL,39),(1040,0,15,NULL,NULL,40),(1041,0,15,NULL,NULL,41),(1042,0,15,NULL,NULL,42),(1043,0,15,NULL,NULL,43),(1044,0,15,NULL,NULL,44),(1045,0,15,NULL,NULL,45),(1046,0,15,NULL,NULL,46),(1047,0,15,NULL,NULL,47),(1048,0,15,NULL,NULL,48),(1049,0,15,NULL,NULL,49),(1050,0,15,NULL,NULL,50),(1051,0,15,NULL,NULL,51),(1052,0,15,NULL,NULL,52),(1053,0,15,NULL,NULL,53),(1054,0,15,NULL,NULL,54),(1055,0,15,NULL,NULL,55),(1056,0,15,NULL,NULL,56),(1057,0,15,NULL,NULL,57),(1058,0,15,NULL,NULL,58),(1059,0,15,NULL,NULL,59),(1060,0,15,NULL,NULL,60),(1061,0,15,NULL,NULL,61),(1062,0,15,NULL,NULL,62),(1063,0,15,NULL,NULL,63),(1064,0,15,NULL,NULL,64),(1065,0,15,NULL,NULL,65),(1066,0,15,NULL,NULL,66),(1067,0,15,NULL,NULL,67),(1068,0,15,NULL,NULL,68),(1069,0,15,NULL,NULL,69),(1070,0,15,NULL,NULL,70),(1071,0,15,NULL,NULL,71),(1072,0,15,NULL,NULL,72),(1073,0,15,NULL,NULL,73),(1074,0,15,NULL,NULL,74),(1075,0,15,NULL,NULL,75),(1076,0,15,NULL,NULL,76),(1077,0,15,NULL,NULL,77),(1078,0,15,NULL,NULL,78),(1079,0,15,NULL,NULL,79),(1080,0,15,NULL,NULL,80),(1081,0,15,NULL,NULL,81),(1082,0,15,NULL,NULL,82),(1083,0,15,NULL,NULL,83),(1084,0,15,NULL,NULL,84),(1085,0,15,NULL,NULL,85),(1086,0,15,NULL,NULL,86),(1087,0,15,NULL,NULL,87),(1088,0,15,NULL,NULL,88),(1089,0,15,NULL,NULL,89),(1090,0,15,NULL,NULL,90),(1091,0,15,NULL,NULL,91),(1092,0,15,NULL,NULL,92),(1093,0,15,NULL,NULL,93),(1094,0,15,NULL,NULL,94),(1095,0,15,NULL,NULL,95),(1096,0,15,NULL,NULL,96),(1097,0,15,NULL,NULL,97),(1098,0,15,NULL,NULL,98),(1099,0,15,NULL,NULL,99),(1100,0,15,NULL,NULL,100),(1101,2,16,NULL,NULL,1),(1102,0,16,NULL,NULL,2),(1103,0,16,NULL,NULL,3),(1104,0,16,NULL,NULL,4),(1105,0,16,NULL,NULL,5),(1106,0,16,NULL,NULL,6),(1107,0,16,NULL,NULL,7),(1108,0,16,NULL,NULL,8),(1109,0,16,NULL,NULL,9),(1110,0,16,NULL,NULL,10),(1111,0,16,NULL,NULL,11),(1112,0,16,NULL,NULL,12),(1113,0,16,NULL,NULL,13),(1114,0,16,NULL,NULL,14),(1115,0,16,NULL,NULL,15),(1116,0,16,NULL,NULL,16),(1117,0,16,NULL,NULL,17),(1118,0,16,NULL,NULL,18),(1119,0,16,NULL,NULL,19),(1120,0,16,NULL,NULL,20),(1121,0,16,NULL,NULL,21),(1122,0,16,NULL,NULL,22),(1123,0,16,NULL,NULL,23),(1124,0,16,NULL,NULL,24),(1125,0,16,NULL,NULL,25),(1126,0,16,NULL,NULL,26),(1127,0,16,NULL,NULL,27),(1128,0,16,NULL,NULL,28),(1129,0,16,NULL,NULL,29),(1130,0,16,NULL,NULL,30),(1131,0,16,NULL,NULL,31),(1132,0,16,NULL,NULL,32),(1133,0,16,NULL,NULL,33),(1134,0,16,NULL,NULL,34),(1135,0,16,NULL,NULL,35),(1136,0,16,NULL,NULL,36),(1137,0,16,NULL,NULL,37),(1138,0,16,NULL,NULL,38),(1139,0,16,NULL,NULL,39),(1140,0,16,NULL,NULL,40),(1141,0,16,NULL,NULL,41),(1142,0,16,NULL,NULL,42),(1143,0,16,NULL,NULL,43),(1144,0,16,NULL,NULL,44),(1145,0,16,NULL,NULL,45),(1146,0,16,NULL,NULL,46),(1147,0,16,NULL,NULL,47),(1148,0,16,NULL,NULL,48),(1149,0,16,NULL,NULL,49),(1150,0,16,NULL,NULL,50),(1151,0,16,NULL,NULL,51),(1152,0,16,NULL,NULL,52),(1153,0,16,NULL,NULL,53),(1154,0,16,NULL,NULL,54),(1155,0,16,NULL,NULL,55),(1156,0,16,NULL,NULL,56),(1157,0,16,NULL,NULL,57),(1158,0,16,NULL,NULL,58),(1159,0,16,NULL,NULL,59),(1160,0,16,NULL,NULL,60),(1161,0,16,NULL,NULL,61),(1162,0,16,NULL,NULL,62),(1163,0,16,NULL,NULL,63),(1164,0,16,NULL,NULL,64),(1165,0,16,NULL,NULL,65),(1166,0,16,NULL,NULL,66),(1167,0,16,NULL,NULL,67),(1168,0,16,NULL,NULL,68),(1169,0,16,NULL,NULL,69),(1170,0,16,NULL,NULL,70),(1171,0,16,NULL,NULL,71),(1172,0,16,NULL,NULL,72),(1173,0,16,NULL,NULL,73),(1174,0,16,NULL,NULL,74),(1175,0,16,NULL,NULL,75),(1176,0,16,NULL,NULL,76),(1177,0,16,NULL,NULL,77),(1178,0,16,NULL,NULL,78),(1179,0,16,NULL,NULL,79),(1180,0,16,NULL,NULL,80),(1181,0,16,NULL,NULL,81),(1182,0,16,NULL,NULL,82),(1183,0,16,NULL,NULL,83),(1184,0,16,NULL,NULL,84),(1185,0,16,NULL,NULL,85),(1186,0,16,NULL,NULL,86),(1187,0,16,NULL,NULL,87),(1188,0,16,NULL,NULL,88),(1189,0,16,NULL,NULL,89),(1190,0,16,NULL,NULL,90),(1191,0,16,NULL,NULL,91),(1192,0,16,NULL,NULL,92),(1193,0,16,NULL,NULL,93),(1194,0,16,NULL,NULL,94),(1195,0,16,NULL,NULL,95),(1196,0,16,NULL,NULL,96),(1197,0,16,NULL,NULL,97),(1198,0,16,NULL,NULL,98),(1199,0,16,NULL,NULL,99),(1200,0,16,NULL,NULL,100),(1201,0,17,NULL,NULL,1),(1202,0,17,NULL,NULL,2),(1203,0,17,NULL,NULL,3),(1204,0,17,NULL,NULL,4),(1205,0,17,NULL,NULL,5),(1206,0,17,NULL,NULL,6),(1207,0,17,NULL,NULL,7),(1208,0,17,NULL,NULL,8),(1209,0,17,NULL,NULL,9),(1210,0,17,NULL,NULL,10),(1211,0,17,NULL,NULL,11),(1212,0,17,NULL,NULL,12),(1213,0,17,NULL,NULL,13),(1214,0,17,NULL,NULL,14),(1215,0,17,NULL,NULL,15),(1216,0,17,NULL,NULL,16),(1217,0,17,NULL,NULL,17),(1218,0,17,NULL,NULL,18),(1219,0,17,NULL,NULL,19),(1220,0,17,NULL,NULL,20),(1221,0,17,NULL,NULL,21),(1222,0,17,NULL,NULL,22),(1223,0,17,NULL,NULL,23),(1224,0,17,NULL,NULL,24),(1225,0,17,NULL,NULL,25),(1226,0,17,NULL,NULL,26),(1227,0,17,NULL,NULL,27),(1228,0,17,NULL,NULL,28),(1229,0,17,NULL,NULL,29),(1230,0,17,NULL,NULL,30),(1231,0,17,NULL,NULL,31),(1232,0,17,NULL,NULL,32),(1233,0,17,NULL,NULL,33),(1234,0,17,NULL,NULL,34),(1235,0,17,NULL,NULL,35),(1236,0,17,NULL,NULL,36),(1237,0,17,NULL,NULL,37),(1238,0,17,NULL,NULL,38),(1239,0,17,NULL,NULL,39),(1240,0,17,NULL,NULL,40),(1241,0,17,NULL,NULL,41),(1242,0,17,NULL,NULL,42),(1243,0,17,NULL,NULL,43),(1244,0,17,NULL,NULL,44),(1245,0,17,NULL,NULL,45),(1246,0,17,NULL,NULL,46),(1247,0,17,NULL,NULL,47),(1248,0,17,NULL,NULL,48),(1249,0,17,NULL,NULL,49),(1250,0,17,NULL,NULL,50),(1251,0,17,NULL,NULL,51),(1252,0,17,NULL,NULL,52),(1253,0,17,NULL,NULL,53),(1254,0,17,NULL,NULL,54),(1255,0,17,NULL,NULL,55),(1256,0,17,NULL,NULL,56),(1257,0,17,NULL,NULL,57),(1258,0,17,NULL,NULL,58),(1259,0,17,NULL,NULL,59),(1260,0,17,NULL,NULL,60),(1261,0,17,NULL,NULL,61),(1262,0,17,NULL,NULL,62),(1263,0,17,NULL,NULL,63),(1264,0,17,NULL,NULL,64),(1265,0,17,NULL,NULL,65),(1266,0,17,NULL,NULL,66),(1267,0,17,NULL,NULL,67),(1268,0,17,NULL,NULL,68),(1269,0,17,NULL,NULL,69),(1270,0,17,NULL,NULL,70),(1271,0,17,NULL,NULL,71),(1272,0,17,NULL,NULL,72),(1273,0,17,NULL,NULL,73),(1274,0,17,NULL,NULL,74),(1275,0,17,NULL,NULL,75),(1276,0,17,NULL,NULL,76),(1277,0,17,NULL,NULL,77),(1278,0,17,NULL,NULL,78),(1279,0,17,NULL,NULL,79),(1280,0,17,NULL,NULL,80),(1281,0,17,NULL,NULL,81),(1282,0,17,NULL,NULL,82),(1283,0,17,NULL,NULL,83),(1284,0,17,NULL,NULL,84),(1285,0,17,NULL,NULL,85),(1286,0,17,NULL,NULL,86),(1287,0,17,NULL,NULL,87),(1288,0,17,NULL,NULL,88),(1289,0,17,NULL,NULL,89),(1290,0,17,NULL,NULL,90),(1291,0,17,NULL,NULL,91),(1292,0,17,NULL,NULL,92),(1293,0,17,NULL,NULL,93),(1294,0,17,NULL,NULL,94),(1295,0,17,NULL,NULL,95),(1296,0,17,NULL,NULL,96),(1297,0,17,NULL,NULL,97),(1298,0,17,NULL,NULL,98),(1299,0,17,NULL,NULL,99),(1300,0,17,NULL,NULL,100),(1301,0,18,NULL,NULL,1),(1302,0,18,NULL,NULL,2),(1303,0,18,NULL,NULL,3),(1304,0,18,NULL,NULL,4),(1305,0,18,NULL,NULL,5),(1306,0,18,NULL,NULL,6),(1307,0,18,NULL,NULL,7),(1308,0,18,NULL,NULL,8),(1309,0,18,NULL,NULL,9),(1310,0,18,NULL,NULL,10),(1311,0,18,NULL,NULL,11),(1312,0,18,NULL,NULL,12),(1313,0,18,NULL,NULL,13),(1314,0,18,NULL,NULL,14),(1315,0,18,NULL,NULL,15),(1316,0,18,NULL,NULL,16),(1317,0,18,NULL,NULL,17),(1318,0,18,NULL,NULL,18),(1319,0,18,NULL,NULL,19),(1320,0,18,NULL,NULL,20),(1321,0,18,NULL,NULL,21),(1322,0,18,NULL,NULL,22),(1323,0,18,NULL,NULL,23),(1324,0,18,NULL,NULL,24),(1325,0,18,NULL,NULL,25),(1326,0,18,NULL,NULL,26),(1327,0,18,NULL,NULL,27),(1328,0,18,NULL,NULL,28),(1329,0,18,NULL,NULL,29),(1330,0,18,NULL,NULL,30),(1331,0,18,NULL,NULL,31),(1332,0,18,NULL,NULL,32),(1333,0,18,NULL,NULL,33),(1334,0,18,NULL,NULL,34),(1335,0,18,NULL,NULL,35),(1336,0,18,NULL,NULL,36),(1337,0,18,NULL,NULL,37),(1338,0,18,NULL,NULL,38),(1339,0,18,NULL,NULL,39),(1340,0,18,NULL,NULL,40),(1341,0,18,NULL,NULL,41),(1342,0,18,NULL,NULL,42),(1343,0,18,NULL,NULL,43),(1344,0,18,NULL,NULL,44),(1345,0,18,NULL,NULL,45),(1346,0,18,NULL,NULL,46),(1347,0,18,NULL,NULL,47),(1348,0,18,NULL,NULL,48),(1349,0,18,NULL,NULL,49),(1350,0,18,NULL,NULL,50),(1351,0,18,NULL,NULL,51),(1352,0,18,NULL,NULL,52),(1353,0,18,NULL,NULL,53),(1354,0,18,NULL,NULL,54),(1355,0,18,NULL,NULL,55),(1356,0,18,NULL,NULL,56),(1357,0,18,NULL,NULL,57),(1358,0,18,NULL,NULL,58),(1359,0,18,NULL,NULL,59),(1360,0,18,NULL,NULL,60),(1361,0,18,NULL,NULL,61),(1362,0,18,NULL,NULL,62),(1363,0,18,NULL,NULL,63),(1364,0,18,NULL,NULL,64),(1365,0,18,NULL,NULL,65),(1366,0,18,NULL,NULL,66),(1367,0,18,NULL,NULL,67),(1368,0,18,NULL,NULL,68),(1369,0,18,NULL,NULL,69),(1370,0,18,NULL,NULL,70),(1371,0,18,NULL,NULL,71),(1372,0,18,NULL,NULL,72),(1373,0,18,NULL,NULL,73),(1374,0,18,NULL,NULL,74),(1375,0,18,NULL,NULL,75),(1376,0,18,NULL,NULL,76),(1377,0,18,NULL,NULL,77),(1378,0,18,NULL,NULL,78),(1379,0,18,NULL,NULL,79),(1380,0,18,NULL,NULL,80),(1381,0,18,NULL,NULL,81),(1382,0,18,NULL,NULL,82),(1383,0,18,NULL,NULL,83),(1384,0,18,NULL,NULL,84),(1385,0,18,NULL,NULL,85),(1386,0,18,NULL,NULL,86),(1387,0,18,NULL,NULL,87),(1388,0,18,NULL,NULL,88),(1389,0,18,NULL,NULL,89),(1390,0,18,NULL,NULL,90),(1391,0,18,NULL,NULL,91),(1392,0,18,NULL,NULL,92),(1393,0,18,NULL,NULL,93),(1394,0,18,NULL,NULL,94),(1395,0,18,NULL,NULL,95),(1396,0,18,NULL,NULL,96),(1397,0,18,NULL,NULL,97),(1398,0,18,NULL,NULL,98),(1399,0,18,NULL,NULL,99),(1400,0,18,NULL,NULL,100),(1401,0,19,NULL,NULL,1),(1402,0,19,NULL,NULL,2),(1403,0,19,NULL,NULL,3),(1404,0,19,NULL,NULL,4),(1405,0,19,NULL,NULL,5),(1406,0,19,NULL,NULL,6),(1407,0,19,NULL,NULL,7),(1408,0,19,NULL,NULL,8),(1409,0,19,NULL,NULL,9),(1410,0,19,NULL,NULL,10),(1411,0,19,NULL,NULL,11),(1412,0,19,NULL,NULL,12),(1413,0,19,NULL,NULL,13),(1414,0,19,NULL,NULL,14),(1415,0,19,NULL,NULL,15),(1416,0,19,NULL,NULL,16),(1417,0,19,NULL,NULL,17),(1418,0,19,NULL,NULL,18),(1419,0,19,NULL,NULL,19),(1420,0,19,NULL,NULL,20),(1421,0,19,NULL,NULL,21),(1422,0,19,NULL,NULL,22),(1423,0,19,NULL,NULL,23),(1424,0,19,NULL,NULL,24),(1425,0,19,NULL,NULL,25),(1426,0,19,NULL,NULL,26),(1427,0,19,NULL,NULL,27),(1428,0,19,NULL,NULL,28),(1429,0,19,NULL,NULL,29),(1430,0,19,NULL,NULL,30),(1431,0,19,NULL,NULL,31),(1432,0,19,NULL,NULL,32),(1433,0,19,NULL,NULL,33),(1434,0,19,NULL,NULL,34),(1435,0,19,NULL,NULL,35),(1436,0,19,NULL,NULL,36),(1437,0,19,NULL,NULL,37),(1438,0,19,NULL,NULL,38),(1439,0,19,NULL,NULL,39),(1440,0,19,NULL,NULL,40),(1441,0,19,NULL,NULL,41),(1442,0,19,NULL,NULL,42),(1443,0,19,NULL,NULL,43),(1444,0,19,NULL,NULL,44),(1445,0,19,NULL,NULL,45),(1446,0,19,NULL,NULL,46),(1447,0,19,NULL,NULL,47),(1448,0,19,NULL,NULL,48),(1449,0,19,NULL,NULL,49),(1450,0,19,NULL,NULL,50),(1451,0,19,NULL,NULL,51),(1452,0,19,NULL,NULL,52),(1453,0,19,NULL,NULL,53),(1454,0,19,NULL,NULL,54),(1455,0,19,NULL,NULL,55),(1456,0,19,NULL,NULL,56),(1457,0,19,NULL,NULL,57),(1458,0,19,NULL,NULL,58),(1459,0,19,NULL,NULL,59),(1460,0,19,NULL,NULL,60),(1461,0,19,NULL,NULL,61),(1462,0,19,NULL,NULL,62),(1463,0,19,NULL,NULL,63),(1464,0,19,NULL,NULL,64),(1465,0,19,NULL,NULL,65),(1466,0,19,NULL,NULL,66),(1467,0,19,NULL,NULL,67),(1468,0,19,NULL,NULL,68),(1469,0,19,NULL,NULL,69),(1470,0,19,NULL,NULL,70),(1471,0,19,NULL,NULL,71),(1472,0,19,NULL,NULL,72),(1473,0,19,NULL,NULL,73),(1474,0,19,NULL,NULL,74),(1475,0,19,NULL,NULL,75),(1476,0,19,NULL,NULL,76),(1477,0,19,NULL,NULL,77),(1478,0,19,NULL,NULL,78),(1479,0,19,NULL,NULL,79),(1480,0,19,NULL,NULL,80),(1481,0,19,NULL,NULL,81),(1482,0,19,NULL,NULL,82),(1483,0,19,NULL,NULL,83),(1484,0,19,NULL,NULL,84),(1485,0,19,NULL,NULL,85),(1486,0,19,NULL,NULL,86),(1487,0,19,NULL,NULL,87),(1488,0,19,NULL,NULL,88),(1489,0,19,NULL,NULL,89),(1490,0,19,NULL,NULL,90),(1491,0,19,NULL,NULL,91),(1492,0,19,NULL,NULL,92),(1493,0,19,NULL,NULL,93),(1494,0,19,NULL,NULL,94),(1495,0,19,NULL,NULL,95),(1496,0,19,NULL,NULL,96),(1497,0,19,NULL,NULL,97),(1498,0,19,NULL,NULL,98),(1499,0,19,NULL,NULL,99),(1500,0,19,NULL,NULL,100),(1501,0,20,NULL,NULL,1),(1502,0,20,NULL,NULL,2),(1503,0,20,NULL,NULL,3),(1504,0,20,NULL,NULL,4),(1505,0,20,NULL,NULL,5),(1506,0,20,NULL,NULL,6),(1507,0,20,NULL,NULL,7),(1508,0,20,NULL,NULL,8),(1509,0,20,NULL,NULL,9),(1510,0,20,NULL,NULL,10),(1511,0,20,NULL,NULL,11),(1512,0,20,NULL,NULL,12),(1513,0,20,NULL,NULL,13),(1514,0,20,NULL,NULL,14),(1515,0,20,NULL,NULL,15),(1516,0,20,NULL,NULL,16),(1517,0,20,NULL,NULL,17),(1518,0,20,NULL,NULL,18),(1519,0,20,NULL,NULL,19),(1520,0,20,NULL,NULL,20),(1521,0,20,NULL,NULL,21),(1522,0,20,NULL,NULL,22),(1523,0,20,NULL,NULL,23),(1524,0,20,NULL,NULL,24),(1525,0,20,NULL,NULL,25),(1526,0,20,NULL,NULL,26),(1527,0,20,NULL,NULL,27),(1528,0,20,NULL,NULL,28),(1529,0,20,NULL,NULL,29),(1530,0,20,NULL,NULL,30),(1531,0,20,NULL,NULL,31),(1532,0,20,NULL,NULL,32),(1533,0,20,NULL,NULL,33),(1534,0,20,NULL,NULL,34),(1535,0,20,NULL,NULL,35),(1536,0,20,NULL,NULL,36),(1537,0,20,NULL,NULL,37),(1538,0,20,NULL,NULL,38),(1539,0,20,NULL,NULL,39),(1540,0,20,NULL,NULL,40),(1541,0,20,NULL,NULL,41),(1542,0,20,NULL,NULL,42),(1543,0,20,NULL,NULL,43),(1544,0,20,NULL,NULL,44),(1545,0,20,NULL,NULL,45),(1546,0,20,NULL,NULL,46),(1547,0,20,NULL,NULL,47),(1548,0,20,NULL,NULL,48),(1549,0,20,NULL,NULL,49),(1550,0,20,NULL,NULL,50),(1551,0,20,NULL,NULL,51),(1552,0,20,NULL,NULL,52),(1553,0,20,NULL,NULL,53),(1554,0,20,NULL,NULL,54),(1555,0,20,NULL,NULL,55),(1556,0,20,NULL,NULL,56),(1557,0,20,NULL,NULL,57),(1558,0,20,NULL,NULL,58),(1559,0,20,NULL,NULL,59),(1560,0,20,NULL,NULL,60),(1561,0,20,NULL,NULL,61),(1562,0,20,NULL,NULL,62),(1563,0,20,NULL,NULL,63),(1564,0,20,NULL,NULL,64),(1565,0,20,NULL,NULL,65),(1566,0,20,NULL,NULL,66),(1567,0,20,NULL,NULL,67),(1568,0,20,NULL,NULL,68),(1569,0,20,NULL,NULL,69),(1570,0,20,NULL,NULL,70),(1571,0,20,NULL,NULL,71),(1572,0,20,NULL,NULL,72),(1573,0,20,NULL,NULL,73),(1574,0,20,NULL,NULL,74),(1575,0,20,NULL,NULL,75),(1576,0,20,NULL,NULL,76),(1577,0,20,NULL,NULL,77),(1578,0,20,NULL,NULL,78),(1579,0,20,NULL,NULL,79),(1580,0,20,NULL,NULL,80),(1581,0,20,NULL,NULL,81),(1582,0,20,NULL,NULL,82),(1583,0,20,NULL,NULL,83),(1584,0,20,NULL,NULL,84),(1585,0,20,NULL,NULL,85),(1586,0,20,NULL,NULL,86),(1587,0,20,NULL,NULL,87),(1588,0,20,NULL,NULL,88),(1589,0,20,NULL,NULL,89),(1590,0,20,NULL,NULL,90),(1591,0,20,NULL,NULL,91),(1592,0,20,NULL,NULL,92),(1593,0,20,NULL,NULL,93),(1594,0,20,NULL,NULL,94),(1595,0,20,NULL,NULL,95),(1596,0,20,NULL,NULL,96),(1597,0,20,NULL,NULL,97),(1598,0,20,NULL,NULL,98),(1599,0,20,NULL,NULL,99),(1600,0,20,NULL,NULL,100),(1601,0,21,NULL,NULL,1),(1602,0,21,NULL,NULL,2),(1603,0,21,NULL,NULL,3),(1604,0,21,NULL,NULL,4),(1605,0,21,NULL,NULL,5),(1606,0,21,NULL,NULL,6),(1607,0,21,NULL,NULL,7),(1608,0,21,NULL,NULL,8),(1609,0,21,NULL,NULL,9),(1610,0,21,NULL,NULL,10),(1611,0,21,NULL,NULL,11),(1612,0,21,NULL,NULL,12),(1613,0,21,NULL,NULL,13),(1614,0,21,NULL,NULL,14),(1615,0,21,NULL,NULL,15),(1616,0,21,NULL,NULL,16),(1617,0,21,NULL,NULL,17),(1618,0,21,NULL,NULL,18),(1619,0,21,NULL,NULL,19),(1620,0,21,NULL,NULL,20),(1621,0,21,NULL,NULL,21),(1622,0,21,NULL,NULL,22),(1623,0,21,NULL,NULL,23),(1624,0,21,NULL,NULL,24),(1625,0,21,NULL,NULL,25),(1626,0,21,NULL,NULL,26),(1627,0,21,NULL,NULL,27),(1628,0,21,NULL,NULL,28),(1629,0,21,NULL,NULL,29),(1630,0,21,NULL,NULL,30),(1631,0,21,NULL,NULL,31),(1632,0,21,NULL,NULL,32),(1633,0,21,NULL,NULL,33),(1634,0,21,NULL,NULL,34),(1635,0,21,NULL,NULL,35),(1636,0,21,NULL,NULL,36),(1637,0,21,NULL,NULL,37),(1638,0,21,NULL,NULL,38),(1639,0,21,NULL,NULL,39),(1640,0,21,NULL,NULL,40),(1641,0,21,NULL,NULL,41),(1642,0,21,NULL,NULL,42),(1643,0,21,NULL,NULL,43),(1644,0,21,NULL,NULL,44),(1645,0,21,NULL,NULL,45),(1646,0,21,NULL,NULL,46),(1647,0,21,NULL,NULL,47),(1648,0,21,NULL,NULL,48),(1649,0,21,NULL,NULL,49),(1650,0,21,NULL,NULL,50),(1651,0,21,NULL,NULL,51),(1652,0,21,NULL,NULL,52),(1653,0,21,NULL,NULL,53),(1654,0,21,NULL,NULL,54),(1655,0,21,NULL,NULL,55),(1656,0,21,NULL,NULL,56),(1657,0,21,NULL,NULL,57),(1658,0,21,NULL,NULL,58),(1659,0,21,NULL,NULL,59),(1660,0,21,NULL,NULL,60),(1661,0,21,NULL,NULL,61),(1662,0,21,NULL,NULL,62),(1663,0,21,NULL,NULL,63),(1664,0,21,NULL,NULL,64),(1665,0,21,NULL,NULL,65),(1666,0,21,NULL,NULL,66),(1667,0,21,NULL,NULL,67),(1668,0,21,NULL,NULL,68),(1669,0,21,NULL,NULL,69),(1670,0,21,NULL,NULL,70),(1671,0,21,NULL,NULL,71),(1672,0,21,NULL,NULL,72),(1673,0,21,NULL,NULL,73),(1674,0,21,NULL,NULL,74),(1675,0,21,NULL,NULL,75),(1676,0,21,NULL,NULL,76),(1677,0,21,NULL,NULL,77),(1678,0,21,NULL,NULL,78),(1679,0,21,NULL,NULL,79),(1680,0,21,NULL,NULL,80),(1681,0,21,NULL,NULL,81),(1682,0,21,NULL,NULL,82),(1683,0,21,NULL,NULL,83),(1684,0,21,NULL,NULL,84),(1685,0,21,NULL,NULL,85),(1686,0,21,NULL,NULL,86),(1687,0,21,NULL,NULL,87),(1688,0,21,NULL,NULL,88),(1689,0,21,NULL,NULL,89),(1690,0,21,NULL,NULL,90),(1691,0,21,NULL,NULL,91),(1692,0,21,NULL,NULL,92),(1693,0,21,NULL,NULL,93),(1694,0,21,NULL,NULL,94),(1695,0,21,NULL,NULL,95),(1696,0,21,NULL,NULL,96),(1697,0,21,NULL,NULL,97),(1698,0,21,NULL,NULL,98),(1699,0,21,NULL,NULL,99),(1700,0,21,NULL,NULL,100),(1701,0,22,NULL,NULL,1),(1702,0,22,NULL,NULL,2),(1703,0,22,NULL,NULL,3),(1704,0,22,NULL,NULL,4),(1705,0,22,NULL,NULL,5),(1706,0,22,NULL,NULL,6),(1707,0,22,NULL,NULL,7),(1708,0,22,NULL,NULL,8),(1709,0,22,NULL,NULL,9),(1710,0,22,NULL,NULL,10),(1711,0,22,NULL,NULL,11),(1712,0,22,NULL,NULL,12),(1713,0,22,NULL,NULL,13),(1714,0,22,NULL,NULL,14),(1715,0,22,NULL,NULL,15),(1716,0,22,NULL,NULL,16),(1717,0,22,NULL,NULL,17),(1718,0,22,NULL,NULL,18),(1719,0,22,NULL,NULL,19),(1720,0,22,NULL,NULL,20),(1721,0,22,NULL,NULL,21),(1722,0,22,NULL,NULL,22),(1723,0,22,NULL,NULL,23),(1724,0,22,NULL,NULL,24),(1725,0,22,NULL,NULL,25),(1726,0,22,NULL,NULL,26),(1727,0,22,NULL,NULL,27),(1728,0,22,NULL,NULL,28),(1729,0,22,NULL,NULL,29),(1730,0,22,NULL,NULL,30),(1731,0,22,NULL,NULL,31),(1732,0,22,NULL,NULL,32),(1733,0,22,NULL,NULL,33),(1734,0,22,NULL,NULL,34),(1735,0,22,NULL,NULL,35),(1736,0,22,NULL,NULL,36),(1737,0,22,NULL,NULL,37),(1738,0,22,NULL,NULL,38),(1739,0,22,NULL,NULL,39),(1740,0,22,NULL,NULL,40),(1741,0,22,NULL,NULL,41),(1742,0,22,NULL,NULL,42),(1743,0,22,NULL,NULL,43),(1744,0,22,NULL,NULL,44),(1745,0,22,NULL,NULL,45),(1746,0,22,NULL,NULL,46),(1747,0,22,NULL,NULL,47),(1748,0,22,NULL,NULL,48),(1749,0,22,NULL,NULL,49),(1750,0,22,NULL,NULL,50),(1751,0,22,NULL,NULL,51),(1752,0,22,NULL,NULL,52),(1753,0,22,NULL,NULL,53),(1754,0,22,NULL,NULL,54),(1755,0,22,NULL,NULL,55),(1756,0,22,NULL,NULL,56),(1757,0,22,NULL,NULL,57),(1758,0,22,NULL,NULL,58),(1759,0,22,NULL,NULL,59),(1760,0,22,NULL,NULL,60),(1761,0,22,NULL,NULL,61),(1762,0,22,NULL,NULL,62),(1763,0,22,NULL,NULL,63),(1764,0,22,NULL,NULL,64),(1765,0,22,NULL,NULL,65),(1766,0,22,NULL,NULL,66),(1767,0,22,NULL,NULL,67),(1768,0,22,NULL,NULL,68),(1769,0,22,NULL,NULL,69),(1770,0,22,NULL,NULL,70),(1771,0,22,NULL,NULL,71),(1772,0,22,NULL,NULL,72),(1773,0,22,NULL,NULL,73),(1774,0,22,NULL,NULL,74),(1775,0,22,NULL,NULL,75),(1776,0,22,NULL,NULL,76),(1777,0,22,NULL,NULL,77),(1778,0,22,NULL,NULL,78),(1779,0,22,NULL,NULL,79),(1780,0,22,NULL,NULL,80),(1781,0,22,NULL,NULL,81),(1782,0,22,NULL,NULL,82),(1783,0,22,NULL,NULL,83),(1784,0,22,NULL,NULL,84),(1785,0,22,NULL,NULL,85),(1786,0,22,NULL,NULL,86),(1787,0,22,NULL,NULL,87),(1788,0,22,NULL,NULL,88),(1789,0,22,NULL,NULL,89),(1790,0,22,NULL,NULL,90),(1791,0,22,NULL,NULL,91),(1792,0,22,NULL,NULL,92),(1793,0,22,NULL,NULL,93),(1794,0,22,NULL,NULL,94),(1795,0,22,NULL,NULL,95),(1796,0,22,NULL,NULL,96),(1797,0,22,NULL,NULL,97),(1798,0,22,NULL,NULL,98),(1799,0,22,NULL,NULL,99),(1800,0,22,NULL,NULL,100),(1801,0,22,NULL,NULL,101),(1802,0,22,NULL,NULL,102),(1803,0,22,NULL,NULL,103),(1804,0,22,NULL,NULL,104),(1805,0,22,NULL,NULL,105),(1806,0,22,NULL,NULL,106),(1807,0,22,NULL,NULL,107),(1808,0,22,NULL,NULL,108),(1809,0,22,NULL,NULL,109),(1810,0,22,NULL,NULL,110),(1811,0,22,NULL,NULL,111),(1812,0,22,NULL,NULL,112),(1813,0,22,NULL,NULL,113),(1814,0,22,NULL,NULL,114),(1815,0,22,NULL,NULL,115),(1816,0,22,NULL,NULL,116),(1817,0,22,NULL,NULL,117),(1818,0,22,NULL,NULL,118),(1819,0,22,NULL,NULL,119),(1820,0,22,NULL,NULL,120),(1821,0,22,NULL,NULL,121),(1822,0,22,NULL,NULL,122),(1823,0,22,NULL,NULL,123),(1824,0,22,NULL,NULL,124),(1825,0,22,NULL,NULL,125),(1826,0,22,NULL,NULL,126),(1827,0,22,NULL,NULL,127),(1828,0,22,NULL,NULL,128),(1829,0,22,NULL,NULL,129),(1830,0,22,NULL,NULL,130),(1831,0,22,NULL,NULL,131),(1832,0,22,NULL,NULL,132),(1833,0,22,NULL,NULL,133),(1834,0,22,NULL,NULL,134),(1835,0,22,NULL,NULL,135),(1836,0,22,NULL,NULL,136),(1837,0,22,NULL,NULL,137),(1838,0,22,NULL,NULL,138),(1839,0,22,NULL,NULL,139),(1840,0,22,NULL,NULL,140),(1841,0,22,NULL,NULL,141),(1842,0,22,NULL,NULL,142),(1843,0,22,NULL,NULL,143),(1844,0,22,NULL,NULL,144),(1845,0,22,NULL,NULL,145),(1846,0,22,NULL,NULL,146),(1847,0,22,NULL,NULL,147),(1848,0,22,NULL,NULL,148),(1849,0,22,NULL,NULL,149),(1850,0,22,NULL,NULL,150),(1851,0,22,NULL,NULL,151),(1852,0,22,NULL,NULL,152),(1853,0,22,NULL,NULL,153),(1854,0,22,NULL,NULL,154),(1855,0,22,NULL,NULL,155),(1856,0,22,NULL,NULL,156),(1857,0,22,NULL,NULL,157),(1858,0,22,NULL,NULL,158),(1859,0,22,NULL,NULL,159),(1860,0,22,NULL,NULL,160),(1861,0,22,NULL,NULL,161),(1862,0,22,NULL,NULL,162),(1863,0,22,NULL,NULL,163),(1864,0,22,NULL,NULL,164),(1865,0,22,NULL,NULL,165),(1866,0,22,NULL,NULL,166),(1867,0,22,NULL,NULL,167),(1868,0,22,NULL,NULL,168),(1869,0,22,NULL,NULL,169),(1870,0,23,NULL,NULL,1),(1871,0,23,NULL,NULL,2),(1872,0,23,NULL,NULL,3),(1873,0,23,NULL,NULL,4),(1874,0,23,NULL,NULL,5),(1875,0,23,NULL,NULL,6),(1876,0,23,NULL,NULL,7),(1877,0,23,NULL,NULL,8),(1878,0,23,NULL,NULL,9),(1879,0,23,NULL,NULL,10),(1880,0,23,NULL,NULL,11),(1881,0,23,NULL,NULL,12),(1882,0,23,NULL,NULL,13),(1883,0,23,NULL,NULL,14),(1884,0,23,NULL,NULL,15),(1885,0,23,NULL,NULL,16),(1886,0,23,NULL,NULL,17),(1887,0,23,NULL,NULL,18),(1888,0,23,NULL,NULL,19),(1889,0,23,NULL,NULL,20),(1890,0,23,NULL,NULL,21),(1891,0,23,NULL,NULL,22),(1892,0,23,NULL,NULL,23),(1893,0,23,NULL,NULL,24),(1894,0,23,NULL,NULL,25),(1895,0,23,NULL,NULL,26),(1896,0,23,NULL,NULL,27),(1897,0,23,NULL,NULL,28),(1898,0,23,NULL,NULL,29),(1899,0,23,NULL,NULL,30),(1900,0,23,NULL,NULL,31),(1901,0,23,NULL,NULL,32),(1902,0,23,NULL,NULL,33),(1903,0,23,NULL,NULL,34),(1904,0,23,NULL,NULL,35),(1905,0,23,NULL,NULL,36),(1906,0,23,NULL,NULL,37),(1907,0,23,NULL,NULL,38),(1908,0,23,NULL,NULL,39),(1909,0,23,NULL,NULL,40),(1910,0,23,NULL,NULL,41),(1911,0,23,NULL,NULL,42),(1912,0,23,NULL,NULL,43),(1913,0,23,NULL,NULL,44),(1914,0,23,NULL,NULL,45),(1915,0,23,NULL,NULL,46),(1916,0,23,NULL,NULL,47),(1917,0,23,NULL,NULL,48),(1918,0,23,NULL,NULL,49),(1919,0,23,NULL,NULL,50),(1920,0,23,NULL,NULL,51),(1921,0,23,NULL,NULL,52),(1922,0,23,NULL,NULL,53),(1923,0,23,NULL,NULL,54),(1924,0,23,NULL,NULL,55),(1925,0,23,NULL,NULL,56),(1926,0,23,NULL,NULL,57),(1927,0,23,NULL,NULL,58),(1928,0,23,NULL,NULL,59),(1929,0,23,NULL,NULL,60),(1930,0,23,NULL,NULL,61),(1931,0,23,NULL,NULL,62),(1932,0,23,NULL,NULL,63),(1933,0,23,NULL,NULL,64),(1934,0,23,NULL,NULL,65),(1935,0,23,NULL,NULL,66),(1936,0,23,NULL,NULL,67),(1937,0,23,NULL,NULL,68),(1938,0,23,NULL,NULL,69),(1939,0,23,NULL,NULL,70),(1940,0,23,NULL,NULL,71),(1941,0,23,NULL,NULL,72),(1942,0,23,NULL,NULL,73),(1943,0,23,NULL,NULL,74),(1944,0,23,NULL,NULL,75),(1945,0,23,NULL,NULL,76),(1946,0,23,NULL,NULL,77),(1947,0,23,NULL,NULL,78),(1948,0,23,NULL,NULL,79),(1949,0,23,NULL,NULL,80),(1950,0,23,NULL,NULL,81),(1951,0,23,NULL,NULL,82),(1952,0,23,NULL,NULL,83),(1953,0,23,NULL,NULL,84),(1954,0,23,NULL,NULL,85),(1955,0,23,NULL,NULL,86),(1956,0,23,NULL,NULL,87),(1957,0,23,NULL,NULL,88),(1958,0,23,NULL,NULL,89),(1959,0,23,NULL,NULL,90),(1960,0,23,NULL,NULL,91),(1961,0,23,NULL,NULL,92),(1962,0,23,NULL,NULL,93),(1963,0,23,NULL,NULL,94),(1964,0,23,NULL,NULL,95),(1965,0,23,NULL,NULL,96),(1966,0,23,NULL,NULL,97),(1967,0,23,NULL,NULL,98),(1968,0,23,NULL,NULL,99),(1969,0,23,NULL,NULL,100),(1970,0,23,NULL,NULL,101),(1971,0,23,NULL,NULL,102),(1972,0,23,NULL,NULL,103),(1973,0,23,NULL,NULL,104),(1974,0,23,NULL,NULL,105),(1975,0,23,NULL,NULL,106),(1976,0,23,NULL,NULL,107),(1977,0,23,NULL,NULL,108),(1978,0,23,NULL,NULL,109),(1979,0,23,NULL,NULL,110),(1980,0,23,NULL,NULL,111),(1981,0,23,NULL,NULL,112),(1982,0,23,NULL,NULL,113),(1983,0,23,NULL,NULL,114),(1984,0,23,NULL,NULL,115),(1985,0,23,NULL,NULL,116),(1986,0,23,NULL,NULL,117),(1987,0,23,NULL,NULL,118),(1988,0,23,NULL,NULL,119),(1989,0,23,NULL,NULL,120),(1990,0,23,NULL,NULL,121),(1991,0,23,NULL,NULL,122),(1992,0,23,NULL,NULL,123),(1993,0,23,NULL,NULL,124),(1994,0,23,NULL,NULL,125),(1995,0,23,NULL,NULL,126),(1996,0,23,NULL,NULL,127),(1997,0,23,NULL,NULL,128),(1998,0,23,NULL,NULL,129),(1999,0,23,NULL,NULL,130),(2000,0,23,NULL,NULL,131),(2001,0,23,NULL,NULL,132),(2002,0,23,NULL,NULL,133),(2003,0,23,NULL,NULL,134),(2004,0,23,NULL,NULL,135),(2005,0,23,NULL,NULL,136),(2006,0,23,NULL,NULL,137),(2007,0,23,NULL,NULL,138),(2008,0,23,NULL,NULL,139),(2009,0,23,NULL,NULL,140),(2010,0,23,NULL,NULL,141),(2011,0,23,NULL,NULL,142),(2012,0,23,NULL,NULL,143),(2013,0,23,NULL,NULL,144),(2014,0,23,NULL,NULL,145),(2015,0,23,NULL,NULL,146),(2016,0,23,NULL,NULL,147),(2017,0,23,NULL,NULL,148),(2018,0,23,NULL,NULL,149),(2019,0,23,NULL,NULL,150),(2020,0,23,NULL,NULL,151),(2021,0,23,NULL,NULL,152),(2022,0,23,NULL,NULL,153),(2023,0,23,NULL,NULL,154),(2024,0,23,NULL,NULL,155),(2025,0,23,NULL,NULL,156),(2026,0,23,NULL,NULL,157),(2027,0,23,NULL,NULL,158),(2028,0,23,NULL,NULL,159),(2029,0,23,NULL,NULL,160),(2030,0,23,NULL,NULL,161),(2031,0,23,NULL,NULL,162),(2032,0,23,NULL,NULL,163),(2033,0,23,NULL,NULL,164),(2034,0,23,NULL,NULL,165),(2035,0,23,NULL,NULL,166),(2036,0,23,NULL,NULL,167),(2037,0,23,NULL,NULL,168),(2038,0,23,NULL,NULL,169);
/*!40000 ALTER TABLE `respuestas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `roles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Role` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'Usuario'),(2,'Administrador'),(3,'Evaluador');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sections`
--

DROP TABLE IF EXISTS `sections`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sections` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(120) NOT NULL,
  `AssessmentId` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`),
  KEY `IX_Sections_AssessmentId` (`AssessmentId`),
  CONSTRAINT `FK_Sections_Assessment_AssessmentId` FOREIGN KEY (`AssessmentId`) REFERENCES `assessment` (`AssessmentId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sections`
--

LOCK TABLES `sections` WRITE;
/*!40000 ALTER TABLE `sections` DISABLE KEYS */;
INSERT INTO `sections` VALUES (1,'CEREMONIAS',1),(2,'ROLES',1),(3,'ARTEFACTOS',1),(4,'KANBAN-CEREMONIAS',2),(5,'KANBAN-ROLES',2);
/*!40000 ALTER TABLE `sections` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userproyectos`
--

DROP TABLE IF EXISTS `userproyectos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `userproyectos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserNombre` varchar(127) NOT NULL,
  `ProyectoId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserProyecto_ProyectoId` (`ProyectoId`),
  KEY `IX_UserProyecto_UserNombre` (`UserNombre`),
  CONSTRAINT `FK_UserProyecto_Proyectos_ProyectoId` FOREIGN KEY (`ProyectoId`) REFERENCES `proyectos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_UserProyecto_Users_UserNombre` FOREIGN KEY (`UserNombre`) REFERENCES `users` (`Nombre`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userproyectos`
--

LOCK TABLES `userproyectos` WRITE;
/*!40000 ALTER TABLE `userproyectos` DISABLE KEYS */;
INSERT INTO `userproyectos` VALUES (1,'Admin',1),(2,'Admin',2),(3,'Admin',3),(4,'User',1),(5,'mgonzaez',5);
/*!40000 ALTER TABLE `userproyectos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `Nombre` varchar(127) NOT NULL,
  `Password` longtext NOT NULL,
  `RoleId` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Nombre`),
  KEY `IX_Users_RoleId` (`RoleId`),
  CONSTRAINT `FK_Users_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES ('Admin','c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f',2),('mgonzaez','default-password',1),('Test','532eaabd9574880dbf76b9b8cc00832c20a6ec113d682299550d7a6e0f345e25',1),('User','b512d97e7cbf97c273e4db073bbb547aa65a84589227f8f3d9e4a72b9372a24d',1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'agilemeter'
--

--
-- Dumping routines for database 'agilemeter'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-10-31 11:36:40
