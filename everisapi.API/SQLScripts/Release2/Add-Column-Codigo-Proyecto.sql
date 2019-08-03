/*
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Proyectos",
                maxLength: 100,
                nullable: true);

*/

ALTER TABLE agilemeter.proyectos
ADD Codigo varchar(100) DEFAULT NULL;