--Se agrega el equipo BlackOps al nombre del proyecto
UPDATE agilemeter.proyectos 
SET NOMBRE = concat(NOMBRE, ' - BlackOps')
WHERE NOMBRE = 'TESCO';

--Se agrega el equipo Marvel a BCA al nombre del proyecto
UPDATE agilemeter.proyectos 
SET NOMBRE = concat(NOMBRE, ' - Marvel')
WHERE NOMBRE = 'BCA';

--Se agrega el equipo Hoteles a BestDay al nombre del proyecto
UPDATE agilemeter.proyectos 
SET NOMBRE = concat(NOMBRE, ' - Hoteles')
WHERE NOMBRE = 'BestDay';

--Se modifica TVE para que sea el equipo RANDSTAD - Hoteles
UPDATE agilemeter.proyectos 
SET NOMBRE = 'RANDSTAD - Hoteles'
WHERE NOMBRE = 'TVE';