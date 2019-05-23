using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class ModificacionTablaProyectos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Oficina",
                table: "Proyectos",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Proyecto",
                table: "Proyectos",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Unidad",
                table: "Proyectos",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Oficina",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "Proyecto",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "Unidad",
                table: "Proyectos");
        }
    }
}
