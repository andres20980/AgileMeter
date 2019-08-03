using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class AddColumnCodigoProyecto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Proyectos",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Proyectos");
        }
    }
}
