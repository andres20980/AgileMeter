--Creamos la columna de PesoNivel1
ALTER TABLE agilemeter.sections
ADD PesoNivel1 int(11);

--Creamos la columna de PesoNivel2
ALTER TABLE agilemeter.sections
ADD PesoNivel2 int(11);

--Creamos la columna de PesoNivel3
ALTER TABLE agilemeter.sections
ADD PesoNivel3 int(11);

--Insertamos el valor del peso de Nivel 1
UPDATE agilemeter.sections
SET PesoNivel1=60;

--Insertamos el valor del peso de Nivel 2
UPDATE agilemeter.sections
SET PesoNivel2=25;

--Insertamos el valor del peso de Nivel 3
UPDATE agilemeter.sections
SET PesoNivel3=15;

--Actualizamos los valores de los pesos de cada sección
UPDATE agilemeter.sections SET Peso='15' WHERE Id='1';
UPDATE agilemeter.sections SET Peso='15' WHERE Id='3';
UPDATE agilemeter.sections SET Peso='20' WHERE Id='4';