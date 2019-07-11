using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class AddTablesIdiomas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleEN",
                table: "Roles",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleES",
                table: "Roles",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OficinaNombreEn",
                table: "Oficina",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OficinaNombreEs",
                table: "Oficina",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Idiomas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(maxLength: 7, nullable: false),
                    Nombre = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idiomas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AsignacionesIdiomas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Asignacion = table.Column<string>(maxLength: 50, nullable: false),
                    AsignacionesId = table.Column<int>(nullable: false),
                    Codigo = table.Column<int>(nullable: true),
                    CodigoIdioma = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionesIdiomas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AsignacionesIdiomas_Idiomas_Codigo",
                        column: x => x.Codigo,
                        principalTable: "Idiomas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AsignacionesIdiomas_Asignaciones_ID",
                        column: x => x.ID,
                        principalTable: "Asignaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreguntasIdiomas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<int>(nullable: true),
                    CodigoIdioma = table.Column<string>(nullable: true),
                    Pregunta = table.Column<string>(maxLength: 500, nullable: false),
                    PreguntaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreguntasIdiomas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PreguntasIdiomas_Idiomas_Codigo",
                        column: x => x.Codigo,
                        principalTable: "Idiomas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreguntasIdiomas_Preguntas_ID",
                        column: x => x.ID,
                        principalTable: "Preguntas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionsIdioma",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<int>(nullable: true),
                    CodigoIdioma = table.Column<string>(nullable: true),
                    Sections = table.Column<string>(maxLength: 120, nullable: false),
                    SectionsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionsIdioma", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SectionsIdioma_Idiomas_Codigo",
                        column: x => x.Codigo,
                        principalTable: "Idiomas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SectionsIdioma_Sections_ID",
                        column: x => x.ID,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionesIdiomas_Codigo",
                table: "AsignacionesIdiomas",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_PreguntasIdiomas_Codigo",
                table: "PreguntasIdiomas",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_SectionsIdioma_Codigo",
                table: "SectionsIdioma",
                column: "Codigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsignacionesIdiomas");

            migrationBuilder.DropTable(
                name: "PreguntasIdiomas");

            migrationBuilder.DropTable(
                name: "SectionsIdioma");

            migrationBuilder.DropTable(
                name: "Idiomas");

            migrationBuilder.DropColumn(
                name: "RoleEN",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RoleES",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "OficinaNombreEn",
                table: "Oficina");

            migrationBuilder.DropColumn(
                name: "OficinaNombreEs",
                table: "Oficina");
        }
    }
}
