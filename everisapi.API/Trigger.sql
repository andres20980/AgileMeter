CREATE DEFINER=`root`@`localhost` TRIGGER `agilemeter`.`evaluaciones_AFTER_INSERT` AFTER INSERT ON `Evaluaciones` FOR EACH ROW
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE PreId INT DEFAULT NEW.Id;
    DECLARE ids INT;
    DECLARE cur CURSOR FOR SELECT id FROM Preguntas;
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    OPEN cur;
        ins_loop: LOOP
            FETCH cur INTO ids;
            IF done THEN
                LEAVE ins_loop;
            END IF;
            INSERT INTO Respuestas (Estado, PreguntaId, EvaluacionId) VALUES (0 , ids , PreId);
        END LOOP;
    CLOSE cur;
END
