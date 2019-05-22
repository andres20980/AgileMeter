CREATE DATABASE  IF NOT EXISTS `agilemeter` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `agilemeter`;
-- MySQL dump 10.13  Distrib 5.7.26, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: agilemeter
-- ------------------------------------------------------
-- Server version	5.7.26

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
INSERT INTO `__efmigrationshistory` VALUES ('20181023105227_AddUserProyecto','2.0.2-rtm-10011'),('20181025094247_RemoveUserRoles','2.0.2-rtm-10011'),('20181026062550_AddAssessments','2.0.2-rtm-10011'),('20181029085809_LinkAssessmentWithSections','2.0.2-rtm-10011'),('20181029103724_LinkEvaluationsWithAssessments','2.0.2-rtm-10011'),('20181113120302_AddUserNameToEvaluation','2.0.2-rtm-10011'),('20181203104813_TestProyects','2.0.2-rtm-10011'),('20181217133407_AñadirPesosASecciones','2.0.2-rtm-10011'),('20181218081534_PreguntaHabilitante','2.0.2-rtm-10011'),('20190108132133_AddLastQuestionUpdate','2.0.2-rtm-10011'),('20190109093506_PesoIntToFloat','2.0.2-rtm-10011'),('20190109135727_NivelPreguntas','2.0.2-rtm-10011'),('20190201074324_SectionPesosNiveles','2.0.2-rtm-10011'),('20190503111708_AddTableOficina-Unidad_Linea','2.0.2-rtm-10011'),('20190522091354_NombreCompleto_Activo','2.0.2-rtm-10011');
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
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `asignaciones`
--

LOCK TABLES `asignaciones` WRITE;
/*!40000 ALTER TABLE `asignaciones` DISABLE KEYS */;
INSERT INTO `asignaciones` VALUES (1,'Product Owner',20,1),(2,'Scrum Master',40,1),(3,'Equipo Desarrollo',40,1),(4,'Daily Scrum',20,2),(5,'Retrospective',30,2),(6,'Sprint Review',10,2),(7,'Sprint Planning',15,2),(8,'Refinement',5,2),(9,'Sprint',20,2),(10,'Product Backlog',40,3),(11,'Sprint Backlog',40,3),(12,'Incremento',20,3),(13,'Cultura',30,4),(14,'Disciplina',35,4),(15,'Mejora Continua',35,4),(16,'Métricas',35,5),(17,'Implementación',40,5),(18,'Objetivos',25,5);
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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `assessment`
--

LOCK TABLES `assessment` WRITE;
/*!40000 ALTER TABLE `assessment` DISABLE KEYS */;
INSERT INTO `assessment` VALUES (1,'SCRUM');
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
  `UserNombre` varchar(127) DEFAULT NULL,
  `LastQuestionUpdated` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Evaluaciones_ProyectoId` (`ProyectoId`),
  KEY `IX_Evaluaciones_AssessmentId` (`AssessmentId`),
  KEY `IX_Evaluaciones_UserNombre` (`UserNombre`),
  CONSTRAINT `FK_Evaluaciones_Assessment_AssessmentId` FOREIGN KEY (`AssessmentId`) REFERENCES `assessment` (`AssessmentId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Evaluaciones_Preguntas_Id` FOREIGN KEY (`Id`) REFERENCES `preguntas` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Evaluaciones_Proyectos_ProyectoId` FOREIGN KEY (`ProyectoId`) REFERENCES `proyectos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Evaluaciones_Users_UserNombre` FOREIGN KEY (`UserNombre`) REFERENCES `users` (`Nombre`) ON DELETE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `linea`
--

DROP TABLE IF EXISTS `linea`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `linea` (
  `LineaId` int(11) NOT NULL AUTO_INCREMENT,
  `LineaNombre` varchar(50) NOT NULL,
  `UnidadId` int(11) NOT NULL,
  PRIMARY KEY (`LineaId`),
  KEY `IX_Linea_UnidadId` (`UnidadId`),
  CONSTRAINT `FK_Linea_Unidad_UnidadId` FOREIGN KEY (`UnidadId`) REFERENCES `unidad` (`UnidadId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `linea`
--

LOCK TABLES `linea` WRITE;
/*!40000 ALTER TABLE `linea` DISABLE KEYS */;
INSERT INTO `linea` VALUES (1,'Linea de Prueba',1),(2,'L. BigData',2),(3,'L. CHAFEA',2),(4,'L. Herramientas',3),(5,'GESTOROT',4),(6,'PGECADIS',4),(7,'DES BMW SA3',5),(8,'DES BID',6);
/*!40000 ALTER TABLE `linea` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `oficina`
--

DROP TABLE IF EXISTS `oficina`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `oficina` (
  `OficinaId` int(11) NOT NULL AUTO_INCREMENT,
  `OficinaNombre` varchar(50) NOT NULL,
  PRIMARY KEY (`OficinaId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `oficina`
--

LOCK TABLES `oficina` WRITE;
/*!40000 ALTER TABLE `oficina` DISABLE KEYS */;
INSERT INTO `oficina` VALUES (1,'Oficina de Prueba'),(2,'Murcia'),(3,'Alicante'),(4,'Salamanca'),(5,'Lisboa'),(6,'Sevilla');
/*!40000 ALTER TABLE `oficina` ENABLE KEYS */;
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
  `Pregunta` varchar(500) NOT NULL,
  `Peso` float NOT NULL,
  `EsHabilitante` bit(1) NOT NULL DEFAULT b'0',
  `PreguntaHabilitanteId` int(11) DEFAULT NULL,
  `Nivel` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_Preguntas_AsignacionId` (`AsignacionId`),
  KEY `IX_Preguntas_PreguntaHabilitanteId` (`PreguntaHabilitanteId`),
  CONSTRAINT `FK_Preguntas_Asignaciones_AsignacionId` FOREIGN KEY (`AsignacionId`) REFERENCES `asignaciones` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Preguntas_Preguntas_PreguntaHabilitanteId` FOREIGN KEY (`PreguntaHabilitanteId`) REFERENCES `preguntas` (`Id`) ON DELETE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=142 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `preguntas`
--

LOCK TABLES `preguntas` WRITE;
/*!40000 ALTER TABLE `preguntas` DISABLE KEYS */;
INSERT INTO `preguntas` VALUES (1,1,'Si','¿Existe el rol de Product Owner en el equipo?',0,_binary '',NULL,1),(2,1,'Si','¿El Product Owner tiene poder para priorizar los elementos del Product Backlog?',0.9,_binary '\0',1,2),(3,1,'Si','¿El Product Owner tiene el conocimiento suficiente para priorizar?',0.55,_binary '\0',1,3),(4,1,'Si','¿El Product Owner tiene contacto directo con el equipo de desarrollo?',0.46,_binary '\0',1,1),(5,1,'Si','¿El Product Owner tiene contacto directo con los interesados?',0.06,_binary '\0',1,2),(6,1,'Si','¿El Product Owner tiene voz única (Si hay más de un Product Owner, solo hay una opinión)?',0.45,_binary '\0',1,3),(7,1,'Si','¿El Product Owner tiene la visión del producto?',0.46,_binary '\0',1,1),(8,1,'No','¿El Product Owner hace otras labores (codificar por ejemplo)?',0.04,_binary '\0',1,2),(9,1,'No','¿El Product Owner toma decisiones técnicas?',0.08,_binary '\0',1,1),(10,2,'Si','¿Existe el rol de Scrum Master en el equipo?',0,_binary '',NULL,1),(11,2,'Si','¿El Scrum Master se enfoca en la resolución de impedimentos?',0.48,_binary '\0',10,1),(12,2,'Si','¿El Scrum Master escala los impedimentos?',0.31,_binary '\0',10,2),(13,2,'No','¿El Scrum Master hace otras labores (codificar/analizar por ejemplo)?',0.08,_binary '\0',10,2),(14,2,'No','¿El Scrum Master toma decisiones técnicas o de negocio?',0.15,_binary '\0',10,2),(15,2,'Si','¿El Scrum Master ayuda/guía al Product Owner para realizar correctamente su trabajo?',0.52,_binary '\0',10,1),(16,2,'Si','¿El Scrum Master empodera al equipo?',0.85,_binary '\0',10,3),(17,2,'No','¿El Scrum Master asume la responsabilidad si el equipo falla? ',0.15,_binary '\0',10,3),(18,2,'Si','¿El Scrum Master permite que el equipo experimente y se equivoque?',0.46,_binary '\0',10,2),(19,3,'Si','¿El equipo de desarrollo tiene todas las competencias necesarias?',0.75,_binary '\0',NULL,2),(20,3,'No','¿Existen miembros del equipo de desarrollo encasillados, no conociendo absolutamente nada de otras áreas?',0.06,_binary '\0',NULL,2),(21,3,'Si','¿Los miembros del equipo de desarrollo interactúan juntos en el desarrrollo de la solución?',0.38,_binary '\0',NULL,1),(22,3,'Si','¿Hay como máximo 9 personas en el equipo de desarrollo?',0.46,_binary '\0',NULL,1),(23,3,'No','¿Hay algún miembro del equipo de desarrollo que no esté alineado con Scrum?',0.16,_binary '\0',NULL,1),(24,3,'Si','¿Tiene el equipo de desarrollo un drag factor interiorizado, planificado y consensuado con los stakeholders?',0.15,_binary '\0',NULL,3),(25,3,'Si','¿El equipo de desarrollo usa o dispone de herramientas para organizar sus tareas?',0.19,_binary '\0',NULL,2),(26,3,'No','¿El equipo de desarrollo tiene dependencias no resueltas?',0.85,_binary '\0',NULL,3),(27,4,'Si','¿Se realiza la Daily Scrum?',0,_binary '',NULL,1),(28,4,'Si','¿Solo interviene el equipo de desarrollo?',0.67,_binary '\0',27,1),(29,4,'Si','¿Se emplean como máximo 15 min?',0.13,_binary '\0',27,1),(30,4,'Si','¿Se mencionan los problemas e impedimentos?',0.73,_binary '\0',27,2),(31,4,'Si','¿Se revisan los objetivos del Sprint?',1,_binary '\0',27,3),(32,4,'Si','¿Se realiza siempre a la misma hora y lugar?',0.2,_binary '\0',27,1),(33,4,'No','¿Interviene gente que no pertenece al equipo Scrum?',0.09,_binary '\0',27,2),(34,4,'No','¿Se discute sobre soluciones técnicas durante la Daily Scrum?',0.18,_binary '\0',27,2),(35,5,'Si','¿Se realiza la Retrospective al final de cada sprint?',0,_binary '',NULL,1),(36,5,'Si','¿Hay alguien que haga de facilitador?',0.33,_binary '\0',35,2),(37,5,'Si','¿El equipo Scrum al completo participa?',0.2,_binary '\0',35,1),(38,5,'Si','¿Se analizan los problemas en profundidad?',0.33,_binary '\0',35,2),(39,5,'Si','¿Se proponen soluciones a los problemas detectados?',0.77,_binary '\0',35,1),(40,5,'No','¿Participa gente que no pertenece al equipo?',0.03,_binary '\0',35,1),(41,5,'Si','¿Todo el equipo expresa su punto de vista?',0.75,_binary '\0',35,3),(42,5,'Si','¿Se analizan las métricas y su impacto durante la retro?',0.25,_binary '\0',35,3),(43,5,'Si','¿Se hace seguimiento a las acciones de las Retrospectives anteriores?',0.34,_binary '\0',35,2),(44,6,'Si','¿Se realiza la Sprint Review al final de cada sprint?',0,_binary '',NULL,1),(45,6,'Si','¿Se muestra software funcionando y probado?',0.75,_binary '\0',44,1),(46,6,'Si','¿Se recibe feedback de interesados y Product Owner?',0.35,_binary '\0',44,2),(47,6,'No','¿Se mencionan los items inacabados?',0.06,_binary '\0',44,1),(48,6,'Si','¿Se revisa si se ha alcanzado el objetivo del Sprint?',0.47,_binary '\0',44,2),(49,6,'No','¿Se muestran los items desarrollados pero no probados?',0.06,_binary '\0',44,2),(50,6,'Si','¿Se invitan a los stakeholders a que participen?',0.2,_binary '\0',44,3),(51,6,'Si','¿Se revisa lo que vamos a incluir en el siguiente sprint?',0.8,_binary '\0',44,3),(52,6,'Si','¿Participa todo el equipo Scrum?',0.19,_binary '\0',44,1),(53,6,'Si','¿Es el equipo de desarrollo el que enseña el incremento?',0.12,_binary '\0',44,2),(54,7,'Si','¿Se realiza Sprint Planning por cada Sprint?',0,_binary '',NULL,1),(55,7,'Si','¿El Product Owner está disponible para dudas?',0.27,_binary '\0',54,1),(56,7,'Si','¿El equipo de desarrollo completo participa?',0.36,_binary '\0',54,1),(57,7,'Si','¿El resultado de la sesión es el plan del Sprint?',0.06,_binary '\0',54,1),(58,7,'Si','¿El equipo completo cree que el plan es alcanzable?',0.5,_binary '\0',54,2),(59,7,'Si','¿Se llega a un consenso entre el Product Owner y el equipo de desarrollo en el alcance del Sprint Backlog?',0.38,_binary '\0',54,3),(60,7,'Si','¿Los Product Backlog Items se dividen en tareas?',0.14,_binary '\0',54,1),(61,7,'Si','¿Los Product Backlog Items son estimados?',0.11,_binary '\0',54,1),(62,7,'Si','¿Se adquiere un compromiso por parte del equipo de desarrollo?',0.5,_binary '\0',54,2),(63,7,'Si','¿Se analizan las dependencias que pueden surgir entre los Product Backlog Items?',0.62,_binary '\0',54,3),(64,7,'No','¿Participa el Product Owner/Scrum Master en las estimaciones de los Product Backlog Items?',0.06,_binary '\0',54,1),(65,8,'Si','¿Se realiza Refinement?',0,_binary '',NULL,1),(66,8,'Si','¿Es el Product Owner quien solicita hacer una refinement?',0.77,_binary '\0',65,1),(67,8,'Si','¿El Product Owner lleva las User Stories definidas para discutirlas?',0.83,_binary '\0',65,2),(68,8,'No','¿Se dedica más del 10 % de la capacidad del equipo desarrollo?',1,_binary '\0',65,3),(69,8,'Si','¿Se tratan los Product Backlog Items que son más prioritarios del Product Backlog?',0.23,_binary '\0',65,1),(70,8,'Si','¿Participan los perfiles necesarios en la Refinement?',0.17,_binary '\0',65,2),(71,9,'Si','¿Las iteraciones siempre duran lo mismo?',0,_binary '',NULL,1),(72,9,'Si','¿La duración de las iteraciones es menor a un mes?',1,_binary '\0',71,1),(73,9,'No','¿El equipo varia durante el Sprint?',0.31,_binary '\0',71,2),(74,9,'No','¿Se continua el sprint aunque no tenga sentido el objetivo a alcanzar?',0.69,_binary '\0',71,2),(75,10,'Si','¿Existe PB?',0,_binary '',NULL,1),(76,10,'Si','¿EL PB refleja la visión del producto?',0.23,_binary '\0',75,2),(77,10,'Si','¿El PB es visible para todos los miembros del equipo?',0.47,_binary '\0',75,2),(78,10,'Si','¿Los PBIs se priorizan por su valor de negocio?',0.87,_binary '\0',75,1),(79,10,'Si','¿El alcance de los PBIs más prioritarios está suficientemente claro como para incluirlos en un Sprint?',0.2,_binary '\0',75,2),(80,10,'No','¿Es el alcance de los PBIs inmodificable?',0.41,_binary '\0',75,3),(81,10,'Si','¿Los PBI son tan pequeños como para abordarse en un Sprint?',0.13,_binary '\0',75,1),(82,10,'Si','¿El Equipo Scrum entiende el propósito de todos los PBIs?',0.07,_binary '\0',75,2),(83,10,'Si','¿El equipo de desarrollo influye en la priorización del PB?',0.2,_binary '\0',75,3),(84,10,'Si','¿El PB incluye algunas de las acciones elegidas en la Retrospectiva?',0.39,_binary '\0',75,3),(85,10,'Si','¿Se refinan los PBIs antes de llegar a un Sprint Planning?',0.03,_binary '\0',75,2),(86,11,'Si','¿Existe SB?',0,_binary '',NULL,1),(87,11,'Si','¿El SB refleja el compromiso para el Sprint?',0.58,_binary '\0',86,1),(88,11,'Si','¿El SB es visible para todos los miembros del equipo?',0.42,_binary '\0',86,1),(89,11,'Si','¿El SB se revisa diariamente?',1,_binary '\0',86,2),(90,11,'No','¿El PO ordena la prioridad de los items en el SB?',1,_binary '\0',86,3),(91,12,'Si','¿El incremento tiene calidad para subirse a producción si el negocio así lo pidiera en cualquier momento?',0.6,_binary '\0',NULL,2),(92,12,'No','¿Al finalizar el Sprint el incremento resultante siempre se sube a producción?',0.14,_binary '\0',NULL,2),(93,12,'Si','¿Existe DoD?',0,_binary '',NULL,1),(94,12,'Si','¿El DoD incluye los criterios de aceptación de los PBIs?',0.55,_binary '\0',93,1),(95,12,'Si','¿El DoD incluye los requisitos no funcionales?',0.08,_binary '\0',93,2),(96,12,'Si','¿El DoD es consistente con un incremento del producto potencialmente entregable?',0.68,_binary '\0',93,3),(97,12,'Si','¿El equipo entiende el DoD?',0.45,_binary '\0',93,1),(98,12,'No','¿El DoD es creado sólo por el equipo de desarrollo?',0.02,_binary '\0',93,2),(99,12,'Si','¿Se revisa el DoD para que sea consistente con el propio producto?',0.32,_binary '\0',93,3),(100,12,'Si','¿Tanto PO como equipo están de acuerdo con el DoD?',0.16,_binary '\0',93,2),(101,13,'Si','¿El equipo cumple con el compromiso adquirido?',0.25,_binary '\0',NULL,1),(102,13,'Si','¿Los líderes o managers de la organización conocen y comparten los principios ágiles?',0.75,_binary '\0',NULL,1),(103,13,'Si','¿El SM asesora/guía en Scrum al resto de la organización?',0.81,_binary '\0',NULL,2),(104,13,'No','¿El equipo se resiste a la transformación digital?',0.19,_binary '\0',NULL,2),(105,13,'Si','¿El equipo participa de las decisiones sobre las propuestas de nuevos proyectos o servicios?',0.23,_binary '\0',NULL,3),(106,13,'Si','¿El equipo está involucrado en el proceso de incorporación o salida de los miembros del propio equipo?',0.33,_binary '\0',NULL,3),(107,13,'Si','¿El equipo participa de la transformación de su área?',0.44,_binary '\0',NULL,3),(108,14,'No','¿El equipo es interrumpido frecuentemente durante el Sprint para otras necesidades diferentes al objetivo de propio Sprint?',0.3,_binary '\0',NULL,2),(109,14,'No','¿Se realizan reuniones adicionales que estén fuera del framework de Scrum?',0.3,_binary '\0',NULL,2),(110,14,'Si','¿Se respetan los timeboxes de las reuniones?',0.75,_binary '\0',NULL,1),(111,14,'Si','¿Los miembros del equipo convocados a una reunión están presentes al inicio de la misma?',0.25,_binary '\0',NULL,1),(112,14,'No','¿Se desvían las reuniones de sus objetivos?',0.4,_binary '\0',NULL,2),(113,14,'Si','¿Se respetan las decisiones del equipo que solo afectan al equipo?',1,_binary '\0',NULL,3),(114,15,'Si','¿El equipo practica la mejora continua y evoluciona su forma de trabajo?',0.46,_binary '\0',NULL,1),(115,15,'No','¿Se buscan culpables de las malas decisiones del equipo?',0.28,_binary '\0',NULL,1),(116,15,'Si','¿Existe un ambiente de confianza donde el equipo pueda expresar abiertamente su opinión, cómo se encuentra ...?',1,_binary '\0',NULL,3),(117,15,'Si','¿El equipo dispone de espacio/tiempo para dedicar a la mejora continua?',0.26,_binary '\0',NULL,1),(118,15,'No','¿Existen agentes externos que interfieren en la toma de decisiones del equipo?',1,_binary '\0',NULL,2),(119,16,'No','¿El equipo estima los PBIs en horas?',0.5,_binary '\0',NULL,1),(120,16,'Si','¿El equipo usa patrones para estimar?',0.5,_binary '\0',NULL,1),(121,16,'Si','¿Estos patrones se revisan por el equipo?',0.29,_binary '\0',NULL,2),(122,16,'Si','¿El equipo utiliza estos pratones para estimar las propuestas de nuevos proyectos?',0.12,_binary '\0',NULL,3),(123,16,'Si','¿El equipo conoce su velocidad?',0,_binary '',NULL,2),(124,16,'No','¿Para el cálculo de la velocidad el equipo tiene en cuenta los items no completados?',0.29,_binary '\0',123,2),(125,16,'Si','¿El equipo tiene en cuenta su velocidad para establecer el compromiso en una nueva iteración?',0.75,_binary '\0',123,3),(126,16,'Si','¿El equipo usa el Burndown Chart para visualizar su progreso durante el Sprint?',0,_binary '',NULL,2),(127,16,'Si','¿El Burndown Chart es visible para todos los miembros del equipo?',0.42,_binary '\0',126,2),(128,16,'Si','¿El equipo actualiza su Burndown Chart diariamente?',0.13,_binary '\0',126,3),(129,17,'Si','¿Tiene el equipo la visión global del proyecto?',0.69,_binary '\0',NULL,2),(130,17,'Si','¿Cuenta el equipo con el espacio adecuado para su desempeño?',0.14,_binary '\0',NULL,2),(131,17,'Si','¿Utiliza el equipo un panel físico?',0.47,_binary '\0',NULL,1),(132,17,'Si','¿Utiliza el equipo un panel digital?',0.53,_binary '\0',NULL,1),(133,17,'Si','¿Está todo el equipo deslocalizado?',0,_binary '',NULL,2),(134,17,'Si','¿El equipo utiliza sistemas de videoconferencia para sus reuniones?',0.14,_binary '\0',133,2),(135,17,'Si','¿Cuenta el equipo con un espacio colaborativo para la gestión de la información?',0.03,_binary '\0',133,2),(136,17,'Si','¿Aplican alguna técnica específica para colaborar durante los eventos de manera remota?',1,_binary '\0',133,3),(137,18,'Si','¿El equipo ha definido su misión?',0.44,_binary '\0',NULL,2),(138,18,'Si','¿El equipo ha definido su visión?',0.56,_binary '\0',NULL,2),(139,18,'Si','¿El equipo ha definido sus valores?',0.47,_binary '\0',NULL,3),(140,18,'Si','¿Están alineados éstos con la proyección profesional de sus miembros?',0.53,_binary '\0',NULL,3),(141,18,'Si','¿El equipo conoce lo que la organización espera de él?',1,_binary '\0',NULL,1);
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
  `TestProject` bit(1) NOT NULL DEFAULT b'0',
  `ProjectSize` int(11) NOT NULL DEFAULT '0',
  `LineaId` int(11) NOT NULL DEFAULT '0',
  `OficinaId` int(11) NOT NULL DEFAULT '0',
  `UnidadId` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_Proyectos_UserNombre` (`UserNombre`),
  KEY `FK_Proyectos_Linea_LineaId` (`LineaId`),
  KEY `FK_Proyectos_Oficina_OficinaId` (`OficinaId`),
  KEY `FK_Proyectos_Unidad_UnidadId` (`UnidadId`),
  CONSTRAINT `FK_Proyectos_Linea_LineaId` FOREIGN KEY (`LineaId`) REFERENCES `linea` (`LineaId`),
  CONSTRAINT `FK_Proyectos_Oficina_OficinaId` FOREIGN KEY (`OficinaId`) REFERENCES `oficina` (`OficinaId`),
  CONSTRAINT `FK_Proyectos_Unidad_UnidadId` FOREIGN KEY (`UnidadId`) REFERENCES `unidad` (`UnidadId`),
  CONSTRAINT `FK_Proyectos_Users_UserNombre` FOREIGN KEY (`UserNombre`) REFERENCES `users` (`Nombre`) ON DELETE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=7474 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
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
  `Peso` int(11) NOT NULL DEFAULT '0',
  `PesoNivel1` int(11) DEFAULT NULL,
  `PesoNivel2` int(11) DEFAULT NULL,
  `PesoNivel3` int(11) DEFAULT NULL,
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
INSERT INTO `sections` VALUES (1,'EQUIPO',1,15,60,25,15),(2,'EVENTOS',1,20,60,25,15),(3,' HERRAMIENTAS ',1,15,60,25,15),(4,'MINDSET',1,20,60,25,15),(5,'APLICACIÓN PRÁCTICA',1,30,60,25,15);
/*!40000 ALTER TABLE `sections` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unidad`
--

DROP TABLE IF EXISTS `unidad`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unidad` (
  `UnidadId` int(11) NOT NULL AUTO_INCREMENT,
  `OficinaId` int(11) NOT NULL,
  `UnidadNombre` varchar(50) NOT NULL,
  PRIMARY KEY (`UnidadId`),
  KEY `IX_Unidad_OficinaId` (`OficinaId`),
  CONSTRAINT `FK_Unidad_Oficina_OficinaId` FOREIGN KEY (`OficinaId`) REFERENCES `oficina` (`OficinaId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unidad`
--

LOCK TABLES `unidad` WRITE;
/*!40000 ALTER TABLE `unidad` DISABLE KEYS */;
INSERT INTO `unidad` VALUES (1,1,'Unidad de Prueba'),(2,2,'Centers'),(3,3,'Centers'),(4,4,'Centers'),(5,5,'Centers'),(6,6,'Centers');
/*!40000 ALTER TABLE `unidad` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `Activo` bit(1) NOT NULL DEFAULT b'1',
  `NombreCompleto` longtext,
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
INSERT INTO `users` VALUES ('Admin','c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f',2,_binary '',NULL),,('fvellari','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4',2,_binary '','Francisco Jose Vellarino Peces'),('jbeltrma','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4',2,_binary '','Jose Antonio Beltran Marquez'),('marcossa','03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4',2,_binary '','Moises Arcos Santiago');
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

-- Dump completed on 2019-05-22 12:53:25
