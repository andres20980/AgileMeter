using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class PreguntaHabilitante : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsHabilitante",
                table: "Preguntas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PreguntaHabilitanteId",
                table: "Preguntas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_PreguntaHabilitanteId",
                table: "Preguntas",
                column: "PreguntaHabilitanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Preguntas_Preguntas_PreguntaHabilitanteId",
                table: "Preguntas",
                column: "PreguntaHabilitanteId",
                principalTable: "Preguntas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Preguntas_Preguntas_PreguntaHabilitanteId",
                table: "Preguntas");

            migrationBuilder.DropIndex(
                name: "IX_Preguntas_PreguntaHabilitanteId",
                table: "Preguntas");

            migrationBuilder.DropColumn(
                name: "EsHabilitante",
                table: "Preguntas");

            migrationBuilder.DropColumn(
                name: "PreguntaHabilitanteId",
                table: "Preguntas");
        }
    }
}
