--Se otorga permisos de administrador a marcossa
UPDATE agilemeter.users 
SET RoleId = 2
WHERE Nombre = 'marcossa';
