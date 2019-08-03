using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class RemoveColsForTranslate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Pregunta",
                table: "Preguntas");

            migrationBuilder.DropColumn(
                name: "OficinaNombre",
                table: "Oficina");

            migrationBuilder.DropColumn(
                name: "Idioma",
                table: "Idiomas");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Asignaciones");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Sections",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Roles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pregunta",
                table: "Preguntas",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OficinaNombre",
                table: "Oficina",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Idioma",
                table: "Idiomas",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Asignaciones",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
