using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class AddLastQuestionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastQuestionUpdated",
                table: "Evaluaciones",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluaciones_Preguntas_Id",
                table: "Evaluaciones",
                column: "Id",
                principalTable: "Preguntas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluaciones_Preguntas_Id",
                table: "Evaluaciones");

            migrationBuilder.DropColumn(
                name: "LastQuestionUpdated",
                table: "Evaluaciones");
        }
    }
}
