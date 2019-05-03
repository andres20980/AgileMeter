--Se agregan nuevas columnas a la tabla
ALTER TABLE agilemeter.proyectos
ADD OfficeName varchar(50);

ALTER TABLE agilemeter.proyectos
ADD UnityName varchar(50);

ALTER TABLE agilemeter.proyectos
ADD TeamName varchar(50);

ALTER TABLE agilemeter.proyectos
ADD ProjectSize int(3);

UPDATE agilemeter.proyectos 
SET TeamName = 'Marvel',
Nombre = 'BCA'
WHERE Nombre = 'BCA - Marvel';

UPDATE agilemeter.proyectos 
SET TeamName = 'BlackOps',
Nombre = 'TESCO'
WHERE Nombre = 'TESCO - BlackOps';

UPDATE agilemeter.proyectos 
SET TeamName = 'Hoteles',
Nombre = 'BestDay'
WHERE Nombre = 'BestDay - Hoteles';

UPDATE agilemeter.proyectos 
SET TeamName = 'Hoteles',
Nombre = 'RANDSTAD'
WHERE Nombre = 'RANDSTAD - Hoteles';